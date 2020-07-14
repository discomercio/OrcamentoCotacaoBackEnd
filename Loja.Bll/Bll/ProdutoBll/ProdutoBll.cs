using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Loja.Data;
using System.Linq;
using Loja.Modelos;
using Loja.Bll.Dto.ProdutoDto;
using Microsoft.EntityFrameworkCore;
using Loja.Bll.RegrasCtrlEstoque;

namespace Loja.Bll.ProdutoBll
{
    public class ProdutoBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public ProdutoBll(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<ConsultaProdutosDto>> ConsultarListaProdutos(string fabricante)
        {
            //paraTeste
            string loja = "202";

            var db = contextoProvider.GetContextoLeitura();

            int n_fabricante;

            if (!int.TryParse(fabricante, out n_fabricante))
            {
                fabricante = (from c in db.Tfabricantes
                              where c.Nome == fabricante
                              select c.Fabricante).SingleOrDefault();
            }


            var lstProdutosTask = from c in db.TprodutoLojas
                                  join cc in db.Tprodutos
                                  on new { c.Fabricante, c.Produto } equals new { cc.Fabricante, cc.Produto }
                                  where (c.Vendavel == "S" || c.Vendavel == "X") &&
                                        c.Fabricante == fabricante &&
                                        c.Loja == loja
                                  orderby c.Fabricante, cc.Descricao.ToUpper()
                                  select new ConsultaProdutosDto
                                  {
                                      Fabricante = c.Fabricante,
                                      Produto = c.Produto,
                                      Descricao = cc.Descricao,
                                      Preco = c.Preco_Lista,
                                      Cor = c.Cor,
                                      Vendavel = c.Vendavel
                                  };


            List<ConsultaProdutosDto> list = lstProdutosTask.ToList();

            foreach (var i in lstProdutosTask)
            {
                list.Add(new ConsultaProdutosDto
                {

                    Fabricante = i.Fabricante,
                    Produto = i.Produto,
                    Descricao = i.Descricao,
                    Preco = i.Preco, //((decimal)i.Preco).ToString("0.00")
                    Cor = i.Cor,
                    Vendavel = i.Vendavel
                }); ; ;
            }

            return await Task.FromResult(lstProdutosTask);
        }

        public async Task<string> BuscarFabricante(string fabricante)
        {
            var db = contextoProvider.GetContextoLeitura();

            int n_fabricante;
            string nomeFabricante;

            if (!int.TryParse(fabricante, out n_fabricante))
            {
                nomeFabricante = (from c in db.Tfabricantes
                                  where c.Nome == fabricante
                                  select c.Nome).SingleOrDefault();
            }
            else
            {
                nomeFabricante = (from c in db.Tfabricantes
                                  where c.Fabricante == fabricante
                                  select c.Nome).SingleOrDefault();
            }

            return await Task.FromResult(nomeFabricante);
        }

        public async Task<ProdutoComboDto> ListaProdutosCombo(string loja, string id_cliente)
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
            string cliente_regra = Util.Util.MultiCdRegraDeterminaPessoa(cliente.tipo_cliente, cliente.contribuite_icms_status,
                cliente.produtor_rural_status);

            var lstProdutosCompostos = BuscarProdutosCompostos(loja);
            List<ProdutoDto> lstTodosProdutos = (await BuscarTodosProdutos(loja)).ToList();

            List<RegrasBll> lst_cliente_regra = new List<RegrasBll>();
            MontaListaRegras(lstTodosProdutos, lst_cliente_regra);

            List<string> lstErros = new List<string>();
            await Util.Util.ObterCtrlEstoqueProdutoRegra_Teste(lstErros, lst_cliente_regra, cliente.uf, cliente_regra, contextoProvider);

            //afazer: Verificar disponibilidade de estoque
            Util.Util.ObterDisponibilidadeEstoque(lst_cliente_regra, lstTodosProdutos, lstErros, contextoProvider);

            //retorna as qtdes disponiveis
            await Util.Util.VerificarEstoque(lst_cliente_regra, contextoProvider);
            await Util.Util.VerificarEstoqueComSubQuery(lst_cliente_regra, contextoProvider);

            //buscar o parametro produto 001020 indice 12
            Tparametro tparametro = await Util.Util.BuscarRegistroParametro(Constantes.Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal, contextoProvider);
            //atribui a qtde de estoque para o produto
            IncluirEstoqueProduto(lst_cliente_regra, lstTodosProdutos, tparametro);

            //afazer: Msg de alertas para os produtos
            await ExisteMensagensAlertaProdutos(lstTodosProdutos);

            if (lstErros.Count > 0)
            {
                //chama o metodo verificar regras
            }

            retorno.ProdutoCompostoDto = (await lstProdutosCompostos).ToList();
            retorno.ProdutoDto = lstTodosProdutos;
            return retorno;
        }

        public async Task<IEnumerable<ProdutoCompostoDto>> BuscarProdutosCompostos(string loja)
        {

            //loja = "202";
            var db = contextoProvider.GetContextoLeitura();

            //var produtosCompostosTask = from d in (from c in db.Tprodutos
            //                                       join pc in db.TecProdutoCompostos on c.Produto equals pc.Produto_Composto
            //                                       join pci in db.TecProdutoCompostoItems on pc.Fabricante_Composto equals pci.Fabricante_composto
            //                                       join pl in db.TprodutoLojas on pci.Produto_item equals pl.Produto
            //                                       join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
            //                                       where pl.Loja == loja &&
            //                                             pl.Vendavel == "S" &&
            //                                             c.Fabricante == pc.Fabricante_Composto &&
            //                                             pc.Produto_Composto == pci.Produto_composto
            //                                       select new
            //                                       {

            //                                           fabricante_pai = c.Fabricante,
            //                                           fabricante_pai_nome = fab.Nome,
            //                                           produto_pai = c.Produto,
            //                                           valor = (decimal)pl.Preco_Lista,
            //                                           qtde = (int)pci.Qtde,
            //                                           produtosFilhos = new ProdutoFilhoDto
            //                                           {
            //                                               Fabricante = c.Fabricante,
            //                                               Fabricante_Nome = fab.Nome,
            //                                               Produto = pci.Produto_item,
            //                                               Qtde = pci.Qtde
            //                                           }
            //                                       })
            //                            group d by d.produto_pai into g
            //                            select new ProdutoCompostoDto
            //                            {
            //                                PaiFabricante = g.Select(r => r.fabricante_pai).FirstOrDefault(),
            //                                PaiFabricanteNome = g.Select(r => r.fabricante_pai_nome).FirstOrDefault(),
            //                                PaiProduto = g.Select(r => r.produto_pai).FirstOrDefault(),
            //                                Preco_total_Itens = g.Sum(r => r.qtde * r.valor),
            //                                Filhos = g.Select(r => r.produtosFilhos).ToList()
            //                            };
            var produtosCompostosTask = from c in db.Tprodutos
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
                                        };

            var teste = await produtosCompostosTask.ToListAsync();

            var testeTask = from c in teste
                            group c by c.produto_pai into g
                            select new ProdutoCompostoDto
                            {
                                PaiFabricante = g.Select(r => r.fabricante_pai).FirstOrDefault(),
                                PaiFabricanteNome = g.Select(r => r.fabricante_pai_nome).FirstOrDefault(),
                                PaiProduto = g.Select(r => r.produto_pai).FirstOrDefault(),
                                Preco_total_Itens = g.Sum(r => r.qtde * r.valor),
                                Filhos = g.Select(r => r.produtosFilhos).ToList()
                            };

            List<ProdutoCompostoDto> produto = testeTask.ToList();

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

    }
}
