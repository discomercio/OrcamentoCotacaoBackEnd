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

namespace PrepedidoBusiness.Utils
{
    public class Util
    {
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

            cpf_cnpj = cpf_cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

            if (cpf_cnpj.Length >= 11)
            {
                retorno = true;

                if (cpf_cnpj.Length >= 14)
                {
                    retorno = false;
                }
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

        public static string ObterDescricao_Cod(string grupo, string cod, ContextoBdProvider contextoProvider)
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

        /*afazer: Precisa pedir acesso
         * servidor: Win2008R2BS,29981
         * BD: CEP_Homologacao
         */
        //public static string BuscarCep(string tipo)


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
                //afazer: Passar a quantidade de parcelas
                if (c_custoFinancFornecQtdeParcelas <= 0)
                {
                    lstErros.Add("Não foi informada a quantidade de parcelas para a forma de pagamento selecionada " +
                        "(" + DescricaoCustoFornecTipoParcelamento(custoFinanceiroTipoParcelato) + ")");
                    retorno = false;
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

        public static async Task<string> GerarNsu(ContextoBdGravacao dbgravacao, string id_nsu)
        {
            string retorno = "";
            int n_nsu = -1;
            string s = "0";
            int asc;
            char chr;

            if (id_nsu == "")
                retorno = "Não foi especificado o NSU a ser gerado!!";

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
                                //Precisa revisar essa parte, pois lendo a doc do BD e analisando os dados na base não bate
                                asc = int.Parse(controle.Ano_Letra_Seq) + controle.Ano_Letra_Step;
                                chr = (char)asc;
                            }
                        }
                    }
                    n_nsu = int.Parse(controle.Nsu);
                }
                if (n_nsu < 0)
                {
                    retorno = "O NSU gerado é inválido!!";
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

        public static bool LojaHabilitadaProdutosECommerce(string loja)
        {
            bool retorno = false;

            if (loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_BONSHOP)
                retorno = true;
            if (IsLojaVrf(loja))
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_MARCELO_ARTVEN)
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_BONSHOP_LAB)
                retorno = true;

            return retorno;
        }

        private static bool IsLojaVrf(string loja)
        {
            bool retorno = false;

            if (loja == Constantes.NUMERO_LOJA_VRF ||
                loja == Constantes.NUMERO_LOJA_VRF2 ||
                loja == Constantes.NUMERO_LOJA_VRF3 ||
                loja == Constantes.NUMERO_LOJA_VRF4)
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

            var dbTwmsRegraCdXUfXPessoaXCds = (from c in db.TwmsRegraCdXUfXPessoaXCds
                                               join nfe in db.TnfEmitentes on c.Id_nfe_emitente equals nfe.Id
                                               select c).ToList();

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
                                  regra3 = r3,
                                  regra4 = dbTwmsRegraCdXUfXPessoaXCds.Where(r => r.Id_wms_regra_cd_x_uf_x_pessoa == r3.Id).ToList(),
                              };
            var lista = await testeRegras.ToListAsync();

            RegrasBll itemRegra = new RegrasBll();

            foreach (var item in lstRegrasCrtlEstoque)
            {
                foreach (var r in lista)
                {
                    if (r.prod_x_reg.Produto == item.Produto)
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

                        foreach (var r4 in r.regra4)
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

        #region EstoqueVerificaDisponibilidadeIntegralV2
        //public static bool EstoqueVerificaDisponibilidadeIntegralV2(t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD regra, ContextoProvider contextoProvider)
        //{
        //    bool retorno = false;

        //    var lst1 = BuscarListaQtdeEstoque(contextoProvider);
        //    var lst2 = BuscarListaQtdeEstoqueComSubquery(contextoProvider);

        //    foreach (var p1 in lst1.Result)
        //    {
        //        foreach(var r in regra)
        //        {
        //            if(p1.Id_nfe_emitente != 0)
        //            {

        //            }
        //        }
        //    }

        //    if (regra.Id_nfe_emitente != 0)
        //    {

        //    }

        //    return retorno;
        //}
        #endregion

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

                                if (regra.Produto == p1.Produto)
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
                                if (regra.Produto == p2.Produto)
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

            var lstEstoqueQtdeUtilZero = from c in db.Testoques.Include(r => r.TestoqueItem)
                                         where (c.TestoqueItem.Qtde - c.TestoqueItem.Qtde_utilizada) > 0 &&
                                               c.TestoqueItem.Qtde_utilizada.HasValue
                                         select new ProdutosEstoqueDto
                                         {
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

            var lstEstoqueQtdeUtilZeroComSubQuery = from c in db.Testoques.Include(r => r.TestoqueItem)
                                                    where ((c.TestoqueItem.Qtde - c.TestoqueItem.Qtde_utilizada) > 0) &&
                                                          ((c.TestoqueItem.Qtde_utilizada.HasValue) ||
                                                          (from d in db.TnfEmitentes
                                                           where d.St_Habilitado_Ctrl_Estoque == 1 && d.St_Ativo == 1
                                                           select d.Id)
                                                           .Contains(c.Id_nfe_emitente))
                                                    select new ProdutosEstoqueDto
                                                    {
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
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    foreach (var regra in lstRegras)
                    {
                        if (!string.IsNullOrEmpty(regra.Produto))
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
                if (!string.IsNullOrEmpty(r.Produto))
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
                                            //if (!Util.EstoqueVerificaDisponibilidadeIntegralV2(p, contextoProvider))
                                            //{
                                            //    lstErros.Add("Falha ao tentar consultar disponibilidade no estoque do produto (" +
                                            //        r.Fabricante + ")" + r.Produto);
                                            //}
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

    }
}
