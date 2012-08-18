using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LanManager.BLL;
using System.Configuration;
using LanManager.BLL.Administrator;
using LanManager.OSNativeUtils.Security.Windows;

namespace LanManager.Client
{
    /// <summary>
    /// Interaction logic for LogOn.xaml
    /// </summary>
    public partial class LogOn : Window
    {
        Window ParentWindow { get; set; }
        private bool shouldClose;

        public LogOn()
        {
            InitializeComponent();
        }

        private void btnLogOn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rdbCliente.IsChecked.Value)
                {
                    using (ClientSessionManager cl = new ClientSessionManager())
                    {

                        BLL.Client client = cl.LogOn(txtUserName.Text, txtPassword.Password, new Guid(ConfigurationManager.AppSettings["machineId"]));
                        if(client.PasswordExpired)
                        {
                            ChangePassword change = new ChangePassword(client);
                            change.ShowDialog();
                        }
                        Principal winPrincipal = new Principal();
                        winPrincipal.Show();
                        shouldClose = true;
                        Close();
                    }
                }
                else if (rdbAdmin.IsChecked.Value)
                {
                    using (AdminManager context = new AdminManager())
                    {
                        if (context.LogOnAdmin(txtUserName.Text, txtPassword.Password, null) != null)
                        {
                            System.Windows.Application.Current.Shutdown();
                        }
                        else
                        {
                            MessageBox.Show("Nome de usuário ou senha inválidos", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !shouldClose;
        }
    }
}
