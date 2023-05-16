using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using Prepedido.Dto;
using Produto;
using Produto.Dados;
using System;
using System.Collections.Generic;
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

                ProdutoComboDto response = new ProdutoComboDto();
                response.ProdutoDto = new List<ProdutoDto>();
                response.ProdutoCompostoDto = new List<ProdutoCompostoDto>();

                foreach (var produto in aux.ProdutoDados)
                {
                    var responseItem = ProdutoDto.ProdutoDto_De_ProdutoDados(produto);

                    var pai = aux.ProdutoCompostoDados.Where(x => x.Filhos.Where(f => f.Produto == produto.Produto).Any()).FirstOrDefault();
                    if (pai != null)
                    {
                        responseItem.UnitarioVendavel = false;
                    }
                    response.ProdutoDto.Add(responseItem);
                }

                foreach (ProdutoCompostoDados composto in aux.ProdutoCompostoDados)
                {
                    var produtoCompostoResponse = new ProdutoCompostoDto();
                    produtoCompostoResponse.Filhos = new List<ProdutoFilhoDto>();
                    decimal? somaFilhotes = 0;

                    var produtoCompostoResponseApoio = new ProdutoCompostoDto();
                    produtoCompostoResponseApoio.Filhos = new List<ProdutoFilhoDto>();

                    var filhotes = (from c in aux.ProdutoDados
                                    join d in composto.Filhos on new { a = c.Fabricante, b = c.Produto } equals new { a = d.Fabricante, b = d.Produto }
                                    select c).ToList();

                    if (composto.Filhos.Count != filhotes.Count)
                    {
                        var prodARemover = response.ProdutoDto.Where(x => x.Fabricante == composto.PaiFabricante && x.Produto == composto.PaiProduto).FirstOrDefault();
                        if (prodARemover != null) response.ProdutoDto.Remove(prodARemover);
                        continue;
                    }

                    foreach (var filho in filhotes)
                    {
                        var compostoFilho = composto.Filhos.Where(x => x.Produto == filho.Produto).FirstOrDefault();
                        produtoCompostoResponseApoio.Filhos.Add(ProdutoFilhoDto.ProdutoFilhoDto_De_ProdutoFilhoDados(compostoFilho));

                        somaFilhotes += Math.Round((decimal)filho.Preco_lista * compostoFilho.Qtde, 2);
                    }

                    var pai = aux.ProdutoDados.Where(x => x.Fabricante == composto.PaiFabricante && x.Produto == composto.PaiProduto).FirstOrDefault();

                    var novoItem = new ProdutoDto();
                    novoItem.Fabricante = composto.PaiFabricante;
                    novoItem.Fabricante_Nome = composto.PaiFabricanteNome;
                    novoItem.Produto = composto.PaiProduto;
                    novoItem.Descricao_html = pai != null ? pai.Descricao_html : composto.PaiDescricao;
                    novoItem.Preco_lista = (decimal)somaFilhotes;
                    novoItem.Qtde_Max_Venda = filhotes.Min(x => x.Qtde_Max_Venda);
                    novoItem.Estoque = filhotes.Min(x => x.Estoque);
                    novoItem.Alertas = filhotes.Min(x => x.Alertas);

                    if (pai != null) response.ProdutoDto.RemoveAll(x => x.Produto == pai.Produto);

                    response.ProdutoDto.Add(novoItem);

                    produtoCompostoResponse = ProdutoCompostoDto.ProdutoCompostoDto_De_ProdutoCompostoDados(composto);

                    produtoCompostoResponse.Preco_total_Itens = (decimal)somaFilhotes;
                    produtoCompostoResponse.Filhos = produtoCompostoResponseApoio.Filhos;
                    response.ProdutoCompostoDto.Add(produtoCompostoResponse);
                }
                return response;
            }
        }
    }
}