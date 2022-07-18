using AutoMapper;
using FormaPagamento;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using MeioPagamentos;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento.MeiosPagamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class FormaPagtoOrcamentoCotacaoBll
    {
        private readonly FormaPagtoBll _formaPagtoBll;
        private readonly MeiosPagamentosBll _meiosPagamentosBll;
        private readonly OrcamentoCotacaoOpcaoPagto.OrcamentoCotacaoOpcaoPagtoBll orcamentoCotacaoOpcaoPagtoBll;
        private readonly IMapper mapper;


        public FormaPagtoOrcamentoCotacaoBll(FormaPagtoBll formaPagtoBll, MeiosPagamentosBll _meiosPagamentosBll,
            OrcamentoCotacaoOpcaoPagto.OrcamentoCotacaoOpcaoPagtoBll orcamentoCotacaoOpcaoPagtoBll,
            IMapper mapper)
        {
            this._formaPagtoBll = formaPagtoBll;
            this._meiosPagamentosBll = _meiosPagamentosBll;
            this.orcamentoCotacaoOpcaoPagtoBll = orcamentoCotacaoOpcaoPagtoBll;
            this.mapper = mapper;
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

        public List<TorcamentoCotacaoOpcaoPagto> CadastrarOrcamentoCotacaoOpcaoPagtoComTransacao(List<FormaPagtoCriacaoRequestViewModel> FormaPagtos,
            int idOrcamentoCotacaoOpcao, ContextoBdGravacao contextoBdGravacao)
        {
            List<TorcamentoCotacaoOpcaoPagto> retorno = new List<TorcamentoCotacaoOpcaoPagto>();
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

                retorno.Add(orcamentoCotacaoOpcaoPagtoBll.InserirComTransacao(torcamentoCotacaoOpcaoPagto, contextoBdGravacao));
            }

            return retorno;
        }

        public List<TorcamentoCotacaoOpcaoPagto> AtualizarOrcamentoCotacaoOpcaoPagtoComTransacao(List<FormaPagtoCriacaoResponseViewModel> formaPagtos,
            int idOrcamentoOpcao, List<TorcamentoCotacaoOpcaoPagto> formaPagtoAntiga, ContextoBdGravacao dbGravacao)
        {
            List<TorcamentoCotacaoOpcaoPagto> lstRetorno = new List<TorcamentoCotacaoOpcaoPagto>();
            foreach (var pagto in formaPagtos)
            {
                var f = formaPagtoAntiga.Where(x => x.Id == pagto.Id).FirstOrDefault();
                if (f == null)
                {
                    var p = mapper.Map<FormaPagtoCriacaoRequestViewModel>(pagto);
                    List<FormaPagtoCriacaoRequestViewModel> lstPagto = new List<FormaPagtoCriacaoRequestViewModel>();
                    lstPagto.Add(p);
                    lstRetorno.Add(CadastrarOrcamentoCotacaoOpcaoPagtoComTransacao(lstPagto, idOrcamentoOpcao, dbGravacao).FirstOrDefault());
                }
                else
                {
                    TorcamentoCotacaoOpcaoPagto torcamentoCotacaoOpcaoPagto = new TorcamentoCotacaoOpcaoPagto()
                    {
                        Id = pagto.Id,
                        IdOrcamentoCotacaoOpcao = idOrcamentoOpcao,
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

                    lstRetorno.Add(orcamentoCotacaoOpcaoPagtoBll.AtualizarComTransacao(torcamentoCotacaoOpcaoPagto, dbGravacao));
                }
            }

            return lstRetorno;
        }

        public List<TorcamentoCotacaoOpcaoPagto> BuscarOpcaoFormasPagtos(int idOpcao)
        {
            if (idOpcao == 0) return null;

            var opcaoFormaPagtos = orcamentoCotacaoOpcaoPagtoBll.PorFiltro(new TorcamentoCotacaoOpcaoPagtoFiltro() { IdOpcao = idOpcao });

            return opcaoFormaPagtos;
        }
    }
}
