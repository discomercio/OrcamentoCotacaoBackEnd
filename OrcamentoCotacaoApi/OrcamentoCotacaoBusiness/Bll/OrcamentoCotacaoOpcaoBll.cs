using AutoMapper;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.Orcamento;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.Orcamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentoCotacaoOpcaoBll
    {
        private readonly ContextoBdProvider contextoBdProvider;
        private readonly OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll;
        private readonly IMapper mapper;
        private readonly ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll;
        private readonly FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll;
        private readonly ILogger<OrcamentoCotacaoOpcaoBll> _logger;

        public OrcamentoCotacaoOpcaoBll(ContextoBdProvider contextoBdProvider,
            OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll, IMapper mapper,
            ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll,
            FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll, ILogger<OrcamentoCotacaoOpcaoBll> _logger)
        {
            this.contextoBdProvider = contextoBdProvider;
            this.orcamentoCotacaoOpcaoBll = orcamentoCotacaoOpcaoBll;
            this.mapper = mapper;
            this.produtoOrcamentoCotacaoBll = produtoOrcamentoCotacaoBll;
            this.formaPagtoOrcamentoCotacaoBll = formaPagtoOrcamentoCotacaoBll;
            this._logger = _logger;
        }

        public CadastroOrcamentoOpcaoResponse CadastrarOrcamentoCotacaoOpcoesComTransacao(List<CadastroOrcamentoOpcaoRequest> orcamentoOpcoes,
            int idOrcamentoCotacao, UsuarioLogin usuarioLogado, ContextoBdGravacao contextoBdGravacao, string loja,
            Guid correlationId)
        {
            var response = new CadastroOrcamentoOpcaoResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();
            _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Iniciando cadastro das opções de orçamento. Opções => [{JsonSerializer.Serialize(orcamentoOpcoes)}].");

            int seq = 0;
            foreach (var opcao in orcamentoOpcoes)
            {
                seq++;
                TorcamentoCotacaoOpcao torcamentoCotacaoOpcao = MontarTorcamentoCotacaoOpcao(opcao, idOrcamentoCotacao,
                    usuarioLogado, seq);
                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Cadastrando opção {seq}. t_ORCAMENTO_COTACAO_OPCAO => [{JsonSerializer.Serialize(torcamentoCotacaoOpcao)}].");
                var opcaoResponse = orcamentoCotacaoOpcaoBll.InserirComTransacao(torcamentoCotacaoOpcao, contextoBdGravacao);
                if (torcamentoCotacaoOpcao.Id == 0)
                {
                    response.Mensagem = "Ops! Não gerou Id na opção de orçamento!";
                    return response;
                }

                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Cadastrando formas de pagamentos da opção {seq}.");
                var responseOpcoesPagtoResponse = formaPagtoOrcamentoCotacaoBll.CadastrarOrcamentoCotacaoOpcaoPagtoComTransacao(opcao.FormaPagto, opcaoResponse.Id, contextoBdGravacao, response.CorrelationId);
                if (responseOpcoesPagtoResponse.Sucesso && responseOpcoesPagtoResponse.TorcamentoCotacaoOpcaoPagtos.Count == 0)
                {
                    response.Mensagem = "Falha ao gravar forma de pagamento!";
                    return response;
                }

                _logger.LogInformation($"Método Cadastrar opções de orçamento - Cadastrando produtos unificados e atômicos da opção {seq}");
                var responseUnificadosEAtomicos = produtoOrcamentoCotacaoBll.CadastrarOrcamentoCotacaoOpcaoProdutosUnificadosEAtomicosComTransacao(opcao,
                    opcaoResponse.Id, loja, contextoBdGravacao, correlationId);

                if (!string.IsNullOrEmpty(responseUnificadosEAtomicos.Mensagem))
                {
                    response.Mensagem = responseUnificadosEAtomicos.Mensagem;
                    return response;
                }
                if (responseUnificadosEAtomicos.Sucesso && responseUnificadosEAtomicos.TorcamentoCotacaoItemUnificados.Count == 0)
                {
                    response.Mensagem = "Ops! Falha ao salvar os pagamentos e produtos!";
                    return response;
                }

                _logger.LogInformation($"Método Cadastrar opções de orçamento - Cadastrando custo de produtos atômicos da opção {seq}");
                var responseAtomicosCustoFin = produtoOrcamentoCotacaoBll.CadastrarProdutoAtomicoCustoFinComTransacao(responseOpcoesPagtoResponse.TorcamentoCotacaoOpcaoPagtos,
                    responseUnificadosEAtomicos.TorcamentoCotacaoItemUnificados, opcao.ListaProdutos, loja, contextoBdGravacao, correlationId);
                if (!string.IsNullOrEmpty(responseAtomicosCustoFin.Mensagem))
                {
                    response.Mensagem = responseAtomicosCustoFin.Mensagem;
                    return response;
                }
                if (responseAtomicosCustoFin.Sucesso && responseAtomicosCustoFin.TorcamentoCotacaoOpcaoItemAtomicoCustoFins.Count == 0)
                {
                    response.Mensagem = "Ops! Falha ao salvar custos dos produto!";
                    return response;
                }
            }

            response.Sucesso = true;
            return response;
        }

        private TorcamentoCotacaoOpcao MontarTorcamentoCotacaoOpcao(CadastroOrcamentoOpcaoRequest opcao, int idOrcamentoCotacao,
            UsuarioLogin usuarioLogado, int sequencia)
        {
            return new TorcamentoCotacaoOpcao()
            {
                IdOrcamentoCotacao = idOrcamentoCotacao,
                PercRT = opcao.PercRT,
                Sequencia = sequencia,
                IdTipoUsuarioContextoCadastro = (int)usuarioLogado.TipoUsuario,
                IdUsuarioCadastro = usuarioLogado.Id,
                DataCadastro = DateTime.Now.Date,
                DataHoraCadastro = DateTime.Now
            };


        }

        public AtualizarOrcamentoOpcaoResponse AtualizarOrcamentoOpcao(AtualizarOrcamentoOpcaoRequest opcao, UsuarioLogin usuarioLogado,
            OrcamentoResponse orcamento)
        {
            var response = new AtualizarOrcamentoOpcaoResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Início da atualização da opção Id[{opcao.Id}] orçamento Id[{opcao.IdOrcamentoCotacao}].");

            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Buscando opção a ser atualizada.");
            var tOrcamentoCotacaoOpcoes = orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro() { IdOrcamentoCotacao = opcao.IdOrcamentoCotacao });
            if (tOrcamentoCotacaoOpcoes == null)
            {
                response.Mensagem = "Falha ao buscar opção de orçamento";
                return response;
            }

            var tOrcamentoCotacaoOpcaoAntiga = tOrcamentoCotacaoOpcoes.Where(x => x.Id == opcao.Id).FirstOrDefault();
            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Retorno da busca de opção a ser atualizada. Response => [{JsonSerializer.Serialize(tOrcamentoCotacaoOpcaoAntiga)}]");

            var opcaoNovo = new TorcamentoCotacaoOpcao()
            {
                Id = opcao.Id,
                IdOrcamentoCotacao = opcao.IdOrcamentoCotacao,
                PercRT = opcao.PercRT,
                Sequencia = opcao.Sequencia,
                DataCadastro = tOrcamentoCotacaoOpcaoAntiga.DataCadastro,
                DataHoraCadastro = tOrcamentoCotacaoOpcaoAntiga.DataHoraCadastro,
                IdUsuarioCadastro = tOrcamentoCotacaoOpcaoAntiga.IdUsuarioCadastro,
                IdTipoUsuarioContextoCadastro = tOrcamentoCotacaoOpcaoAntiga.IdTipoUsuarioContextoCadastro,
                IdTipoUsuarioContextoUltAtualizacao = (int)usuarioLogado.TipoUsuario,
                DataHoraUltAtualizacao = DateTime.Now,
                IdUsuarioUltAtualizacao = usuarioLogado.Id,
            };

            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Buscando todas as formas de pagamentos da opção a ser atualizada.");
            var tOrcamentoCotacaoOpcaoPagtosAntiga = formaPagtoOrcamentoCotacaoBll.BuscarOpcaoFormasPagtos(opcao.Id);
            if (tOrcamentoCotacaoOpcaoPagtosAntiga == null)
            {
                response.Mensagem = "Falha ao busca formas de pagamentos da opção!";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Retorno da busca de todas as formas de pagamentos da opção a ser atualizada. Response => [{JsonSerializer.Serialize(tOrcamentoCotacaoOpcaoPagtosAntiga)}]");


            using (var dbGravacao = contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Atualizando opção. Id => [{opcaoNovo.Id}]");
                    var tOpcao = orcamentoCotacaoOpcaoBll.AtualizarComTransacao(opcaoNovo, dbGravacao);

                    var responseUnificados = produtoOrcamentoCotacaoBll
                        .AtualizarOrcamentoCotacaoOpcaoProdutosUnificadosComTransacao(opcao.ListaProdutos, opcao.Id, dbGravacao, opcao.CorrelationId);
                    if (!string.IsNullOrEmpty(responseUnificados.Mensagem))
                    {
                        response.Mensagem = responseUnificados.Mensagem;
                        return response;
                    }

                    tOpcao.TorcamentoCotacaoItemUnificados = responseUnificados.TorcamentoCotacaoItemUnificados;
                    if (tOpcao.TorcamentoCotacaoItemUnificados == null)
                    {
                        response.Mensagem = "Falha ao atualizar os produtos!";
                        return response;
                    }

                    var responseAtomicos = produtoOrcamentoCotacaoBll
                        .AtualizarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(opcao.ListaProdutos, opcao.Id,
                        tOpcao.TorcamentoCotacaoItemUnificados, dbGravacao, opcao.CorrelationId);
                    if (!string.IsNullOrEmpty(responseAtomicos.Mensagem))
                    {
                        response.Mensagem = responseAtomicos.Mensagem;
                        return response;
                    }

                    if (responseAtomicos.TorcamentoCotacaoOpcaoItemAtomicos == null)
                    {
                        response.Mensagem = "Falha ao atualizar os produtos!";
                        return response;
                    }                    

                    var responseFormaPagto = formaPagtoOrcamentoCotacaoBll
                        .AtualizarOrcamentoCotacaoOpcaoPagtoComTransacao(opcao, tOrcamentoCotacaoOpcaoPagtosAntiga, dbGravacao);
                    if (!string.IsNullOrEmpty(responseFormaPagto.Mensagem))
                    {
                        response.Mensagem = responseFormaPagto.Mensagem;
                        return response;
                    }
                    if (responseFormaPagto.TorcamentoCotacaoOpcaoPagtos == null)
                    {
                        response.Mensagem = "Falha ao atualizar as formas de pagamentos!";
                        return response;
                    }

                    var responseAtomicosCustoFin = produtoOrcamentoCotacaoBll.AtualizarProdutoAtomicoCustoFinComTransacao(opcao,
                        tOpcao.TorcamentoCotacaoItemUnificados, responseFormaPagto.TorcamentoCotacaoOpcaoPagtos, dbGravacao, 
                        usuarioLogado, orcamento, opcao.CorrelationId);
                    if (!string.IsNullOrEmpty(responseAtomicosCustoFin.Mensagem))
                    {
                        response.Mensagem = responseAtomicosCustoFin.Mensagem;
                        return response;
                    }
                    if (responseAtomicosCustoFin.TorcamentoCotacaoOpcaoItemAtomicoCustoFins == null)
                    {
                        response.Mensagem = "Falha ao atualizar produtos!";
                        return response;
                    }

                    dbGravacao.transacao.Commit();
                }
                catch (Exception ex)
                {
                    dbGravacao.transacao.Rollback();
                    throw new Exception(ex.Message);
                }
            }

            response.Sucesso = true;
            return response;
        }

        public List<OrcamentoOpcaoResponseViewModel> PorFiltro(TorcamentoCotacaoOpcaoFiltro filtro)
        {
            var orcamentoOpcoes = orcamentoCotacaoOpcaoBll.PorFiltro(filtro);

            if (orcamentoOpcoes == null) throw new ArgumentException("Falha ao buscar as opções do orçamento!");

            List<OrcamentoOpcaoResponseViewModel> orcamentoOpcoesResponse = new List<OrcamentoOpcaoResponseViewModel>();

            foreach (var opcao in orcamentoOpcoes)
            {
                var opcaoFormaPagtos = formaPagtoOrcamentoCotacaoBll.BuscarOpcaoFormasPagtos(opcao.Id);

                if (opcaoFormaPagtos == null) throw new ArgumentException("Pagamento da opção não encontrado!");

                var itensOpcao = produtoOrcamentoCotacaoBll.BuscarOpcaoProdutos(opcao.Id).Result;

                if (itensOpcao == null) throw new ArgumentException("Produtos da opção não encontrados!");

                OrcamentoOpcaoResponseViewModel opcaoResponse = new OrcamentoOpcaoResponseViewModel()
                {
                    Id = opcao.Id,
                    IdOrcamentoCotacao = filtro.IdOrcamentoCotacao,
                    PercRT = opcao.PercRT,
                    Sequencia = opcao.Sequencia,
                    VlTotal = itensOpcao.Sum(x => x.TotalItem),
                    ListaProdutos = itensOpcao,
                    FormaPagto = mapper.Map<List<FormaPagtoCriacaoResponseViewModel>>(opcaoFormaPagtos)
                };

                orcamentoOpcoesResponse.Add(opcaoResponse);
            }

            return orcamentoOpcoesResponse;
        }

        public List<TorcamentoCotacaoOpcao> PorFiltroComTransacao(TorcamentoCotacaoOpcaoFiltro filtro, ContextoBdGravacao dbGravacao)
        {
            return orcamentoCotacaoOpcaoBll.PorFiltroComTransacao(filtro, dbGravacao);
        }

        public TorcamentoCotacaoOpcao AtualizarOpcaoComTransacao(TorcamentoCotacaoOpcao opcaoPagto, ContextoBdGravacao dbGravacao)
        {
            return orcamentoCotacaoOpcaoBll.AtualizarComTransacao(opcaoPagto, dbGravacao);
        }
    }
}
