using FormaPagamento;
using InfraBanco.Constantes;
using MeioPagamentos;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento.MeiosPagamento;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class FormaPagtoOrcamentoCotacaoBll
    {
        private readonly FormaPagtoBll _formaPagtoBll;
        private readonly MeiosPagamentosBll _meiosPagamentosBll;

        public FormaPagtoOrcamentoCotacaoBll(FormaPagtoBll formaPagtoBll, MeiosPagamentosBll _meiosPagamentosBll)
        {
            this._formaPagtoBll = formaPagtoBll;
            this._meiosPagamentosBll = _meiosPagamentosBll;
        }

        public List<FormaPagamentoResponseViewModel> BuscarFormasPagamentos(string tipoCliente, short tipoUsuario, string apelido, bool comIndicacao)
        {
            var tiposPagtos = _formaPagtoBll.BuscarFormasPagtos_Gabriel(true, Constantes.Modulos.COD_MODULO_ORCAMENTOCOTACAO, tipoCliente, false, true, tipoUsuario);

            if (tiposPagtos == null) return null;

            return BuscarMeiosPagamento(tiposPagtos, tipoCliente, tipoUsuario, apelido);





            List<FormaPagamentoResponseViewModel> response = new List<FormaPagamentoResponseViewModel>();
            var saida = _formaPagtoBll.BuscarFormasPagtos(true, Constantes.Modulos.COD_MODULO_ORCAMENTOCOTACAO, tipoCliente, false, true, tipoUsuario, apelido);

            //foreach (var s in saida)
            //{
            //    response.Add(
            //        new FormaPagamentoResponseViewModel
            //        {
            //            IdTipoPagamento = s.IdTipoPagamento,
            //            MeiosPagamentos = s.MeiosPagamentos.Select(x=>
            //            new MeioPagamentoResponseViewModel
            //            {
            //                Id = x.Id,
            //                IdTipoParcela = x.IdTipoParcela,
            //                Descricao = x.Descricao,
            //                Ordenacao = x.Ordenacao
            //            }).ToList()
            //        });
            //}

            return response;
        }

        private List<FormaPagamentoResponseViewModel> BuscarMeiosPagamento(List<InfraBanco.Modelos.TcfgPagtoFormaStatus> tiposPagtos, 
            string tipoCliente, int? tipoUsuario, string apelido)
        {
            if (tiposPagtos == null) return null;

            List<FormaPagamentoResponseViewModel> response = new List<FormaPagamentoResponseViewModel>();
            List<InfraBanco.Modelos.TcfgPagtoMeioStatus> meiosPagamento = new List<InfraBanco.Modelos.TcfgPagtoMeioStatus>();

            foreach (var fp in tiposPagtos)
            {
                FormaPagamentoResponseViewModel item = new FormaPagamentoResponseViewModel();
                item.IdTipoPagamento = fp.TcfgPagtoForma.Id;

                var filtro = CriarFiltro(fp, tipoCliente, (short)tipoUsuario, apelido);
                meiosPagamento = _meiosPagamentosBll.PorFiltro(filtro);
                item.MeiosPagamentos = MeioPagamentoResponseViewModel.ListaMeioPagamentoResponseViewModel_De_TcfgPagtoMeioStatus(meiosPagamento);

                response.Add(item);
            }

            return response;
        }

        private InfraBanco.Modelos.Filtros.TcfgPagtoMeioStatusFiltro CriarFiltro(InfraBanco.Modelos.TcfgPagtoFormaStatus tipoPagto,
            string tipoCliente, short tipoUsuario, string apelido)
        {
            var filtro = new InfraBanco.Modelos.Filtros.TcfgPagtoMeioStatusFiltro()
            {
                IdCfgModulo = (short)Constantes.Modulos.COD_MODULO_ORCAMENTOCOTACAO,
                IdCfgTipoPessoaCliente = (short)(tipoCliente == Constantes.ID_PF ? 1 : 2),
                IdCfgTipoUsuario = tipoUsuario,
                PedidoComIndicador = 0,
                Habilitado = 1,
                IncluirTcfgPagtoMeio = true,
                IncluirTorcamentistaEIndicadorRestricaoFormaPagtos = true,
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
                    //filtro.IdTipoParcela = (short)Constantes.TipoParcela.DEMAIS_PARCELAS_PRESTACAO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    filtro.IncluirTcfgTipoParcela = true;
                    filtro.IdTipoParcela = (short)Constantes.TipoParcela.PRIMEIRA_PRESTACAO;
                    //filtro.IdTipoParcela = (short)Constantes.TipoParcela.DEMAIS_PARCELAS_PRESTACAO;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    filtro.IdTipoParcela = (short)Constantes.TipoParcela.PARCELA_UNICA;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    filtro.IdCfgPagtoForma = Int16.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA);
                    break;
            }

            return filtro;
        }

    }
}
