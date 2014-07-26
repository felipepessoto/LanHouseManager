using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LanManager.BLL;
using LanManager.OSNativeUtils.Security.Windows;
using Application=LanManager.BLL.Application;

namespace LanManager.Client
{
    /// <summary>
    /// Interaction logic for Principal.xaml
    /// </summary>
    public partial class Principal
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private readonly Dictionary<Process, ClientApplicationLog> processMonitor = new Dictionary<Process, ClientApplicationLog>();
        private readonly int minutesLeft;
        private bool shouldClose;
        private bool alarmPlayed;

        public Principal()
        {
            InitializeComponent();

            minutesLeft = new ClientSessionManager().GetMinutesLeftWithDebitLimit();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();

            SetLabels();
            LoadApplications();
        }

        private void SetLabels()
        {
            if (!ClientSessionManager.CanContinueAfterMidnight())
                lblMeiaNoite.Visibility = Visibility.Visible;
        }

        private void LoadApplications()
        {
            PermissionManager permissoes = new PermissionManager();
            IEnumerable<LanManager.BLL.Application> aplicacoes = permissoes.ReturnApplicationsAllowed();

            var gruposNomes =
                aplicacoes.Select(x => (x.ApplicationGroup == null ? "Outros" : x.ApplicationGroup.Name)).OrderBy(x => x)
                    .Distinct().ToArray();

            foreach (var grupo in gruposNomes)
            {
                string nomeGrupo = grupo;

                WrapPanel pnlAplicacoes = new WrapPanel();

                TabItem novaTab = new TabItem { Name = nomeGrupo, Header = nomeGrupo, Content = pnlAplicacoes };

                tabAplicativos.Items.Add(novaTab);

                foreach (var aplicacao in aplicacoes.Where(x => (x.ApplicationGroup == null ? "Outros" : x.ApplicationGroup.Name) == nomeGrupo))
                {
                    System.Windows.Controls.Image imagemAplicacao = new System.Windows.Controls.Image
                                                                        {
                                                                            ToolTip = aplicacao.Name,
                                                                            Width = 80,
                                                                            Height = 80
                                                                        };

                    Label lblAppName = new Label();
                    lblAppName.Content = aplicacao.Name;
                    StackPanel imagemContent = new StackPanel();
                    imagemContent.Children.Add(imagemAplicacao);
                    imagemContent.Children.Add(lblAppName);

                    FrameApplication imageContainer = new FrameApplication
                                                          {
                                                              ApplicationInfo = aplicacao,
                                                              Content = imagemContent
                                                          };
                    imageContainer.MouseDoubleClick += imageContainer_MouseDoubleClick;

                    try
                    {
                        imagemAplicacao.Source = aplicacao.Icon == null
                                                     ? FileToImage(aplicacao.DefaultPath)
                                                     : ByteToImage(aplicacao.Icon);
                    }
                    catch
                    {
                    }
                    pnlAplicacoes.Children.Add(imageContainer);
                }

            }
        }

        void imageContainer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FrameApplication frame = (FrameApplication) sender;

            using (ApplicationManager context = new ApplicationManager())
            {
                KeyValuePair<Process, ClientApplicationLog> novoProcesso =
                    context.StartProcess(frame.ApplicationInfo);
                novoProcesso.Key.Exited += processoAtivo_Exited;
                novoProcesso.Key.EnableRaisingEvents = true;
                processMonitor.Add(novoProcesso.Key, novoProcesso.Value);
            }
        }

        void processoAtivo_Exited(object sender, EventArgs e)
        {
            Process processoQueFechou = (Process)sender;
            ClientApplicationLog logApp = processMonitor[processoQueFechou];
            processMonitor.Remove(processoQueFechou);

            using (ApplicationManager context = new ApplicationManager())
            {
                context.WriteClientClosedApplicationLog(logApp);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToString("HH:mm");
            TimeSpan tempoAlarme = TimeSpan.FromMinutes(3);
            using (ClientSessionManager contexto = new ClientSessionManager())
            {
                TimeSpan elapsedTime = contexto.GetElapsedTime();
                lblElapsedTime.Content = elapsedTime.Hours.ToString("00") + ":" + elapsedTime.Minutes.ToString("00");

                if (contexto.CheckSessionIsClosed() || minutesLeft <= elapsedTime.TotalMinutes || contexto.CheckCantContinueLogged(TimeSpan.Zero))
                {
                    CloseSession();
                }
                //Alarme
                if(minutesLeft <= elapsedTime.Add(tempoAlarme).TotalMinutes || contexto.CheckCantContinueLogged(tempoAlarme))
                {
                    if (!alarmPlayed)
                    {
                        alarmPlayed = true;
                        System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer();
                        myPlayer.Stream = ResourceSet.Resources.alarm;
                        myPlayer.Play();
                    }
                }
            }

        }

        private void btnSair_Click(object sender, RoutedEventArgs e)
        {
            CloseSession();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(shouldClose)
            {
                timer.Stop();
                Process[] processes = LanManager.OSNativeUtils.Environment.Windows.Desktop.GetInputProcesses();
                Process thisProc = Process.GetCurrentProcess();
                foreach (Process p in processes)
                {
                    if (p.ProcessName != thisProc.ProcessName)
                    {
                        p.Kill();
                    }
                }
            }
            else
            {
                e.Cancel = true;   
            }
        }

        private void CloseSession()
        {
            using (ClientSessionManager context = new ClientSessionManager())
            {
                context.CloseAllSessions();
            }
            new LogOn().Show();
            shouldClose = true;
            Close();
        }

        private static ImageSource ByteToImage(byte[] icon)
        {
            try
            {
                var img = new BitmapImage();
                img.BeginInit();
                img.StreamSource = new MemoryStream(icon);
                img.EndInit();
                return img;
            }
            catch
            {
                return null;//TODO retornar imagem padrao
            }
        }

        private static ImageSource FileToImage(string filePath)
        {
            System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(filePath);
            MemoryStream strm = new MemoryStream();
            ico.Save(strm);
            IconBitmapDecoder ibd = new IconBitmapDecoder(strm, BitmapCreateOptions.None, BitmapCacheOption.Default);

            return ibd.Frames[0];
        }

        public class FrameApplication : Frame
        {
            public BLL.Application ApplicationInfo { get; set; }
        }
    }
}
