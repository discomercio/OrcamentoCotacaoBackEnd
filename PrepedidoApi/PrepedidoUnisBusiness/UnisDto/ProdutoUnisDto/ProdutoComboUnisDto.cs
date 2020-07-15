using PrepedidoBusiness.Bll.ProdutoBll.ProdutoDados;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto
{
    public class ProdutoComboUnisDto
    {
        public List<ProdutoUnisDto> ProdutoUnisDto { get; set; }
        public List<ProdutoCompostoUnisDto> ProdutoCompostoUnisDto { get; set; }

        internal static ProdutoComboUnisDto ProdutoComboUnisDtoDeProdutoComboDados(ProdutoComboDados p)
        {
            return new ProdutoComboUnisDto()
            {
                ProdutoUnisDto = ProdutoUnisDtoListaDeProdutoDados(p.ProdutoDados),
                ProdutoCompostoUnisDto = PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto.ProdutoCompostoUnisDto.ProdutoCompostoUnisDtoListaDeProdutoCompostoDados(p.ProdutoCompostoDados)
            };
        }

        internal static List<ProdutoUnisDto> ProdutoUnisDtoListaDeProdutoDados(List<ProdutoDados> produtoDados)
        {
            var ret = new List<ProdutoUnisDto>();
            foreach (var p in produtoDados)
                ret.Add(PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto.ProdutoUnisDto.ProdutoUnisDtoDeProdutoDados(p));
            return ret;
        }
    }
}
