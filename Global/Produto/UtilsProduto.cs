using InfraBanco;
using InfraBanco.Modelos;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using Produto.RegrasCrtlEstoque;
using Produto.Dados;
using UtilsGlobais;

namespace Produto
{
    public class UtilsProduto
    {
        public static async Task<Tparametro> BuscarRegistroParametro(string id, ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var parametroTask = from c in db.Tparametros
                                where c.Id == id
                                select c;

            var parametro = await parametroTask.FirstOrDefaultAsync();

            return parametro;
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

        public static string MultiCdRegraDeterminaPessoa(string tipoCliente, byte contribuinteIcmsStatus, byte produtoRuralStatus)
        {
            string tipoPessoa = "";

            if (tipoCliente == Constantes.ID_PF)
            {
                if (produtoRuralStatus == (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL;
                else if (produtoRuralStatus == (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA;
            }
            else if (tipoCliente == Constantes.ID_PJ)
            {
                if (contribuinteIcmsStatus == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE;
                else if (contribuinteIcmsStatus == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE;
                else if (contribuinteIcmsStatus == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO;
            }

            return tipoPessoa;
        }

        public static void ObterDisponibilidadeEstoque(List<RegrasBll> lstRegrasCrtlEstoque, List<Produto.Dados.ProdutoDados> lst_produtos,
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

        private static async Task<IEnumerable<ProdutosEstoqueDados>> BuscarListaQtdeEstoque(ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lstEstoqueQtdeUtilZero = from c in db.Testoques
                                         where (c.TestoqueItem.Qtde - c.TestoqueItem.Qtde_utilizada) > 0 &&
                                               c.TestoqueItem.Qtde_utilizada.HasValue
                                         select new ProdutosEstoqueDados
                                         {
                                             Fabricante = c.TestoqueItem.Fabricante,
                                             Produto = c.TestoqueItem.Produto,
                                             Qtde = (int)c.TestoqueItem.Qtde,
                                             Qtde_Utilizada = (int)c.TestoqueItem.Qtde_utilizada,
                                             Id_nfe_emitente = c.Id_nfe_emitente
                                         };

            List<ProdutosEstoqueDados> produtosEstoqueDtos = await lstEstoqueQtdeUtilZero.ToListAsync();

            return produtosEstoqueDtos;
        }

        private static async Task<IEnumerable<ProdutosEstoqueDados>> BuscarListaQtdeEstoqueComSubquery(ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lstEstoqueQtdeUtilZeroComSubQuery = from c in db.Testoques
                                                    where ((c.TestoqueItem.Qtde - c.TestoqueItem.Qtde_utilizada) > 0) &&
                                                          ((c.TestoqueItem.Qtde_utilizada.HasValue) ||
                                                          (from d in db.TnfEmitentes
                                                           where d.St_Habilitado_Ctrl_Estoque == 1 && d.St_Ativo == 1
                                                           select d.Id)
                                                           .Contains(c.Id_nfe_emitente))
                                                    select new ProdutosEstoqueDados
                                                    {
                                                        Fabricante = c.TestoqueItem.Fabricante,
                                                        Produto = c.TestoqueItem.Produto,
                                                        Qtde = (int)c.TestoqueItem.Qtde,
                                                        Qtde_Utilizada = (int)c.TestoqueItem.Qtde_utilizada,
                                                        Id_nfe_emitente = c.Id_nfe_emitente
                                                    };

            List<ProdutosEstoqueDados> produtosEstoqueDtos = await lstEstoqueQtdeUtilZeroComSubQuery.ToListAsync();

            return produtosEstoqueDtos;
        }

    }
}