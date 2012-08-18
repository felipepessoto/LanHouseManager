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

namespace LanManager.Client
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        private DAL.Client client;
        private bool shouldClose;

        public ChangePassword(DAL.Client client)
        {
            this.client = client;
            InitializeComponent();
            MessageBox.Show("Escolha uma nova senha");
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Length < 6)
            {
                MessageBox.Show("A senha deve ter entre 6 e 20 caracteres");
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("As senhas digitadas não conferem");
                txtPassword.Focus();
                return;
            }

            using(ClientManager context = new ClientManager())
            {
                context.ChangePassword(client, txtPassword.Password);
            }
            shouldClose = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !shouldClose;
        }
    }
}
