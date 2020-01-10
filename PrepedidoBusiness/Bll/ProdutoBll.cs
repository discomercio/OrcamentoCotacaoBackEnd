using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Produto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfraBanco.Modelos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PrepedidoBusiness.Utils;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Bll.Regras;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using PrepedidoBusiness.Dto.Prepedido;

namespace PrepedidoBusiness.Bll
{
    public class ProdutoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public ProdutoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }
        #region
        //public async Task<IEnumerable<ProdutoDto>> BuscarProduto(string codProduto, string loja, string apelido, List<string> lstErros)
        //{
        //    //paraTeste
        //    //apelido = "MARISARJ";
        //    //codProduto = "003000";
        //    //loja = "202";
        //    int qtde = 0;
        //    bool lojaHabilitada = false;
        //    decimal vlProdCompostoPrecoListaLoja = 0;
        //    ProdutoDto produtoDto = new ProdutoDto();

        //    List<ProdutoDto> lstProduto = new List<ProdutoDto>();

        //    var db = contextoProvider.GetContextoLeitura();

        //    if (string.IsNullOrEmpty(codProduto))
        //        return null;
        //    else if (string.IsNullOrEmpty(loja))
        //        return null;

        //    if (Util.LojaHabilitadaProdutosECommerce(loja))
        //    {
        //        var prodCompostoTask = from c in db.TecProdutoCompostos
        //                               where c.Produto_Composto == codProduto
        //                               select c;

        //        string parada = "";

        //        var prodComposto = prodCompostoTask.FirstOrDefault();

        //        if (prodComposto.Produto_Composto != null)
        //        {
        //            var prodCompostoItensTask = from c in db.TecProdutoCompostoItems
        //                                        where c.Fabricante_composto == prodComposto.Fabricante_Composto &&
        //                                              c.Produto_composto == prodComposto.Produto_Composto &&
        //                                              c.Excluido_status == 0
        //                                        orderby c.Sequencia
        //                                        select c;
        //            var prodCompostoItens = prodCompostoItensTask.ToList();

        //            if (prodCompostoItens.Count > 0)
        //            {
        //                foreach (var pi in prodCompostoItens)
        //                {
        //                    var produtoTask = from c in db.Tprodutos.Include(r => r.TprodutoLoja)
        //                                      where c.TprodutoLoja.Fabricante == pi.Fabricante_item &&
        //                                            c.TprodutoLoja.Produto == pi.Produto_item &&
        //                                            c.TprodutoLoja.Loja == loja
        //                                      select c;

        //                    var produto = await produtoTask.FirstOrDefaultAsync();

        //                    if (string.IsNullOrEmpty(produto.Produto))
        //                        lstErros.Add("O produto(" + pi.Fabricante_item + ")" + pi.Produto_item + " não está disponível para a loja " + loja + "!!");
        //                    else
        //                    {
        //                        produtoDto = new ProdutoDto
        //                        {
        //                            Fabricante = pi.Fabricante_item,
        //                            Produto = pi.Produto_item,
        //                            Qtde = pi.Qtde,
        //                            ValorLista = produto.TprodutoLoja.Preco_Lista,
        //                            Descricao = produto.Descricao
        //                        };
        //                    }
        //                }
        //            }

        //        }
        //        else
        //        {
        //            var produtoTask = from c in db.Tprodutos.Include(r => r.TprodutoLoja)
        //                              where c.TprodutoLoja.Produto == codProduto &&
        //                                    c.TprodutoLoja.Loja == loja
        //                              select c;

        //            var produto = await produtoTask.FirstOrDefaultAsync();

        //            if (string.IsNullOrEmpty(produto.Produto))
        //                lstErros.Add("Produto '" + codProduto + "' não foi encontrado para a loja " + loja + "!!");
        //            else
        //            {
        //                produtoDto = new ProdutoDto
        //                {
        //                    Fabricante = produto.Fabricante,
        //                    Produto = produto.Produto,
        //                    Qtde = qtde,
        //                    ValorLista = produto.TprodutoLoja.Preco_Lista,
        //                    Descricao = produto.Descricao
        //                };
        //            }
        //        }
        //    }



        //    return lstProduto;
        //}

        //public async Task<ProdutoDto> ListarProdutosCombo(string loja)
        //{
        //    //Necessario buscar os dados do Orcamentista para verificação abaixo
        //    if (Util.LojaHabilitadaProdutosECommerce(loja))
        //    {

        //    }
        //}
        #endregion

        public async Task<ProdutoComboDto> ListaProdutosCombo(string apelido, string loja, string id_cliente)
        {
            ProdutoComboDto retorno = new ProdutoComboDto();
            //string loja = "202";
            //string id_cliente = "000000605954";

            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = (from c in db.Tclientes
                               where c.Id == id_cliente
                               select new
                               {
                                   tipo_cliente = c.Tipo,
                                   contribuite_icms_status = c.Contribuinte_Icms_Status,
                                   produtor_rural_status = c.Produtor_Rural_Status,
                                   uf = c.Uf
                               }).FirstOrDefaultAsync();

            var cliente = await clienteTask;

            ////obtém  a sigla para regra
            string cliente_regra = Util.MultiCdRegraDeterminaPessoa(cliente.tipo_cliente, cliente.contribuite_icms_status,
                cliente.produtor_rural_status);

            var lstProdutosCompostos = BuscarProdutosCompostos(loja);
            List<ProdutoDto> lstTodosProdutos = (await BuscarTodosProdutos(loja)).ToList();

            List<RegrasBll> lst_cliente_regra = new List<RegrasBll>();
            MontaListaRegras(lstTodosProdutos, lst_cliente_regra);

            List<string> lstErros = new List<string>();
            await Util.ObterCtrlEstoqueProdutoRegra_Teste(lstErros, lst_cliente_regra, cliente.uf, cliente_regra, contextoProvider);

            
            Util.ObterDisponibilidadeEstoque(lst_cliente_regra, lstTodosProdutos, lstErros, contextoProvider);

            //retorna as qtdes disponiveis
            await Util.VerificarEstoque(lst_cliente_regra, contextoProvider);
            await Util.VerificarEstoqueComSubQuery(lst_cliente_regra, contextoProvider);

            //buscar o parametro produto 001020 indice 12
            Tparametro tparametro = await Util.BuscarRegistroParametro(Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal, contextoProvider);
            //atribui a qtde de estoque para o produto
            IncluirEstoqueProduto(lst_cliente_regra, lstTodosProdutos, tparametro);

            
            await ExisteMensagensAlertaProdutos(lstTodosProdutos);

            if (lstErros.Count > 0)
            {
                //chama o metodo verificar regras
            }

            retorno.ProdutoCompostoDto = (await lstProdutosCompostos).ToList();
            retorno.ProdutoDto = lstTodosProdutos;
            return retorno;
        }

        private async Task ExisteMensagensAlertaProdutos(List<ProdutoDto> lst_produtos)
        {
            var db = contextoProvider.GetContextoLeitura();


            var alertasTask = from c in db.TprodutoXAlertas.Include(r => r.TalertaProduto).Include(r => r.Tproduto)
                              where c.TalertaProduto.Ativo == "S"
                              orderby c.Dt_Cadastro, c.Id_Alerta
                              select new
                              {
                                  alerta_fabricante = c.Fabricante,
                                  alerta_produto = c.Produto,
                                  alerta_mensagem = c.TalertaProduto.Mensagem,
                                  alerta_descricao = c.Tproduto.Descricao
                              };

            var alertas = await alertasTask.ToListAsync();

            foreach (var p in lst_produtos)
            {
                foreach (var m in alertas)
                {
                    if (string.IsNullOrEmpty(m.alerta_fabricante))
                    {
                        if (m.alerta_fabricante == p.Fabricante && m.alerta_produto == p.Produto)
                        {
                            p.Alertas = m.alerta_mensagem;
                        }
                    }
                }
            }
        }

        public void MontaListaRegras(List<ProdutoDto> lst_produtos, List<RegrasBll> lst_cliente_regra)
        {
            foreach (var p in lst_produtos)
            {
                lst_cliente_regra.Add(new RegrasBll
                {
                    Fabricante = p.Fabricante,
                    Produto = p.Produto
                });
            }
        }

        public async Task<IEnumerable<ProdutoCompostoDto>> BuscarProdutosCompostos(string loja)
        {

            //loja = "202";
            var db = contextoProvider.GetContextoLeitura();

            var produtosCompostosTask = from d in (from c in db.Tprodutos
                                                   join pc in db.TecProdutoCompostos on c.Produto equals pc.Produto_Composto
                                                   join pci in db.TecProdutoCompostoItems on pc.Fabricante_Composto equals pci.Fabricante_composto
                                                   join pl in db.TprodutoLojas on pci.Produto_item equals pl.Produto
                                                   join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
                                                   where pl.Loja == loja &&
                                                         pl.Vendavel == "S" &&
                                                         c.Fabricante == pc.Fabricante_Composto &&
                                                         pc.Produto_Composto == pci.Produto_composto
                                                   select new
                                                   {

                                                       fabricante_pai = c.Fabricante,
                                                       fabricante_pai_nome = fab.Nome,
                                                       produto_pai = c.Produto,
                                                       valor = (decimal)pl.Preco_Lista,
                                                       qtde = (int)pci.Qtde,
                                                       produtosFilhos = new ProdutoFilhoDto
                                                       {
                                                           Fabricante = c.Fabricante,
                                                           Fabricante_Nome = fab.Nome,
                                                           Produto = pci.Produto_item,
                                                           Qtde = pci.Qtde
                                                       }
                                                   })
                                        group d by d.produto_pai into g
                                        select new ProdutoCompostoDto
                                        {
                                            PaiFabricante = g.Select(r => r.fabricante_pai).FirstOrDefault(),
                                            PaiFabricanteNome = g.Select(r => r.fabricante_pai_nome).FirstOrDefault(),
                                            PaiProduto = g.Select(r => r.produto_pai).FirstOrDefault(),
                                            Preco_total_Itens = g.Sum(r => r.qtde * r.valor),
                                            Filhos = g.Select(r => r.produtosFilhos).ToList()
                                        };

            List<ProdutoCompostoDto> produto = await produtosCompostosTask.ToListAsync();

            return produto;
        }

        public async Task<IEnumerable<ProdutoDto>> BuscarTodosProdutos(string loja)
        {
            //loja = "202";

            var db = contextoProvider.GetContextoLeitura();

            var todosProdutosTask = from c in db.Tprodutos
                                    join pl in db.TprodutoLojas on c.Produto equals pl.Produto
                                    join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
                                    where pl.Vendavel == "S" &&
                                          pl.Loja == loja
                                    select new ProdutoDto
                                    {
                                        Fabricante = c.Fabricante,
                                        Fabricante_Nome = fab.Nome,
                                        Produto = pl.Produto,
                                        Descricao_html = c.Descricao_Html,
                                        Preco_lista = pl.Preco_Lista,
                                        Qtde_Max_Venda = pl.Qtde_Max_Venda
                                    };

            List<ProdutoDto> lstTodosProdutos = await todosProdutosTask.ToListAsync();

            return lstTodosProdutos;
        }

        /*Analisar a necessidade, 
         * pois estamos realizando a busca apenas em produtos que 
         * a subtração entre qtde e qtde_utilizada seja maior que 0
         */
        private void IncluirEstoqueProduto(List<RegrasBll> lstRegras, List<ProdutoDto> lst_produtos, Tparametro parametro)
        {
            int qtde_estoque_total_disponivel = 0;
            int qtde_estoque_total_disponivel_global = 0;

            foreach (var p in lst_produtos)
            {
                if (!string.IsNullOrEmpty(p.Produto))
                {
                    foreach (var regra in lstRegras)
                    {

                        if (regra.TwmsRegraCd != null)
                        {
                            foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (r.Id_nfe_emitente > 0)
                                {
                                    if (r.St_inativo == 0)
                                    {
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.Produto)
                                        {
                                            qtde_estoque_total_disponivel += r.Estoque_Qtde;
                                            if (qtde_estoque_total_disponivel_global == 0)
                                            {
                                                if (r.Estoque_Qtde_Estoque_Global != null)
                                                    qtde_estoque_total_disponivel_global = (int)r.Estoque_Qtde_Estoque_Global;
                                            }
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
                            //p.Qtde_estoque_total_disponivel = 0;
                            p.Estoque = 0;
                        }
                        else
                        {
                            p.Estoque = qtde_estoque_total_disponivel_global;
                        }
                    }
                    else
                    {
                        //p.Qtde_estoque_total_disponivel = (short?)qtde_estoque_total_disponivel;
                        p.Estoque = qtde_estoque_total_disponivel;
                    }
                }
                qtde_estoque_total_disponivel_global = 0;
                qtde_estoque_total_disponivel = 0;
            }

        }

        //public async Task ObterCtrlEstoqueProdutoRegra_Teste(List<string> lstErros,
        //    List<RegrasBll> lstRegrasCrtlEstoque, string uf, string cliente_regra)
        //{
        //    //o cliente esta sendo passado como dynamic
        //    //string uf = cliente.uf;
        //    //string tipo = cliente.tipo_cliente;


        //    var db = contextoProvider.GetContextoLeitura();

        //    var dbTwmsRegraCdXUfXPessoaXCds = (from c in db.TwmsRegraCdXUfXPessoaXCds
        //                                       join nfe in db.TnfEmitentes on c.Id_nfe_emitente equals nfe.Id
        //                                       select c).ToList();

        //    //essa query esta copiando o id do produto 
        //    var testeRegras = from c in db.TprodutoXwmsRegraCds
        //                      join r1 in db.TwmsRegraCds on c.Id_wms_regra_cd equals r1.Id
        //                      join r2 in db.TwmsRegraCdXUfs on r1.Id equals r2.Id_wms_regra_cd
        //                      join r3 in db.TwmsRegraCdXUfPessoas on r2.Id equals r3.Id_wms_regra_cd_x_uf
        //                      where r2.Uf == uf &&
        //                            r3.Tipo_pessoa == cliente_regra
        //                      orderby c.Produto
        //                      select new
        //                      {
        //                          prod_x_reg = c,
        //                          regra1 = r1,
        //                          regra2 = r2,
        //                          regra3 = r3,
        //                          regra4 = dbTwmsRegraCdXUfXPessoaXCds.Where(r => r.Id_wms_regra_cd_x_uf_x_pessoa == r3.Id).ToList(),
        //                      };
        //    var lista = await testeRegras.ToListAsync();

        //    RegrasBll itemRegra = new RegrasBll();

        //    foreach (var item in lstRegrasCrtlEstoque)
        //    {
        //        foreach (var r in lista)
        //        {
        //            if (r.prod_x_reg.Produto == item.Produto)
        //            {
        //                item.St_Regra = true;
        //                item.TwmsRegraCd = new t_WMS_REGRA_CD
        //                {
        //                    Id = r.regra1.Id,
        //                    Apelido = r.regra1.Apelido,
        //                    Descricao = r.regra1.Descricao,
        //                    St_inativo = r.regra1.St_inativo

        //                };
        //                item.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
        //                {
        //                    Id = r.regra2.Id,
        //                    Id_wms_regra_cd = r.regra2.Id_wms_regra_cd,
        //                    Uf = r.regra2.Uf,
        //                    St_inativo = r.regra2.St_inativo
        //                };
        //                item.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
        //                {
        //                    Id = r.regra3.Id,
        //                    Id_wms_regra_cd_x_uf = r.regra3.Id_wms_regra_cd_x_uf,
        //                    Tipo_pessoa = r.regra3.Tipo_pessoa,
        //                    St_inativo = r.regra3.St_inativo,
        //                    Spe_id_nfe_emitente = r.regra3.Spe_id_nfe_emitente
        //                };
        //                item.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();

        //                foreach (var r4 in r.regra4)
        //                {
        //                    t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
        //                    {
        //                        Id = r4.Id,
        //                        Id_wms_regra_cd_x_uf_x_pessoa = r4.Id_wms_regra_cd_x_uf_x_pessoa,
        //                        Id_nfe_emitente = r4.Id_nfe_emitente,
        //                        Ordem_prioridade = r4.Ordem_prioridade,
        //                        St_inativo = r4.St_inativo
        //                    };
        //                    item.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
        //                }


        //            }
        //        }
        //    }

        //    //return lstRegrasCrtlEstoque;
        //}


        public static void VerificarRegrasAssociadasAosProdutos(List<RegrasBll> lstRegras, List<string> lstErros, DadosClienteCadastroDto cliente)
        {
            foreach (var r in lstRegras)
            {
                if (!string.IsNullOrEmpty(r.Produto))
                {
                    if (r.TwmsRegraCd.Id == 0)
                        lstErros.Add("Produto (" + r.Fabricante + ")" + r.Produto + " não possui regra de consumo do estoque associada");
                    else if (r.TwmsRegraCd.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto(" +
                            r.Fabricante + ")" + r.Produto + " está desativada");
                    }
                    else if (r.TwmsRegraCdXUf.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " está bloqueada para a UF '" + cliente.Uf + "'");
                    }
                    else if (r.TwmsRegraCdXUfXPessoa != null && r.TwmsRegraCdXUfXPessoa.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " está bloqueada para clientes '" + cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    else if (r.TwmsRegraCdXUfXPessoa != null && r.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    int verificaErros = 0;
                    if (r.TwmsCdXUfXPessoaXCd != null)
                    {
                        foreach (var re in r.TwmsCdXUfXPessoaXCd)
                        {
                            if (re.Id_nfe_emitente > 0)
                            {
                                if (re.St_inativo == 0)
                                    verificaErros++;
                            }
                        }
                    }
                    if (verificaErros == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " não especifica nenhum CD ativo para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                }
            }
        }

    }
}
