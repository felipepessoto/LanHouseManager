using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LanManager.BLL;
using LanManager.BLL.Administrator;
using LanManager.Instrumentation;

namespace LanManager.Server
{
    /// <summary>
    /// Interaction logic for ManageClients.xaml
    /// </summary>
    public partial class ManageClients : Window
    {
        private Guid editingClientId;

        public ManageClients()
        {
            InitializeComponent();
            SetLabels();

            LoadParents();
        }

        private void LoadParents()
        {
            using (ClientManager context = new ClientManager())
            {
                var parentList = context.GetClients18YearsOld(null);
                parentList.Insert(0, new Client {FullName = "<Nenhum>"});
                cboAddPrincipal.ItemsSource = cboEditPrincipal.ItemsSource = parentList;
                cboAddPrincipal.SelectedIndex = 0;
            }
        }

        private void SetLabels()
        {
            dtpAddBirthDate.SelectedDate = DateTime.Now;

            cboAddSchoolTime.SelectedValuePath = "Value";
            cboAddSchoolTime.DisplayMemberPath = "Text";
            cboAddSchoolTime.Items.Add(new { Text = ResourceSet.Resources.SchoolTime0, Value = 0 });
            cboAddSchoolTime.Items.Add(new { Text = ResourceSet.Resources.SchoolTime1, Value = 1 });
            cboAddSchoolTime.Items.Add(new { Text = ResourceSet.Resources.SchoolTime2, Value = 2 });
            cboAddSchoolTime.Items.Add(new { Text = ResourceSet.Resources.SchoolTime3, Value = 3 });
            cboAddSchoolTime.SelectedIndex = 0;
            cboAddPrincipal.DisplayMemberPath = "FullName";
            cboAddPrincipal.SelectedValuePath = "Id";

            cboEditSchoolTime.SelectedValuePath = "Value";
            cboEditSchoolTime.DisplayMemberPath = "Text";
            cboEditSchoolTime.Items.Add(new { Text = ResourceSet.Resources.SchoolTime0, Value = 0 });
            cboEditSchoolTime.Items.Add(new { Text = ResourceSet.Resources.SchoolTime1, Value = 1 });
            cboEditSchoolTime.Items.Add(new { Text = ResourceSet.Resources.SchoolTime2, Value = 2 });
            cboEditSchoolTime.Items.Add(new { Text = ResourceSet.Resources.SchoolTime3, Value = 3 });
            cboEditSchoolTime.SelectedIndex = 0;
            cboEditPrincipal.DisplayMemberPath = "FullName";
            cboEditPrincipal.SelectedValuePath = "Id";
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddUserName.Text.Length == 0)
            {
                MessageBox.Show("O campo nome de usuário é obrigatório");
                txtAddUserName.Focus();
                return;
            }

            if (txtAddPassword.Password.Length < 6)
            {
                MessageBox.Show("A senha deve ter entre 6 e 20 caracteres");
                txtAddPassword.Focus();
                return;
            }

            if (txtAddPassword.Password != txtAddPasswordConfirm.Password)
            {
                MessageBox.Show("As senhas digitadas não conferem");
                txtAddPassword.Focus();
                return;
            }

            long? phoneNumber = null;
            if (!string.IsNullOrEmpty(txtAddPhone.Text))
            {
                long phoneNumberAux;
                if (long.TryParse(txtAddPhone.Text, out phoneNumberAux))
                {
                    phoneNumber = phoneNumberAux;
                }
            }


            long? mobilePhoneNumber = null;
            if (!string.IsNullOrEmpty(txtAddMobilePhone.Text))
            {
                long mobilePhoneNumberAux;
                if (long.TryParse(txtAddMobilePhone.Text, out mobilePhoneNumberAux))
                {
                    mobilePhoneNumber = mobilePhoneNumberAux;
                }
            }

            if (txtAddFullName.Text.Length == 0)
            {
                MessageBox.Show("O campo nome completo é obrigatório");
                txtAddFullName.Focus();
                return;
            }

            if (txtAddDocumentId.Text.Length == 0)
            {
                MessageBox.Show("O campo RG é obrigatório");
                txtAddDocumentId.Focus();
                return;
            }

            if (!ValidateDocumentId(txtAddDocumentId.Text))
            {
                MessageBox.Show("RG inválido");
                txtAddDocumentId.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtAddCPF.Text) && !ValidarCpf(txtAddCPF.Text))
            {
                MessageBox.Show("CPF inválido");
                txtAddCPF.Focus();
                return;

            }

            if (txtAddStreetAddress.Text.Length == 0)
            {
                MessageBox.Show("O campo endereço é obrigatório");
                txtAddStreetAddress.Focus();
                return;
            }

            if (txtAddState.Text.Length == 0)
            {
                MessageBox.Show("O campo estado é obrigatório");
                txtAddState.Focus();
                return;
            }

            if (!dtpAddBirthDate.SelectedDate.HasValue || dtpAddBirthDate.SelectedDate.Value.Year < 1800 || dtpAddBirthDate.SelectedDate.Value > DateTime.Now)
            {
                MessageBox.Show("Data de nascimento inválida");
                dtpAddBirthDate.Focus();
                return;
            }

            if (dtpAddBirthDate.SelectedDate.Value > DateTime.Now.AddYears(-18))
            {
                if (txtAddFatherName.Text.Length == 0 && txtAddMotherName.Text.Length == 0)
                {
                    MessageBox.Show("O campo nome do responsável é obrigatório para menores de 18 anos");
                    txtAddFatherName.Focus();
                    return;
                }

                if (txtAddSchool.Text.Length == 0)
                {
                    MessageBox.Show("O campo escola é obrigatório para menores de 18 anos");
                    txtAddSchool.Focus();
                    return;
                }

                if (cboAddSchoolTime.SelectedIndex == 0)
                {
                    MessageBox.Show("O campo horário escolar é obrigatório para menores de 18 anos");
                    cboAddSchoolTime.IsDropDownOpen = true;
                    return;
                }

            }

            if (dtpAddBirthDate.SelectedDate.Value > DateTime.Now.AddYears(-16) && cboAddPrincipal.SelectedIndex == 0)
            {
                MessageBox.Show("O responsável é obrigatório para menores de 16 anos");
                cboAddPrincipal.IsDropDownOpen = true;
                return;
            }

            Client newClient = new Client
                                   {
                                       Id = Guid.NewGuid(),
                                       RegisterDate = DateTime.Now,
                                       UserName = txtAddUserName.Text,
                                       Password = txtAddPassword.Password,
                                       PasswordExpired = true,
                                       MinutesLeft = 0,
                                       MinutesBonus = 0,
                                       MaxDebit = int.Parse(txtAddMaxDebit.Text),
                                       HasMidnightPermission = chkAddHasMidnightPermission.IsChecked.Value,
                                       CanAccessAnyApplication = chkAddCanAccessAnyApp.IsChecked.Value,
                                       CanAccessAnyTime = chkAddCanAccessAnyTime.IsChecked.Value,
                                       FullName = txtAddFullName.Text,
                                       BirthDate = dtpAddBirthDate.SelectedDate.Value,
                                       DocumentID = txtAddDocumentId.Text,
                                       Nickname = txtAddNickName.Text,
                                       Active = true,
                                       FatherName = txtAddFatherName.Text,
                                       MotherName = txtAddMotherName.Text,
                                       StreetAddress = txtAddStreetAddress.Text,
                                       Neighborhood = txtAddNeighborhood.Text,
                                       City = txtAddCity.Text,
                                       State = txtAddState.Text,
                                       Country = txtAddCountry.Text,
                                       ZipCode = txtAddZip.Text,
                                       Phone = phoneNumber,
                                       MobilePhone = mobilePhoneNumber,
                                       Email = txtAddEmail.Text,
                                       School = txtAddSchool.Text,
                                       SchoolTime = int.Parse(cboAddSchoolTime.SelectedValue.ToString()),
                                       CPF = txtAddCPF.Text
                                   };

            using(ClientManager context = new ClientManager())
            {
                if (context.SearchByUsername(newClient.UserName, null) != null)
                {
                    MessageBox.Show("Este nome de usuário já existe");
                    txtAddUserName.Focus();
                    return;
                }

                if (context.SearchByDocumentID(newClient.DocumentID, null) != null)
                {
                    if (MessageBox.Show("Já existe um cliente com este RG, continuar?", "Continuar?", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                if (newClient.CPF.Length > 0 && context.SearchByCPF(newClient.CPF, null) != null)
                {
                    if (MessageBox.Show("Já existe um cliente com este CPF, continuar?", "Continuar?", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                if (cboAddPrincipal.SelectedIndex > 0)
                {
                    var mainClient = context.GetClientsById(new Guid(cboAddPrincipal.SelectedValue.ToString()), null);
                    newClient.Parent = mainClient;
                }
                context.CreateClient(newClient);
            }

            Log.WriteAdminLog("Novo cliente cadastrado: " + txtAddUserName.Text, LogOn.LoggedAdmin.UserName);
            MessageBox.Show("Novo cliente registrado");
            ClearAllAddFields();
            LoadParents();
        }

        private void btnSearchClient_Click(object sender, RoutedEventArgs e)
        {
            SearchClients();
        }

        private void SearchClients()
        {
            using (ClientManager context = new ClientManager())
            {
                var clients = context.SearchClients(txtSearch.Text, null);
                dtgSearchResult.ItemsSource = (from cl in clients
                                               select
                                                   new DtgSearchView
                                                       {
                                                           Id = cl.Id,
                                                           UserName = cl.UserName,
                                                           FullName = cl.FullName,
                                                           DocumentID = cl.DocumentID,
                                                           BirthDate = cl.BirthDate,
                                                           Email = cl.Email,
                                                           MinutesLeft = cl.MinutesLeft,
                                                           MinutesBonus = cl.MinutesBonus,
                                                           MaxDebit = cl.MaxDebit
                                                       }).ToList();

            }
        }

        class DtgSearchView
        {
            public Guid Id { get; set; }
            public string UserName { get; set; }
            public string FullName { get; set; }
            public string DocumentID { get; set; }
            public DateTime BirthDate { get; set; }
            public string Email { get; set; }
            public int MinutesLeft { get; set; }
            public int MinutesBonus { get; set; }
            public int MaxDebit { get; set; }
        }

        private void btnEditClient_Click(object sender, RoutedEventArgs e)
        {
            long? phoneNumber = null;
            if (!string.IsNullOrEmpty(txtEditPhone.Text))
            {
                long phoneNumberAux;
                if (long.TryParse(txtEditPhone.Text, out phoneNumberAux))
                {
                    phoneNumber = phoneNumberAux;
                }
            }

            long? mobilePhoneNumber = null;
            if (!string.IsNullOrEmpty(txtEditMobilePhone.Text))
            {
                long mobilePhoneNumberAux;
                if (long.TryParse(txtEditMobilePhone.Text, out mobilePhoneNumberAux))
                {
                    mobilePhoneNumber = mobilePhoneNumberAux;
                }
            }

            if (txtEditPassword.Password.Length > 0 && txtEditPassword.Password.Length < 6)
            {
                MessageBox.Show("A senha deve ter entre 6 e 20 caracteres");
                txtEditPassword.Focus();
                return;
            }

            if (txtEditPassword.Password.Length > 0 && txtEditPassword.Password != txtEditPasswordConfirm.Password)
            {
                MessageBox.Show("As senhas digitadas não conferem");
                txtEditPassword.Focus();
                return;
            }

            if (txtEditFullName.Text.Length == 0)
            {
                MessageBox.Show("O campo nome completo é obrigatório");
                txtEditFullName.Focus();
                return;
            }

            if (txtEditDocumentId.Text.Length == 0)
            {
                MessageBox.Show("O campo RG é obrigatório");
                txtEditDocumentId.Focus();
                return;
            }

            if (!ValidateDocumentId(txtEditDocumentId.Text))
            {
                MessageBox.Show("RG inválido");
                txtEditDocumentId.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtEditCPF.Text) && !ValidarCpf(txtEditCPF.Text))
            {
                MessageBox.Show("CPF inválido");
                txtEditCPF.Focus();
                return;
            }

            if (txtEditStreetAddress.Text.Length == 0)
            {
                MessageBox.Show("O campo endereço é obrigatório");
                txtEditStreetAddress.Focus();
                return;
            }

            if (txtEditState.Text.Length == 0)
            {
                MessageBox.Show("O campo estado é obrigatório");
                txtEditState.Focus();
                return;
            }

            if (!dtpEditBirthDate.SelectedDate.HasValue || dtpEditBirthDate.SelectedDate.Value.Year < 1800 || dtpEditBirthDate.SelectedDate.Value > DateTime.Now)
            {
                MessageBox.Show("Data de nascimento inválida");
                dtpAddBirthDate.Focus();
                return;
            }

            if (dtpEditBirthDate.SelectedDate.Value > DateTime.Now.AddYears(-18))
            {
                if (txtEditFatherName.Text.Length == 0 && txtEditMotherName.Text.Length == 0)
                {
                    MessageBox.Show("O campo nome do responsável é obrigatório para menores de 18 anos");
                    txtEditFatherName.Focus();
                    return;
                }

                if (txtEditSchool.Text.Length == 0)
                {
                    MessageBox.Show("O campo escola é obrigatório para menores de 18 anos");
                    txtEditSchool.Focus();
                    return;
                }


                if (cboEditSchoolTime.SelectedIndex == 0)
                {
                    MessageBox.Show("O campo horário escolar é obrigatório para menores de 18 anos");
                    cboEditSchoolTime.IsDropDownOpen = true;
                    return;
                }
            }

            if (dtpEditBirthDate.SelectedDate.Value > DateTime.Now.AddYears(-16) && cboEditPrincipal.SelectedIndex == 0)
            {
                MessageBox.Show("O responsável é obrigatório para menores de 16 anos");
                cboEditPrincipal.IsDropDownOpen = true;
                return;
            }

            using (ClientManager context = new ClientManager())
            {
                Client editedClient = context.GetClientsById(editingClientId, null);

                editedClient.UserName = txtEditUserName.Text;
                if (txtEditPassword.Password.Length > 0)
                {
                    editedClient.Password = txtEditPassword.Password;
                    editedClient.PasswordExpired = true;
                }
                editedClient.MaxDebit = int.Parse(txtEditMaxDebit.Text);
                editedClient.HasMidnightPermission = chkEditHasMidnightPermission.IsChecked.Value;
                editedClient.CanAccessAnyApplication = chkEditCanAccessAnyApp.IsChecked.Value;
                editedClient.CanAccessAnyTime = chkEditCanAccessAnyTime.IsChecked.Value;
                editedClient.FullName = txtEditFullName.Text;
                editedClient.BirthDate = dtpEditBirthDate.SelectedDate.Value;
                editedClient.DocumentID = txtEditDocumentId.Text;
                editedClient.Nickname = txtEditNickName.Text;
                editedClient.Active = chkEditActive.IsChecked.Value;
                editedClient.FatherName = txtEditFatherName.Text;
                editedClient.MotherName = txtEditMotherName.Text;
                editedClient.StreetAddress = txtEditStreetAddress.Text;
                editedClient.Neighborhood = txtEditNeighborhood.Text;
                editedClient.City = txtEditCity.Text;
                editedClient.State = txtEditState.Text;
                editedClient.Country = txtEditCountry.Text;
                editedClient.ZipCode = txtEditZip.Text;
                editedClient.Phone = phoneNumber;
                editedClient.MobilePhone = mobilePhoneNumber;
                editedClient.Email = txtEditEmail.Text;
                editedClient.School = txtEditSchool.Text;
                editedClient.SchoolTime = int.Parse(cboEditSchoolTime.SelectedValue.ToString());
                editedClient.LastUpdateDate = DateTime.Now;
                editedClient.CPF = txtEditCPF.Text;

                if (cboEditPrincipal.SelectedIndex > 0)
                {
                    var mainClient = context.GetClientsById(new Guid(cboEditPrincipal.SelectedValue.ToString()), null);
                    editedClient.Parent = mainClient;
                }
                else
                {
                    editedClient.Parent = null;
                }

                context.SaveChanges();
            }

            Log.WriteAdminLog("Cliente editado: " + txtEditUserName.Text, LogOn.LoggedAdmin.UserName);
            MessageBox.Show("Alterações realizadas com sucesso");
            tabEditClient.Visibility = Visibility.Hidden;
            tabClient.SelectedItem = tabSearchClient;
            txtEditPassword.Clear();
            txtEditPasswordConfirm.Clear();
            LoadParents();
            SearchClients();
        }

        private void SearchEditClientButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            editingClientId = new Guid(button.CommandParameter.ToString());

            Client editingClient;
            using (ClientManager context = new ClientManager())
            {
                editingClient = context.GetClientsById(editingClientId, new[] { "Parent" });
            }

            if (editingClient.Parent == null)
            {
                cboEditPrincipal.SelectedIndex = 0;
            }
            else
            {
                cboEditPrincipal.SelectedValue = editingClient.Parent.Id;
            }

            txtEditUserName.Text = editingClient.UserName;
            txtEditMaxDebit.Text = editingClient.MaxDebit.ToString();
            chkEditHasMidnightPermission.IsChecked = editingClient.HasMidnightPermission;
            chkEditCanAccessAnyApp.IsChecked = editingClient.CanAccessAnyApplication;
            chkEditCanAccessAnyTime.IsChecked = editingClient.CanAccessAnyTime;
            txtEditFullName.Text = editingClient.FullName;
            dtpEditBirthDate.SelectedDate = editingClient.BirthDate;
            txtEditDocumentId.Text = editingClient.DocumentID;
            txtEditNickName.Text = editingClient.Nickname;
            chkEditActive.IsChecked = editingClient.Active;
            txtEditFatherName.Text = editingClient.FatherName;
            txtEditMotherName.Text = editingClient.MotherName;
            txtEditStreetAddress.Text = editingClient.StreetAddress;
            txtEditCity.Text = editingClient.City;
            txtEditState.Text = editingClient.State;
            txtEditCountry.Text = editingClient.Country;
            txtEditZip.Text = editingClient.ZipCode;
            txtEditPhone.Text = editingClient.Phone.ToString();
            txtEditMobilePhone.Text = editingClient.MobilePhone.ToString();
            txtEditEmail.Text = editingClient.Email;
            txtEditSchool.Text = editingClient.School;
            cboEditSchoolTime.SelectedValue = editingClient.SchoolTime;
            txtEditCPF.Text = editingClient.CPF;

            tabEditClient.Visibility = Visibility.Visible;
            tabClient.SelectedItem = tabEditClient;

            //}
        }

        private void DetailSearchClientButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            StackPanel panelData = ((StackPanel) button.Parent);

            Guid clientId = new Guid(button.CommandParameter.ToString());

            decimal money;
            if (!decimal.TryParse(((TextBox) button.FindName("changeMinutesLeft")).Text, out money))
            {
                MessageBox.Show("Valor inválido");
                panelData.Children[0].Focus();
                return;
            }
            money = decimal.Round(money, 2);

            if (money < 0 && MessageBox.Show("Você inseriu um número negativo, deseja debitar os créditos?", "Deseja debitar?", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                panelData.Children[0].Focus();
                return;
            }

            int minutesBonus;
            if (!int.TryParse(((TextBox) button.FindName("changeMinutesBonus")).Text, out minutesBonus))
            {
                MessageBox.Show("Digite apenas números");
                panelData.Children[1].Focus();
                return;
            }

            if (minutesBonus < 0)
            {
                MessageBox.Show("Não é permitido bônus negativo");
                panelData.Children[1].Focus();
                return;
            }

            decimal hourValue; 
            using (ConfigManager context = new ConfigManager())
            {
                hourValue = context.ValorHora;
            }
            int minutesPurchased = (int) (money/hourValue*60m);

            using (ClientManager context = new ClientManager())
            {
                Client editingClient = context.GetClientsById(clientId, null);
                if (minutesPurchased > 0 || editingClient.MinutesBonus != minutesBonus)
                {
                    Log.WriteAdminLog("Créditos adicionados para o cliente: " + editingClient.UserName + ". Pagos: " + minutesPurchased + ". Bonus: " + (minutesBonus - editingClient.MinutesBonus), LogOn.LoggedAdmin.UserName);
                }
                editingClient.MinutesLeft += minutesPurchased;
                editingClient.MinutesBonus = minutesBonus;

                if (minutesPurchased > 0)
                {
                    MinutesPurchased purchased = MinutesPurchased.CreateMinutesPurchased(Guid.NewGuid(), minutesPurchased, DateTime.Now);
                    purchased.Client = editingClient;
                    context.CreateMinutesPurchasedHistory(purchased);

                    Nfe nota = new Nfe();
                    nota.GerarNfe(editingClient, minutesPurchased / 60m, hourValue);
                }
                context.SaveChanges();
            }

            MessageBox.Show("Créditos atualizados");

            SearchClients();
        }

        private static bool ValidateDocumentId(string documentId)
        {
            for (int i = 0; i < documentId.Length; i++)
            {
                //Aceita letra no ultimo campo
                if ((!char.IsDigit(documentId[i]) && i < documentId.Length - 1) && documentId[i] != '.' && documentId[i] != '-')
                {
                    return false;
                }
            }
            return true;
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

        private static bool ValidarCpf(string cpf)
        {
            int[] multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        private void ClearAllAddFields()
        {
            cboAddPrincipal.SelectedIndex = 0;
            txtAddUserName.Clear();
            txtAddPassword.Clear();
            txtAddPasswordConfirm.Clear();
            txtAddMaxDebit.Text = "0";
            chkAddHasMidnightPermission.IsChecked = false;
            chkAddCanAccessAnyApp.IsChecked = false;
            chkAddCanAccessAnyTime.IsChecked = false;
            txtAddFullName.Clear();
            dtpAddBirthDate.SelectedDate = DateTime.Now;
            txtAddDocumentId.Clear();
            txtAddNickName.Clear();
            txtAddFatherName.Clear();
            txtAddMotherName.Clear();
            txtAddStreetAddress.Clear();
            txtAddCity.Clear();
            txtAddState.Clear();
            txtAddCountry.Clear();
            txtAddZip.Clear();
            txtAddPhone.Clear();
            txtAddMobilePhone.Clear();
            txtAddEmail.Clear();
            txtAddSchool.Clear();
            cboAddSchoolTime.SelectedIndex = 0;
            txtAddCPF.Clear();
        }

        private void dtgSearchResult_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
