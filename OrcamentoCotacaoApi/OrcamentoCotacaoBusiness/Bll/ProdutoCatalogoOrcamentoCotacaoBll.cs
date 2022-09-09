using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Http;
using Produto;
using ProdutoCatalogo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public TprodutoCatalogo Detalhes(int id)
        {
            return _bll.Detalhes(id);
        }

        public string ExcluirImagem(int idProduto, int idImagem, string caminho)
        {
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var tProdutoCatalogoImagem = ObterListaImagensPorId(idProduto);
                    if (tProdutoCatalogoImagem.Count == 0) return "Ops! Imagem não encontrada!";

                    if (!_bll.ExcluirImagemComTransacao(idProduto, idImagem, dbGravacao)) return "Falha ao excluir a imagem do produto!";

                    var file = Path.Combine(caminho, tProdutoCatalogoImagem[0].Caminho);
                    if (!File.Exists(file))
                        return "Ops! O arquivo não foi encontrado no diretório!";

                    File.Delete(file);

                    if (File.Exists(file)) return "Falha ao deletar arquivo do diretório!";

                    dbGravacao.transacao.Commit();
                    return null;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
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

        public async Task<string> Atualizar(TprodutoCatalogo produtoCatalogo, IFormFile arquivo, string caminho)
        {
            var retornoValidacao = await ValidarTiposPropriedadesProdutoCatalogo(produtoCatalogo.campos);

            if (!string.IsNullOrEmpty(retornoValidacao)) return retornoValidacao;

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    if (produtoCatalogo == null)
                        throw new ArgumentException("Ops! Parece que não existe dados de produto para catálogo!");

                    produtoCatalogo = _bll.AtualizarComTransacao(produtoCatalogo, dbGravacao);

                    if (produtoCatalogo == null)
                        return "Ops! Erro ao atualizar produto!";

                    if (produtoCatalogo.campos == null || produtoCatalogo.campos.Count == 0)
                        return "Ops! As propriedades do produto não pode estar vazio!";

                    produtoCatalogo.campos = _bll.AtualizarItensComTransacao(produtoCatalogo.campos, produtoCatalogo.Id,
                        dbGravacao);
                    if (produtoCatalogo.campos == null || produtoCatalogo.campos.Count == 0)
                        return "Falha ao atualizar as propriedades do produto catálogo!";

                    if (arquivo != null)
                    {
                        var retorno = await CriarImagemComTransacao(arquivo, produtoCatalogo.imagens, caminho, produtoCatalogo.Id,
                            dbGravacao);
                        if (!string.IsNullOrEmpty(retorno))
                        {
                            dbGravacao.transacao.Rollback();
                            return retorno;
                        }
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

        public async Task<string> Criar(TprodutoCatalogo produtoCatalogo, string usuario_cadastro,
            IFormFile arquivo, string caminho)
        {
            var retornoValidacao = await ValidarTiposPropriedadesProdutoCatalogo(produtoCatalogo.campos);

            if (!string.IsNullOrEmpty(retornoValidacao)) return retornoValidacao;

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
                        {
                            dbGravacao.transacao.Rollback();
                            return retorno;
                        }
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

        private async Task<string> ValidarTiposPropriedadesProdutoCatalogo(List<TprodutoCatalogoItem> propriedades)
        {
            var tProdutoCatalogoPropriedades = await _produtoGeralBll.ObterListaPropriedadesProdutos();
            if (tProdutoCatalogoPropriedades == null)
                return "Falha ao validar propriedades do produto!";

            string retorno;

            retorno = await ValidarTiposPropriedadesTextoLivreProdutoCatalogo(propriedades, tProdutoCatalogoPropriedades);
            if (!string.IsNullOrEmpty(retorno)) return retorno;

            return await Task.FromResult(retorno);
        }

        private async Task<string> ValidarTiposPropriedadesTextoLivreProdutoCatalogo(List<TprodutoCatalogoItem> propriedades,
            List<Produto.Dados.ProdutoCatalogoPropriedadeDados> propriedadeDados)
        {
            var propriedadesTextoLivre = propriedadeDados.Where(x => x.IdCfgTipoPropriedade == 0).ToList();
            if (propriedadesTextoLivre == null)
                return "Falha ao buscar propriedades do produto";

            string retorno = "";
            foreach (var prop in propriedades)
            {
                var item = propriedadesTextoLivre.Where(x => x.id == prop.IdProdutoCatalogoPropriedade).FirstOrDefault();
                if (item != null)
                {
                    var tCfgDataType = _bll.ObterTipoPropriedadePorFiltro(new TcfgDataTypeFiltro() { Id = item.IdCfgDataType }).FirstOrDefault();
                    if (tCfgDataType == null)
                    {
                        retorno = $"Falha ao validar a propriedade '{item.descricao}'.";
                        break;
                    }

                    if (tCfgDataType.Sigla == "real")
                    {
                        if (!prop.Valor.Contains("."))
                            retorno = $"Propriedade '{item.descricao}' precisa conter ponto ('.')!";

                        if (!string.IsNullOrEmpty(retorno)) break;

                        if (!Single.TryParse(prop.Valor, out float valor))
                            retorno = $"Propriedade '{item.descricao}' está inválida!";

                        if (!string.IsNullOrEmpty(retorno)) break;
                    }

                    if (tCfgDataType.Sigla == "string")
                    {
                        if (string.IsNullOrEmpty(prop.Valor))
                            retorno = $"Propriedade '{item.descricao}' precisa ser preenchida!";

                        if (!string.IsNullOrEmpty(retorno)) break;
                    }

                    if (tCfgDataType.Sigla == "int")
                    {
                        if (prop.Valor.Contains("."))
                            retorno = $"Propriedade '{item.descricao}' não pode conter letras e símbolos!";

                        if (!string.IsNullOrEmpty(retorno)) break;

                        if (!int.TryParse(prop.Valor, out int valor))
                            retorno = $"Propriedade '{item.descricao}' está inválida!";

                        if (!string.IsNullOrEmpty(retorno)) break;
                    }
                }
            }

            return await Task.FromResult(retorno);
        }
    }
}
