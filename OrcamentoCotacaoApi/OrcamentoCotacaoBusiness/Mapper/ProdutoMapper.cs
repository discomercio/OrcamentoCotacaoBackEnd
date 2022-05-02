using AutoMapper;
using OrcamentoCotacaoBusiness.Models.Response;
using Produto.Dados;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Mapper
{
    public class ProdutoMapper : Profile
    {
        public ProdutoMapper()
        {
            CreateMap<ProdutoCompostoDados, ProdutoCompostoResponseViewModel>();
            CreateMap<ProdutoCatalogoItemProdutosAtivosDados, ProdutoCatalogoItemProdutosAtivosResponseViewModel>();
            //CreateMap<ProdutoCatalogoItemProdutosAtivosResponseViewModel, ProdutoCatalogoItemProdutosAtivosDados>();
        }
    }
}
