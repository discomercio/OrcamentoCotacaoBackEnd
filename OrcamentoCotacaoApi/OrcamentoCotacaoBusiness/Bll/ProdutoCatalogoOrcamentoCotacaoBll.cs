using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProdutoCatalogoOrcamentoCotacaoBll> _logger;

        public ProdutoCatalogoOrcamentoCotacaoBll(
            ProdutoCatalogoBll bll,
            InfraBanco.ContextoBdProvider contextoBdProvider, 
            ProdutoGeralBll produtoGeralBll,
            ILogger<ProdutoCatalogoOrcamentoCotacaoBll> logger)
        {
            _bll = bll;
            _contextoBdProvider = contextoBdProvider;
            _produtoGeralBll = produtoGeralBll;
            _logger = logger;
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

        public List<TprodutoCatalogoImagem> ObterDadosImagemPorProduto(string produto)
        {
            return _bll.ObterDadosImagemPorProduto(produto);
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
            _logger.LogInformation($"Método Criar Produto");
            
            var retornoValidacao = await ValidarTiposPropriedadesProdutoCatalogo(produtoCatalogo.campos);

            _logger.LogInformation($"Método Criar Produto - Verifica e valida tipo de propriedades.");
            if (!string.IsNullOrEmpty(retornoValidacao))
            {
                _logger.LogInformation($"Método Criar Produto - Houve um erro na validação de propriedades.");
                return retornoValidacao;
            }

            _logger.LogInformation($"Método Criar Produto - Inicia contexto de transação de banco de dados.");
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    _logger.LogInformation($"Método Criar Produto - Verifica catalago do produto.");
                    if (produtoCatalogo == null)
                    {
                        return "Ops! Parece que não existe dados de produto para catálogo!";
                    }

                    _logger.LogInformation($"Método Criar Produto - Grava Produto.");
                    produtoCatalogo = _bll.CriarComTransacao(produtoCatalogo, usuario_cadastro, dbGravacao);

                    _logger.LogInformation($"Método Criar Produto - Verifica se produto foi saldo com sucesso.");
                    if (produtoCatalogo.Id == 0)
                    {
                        return "Ops! Erro ao criar novo produto!";
                    }

                    _logger.LogInformation($"Método Criar Produto - Verifica se existe propriedades do produto.");
                    if (produtoCatalogo.campos == null
                        || produtoCatalogo.campos.Count == 0)
                    {
                        return "Ops! As propriedades do produto não pode estar vazio!";
                    }

                    _logger.LogInformation($"Método Criar Produto - Grava as propriedade relacionando com produto atual.");
                    produtoCatalogo.campos = _bll.CriarItensComTransacao(
                        produtoCatalogo.campos,
                        produtoCatalogo.Id,
                        dbGravacao);

                    _logger.LogInformation($"Método Criar Produto - Verifica se tem imagens.");
                    if (produtoCatalogo.imagens != null && produtoCatalogo.imagens.Count > 0)
                    {
                        _logger.LogInformation($"Método Criar Produto - Grava a imagem e faz upload no diretio - [{caminho}].");
                        var retorno = await CriarImagemComTransacao(
                            arquivo,
                            produtoCatalogo.imagens,
                            caminho,
                            produtoCatalogo.Id,
                            dbGravacao);

                        _logger.LogInformation($"Método Criar Produto - Verificação se cadastro e upload da imagem de certo.");
                        if (!string.IsNullOrEmpty(retorno))
                        {
                            _logger.LogInformation($"Método Criar Produto - Houve um problema no cadastro da imagem.");
                            dbGravacao.transacao.Rollback();
                            return retorno;
                        }
                    }

                    _logger.LogInformation($"Método Criar Produto - Commit do cadastro do produto.");
                    dbGravacao.transacao.Commit();
                    return null;

                }
                catch (Exception ex)
                {
                    var innerException = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                    var msgError = $"Método Criar Produto - Houve uma Exception no processo de Gravação e Upload do produto. Exception: {ex.Message} / InnerException: {innerException}";

                    _logger.LogError(msgError);
                    return msgError;
                }
            }
        }

        public async Task<string> CriarImagemComTransacao(
            IFormFile arquivo, 
            List<TprodutoCatalogoImagem> produtoCatalogoImagens,
            string caminho, 
            int idProdutoCatalogo, 
            InfraBanco.ContextoBdGravacao dbGravacao)
        {
            _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao.");

            _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao - Verifica se tem imgem para salvar e fazer upload.");
            if (arquivo == null)
            {
                var nomeArquivoCopia = produtoCatalogoImagens[0].Caminho;
                var extensaoCopia = produtoCatalogoImagens[0].Caminho.Split(".")[1];
                
                var novoNomeArquivo = CriarNomeArquivo(extensaoCopia);
                
                if (string.IsNullOrEmpty(novoNomeArquivo))
                {
                    return "Falha ao gerar nome do arquivo!";
                }

                produtoCatalogoImagens[0].Caminho = novoNomeArquivo;

                produtoCatalogoImagens = _bll.CriarImagensComTransacao(
                    produtoCatalogoImagens,
                    idProdutoCatalogo, dbGravacao);

                if (produtoCatalogoImagens == null || produtoCatalogoImagens[0].Id == 0)
                {
                    return "Ops! Erro ao salvar dados da imagem!";
                }

                var arquivoABuscar = Path.Combine(caminho, nomeArquivoCopia);

                var retorno = await ClonarImagemDiretorio(arquivoABuscar, novoNomeArquivo, caminho);

                if (!string.IsNullOrEmpty(retorno))
                {
                    return retorno;
                }

                return null;
            }
            else
            {
                var extensao = arquivo.FileName.Substring(arquivo.FileName.Length - 3, 3);

                var nomeArquivo = CriarNomeArquivo(extensao);

                _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao. Verifica novo nome do arquivo [{nomeArquivo}]");
                if (string.IsNullOrEmpty(nomeArquivo))
                {
                    _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao. Houve uma falha para gerar novo do arquivo.");
                    return "Falha ao gerar nome do arquivo!";
                }

                produtoCatalogoImagens[0].Caminho = nomeArquivo;

                _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao. Grava imagem {nomeArquivo} com código do produto {idProdutoCatalogo}.");
                produtoCatalogoImagens = _bll.CriarImagensComTransacao(
                    produtoCatalogoImagens,
                    idProdutoCatalogo,
                    dbGravacao);

                _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao. Verifica se gravação da imagem com código da imagem.");
                if (produtoCatalogoImagens == null || produtoCatalogoImagens[0].Id == 0)
                {
                    _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao. Houve um erro na gravação do arquivo no banco de dados.");
                    return "Ops! Erro ao salvar dados da imagem!";
                }

                _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao. Upload da imagem no diretório: {caminho} e nome do arquivo:{nomeArquivo}.");
                var retorno = await InserirImagemDiretorio(arquivo, caminho, nomeArquivo);

                _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao. Verifica se Upload foi com sucesso.");
                if (!string.IsNullOrEmpty(retorno))
                {
                    _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao. Houve um erro no Upload do arquivo. Mensagem de erro: {retorno}");
                    return retorno;
                }

                _logger.LogInformation($"Método Criar Produto - CriarImagemComTransacao. Processo de gravação e upload foi finalizado com sucesso.");
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
