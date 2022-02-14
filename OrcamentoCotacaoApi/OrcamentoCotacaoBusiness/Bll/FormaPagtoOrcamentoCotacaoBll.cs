using FormaPagamento;
using InfraBanco.Constantes;
using MeioPagamentos;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento.MeiosPagamento;
using PrepedidoBusiness.Dto.FormaPagto;
using System;
using System.Collections.Generic;

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

        public List<FormaPagamentoResponseViewModel> BuscarFormasPagamentos(string tipoCliente, string usuario)
        {
            short tipoUsuario = 3;
            var tiposPagtos = _formaPagtoBll.BuscarFormasPagtos(true, Constantes.Modulos.COD_MODULO_ORCAMENTOCOTACAO, tipoCliente, false, true, tipoUsuario);

            if (tiposPagtos == null) return null;

            return BuscarMeiosPagamento(tiposPagtos, tipoCliente, tipoUsuario);
        }

        private List<FormaPagamentoResponseViewModel> BuscarMeiosPagamento(List<InfraBanco.Modelos.TcfgPagtoFormaStatus> tiposPagtos, string tipoCliente, short tipoUsuario)
        {
            if (tiposPagtos == null) return null;

            List<FormaPagamentoResponseViewModel> response = new List<FormaPagamentoResponseViewModel>();
            List<InfraBanco.Modelos.TcfgPagtoMeioStatus> meiosPagamento = new List<InfraBanco.Modelos.TcfgPagtoMeioStatus>();

            foreach (var fp in tiposPagtos)
            {
                FormaPagamentoResponseViewModel item = new FormaPagamentoResponseViewModel();
                item.IdTipoPagamento = fp.TcfgPagtoForma.Id;

                if (fp.TcfgPagtoForma.Id.ToString() != Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO &&
                    fp.TcfgPagtoForma.Id.ToString() != Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                {
                    var filtro = CriarFiltro(fp, tipoCliente, tipoUsuario);
                    meiosPagamento = _meiosPagamentosBll.BuscarMeiosPagamento(filtro);
                    item.MeioPagamentoResponseViewModel = MeioPagamentoResponseViewModel.ListaMeioPagamentoResponseViewModel_De_TcfgPagtoMeioStatus(meiosPagamento);
                }

                response.Add(item);
            }

            return response;
        }

        private InfraBanco.Modelos.Filtros.TcfgPagtoMeioStatusFiltro CriarFiltro(InfraBanco.Modelos.TcfgPagtoFormaStatus tipoPagto,
            string tipoCliente, short tipoUsuario)
        {
            var filtro = new InfraBanco.Modelos.Filtros.TcfgPagtoMeioStatusFiltro()
            {
                IdCfgModulo = (short)Constantes.Modulos.COD_MODULO_ORCAMENTOCOTACAO,
                IdCfgTipoPessoaCliente = (short)(tipoCliente == Constantes.ID_PF ? 1 : 2),
                IdCfgTipoUsuario = tipoUsuario,
                PedidoComIndicador = 0,
                Habilitado = 1,
                IncluirTcfgPagtoMeio = true
            };

            switch (tipoPagto.TcfgPagtoForma.Id.ToString())
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    filtro.IdCfgPagtoForma = (short)Constantes.TipoParcela.A_VISTA;
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    filtro.IncluirTcfgTipoParcela = true;
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
            }

            return filtro;
        }

    }
}
