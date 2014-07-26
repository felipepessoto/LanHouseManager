using System.Windows;
using System.Windows.Controls;
using LanManager.BLL;
using LanManager.BLL.Administrator;

namespace LanManager.Server
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        public Config()
        {
            InitializeComponent();

            using (ConfigManager configManager = new ConfigManager())
            {
                txtNomeEmpresa.Text = configManager.RazaoSocial;
                txtNomeFantasia.Text = configManager.NomeFantasia;
                txtCnpj.Text = configManager.Cnpj;
                txtLogradouro.Text = configManager.Logradouro;
                txtNumero.Text = configManager.Numero;
                txtBairro.Text = configManager.Bairro;
                txtCidade.Text = configManager.Cidade;
                txtEstado.Text = configManager.Estado;
                txtCEP.Text = configManager.Cep;
                txtTelefone.Text = configManager.Telefone;
                txtInscEstadual.Text = configManager.InscricaoEstadual;
                txtValorHora.Text = configManager.ValorHora.ToString();
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if(!ValidaCnpj(txtCnpj.Text))
            {
                MessageBox.Show("CNPJ inválido");
                return;
            }

            decimal valorHora;
            if(!decimal.TryParse(txtValorHora.Text, out valorHora))
            {
                MessageBox.Show("Valor da hora inválido");
                return;
            }

            valorHora = decimal.Round(valorHora, 2);

            using(ConfigManager configManager = new ConfigManager())
            {
                configManager.RazaoSocial = txtNomeEmpresa.Text;
                configManager.NomeFantasia = txtNomeFantasia.Text;
                configManager.Cnpj = txtCnpj.Text;
                configManager.Logradouro = txtLogradouro.Text;
                configManager.Numero = txtNumero.Text;
                configManager.Bairro = txtBairro.Text;
                configManager.Cidade = txtCidade.Text;
                configManager.Estado = txtEstado.Text;
                configManager.Cep = txtCEP.Text;
                configManager.Telefone = txtTelefone.Text;
                configManager.InscricaoEstadual = txtInscEstadual.Text;
                configManager.ValorHora = valorHora;
                configManager.SaveChanges();
            }
            Log.WriteAdminLog("Alterações de configuração", LogOn.LoggedAdmin.UserName);
            MessageBox.Show("Configurações salvas");
            Close();
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

        public bool ValidaCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

    }
}
