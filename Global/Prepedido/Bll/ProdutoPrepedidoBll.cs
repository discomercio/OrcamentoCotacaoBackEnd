using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using Prepedido.Dto;
using Produto;
using Produto.Dados;
using System.Linq;
using System.Threading.Tasks;

namespace Prepedido.Bll
{
    public class ProdutoPrepedidoBll
    {
        private readonly ProdutoGeralBll produtoGeralBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public ProdutoPrepedidoBll(
            Produto.ProdutoGeralBll produtoGeralBll,
            InfraBanco.ContextoBdProvider contextoProvider
            )
        {
            this.produtoGeralBll = produtoGeralBll;
            this.contextoProvider = contextoProvider;
        }

        public async Task<ProdutoComboDto> ListaProdutosComboApiArclube(string loja, string id_cliente)
        {
            ProdutoComboDados aux;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                //Buscar dados do cliente
                var clienteTask = await (from c in db.Tcliente
                                         where (c.Id == id_cliente || id_cliente == null)
                                         select new
                                         {
                                             tipo_cliente = c.Tipo,
                                             contribuite_icms_status = c.Contribuinte_Icms_Status,
                                             produtor_rural_status = c.Produtor_Rural_Status,
                                             uf = c.Uf
                                         }).FirstOrDefaultAsync();

                var cliente = clienteTask;

                if (cliente == null)
                    return null;

                aux = await produtoGeralBll.ListaProdutosComboDados(loja, cliente.uf, cliente.tipo_cliente,
                    (Constantes.ContribuinteICMS)cliente.contribuite_icms_status,
                    (Constantes.ProdutorRural)cliente.produtor_rural_status);

                //tratar a lista de compostos
                foreach (Produto.Dados.ProdutoCompostoDados composto in aux.ProdutoCompostoDados)
                {
                    decimal? somaFilhotes = 0;

                    var filhotes = (from c in aux.ProdutoDados
                                    join d in composto.Filhos on new { a = c.Fabricante, b = c.Produto } equals new { a = d.Fabricante, b = d.Produto }
                                    select c).ToList();


                    if (composto.Filhos.Count != filhotes.Count)
                    {
                        var prodARemover = aux.ProdutoDados.Where(x => x.Fabricante == composto.PaiFabricante && x.Produto == composto.PaiProduto).FirstOrDefault();
                        if (prodARemover != null) aux.ProdutoDados.Remove(prodARemover);
                        continue;
                    }

                    foreach (var filho in filhotes)
                    {
                        var compostoFilho = composto.Filhos.Where(x => x.Produto == filho.Produto).FirstOrDefault();

                        somaFilhotes += filho.Preco_lista * compostoFilho.Qtde;
                    }

                    var pai = aux.ProdutoDados.Where(x => x.Fabricante == composto.PaiFabricante && x.Produto == composto.PaiProduto).FirstOrDefault();
                    if (pai == null)
                    {
                        var produtoCompostoAInserir = new Produto.Dados.ProdutoDados()
                        {
                            Fabricante = composto.PaiFabricante,
                            Fabricante_Nome = composto.PaiFabricanteNome,
                            Produto = composto.PaiProduto,
                            Descricao_html = composto.PaiDescricao,
                            Descricao = composto.PaiDescricao,
                            Preco_lista = somaFilhotes,
                            Qtde_Max_Venda = filhotes.Min(x => x.Qtde_Max_Venda),
                            Desc_Max = filhotes.Min(x => x.Desc_Max)
                        };
                        aux.ProdutoDados.Add(produtoCompostoAInserir);
                    }
                    else
                    {
                        pai.Preco_lista = somaFilhotes;
                    }
                }
            }

            return ProdutoComboDto.ProdutoComboDto_De_ProdutoComboDados(aux);
        }
    }
}