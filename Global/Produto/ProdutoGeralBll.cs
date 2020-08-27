using InfraBanco.Constantes;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfraBanco.Modelos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Produto.RegrasCrtlEstoque;
using Produto.Dados;

namespace Produto
{
    public class ProdutoGeralBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public ProdutoGeralBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<ProdutoComboDados> ListaProdutosComboDados(string loja, string id_cliente, string cpf_cnpj)
        {
            ProdutoComboDados retorno = new ProdutoComboDados();

            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = (from c in db.Tclientes
                               where (c.Id == id_cliente || id_cliente == null) && (c.Cnpj_Cpf == cpf_cnpj || cpf_cnpj == null)
                               select new
                               {
                                   tipo_cliente = c.Tipo,
                                   contribuite_icms_status = c.Contribuinte_Icms_Status,
                                   produtor_rural_status = c.Produtor_Rural_Status,
                                   uf = c.Uf
                               }).FirstOrDefaultAsync();

            var cliente = await clienteTask;
            if (cliente == null)
                return null;

            //obtém  a sigla para regra
            string cliente_regra = UtilsProduto.MultiCdRegraDeterminaPessoa(cliente.tipo_cliente, cliente.contribuite_icms_status,
                cliente.produtor_rural_status);

            var lstProdutosCompostos = BuscarProdutosCompostos(loja);
            List<Produto.Dados.ProdutoDados> lstTodosProdutos = (await BuscarTodosProdutos(loja)).ToList();

            List<RegrasBll> lst_cliente_regra = new List<RegrasBll>();
            MontaListaRegras(lstTodosProdutos, lst_cliente_regra);

            List<string> lstErros = new List<string>();
            await UtilsProduto.ObterCtrlEstoqueProdutoRegra_Teste(lstErros, lst_cliente_regra, cliente.uf, cliente_regra, contextoProvider);

            UtilsProduto.ObterDisponibilidadeEstoque(lst_cliente_regra, lstTodosProdutos, lstErros, contextoProvider);

            //retorna as qtdes disponiveis
            await UtilsProduto.VerificarEstoque(lst_cliente_regra, contextoProvider);
            await UtilsProduto.VerificarEstoqueComSubQuery(lst_cliente_regra, contextoProvider);

            //buscar o parametro produto 001020 indice 12
            Tparametro tparametro = await UtilsProduto.BuscarRegistroParametro(Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal, contextoProvider);
            //atribui a qtde de estoque para o produto
            IncluirEstoqueProduto(lst_cliente_regra, lstTodosProdutos, tparametro);

            await ExisteMensagensAlertaProdutos(lstTodosProdutos);

            retorno.ProdutoCompostoDados = (await lstProdutosCompostos).ToList();
            retorno.ProdutoDados = lstTodosProdutos;
            return retorno;
        }

        private async Task ExisteMensagensAlertaProdutos(List<Produto.Dados.ProdutoDados> lst_produtos)
        {
            var db = contextoProvider.GetContextoLeitura();

            var alertasTask = from c in db.TprodutoXAlertas
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

        private void MontaListaRegras(List<Produto.Dados.ProdutoDados> lst_produtos, List<RegrasBll> lst_cliente_regra)
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

        private async Task<IEnumerable<ProdutoCompostoDados>> BuscarProdutosCompostos(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtosCompostosSemGrupo = await (from d in (from c in db.Tprodutos
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
                                                                  produtosFilhos = new ProdutoFilhoDados
                                                                  {
                                                                      Fabricante = c.Fabricante,
                                                                      Fabricante_Nome = fab.Nome,
                                                                      Produto = pci.Produto_item,
                                                                      Qtde = pci.Qtde
                                                                  }
                                                              })
                                                   select d).ToListAsync();

            var produtosCompostosGrupo = from d in produtosCompostosSemGrupo
                                         group d by d.produto_pai into g
                                         select g;
            var produtosCompostosLista = from g in produtosCompostosGrupo
                                         select new ProdutoCompostoDados
                                         {
                                             PaiFabricante = g.OrderBy(r => r.fabricante_pai).Select(r => r.fabricante_pai).FirstOrDefault(),
                                             PaiFabricanteNome = g.OrderBy(r => r.fabricante_pai_nome).Select(r => r.fabricante_pai_nome).FirstOrDefault(),
                                             PaiProduto = g.OrderBy(r => r.produto_pai).Select(r => r.produto_pai).FirstOrDefault(),
                                             Preco_total_Itens = g.Sum(r => r.qtde * r.valor),
                                             Filhos = g.Select(r => r.produtosFilhos).ToList()
                                         };

            List<ProdutoCompostoDados> produto = produtosCompostosLista.ToList();

            return produto;
        }


        private async Task<IEnumerable<Produto.Dados.ProdutoDados>> BuscarTodosProdutos(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            var todosProdutosTask = from c in db.Tprodutos
                                    join pl in db.TprodutoLojas on c.Produto equals pl.Produto
                                    join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
                                    where pl.Vendavel == "S" &&
                                          pl.Loja == loja
                                    select new Produto.Dados.ProdutoDados
                                    {
                                        Fabricante = c.Fabricante,
                                        Fabricante_Nome = fab.Nome,
                                        Produto = pl.Produto,
                                        Descricao_html = c.Descricao_Html,
                                        Descricao = c.Descricao,
                                        Preco_lista = pl.Preco_Lista,
                                        Qtde_Max_Venda = pl.Qtde_Max_Venda,
                                        Desc_Max = pl.Desc_Max
                                    };

            List<Produto.Dados.ProdutoDados> lstTodosProdutos = await todosProdutosTask.ToListAsync();

            return lstTodosProdutos;
        }

        /*Analisar a necessidade, 
         * pois estamos realizando a busca apenas em produtos que 
         * a subtração entre qtde e qtde_utilizada seja maior que 0
         */
        private void IncluirEstoqueProduto(List<RegrasBll> lstRegras, List<Produto.Dados.ProdutoDados> lst_produtos, Tparametro parametro)
        {
            int qtde_estoque_total_disponivel = 0;
            int qtde_estoque_total_disponivel_global = 0;

            foreach (var p in lst_produtos)
            {
                if (!string.IsNullOrEmpty(p.Produto) && !string.IsNullOrEmpty(p.Fabricante))
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
                            p.Estoque = 0;
                        }
                        else
                        {
                            p.Estoque = qtde_estoque_total_disponivel_global;
                        }
                    }
                    else
                    {
                        p.Estoque = qtde_estoque_total_disponivel;
                    }
                }
                qtde_estoque_total_disponivel_global = 0;
                qtde_estoque_total_disponivel = 0;
            }

        }

        public static void VerificarRegrasAssociadasAosProdutos(List<RegrasBll> lstRegras, List<string> lstErros, string clienteUf, string clienteTipo)
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
                            r.Fabricante + ")" + r.Produto + " está bloqueada para a UF '" + clienteUf + "'");
                    }
                    else if (r.TwmsRegraCdXUfXPessoa != null && r.TwmsRegraCdXUfXPessoa.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " está bloqueada para clientes '" + clienteTipo + "' da UF '" + clienteUf + "'");
                    }
                    else if (r.TwmsRegraCdXUfXPessoa != null && r.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" +
                            clienteTipo + "' da UF '" + clienteUf + "'");
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
                            clienteTipo + "' da UF '" + clienteUf + "'");
                    }
                }
            }
        }
    }
}
