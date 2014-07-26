using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LanManager.BLL;
using System.Collections;

namespace LanManager.Server
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Principal : Window
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private ManageClients manageClientswindow;
        private ManageAdmins manageAdminswindow;
        private ManageApps manageAppswindow;
        private ManageAppGroups manageAppGroupwindow;
        private Config configWindow;

        public Principal()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            using (ClientSessionManager contextoSession = new ClientSessionManager())
            {
                var openedSessions = contextoSession.GetOpenSessions(new[] { "Client", "AccessPoint" });

                foreach (var session in openedSessions)
                {
                    if (session.LastClientPing < DateTime.Now.AddMinutes(-5))
                    {
                        contextoSession.CloseSession(session.Id, false);
                    }
                }

                openedSessions.ForEach(x => x.Client.MinutesLeft = x.Client.MinutesLeft + x.Client.MinutesBonus - (int)(DateTime.Now - x.StartDate).TotalMinutes);
                ltvClientes.ItemsSource = openedSessions;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtpReportStart.SelectedDate = DateTime.Now.AddMonths(-1);
            dtpReportEnd.SelectedDate = DateTime.Now;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-br");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Guid sessionId = (Guid) ((Button) sender).CommandParameter;

            using (ClientSessionManager contextoSession = new ClientSessionManager())
            {
                contextoSession.CloseSession(sessionId, false);
            }
        }

        private void btnReportSearch_Click(object sender, RoutedEventArgs e)
        {
            dtgRelatorio.Visibility = Visibility.Hidden;
            ChartMinutosGastos.Visibility = Visibility.Hidden;
            ChartMinutosGastosMensal.Visibility = Visibility.Hidden;
            ChartNewUsers.Visibility = Visibility.Hidden;

            lblTempoTotal.Content = string.Empty;

            if (cbbModoRelatorio.SelectedIndex <= 2)
            {
                ShowMinutesReport();
            }
            else if (cbbModoRelatorio.SelectedIndex == 3)
            {
                ShowNewUsersReport();
            }
        }

        private void ShowMinutesReport()
        {
            List<ClientSessionClient> dados;
            using (ClientSessionManager contextoSession = new ClientSessionManager())
            {
                if (chbFilterByDate.IsChecked.Value)
                {
                    if (!dtpReportStart.SelectedDate.HasValue || !dtpReportEnd.SelectedDate.HasValue)
                    {
                        MessageBox.Show("Selecione a data desejada");
                        return;
                    }

                    dados = contextoSession.GenerateReport(dtpReportStart.SelectedDate.Value,
                                                           dtpReportEnd.SelectedDate.Value.AddDays
                                                               (1), txtUsernameClient.Text, null);
                }
                else
                {
                    dados = contextoSession.GenerateReport(null, null, txtUsernameClient.Text, null);
                }
            }

            lblTempoTotal.Content = "Tempo Total: " + dados.Sum(x => x.Duration.TotalMinutes).ToString("0.00") + " minutos";

            if (dados.Count > 0)
            {
                if (cbbModoRelatorio.SelectedIndex == 0)
                {
                    dtgRelatorio.Visibility = Visibility.Visible;
                    dtgRelatorio.ItemsSource = dados;
                }
                else if (cbbModoRelatorio.SelectedIndex == 1)
                {
                    ChartMinutosGastos.Visibility = Visibility.Visible;
                    var dadosChartMinutosGastosPagos = from d in dados
                                                       group d by (d.Session.StartDate - DateTime.MinValue).Days into f
                                                                                                                     select new { StartDate = DateTime.MinValue.AddDays(f.Key), MinutesPaid = f.Sum(x => x.Session.MinutesPaid) };

                    ChartMinutosGastosPagos.ItemsSource = dadosChartMinutosGastosPagos;

                    var dadosChartMinutosGastosBonus = from d in dados
                                                       group d by (d.Session.StartDate - DateTime.MinValue).Days into f
                                                                                                                     select new { StartDate = DateTime.MinValue.AddDays(f.Key), MinutesBonusUsed = f.Sum(x => x.Session.MinutesBonusUsed) };

                    ChartMinutosGastosBonus.ItemsSource = dadosChartMinutosGastosBonus;

                    ChartMinutosGastosEixoY.Maximum = 1.1 * Math.Max(dadosChartMinutosGastosPagos.Max(x => x.MinutesPaid).Value, dadosChartMinutosGastosBonus.Max(x => x.MinutesBonusUsed).Value);
                    ChartMinutosGastosEixoY.Interval = Math.Ceiling((ChartMinutosGastosEixoY.Maximum ?? 10) / 10);

                    ChartMinutosGastosEixoX.Minimum = dados.Min(x => x.Session.StartDate).AddMonths(-1);
                    ChartMinutosGastosEixoX.Maximum = dados.Max(x => x.Session.StartDate).AddMonths(1);
                }
                else if (cbbModoRelatorio.SelectedIndex == 2)
                {
                    ChartMinutosGastosMensal.Visibility = Visibility.Visible;
                    var dadosChartMinutosGastosPagos = from d in dados
                                                       group d by (DateTime.ParseExact("01/" + d.Session.StartDate.Month.ToString("00") + "/" + d.Session.StartDate.Year, "dd/MM/yyyy", null)) into f
                                                                                                                                                                                                   select new { StartDate = f.Key, MinutesPaid = f.Sum(x => x.Session.MinutesPaid) };

                    ChartMinutosGastosPagosMensal.ItemsSource = dadosChartMinutosGastosPagos;

                    var dadosChartMinutosGastosBonus = from d in dados
                                                       group d by (DateTime.ParseExact("01/" + d.Session.StartDate.Month.ToString("00") + "/" + d.Session.StartDate.Year, "dd/MM/yyyy", null)) into f
                                                                                                                                                                                                   select new { StartDate = f.Key, MinutesBonusUsed = f.Sum(x => x.Session.MinutesBonusUsed) };

                    ChartMinutosGastosBonusMensal.ItemsSource = dadosChartMinutosGastosBonus;

                    ChartMinutosGastosEixoYMensal.Maximum = 1.1 * Math.Max(dadosChartMinutosGastosPagos.Max(x => x.MinutesPaid).Value, dadosChartMinutosGastosBonus.Max(x => x.MinutesBonusUsed).Value);
                    ChartMinutosGastosEixoYMensal.Interval = Math.Ceiling((ChartMinutosGastosEixoYMensal.Maximum ?? 10) / 10);

                    ChartMinutosGastosEixoXMensal.Minimum = dados.Min(x => x.Session.StartDate).AddMonths(-1);
                    ChartMinutosGastosEixoXMensal.Maximum = dados.Max(x => x.Session.StartDate).AddMonths(1);
                    ChartMinutosGastosEixoXMensal.Interval = 1;
                }
            }
        }

        private void ShowNewUsersReport()
        {
            List<ReportNewuser> dados;
            using (ClientManager contextoSession = new ClientManager())
            {
                dados = contextoSession.GenerateNewUsersReport(dtpReportStart.SelectedDate.Value, dtpReportEnd.SelectedDate.Value.AddDays(1), null);
            }

            if (dados.Count > 0 && cbbModoRelatorio.SelectedIndex == 3)
            {
                ChartNewUsers.Visibility = Visibility.Visible;
                //var dadosChartMinutosGastosPagos = from d in dados
                //                                   group d by (DateTime.ParseExact("01/" + d.Session.StartDate.Month.ToString("00") + "/" + d.Session.StartDate.Year, "dd/MM/yyyy", null)) into f
                //                                   select new { StartDate = f.Key, MinutesPaid = f.Sum(x => x.Session.MinutesPaid) };

                ChartNewUsersLine.ItemsSource = dados;

                ChartNewUsersEixoYMensal.Maximum = 1.1*dados.Max(x => x.Count);
                ChartNewUsersEixoYMensal.Interval = Math.Ceiling((ChartNewUsersEixoYMensal.Maximum ?? 10)/10);

                ChartNewUsersEixoXMensal.Minimum = dados.Min(x => x.RegisterDate).AddMonths(-1);
                ChartNewUsersEixoXMensal.Maximum = dados.Max(x => x.RegisterDate).AddMonths(1);
                ChartNewUsersEixoXMensal.Interval = 1;
            }
        }

        private void btnManageClients_Click(object sender, RoutedEventArgs e)
        {
            if (manageClientswindow == null || !manageClientswindow.IsLoaded)
                manageClientswindow = new ManageClients();
            manageClientswindow.Show();
        }

        private void btnManageAdmins_Click(object sender, RoutedEventArgs e)
        {
            if (manageAdminswindow == null || !manageAdminswindow.IsLoaded)
                manageAdminswindow = new ManageAdmins();
            manageAdminswindow.Show();
        }

        private void btnManageApps_Click(object sender, RoutedEventArgs e)
        {
            if (manageAppswindow == null || !manageAppswindow.IsLoaded)
                manageAppswindow = new ManageApps();
            manageAppswindow.Show();
        }

        private void btnManageAppGroups_Click(object sender, RoutedEventArgs e)
        {
            if (manageAppGroupwindow == null || !manageAppGroupwindow.IsLoaded)
                manageAppGroupwindow = new ManageAppGroups();
            manageAppGroupwindow.Show();
        }

        private void btnConfigs_Click(object sender, RoutedEventArgs e)
        {
            if (configWindow == null || !configWindow.IsLoaded)
                configWindow = new Config();
            configWindow.Show();
        }
    }
}
