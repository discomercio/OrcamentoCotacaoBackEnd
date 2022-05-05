using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System.Collections.Generic;

namespace ProdutoCatalogo
{
    public class ProdutoCatalogoBll : BaseBLL<TprodutoCatalogo, TprodutoCatalogoFiltro>
    {
        private readonly ProdutoCatalogoData _data;

        public ProdutoCatalogoBll(ContextoBdProvider contextoBdProvider) : base(new ProdutoCatalogoData(contextoBdProvider))
        {
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

        public List<TprodutoCatalogoItem> ObterListaItens(int id)
        {
            return _data.ObterListaItens(id);
        }

        public List<TprodutoCatalogoImagem> ObterListaImagensPorId(int id)
        {
            return _data.ObterListaImagensPorId(id);
        }

        public List<TprodutoCatalogoImagem> ObterListaImagem(List<int>idProdutos)
        {
            return _data.ObterListaImagens(idProdutos);
        }

        public bool SalvarArquivo(string nomeArquivo, int idProdutoCatalogo, int idTipo, string ordem)
        {
            return _data.SalvarArquivo(nomeArquivo, idProdutoCatalogo, idTipo, ordem);
        }

        public bool Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro)
        {
            //TODO: NAO TEM COMO DESABILITAR TRACKING
            var prodCatalogo = _data.Criar(produtoCatalogo, usuario_cadastro);

            if (prodCatalogo != null && prodCatalogo.Id > 0)
            {
                if (prodCatalogo.campos?.Count > 0)
                {
                    foreach (var c in prodCatalogo.campos)
                    {
                        _data.CriarItens(c);
                    }
                }

                if (prodCatalogo.imagens?.Count > 0)
                {
                    foreach (var img in prodCatalogo.imagens)
                    {
                        _data.CriarImagens(img);
                    }
                }

                _data.ExcluirImagemTmp();

                return true;
            }

            return false;
        }
    }
}
