﻿using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request.ProdutoCatalogo;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.ProdutoCatalogo;
using Produto;
using ProdutoCatalogo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private readonly Cfg.CfgOperacao.CfgOperacaoBll _cfgOperacaoBll;

        public ProdutoCatalogoOrcamentoCotacaoBll(
            ProdutoCatalogoBll bll,
            InfraBanco.ContextoBdProvider contextoBdProvider,
            ProdutoGeralBll produtoGeralBll,
            ILogger<ProdutoCatalogoOrcamentoCotacaoBll> logger,
            Cfg.CfgOperacao.CfgOperacaoBll cfgOperacaoBll)
        {
            _bll = bll;
            _contextoBdProvider = contextoBdProvider;
            _produtoGeralBll = produtoGeralBll;
            _logger = logger;
            _cfgOperacaoBll = cfgOperacaoBll;
        }

        public List<TprodutoCatalogo> PorFiltro(TprodutoCatalogoFiltro filtro)
        {
            return _bll.PorFiltro(filtro);
        }

        public ListaProdutoCatalogoListarResponse ListarProdutoCatalogo(ProdutoCatalogoListarRequest request)
        {
            var response = new ListaProdutoCatalogoListarResponse();
            response.Sucesso = false;
            response.ListaProdutoCatalogoResponse = new List<ProdutoCatalogoListarResponse>();

            var produtoCatalogoListarDto = _bll.ListarProdutoCatalogo(
                                request.FabricantesSelecionados,
                                request.CodAlfaNumFabricanteSelecionado,
                                request.DescargaCondensadoraSelecionado,
                                request.VoltagemSelecionadas,
                                request.CapacidadeSelecionadas,
                                request.CicloSelecionado,
                                request.TipoUnidadeSelecionado,
                                request.ImagemSelecionado,
                                request.AtivoSelecionado,
                                request.Pagina,
                                request.QtdeItensPorPagina,
                                request.NomeColunaOrdenacao);

            response.QtdeRegistros = produtoCatalogoListarDto.Count;

            if (request.QtdeItensPorPagina != 0)
            {
                produtoCatalogoListarDto = produtoCatalogoListarDto.Skip(request.Pagina * request.QtdeItensPorPagina).Take(request.QtdeItensPorPagina).ToList();
            }

            foreach (var produtoCatalogoItem in produtoCatalogoListarDto)
            {
                response.ListaProdutoCatalogoResponse.Add(new ProdutoCatalogoListarResponse()
                {
                    Ativo = produtoCatalogoItem.Ativo,
                    Capacidade = produtoCatalogoItem.Capacidade,
                    Ciclo = produtoCatalogoItem.Ciclo,
                    CodAlfanumericoFabricante = produtoCatalogoItem.CodAlfanumericoFabricante,
                    Codigo = produtoCatalogoItem.Codigo,
                    CodigoFabricante = produtoCatalogoItem.CodigoFabricante,
                    DescargaCondensadora = produtoCatalogoItem.DescargaCondensadora,
                    DescricaoCompleta = produtoCatalogoItem.DescricaoCompleta,
                    Fabricante = produtoCatalogoItem.Fabricante,
                    Id = produtoCatalogoItem.Id,
                    IdCapacidade = produtoCatalogoItem.IdCapacidade,
                    IdCiclo = produtoCatalogoItem.IdCiclo,
                    IdDescargaCondensadora = produtoCatalogoItem.IdDescargaCondensadora,
                    IdTipoUnidade = produtoCatalogoItem.IdTipoUnidade,
                    IdVoltagem = produtoCatalogoItem.IdVoltagem,
                    Imagem = produtoCatalogoItem.Imagem,
                    TipoUnidade = produtoCatalogoItem.TipoUnidade,
                    Voltagem = produtoCatalogoItem.Voltagem
                });
            }

            if (response.ListaProdutoCatalogoResponse.Count <= 0) response.QtdeRegistros = 0;

            response.Sucesso = true;
            return response;

        }

        public string Excluir(int id, string caminho)
        {
            var tProdutoCatalogoAntigo = _bll.BuscarTprodutoCatalogo(new TprodutoCatalogoFiltro() { Id = id.ToString(), IncluirImagem = true, IncluirPropriedades = true }).FirstOrDefault();
            if (tProdutoCatalogoAntigo == null)
            {
                return "Produto não encontrado!";
            }

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                if (tProdutoCatalogoAntigo.imagem != null)
                {
                    var file = Path.Combine(caminho, tProdutoCatalogoAntigo.imagem.Caminho);
                    if (!File.Exists(file))
                        return "Ops! O arquivo não foi encontrado no diretório!";

                    File.Delete(file);

                    if (File.Exists(file)) return "Falha ao deletar arquivo do diretório!";

                    if (!_bll.ExcluirImagemComTransacao(id, tProdutoCatalogoAntigo.imagem.Id, dbGravacao)) return "Falha ao excluir a imagem do produto!";

                }

                if (!_bll.ExcluirItensComTransacao(tProdutoCatalogoAntigo.campos, dbGravacao))
                {
                    return "Falha ao excluir propriedades do produto.";
                };

                var remover = _bll.ExcluirProdutoCatalogoComTransacao(id, dbGravacao);

                dbGravacao.SaveChanges();
                dbGravacao.transacao.Commit();
            }

            return null;
        }

        public TprodutoCatalogo Detalhes(int id)
        {
            return _bll.Detalhes(id);
        }

        public ConsultaProdutoCatalogoAtivoResponse ConsultarProdutoCatalogoAtivo(ConsultaProdutoCatalogoAtivoRequest request)
        {
            var response = new ConsultaProdutoCatalogoAtivoResponse();
            response.Sucesso = false;

            var retorno = _bll.ConsultarProdutoCatalogoAtivo(request.Id);
            if (retorno == null)
            {
                response.Mensagem = "Falha ao buscar produto!";
                return response;
            }

            var tProduto = (TprodutoCatalogo)retorno.GetType().GetProperty("produto").GetValue(retorno, null);
            response.Fabricante = tProduto.Fabricante;
            response.FabricanteNome = tProduto.Tfabricante.Nome;
            response.Produto = tProduto.Produto;
            response.ProdutoNome = tProduto.Nome;
            response.ProdutoDescricaoCompleta = tProduto.Descricao;

            var propriedades = _bll.BuscarPropriedadesProdutoCatalogoAtivo(request.Id).ToList();
            if (propriedades != null)
            {
                response.ListPropriedades = new List<ConsultaProdutoCatalogoPropriedadeResponse>();
                foreach (var prop in propriedades)
                {
                    var propResponse = new ConsultaProdutoCatalogoPropriedadeResponse();
                    propResponse.Nome = (string)prop.GetType().GetProperty("nome").GetValue(prop, null);
                    var valorTexto = (string)prop.GetType().GetProperty("valorTexto").GetValue(prop, null);
                    if (!string.IsNullOrEmpty(valorTexto))
                    {
                        propResponse.Valor = valorTexto;
                    }
                    else
                    {
                        propResponse.Valor = (string)prop.GetType().GetProperty("valorLista").GetValue(prop, null);
                    }

                    response.ListPropriedades.Add(propResponse);
                }
            }

            var imagem = _bll.ObterListaImagensPorId(request.Id).FirstOrDefault();
            response.ImagemCaminho = imagem != null ? imagem.Caminho : "sem-imagem.png";

            response.Sucesso = true;
            return response;
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

        public async Task<string> Atualizar(TprodutoCatalogo produtoCatalogo, IFormFile arquivo, string caminho,
            string ip, UsuarioLogin usuarioLogin)
        {
            var retornoValidacao = await ValidarTiposPropriedadesProdutoCatalogo(produtoCatalogo.campos);

            if (!string.IsNullOrEmpty(retornoValidacao)) return retornoValidacao;

            var tProdutoCatalogoAntigo = _bll.BuscarTprodutoCatalogo(new TprodutoCatalogoFiltro() { Id = produtoCatalogo.Id.ToString(), IncluirImagem = true, IncluirPropriedades = true }).FirstOrDefault();

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    if (produtoCatalogo == null)
                        return "Ops! Parece que não existe dados de produto para catálogo!";


                    var tProdutoCatalogo = _bll.AtualizarComTransacao(produtoCatalogo, dbGravacao);

                    if (tProdutoCatalogo == null)
                        return "Ops! Erro ao atualizar produto!";

                    if (produtoCatalogo.campos == null || produtoCatalogo.campos.Count == 0)
                        return "Ops! As propriedades do produto não pode estar vazio!";

                    string log = "";
                    log = UtilsGlobais.Util.MontalogComparacao(tProdutoCatalogo, tProdutoCatalogoAntigo, log, "");
                    if (!string.IsNullOrEmpty(log)) log = $"Produto: produto={tProdutoCatalogo.Produto}; id={tProdutoCatalogo.Id}; {log}";

                    tProdutoCatalogo.campos = _bll.AtualizarItensComTransacao(produtoCatalogo.campos, tProdutoCatalogo.Id,
                        dbGravacao);
                    if (tProdutoCatalogo.campos == null || tProdutoCatalogo.campos.Count == 0)
                        return "Falha ao atualizar as propriedades do produto catálogo!";

                    //log = log + "\r";
                    string logPropriedades = "";

                    foreach (var prop in tProdutoCatalogo.campos)
                    {
                        string camposAOmitir = "|id_produto_catalogo|";
                        string logApoio = "";
                        var propAntiga = tProdutoCatalogoAntigo.campos
                            .Where(x => x.IdProdutoCatalogoPropriedade == prop.IdProdutoCatalogoPropriedade).FirstOrDefault();
                        if (propAntiga == null)
                            logApoio = UtilsGlobais.Util.MontaLog(prop, logApoio, camposAOmitir);
                        else
                        {
                            string logVerificacao = "";
                            logVerificacao = UtilsGlobais.Util.MontalogComparacao(prop, propAntiga, logVerificacao, camposAOmitir);
                            if (!string.IsNullOrEmpty(logVerificacao)) logApoio += $"id_produto_catalogo_propriedade={prop.IdProdutoCatalogoPropriedade}; {logVerificacao}";
                        }

                        if (!string.IsNullOrEmpty(logApoio))
                            logPropriedades = $"{logPropriedades}\r {logApoio}";
                    }
                    if (!string.IsNullOrEmpty(logPropriedades)) log = $"{log}\rLista de propriedades incluídas/alteradas: {logPropriedades}";

                    var logExclusaoPropriedade = "";
                    foreach (var propAntiga in tProdutoCatalogoAntigo.campos)
                    {
                        var lstPropriedades = await _produtoGeralBll.ObterListaPropriedadesProdutos();
                        var lstPropriedadesOpcoes = await _produtoGeralBll.ObterListaPropriedadesOpcoes();

                        var propriedade = lstPropriedades.Where(x => x.id == propAntiga.IdProdutoCatalogoPropriedade).FirstOrDefault();
                        var propriedadeOpcao = lstPropriedadesOpcoes.Where(x => x.id_produto_catalogo_propriedade == propAntiga.IdProdutoCatalogoPropriedade && x.id == propAntiga.IdProdutoCatalogoPropriedadeOpcao).FirstOrDefault();
                        propAntiga.TprodutoCatalogoPropriedade = new TProdutoCatalogoPropriedade();

                        var propNovo = tProdutoCatalogo.campos
                            .Where(x => x.IdProdutoCatalogoPropriedade == propAntiga.IdProdutoCatalogoPropriedade).FirstOrDefault();
                        if (propNovo == null)
                        {
                            propNovo = new TprodutoCatalogoItem();
                            propNovo.IdProdutoCatalogo = propAntiga.IdProdutoCatalogo;
                            string logApoio = $"id={propAntiga.IdProdutoCatalogoPropriedade}; nome={propriedade.descricao}; valor={propAntiga.Valor}; ";
                            logExclusaoPropriedade = $"{logExclusaoPropriedade}\r {logApoio}";
                        }

                    }

                    if (!string.IsNullOrEmpty(logExclusaoPropriedade)) log = $"{log}\rLista de propriedades excluídas: {logExclusaoPropriedade}";

                    if (arquivo != null)
                    {
                        var retorno = await CriarImagemComTransacao(arquivo, produtoCatalogo.imagem, caminho, tProdutoCatalogo.Id,
                            dbGravacao);
                        if (!retorno.Sucesso)
                        {
                            dbGravacao.transacao.Rollback();
                            return retorno.Mensagem;
                        }

                        var logImagem = "\nImagem: ";

                        string camposAOmitir = "|ordem|";
                        logImagem = UtilsGlobais.Util.MontaLog(retorno.TprodutoCatalogoImagem, logImagem, camposAOmitir);
                        log = log + logImagem;
                    }

                    var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = 8 }, dbGravacao).FirstOrDefault();
                    if (cfgOperacao == null)
                    {
                        return "Ops! Falha ao editar produto.";
                    }
                    var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogin.TipoUsuario, usuarioLogin.Id, null, null, null, null,
                        InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);

                    dbGravacao.transacao.Commit();
                    return null;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public async Task<CadastroProdutoCatalogoResponse> Criar(
            TprodutoCatalogo produtoCatalogo1,
            string usuario_cadastro,
            IFormFile arquivo,
            string caminho, UsuarioLogin usuarioLogin, string ip)
        {
            var response = new CadastroProdutoCatalogoResponse();
            response.Sucesso = false;

            var retornoValidacao = await ValidarTiposPropriedadesProdutoCatalogo(produtoCatalogo1.campos);

            if (!string.IsNullOrEmpty(retornoValidacao))
            {
                response.Mensagem = retornoValidacao;
                return response;
            }

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                if (produtoCatalogo1 == null)
                {
                    response.Mensagem = "Ops! Parece que não existe dados de produto para catálogo!";
                    return response;
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
                    response.Mensagem = "Ops! Erro ao criar novo produto!";
                    return response;
                }
                string log = "";
                string camposAOmitir = "|usuario_cadastro|usuario_edicao|dt_cadastro|dt_edicao|";

                log = UtilsGlobais.Util.MontaLog(prod, log, camposAOmitir);
                log = $"Produto: {log}";

                var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = 7 }, dbGravacao).FirstOrDefault();
                if (cfgOperacao == null)
                {
                    response.Mensagem = "Ops! Falha ao cadastrar produto.";
                    return response;
                }

                if (produtoCatalogo1.campos == null
                    || produtoCatalogo1.campos.Count == 0)
                {
                    response.Mensagem = "Ops! As propriedades do produto não pode estar vazio!";
                    return response;
                }

                tProdutoCatalogo.campos = _bll.CriarItensComTransacao(
                    produtoCatalogo1.campos,
                    tProdutoCatalogo.Id,
                    dbGravacao);

                log = log + "\r";
                string logProdutos = "";
                foreach (var prop in tProdutoCatalogo.campos)
                {
                    string propriedadesAOmitir = "|id_produto_catalogo|";
                    logProdutos = UtilsGlobais.Util.MontaLog(prop, logProdutos, propriedadesAOmitir);
                    logProdutos = logProdutos + "\r";
                }

                log = $"{log}Lista de propriedades: {logProdutos}";

                if (produtoCatalogo1.imagem != null)
                {
                    var retorno = await CriarImagemComTransacao(
                        arquivo,
                        produtoCatalogo1.imagem,
                        caminho,
                        tProdutoCatalogo.Id,
                        dbGravacao);

                    if (!retorno.Sucesso)
                    {
                        dbGravacao.transacao.Rollback();
                        response.Mensagem = retorno.Mensagem;
                        return response;
                    }

                    var logImagem = "Imagem: ";
                    logImagem = UtilsGlobais.Util.MontaLog(retorno.TprodutoCatalogoImagem, logImagem, "");
                    log = log + logImagem;
                }

                var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogin.TipoUsuario, usuarioLogin.Id, null, null, null, null,
                    InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);

                dbGravacao.transacao.Commit();
            }

            response.Sucesso = true;
            return response;
        }

        public async Task<CadastroProdutoCatalogoImagemResponse> CriarImagemComTransacao(
            IFormFile arquivo,
            TprodutoCatalogoImagem produtoCatalogoImagem,
            string caminho,
            int idProdutoCatalogo,
            InfraBanco.ContextoBdGravacao dbGravacao)
        {
            var response = new CadastroProdutoCatalogoImagemResponse();
            response.Sucesso = false;

            if (arquivo == null)
            {
                var nomeArquivoCopia = produtoCatalogoImagem.Caminho;
                var extensaoCopia = produtoCatalogoImagem.Caminho.Split(".")[1];

                var novoNomeArquivo = CriarNomeArquivo(extensaoCopia);

                if (string.IsNullOrEmpty(novoNomeArquivo))
                {
                    response.Mensagem = "Falha ao gerar nome do arquivo!";
                    return response;
                }

                produtoCatalogoImagem.Caminho = novoNomeArquivo;

                response.TprodutoCatalogoImagem = _bll.CriarImagensComTransacao(
                    produtoCatalogoImagem,
                    idProdutoCatalogo, dbGravacao);

                if (response.TprodutoCatalogoImagem == null)
                {
                    response.Mensagem = "Ops! Erro ao salvar dados da imagem!";
                    return response;
                }

                var arquivoABuscar = Path.Combine(caminho, nomeArquivoCopia);

                var retorno = await ClonarImagemDiretorio(arquivoABuscar, novoNomeArquivo, caminho);

                if (!string.IsNullOrEmpty(retorno))
                {
                    response.Mensagem = retorno;
                    return response;
                }

                response.Sucesso = true;
                return response;
            }
            else
            {
                var extensao = arquivo.FileName.Substring(arquivo.FileName.Length - 4, 4).Replace(".", "").Trim();

                var nomeArquivo = CriarNomeArquivo(extensao);

                if (string.IsNullOrEmpty(nomeArquivo))
                {
                    response.Mensagem = "Falha ao gerar nome do arquivo!";
                    return response;
                }

                produtoCatalogoImagem.Caminho = nomeArquivo;

                response.TprodutoCatalogoImagem = _bll.CriarImagensComTransacao(
                    produtoCatalogoImagem,
                    idProdutoCatalogo,
                    dbGravacao);

                if (response.TprodutoCatalogoImagem == null)
                {
                    response.Mensagem = "Ops! Erro ao salvar dados da imagem!";
                    return response;
                }

                var retorno = await InserirImagemDiretorio(arquivo, caminho, nomeArquivo);

                if (!string.IsNullOrEmpty(retorno))
                {
                    response.Mensagem = retorno;
                    return response;
                }

                response.Sucesso = true;
                return response;
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

        public async Task<string> GravarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade,
            UsuarioLogin usuarioLogado, string ip)
        {
            var validacao = await ValidarPropriedade(produtoCatalogoPropriedade);
            if (!string.IsNullOrEmpty(validacao)) return validacao;

            _logger.LogInformation($"GravarPropriedadesProdutos: Cadastrando propriedade.");
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var tProdutoCatalogoPropriedade = await _produtoGeralBll.GravarPropriedadeComTransacao(produtoCatalogoPropriedade, dbGravacao);
                if (tProdutoCatalogoPropriedade.id == 0) return "Falha ao gravar propriedade!";

                string logOpcoes = "";
                string omitirCampos = "";
                if (produtoCatalogoPropriedade.IdCfgTipoPropriedade == 1)
                {
                    omitirCampos = "|dt_cadastro|usuario_cadastro|";
                    int index = 100;
                    foreach (var opcao in produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados)
                    {
                        opcao.id_produto_catalogo_propriedade = tProdutoCatalogoPropriedade.id;
                        opcao.ordem = index;
                        var tProdutoCatalogoPropriedadeOpcao = await _produtoGeralBll.GravarPropriedadeOpcaoComTransacao(opcao, dbGravacao);
                        if (tProdutoCatalogoPropriedadeOpcao.id == 0) return "Falha ao gravar opção da propriedade";
                        index = index + 100;

                        if (!string.IsNullOrEmpty(logOpcoes)) logOpcoes += "\r   ";
                        logOpcoes = UtilsGlobais.Util.MontaLog(tProdutoCatalogoPropriedadeOpcao, logOpcoes, omitirCampos);
                    }
                }

                string logPropriedade = "";
                omitirCampos = "|dt_cadastro|usuario_cadastro|";
                logPropriedade = UtilsGlobais.Util.MontaLog(tProdutoCatalogoPropriedade, logPropriedade, omitirCampos);

                var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = 9 }, dbGravacao).FirstOrDefault();
                if (cfgOperacao == null)
                {
                    return "Ops! Falha ao cadastrar propriedade.";
                }

                string log = $"Propriedade: {logPropriedade}";

                if (!string.IsNullOrEmpty(logOpcoes)) log += $"\rLista de valores válidos:\r   {logOpcoes}";

                var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogado.TipoUsuario,
                    usuarioLogado.Id, null, null, null, null,
                    InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);

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
            Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade, UsuarioLogin usuarioLogado, string ip)
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

            var tiposEdicaoPermissaoCadastro = _bll.ObterTipoPermissaoEdicaoCadastro();
            if (tiposEdicaoPermissaoCadastro == null)
            {
                retorno.Mensagem = "Falha ao buscar a lista de tipos de permissão para edição de cadastro";
                return retorno;
            }
            var tipoPermissaoEdicaoCadastroUsuario = tiposEdicaoPermissaoCadastro.Where(x => x.Id == 0).FirstOrDefault();
            if (tipoPermissaoEdicaoCadastroUsuario == null)
            {
                retorno.Mensagem = "Ops! Não encontramos a informação do tipo de permissão para edição de cadastro!";
                return retorno;
            }

            _logger.LogInformation($"AtualizarPropriedadesProdutos: Verificando regras para atualiza propriedade.");
            retorno = await VerificarRegraEdicaoPropriedadesProdutosOpcao(produtoCatalogoPropriedade, prodPropriedadesParaComparacao, tipoPermissaoEdicaoCadastroUsuario);
            if (!string.IsNullOrEmpty(retorno.Mensagem) || retorno.ProdutosCatalogo != null) return retorno;

            produtoCatalogoPropriedade.dt_cadastro = prodPropriedadesParaComparacao.dt_cadastro;

            var tProdutoCatalogoPropriedadeAntigo = new TProdutoCatalogoPropriedade()
            {
                id = prodPropriedadesParaComparacao.id,
                IdCfgTipoPropriedade = prodPropriedadesParaComparacao.IdCfgTipoPropriedade,
                IdCfgTipoPermissaoEdicaoCadastro = prodPropriedadesParaComparacao.IdCfgTipoPermissaoEdicaoCadastro,
                IdCfgDataType = prodPropriedadesParaComparacao.IdCfgDataType,
                descricao = prodPropriedadesParaComparacao.descricao,
                oculto = prodPropriedadesParaComparacao.oculto,
                ordem = prodPropriedadesParaComparacao.ordem,
                dt_cadastro = prodPropriedadesParaComparacao.dt_cadastro,
                usuario_cadastro = prodPropriedadesParaComparacao.usuario_cadastro
            };

            string log = $"Propriedade: id={produtoCatalogoPropriedade.id}; ";
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                _logger.LogInformation($"AtualizarPropriedadesProdutos: Atualizando propriedade.");
                var tProdutoCatalogoPropriedade = await _produtoGeralBll.AtualizarPropriedadeComTransacao(produtoCatalogoPropriedade, dbGravacao);

                string logPropriedade = UtilsGlobais.Util.MontalogComparacao(tProdutoCatalogoPropriedade, tProdutoCatalogoPropriedadeAntigo, "", "");
                if (!string.IsNullOrEmpty(logPropriedade)) log += $"{logPropriedade}";

                if (produtoCatalogoPropriedade.IdCfgTipoPropriedade == 1 &&
                    produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro == tipoPermissaoEdicaoCadastroUsuario.Id)
                {
                    _logger.LogInformation($"AtualizarPropriedadesProdutos: Atualizando opcoes da propriedade.");
                    retorno = await AtualizarPropriedadesProdutosOpcao(produtoCatalogoPropriedade, prodPropriedadesParaComparacao, tProdutoCatalogoPropriedade.id, dbGravacao);
                    if (!string.IsNullOrEmpty(retorno.Mensagem)) return retorno;

                    if (!string.IsNullOrEmpty(retorno.LogRetorno)) log += $"\r{retorno.LogRetorno}";
                }

                var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = 10 }, dbGravacao).FirstOrDefault();
                if (cfgOperacao == null)
                {
                    retorno.Mensagem = "Ops! Falha ao atualizar propriedade.";
                }
                var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogado.TipoUsuario,
                    usuarioLogado.Id, null, null, null, null,
                    InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);

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

            string logRemocao = "";
            string logInclusao = "";
            string logAtualizacao = "";

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
                    if (!string.IsNullOrEmpty(logRemocao)) logRemocao += "\r   ";
                    logRemocao += $"id={prop.id}; id_produto_catalogo_propriedade={prop.id_produto_catalogo_propriedade}; valor={prop.valor}; ";
                }
            }

            int index = 100;
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

                    string omitirCampos = "|dt_cadastro|usuario_cadastro|";
                    if (!string.IsNullOrEmpty(logInclusao)) logInclusao += "\r   ";
                    logInclusao = UtilsGlobais.Util.MontaLog(tProdutoCatalogoPropriedadeOpcao, logInclusao, omitirCampos);
                }

                if (existe != null)
                {
                    if (existe.oculto != prop.oculto || existe.valor != prop.valor || existe.ordem != prop.ordem)
                    {
                        _logger.LogInformation($"AtualizarPropriedadesProdutosOpcao: Atualizando opcao da propriedade. Opcao => [{JsonSerializer.Serialize(prop)}]");
                        var tPropriedadeOpcao = await _produtoGeralBll.AtualizarPropriedadeOpcaoComTransacao(prop, dbGravacao);
                        if (tPropriedadeOpcao.id == 0)
                        {
                            retorno.Mensagem = "Falha ao atualizar opção da propriedade";
                            return retorno;
                        }

                        var propAntiga = new TProdutoCatalogoPropriedadeOpcao();
                        propAntiga.id = prop.id;
                        propAntiga.id_produto_catalogo_propriedade = existe.id_produto_catalogo_propriedade;
                        propAntiga.valor = existe.valor;
                        propAntiga.oculto = existe.oculto;
                        propAntiga.ordem = existe.ordem;
                        propAntiga.dt_cadastro = existe.dt_cadastro;
                        propAntiga.usuario_cadastro = existe.usuario_cadastro;

                        if (!string.IsNullOrEmpty(logAtualizacao)) logAtualizacao += "\r   ";
                        string logApoio = UtilsGlobais.Util.MontalogComparacao(tPropriedadeOpcao, propAntiga, "", "");
                        if (!string.IsNullOrEmpty(logApoio)) logAtualizacao += $"id={propAntiga.id}; {logApoio}";

                    }
                }
                index = index + 100;
            }

            if (!string.IsNullOrEmpty(logAtualizacao)) retorno.LogRetorno = $"Lista de valores válidos atualizados: {logAtualizacao}";
            if (!string.IsNullOrEmpty(logInclusao)) retorno.LogRetorno += $"\rLista de valores válidos inseridos: {logInclusao}";
            if (!string.IsNullOrEmpty(logRemocao)) retorno.LogRetorno += $"\rLista de valores válidos removidos: {logRemocao}";


            return retorno;
        }

        private async Task<ProdutoCatalogoPropriedadeResponseViewModel> VerificarRegraEdicaoPropriedadesProdutosOpcao(
            Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade,
            Produto.Dados.ProdutoCatalogoPropriedadeDados prodPropriedadesParaComparacao, TcfgTipoPermissaoEdicaoCadastro tipoPermissaoEdicaoCadastroUsuario)
        {
            var retorno = new ProdutoCatalogoPropriedadeResponseViewModel();
            retorno.Sucesso = false;

            //id == 0 => Usuário
            if (produtoCatalogoPropriedade.IdCfgTipoPermissaoEdicaoCadastro == tipoPermissaoEdicaoCadastroUsuario.Id)
            {
                if (produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados != null)
                {
                    foreach (var prop in prodPropriedadesParaComparacao.produtoCatalogoPropriedadeOpcoesDados)
                    {
                        var comparar = produtoCatalogoPropriedade.produtoCatalogoPropriedadeOpcoesDados
                                .Where(x => x.id == prop.id).FirstOrDefault();

                        if (comparar == null)
                        {
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
            }


            return retorno;
        }

        public async Task<bool> ObterPropriedadesUtilizadosPorProdutos(int idPropriedade)
        {
            var propriedadesUtilizadas = await _produtoGeralBll.ObterPropriedadesUtilizadosPorProdutos(idPropriedade);

            return propriedadesUtilizadas;
        }

        public async Task<ProdutoCatalogoPropriedadeResponseViewModel> ExcluirPropriedades(int idPropriedade, UsuarioLogin usuarioLogado, string ip)
        {
            var response = new ProdutoCatalogoPropriedadeResponseViewModel();
            response.Sucesso = false;
            if (idPropriedade <= 10000)
            {
                response.Mensagem = "Essa propriedade não pode ser excluída!";
                return response;
            }

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    string logExclusao = await _produtoGeralBll.ExcluirPropriedades(idPropriedade, dbGravacao);

                    var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = 11 }, dbGravacao).FirstOrDefault();
                    if (cfgOperacao == null)
                    {
                        return response;
                    }

                    string log = $"Propriedade: {logExclusao}";
                    var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogado.TipoUsuario,
                        usuarioLogado.Id, null, null, null, null,
                        InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);

                    dbGravacao.transacao.Commit();

                    response.Sucesso = true;
                    return response;
                }
                catch
                {
                    dbGravacao.transacao.Rollback();
                    return response;
                }
            }
        }
    }
}