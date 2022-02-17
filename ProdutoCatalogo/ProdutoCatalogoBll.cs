using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System.Collections.Generic;

namespace ProdutoCatalogo
{
    public class ProdutoCatalogoBll : BaseBLL<TprodutoCatalogo, TprodutoCatalogoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;
        private readonly ProdutoCatalogoData _data;

        public ProdutoCatalogoBll(ContextoBdProvider contextoBdProvider) : base(new ProdutoCatalogoData(contextoBdProvider))
        {
            this.contextoProvider = contextoBdProvider;
            _data = (ProdutoCatalogoData)base.data;
        }

        public TprodutoCatalogo Detalhes(int id)
        {
            return _data.Detalhes(id);
        }

        public bool ExcluirImagem(int idProduto, int idImagem)
        {
            return _data.ExcluirImagem(idProduto, idImagem);
        }

        public List<TprodutoCatalogoItens> ObterListaItens(int id)
        {
            return _data.ObterListaItens(id);
        }

        public List<TprodutoCatalogoImagem> ObterListaImagens(int id)
        {
            return _data.ObterListaImagens(id);
        }

        public bool SalvarArquivo(string nomeArquivo, int idProdutoCatalogo, int idTipo, string ordem)
        {
            return _data.SalvarArquivo(nomeArquivo, idProdutoCatalogo, idTipo, ordem);
        }

        public bool Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro)
        {
            return _data.Criar(produtoCatalogo, usuario_cadastro);
        }
    }
}
