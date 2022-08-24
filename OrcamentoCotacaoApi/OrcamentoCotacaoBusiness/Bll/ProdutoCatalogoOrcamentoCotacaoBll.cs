using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using ProdutoCatalogo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

        public bool Excluir(int id)
        {
            return _bll.Excluir(id);
        }

        public bool Atualizar(TprodutoCatalogo produtoCatalogo)
        {
            return _bll.Atualizar(produtoCatalogo);
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

        public async Task<string> Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro, string caminho)
        {
            try
            {
                if (produtoCatalogo == null)
                    return "Ops! Parece que não existe dados de produto para catálogo!";

                produtoCatalogo.Id = _bll.Criar(produtoCatalogo, usuario_cadastro);

                if (produtoCatalogo.Id == 0)
                    return "Ops! Erro ao criar novo produto!";

                if (produtoCatalogo.imagens.Count > 0)
                {
                    Guid idArquivo = Guid.NewGuid();
                    var extensao = produtoCatalogo.imagens[0].Caminho.Substring(produtoCatalogo.imagens[0].Caminho.Length - 3, 3);
                    if (extensao != "png" && extensao != "jpeg" && extensao != "bmp" && extensao != "jpg")
                        return "Formato inválido. O arquivo deve ser imagem png, jpg ou bmp.";

                    var nome = produtoCatalogo.imagens[0].Caminho;
                    var file = Path.Combine(caminho, $"{idArquivo}.{extensao}");
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await fileStream.CopyToAsync(fileStream);
                    }
                    //File.Create(file);

                    SalvarArquivo($"{idArquivo}.{extensao}", produtoCatalogo.Id, 1, "0");

                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
