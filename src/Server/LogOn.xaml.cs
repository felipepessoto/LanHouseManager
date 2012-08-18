using System.Windows;
using LanManager.BLL.Admin;
using LanManager.Instrumentation;

namespace LanManager.Server
{
    /// <summary>
    /// Interaction logic for LogOn.xaml
    /// </summary>
    public partial class LogOn : Window
    {
        internal static DAL.Admin LoggedAdmin;

        public LogOn()
        {
            InitializeComponent();
            txtUserName.Focus();
        }

        private void btnLogOn_Click(object sender, RoutedEventArgs e)
        {
            using (AdminManager context = new AdminManager())
            {
                LoggedAdmin = context.LogOnAdmin(txtUserName.Text, txtPassword.Password, null);
                if (LoggedAdmin != null)
                {
                    Log.WriteAdminLog("Login", LoggedAdmin.UserName);
                    Principal winPrincipal = new Principal();
                    winPrincipal.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show(ResourceSet.Resources.AdminLogOnInvalid);
                }
            }
        }
    }
}