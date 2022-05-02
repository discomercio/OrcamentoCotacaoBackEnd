using AutoMapper;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using ProdutoCatalogo;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class ProdutoCatalogoOrcamentoCotacaoBll
    {
        private readonly ProdutoCatalogoBll _bll;
        private readonly IMapper _mapper;

        public ProdutoCatalogoOrcamentoCotacaoBll(ProdutoCatalogoBll bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        public List<TprodutoCatalogo> PorFiltro(TprodutoCatalogoFiltro filtro)
        {
            var produtosCatalogos = _bll.PorFiltro(new TprodutoCatalogoFiltro() { IncluirImagem = true }) ;
            var idProdutos = from c in produtosCatalogos 
                             select c.Id;

            var imagens = _bll.ObterListaImagem(idProdutos.ToList());

            foreach(var prod in produtosCatalogos)
            {
                var img = imagens.Where(x => x.IdProdutoCatalogo == prod.Id).FirstOrDefault();
                if(img != null)
                {
                    prod.imagens = new List<TprodutoCatalogoImagem>();
                    prod.imagens.Add(img);
                }
            }
            return produtosCatalogos;
        }

        public void Excluir(int id)
        {
            _bll.Excluir(new TprodutoCatalogo() { Id = id });
        }

        public void Atualizar(TprodutoCatalogo produtoCatalogo)
        {
            _bll.Atualizar(produtoCatalogo);
        }

        public TprodutoCatalogo Detalhes(int id)
        {
            return _bll.Detalhes(id);
        }

        public bool ExcluirImagem(int idProduto, int idImagem)
        {
            return _bll.ExcluirImagem(idProduto, idImagem);
        }

        public List<TprodutoCatalogoItem> ObterListaItens(int id)
        {
            return _bll.ObterListaItens(id);
        }

        public List<TprodutoCatalogoImagem> ObterListaImagensPorId(int id)
        {
            return _bll.ObterListaImagensPorId(id);
        }

        public bool SalvarArquivo(string nomeArquivo, int idProdutoCatalogo, int idTipo, string ordem)
        {
            return _bll.SalvarArquivo(nomeArquivo, idProdutoCatalogo, idTipo, ordem);
        }

        public bool Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro)
        {
            return _bll.Criar(produtoCatalogo, usuario_cadastro);
        }

        public bool CriarItem(TprodutoCatalogoItem produtoCatalogoItem)
        {
            return _bll.CriarItem(produtoCatalogoItem);
        }
    }
}
