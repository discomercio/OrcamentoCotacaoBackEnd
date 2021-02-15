using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace UtilsGlobais
{
    public static class Log
    {
        #region[ enterParaLogBanco ]
        public static string EnterParaLogBanco()
        {
            char ret = (char)13;
            return ret.ToString();
        }
        #endregion

        private static readonly CultureInfo FormatarEmPortugues = new CultureInfo("pt-BR");
        public static string Formata_moeda_log(decimal? valor)
        {
            //se null, retorna vazio
            if (valor == null)
                return "";

            //formato: 1.053,81
            var ret = string.Format(FormatarEmPortugues, "{0:n}", valor);
            return ret;
        }
        public static string Formata_perc_comissao_log(float? valor)
        {
            //se null, retorna vazio
            if (valor == null)
                return "";

            //formato: 1.053,8
            var ret = string.Format(FormatarEmPortugues, "{0:0.0}", valor);
            return ret;
        }
        public static string Formata_texto_log_numeros<T>(T valor)
        {
            //se null, retorna duas aspas
            if (valor == null)
                return "\"\"";

            //formato: 1,5
            var ret = string.Format(FormatarEmPortugues, "{0}", valor);
            return ret;
        }

        public static string Formata_data_log(DateTime? valor)
        {
            //se null, retorna vazio
            if (valor == null)
                return "";

            //'   Formata somente a data: DD/MM/YYYY
            var ret = valor.Value.ToString("dd/MM/yyyy");
            return ret;
        }

        public static string Formata_texto_log(string valor)
        {
            //se null, retorna duas aspas
            if (string.IsNullOrWhiteSpace(valor))
                return "\"\"";

            return valor;
        }

        //' ---------------------------------------------------------------
        //'   LOG_PRODUTO_MONTA
        public static string Log_produto_monta(int? quantidade, string id_fabricante, string id_produto)
        {
            string s;

            s = " " + (quantidade ?? 0).ToString() + "x" + (id_produto ?? "").Trim();
            if ((id_fabricante ?? "").Trim() != "")
                s = s + "(" + (id_fabricante ?? "").Trim() + ")";
            return s;
        }
    }
}
