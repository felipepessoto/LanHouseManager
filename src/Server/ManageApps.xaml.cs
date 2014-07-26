using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace LanManager.Server
{
    /// <summary>
    /// Interaction logic for ManageApps.xaml
    /// </summary>
    public partial class ManageApps : Window
    {
        private Guid editingAppId;

        public ManageApps()
        {
            InitializeComponent();

            using (ApplicationManager context = new ApplicationManager())
            {
                var groups = context.SearchAllGroups(null);
                groups.Insert(0, new ApplicationGroup { Name = "<Nenhum>" });
                cboAddGroup.DisplayMemberPath = "Name";
                cboAddGroup.SelectedValuePath = "Id";
                cboAddGroup.ItemsSource = groups;
                cboAddGroup.SelectedIndex = 0;

                cboEditGroup.DisplayMemberPath = "Name";
                cboEditGroup.SelectedValuePath = "Id";
                cboEditGroup.ItemsSource = groups;
                cboEditGroup.SelectedIndex = 0;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int age;
            int.TryParse(txtAddAge.Text, out age);

            if(txtAddName.Text.Length == 0)
            {
                MessageBox.Show("Nome obrigatório");
                txtAddName.Focus();
                return;
            }

            if (txtAddPath.Text.Length == 0)
            {
                MessageBox.Show("Caminho do aplicativo obrigatório");
                txtAddPath.Focus();
                return;
            }

            byte[] imageBytes = null;
            if (txtAddImage.Text.Length > 0)
            {
                if (!File.Exists(txtAddImage.Text))
                {
                    MessageBox.Show("Imagem não encontrada");
                    txtAddImage.Focus();
                    return;
                }

                FileStream fStream = new FileStream(txtAddImage.Text, FileMode.Open, FileAccess.Read);

                if (fStream.Length > 5 * 1024 * 1024)
                {
                    MessageBox.Show("Imagem muito grande. Limite de 5MB");
                    txtAddImage.Focus();
                    return;
                }


                imageBytes = new byte[fStream.Length];
                fStream.Read(imageBytes, 0, (int) fStream.Length);
            }


            BLL.Application newApp = new BLL.Application();
            newApp.Name = txtAddName.Text;
            newApp.DefaultPath = txtAddPath.Text;
            newApp.RunArguments = txtAddArg.Text;
            newApp.MinimumAge = age;
            newApp.Active = chkAddActive.IsChecked.Value;
            newApp.Icon = imageBytes;
            newApp.InstalledAt = txtAddInstalledAt.Text;

            using (ApplicationManager context = new ApplicationManager())
            {
                if (cboAddGroup.SelectedIndex > 0)
                {
                    newApp.ApplicationGroup = context.SearchGroupById(new Guid(cboAddGroup.SelectedValue.ToString()),
                                                                      null);
                }
                context.CreateApplication(newApp);
            }

            Log.WriteAdminLog("Novo aplicativo cadastrado: " + txtAddName.Text, LogOn.LoggedAdmin.UserName);
            MessageBox.Show("Novo aplicativo cadastrado");

            txtAddName.Clear();
            txtAddAge.Clear();
            txtAddPath.Clear();
            txtAddPath.Clear();
            txtAddImage.Clear();
            txtAddArg.Clear();
            txtAddInstalledAt.Clear();
            cboAddGroup.SelectedIndex = 0;
            chkAddActive.IsChecked = false;
        }

        private static string SearchDialog(string filter)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            if (dialog.ShowDialog().Value)
            {
                return dialog.FileName;
            }

            return null;
        }

        private void btnAddSearchPath_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = SearchDialog("executable files (*.exe)|*.exe|all files (*.*)|*.*");

            if (!string.IsNullOrEmpty(imagePath))
            {
                txtAddPath.Text = imagePath;
            }
        }

        private void btnAddSearchImage_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = SearchDialog("jpg files (*.jpg)|*.jpg|gif files (*.gif)|*.gif|png files (*.png)|*.png");

            if (!string.IsNullOrEmpty(imagePath))
            {
                txtAddImage.Text = imagePath;
            }
        }

        private void btnEditSearchPath_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = SearchDialog("executable files (*.exe)|*.exe|all files (*.*)|*.*");

            if (!string.IsNullOrEmpty(imagePath))
            {
                txtEditPath.Text = imagePath;
            }
        }

        private void btnEditSearchImage_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = SearchDialog("jpg files (*.jpg)|*.jpg|gif files (*.gif)|*.gif|png files (*.png)|*.png");

            if (!string.IsNullOrEmpty(imagePath))
            {
                txtEditImage.Text = imagePath;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            int age;
            int.TryParse(txtEditAge.Text, out age);

            if (txtEditName.Text.Length == 0)
            {
                MessageBox.Show("Nome obrigatório");
                txtEditName.Focus();
                return;
            }

            if (txtEditPath.Text.Length == 0)
            {
                MessageBox.Show("Caminho do aplicativo obrigatório");
                txtEditPath.Focus();
                return;
            }

            byte[] imageBytes = null;
            if (txtEditImage.Text.Length > 0)
            {
                if (!File.Exists(txtEditImage.Text))
                {
                    MessageBox.Show("Imagem não encontrada");
                    txtEditImage.Focus();
                    return;
                }

                FileStream fStream = new FileStream(txtEditImage.Text, FileMode.Open, FileAccess.Read);

                if (fStream.Length > 5 * 1024 * 1024)
                {
                    MessageBox.Show("Imagem muito grande. Limite de 5MB");
                    txtEditImage.Focus();
                    return;
                }


                imageBytes = new byte[fStream.Length];
                fStream.Read(imageBytes, 0, (int)fStream.Length);
            }

            using (ApplicationManager context = new ApplicationManager())
            {
                BLL.Application editingApp = context.GetApplicationById(editingAppId, null);
                editingApp.Name = txtEditName.Text;
                editingApp.DefaultPath = txtEditPath.Text;
                editingApp.RunArguments = txtEditArg.Text;
                editingApp.MinimumAge = age;
                editingApp.Active = chkEditActive.IsChecked.Value;
                editingApp.Icon = imageBytes ?? editingApp.Icon;
                editingApp.InstalledAt = txtEditInstalledAt.Text;

                if (cboEditGroup.SelectedIndex > 0)
                {
                    editingApp.ApplicationGroup = context.SearchGroupById(new Guid(cboEditGroup.SelectedValue.ToString()),
                                                                      null);
                }
                context.SaveChanges();

                tabEdit.Visibility = Visibility.Hidden;
                tabPrincipal.SelectedItem = tabSearch;
            }
            Log.WriteAdminLog("Aplicativo editado: " + txtEditName.Text, LogOn.LoggedAdmin.UserName);
            MessageBox.Show("Alterações realizadas com sucesso");
            SearchApps();
        }

        private void btnSearchApp_Click(object sender, RoutedEventArgs e)
        {
            SearchApps();
        }

        private void SearchApps()
        {
            using (ApplicationManager context = new ApplicationManager())
            {
                var apps = context.SearchApplications(txtSearch.Text, null);
                dtgSearchResult.ItemsSource = apps;

            }
        }

        private void dtgSearchResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var linha = ((DataGrid)sender).SelectedItem as BLL.Application;

            if (linha != null)
            {
                BLL.Application editingApp;
                editingAppId = linha.Id;

                using (ApplicationManager context = new ApplicationManager())
                {
                    editingApp = context.GetApplicationById(editingAppId, new[] { "ApplicationGroup" });
                }

                if (editingApp.ApplicationGroup == null)
                {
                    cboEditGroup.SelectedIndex = 0;
                }
                else
                {
                    cboEditGroup.SelectedValue = editingApp.ApplicationGroup.Id;
                }

                txtEditName.Text = editingApp.Name;
                txtEditPath.Text = editingApp.DefaultPath;
                txtEditArg.Text = editingApp.RunArguments;
                txtEditAge.Text = editingApp.MinimumAge.ToString();
                txtEditInstalledAt.Text = editingApp.InstalledAt;
                imgEditImage.Source = ByteToImage(editingApp.Icon);
                txtEditImage.Clear();
                chkEditActive.IsChecked = editingApp.Active;

                tabEdit.Visibility = Visibility.Visible;
                tabPrincipal.SelectedItem = tabEdit;

            }
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
                return null;
            }
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string numbers = string.Empty;
            for (int i = 0; i < ((TextBox)sender).Text.Length; i++)
            {
                if (char.IsDigit(((TextBox)sender).Text[i]))
                    numbers += ((TextBox)sender).Text[i];
            }
            ((TextBox)sender).Text = numbers;
        }

        private void dtgSearchResult_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}