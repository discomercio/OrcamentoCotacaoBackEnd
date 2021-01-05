using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Especificacao.Testes.Utils
{
    /*
     * classe para altear uma propriedade baseado no nome
     * usamos bastante por causa dos When Informo "campo = "valor"
     * lendo por reflexão, não precisamos testar todos os campos
     * 
     * podemos receber campos com ponto, por exemplo, FormaPagtoCriacao.Tipo_Parcelamento
     * 
     * 
     * */
    public static class WhenInformoCampo
    {
        public static bool InformarCampo(string campoCompleto, string valor, object destino)
        {
            string campoSimples = campoCompleto.Split('.')[0];
            string restoCampos = campoCompleto.Replace(campoSimples, "");
            //e tira o ponto da frente
            if (restoCampos.StartsWith("."))
                restoCampos = restoCampos.Substring(1);

            PropertyInfo[] properties = destino.GetType().GetProperties();
            foreach (var p in properties.Where(p => campoSimples.ToLower() == p.Name.ToLower()))
            {
                //se tiver mais campos, chama recursivamente
                if (restoCampos != "")
                {
                    object? destinoNovo = p.GetValue(destino);
                    if (destinoNovo == null)
                        return false;
                    return InformarCampo(restoCampos, valor, destinoNovo);
                }
                else
                {
                    string propertyTypeName = p.PropertyType.Name;
                    if (propertyTypeName.Contains("Nullable"))
                        propertyTypeName = Nullable.GetUnderlyingType(p.PropertyType)?.Name ?? "desconhecido";

                    //altera a propriedade
                    switch (propertyTypeName)
                    {
                        case "Byte":
                            valor = ConverterContribuinteICMS(campoSimples, valor);
                            valor = ConverterProdutorRural(campoSimples, valor);
                            p.SetValue(destino, Byte.Parse(valor));
                            break;
                        case "Int16":
                            valor = ConverterTipo_Parcelamento(campoSimples, valor);
                            p.SetValue(destino, System.Int16.Parse(valor));
                            break;
                        case "Int32":
                            valor = ConverterTipo_Parcelamento(campoSimples, valor);
                            p.SetValue(destino, System.Int32.Parse(valor));
                            break;
                        case "Decimal":
                            valor = ConverterTipo_Parcelamento(campoSimples, valor);
                            p.SetValue(destino, System.Decimal.Parse(valor));
                            break;
                        case "Boolean":
                            p.SetValue(destino, System.Boolean.Parse(valor));
                            break;
                        default:
                            valor = ConverterCustoFinancFornecTipoParcelamento(campoSimples, valor);
                            if (valor.ToLower() == "null")
                            {
                                p.SetValue(destino, null);
                            }
                            else
                            {
                                //se o tipo estiver errado vai dar erro, tudo bem...
                                p.SetValue(destino, valor);
                            }
                            break;
                    }
                    return true;
                }
            }
            return false;
        }

        private static string ConverterTipo_Parcelamento(string campo, string valor)
        {
            if (!campo.ToLower().Contains("Tipo_Parcelamento".ToLower()))
                return valor;
            switch (valor)
            {
                case "COD_FORMA_PAGTO_A_VISTA":
                    valor = InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA;
                    break;
                case "COD_FORMA_PAGTO_PARCELADO_CARTAO":
                    valor = InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO;
                    break;
                case "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA":
                    valor = InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA;
                    break;
                case "COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA":
                    valor = InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA;
                    break;
                case "COD_FORMA_PAGTO_PARCELA_UNICA":
                    valor = InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA;
                    break;
                case "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA":
                    valor = InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA;
                    break;
                case "COD_FORMA_PAGTO inválido":
                    valor = "99";
                    break;
                default:
                    return valor;
            }
            return valor;
        }
        private static string ConverterContribuinteICMS(string campo, string valor)
        {
            if (!campo.Contains("contribuinte_icms_status"))
                return valor;
            InfraBanco.Constantes.Constantes.ContribuinteICMS valorContribuinteICMS;
            switch (valor)
            {
                case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL":
                    valorContribuinteICMS = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
                    break;
                case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO":
                    valorContribuinteICMS = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
                    break;
                case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM":
                    valorContribuinteICMS = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                    break;
                case "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO":
                    valorContribuinteICMS = InfraBanco.Constantes.Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO;
                    break;
                default:
                    return valor;
            }
            return ((int)valorContribuinteICMS).ToString();
        }

        private static string ConverterProdutorRural(string campo, string valor)
        {
            if (!campo.Contains("produtor_rural_status"))
                return valor;
            InfraBanco.Constantes.Constantes.ProdutorRual valorProdutorRural;
            switch (valor)
            {
                case "COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL":
                    valorProdutorRural = InfraBanco.Constantes.Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
                    break;
                case "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO":
                    valorProdutorRural = InfraBanco.Constantes.Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;
                    break;
                case "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM":
                    valorProdutorRural = InfraBanco.Constantes.Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM;
                    break;
                default:
                    return valor;
            }
            return ((int)valorProdutorRural).ToString();
        }

        private static string ConverterCustoFinancFornecTipoParcelamento(string campo, string valor)
        {
            if (!campo.ToLower().Contains("CustoFinancFornecTipoParcelamento".ToLower()))
                return valor;
            switch (valor)
            {
                case "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA":
                    valor = InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA;
                    break;
                case "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA":
                    valor = InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
                    break;
                case "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA":
                    valor = InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA;
                    break;
                default:
                    return valor;
            }
            return valor;
        }
    }
}
