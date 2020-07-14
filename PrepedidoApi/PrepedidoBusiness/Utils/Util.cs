using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using PrepedidoBusiness.Bll.Regras;
using PrepedidoBusiness.Dtos.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dto.Produto;
using System.Globalization;

namespace PrepedidoBusiness.Utils
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

        public static string FormatarTelefones(string telefone)
        {
            return telefone.Insert(telefone.Length - 4, "-");
        }

        public static bool ValidaCPF(string cpf_cnpj)
        {
            bool retorno = false;

            cpf_cnpj = cpf_cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

            if (cpf_cnpj.Length > 11)
            {
                retorno = false;
            }
            if (cpf_cnpj.Length == 11)
            {
                retorno = true;
            }

            return retorno;
        }

        public static bool ValidaCNPJ(string cpf_cnpj)
        {
            bool retorno = false;

            cpf_cnpj = cpf_cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

            if (cpf_cnpj.Length == 14)
            {
                retorno = true;
            }
            if (cpf_cnpj.Length < 11)
            {
                retorno = false;
            }

            return retorno;
        }

        public static string SoDigitosCpf_Cnpj(string cpf_cnpj)
        {
            string retorno = "";

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

        public static string OpcaoFormaPagto(string codigo)
        {
            string retorno = "";

            switch (codigo)
            {
                case Constantes.ID_FORMA_PAGTO_DINHEIRO:
                    retorno = "Dinheiro";
                    break;
                case Constantes.ID_FORMA_PAGTO_DEPOSITO:
                    retorno = "Depósito";
                    break;
                case Constantes.ID_FORMA_PAGTO_CHEQUE:
                    retorno = "Cheque";
                    break;
                case Constantes.ID_FORMA_PAGTO_BOLETO:
                    retorno = "Boleto";
                    break;
                case Constantes.ID_FORMA_PAGTO_CARTAO:
                    retorno = "Cartão (internet)";
                    break;
                case Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA:
                    retorno = "Cartão (maquineta)";
                    break;
                case Constantes.ID_FORMA_PAGTO_BOLETO_AV:
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
            bool retorno = false;
            string cepFormat = "";

            if (cep != "")
            {
                cepFormat = cep.Replace("-", "");
                if (cepFormat.Length == 8)
                    retorno = true;
            }

            return retorno;
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
                        if (campo_atual.IndexOf("Endereço entrega=mesmo do cadastro") >= 0 && !endEntregaMesmo)
                        {
                            endEntregaMesmo = true;
                            log = log + campo_atual + "; ";
                        }

                        if (campo_atual == coluna)
                        {
                            //pegando o valor coluna
                            var value = (c.GetValue(obj, null));
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
                            s = Utils.Util.Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
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
                s = Utils.Util.Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
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

        //afazer: alterar o método para buscar no banco de dados e comparar, nã será mais feito a verificação nas constantes
        //verificar melhor como sera feito a comparação pois a variavel "loja" é um número e o retorno do banco 
        //será uma sigla Ex: "AC / BS / VRF"
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

        public static string MultiCdRegraDeterminaPessoa(string tipoCliente, byte contribuinteIcmsStatus, byte produtoRuralStatus)
        {
            string tipoPessoa = "";

            if (tipoCliente == Constantes.ID_PF)
            {
                if (produtoRuralStatus == byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL;
                else if (produtoRuralStatus == byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA;
            }
            else if (tipoCliente == Constantes.ID_PJ)
            {
                if (contribuinteIcmsStatus == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE;
                else if (contribuinteIcmsStatus == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE;
                else if (contribuinteIcmsStatus == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO;
            }

            return tipoPessoa;
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

        public static async Task ObterCtrlEstoqueProdutoRegra_Teste(List<string> lstErros,
            List<RegrasBll> lstRegrasCrtlEstoque, string uf, string cliente_regra, ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var dbTwmsRegraCdXUfXPessoaXCds = from c in db.TwmsRegraCdXUfXPessoaXCds
                                              join nfe in db.TnfEmitentes on c.Id_nfe_emitente equals nfe.Id
                                              select c;
            List<TwmsRegraCdXUfXPessoaXCd> lstRegra = await dbTwmsRegraCdXUfXPessoaXCds.ToListAsync();

            //essa query esta copiando o id do produto 
            var testeRegras = from c in db.TprodutoXwmsRegraCds
                              join r1 in db.TwmsRegraCds on c.Id_wms_regra_cd equals r1.Id
                              join r2 in db.TwmsRegraCdXUfs on r1.Id equals r2.Id_wms_regra_cd
                              join r3 in db.TwmsRegraCdXUfPessoas on r2.Id equals r3.Id_wms_regra_cd_x_uf
                              where r2.Uf == uf &&
                                    r3.Tipo_pessoa == cliente_regra
                              orderby c.Produto
                              select new
                              {
                                  prod_x_reg = c,
                                  regra1 = r1,
                                  regra2 = r2,
                                  regra3 = r3
                              };
            var lista = await testeRegras.ToListAsync();

            RegrasBll itemRegra = new RegrasBll();

            foreach (var item in lstRegrasCrtlEstoque)
            {
                foreach (var r in lista)
                {
                    if (r.prod_x_reg.Produto == item.Produto && 
                        r.prod_x_reg.Fabricante == item.Fabricante)
                    {
                        item.St_Regra = true;
                        item.TwmsRegraCd = new t_WMS_REGRA_CD
                        {
                            Id = r.regra1.Id,
                            Apelido = r.regra1.Apelido,
                            Descricao = r.regra1.Descricao,
                            St_inativo = r.regra1.St_inativo

                        };
                        item.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
                        {
                            Id = r.regra2.Id,
                            Id_wms_regra_cd = r.regra2.Id_wms_regra_cd,
                            Uf = r.regra2.Uf,
                            St_inativo = r.regra2.St_inativo
                        };
                        item.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
                        {
                            Id = r.regra3.Id,
                            Id_wms_regra_cd_x_uf = r.regra3.Id_wms_regra_cd_x_uf,
                            Tipo_pessoa = r.regra3.Tipo_pessoa,
                            St_inativo = r.regra3.St_inativo,
                            Spe_id_nfe_emitente = r.regra3.Spe_id_nfe_emitente
                        };
                        item.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();

                        foreach (var r4 in lstRegra)
                        {
                            if (r4.Id_wms_regra_cd_x_uf_x_pessoa == r.regra3.Id)
                            {
                                t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
                                {
                                    Id = r4.Id,
                                    Id_wms_regra_cd_x_uf_x_pessoa = r4.Id_wms_regra_cd_x_uf_x_pessoa,
                                    Id_nfe_emitente = r4.Id_nfe_emitente,
                                    Ordem_prioridade = r4.Ordem_prioridade,
                                    St_inativo = r4.St_inativo
                                };
                                item.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
                            }
                        }
                    }
                }
            }
        }

        public static async Task VerificarEstoque(List<RegrasBll> lst_cliente_regra, ContextoBdProvider contextoProvider)
        {
            var lst1 = await BuscarListaQtdeEstoque(contextoProvider);


            foreach (var regra in lst_cliente_regra)
            {
                if (regra.TwmsRegraCd != null)
                {
                    foreach (var p1 in lst1)
                    {
                        foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                        {
                            if (r.Id_nfe_emitente != 0)
                            {

                                if (regra.Produto == p1.Produto && regra.Fabricante == p1.Fabricante)
                                {
                                    //verificar se é inativo
                                    if (r.St_inativo == 0)
                                    {
                                        //valor subtraido
                                        r.Estoque_Qtde += (short)(p1.Qtde - p1.Qtde_Utilizada);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public static async Task VerificarEstoqueComSubQuery(List<RegrasBll> lst_cliente_regra, ContextoBdProvider contextoProvider)
        {
            var lst2 = await BuscarListaQtdeEstoqueComSubquery(contextoProvider);

            foreach (var regra in lst_cliente_regra)
            {
                if (regra.TwmsRegraCd != null)
                {
                    foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                    {
                        if (r.Id_nfe_emitente != 0)
                        {
                            foreach (var p2 in lst2)
                            {
                                if (regra.Produto == p2.Produto && regra.Fabricante == p2.Fabricante)
                                {
                                    //valor subtraido
                                    if (!r.Estoque_Qtde_Estoque_Global.HasValue)
                                        r.Estoque_Qtde_Estoque_Global = 0;
                                    r.Estoque_Qtde_Estoque_Global += (short)(p2.Qtde - p2.Qtde_Utilizada);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static async Task<IEnumerable<ProdutosEstoqueDto>> BuscarListaQtdeEstoque(ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lstEstoqueQtdeUtilZero = from c in db.Testoques
                                         where (c.TestoqueItem.Qtde - c.TestoqueItem.Qtde_utilizada) > 0 &&
                                               c.TestoqueItem.Qtde_utilizada.HasValue
                                         select new ProdutosEstoqueDto
                                         {
                                             Fabricante = c.TestoqueItem.Fabricante,
                                             Produto = c.TestoqueItem.Produto,
                                             Qtde = (int)c.TestoqueItem.Qtde,
                                             Qtde_Utilizada = (int)c.TestoqueItem.Qtde_utilizada,
                                             Id_nfe_emitente = c.Id_nfe_emitente
                                         };

            List<ProdutosEstoqueDto> produtosEstoqueDtos = await lstEstoqueQtdeUtilZero.ToListAsync();

            return produtosEstoqueDtos;
        }

        private static async Task<IEnumerable<ProdutosEstoqueDto>> BuscarListaQtdeEstoqueComSubquery(ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lstEstoqueQtdeUtilZeroComSubQuery = from c in db.Testoques
                                                    where ((c.TestoqueItem.Qtde - c.TestoqueItem.Qtde_utilizada) > 0) &&
                                                          ((c.TestoqueItem.Qtde_utilizada.HasValue) ||
                                                          (from d in db.TnfEmitentes
                                                           where d.St_Habilitado_Ctrl_Estoque == 1 && d.St_Ativo == 1
                                                           select d.Id)
                                                           .Contains(c.Id_nfe_emitente))
                                                    select new ProdutosEstoqueDto
                                                    {
                                                        Fabricante = c.TestoqueItem.Fabricante,
                                                        Produto = c.TestoqueItem.Produto,
                                                        Qtde = (int)c.TestoqueItem.Qtde,
                                                        Qtde_Utilizada = (int)c.TestoqueItem.Qtde_utilizada,
                                                        Id_nfe_emitente = c.Id_nfe_emitente
                                                    };

            List<ProdutosEstoqueDto> produtosEstoqueDtos = await lstEstoqueQtdeUtilZeroComSubQuery.ToListAsync();

            return produtosEstoqueDtos;
        }

        public static bool VerificarEstoqueInsuficiente(List<RegrasBll> lstRegras, PrePedidoDto prepedido, Tparametro parametro)
        {
            bool retorno = false;
            int qtde_estoque_total_disponivel = 0;
            int qtde_estoque_total_disponivel_global = 0;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto) && !string.IsNullOrEmpty(p.Fabricante))
                {
                    foreach (var regra in lstRegras)
                    {
                        if (!string.IsNullOrEmpty(regra.Produto) && !string.IsNullOrEmpty(regra.Fabricante))
                        {
                            foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (r.Id_nfe_emitente > 0)
                                {
                                    if (r.St_inativo == 0)
                                    {
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.NumProduto)
                                        {
                                            qtde_estoque_total_disponivel += r.Estoque_Qtde;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (parametro.Campo_inteiro == 1)
                    {
                        if (qtde_estoque_total_disponivel_global == 0)
                        {
                            p.Qtde_estoque_total_disponivel = 0;

                        }
                    }
                    else
                    {
                        p.Qtde_estoque_total_disponivel = (short?)qtde_estoque_total_disponivel;
                    }
                    if (p.Qtde > p.Qtde_estoque_total_disponivel)
                        retorno = true;
                }
            }
            return retorno;

        }

        public static void ObterDisponibilidadeEstoque(List<RegrasBll> lstRegrasCrtlEstoque, List<ProdutoDto> lst_produtos,
            List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            foreach (var r in lstRegrasCrtlEstoque)
            {
                if (!string.IsNullOrEmpty(r.Produto) && !string.IsNullOrEmpty(r.Fabricante))
                {
                    if (r.TwmsRegraCd != null)
                    {
                        foreach (var p in r.TwmsCdXUfXPessoaXCd)
                        {
                            if (p.Id_nfe_emitente > 0)
                            {
                                if (p.St_inativo == 0)
                                {
                                    foreach (var produto in lst_produtos)
                                    {
                                        if (r.Fabricante == produto.Fabricante && r.Produto == produto.Produto)
                                        {
                                            p.Estoque_Fabricante = produto.Fabricante;
                                            p.Estoque_Produto = produto.Produto;
                                            p.Estoque_DescricaoHtml = produto.Descricao_html;
                                            //p.Estoque_Qtde_Solicitado = essa variavel não deve ser utilizada, a qtde só sera solicitada 
                                            //quando o usuario inserir a qtde 
                                            p.Estoque_Qtde = 0;
                                            
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
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

    }
}
