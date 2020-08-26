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

       
    }
}
