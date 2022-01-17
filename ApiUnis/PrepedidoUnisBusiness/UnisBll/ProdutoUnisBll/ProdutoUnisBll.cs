using PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto;
using Produto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll
{
    public class ProdutoUnisBll
    {
        private readonly ProdutoGeralBll produtoBll;

        public ProdutoUnisBll(ProdutoGeralBll produtoBll)
        {
            this.produtoBll = produtoBll;
        }
        public async Task<ProdutoComboUnisDto> ListaProdutosCombo(string loja, string cpf_cnpj)
        {
            if (loja == null)
                return null;
            if (cpf_cnpj == null)
                return null;
            var ret = await produtoBll.ListaProdutosComboDados(loja, null, cpf_cnpj);
            if (ret == null)
                return null;

            ProdutoComboUnisDto produtoComboUnisDto = ProdutoComboUnisDto.ProdutoComboUnisDtoDeProdutoComboDados(ret);
            return produtoComboUnisDto;
        }

        public async Task<List<ProdutoUnisDto>> ListarProdutos(string loja)
        {
            var ret = await produtoBll.BuscarTodosProdutos(loja);
            if (ret == null)
                return null;

            //ProdutoComboUnisDto produtoComboUnisDto = ProdutoComboUnisDto.ProdutoComboUnisDtoDeProdutoComboDados(ret);
            return ((List<Produto.Dados.ProdutoDados>)ret).Select(x => new ProdutoUnisDto() { Produto = x.Produto  }).ToList();
        }
    }
}
