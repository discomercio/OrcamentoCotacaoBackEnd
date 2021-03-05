using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsGlobais
{
    public static class TratamentoTexto
    {
        public static string Retorna_so_digitos(string s_numero)
        {
            s_numero ??= "";
            return System.Text.RegularExpressions.Regex.Replace(s_numero, @"\D", "");
        }

        public static string Retira_acentuacao(string texto)
        {
            //' ------------------------------------------------------------------------
            //'   RETIRA ACENTUACAO
            //'   Retira a acentuação do texto
            texto ??= "";
            var s_resp = new StringBuilder();
            foreach (var letra in texto)
            {
                string c = letra.ToString();
                if ((c == "Á") || (c == "À") || (c == "Ã") || (c == "Â") || (c == "Ä"))
                    c = "A";
                else if ((c == "á") || (c == "à") || (c == "ã") || (c == "â") || (c == "ä"))
                    c = "a";
                else if ((c == "É") || (c == "È") || (c == "Ê") || (c == "Ë"))
                    c = "E";
                else if ((c == "é") || (c == "è") || (c == "ê") || (c == "ë"))
                    c = "e";
                else if ((c == "Í") || (c == "Ì") || (c == "Î") || (c == "Ï"))
                    c = "I";
                else if ((c == "í") || (c == "ì") || (c == "î") || (c == "ï"))
                    c = "i";
                else if ((c == "Ó") || (c == "Ò") || (c == "Õ") || (c == "Ô") || (c == "Ö"))
                    c = "O";
                else if ((c == "ó") || (c == "ò") || (c == "õ") || (c == "ô") || (c == "ö"))
                    c = "o";
                else if ((c == "Ú") || (c == "Ù") || (c == "Û") || (c == "Ü"))
                    c = "U";
                else if ((c == "ú") || (c == "ù") || (c == "û") || (c == "ü"))
                    c = "u";
                else if (c == "Ç")
                    c = "C";
                else if (c == "ç")
                    c = "c";
                else if (c == "Ñ")
                    c = "N";
                else if (c == "ñ")
                    c = "n";
                else if (c == "ÿ")
                    c = "y";


                s_resp.Append(c);
            }
            return s_resp.ToString();
        }

    }
}
