using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;

namespace PrepedidoBusiness.Utils
{
    public class Util
    {
        //private readonly InfraBanco.ContextoProvider contextoProvider;

        //public Util(InfraBanco.ContextoProvider contextoProvider)
        //{
        //    this.contextoProvider = contextoProvider;
        //}

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

        public static bool ValidaCpf_Cnpj(string cpf_cnpj)
        {
            bool retorno = false;
            string formatCpf_Cnpj = "";

            if (cpf_cnpj.Length > 11)
            {
                if (cpf_cnpj.Length > 12)
                {
                    formatCpf_Cnpj = cpf_cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                    retorno = true;
                }
                else
                {
                    formatCpf_Cnpj = cpf_cnpj.Replace(".", "");
                    retorno = true;
                }
            }
            else
            {
                formatCpf_Cnpj = cpf_cnpj.Replace(".", "").Replace("-", "");
                retorno = true;
            }

            return retorno;
        }

        public static string ObterDescricao_Cod(string grupo, string cod, ContextoProvider contextoProvider)
        {
            var db = contextoProvider.GetContexto();

            var desc = from c in db.TcodigoDescricaos
                       where c.Grupo == grupo && c.Codigo == cod
                       select c.Descricao;

            string result = desc.FirstOrDefault();

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

            if (uf.Length == 2)
                if (sigla.Contains(uf))
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

        public static bool GravaLog(string apelido, string loja, string pedido, string id_cliente,
            string operação, string complemento, ContextoProvider contexto)
        {
            if (apelido == null)
                return false;

            var db = contexto.GetContexto();

            Tlog log = new Tlog
            {
                Usuario = apelido,
                Loja = loja,
                Pedido = pedido,
                Id_Cliente = id_cliente,
                Operacao = operação,
                Complemento = complemento
            };

            db.Add(log);
            db.SaveChanges();

            return true;
        }
    }
}
