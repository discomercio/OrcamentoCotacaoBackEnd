using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using InfraBanco;
using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Modelos;

namespace UtilsGlobais
{
    public class Util
    {
        public static string HoraParaBanco(DateTime hora)
        {
            return hora.Hour.ToString().PadLeft(2, '0') +
                    hora.Minute.ToString().PadLeft(2, '0') +
                    hora.Second.ToString().PadLeft(2, '0');
        }

        public static string FormatCpf_Cnpj_Ie(string cpf_cnpj)
        {
            //caso esteja vazio, não formatamos
            if (!UInt64.TryParse(cpf_cnpj, out UInt64 convertido))
                return cpf_cnpj;

            if (cpf_cnpj.Length > 11)
            {
                if (cpf_cnpj.Length > 12)
                    return convertido.ToString(@"00\.000\.000\/0000\-00");
                else
                    return convertido.ToString(@"000\.000\.000\.000");
            }
            else
            {
                return convertido.ToString(@"000\.000\.000\-00");
            }

        }

        public static string Telefone_SoDigito(string tel)
        {
            return tel.Replace("-", "");
        }
        public static string FormatarTelefones(string telefone)
        {
            return telefone.Insert(telefone.Length - 4, "-");
        }

        public static bool ValidaCPF(string cpf)
        {
            string valor = cpf.Replace(".", "").Replace("/", "").Replace("-", "");

            valor = valor.Replace("-", "");

            if (valor.Length != 11) return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
            {
                if (valor[i] != valor[0])
                    igual = false;
            }

            if (igual) return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
            {
                numeros[i] = int.Parse(valor[i].ToString());
            }

            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * numeros[i];
            }

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }

            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += (11 - i) * numeros[i];
            }

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
            {
                if (numeros[10] != 11 - resultado)
                    return false;
            }

            return true;
        }

        public static bool ValidaCNPJ(string cnpj)
        {
            string p1 = "543298765432";
            string p2 = "6543298765432";

            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            if (cnpj == "") return true;
            if (cnpj.Length != 14) return false;

            // DÍGITOS TODOS IGUAIS?
            bool tudo_igual = true;
            for (int i = 0; i < (cnpj.Length - 1); i++)
                if (cnpj.Substring(i, i + 1) != cnpj.Substring(i + 1, i + 2))
                {
                    tudo_igual = false;
                    break;
                }

            if (tudo_igual) return false;

            // VERIFICA O PRIMEIRO CHECK DIGIT
            long d = 0;

            for (int i = 0; i < 12; i++)
            {
                d = d + (Convert.ToInt64(p1.Substring(i, 1)) * Convert.ToInt64(cnpj.Substring(i, 1)));
            }


            d = 11 - (d % 11);
            if (d > 9) d = 0;
            if (d != Convert.ToInt64(cnpj.Substring(12, 1))) return false;

            // VERIFICA O SEGUNDO CHECK DIGIT
            d = 0;
            for (int i = 0; i < 13; i++)
                d = d + Convert.ToInt64(p2.Substring(i, 1)) * Convert.ToInt64(cnpj.Substring(i, 1));

            d = 11 - (d % 11);
            if (d > 9) d = 0;
            if (d != Convert.ToInt32(cnpj.Substring(13, 1))) return false;

            return true;
        }

        public static string SoDigitosCpf_Cnpj(string cpf_cnpj)
        {
            string retorno;

            if (cpf_cnpj.Length > 11)
            {
                if (cpf_cnpj.Length > 12)
                    retorno = cpf_cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                else
                    retorno = cpf_cnpj.Replace(".", "");
            }
            else
                retorno = cpf_cnpj.Replace(".", "").Replace("-", "");

            return retorno;
        }

        public static string RemoverAcentuacao(string text)
        {
            return new string(text
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        public static async Task<string> ObterDescricao_Cod(string grupo, string cod, ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var desc = from c in db.TcodigoDescricaos
                       where c.Grupo == grupo && c.Codigo == cod
                       select c.Descricao;

            string result = await desc.FirstOrDefaultAsync();

            if (result == null || result == "")
                return "Código não cadastrado (" + cod + ")";

            return result;
        }

        public static string OpcaoFormaPagto(short codigo)
        {
            string retorno = "";

            switch (codigo)
            {
                case (short)Constantes.FormaPagto.ID_FORMA_PAGTO_DINHEIRO:
                    retorno = "Dinheiro";
                    break;
                case (short)Constantes.FormaPagto.ID_FORMA_PAGTO_DEPOSITO:
                    retorno = "Depósito";
                    break;
                case (short)Constantes.FormaPagto.ID_FORMA_PAGTO_CHEQUE:
                    retorno = "Cheque";
                    break;
                case (short)Constantes.FormaPagto.ID_FORMA_PAGTO_BOLETO:
                    retorno = "Boleto";
                    break;
                case (short)Constantes.FormaPagto.ID_FORMA_PAGTO_CARTAO:
                    retorno = "Cartão (internet)";
                    break;
                case (short)Constantes.FormaPagto.ID_FORMA_PAGTO_CARTAO_MAQUINETA:
                    retorno = "Cartão (maquineta)";
                    break;
                case (short)Constantes.FormaPagto.ID_FORMA_PAGTO_BOLETO_AV:
                    retorno = "Boleto AV";
                    break;
            };

            return retorno;
        }

        public static bool VerificaUf(string uf)
        {
            bool retorno = false;
            string sigla = "AC AL AM AP BA CE DF ES GO MA MG MS MT PA PB PE PI PR RJ RN RO RR RS SC SE SP TO  ";

            if (uf.Length == 2 && sigla.Contains(uf.ToUpper()))
                retorno = true;

            return retorno;
        }

        public static bool VerificaCep(string cep)
        {
            string cepFormat;

            if (cep != "")
            {
                cepFormat = cep.Replace("-", "");
                if (cepFormat.Length == 5)
                    return true;
                if (cepFormat.Length == 8)
                    return true;
            }

            return false;
        }

        public static bool gera_chave_codificacao(Int32 fator, ref String chave_gerada)
        {
            int COD_MINIMO = 35;
            int COD_MAXIMO = 96;
            int TAMANHO_CHAVE = 128;
            int i;
            Int64 k;
            StringBuilder s = new StringBuilder("");

            for (i = 1; i <= TAMANHO_CHAVE; i++)
            {
                k = COD_MAXIMO - COD_MINIMO + 1;
                k = k * fator;
                k = (k * i) + COD_MINIMO;
                k = k % 128;
                s.Append(Texto.chr((short)k));
            }
            chave_gerada = s.ToString();
            return true;
        }

        public static void shift_esquerda(ref byte byteNumero, byte byteCasas)
        {
            int i;
            String s_byte;

            // Transforma decimal -> binário ('0101...')
            s_byte = converte_dec_para_bin(byteNumero);

            // Rotaciona
            for (i = 1; i <= byteCasas; i++)
            {
                s_byte = Texto.rightStr(s_byte, s_byte.Length - 1);
                s_byte += "0";
            }

            // Transforma binário -> decimal
            byteNumero = converte_bin_para_dec(s_byte);
        }

        private static String converte_dec_para_bin(byte byteNumero)
        {
            String s;
            s = Convert.ToString(byteNumero, 2);
            s = s.PadLeft(8, '0');
            return s;
        }

        private static byte converte_bin_para_dec(String strNumero)
        {
            try
            {
                return Convert.ToByte(strNumero, 2);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void rotaciona_esquerda(ref byte byteNumero, byte byteCasas)
        {
            int i;
            String s;
            String s_byte;

            // Transforma decimal -> binário ('0101...')
            s_byte = converte_dec_para_bin(byteNumero);

            // Rotaciona
            for (i = 1; i <= byteCasas; i++)
            {
                s = Texto.leftStr(s_byte, 1);
                s_byte = Texto.rightStr(s_byte, s_byte.Length - 1);
                s_byte += s;
            }

            // Transforma binário -> decimal
            byteNumero = converte_bin_para_dec(s_byte);
        }

        public static void rotaciona_direita(ref byte byteNumero, byte byteCasas)
        {
            int i;
            String s;
            String s_byte;

            // Transforma decimal -> binário ('0101...')
            s_byte = converte_dec_para_bin(byteNumero);

            //Rotaciona
            for (i = 1; i <= byteCasas; i++)
            {
                s = Texto.rightStr(s_byte, 1);
                s_byte = Texto.leftStr(s_byte, s_byte.Length - 1);
                s_byte = s + s_byte;
            }
            // Transforma binário -> decimal
            byteNumero = converte_bin_para_dec(s_byte);
        }

        #region Antigo gera chave
        //antigo
        public static string GeraChave(int fator)
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
        #endregion

        public static string FormataHora(string hora)
        {
            string retorno = "";
            if (hora.Length >= 4)
            {
                string _hora = hora.Substring(0, 2);
                string _minuto = hora.Substring(2, 2);

                retorno = _hora + ":" + _minuto;
            }

            return retorno;
        }

        public static string decodificaDado(String strOrigem, int fator)
        {
            byte i;
            byte i_chave;
            byte i_dado;
            byte k;
            String s_origem;
            String s_destino;
            String s;
            String chave = "";

            string strDestino = "";

            if (strOrigem == null) return strDestino;
            if (strOrigem.Trim().Length == 0) return strDestino;

            if (!gera_chave_codificacao(fator, ref chave)) return strDestino;

            s_destino = "";
            s_origem = strOrigem.Trim();

            // Possui prefixo '0x'?
            if (!Texto.leftStr(s_origem, Constantes.PREFIXO_SENHA_FORMATADA.Length).Equals(Constantes.PREFIXO_SENHA_FORMATADA)) return strDestino;

            // Retira prefixo '0x' da máscara (imita formato timestamp)
            s_origem = Texto.rightStr(s_origem, s_origem.Length - Constantes.PREFIXO_SENHA_FORMATADA.Length);
            s_origem = s_origem.ToUpper();

            // Retira caracteres de preenchimento (imita formato timestamp)
            s = Texto.leftStr(s_origem, Constantes.TAMANHO_CAMPO_COMPRIMENTO_SENHA);
            s = "0x" + s;
            try
            {
                i = Convert.ToByte(s, 16);
            }
            catch (Exception)
            {
                i = 0;
            }

            if (i != 0)
            {
                s_origem = Texto.rightStr(s_origem, i);
            }
            else
            {
                while (s_origem.Substring(0, 2).Equals("00"))
                {
                    s_origem = Texto.rightStr(s_origem, s_origem.Length - 2);
                }
            }

            // Hexadecimal -> ASCII
            for (i = 1; i <= s_origem.Length; i += 2)
            {
                s = s_origem.Substring(i - 1, 2);
                s = "0x" + s;
                s_destino += Texto.chr(Convert.ToByte(s, 16));
            }

            // Descriptografa pela chave
            s_origem = s_destino;
            s_destino = "";
            for (i = 1; i <= s_origem.Length; i++)
            {
                i_chave = Texto.asc(chave.ToCharArray()[i - 1]);
                Util.shift_esquerda(ref i_chave, 1);
                i_chave++;

                i_dado = Texto.asc(s_origem.ToCharArray()[i - 1]);
                // XOR
                k = (byte)(i_chave ^ i_dado);

                Util.rotaciona_direita(ref k, 1);
                s_destino = s_destino + Texto.chr(k);
            }

            strDestino = s_destino;
            return strDestino;
        }

        public static string DecodificaSenha(string origem, string chave)
        {
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
                    codificar = s_origem.Substring((i - 1), (s_origem.Length - i + 1)); ;
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

        public static string codificaDado(String strOrigem, bool blnIncluiPreenchimento)
        {
            byte i;
            int i_tam_senha;
            byte i_chave;
            byte i_dado;
            byte k;
            String s_origem;
            String s_destino;
            String s;
            String chave;

            string strDestino = "";

            // Senha de origem está vazia
            if (strOrigem == null) return strDestino;
            if (strOrigem.Trim().Length == 0) return strDestino;

            // Senha excede tamanho
            if (strOrigem.Trim().Length > (Constantes.TAMANHO_SENHA_FORMATADA -
                Constantes.PREFIXO_SENHA_FORMATADA.Length) / 2) return strDestino;

            // Gera chave de criptografia
            chave = "";
            if (!Util.gera_chave_codificacao(Constantes.FATOR_CRIPTO, ref chave)) return strDestino;

            s_destino = "";
            s_origem = strOrigem.Trim();

            // Criptografa pela chave
            for (i = 1; i <= s_origem.Length; i++)
            {
                i_chave = Texto.asc(chave.ToCharArray()[i - 1]);
                Util.shift_esquerda(ref i_chave, 1);
                i_chave++;

                i_dado = Texto.asc(s_origem.ToCharArray()[i - 1]);
                Util.rotaciona_esquerda(ref i_dado, 1);

                // XOR
                k = (byte)(i_chave ^ i_dado);

                s_destino = s_destino + Texto.chr(k);
            }

            // ASCII -> Hexadecimal
            s_origem = s_destino;
            s_destino = "";
            for (i = 1; i <= s_origem.Length; i++)
            {
                k = Texto.asc(s_origem.ToCharArray()[i - 1]);
                s = Texto.hex(k);
                s = s.PadLeft(2, '0');
                s_destino = s_destino + s;
            }

            // Guarda o tamanho real da senha
            i_tam_senha = s_destino.Length;

            if (blnIncluiPreenchimento)
            {
                // Coloca máscara (imita formato timestamp)
                i = 0;
                while (s_destino.Length < (Constantes.TAMANHO_SENHA_FORMATADA -
                    Constantes.PREFIXO_SENHA_FORMATADA.Length - Constantes.TAMANHO_CAMPO_COMPRIMENTO_SENHA))
                {
                    // Ao invés de preencheer com zeros, gera código p/ preenchimento
                    i++;
                    s = Texto.hex(i ^ (Convert.ToInt16("0x" + s_destino.Substring(s_destino.Length - (i - 1) - 1, 1), 16)) ^ (Convert.ToInt16("0x" + s_destino.Substring(s_destino.Length - i - 1, 1), 16)));
                    // Adiciona um caracter por vez p/ não ter o risco de ultrapassar o tamanho máximo
                    s_destino = Texto.rightStr(s, 1) + s_destino;
                }

                // Adiciona prefixo e tamanho real da senha
                s = Texto.hex(i_tam_senha);
                s = s.PadLeft(2, '0');
            }
            else
            {
                while (s_destino.Length < (Constantes.TAMANHO_SENHA_FORMATADA -
                    Constantes.PREFIXO_SENHA_FORMATADA.Length - Constantes.TAMANHO_CAMPO_COMPRIMENTO_SENHA))
                {
                    s_destino = "0" + s_destino;
                }
                s = "00";
            }

            //vou incluir uma verificação para saber se esta correto a qtde de caracteres da senha codificada
            if ((Constantes.PREFIXO_SENHA_FORMATADA + s_destino).Length < 32)
            {
                s_destino = Constantes.PREFIXO_SENHA_FORMATADA + s + s_destino;
            }
            else
            {
                s_destino = Constantes.PREFIXO_SENHA_FORMATADA + s_destino;
            }

            strDestino = s_destino.ToLower();
            return strDestino;
        }

        public static string GerarSenhaAleatoria()
        {
            string senha = "";
            int i;
            char c;

            while (senha.Length < 10)
            {
                Random random = new Random();
                i = random.Next(122 - 48 + 1, 122 - 48 + 1 + 48);

                if (i >= 48 && i <= 57)
                {
                    c = (char)i;
                }
                else if (i >= 65 && i <= 90)
                {
                    c = (char)i;
                }
                else if (i >= 97 && i <= 122)
                {
                    c = (char)i;
                }
                else
                {
                    c = ' ';
                }

                senha += c.ToString().Trim();
            }


            return senha;

        }

        public static string MontaLogInserir(Object obj, string log, string campos_a_inserir2, bool tratarEndEtg)
        {
            PropertyInfo[] property = obj.GetType().GetProperties();
            string[] campos = campos_a_inserir2.Split('|');
            int indiceDoCampo = 0;
            bool endEntregaMesmo = false;
            bool dataEntregaImediata = false;
            foreach (var campo_atual in campos)
            {
                foreach (var c in property)
                {
                    //pegando o real nome da coluna 
                    ColumnAttribute column = (ColumnAttribute)Attribute.GetCustomAttribute(c, typeof(ColumnAttribute));
                    if (column != null)
                    {
                        string coluna = column.Name;

                        //vl total=" & formata_moeda(vl_total)
                        //colocar esse vl total em 1º
                        if (campo_atual.IndexOf("vl total") >= 0)
                        {
                            log = campo_atual + "; ";
                        }
                        if (campo_atual.IndexOf("Endereço entrega") >= 0 && !endEntregaMesmo)
                        {
                            endEntregaMesmo = true;
                            log = log + campo_atual + "; ";
                        }
                        if (campo_atual.IndexOf("previsão de entrega") >= 0 && !dataEntregaImediata)
                        {
                            dataEntregaImediata = true;
                            log = log + campo_atual + "; ";
                        }

                        if (campo_atual == coluna)
                        {
                            //pegando o valor coluna
                            var value = (c.GetValue(obj, null));
                            if (value == null)
                                log = log + coluna + "=" + "\"\"" + "; ";
                            else if (string.IsNullOrEmpty(value.ToString()))
                                log = log + coluna + "=" + "\"\"" + "; ";
                            else if (value.GetType().Name == "Decimal")
                                log = log + coluna + "=" + string.Format("{0:n}", value) + "; ";
                            else
                                log = log + coluna + "=" + value + "; ";
                        }
                    }
                }
                indiceDoCampo++;
            }
            return log;
        }

        public static string MontaLog(Object obj, string log, string campos_a_omitir)
        {
            PropertyInfo[] property = obj.GetType().GetProperties();
            string[] campos = campos_a_omitir.Split('|');

            foreach (var c in property)
            {
                //pegando o real nome da coluna 
                ColumnAttribute column = (ColumnAttribute)Attribute.GetCustomAttribute(c, typeof(ColumnAttribute));
                if (column != null)
                {
                    string coluna = column.Name;
                    if (!campos_a_omitir.Contains(coluna))
                    {
                        //pegando o valor coluna
                        var value = (c.GetValue(obj, null));
                        if (value == null)
                            log = log + coluna + "=" + "\"\"" + "; ";
                        else if (string.IsNullOrEmpty(value.ToString()))
                            log = log + coluna + "=" + "\"\"" + "; ";
                        else
                            log = log + coluna + "=" + value + "; ";
                    }
                }
            }

            return log;
        }
        public static bool GravaLog(ContextoBdGravacao dbgravacao, string apelido, string loja, string pedido, string id_cliente,
            string operação, string log)
        {
            if (apelido == null)
                return false;

            Tlog tLog = new Tlog
            {
                Data = DateTime.Now,
                Usuario = apelido,
                Loja = loja,
                Pedido = pedido,
                Id_Cliente = id_cliente,
                Operacao = operação,
                Complemento = log
            };

            dbgravacao.Add(tLog);
            dbgravacao.SaveChanges();

            return true;
        }

        public static string Normaliza_Codigo(string cod, int tamanho_default)
        {
            string retorno = cod;
            string s = "0";


            if (cod != "")
            {
                for (int i = cod.Length; i < tamanho_default; i++)
                {
                    retorno = s + retorno;
                }
            }

            return retorno;
        }

        public static bool ValidarTipoCustoFinanceiroFornecedor(List<string> lstErros, string custoFinanceiroTipoParcelato,
            int c_custoFinancFornecQtdeParcelas)
        {
            bool retorno = true;

            if (custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA &&
                custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
            {
                lstErros.Add("A forma de pagamento não foi informada (à vista, com entrada, sem entrada).");
                retorno = false;
            }
            if (custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
            {
                if (custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
                {
                    if (c_custoFinancFornecQtdeParcelas <= 0)
                    {
                        lstErros.Add("Não foi informada a quantidade de parcelas para a forma de pagamento selecionada " +
                            "(" + DescricaoCustoFornecTipoParcelamento(custoFinanceiroTipoParcelato) + ")");
                        retorno = false;
                    }
                }
            }

            return retorno;
        }

        public static string DescricaoCustoFornecTipoParcelamento(string custoFinanceiro)
        {
            string retorno = "";

            switch (custoFinanceiro)
            {
                case Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA:
                    retorno = "Com Entrada";
                    break;
                case Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA:
                    retorno = "Sem Entrada";
                    break;
                case Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA:
                    retorno = "À Vista";
                    break;
            }

            return retorno;
        }

        public static async Task<string> GerarNsu(ContextoBdGravacao dbgravacao, string id_nsu, ContextoBdProvider contextoProvider)
        {
            string retorno = "";
            int n_nsu = -1;
            string s = "0";
            int asc;
            char chr;

            if (id_nsu == "")
                retorno = "Não foi especificado o NSU a ser gerado!";

            for (int i = 0; i <= 100; i++)
            {
                var ret = from c in dbgravacao.Tcontroles
                          where c.Id_Nsu == id_nsu
                          select c;

                var controle = await ret.FirstOrDefaultAsync();


                if (!string.IsNullOrEmpty(controle.Nsu))
                {
                    if (controle.Seq_Anual != 0)
                    {
                        if (DateTime.Now.Year > controle.Dt_Ult_Atualizacao.Year)
                        {
                            s = Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
                            controle.Dt_Ult_Atualizacao = DateTime.Now;
                            if (!String.IsNullOrEmpty(controle.Ano_Letra_Seq))
                            {
                                asc = int.Parse(controle.Ano_Letra_Seq) + controle.Ano_Letra_Step;
                                chr = (char)asc;
                            }
                        }
                    }
                    n_nsu = int.Parse(controle.Nsu);
                }
                if (n_nsu < 0)
                {
                    retorno = "O NSU gerado é inválido!";
                }
                n_nsu += 1;
                s = Convert.ToString(n_nsu);
                s = Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
                if (s.Length == 12)
                {
                    i = 101;
                    //para salvar o novo numero
                    controle.Nsu = s;
                    if (DateTime.Now > controle.Dt_Ult_Atualizacao)
                        controle.Dt_Ult_Atualizacao = DateTime.Now;

                    retorno = controle.Nsu;

                    try
                    {
                        dbgravacao.Update(controle);
                        await dbgravacao.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        retorno = "Não foi possível gerar o NSU, pois ocorreu o seguinte erro: " + ex.HResult + ":" + ex.Message;
                    }
                }
            }

            return retorno;
        }

        public static async Task<IEnumerable<TcodigoDescricao>> ListarCodigoMarketPlace(ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lstTcodigo = from c in db.TcodigoDescricaos
                             where c.Grupo == InfraBanco.Constantes.Constantes.GRUPO_T_CODIGO_DESCRICAO__PEDIDOECOMMERCE_ORIGEM &&
                                   c.St_Inativo == 0
                             select c;

            return await Task.FromResult(lstTcodigo);
        }

        public static async Task<bool> LojaHabilitadaProdutosECommerce(string loja, ContextoBdProvider contextoProvider)
        {
            bool retorno = false;

            if (loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                retorno = true;
            if (await IsLojaBonshop(loja, contextoProvider))
                retorno = true;
            if (await IsLojaVrf(loja, contextoProvider))
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_MARCELO_ARTVEN)
                retorno = true;

            return retorno;
        }

        
        private static async Task<bool> IsLojaVrf(string loja, ContextoBdProvider contextoProvider)
        {
            bool retorno = false;

            var db = contextoProvider.GetContextoLeitura();

            Tloja tLoja = await (from c in db.Tlojas
                                 where c.Loja == loja &&
                                       c.Unidade_Negocio == Constantes.COD_UNIDADE_NEGOCIO_LOJA__VRF
                                 select c).FirstOrDefaultAsync();

            if (tLoja != null)
                retorno = true;

            return retorno;
        }

        private static async Task<bool> IsLojaBonshop(string loja, ContextoBdProvider contextoProvider)
        {
            bool retorno = false;

            var db = contextoProvider.GetContextoLeitura();

            Tloja tLoja = await (from c in db.Tlojas
                                 where c.Loja == loja &&
                                       c.Unidade_Negocio == Constantes.COD_UNIDADE_NEGOCIO_LOJA__BS
                                 select c).FirstOrDefaultAsync();

            if (tLoja != null)
                retorno = true;

            return retorno;
        }


        public static string DescricaoMultiCDRegraTipoPessoa(string codTipoPessoa)
        {
            string retorno = "";

            codTipoPessoa = codTipoPessoa.ToUpper();

            switch (codTipoPessoa)
            {
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA:
                    retorno = "Pessoa Física";
                    break;
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL:
                    retorno = "Produtor Rural";
                    break;
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE:
                    retorno = "PJ Contribuinte";
                    break;
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE:
                    retorno = "PJ Não Contribuinte";
                    break;
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO:
                    retorno = "PJ Isento";
                    break;
            }

            return retorno;
        }

        public static async Task<string> ObterApelidoEmpresaNfeEmitentes(int id_nfe_emitente, ContextoBdProvider contextoProvider)
        {
            string apelidoEmpresa = "";

            if (id_nfe_emitente == 0)
            {
                apelidoEmpresa = "Cliente";
                return apelidoEmpresa;
            }

            var db = contextoProvider.GetContextoLeitura();

            var apelidoTask = from c in db.TnfEmitentes
                              where c.Id == id_nfe_emitente
                              select c.Apelido;

            apelidoEmpresa = await apelidoTask.FirstOrDefaultAsync();

            return apelidoEmpresa;
        }

        public static async Task<Tparametro> BuscarRegistroParametro(string id, ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var parametroTask = from c in db.Tparametros
                                where c.Id == id
                                select c;

            var parametro = await parametroTask.FirstOrDefaultAsync();

            return parametro;

        }

        public static async Task<bool> IsActivatedFlagPedidoUsarMemorizacaoCompletaEnderecos(ContextoBdProvider contextoProvider)
        {
            Tparametro param = await BuscarRegistroParametro(Constantes.ID_PARAMETRO_Flag_Pedido_MemorizacaoCompletaEnderecos,
                contextoProvider);

            if (param.Campo_inteiro == 1)
                return true;

            return false;
        }

        public static async Task<float> VerificarSemDesagioRA(ContextoBdProvider contextoProvider)
        {//busca o percentual de RA sem desagio ID_PARAM_PERC_LIMITE_RA_SEM_DESAGIO            
            Tparametro tparametro = await BuscarRegistroParametro(
                Constantes.ID_PARAMETRO_PERC_DESAGIO_RA_LIQUIDA, contextoProvider);

            float retorno = 0;

            if (tparametro != null)
            {
                retorno = tparametro.Campo_Real;
            }

            return retorno;
        }

        public static DateTime LimiteDataBuscas()
        {
            DateTime data = new DateTime();
            data = DateTime.Now.AddDays(-60);
            return data;
        }

        public static bool ValidarEmail(string email, List<string> lstErros)
        {
            bool retorno;
            retorno = new EmailAddressAttribute().IsValid(email);

            if (!retorno)
                lstErros.Add("E-mail inválido!");

            return retorno;
        }

        public static async Task<int?> VerificarTelefoneRepetidos(string ddd, string tel, string cpf_cnpj, string tipoCliente,
            ContextoBdProvider contextoProvider, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();

            string listaBranca = "|(11)32683471|";
            string concatenaTel = "|(" + ddd.Trim() + ")" + tel.Trim() + "|";

            if (listaBranca.IndexOf(concatenaTel) != -1) return null;

            string[] cpf_cnpjConsulta = new string[] { ddd, "0" + ddd };
            var lstClienteTask = await (from c in db.Tclientes
                                        where cpf_cnpjConsulta.Contains(c.Ddd_Res) && c.Tel_Res == tel ||
                                              cpf_cnpjConsulta.Contains(c.Ddd_Com) && c.Tel_Com == tel ||
                                              cpf_cnpjConsulta.Contains(c.Ddd_Cel) && c.Tel_Cel == tel ||
                                              cpf_cnpjConsulta.Contains(c.Ddd_Com_2) && c.Tel_Com_2 == tel
                                        select c).ToListAsync();

            var lstOrcamentista = await (from c in db.TorcamentistaEindicadors
                                         where cpf_cnpjConsulta.Contains(c.Ddd) && c.Telefone == tel &&
                                               cpf_cnpjConsulta.Contains(c.Ddd_cel) && c.Tel_cel == tel
                                         select c).ToListAsync();

            int qtdCliente = 0;
            int qtdOrcamentista = 0;
            if (tipoCliente == Constantes.ID_PF)
            {
                qtdCliente = lstClienteTask.Where(x => x.Cnpj_Cpf != cpf_cnpj).Count();
                qtdOrcamentista = lstOrcamentista.Where(x => x.Cnpj_cpf != cpf_cnpj).Count();
            }
            else
            {
                qtdCliente = lstClienteTask
                    .Where(x => x.Cnpj_Cpf.Length == 14 &&
                           x.Cnpj_Cpf.Substring(0, 8) != cpf_cnpj.Substring(0, 8))
                    .Count();
                qtdOrcamentista = lstOrcamentista
                    .Where(x => x.Cnpj_cpf.Length == 14 &&
                           x.Cnpj_cpf.Substring(0, 8) != cpf_cnpj.Substring(0, 8))
                    .Count();
            }

            return qtdCliente + qtdOrcamentista;
        }

        public static string FormatarEndereco(string endereco, string numero, string complemento,
            string bairro, string cidade, string uf, string cep)
        {

            string retorno = "";
            if (!string.IsNullOrEmpty(endereco))
                retorno = endereco.Trim();
            if (!string.IsNullOrEmpty(numero))
                retorno += ", " + numero.Trim();
            if (!string.IsNullOrEmpty(complemento))
                retorno += " " + complemento.Trim();
            if (!string.IsNullOrEmpty(bairro))
                retorno += " - " + bairro.Trim();
            if (!string.IsNullOrEmpty(cidade))
                retorno += " - " + cidade.Trim();
            if (!string.IsNullOrEmpty(uf))
                retorno += " - " + uf.Trim();
            if (!string.IsNullOrEmpty(cep))
                retorno += " - " + FormatarCep(cep.Trim());

            return retorno;
        }

        public static string FormatarCep(string cep)
        {
            string sCep = cep.Replace("-", "");
            if (!Util.VerificaCep(sCep))
                return "";

            cep = sCep.Substring(0, 4) + " - " + sCep.Substring(5, 3);

            return cep;
        }
    }
}
