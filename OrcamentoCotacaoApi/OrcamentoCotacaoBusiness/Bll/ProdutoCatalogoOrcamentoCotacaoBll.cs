using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Response;
using Produto;
using ProdutoCatalogo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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

                    var tProdutoCatalogo = _bll.AtualizarComTransacao(produtoCatalogo, dbGravacao);

                    if (tProdutoCatalogo == null)
                        return "Ops! Erro ao atualizar produto!";

                    if (produtoCatalogo.campos == null || produtoCatalogo.campos.Count == 0)
                        return "Ops! As propriedades do produto não pode estar vazio!";

                    tProdutoCatalogo.campos = _bll.AtualizarItensComTransacao(produtoCatalogo.campos, tProdutoCatalogo.Id,
                        dbGravacao);
                    if (tProdutoCatalogo.campos == null || tProdutoCatalogo.campos.Count == 0)
                        return "Falha ao atualizar as propriedades do produto catálogo!";

                    if (arquivo != null)
                    {
                        var retorno = await CriarImagemComTransacao(arquivo, produtoCatalogo.imagem, caminho, tProdutoCatalogo.Id,
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

        public async Task<string> Criar(
            TprodutoCatalogo produtoCatalogo1,
            string usuario_cadastro,
            IFormFile arquivo,
            string caminho)
        {
            var retornoValidacao = await ValidarTiposPropriedadesProdutoCatalogo(produtoCatalogo1.campos);

            if (!string.IsNullOrEmpty(retornoValidacao))
            {
                return retornoValidacao;
            }

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    if (produtoCatalogo1 == null)
                    {
                        return "Ops! Parece que não existe dados de produto para catálogo!";
                    }

                    if (produtoCatalogo1.imagem != null)
                    {
                        var tipo = _bll.BuscarTipoImagemComTransacao(new TprodutoCatalogoImagemTipoFiltro() { Id = 1 }, dbGravacao).FirstOrDefault();
                        produtoCatalogo1.imagem.IdTipoImagem = tipo.Id;
                    }

                    var prod = new TprodutoCatalogo()
                    {
                        Ativo = produtoCatalogo1.Ativo,
                        Descricao = produtoCatalogo1.Descricao,
                        Fabricante = produtoCatalogo1.Fabricante,
                        Nome = produtoCatalogo1.Nome,
                        Produto = produtoCatalogo1.Produto
                    };

                    var tProdutoCatalogo = _bll.CriarComTransacao(prod, usuario_cadastro, dbGravacao);

                    if (tProdutoCatalogo.Id == 0)
                    {
                        return "Ops! Erro ao criar novo produto!";
                    }

                    if (produtoCatalogo1.campos == null
                        || produtoCatalogo1.campos.Count == 0)
                    {
                        return "Ops! As propriedades do produto não pode estar vazio!";
                    }

                    tProdutoCatalogo.campos = _bll.CriarItensComTransacao(
                        produtoCatalogo1.campos,
                        tProdutoCatalogo.Id,
                        dbGravacao);

                    if (produtoCatalogo1.imagem != null)
                    {
                        var retorno = await CriarImagemComTransacao(
                            arquivo,
                            produtoCatalogo1.imagem,
                            caminho,
                            tProdutoCatalogo.Id,
                            dbGravacao);

                        if (!string.IsNullOrEmpty(retorno))
                        {
                            dbGravacao.transacao.Rollback();
                            return retorno;
                        }
                    }
                    //if (produtoCatalogo.imagens != null && produtoCatalogo.imagens.Count > 0)
                    //{
                    //    var retorno = await CriarImagemComTransacao(
                    //        arquivo,
                    //        produtoCatalogo.imagens,
                    //        caminho,
                    //        produtoCatalogo.Id,
                    //        dbGravacao);

                    //    if (!string.IsNullOrEmpty(retorno))
                    //    {
                    //        dbGravacao.transacao.Rollback();
                    //        return retorno;
                    //    }
                    //}

                    dbGravacao.transacao.Commit();
                    return null;

                }
                catch (Exception ex)
                {
                    _logger.LogDebug(JsonSerializer.Serialize(ex));
                    return ex.Message;
                }
            }
        }

        public async Task<string> CriarImagemComTransacao(
            IFormFile arquivo,
            TprodutoCatalogoImagem produtoCatalogoImagem,
            string caminho,
            int idProdutoCatalogo,
            InfraBanco.ContextoBdGravacao dbGravacao)
        {
            if (arquivo == null)
            {
                var nomeArquivoCopia = produtoCatalogoImagem.Caminho;
                var extensaoCopia = produtoCatalogoImagem.Caminho.Split(".")[1];

                var novoNomeArquivo = CriarNomeArquivo(extensaoCopia);

                if (string.IsNullOrEmpty(novoNomeArquivo))
                {
                    return "Falha ao gerar nome do arquivo!";
                }

                produtoCatalogoImagem.Caminho = novoNomeArquivo;

                produtoCatalogoImagem = _bll.CriarImagensComTransacao(
                    produtoCatalogoImagem,
                    idProdutoCatalogo, dbGravacao);

                if (produtoCatalogoImagem == null)
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

                if (string.IsNullOrEmpty(nomeArquivo))
                {
                    return "Falha ao gerar nome do arquivo!";
                }

                produtoCatalogoImagem.Caminho = nomeArquivo;

                produtoCatalogoImagem = _bll.CriarImagensComTransacao(
                    produtoCatalogoImagem,
                    idProdutoCatalogo,
                    dbGravacao);

                if (produtoCatalogoImagem == null)
                {
                    return "Ops! Erro ao salvar dados da imagem!";
                }

                var retorno = await InserirImagemDiretorio(arquivo, caminho, nomeArquivo);

                if (!string.IsNullOrEmpty(retorno))
                {
                    return retorno;
                }

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
                    var tCfgDataType = _bll.ObterDataTypesPorFiltro(new TcfgDataTypeFiltro() { Id = item.IdCfgDataType }).FirstOrDefault();
                    if (tCfgDataType == null)
                    {
                        retorno = $"Falha ao validar a propriedade '{item.descricao}'.";
                        break;
                    }

                    retorno = ValidarTipoPropriedade(prop.Valor, item.descricao, tCfgDataType.Sigla);
                    if (!string.IsNullOrEmpty(retorno)) break;
                }
            }

            return await Task.FromResult(retorno);
        }

        private string ValidarTipoPropriedade(string valor, string descricao, string sigla)
        {
            string retorno = "";
            if (sigla == "real")
            {
                if (!Single.TryParse(valor, out float valorRetorno))
                    retorno = $"Propriedade '{descricao}' está inválida!";

                if (valor.Contains(".")) retorno = $"Propriedade '{descricao}' está com ponto, esperamos vírgula!";
            }

            if (sigla == "string")
            {
                if (string.IsNullOrEmpty(valor))
                    retorno = $"Propriedade '{descricao}' precisa ser preenchida!";
            }

            if (sigla == "int")
            {
                if (!int.TryParse(valor, out int valorRetorno))
                    retorno = $"Propriedade '{descricao}' está inválida!";
            }

            return retorno;
        }

        public async Task<List<TcfgDataType>> BuscarDataTypes()
        {
            return await Task.FromResult(_bll.ObterDataTypesPorFiltro(new TcfgDataTypeFiltro()));
        }

        public async Task<List<TcfgTipoPropriedadeProdutoCatalogo>> BuscarTipoPropriedades()
        {
            return await Task.FromResult(_bll.ObterTipoPropriedadesPorFiltro(new TcfgTipoPropriedadeProdutoCatalogoFiltro()));
        }

        public async Task<string> GravarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            var validacao = await ValidarPropriedade(produtoCatalogoPropriedade);
            if (!string.IsNullOrEmpty(validacao)) return validacao;

            _logger.LogInformation($"GravarPropriedadesProdutos: Cadastrando propriedade.");
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var tProdutoCatalogoPropriedade = await _produtoGeralBll.GravarPropriedadeComTransacao(produtoCatalogoPropriedade, dbGravacao);
                if (tProdutoCatalogoPropriedade.id == 0) return "Falha ao gravar propriedade!";

                if (produtoCatalogoPropriedade.IdCfgTipoPropriedade == 1)
                {
                    int index = 1;
                    foreach (var opcao in produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados)
                    {
                        opcao.id_produto_catalogo_propriedade = tProdutoCatalogoPropriedade.id;
                        opcao.ordem = index;
                        var tProdutoCatalogoPropriedadeOpcao = await _produtoGeralBll.GravarPropriedadeOpcaoComTransacao(opcao, dbGravacao);
                        if (tProdutoCatalogoPropriedadeOpcao.id == 0) return "Falha ao gravar opção da propriedade";
                        index++;
                    }
                }

                dbGravacao.transacao.Commit();
                _logger.LogInformation($"GravarPropriedadesProdutos: Finalizando cadastro de propriedade.");
            }

            return null;
        }

        private async Task<string> ValidarPropriedade(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            if (produtoCatalogoPropriedade == null) return "Dados inválidos!";
            if (string.IsNullOrEmpty(produtoCatalogoPropriedade.descricao)) return "Descrição da propriedade inválido!";

            _logger.LogInformation($"ValidarPropriedade: Buscando lista de DataTypes.");
            var lstTcfgDataType = await BuscarDataTypes();
            _logger.LogInformation($"ValidarPropriedade: Retorno da lista de DataTypes. Retorno => [{JsonSerializer.Serialize(lstTcfgDataType)}]");

            _logger.LogInformation($"ValidarPropriedade: Buscando lista de tipos de propriedades.");
            var lstTcfgTipoPropriedadeProdutoCatalogo = await BuscarTipoPropriedades();
            _logger.LogInformation($"ValidarPropriedade: Retorno da lista de tipos de propriedades. Retorno => [{JsonSerializer.Serialize(lstTcfgTipoPropriedadeProdutoCatalogo)}]");

            if (lstTcfgDataType == null || lstTcfgTipoPropriedadeProdutoCatalogo == null) return "Falha ao buscar dados para validação!";

            _logger.LogInformation($"ValidarPropriedade: Filtrando lista de DataTypes.");
            var dataType = lstTcfgDataType.Where(x => x.Id == produtoCatalogoPropriedade.IdCfgDataType).FirstOrDefault();
            if (dataType == null) return "Falha ao buscar tipos válidos para validação da propriedade!";
            _logger.LogInformation($"ValidarPropriedade: DataType filtrado. Retorno => [{JsonSerializer.Serialize(dataType)}]");

            _logger.LogInformation($"ValidarPropriedade: Filtrando lista de tipo de propriedades.");
            var tipo = lstTcfgTipoPropriedadeProdutoCatalogo.Where(x => x.Id == produtoCatalogoPropriedade.IdCfgTipoPropriedade).FirstOrDefault();
            if (tipo == null) return "Falha ao buscar tipo da propriedade!";
            _logger.LogInformation($"ValidarPropriedade: tipo de propriedade filtrado. Retorno => [{JsonSerializer.Serialize(tipo)}]");

            _logger.LogInformation($"ValidarPropriedade: Verificando tipo de propriedade e lista de opcoes.");
            if (produtoCatalogoPropriedade.IdCfgTipoPropriedade == 0)
            {
                if (produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados != null)
                    return "Se a propriedade é de preenchimento livre, não deve conter lista de valores válidos!";
            }
            if (produtoCatalogoPropriedade.IdCfgTipoPropriedade == 1)
            {
                if (produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados.Count() == 0)
                    return "Se a propriedade é de valores limitados a opções pré-definidas, é necessário informar uma lista de valores válidos!";

                _logger.LogInformation($"ValidarPropriedade: Validando tipo da propriedade da opcao.");
                foreach (var prop in produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados)
                {
                    var retorno = ValidarTipoPropriedade(prop.valor, produtoCatalogoPropriedade.descricao, dataType.Sigla);
                    if (!string.IsNullOrEmpty(retorno)) return retorno;
                }
            }

            return null;
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesProdutos(int id)
        {
            var lstPropriedades = await _produtoGeralBll.ObterListaPropriedadesProdutos(id);

            if (lstPropriedades != null && lstPropriedades.Count > 0)
            {
                if (lstPropriedades[0].IdCfgTipoPropriedade == 1)
                {
                    var lstPropriedadeOpcoes = await _produtoGeralBll.ObterListaPropriedadesOpcoes(id);
                    if (lstPropriedadeOpcoes != null)
                    {
                        lstPropriedades[0].produtoCatalogoPropriedadeOpcoesDados = new List<Produto.Dados.ProdutoCatalogoPropriedadeOpcoesDados>();
                        lstPropriedades[0].produtoCatalogoPropriedadeOpcoesDados = lstPropriedadeOpcoes.OrderBy(x => x.ordem).ToList();
                    }
                }
            }

            return lstPropriedades;
        }

        public async Task<ProdutoCatalogoPropriedadeResponseViewModel> AtualizarPropriedadesProdutos(
            Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            var retorno = new ProdutoCatalogoPropriedadeResponseViewModel();
            retorno.Sucesso = false;

            _logger.LogInformation($"AtualizarPropriedadesProdutos: Validando dados da propriedade.");
            var validacao = await ValidarPropriedade(produtoCatalogoPropriedade);

            if (!string.IsNullOrEmpty(validacao))
            {
                retorno.Mensagem = validacao;
                return retorno;
            }

            _logger.LogInformation($"AtualizarPropriedadesProdutos: Obter propriedade para comparacao.");
            var prodPropriedadesParaComparacao = (await ObterListaPropriedadesProdutos(produtoCatalogoPropriedade.id)).FirstOrDefault();
            _logger.LogInformation($"AtualizarPropriedadesProdutos: Retorno da propriedade para comparacao. Retorno => [{JsonSerializer.Serialize(prodPropriedadesParaComparacao)}].");

            _logger.LogInformation($"AtualizarPropriedadesProdutos: Verificando regras para atualiza propriedade.");
            retorno = await VerificarRegraEdicaoPropriedadesProdutosOpcao(produtoCatalogoPropriedade, prodPropriedadesParaComparacao);
            if (!string.IsNullOrEmpty(retorno.Mensagem) || retorno.ProdutosCatalogo != null) return retorno;

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                _logger.LogInformation($"AtualizarPropriedadesProdutos: Atualizando propriedade.");
                var tProdutoCatalogoPropriedade = await _produtoGeralBll.AtualizarPropriedadeComTransacao(produtoCatalogoPropriedade, dbGravacao);

                if (produtoCatalogoPropriedade.IdCfgTipoPropriedade == 1)
                {
                    _logger.LogInformation($"AtualizarPropriedadesProdutos: Verificando regra para remover opcoes da propriedade.");

                    _logger.LogInformation($"AtualizarPropriedadesProdutos: Atualizando opcoes da propriedade.");
                    retorno = await AtualizarPropriedadesProdutosOpcao(produtoCatalogoPropriedade, prodPropriedadesParaComparacao, tProdutoCatalogoPropriedade.id, dbGravacao);
                    if (!string.IsNullOrEmpty(retorno.Mensagem)) return retorno;
                }

                await dbGravacao.SaveChangesAsync();
                dbGravacao.transacao.Commit();
            }

            retorno.Sucesso = true;

            return retorno;
        }

        public async Task<ProdutoCatalogoPropriedadeResponseViewModel> AtualizarPropriedadesProdutosOpcao(
            Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade,
            Produto.Dados.ProdutoCatalogoPropriedadeDados prodPropriedadesParaComparacao,
            int idPropriedade,
            InfraBanco.ContextoBdGravacao dbGravacao)
        {
            var retorno = new ProdutoCatalogoPropriedadeResponseViewModel();
            retorno.Sucesso = false;

            if (produtoCatalogoPropriedade.IdCfgTipoPropriedade == 1 &&
                    prodPropriedadesParaComparacao.produtoCatalogoPropriedadeOpcoesDados == null)
            {
                retorno.Mensagem = "Falha ao buscar opções da propriedade para comparação!";
                return retorno;
            }

            foreach (var prop in prodPropriedadesParaComparacao.produtoCatalogoPropriedadeOpcoesDados)
            {
                var comparar = produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados
                    .Where(x => x.id == prop.id).FirstOrDefault();

                if (comparar == null)
                {
                    _logger.LogInformation($"AtualizarPropriedadesProdutosOpcao: Removendo opção da propriedade. Opcao => [{JsonSerializer.Serialize(prop)}]");
                    await _produtoGeralBll.RemoverPropriedadeOpcaoComTransacao(prop, dbGravacao);
                }
            }

            int index = 1;
            foreach (var prop in produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados)
            {
                var existe = prodPropriedadesParaComparacao.produtoCatalogoPropriedadeOpcoesDados
                    .Where(x => x.id == prop.id).FirstOrDefault();
                prop.ordem = index;
                if (existe == null)
                {
                    _logger.LogInformation($"AtualizarPropriedadesProdutosOpcao: Inserido nova opcao da propriedade. Opcao => [{JsonSerializer.Serialize(prop)}]");
                    prop.id_produto_catalogo_propriedade = idPropriedade;
                    var tProdutoCatalogoPropriedadeOpcao = await _produtoGeralBll.GravarPropriedadeOpcaoComTransacao(prop, dbGravacao);
                    if (idPropriedade == 0)
                    {
                        retorno.Mensagem = "Falha ao gravar opção da propriedade";
                        return retorno;
                    }
                }

                if (existe != null)
                {
                    if (existe.oculto != prop.oculto || existe.valor != prop.valor || existe.ordem != prop.ordem)
                    {
                        _logger.LogInformation($"AtualizarPropriedadesProdutosOpcao: Atualizando opcao da propriedade. Opcao => [{JsonSerializer.Serialize(prop)}]");
                        var tProproedadeOpcao = await _produtoGeralBll.AtualizarPropriedadeOpcaoComTransacao(prop, dbGravacao);
                        if (tProproedadeOpcao.id == 0)
                        {
                            retorno.Mensagem = "Falha ao atualizar opção da propriedade";
                            return retorno;
                        }
                    }
                }
                index++;
            }

            return retorno;
        }

        private async Task<ProdutoCatalogoPropriedadeResponseViewModel> VerificarRegraEdicaoPropriedadesProdutosOpcao(
            Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade,
            Produto.Dados.ProdutoCatalogoPropriedadeDados prodPropriedadesParaComparacao)
        {
            var retorno = new ProdutoCatalogoPropriedadeResponseViewModel();
            retorno.Sucesso = false;

            var tiposEdicaoPermissaoCadastro = _bll.ObterTipoPermissaoEdicaoCadastro();
            if (tiposEdicaoPermissaoCadastro == null)
            {
                retorno.Mensagem = "Falha ao buscar a lista de tipos de permissão para edição de cadastro";
                return retorno;
            }

            //id == 1 => Sistema
            var tipoPermissaoEdicaoCadastroUsuario = tiposEdicaoPermissaoCadastro.Where(x => x.Id == 1).FirstOrDefault();
            if (tipoPermissaoEdicaoCadastroUsuario == null)
            {
                retorno.Mensagem = "Ops! Não encontramos a informação do tipo de permissão para edição de cadastro!";
                return retorno;
            }

            if (produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados != null)
            {
                if (produtoCatalogoPropriedade.id <= 10000 &&
                produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados.Count != prodPropriedadesParaComparacao.produtoCatalogoPropriedadeOpcoesDados.Count &&
                produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro == tipoPermissaoEdicaoCadastroUsuario.Id)
                {
                    retorno.Mensagem = $"Não é permitido alterar a lista de valores válidos!";
                    return retorno;
                }

                int index = 1;
                foreach (var prop in produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados)
                {
                    if (produtoCatalogoPropriedade.id <= 10000 &&
                                produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro == tipoPermissaoEdicaoCadastroUsuario.Id && prop.id == 0)
                    {
                        retorno.Mensagem = $"Não é permitido inserir um novo valor válido para essa propriedade!";
                        return retorno;
                    }

                    prop.ordem = index;
                    index++;
                }

                foreach (var prop in prodPropriedadesParaComparacao.produtoCatalogoPropriedadeOpcoesDados)
                {
                    var comparar = produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados
                            .Where(x => x.id == prop.id).FirstOrDefault();

                    if (comparar != null)
                    {
                        //verifica se foi editado
                        if (comparar.oculto != prop.oculto || comparar.valor != prop.valor || comparar.ordem != prop.ordem)
                        {
                            //não pode editar opção
                            if (produtoCatalogoPropriedade.id <= 10000 &&
                                produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro == tipoPermissaoEdicaoCadastroUsuario.Id)
                            {
                                retorno.Mensagem = $"A opção '{prop.valor}' não pode ser editada!";
                                return retorno;
                            }
                        }
                    }

                    if (comparar == null)
                    {
                        if (produtoCatalogoPropriedade.id <= 10000 &&
                                produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro == tipoPermissaoEdicaoCadastroUsuario.Id)
                        {
                            if (produtoCatalogoPropriedade.id <= 10000 &&
                                produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro == tipoPermissaoEdicaoCadastroUsuario.Id)
                            {
                                retorno.Mensagem = $"Não é permitido remover valor válido para essa propriedade!";
                                return retorno;
                            }
                        }
                        // não pode remover se tem opção sendo utilizada por produtos
                        _logger.LogInformation($"VerificarRegraRemoverPropriedadesProdutosOpcao: Buscando produtos do catálogo que utilizam a opcao da propriedade que sera removida.");
                        var produtos = await _produtoGeralBll.BuscarProdutosCatalogoPorPropriedadeOpcao(produtoCatalogoPropriedade.id, prop.id);

                        if (produtos.Count > 0)
                        {
                            _logger.LogInformation($"VerificarRegraRemoverPropriedadesProdutosOpcao: retorno da lista de " +
                                $"produtos do catálogo que utilizam a opcao da propriedade que será removida. Retorno => [{JsonSerializer.Serialize(produtos)}].");

                            retorno.ProdutosCatalogo = new List<TprodutoCatalogo>();
                            retorno.ProdutosCatalogo = produtos;
                            return retorno;
                        }
                    }
                }
            }

            return retorno;
        }
    }
}
