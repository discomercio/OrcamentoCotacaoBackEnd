using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Produto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfraBanco.Modelos;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PrepedidoBusiness.Bll
{
    public class ProdutoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public ProdutoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        private bool LojaHabilitadaProdutosECommerce(string loja)
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


        private bool IsLojaVrf(string loja)
        {
            bool retorno = false;

            if (loja == Constantes.NUMERO_LOJA_VRF ||
                loja == Constantes.NUMERO_LOJA_VRF2 ||
                loja == Constantes.NUMERO_LOJA_VRF3 ||
                loja == Constantes.NUMERO_LOJA_VRF4)
                retorno = true;

            return retorno;
        }


        public async Task<IEnumerable<ProdutoDto>> BuscarProduto(string codProduto, string loja, string apelido, List<string> lstErros)
        {
            //paraTeste
            //apelido = "MARISARJ";
            //codProduto = "003000";
            //loja = "202";
            int qtde = 0;
            bool lojaHabilitada = false;
            decimal vlProdCompostoPrecoListaLoja = 0;
            ProdutoDto produtoDto = new ProdutoDto();

            List<ProdutoDto> lstProduto = new List<ProdutoDto>();

            var db = contextoProvider.GetContextoLeitura();

            if (string.IsNullOrEmpty(codProduto))
                return null;
            else if (string.IsNullOrEmpty(loja))
                return null;

            if (LojaHabilitadaProdutosECommerce(loja))
            {
                var prodCompostoTask = from c in db.TecProdutoCompostos
                                       where c.Produto_Composto == codProduto
                                       select c;

                string parada = "";

                var prodComposto = prodCompostoTask.FirstOrDefault();

                if (prodComposto.Produto_Composto != null)
                {
                    var prodCompostoItensTask = from c in db.TecProdutoCompostoItems
                                                where c.Fabricante_composto == prodComposto.Fabricante_Composto &&
                                                      c.Produto_composto == prodComposto.Produto_Composto &&
                                                      c.Excluido_status == 0
                                                orderby c.Sequencia
                                                select c;
                    var prodCompostoItens = prodCompostoItensTask.ToList();

                    if (prodCompostoItens.Count > 0)
                    {
                        foreach (var pi in prodCompostoItens)
                        {
                            var produtoTask = from c in db.Tprodutos.Include(r => r.TprodutoLoja)
                                              where c.TprodutoLoja.Fabricante == pi.Fabricante_item &&
                                                    c.TprodutoLoja.Produto == pi.Produto_item &&
                                                    c.TprodutoLoja.Loja == loja
                                              select c;

                            var produto = await produtoTask.FirstOrDefaultAsync();

                            if (string.IsNullOrEmpty(produto.Produto))
                                lstErros.Add("O produto(" + pi.Fabricante_item + ")" + pi.Produto_item + " não está disponível para a loja " + loja + "!!");
                            else
                            {
                                produtoDto = new ProdutoDto
                                {
                                    Fabricante = pi.Fabricante_item,
                                    Produto = pi.Produto_item,
                                    Qtde = pi.Qtde,
                                    ValorLista = produto.TprodutoLoja.Preco_Lista,
                                    Descricao = produto.Descricao
                                };
                            }
                        }
                    }

                }
                else
                {
                    var produtoTask = from c in db.Tprodutos.Include(r => r.TprodutoLoja)
                                      where c.TprodutoLoja.Produto == codProduto &&
                                            c.TprodutoLoja.Loja == loja
                                      select c;

                    var produto = await produtoTask.FirstOrDefaultAsync();

                    if (string.IsNullOrEmpty(produto.Produto))
                        lstErros.Add("Produto '" + codProduto + "' não foi encontrado para a loja " + loja + "!!");
                    else
                    {
                        produtoDto = new ProdutoDto
                        {
                            Fabricante = produto.Fabricante,
                            Produto = produto.Produto,
                            Qtde = qtde,
                            ValorLista = produto.TprodutoLoja.Preco_Lista,
                            Descricao = produto.Descricao
                        };
                    }
                }
            }
            


            return lstProduto;
        }
    }
}
