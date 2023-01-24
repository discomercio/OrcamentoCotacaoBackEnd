using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System.Collections.Generic;
using System.Linq;

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

        public List<TprodutoCatalogo> BuscarTprodutoCatalogo(TprodutoCatalogoFiltro obj)
        {
            return _data.BuscarTprodutoCatalogo(obj);
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

        public List<TprodutoCatalogoItem> ObterListaItensComTransacao(int idProduto,
            InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            return _data.ObterListaItens(idProduto);
        }

        public List<TprodutoCatalogoImagem> ObterListaImagensPorId(int id)
        {
            return _data.ObterListaImagensPorId(id);
        }

        public List<TprodutoCatalogoImagem> ObterDadosImagemPorProduto(string produto)
        {            
            var produtoCatalogo = _data.PorFiltro(new TprodutoCatalogoFiltro() { Produto = produto }).FirstOrDefault();
            List<TprodutoCatalogoImagem> produtoCatalogoImagemLst = new List<TprodutoCatalogoImagem>();

            if (produtoCatalogo != null)
            {
                produtoCatalogoImagemLst = _data.ObterListaImagensPorId(produtoCatalogo.Id);                
            }

            if (produtoCatalogoImagemLst == null || 
                produtoCatalogoImagemLst.Count == 0 ||
                produtoCatalogo == null
                )
            {
                var produtoCatalogoImagem = new TprodutoCatalogoImagem();

                produtoCatalogoImagem.Id = 1;
                produtoCatalogoImagem.Caminho = "sem-imagem.png";
                produtoCatalogoImagemLst.Add(produtoCatalogoImagem);

            }

            return produtoCatalogoImagemLst;
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

        public TprodutoCatalogo CriarComTransacao(TprodutoCatalogo obj, string usuario_cadastro,
            InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            obj.UsuarioCadastro = usuario_cadastro;
            var tProdCatalogo = _data.CriarComTransacao(obj, usuario_cadastro, contextoBdGravacao);

            return tProdCatalogo;
        }

        public List<TprodutoCatalogoItem> CriarItensComTransacao(List<TprodutoCatalogoItem> propriedades,
            int idProduto, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            List<TprodutoCatalogoItem> retorno = new List<TprodutoCatalogoItem>();
            foreach (var prop in propriedades)
            {
                prop.IdProdutoCatalogo = idProduto;
                if (!string.IsNullOrEmpty(prop.Valor)) prop.IdProdutoCatalogoPropriedadeOpcao = null;
                retorno.Add(_data.CriarItensComTransacao(prop, contextoBdGravacao));
            }

            return retorno;
        }

        public TprodutoCatalogoImagem CriarImagensComTransacao(TprodutoCatalogoImagem img, int idProduto,
            InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            var tipo = BuscarTipoImagemComTransacao(new TprodutoCatalogoImagemTipoFiltro() { Id = 1 }, contextoBdGravacao).FirstOrDefault();
            if (tipo == null) return null;

            img.IdProdutoCatalogo = idProduto;
            img.IdTipoImagem = tipo.Id;
            //List<TprodutoCatalogoImagem> retorno = new List<TprodutoCatalogoImagem>();
            //retorno.Add(_data.CriarImagensComTransacao(img, contextoBdGravacao));
            //return retorno;
            return _data.CriarImagensComTransacao(img, contextoBdGravacao);
        }

        public List<TprodutoCatalogoImagemTipo> BuscarTipoImagemComTransacao(TprodutoCatalogoImagemTipoFiltro filtro,
            InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            return _data.BuscarTipoImagemComTransacao(new TprodutoCatalogoImagemTipoFiltro() { Id = 1 }, contextoBdGravacao);
        }

        public TprodutoCatalogo AtualizarComTransacao(TprodutoCatalogo model,
            InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            var tProdutoCatalogo = _data.AtualizarComTransacao(model, contextoBdGravacao);
            return tProdutoCatalogo;
        }

        public List<TprodutoCatalogoItem> AtualizarItensComTransacao(List<TprodutoCatalogoItem> propriedades,
            int idProduto, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            List<TprodutoCatalogoItem> retorno = new List<TprodutoCatalogoItem>();

            List<TprodutoCatalogoItem> propriedadesAntigas = ObterListaItensComTransacao(idProduto, contextoBdGravacao);
            if (propriedadesAntigas == null || propriedadesAntigas.Count == 0) return null;

            foreach (var prop in propriedadesAntigas)
            {
                var existe = propriedades
                    .Where(x => x.IdProdutoCatalogoPropriedade == prop.IdProdutoCatalogoPropriedade)
                    .FirstOrDefault();

                if (existe == null)
                    if (!_data.ExcluirItensComTransacao(prop, contextoBdGravacao))
                        return null;
            }

            foreach (var prop in propriedades)
            {
                prop.IdProdutoCatalogo = idProduto;
                if (!string.IsNullOrEmpty(prop.Valor)) prop.IdProdutoCatalogoPropriedadeOpcao = null;
                retorno.Add(_data.AtualizarItemComTransacao(prop, contextoBdGravacao));
            }

            return retorno;
        }
        public bool ExcluirImagemComTransacao(int idProduto, int idImagem, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            return _data.ExcluirImagemComTransacao(idProduto, idImagem, contextoBdGravacao);
        }

        public List<TcfgDataType> ObterDataTypesPorFiltro(TcfgDataTypeFiltro filtro)
        {
            return _data.ObterDataTypesPorFiltro(filtro);
        }

        public List<TcfgTipoPropriedadeProdutoCatalogo> ObterTipoPropriedadesPorFiltro(TcfgTipoPropriedadeProdutoCatalogoFiltro filtro)
        {
            return _data.ObterTipoPropriedadesPorFiltro(filtro);
        }

        public List<TcfgTipoPermissaoEdicaoCadastro> ObterTipoPermissaoEdicaoCadastro()
        {
            return _data.ObterTipoPermissaoEdicaoCadastro();
        }

        public List<TprodutoGrupo> BuscarProdutoGrupos(TprodutoGrupoFiltro obj)
        {
            return _data.BuscarGrupos(obj);
        }
    }
}
