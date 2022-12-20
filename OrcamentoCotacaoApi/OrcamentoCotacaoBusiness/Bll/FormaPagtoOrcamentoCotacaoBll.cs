using AutoMapper;
using FormaPagamento;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using MeioPagamentos;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.Orcamento;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento.MeiosPagamento;
using OrcamentoCotacaoBusiness.Models.Response.Orcamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class FormaPagtoOrcamentoCotacaoBll
    {
        private readonly FormaPagtoBll _formaPagtoBll;
        private readonly MeiosPagamentosBll _meiosPagamentosBll;
        private readonly OrcamentoCotacaoOpcaoPagto.OrcamentoCotacaoOpcaoPagtoBll orcamentoCotacaoOpcaoPagtoBll;
        private readonly IMapper mapper;
        private readonly OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll orcamentoCotacaoOpcaoItemAtomicoCustoFinBll;
        private readonly ILogger<FormaPagtoOrcamentoCotacaoBll> _logger;

        public FormaPagtoOrcamentoCotacaoBll(FormaPagtoBll formaPagtoBll, MeiosPagamentosBll _meiosPagamentosBll,
            OrcamentoCotacaoOpcaoPagto.OrcamentoCotacaoOpcaoPagtoBll orcamentoCotacaoOpcaoPagtoBll,
            IMapper mapper,
            OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll orcamentoCotacaoOpcaoItemAtomicoCustoFinBll,
            ILogger<FormaPagtoOrcamentoCotacaoBll> _logger)
        {
            this._formaPagtoBll = formaPagtoBll;
            this._meiosPagamentosBll = _meiosPagamentosBll;
            this.orcamentoCotacaoOpcaoPagtoBll = orcamentoCotacaoOpcaoPagtoBll;
            this.mapper = mapper;
            this.orcamentoCotacaoOpcaoItemAtomicoCustoFinBll = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll;
            this._logger = _logger;
        }

        public List<FormaPagamentoResponseViewModel> BuscarFormasPagamentos(string tipoCliente, Constantes.TipoUsuario tipoUsuario, string apelido, byte comIndicacao)
        {
            var tiposPagtos = _formaPagtoBll.BuscarFormasPagtos(true, Constantes.Modulos.COD_MODULO_ORCAMENTOCOTACAO,
                tipoCliente, comIndicacao == 1, true, Constantes.TipoUsuarioPerfil.getUsuarioPerfil(tipoUsuario));

            if (tiposPagtos == null) return null;

            return BuscarMeiosPagamento(tiposPagtos, tipoCliente, Constantes.TipoUsuarioPerfil.getUsuarioPerfil(tipoUsuario), apelido, comIndicacao);
        }

        private List<FormaPagamentoResponseViewModel> BuscarMeiosPagamento(List<InfraBanco.Modelos.TcfgPagtoFormaStatus> tiposPagtos,
            string tipoCliente, Constantes.eTipoUsuarioPerfil tipoUsuario, string apelido, byte comIndicacao)
        {
            if (tiposPagtos == null) return null;

            List<FormaPagamentoResponseViewModel> response = new List<FormaPagamentoResponseViewModel>();
            List<InfraBanco.Modelos.TcfgPagtoMeioStatus> meiosPagamento = new List<InfraBanco.Modelos.TcfgPagtoMeioStatus>();

            foreach (var fp in tiposPagtos)
            {
                FormaPagamentoResponseViewModel item = new FormaPagamentoResponseViewModel();
                item.IdTipoPagamento = fp.TcfgPagtoForma.Id;
                item.TipoPagamentoDescricao = fp.TcfgPagtoForma.Descricao;

                var filtro = CriarFiltro(fp, tipoCliente, (short)tipoUsuario, apelido, comIndicacao);
                meiosPagamento = _meiosPagamentosBll.PorFiltro(filtro);

                item.MeiosPagamentos = MeioPagamentoResponseViewModel.ListaMeioPagamentoResponseViewModel_De_TcfgPagtoMeioStatus(meiosPagamento);

                response.Add(item);
            }

            return response;
        }

        private InfraBanco.Modelos.Filtros.TcfgPagtoMeioStatusFiltro CriarFiltro(InfraBanco.Modelos.TcfgPagtoFormaStatus tipoPagto,
            string tipoCliente, short tipoUsuario, string apelido, byte comIndicacao)
        {
            var filtro = new InfraBanco.Modelos.Filtros.TcfgPagtoMeioStatusFiltro()
            {
                IdCfgModulo = (short)Constantes.Modulos.COD_MODULO_ORCAMENTOCOTACAO,
                IdCfgTipoPessoaCliente = (short)(tipoCliente == Constantes.ID_PF ? 1 : 2),
                IdCfgTipoUsuario = tipoUsuario,
                PedidoComIndicador = (byte)comIndicacao,
                Habilitado = 1,
                IncluirTcfgPagtoMeio = true,
                IncluirTorcamentistaEIndicadorRestricaoFormaPagtos = comIndicacao == 1 ? true : false,
                Apelido = apelido
            };

            switch (tipoPagto.TcfgPagtoForma.Id.ToString())
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    filtro.IdCfgPagtoForma = Int16.Parse(Constantes.COD_FORMA_PAGTO_A_VISTA);
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    filtro.IdCfgPagtoForma = Int16.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO);
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    filtro.IncluirTcfgTipoParcela = true;
                    filtro.IdCfgPagtoForma = Int16.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA);
                    filtro.IdTipoParcela = (short)Constantes.TipoParcela.PARCELA_DE_ENTRADA;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    filtro.IncluirTcfgTipoParcela = true;
                    filtro.IdTipoParcela = (short)Constantes.TipoParcela.PRIMEIRA_PRESTACAO;
                    filtro.IdCfgPagtoForma = Int16.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA);
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    filtro.IncluirTcfgTipoParcela = true;
                    filtro.IdTipoParcela = (short)Constantes.TipoParcela.PARCELA_UNICA;
                    filtro.IdCfgPagtoForma = Int16.Parse(Constantes.COD_FORMA_PAGTO_PARCELA_UNICA);
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    filtro.IdCfgPagtoForma = Int16.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA);
                    break;
            }

            return filtro;
        }


        public async Task<int?> GetMaximaQtdeParcelasCartaoVisa(Constantes.TipoUsuario tipoUsuario)
        {
            var tipoPerfil = Constantes.TipoUsuarioPerfil.getUsuarioPerfil(tipoUsuario);

            return await _formaPagtoBll.BuscarQtdeParcCartaoVisa();
        }

        public CadastroOrcamentoOpcaoFormaPagtoResponse CadastrarOrcamentoCotacaoOpcaoPagtoComTransacao(List<CadastroOrcamentoOpcaoFormaPagtoRequest> FormaPagtos,
            int idOrcamentoCotacaoOpcao, ContextoBdGravacao contextoBdGravacao, Guid correlationId)
        {
            CadastroOrcamentoOpcaoFormaPagtoResponse response = new CadastroOrcamentoOpcaoFormaPagtoResponse();
            response.Sucesso = true;
            response.TorcamentoCotacaoOpcaoPagtos = new List<TorcamentoCotacaoOpcaoPagto>();
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();
            foreach (var pagto in FormaPagtos)
            {
                TorcamentoCotacaoOpcaoPagto torcamentoCotacaoOpcaoPagto = new TorcamentoCotacaoOpcaoPagto()
                {
                    IdOrcamentoCotacaoOpcao = idOrcamentoCotacaoOpcao,
                    Aprovado = false,
                    Observacao = pagto.Observacao,
                    Tipo_parcelamento = pagto.Tipo_parcelamento,
                    Av_forma_pagto = pagto.Av_forma_pagto,
                    Pc_qtde_parcelas = pagto.Pc_qtde_parcelas,
                    Pc_valor_parcela = pagto.Pc_valor_parcela,
                    Pc_maquineta_qtde_parcelas = pagto.Pc_maquineta_qtde_parcelas,
                    Pc_maquineta_valor_parcela = pagto.Pc_maquineta_valor_parcela,
                    Pce_forma_pagto_entrada = pagto.Pce_forma_pagto_entrada,
                    Pce_forma_pagto_prestacao = pagto.Pce_forma_pagto_prestacao,
                    Pce_entrada_valor = pagto.Pce_entrada_valor,
                    Pce_prestacao_qtde = pagto.Pce_prestacao_qtde,
                    Pce_prestacao_valor = pagto.Pce_prestacao_valor,
                    Pce_prestacao_periodo = pagto.Pce_prestacao_periodo,
                    Pse_forma_pagto_prim_prest = pagto.Pse_forma_pagto_prim_prest,
                    Pse_forma_pagto_demais_prest = pagto.Pse_forma_pagto_demais_prest,
                    Pse_prim_prest_valor = pagto.Pse_prim_prest_valor,
                    Pse_prim_prest_apos = pagto.Pse_prim_prest_apos,
                    Pse_demais_prest_qtde = pagto.Pse_demais_prest_qtde,
                    Pse_demais_prest_valor = pagto.Pse_demais_prest_valor,
                    Pse_demais_prest_periodo = pagto.Pse_demais_prest_periodo,
                    Pu_forma_pagto = pagto.Pu_forma_pagto,
                    Pu_valor = pagto.Pu_valor,
                    Pu_vencto_apos = pagto.Pu_vencto_apos
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Cadastrando forma de pagamento. t_ORCAMENTO_COTACAO_OPCAO => [{JsonSerializer.Serialize(torcamentoCotacaoOpcaoPagto)}].");
                response.TorcamentoCotacaoOpcaoPagtos.Add(orcamentoCotacaoOpcaoPagtoBll.InserirComTransacao(torcamentoCotacaoOpcaoPagto, contextoBdGravacao));
            }

            return response;
        }

        public AtualizarOrcamentoOpcaoFormaPagtoResponse AtualizarOrcamentoCotacaoOpcaoPagtoComTransacao(
            AtualizarOrcamentoOpcaoRequest opcao, List<TorcamentoCotacaoOpcaoPagto> formaPagtoAntiga,
            ContextoBdGravacao dbGravacao)
        {
            var response = new AtualizarOrcamentoOpcaoFormaPagtoResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();
            response.TorcamentoCotacaoOpcaoPagtos = new List<TorcamentoCotacaoOpcaoPagto>();

            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Verificando a remoção de formas de pagamentos");
            string logRemocao = RemoverOrcamentoCotacaoOpcaoPagtoComTransacao(formaPagtoAntiga, opcao, dbGravacao);

            foreach (var pagto in opcao.FormaPagto)
            {
                var f = formaPagtoAntiga.Where(x => x.Id == pagto.Id).FirstOrDefault();
                _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Verificando se a forma de pagamento existe.");

                if (f == null)
                {
                    var p = mapper.Map<CadastroOrcamentoOpcaoFormaPagtoRequest>(pagto);
                    List<CadastroOrcamentoOpcaoFormaPagtoRequest> lstPagto = new List<CadastroOrcamentoOpcaoFormaPagtoRequest>();
                    lstPagto.Add(p);
                    var responseOpcoesPagtoResponse = CadastrarOrcamentoCotacaoOpcaoPagtoComTransacao(lstPagto, opcao.Id, dbGravacao, opcao.CorrelationId);
                    if (responseOpcoesPagtoResponse.Sucesso && responseOpcoesPagtoResponse.TorcamentoCotacaoOpcaoPagtos.Count == 0)
                    {
                        response.Mensagem = "Falha ao gravar forma de pagamento!";
                        return response;
                    }
                    _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Nova forma de pagamento cadastrada. Response => [{JsonSerializer.Serialize(responseOpcoesPagtoResponse.TorcamentoCotacaoOpcaoPagtos.FirstOrDefault())}]");
                    response.TorcamentoCotacaoOpcaoPagtos.Add(responseOpcoesPagtoResponse.TorcamentoCotacaoOpcaoPagtos.FirstOrDefault());
                }
                else
                {
                    TorcamentoCotacaoOpcaoPagto torcamentoCotacaoOpcaoPagto = new TorcamentoCotacaoOpcaoPagto()
                    {
                        Id = pagto.Id,
                        IdOrcamentoCotacaoOpcao = opcao.Id,
                        Aprovado = false,
                        Observacao = pagto.Observacao,
                        Tipo_parcelamento = pagto.Tipo_parcelamento,
                        Av_forma_pagto = pagto.Av_forma_pagto,
                        Pc_qtde_parcelas = pagto.Pc_qtde_parcelas,
                        Pc_valor_parcela = pagto.Pc_valor_parcela,
                        Pc_maquineta_qtde_parcelas = pagto.Pc_maquineta_qtde_parcelas,
                        Pc_maquineta_valor_parcela = pagto.Pc_maquineta_valor_parcela,
                        Pce_forma_pagto_entrada = pagto.Pce_forma_pagto_entrada,
                        Pce_forma_pagto_prestacao = pagto.Pce_forma_pagto_prestacao,
                        Pce_entrada_valor = pagto.Pce_entrada_valor,
                        Pce_prestacao_qtde = pagto.Pce_prestacao_qtde,
                        Pce_prestacao_valor = pagto.Pce_prestacao_valor,
                        Pce_prestacao_periodo = pagto.Pce_prestacao_periodo,
                        Pse_forma_pagto_prim_prest = pagto.Pse_forma_pagto_prim_prest,
                        Pse_forma_pagto_demais_prest = pagto.Pse_forma_pagto_demais_prest,
                        Pse_prim_prest_valor = pagto.Pse_prim_prest_valor,
                        Pse_prim_prest_apos = pagto.Pse_prim_prest_apos,
                        Pse_demais_prest_qtde = pagto.Pse_demais_prest_qtde,
                        Pse_demais_prest_valor = pagto.Pse_demais_prest_valor,
                        Pse_demais_prest_periodo = pagto.Pse_demais_prest_periodo,
                        Pu_forma_pagto = pagto.Pu_forma_pagto,
                        Pu_valor = pagto.Pu_valor,
                        Pu_vencto_apos = pagto.Pu_vencto_apos
                    };

                    _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Atualizando forma de pagamento Id[{torcamentoCotacaoOpcaoPagto.Id}]. Response => [{JsonSerializer.Serialize(torcamentoCotacaoOpcaoPagto)}]");
                    response.TorcamentoCotacaoOpcaoPagtos.Add(orcamentoCotacaoOpcaoPagtoBll.AtualizarComTransacao(torcamentoCotacaoOpcaoPagto, dbGravacao));
                }
            }

            response.Sucesso = true;
            return response;
        }

        public List<TorcamentoCotacaoOpcaoPagto> BuscarOpcaoFormasPagtos(int idOpcao, bool incluirTorcamentoCotacaoOpcaoItemAtomicoCustoFin)
        {
            if (idOpcao == 0) return null;

            var opcaoFormaPagtos = orcamentoCotacaoOpcaoPagtoBll.PorFiltro(new TorcamentoCotacaoOpcaoPagtoFiltro() { IdOpcao = idOpcao,  IncluirTorcamentoCotacaoOpcaoItemAtomicoCustoFin = incluirTorcamentoCotacaoOpcaoItemAtomicoCustoFin});

            return opcaoFormaPagtos;
        }

        public string RemoverOrcamentoCotacaoOpcaoPagtoComTransacao(List<TorcamentoCotacaoOpcaoPagto> formaPagtoAntiga,
            AtualizarOrcamentoOpcaoRequest opcao, ContextoBdGravacao dbGravacao)
        {
            string logRetorno = "";
            string logProduto = "";
            foreach (var pagtoAntigo in formaPagtoAntiga)
            {
                var pagto = opcao.FormaPagto.Where(x => x.Id == pagtoAntigo.Id).FirstOrDefault();

                if (pagto == null)
                {
                    _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. RemoverOrcamentoCotacaoOpcaoPagtoComTransacao. Buscando forma de pagamento. Id => [{pagtoAntigo.Id}]");
                    var itemAtomicoCusto = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.PorFiltro(new TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro() { IdOpcaoPagto = pagtoAntigo.Id });

                    if (itemAtomicoCusto != null)
                    {
                        foreach (var item in itemAtomicoCusto)
                        {
                            orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.ExcluirComTransacao(item, dbGravacao);
                            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. RemoverOrcamentoCotacaoOpcaoPagtoComTransacao. Produto atômico custo fin excluída. Id => [{item.Id}]");

                            if (!string.IsNullOrEmpty(logProduto)) logProduto += $"\r      ";
                            logProduto += $"Id={item.Id}; IdItemAtomico={item.IdItemAtomico}; IdOpcaoPagto={item.IdOpcaoPagto};";
                        }
                    }

                    orcamentoCotacaoOpcaoPagtoBll.ExcluirComTransacao(pagtoAntigo, dbGravacao);
                    _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. RemoverOrcamentoCotacaoOpcaoPagtoComTransacao. Forma de pagamento excluída. Id => [{pagtoAntigo.Id}]");
                    logRetorno = $"Forma pagamento excluída: Id={pagtoAntigo.Id}; IdOrcamentoCotacaoOpcao={pagtoAntigo.IdOrcamentoCotacaoOpcao};\rLista de produtos atômicos custo excluídos: {logProduto}";
                }
            }

            return logRetorno;
        }

        public List<TorcamentoCotacaoOpcaoPagto> PorFiltroComTransacao(TorcamentoCotacaoOpcaoPagtoFiltro filtro, ContextoBdGravacao dbGravacao)
        {
            return orcamentoCotacaoOpcaoPagtoBll.PorFiltroComTransacao(filtro, dbGravacao);
        }

        public TorcamentoCotacaoOpcaoPagto AtualizarOpcaoPagtoComTransacao(TorcamentoCotacaoOpcaoPagto pagto, ContextoBdGravacao dbGravacao)
        {
            return orcamentoCotacaoOpcaoPagtoBll.AtualizarComTransacao(pagto, dbGravacao);
        }
    }
}
