using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace UtilsGlobais
{
    public static class SenhaBll
    {
        public static string GeraChaveSenha()
        {
            return Gera_chave(fator: 1209);
        }
        public static string Gera_chave(int fator)
        {
            const int cod_min = 35;
            const int cod_max = 96;
            const int tamanhoChave = 128;

            string chave = "";

            for (int i = 1; i < tamanhoChave; i++)
            {
                int k = (cod_max - cod_min) + 1;
                k *= fator;
                k = (k * i) + cod_min;
                k %= 128;
                chave += (char)k;
            }

            return chave;
        }

        public static string DecodificaSenha(string origem)
        {
            string chave = GeraChaveSenha();
            string s_destino = "";
            string s_origem = origem;
            int i = s_origem.Length - 2;

            s_origem = s_origem.Substring(s_origem.Length - i, i);
            s_origem = s_origem.ToUpper();

            string s;
            string codificar = "";

            for (i = 1; i <= s_origem.Length; i++)
            {
                s = s_origem.Substring((i - 1), 2);
                if (s != "00")
                {
                    codificar = s_origem.Substring((i - 1), (s_origem.Length - i + 1));
                    break;
                }
                i++;
            }

            for (i = 0; i < codificar.Length; i++)
            {
                s = codificar.Substring(i, 2);
                int hexNumber = int.Parse(s, System.Globalization.NumberStyles.HexNumber);
                s_destino += (char)(hexNumber);
                i++;
            }
            s_origem = s_destino;
            s_destino = "";

            string letra;

            for (i = 0; i < s_origem.Length; i++)
            {
                //pega a letra
                letra = chave.Substring(i, 1);
                //Converte para char
                int i_chave = (Convert.ToChar(letra) * 2) + 1;
                int i_dado = Convert.ToChar(s_origem.Substring(i, 1));

                int contaMod = i_chave ^ i_dado;
                contaMod /= 2;
                s_destino += (char)contaMod;
            }

            return s_destino;
        }

        public static string CodificaSenha(string origem)
        {
            string chave = GeraChaveSenha();
            int i_chave = 0;
            int i_dado = 0;
            string s_origem = origem;
            string letra = "";
            string s_destino = "";

            if (s_origem.Length > 15)
            {
                s_origem = s_origem.Substring(0, 15);
            }

            for (int i = 0; i < s_origem.Length; i++)
            {
                letra = chave.Substring(i, 1);
                i_chave = (Convert.ToChar(letra) * 2) + 1;
                i_dado = Convert.ToChar(s_origem.Substring(i, 1)) * 2;
                int contaMod = i_chave ^ i_dado;
                s_destino += (char)contaMod;
            }

            s_origem = s_destino;
            s_destino = "";
            string destino = "";

            for (int i = 0; i < s_origem.Length; i++)
            {
                letra = s_origem.Substring(i, 1);
                i_chave = (Convert.ToChar(letra));
                string hexNumber = i_chave.ToString("X");

                while (hexNumber.Length < 2)
                {
                    hexNumber += "0";
                }
                destino += hexNumber;
            }

            while (destino.Length < 30)
            {
                destino = "0" + destino;
            }
            s_destino = "0x" + destino.ToUpper();

            return s_destino;
        }
        public static string MontaSessionCtrlInfo(string strUsuario, string strModulo, string strLoja,
            string strTicket, DateTime? dtLogon, DateTime? dtUltAtividade, int fatorCriptografia)
        {
            /*
            ' ------------------------------------------------------------------------
            '   MONTA SESSION CTRL INFO
            '   Monta o conjunto de dados criptografados usados p/ recuperar
            '   a sessão expirada.
            function MontaSessionCtrlInfo(ByVal strUsuario, ByVal strModulo, ByVal strLoja, ByVal strTicket, ByVal dtLogon, ByVal dtUltAtividade)
            dim strSessionCtrlParametro
            dim strSessionCtrlParametroCripto
            dim strChaveCripto
                strUsuario = Trim("" & strUsuario)
                strModulo = Trim("" & strModulo)
                strLoja = Trim("" & strLoja)
                strTicket = Trim("" & strTicket)
                strSessionCtrlParametro = strUsuario & "|" & strModulo & "|" & strLoja & "|" & strTicket & "|" & CStr(CDbl(dtLogon)) & "|" & CStr(CDbl(dtUltAtividade))
                strChaveCripto = gera_chave(FATOR_CRIPTO_SESSION_CTRL)
                strSessionCtrlParametroCripto = CriptografaTexto(strSessionCtrlParametro, strChaveCripto)
                MontaSessionCtrlInfo = strSessionCtrlParametroCripto
            end function

            */
            string ret = "";
            ret += (strUsuario ?? "").Trim();
            ret += "|";
            ret += (strModulo ?? "").Trim();
            ret += "|";
            ret += (strLoja ?? "").Trim();
            ret += "|";
            ret += strTicket ?? "";
            ret += "|";
            ret += FormatarData(dtLogon);
            ret += "|";
            ret += FormatarData(dtUltAtividade);
            var strChaveCripto = Gera_chave(fatorCriptografia);
            return CriptografaTexto(ret, strChaveCripto);
        }

        private static string FormatarData(DateTime? data)
        {
            if (data == null)
                return "";
            return data.Value.ToOADate().ToString();
        }

        //' -----------------------------
        //'   CRIPTOGRAFA TEXTO
        //' 
        public static string CriptografaTexto(string s_origem, string chave)
        {
#pragma warning disable IDE0054 // Use compound assignment
            s_origem = s_origem ?? "";
            chave = chave ?? "x";

            List<int> s_temporario = new List<int>();
            s_origem = s_origem.Trim();

            //    While Len(chave) < Len(s_origem): chave = chave & chave : Wend
            while (chave.Length < s_origem.Length)
                chave = chave + chave;

            //    For i = 1 To Len(s_origem)
            //        i_chave = (Asc(Mid(chave, i, 1)) * 2) + 1
            //        i_dado = Asc(Mid(s_origem, i, 1)) * 2
            //        k = i_chave Xor i_dado
            //        s_destino = s_destino & Chr(k)
            //        Next
            for (int i = 0; i < s_origem.Length; i++)
            {
                var i_chave = (((byte)chave.Substring(i, 1)[0]) * 2) + 1;
                var i_dado = (((byte)s_origem.Substring(i, 1)[0]) * 2);
                var k = i_chave ^ i_dado;
                var letra = k;
                s_temporario.Add(letra);
            }

            //    s_origem = s_destino
            //    s_destino = ""
            //    For i = 1 To Len(s_origem)
            //        k = Asc(Mid(s_origem, i, 1))
            //        s = Hex(k)
            //        While Len(s) < 2: s = "0" & s: Wend
            //        s_destino = s_destino & s
            //        Next
            string s_destino = "";
            foreach (var i in s_temporario)
            {
                s_destino += i.ToString("x2");
            }

            //    While Len(s_destino) < 30: s_destino = "0" & s_destino: Wend 
            //    s_destino = "0x" & LCase(s_destino)
            while (s_destino.Length < 30)
                s_destino = "0" + s_destino;

            //    CriptografaTexto = s_destino
            return s_destino;
#pragma warning restore IDE0054 // Use compound assignment
        }

        //' -----------------------------
        //'   DECRIPTOGRAFA TEXTO
        //' 
        public static string DecriptografaTexto(string s_origem, string chave)
        {
#pragma warning disable IDE0054 // Use compound assignment
            s_origem = s_origem ?? "";
            chave = chave ?? "x";

            List<int> s_temporario = new List<int>();
            s_origem = s_origem.Trim();

            //    i = Len(s_origem) - 2
            //    if i < 0 then i = 0
            //    s_origem = Right(s_origem, i) 
            //    s_origem = UCase(s_origem)
            //tira as duas primeiras letras da string, se for maior que 2
            if (s_origem.Length > 2)
                s_origem = s_origem.Substring(2);
            s_origem = s_origem.ToUpper();

            //    For i = 1 To Len(s_origem) Step 2
            //        s = Mid(s_origem, i, 2)
            //        If s <> "00" Then
            //            s_origem = Right(s_origem, Len(s_origem) - (i - 1))
            //            Exit For
            //            End If
            //        Next
            //tira todos os 00 da frente
            while (s_origem.StartsWith("00"))
                s_origem = s_origem.Substring(2);

            //    For i = 1 To Len(s_origem) Step 2
            //        s = Mid(s_origem, i, 2)
            //        s = "&H" & s
            //        s_destino = s_destino & Chr(s)
            //        Next
            for (int i = 0; i < s_origem.Length; i += 2)
                s_temporario.Add(int.Parse(s_origem.Substring(i, 2), System.Globalization.NumberStyles.HexNumber));

            //    s_origem = s_destino
            //    While Len(chave) < Len(s_origem): chave = chave & chave : Wend
            while (chave.Length < s_origem.Length)
                chave = chave + chave;

            //    s_destino = ""
            //    For i = 1 To Len(s_origem)
            //        i_chave = (Asc(Mid(chave, i, 1)) * 2) + 1
            //        i_dado = Asc(Mid(s_origem, i, 1))
            //        k = i_chave Xor i_dado
            //        k = k \ 2
            //        s_destino = s_destino & Chr(k)
            //        Next
            string s_destino = "";
            for (int i = 0; i < s_temporario.Count; i++)
            {
                var i_chave = (((byte)chave.Substring(i, 1)[0]) * 2) + 1;
                var i_dado = s_temporario[i];
                var k = i_chave ^ i_dado;
                var letra = (char)(k / 2);
                s_destino += letra;
            }

            //    DecriptografaTexto = s_destino
            return s_destino;
#pragma warning restore IDE0054 // Use compound assignment
        }
    }
}
