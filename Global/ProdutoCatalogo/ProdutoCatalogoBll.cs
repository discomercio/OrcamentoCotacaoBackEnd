using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System.Collections.Generic;

namespace ProdutoCatalogo
{
    public class ProdutoCatalogoBll
    {
        private readonly ProdutoCatalogoData _data;

        public ProdutoCatalogoBll(ProdutoCatalogoData data)
        {
            _data = data;
        }

        public TprodutoCatalogo Detalhes(int id)
        {
            return _data.Detalhes(id);
        }

        public List<TprodutoCatalogo> PorFiltro(TprodutoCatalogoFiltro filtro)
        {
            return _data.PorFiltro(filtro);
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

        public List<TprodutoCatalogoImagem> ObterListaImagem(List<int> idProdutos)
        {
            return _data.ObterListaImagens(idProdutos);
        }
        public bool Excluir(int id)
        {
            return _data.Excluir(id);
        }

        public bool SalvarArquivo(string nomeArquivo, int idProdutoCatalogo, int idTipo, string ordem)
        {
            return _data.SalvarArquivo(nomeArquivo, idProdutoCatalogo, idTipo, ordem);
        }

        public int Criar(TprodutoCatalogo obj, string usuario_cadastro)
        {
            //TODO: NAO TEM COMO DESABILITAR TRACKING
            var prodCatalogo = _data.Criar(obj, usuario_cadastro);

            if (prodCatalogo != null && prodCatalogo.Id > 0)
            {
                if (prodCatalogo.campos?.Count > 0)
                {
                    foreach (var c in prodCatalogo.campos)
                    {
                        _data.CriarItens(c);
                    }
                }

                //if (prodCatalogo.imagens?.Count > 0)
                //{
                //    var extensao = prodCatalogo.imagens[0].Caminho.Substring(prodCatalogo.imagens[0].Caminho.Length - 3, 3);
                //    if (extensao == "png" || extensao == "jpeg" || extensao == "bmp" || extensao == "jpg")
                //    {
                //        foreach (var img in prodCatalogo.imagens)
                //        {
                //            _data.CriarImagens(img);
                //        }
                //    }
                //}

                //_data.ExcluirImagemTmp();

                return prodCatalogo.Id;
            }

            return 0;
        }

        public bool Atualizar(TprodutoCatalogo obj)
        {
            //TODO: NAO TEM COMO DESABILITAR TRACKING
            var prodCatalogo = _data.Atualizar(obj);

            if (prodCatalogo != null)
            {
                _data.ExcluirItens(obj);

                if (obj != null && obj.Id > 0)
                {
                    if (obj.campos?.Count > 0)
                    {
                        foreach (var c in obj.campos)
                        {
                            _data.CriarItens(c);
                        }
                    }

                    if (obj.imagens?.Count > 0)
                    {
                        foreach (var img in obj.imagens)
                        {
                            _data.CriarImagens(img);
                        }
                    }

                    _data.ExcluirImagemTmp();

                    return true;
                }
            }

            return false;
        }
    }
}
