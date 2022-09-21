using Produto.Dados;
using System.Collections.Generic;

namespace Prepedido.Dto
{
    public class ProdutoComboDto
    {
        public List<ProdutoDto> ProdutoDto { get; set; }
        public List<ProdutoCompostoDto> ProdutoCompostoDto { get; set; }

        internal static ProdutoComboDto ProdutoComboDto_De_ProdutoComboDados(ProdutoComboDados aux)
        {
            if (aux == null) return null;
            return new ProdutoComboDto()
            {
                ProdutoDto = Prepedido.Dto.ProdutoDto.ProdutoDtoLista_De_ProdutoDados(aux.ProdutoDados),
                ProdutoCompostoDto = Prepedido.Dto.ProdutoCompostoDto.ProdutoCompostoDtoLista_De_ProdutoCompostoDados(aux.ProdutoCompostoDados)
            };
        }
    }
}
