using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using ProdutoCatalogo;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class ProdutoCatalogoOrcamentoCotacaoBll
    {
        private readonly ProdutoCatalogoBll _bll;

        public ProdutoCatalogoOrcamentoCotacaoBll(ProdutoCatalogoBll bll)
        {
            _bll = bll;
        }

        public List<TprodutoCatalogo> PorFiltro(TprodutoCatalogoFiltro filtro)
        {
            return _bll.PorFiltro(filtro);
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

        public List<TprodutoCatalogoImagem> ObterListaImagens(int id)
        {
            return _bll.ObterListaImagens(id);
        }

        public bool SalvarArquivo(string nomeArquivo, int idProdutoCatalogo, int idTipo, string ordem)
        {
            return _bll.SalvarArquivo(nomeArquivo, idProdutoCatalogo, idTipo, ordem);
        }

        public bool Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro)
        {
            return _bll.Criar(produtoCatalogo, usuario_cadastro);
        }
    }
}
