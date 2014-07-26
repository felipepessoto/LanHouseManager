using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LanManager.BLL.Administrator;
using System.Globalization;
using System.IO;

namespace LanManager.BLL
{
    public class Nfe
    {
        public void GerarNfe(Client client, decimal howManyHours, decimal hourValue)
        {
            try
            {
                if(!Directory.Exists("nfe"))
                {
                    Directory.CreateDirectory("nfe");
                }

                using (ConfigManager configs = new ConfigManager())
                {
                    UTF8Encoding utf = new UTF8Encoding();
                    Guid id = Guid.NewGuid();
                    using (XmlTextWriter textWriter = new XmlTextWriter("nfe\\" + id + ".xml", utf))
                    {
                        textWriter.WriteStartDocument();
                        textWriter.WriteStartElement("Nfe", "http://www.portalfiscal.inf.br/nfe");
                        // Nó com tipo de Overload
                        textWriter.WriteStartElement("infNFe"); //Startando o elemento

                        textWriter.WriteStartAttribute("Id"); //Atributos do Nó
                        textWriter.WriteString("Nfe" + id);
                        // escrevendo no atributo
                        textWriter.WriteEndAttribute(); // finalizando o atributo

                        textWriter.WriteStartAttribute("versao"); //Atributos do Nó
                        textWriter.WriteString("1.10");
                        textWriter.WriteEndAttribute();

                        textWriter.WriteStartElement("ide"); //ide

                        textWriter.WriteStartElement("cUF"); 
                        textWriter.WriteString(BuscaCodigoEstado(configs.Estado).ToString()); // Código da Uf
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("cNF");
                        textWriter.WriteString("000000027"); // Código da Nf
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("natOp");
                        textWriter.WriteString("VENDAS DE PRODUCAO DO ESTABELECIMENTO"); // Natureza da Operação
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("indPag");
                        textWriter.WriteString("1"); // indicacao de pagamento
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("serie");
                        textWriter.WriteString("1"); // Série da Nota
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("nNF");
                        textWriter.WriteString(configs.NumeroNota.ToString()); // Numero da NF
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("dEmi"); 
                        textWriter.WriteString(DateTime.Now.ToString("yyyy-MM-dd")); // Data da Emissão
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("dSaiEnt"); 
                        textWriter.WriteString(DateTime.Now.ToString("yyyy-MM-dd")); // Data da Saida
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("tpNF"); 
                        textWriter.WriteString("1"); // Tipo da Nf
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("cMunFG"); 
                        textWriter.WriteString(BuscarCodigoCidade(configs.Cidade)); // Código do municipio fator Gerador
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("tpImp"); 
                        textWriter.WriteString("1"); // Tipo de impressao
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("tpEmis"); 
                        textWriter.WriteString("1"); // Tipo de Emissao
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("cDV"); 
                        textWriter.WriteString("4"); // Código verificador
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("tpAmb"); 
                        textWriter.WriteString("2"); // Tipo de ambiente
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("finNFe"); 
                        textWriter.WriteString("1"); // Finalidade da Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("procEmi"); 
                        textWriter.WriteString("1"); // Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("verProc"); 
                        textWriter.WriteString("1"); // Versao da Proc Nfe
                        textWriter.WriteEndElement();
                        textWriter.WriteEndElement(); //ide

                        textWriter.WriteStartElement("emit"); //ide

                        textWriter.WriteStartElement("CNPJ"); 
                        textWriter.WriteString(configs.Cnpj); // CNPJ Emitente
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xNome"); 
                        textWriter.WriteString(configs.RazaoSocial); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xFant"); 
                        textWriter.WriteString(configs.NomeFantasia); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("enderEmit"); 

                        textWriter.WriteStartElement("xLgr"); 
                        textWriter.WriteString(configs.Logradouro); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("nro"); 
                        textWriter.WriteString(configs.Numero); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xBairro"); 
                        textWriter.WriteString(configs.Bairro); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("cMun"); 
                        textWriter.WriteString(BuscarCodigoCidade(configs.Cidade)); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xMun"); 
                        textWriter.WriteString(configs.Cidade); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("UF"); 
                        textWriter.WriteString(configs.Estado); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("CEP"); 
                        textWriter.WriteString(configs.Cep); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("cPais"); 
                        textWriter.WriteString("1058"); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xPais"); 
                        textWriter.WriteString("Brasil"); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("fone"); 
                        textWriter.WriteString(configs.Telefone); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteEndElement(); //enderEmit

                        textWriter.WriteStartElement("IE"); 
                        textWriter.WriteString(configs.InscricaoEstadual); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteEndElement(); //emit
                        textWriter.WriteStartElement("dest"); //dest

                        textWriter.WriteStartElement("CNPJ"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xNome"); 
                        textWriter.WriteString(client.FullName); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("enderDest"); //enderDest

                        textWriter.WriteStartElement("xLgr"); 
                        textWriter.WriteString(client.StreetAddress); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("nro"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xBairro"); 
                        textWriter.WriteString(client.Neighborhood); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("cMun"); 
                        textWriter.WriteString(BuscarCodigoCidade(client.City)); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xMun"); 
                        textWriter.WriteString(client.City); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("UF"); 
                        textWriter.WriteString(client.State); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("CEP"); 
                        textWriter.WriteString(client.ZipCode); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("cPais"); 
                        textWriter.WriteString("1058"); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xPais"); 
                        textWriter.WriteString("Brasil"); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("fone"); 
                        textWriter.WriteString(client.Phone.ToString()); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteEndElement(); //enderDest

                        textWriter.WriteStartElement("IE"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteEndElement(); //dest

                        textWriter.WriteStartElement("entrega"); 

                        textWriter.WriteStartElement("CNPJ"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xLgr"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("nro"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xBairro"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("cMun"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("xMun"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteStartElement("UF"); 
                        textWriter.WriteString(""); // Versao da Proc Nfe
                        textWriter.WriteEndElement();

                        textWriter.WriteEndElement(); //entrega 
                        int incrementoProd = 1;
                        for (int i = 1; i <= 1; i++) //Itens da Nota
                        {
                            textWriter.WriteStartElement("det"); 
                            textWriter.WriteStartAttribute("nItem"); //Atributos do Nó
                            textWriter.WriteString(incrementoProd.ToString());
                            textWriter.WriteEndAttribute(); // finalizando o atributo
                            textWriter.WriteStartElement("prod"); 

                            textWriter.WriteStartElement("cProd"); 
                            textWriter.WriteString("2"); // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("cEAN"); 
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("xProd"); 
                            textWriter.WriteString("CRÉDITO EM HORAS"); // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("NCM"); 
                            textWriter.WriteString(""); // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("CFOP"); 
                            textWriter.WriteString("5101"); // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("uCom"); 
                            textWriter.WriteString("UN"); // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("qCom"); 
                            textWriter.WriteString(howManyHours.ToString()); // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("vProd");
                            textWriter.WriteString((howManyHours * hourValue).ToString());
                            // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("cEANTrib"); 
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("uTrib"); 
                            textWriter.WriteString("UN"); // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("qTrib");
                            textWriter.WriteString(howManyHours.ToString()); // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteStartElement("vUnTrib"); 
                            textWriter.WriteString(hourValue.ToString());
                                // Versao da Proc Nfe
                            textWriter.WriteEndElement();

                            textWriter.WriteEndElement();
                            textWriter.WriteEndElement();
                            incrementoProd++;
                        }



                        textWriter.WriteEndElement(); //InfNFE
                        textWriter.WriteEndElement(); //Nfe

                        textWriter.Close();
                        configs.NumeroNota += 1;
                        configs.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        public static int BuscaCodigoEstado(string siglaEstado)
        {
            if (siglaEstado == "RO") return 11;
            if (siglaEstado == "AC") return 12;
            if (siglaEstado == "AM") return 13;
            if (siglaEstado == "RR") return 14;
            if (siglaEstado == "PA") return 15;
            if (siglaEstado == "AP") return 16;
            if (siglaEstado == "TO") return 17;
            if (siglaEstado == "MA") return 21;
            if (siglaEstado == "PI") return 22;
            if (siglaEstado == "CE") return 23;
            if (siglaEstado == "RN") return 24;
            if (siglaEstado == "PB") return 25;
            if (siglaEstado == "PE") return 26;
            if (siglaEstado == "AL") return 27;
            if (siglaEstado == "SE") return 28;
            if (siglaEstado == "BA") return 29;
            if (siglaEstado == "MG") return 31;
            if (siglaEstado == "ES") return 32;
            if (siglaEstado == "RJ") return 33;
            if (siglaEstado == "SP") return 35;
            if (siglaEstado == "PR") return 41;
            if (siglaEstado == "SC") return 42;
            if (siglaEstado == "RS") return 43;
            if (siglaEstado == "MS") return 50;
            if (siglaEstado == "MT") return 51;
            if (siglaEstado == "GO") return 52;
            if (siglaEstado == "DF") return 53;
            return 0;
        }

        public static string BuscarCodigoCidade(string cidade)
        {
            return "0";  //TODO implementar
        }
    }
}
