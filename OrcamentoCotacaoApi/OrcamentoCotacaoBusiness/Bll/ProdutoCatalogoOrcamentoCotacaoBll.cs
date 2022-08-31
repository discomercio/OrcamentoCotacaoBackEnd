using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Http;
using Produto;
using ProdutoCatalogo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class ProdutoCatalogoOrcamentoCotacaoBll
    {
        private readonly ProdutoCatalogoBll _bll;
        private readonly InfraBanco.ContextoBdProvider _contextoBdProvider;
        private readonly ProdutoGeralBll _produtoGeralBll;

        public ProdutoCatalogoOrcamentoCotacaoBll(ProdutoCatalogoBll bll,
            InfraBanco.ContextoBdProvider contextoBdProvider, ProdutoGeralBll produtoGeralBll)
        {
            _bll = bll;
            _contextoBdProvider = contextoBdProvider;
            _produtoGeralBll = produtoGeralBll;
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

        public async Task<string> Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro,
            IFormFile arquivo, string caminho)
        {
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    if (produtoCatalogo == null)
                        return "Ops! Parece que não existe dados de produto para catálogo!";

                    produtoCatalogo = _bll.CriarComTransacao(produtoCatalogo, usuario_cadastro, dbGravacao);

                    if (produtoCatalogo.Id == 0)
                        return "Ops! Erro ao criar novo produto!";

                    if (produtoCatalogo.campos == null || produtoCatalogo.campos.Count == 0)
                        return "Ops! As propriedades do produto não pode estar vazio!";

                    produtoCatalogo.campos = _bll.CriarItensComTransacao(produtoCatalogo.campos, produtoCatalogo.Id,
                        dbGravacao);

                    if (produtoCatalogo.imagens != null && produtoCatalogo.imagens.Count > 0)
                    {
                        var retorno = await CriarImagemComTransacao(arquivo, produtoCatalogo.imagens, caminho, produtoCatalogo.Id,
                            dbGravacao);
                        if (!string.IsNullOrEmpty(retorno))
                            return retorno;
                    }

                    dbGravacao.transacao.Commit();
                    return null;

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public async Task<string> CriarImagemComTransacao(IFormFile arquivo, List<TprodutoCatalogoImagem> produtoCatalogoImagens, 
            string caminho, int idProdutoCatalogo, InfraBanco.ContextoBdGravacao dbGravacao)
        {
            if (arquivo == null)
            {
                var nomeArquivoCopia = produtoCatalogoImagens[0].Caminho;
                var extensaoCopia = produtoCatalogoImagens[0].Caminho.Split(".")[1];

                var novoNomeArquivo = CriarNomeArquivo(extensaoCopia);
                if (string.IsNullOrEmpty(novoNomeArquivo))
                    return "Falha ao gerar nome do arquivo!";

                produtoCatalogoImagens[0].Caminho = novoNomeArquivo;
                produtoCatalogoImagens = _bll.CriarImagensComTransacao(produtoCatalogoImagens,
                        idProdutoCatalogo, dbGravacao);

                if (produtoCatalogoImagens[0].Id == 0)
                    return "Ops! Erro ao salvar dados da imagem!";

                var arquivoABuscar = Path.Combine(caminho, nomeArquivoCopia);
                var retorno = await ClonarImagemDiretorio(arquivoABuscar, novoNomeArquivo, caminho);
                if (!string.IsNullOrEmpty(retorno))
                    return retorno;

                return null;
            }
            else
            {
                var extensao = arquivo.FileName.Substring(arquivo.FileName.Length - 3, 3);
                var nomeArquivo = CriarNomeArquivo(extensao);
                if (string.IsNullOrEmpty(nomeArquivo))
                    return "Falha ao gerar nome do arquivo!";

                produtoCatalogoImagens[0].Caminho = nomeArquivo;
                produtoCatalogoImagens = _bll.CriarImagensComTransacao(produtoCatalogoImagens,
                                    idProdutoCatalogo, dbGravacao);
                if (produtoCatalogoImagens[0].Id == 0)
                    return "Ops! Erro ao salvar dados da imagem!";

                var retorno = await InserirImagemDiretorio(arquivo, caminho, nomeArquivo);
                if (!string.IsNullOrEmpty(retorno))
                    return retorno;

                return null;
            }
        }

        private string CriarNomeArquivo(string extensao)
        {
            if (string.IsNullOrEmpty(extensao))
                return null;

            Guid idArquivo = Guid.NewGuid();
            return $"{idArquivo}.{extensao}";
        }

        private async Task<string> ClonarImagemDiretorio(string arquivoABuscar, string novoNomeArquivo, string caminho)
        {
            try
            {
                using (var stream = new FileStream(arquivoABuscar, FileMode.Open))
                {
                    var novoArquivo = Path.Combine(caminho, novoNomeArquivo);

                    using (var fileStream = new FileStream(novoArquivo, FileMode.CreateNew))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    return null;
                }
            }
            catch (Exception)
            {
                return "Falha ao clonar arquivo no diretório!";
            }
        }

        private async Task<string> InserirImagemDiretorio(IFormFile arquivo, string caminho, string nomeArquivo)
        {
            if (arquivo.ContentType.Contains("png") || arquivo.ContentType.Contains("jpeg") || arquivo.ContentType.Contains("bmp") || arquivo.ContentType.Contains("jpg"))
            {
                try
                {
                    var file = Path.Combine(caminho, nomeArquivo);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await arquivo.CopyToAsync(fileStream);
                    }
                    return null;
                }
                catch (Exception)
                {
                    return "Falha ao criar arquivo no diretório!";
                }
            }
            else
            {
                return "Formato inválido. O arquivo deve ser imagem png, jpg ou bmp.";
            }
        }
    }
}
