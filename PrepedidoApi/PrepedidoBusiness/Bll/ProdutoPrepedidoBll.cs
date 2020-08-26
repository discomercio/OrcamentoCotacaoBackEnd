using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using Produto;
using Produto.RegrasCrtlEstoque;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll
{
    public class ProdutoPrepedidoBll
    {
        private readonly ProdutoGeralBll produtoGeralBll;

        public ProdutoPrepedidoBll(Produto.ProdutoGeralBll produtoGeralBll)
        {
            this.produtoGeralBll = produtoGeralBll;
        }
        public async Task<PrepedidoBusiness.Dto.Produto.ProdutoComboDto> ListaProdutosComboApiArclube(string loja, string id_cliente)
        {
            var aux = await produtoGeralBll.ListaProdutosComboDados(loja, id_cliente, null);
            return PrepedidoBusiness.Dto.Produto.ProdutoComboDto.ProdutoComboDtoDeProdutoComboDados(aux);
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

    }
}
