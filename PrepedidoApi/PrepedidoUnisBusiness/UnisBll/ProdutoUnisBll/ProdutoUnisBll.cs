using PrepedidoBusiness.Bll.ProdutoBll;
using PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll
{
    public class ProdutoUnisBll
    {
        private readonly ProdutoGeralBll produtoBll;

        public ProdutoUnisBll(PrepedidoBusiness.Bll.ProdutoBll.ProdutoGeralBll produtoBll)
        {
            this.produtoBll = produtoBll;
        }
        public async Task<ProdutoComboUnisDto> ListaProdutosCombo(string loja, string cpf_cnpj)
        {
            var ret = await produtoBll.ListaProdutosComboDados(loja, null, cpf_cnpj);

            ProdutoComboUnisDto produtoComboUnisDto = ProdutoComboUnisDto.ProdutoComboUnisDtoDeProdutoComboDados(ret);
            return produtoComboUnisDto;
        }
    }
}
