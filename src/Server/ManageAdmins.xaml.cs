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
using LanManager.DAL;
using LanManager.BLL.Admin;
using LanManager.Instrumentation;

namespace LanManager.Server
{
    /// <summary>
    /// Interaction logic for ManageAdmins.xaml
    /// </summary>
    public partial class ManageAdmins : Window
    {
        private Guid editingAdminId;

        public ManageAdmins()
        {
            InitializeComponent();
        }

        private void btnAddAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddUserName.Text.Length == 0)
            {
                MessageBox.Show("Digite o nome de usuário");
                txtAddUserName.Focus();
                return;
            }

            if (txtAddPassword.Password.Length == 0)
            {
                MessageBox.Show("Digite a senha");
                txtAddPassword.Focus();
                return;
            }

            if (txtAddFullName.Text.Length == 0)
            {
                MessageBox.Show("Digite o nome completo");
                txtAddFullName.Focus();
                return;
            }

            if (txtAddPassword.Password != txtAddPasswordConfirm.Password)
            {
                MessageBox.Show("As senhas digitadas não conferem");
                txtAddPasswordConfirm.Focus();
                return;
            }

            Admin newAdmin = new Admin
                                 {
                                     Id = Guid.NewGuid(),
                                     UserName = txtAddUserName.Text,
                                     Password = txtAddPassword.Password,
                                     FullName = txtAddFullName.Text,
                                     Active = chkAddActive.IsChecked.Value
                                 };

            using(AdminManager context = new AdminManager())
            {
                if (context.SearchByUsername(newAdmin.UserName, null) != null)
                {
                    MessageBox.Show(ResourceSet.Resources.MessageAdminExists);
                    return;
                }
                context.CreateAdmin(newAdmin);
            }
            Log.WriteAdminLog("Novo admin cadastrado: " + txtAddUserName.Text, LogOn.LoggedAdmin.UserName);
            MessageBox.Show("Novo admin cadastrado");
            txtAddUserName.Clear();
            txtAddFullName.Clear();
            txtAddPassword.Clear();
            txtAddPasswordConfirm.Clear();
        }

        private void btnSearchAdmin_Click(object sender, RoutedEventArgs e)
        {
            SearchAdmin();
        }

        private void SearchAdmin()
        {
            using (AdminManager context = new AdminManager())
            {
                var admins = context.SearchAdmins(txtSearch.Text, null);
                dtgSearchResult.ItemsSource = (from admin in admins
                                               select
                                                   new DtgSearchView
                                                       {
                                                           Id = admin.Id,
                                                           UserName = admin.UserName,
                                                           FullName = admin.FullName,
                                                           Active = admin.Active,
                                                       }).ToList();

            }
        }

        class DtgSearchView
        {
            public Guid Id { get; set; }
            public string UserName { get; set; }
            public string FullName { get; set; }
            public bool Active { get; set; }
        }

        private void dtgSearchResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var linha = ((Microsoft.Windows.Controls.DataGrid)sender).SelectedItem as DtgSearchView;

            if (linha != null)
            {
                Admin editingAdmin;
                editingAdminId = linha.Id;

                using (AdminManager context = new AdminManager())
                {
                    editingAdmin = context.SearchById(editingAdminId, null);
                }

                txtEditUserName.Text = editingAdmin.UserName;
                txtEditFullName.Text = editingAdmin.FullName;
                chkEditActive.IsChecked = editingAdmin.Active;
                

                tabEditAdmin.Visibility = Visibility.Visible;
                tabAdmin.SelectedValue = tabEditAdmin;
            }
        }

        private void btnEditAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (txtEditPassword.Password.Length > 0 && txtEditPassword.Password != txtEditPasswordConfirm.Password)
            {
                MessageBox.Show("As senhas digitadas não conferem");
                txtEditPassword.Focus();
                return;
            }

            if (txtEditFullName.Text.Length == 0)
            {
                MessageBox.Show("Digite o nome completo");
                txtEditFullName.Focus();
                return;
            }

            using (AdminManager context = new AdminManager())
            {
                try
                {
                    context.EditAdmin(editingAdminId, txtEditPassword.Password, txtEditFullName.Text, chkEditActive.IsChecked.Value);
                }
                catch(UniqueAdminActiveException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            Log.WriteAdminLog("Admin editado: " + txtEditUserName.Text, LogOn.LoggedAdmin.UserName);
            MessageBox.Show("Alterações realizadas com sucesso");
            tabEditAdmin.Visibility = Visibility.Hidden;
            tabAdmin.SelectedItem = tabSearchAdmin;
            txtEditPassword.Clear();
            txtEditPasswordConfirm.Clear();
            SearchAdmin();
        }

        private void dtgSearchResult_BeginningEdit(object sender, Microsoft.Windows.Controls.DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
