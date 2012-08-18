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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LanManager.BLL;
using LanManager.Instrumentation;

namespace LanManager.Server
{
    /// <summary>
    /// Interaction logic for ManageAppGroups.xaml
    /// </summary>
    public partial class ManageAppGroups : Window
    {
        private Guid editingGroupId;

        public ManageAppGroups()
        {
            InitializeComponent();
            RefreshGroupsList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            BLL.ApplicationGroup newAppGroup = new BLL.ApplicationGroup();
            newAppGroup.Name = txtAddName.Text;

            if (txtAddName.Text.Length == 0)
            {
                MessageBox.Show("Nome obrigatório");
                txtAddName.Focus();
                return;
            }

            using (ApplicationManager context = new ApplicationManager())
            {
                if (context.SearchGroupByName(newAppGroup.Name, null).Any())
                {
                    MessageBox.Show("Já existe um grupo com este nome");
                    return;
                }
                context.CreateApplicationGroup(newAppGroup);
            }
            RefreshGroupsList();
            Log.WriteAdminLog("Novo grupo cadastrado: " + txtAddName.Text, LogOn.LoggedAdmin.UserName);
            MessageBox.Show("Novo grupo cadastrado");
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            RefreshGroupsList();
        }

        private void RefreshGroupsList()
        {
            using (ApplicationManager context = new ApplicationManager())
            {
                var appGroups = context.SearchAllGroups(null);
                dtgSearchResult.ItemsSource = appGroups;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (txtEditName.Text.Length == 0)
            {
                MessageBox.Show("Nome obrigatório");
                txtEditName.Focus();
                return;
            }

            using (ApplicationManager context = new ApplicationManager())
            {
                BLL.ApplicationGroup editingApp = context.SearchGroupById(editingGroupId, null);
                editingApp.Name = txtEditName.Text;

                if (context.SearchGroupByName(editingApp.Name, null).Any(x => x.Id != editingGroupId))
                {
                    MessageBox.Show("Já existe um grupo com este nome");
                    return;
                }

                context.SaveChanges();
            }
            Log.WriteAdminLog("Grupo editado: " + txtEditName.Text, LogOn.LoggedAdmin.UserName);
            MessageBox.Show("Alterações realizadas com sucesso");
            tabEdit.Visibility = Visibility.Hidden;
            tabPrincipal.SelectedItem = tabSearch;
            RefreshGroupsList();
        }

        private void dtgSearchResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var linha = ((DataGrid)sender).SelectedItem as BLL.ApplicationGroup;

            if (linha != null)
            {
                BLL.ApplicationGroup editingApp;
                editingGroupId = linha.Id;

                using (ApplicationManager context = new ApplicationManager())
                {
                    editingApp = context.SearchGroupById(editingGroupId, null);
                }

                txtEditName.Text = editingApp.Name;
                tabEdit.Visibility = Visibility.Visible;
                tabPrincipal.SelectedItem = tabEdit;
            }
        }

        private void dtgSearchResult_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
