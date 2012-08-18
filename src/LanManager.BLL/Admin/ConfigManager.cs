using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanManager.DAL;

namespace LanManager.BLL.Admin
{
    public class ConfigManager : BaseManager
    {
        private const string razaoSocialSN = "RazaoSocial";
        private const string nomeFantasiaSN = "NomeFantasia";
        private const string cnpjSN = "CNPJ";
        private const string estado = "UF";
        private const string numeroNota = "numeroNota";
        private const string logradouro = "logradouro";
        private const string numero = "numero";
        private const string complemento = "complemento";
        private const string bairro = "bairro";
        private const string cidade = "cidade";
        private const string cep = "cep";
        private const string pais = "pais";
        private const string telefone = "telefone";
        private const string inscricaoestadual = "inscricaoestadual";
        private const string valorHora = "valorHora";

        public string RazaoSocial
        {
            get
            {
                return GetOrCreateConfig(razaoSocialSN, null);  
            }
            set
            {
                SetOrCreateConfig(razaoSocialSN, value);
            }
        }

        public string NomeFantasia
        {
            get
            {
                return GetOrCreateConfig(nomeFantasiaSN, null);
            }
            set
            {
                SetOrCreateConfig(nomeFantasiaSN, value);
            }
        }

        public string Cnpj
        {
            get
            {
                return GetOrCreateConfig(cnpjSN, null);
            }
            set
            {
                SetOrCreateConfig(cnpjSN, value);
            }
        }

        /// <summary>
        /// Estado(2 Dígitos)
        /// </summary>
        public string Estado
        {
            get
            {
                return GetOrCreateConfig(estado, null);
            }
            set
            {
                SetOrCreateConfig(estado, value);
            }
        }

        public int NumeroNota
        {
            get
            {
                if (string.IsNullOrEmpty(GetOrCreateConfig(numeroNota, null)))
                {
                    SetOrCreateConfig(numeroNota, "0");
                }
                return int.Parse(GetOrCreateConfig(numeroNota, null));
            }
            set
            {
                SetOrCreateConfig(numeroNota, value.ToString());
            }
        }

        #region Endereco
        public string Logradouro
        {
            get
            {
                return GetOrCreateConfig(logradouro, null);
            }
            set
            {
                SetOrCreateConfig(logradouro, value);
            }
        }

        public string Numero
        {
            get
            {
                return GetOrCreateConfig(numero, null);
            }
            set
            {
                SetOrCreateConfig(numero, value);
            }
        }

        public string Complemento
        {
            get
            {
                return GetOrCreateConfig(complemento, null);
            }
            set
            {
                SetOrCreateConfig(complemento, value);
            }
        }

        public string Bairro
        {
            get
            {
                return GetOrCreateConfig(bairro, null);
            }
            set
            {
                SetOrCreateConfig(bairro, value);
            }
        }

        public string Cidade
        {
            get
            {
                return GetOrCreateConfig(cidade, null);
            }
            set
            {
                SetOrCreateConfig(cidade, value);
            }
        }

        public string Cep
        {
            get
            {
                return GetOrCreateConfig(cep, null);
            }
            set
            {
                SetOrCreateConfig(cep, value);
            }
        }

        public string Pais
        {
            get
            {
                return GetOrCreateConfig(pais, null);
            }
            set
            {
                SetOrCreateConfig(pais, value);
            }
        }

        public string Telefone
        {
            get
            {
                return GetOrCreateConfig(telefone, null);
            }
            set
            {
                SetOrCreateConfig(telefone, value);
            }
        }
        #endregion

        public string InscricaoEstadual
        {
            get
            {
                return GetOrCreateConfig(inscricaoestadual, null);
            }
            set
            {
                SetOrCreateConfig(inscricaoestadual, value);
            }
        }

        public decimal ValorHora
        {
            get
            {
                decimal valorHoraDouble;
                decimal.TryParse(GetOrCreateConfig(valorHora, "2"), out valorHoraDouble);
                return valorHoraDouble;
            }
            set
            {
                value = decimal.Round(value, 2);
                SetOrCreateConfig(valorHora, value.ToString());
            }
        }


        private string GetOrCreateConfig(string key, string defaultValue)
        {
            var config = (from conf in db.Config
                         where conf.ShortName == key
                         select conf).FirstOrDefault();

            if (config == null)
            {
                Config newConfig = new Config();
                newConfig.ShortName = key;
                newConfig.Value = defaultValue ?? string.Empty;
                db.AddToConfig(newConfig);
                db.SaveChanges();
                return newConfig.Value;
            }
            return config.Value;
        }

        private void SetOrCreateConfig(string key, string value)
        {
            var config = (from conf in db.Config
                          where conf.ShortName == key
                          select conf).FirstOrDefault();

            if (config == null)
            {
                Config newConfig = new Config();
                newConfig.ShortName = key;
                newConfig.Value = value;
                db.AddToConfig(newConfig);
                db.SaveChanges();
            }
            else
            {
                config.Value = value;
            }
        }
    }
}
