using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.Extensions.Options;
using Orcamento;
using Orcamento.Dto;
using OrcamentoCotacaoBusiness.Models.Response;
using PrepedidoBusiness.Bll;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentoCotacaoBll
    {
        private readonly OrcamentoBll _orcamentoBll;
        private readonly PedidoPrepedidoApiBll _pedidoPrepedidoApiBll;
        private readonly ConfigOrcamentoCotacao _appSettings;

        public OrcamentoCotacaoBll(
            OrcamentoBll orcamentoBll, 
            PedidoPrepedidoApiBll pedidoPrepedidoApiBll, 
            IOptions<ConfigOrcamentoCotacao> appSettings)
        {
            _orcamentoBll = orcamentoBll;
            _pedidoPrepedidoApiBll = pedidoPrepedidoApiBll;
            _appSettings = appSettings.Value;
        }

        public List<OrcamentoCotacaoListaDto> PorFiltro(TorcamentoFiltro tOrcamentoFiltro)
        {
            if (tOrcamentoFiltro.Origem == "ORCAMENTOS")
            {
                return _orcamentoBll.OrcamentoCotacaoPorFiltro(tOrcamentoFiltro);
            }
            else if (tOrcamentoFiltro.Origem == "PENDENTES") //PrePedido/Em Aprovação [tOrcamentos]
            {
                return _orcamentoBll.OrcamentoPorFiltro(tOrcamentoFiltro);
            }
            else //if (tOrcamentoFiltro.Origem == "PEDIDOS")
            {
                return _pedidoPrepedidoApiBll.ListarPedidos(tOrcamentoFiltro);
            }
        }

        public async Task<List<TorcamentoCotacaoStatus>> ObterListaStatus(TorcamentoFiltro tOrcamentoFiltro)
        {
            return await _orcamentoBll.ObterListaStatus(tOrcamentoFiltro);
        }

        public ValidadeResponseViewModel BuscarConfigValidade()
        {
            return new ValidadeResponseViewModel
            {
                QtdeDiasValidade = _appSettings.QtdeDiasValidade,
                QtdeDiasProrrogacao = _appSettings.QtdeDiasProrrogacao,
                QtdeMaxProrrogacao = _appSettings.QtdeMaxProrrogacao,
                QtdeGlobalValidade = _appSettings.QtdeGlobalValidade,
            };
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            return await _orcamentoBll.ObterListaMensagem(IdOrcamentoCotacao);
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagemPendente(int IdOrcamentoCotacao, int IdUsuarioDestinatario)
        {
            return await _orcamentoBll.ObterListaMensagemPendente(IdOrcamentoCotacao, IdUsuarioDestinatario);
        }

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            return  _orcamentoBll.EnviarMensagem(orcamentoCotacaoMensagem);
        }


        public bool MarcarMensagemComoLida(int IdOrcamentoCotacao, int idUsuarioDestinatario)
        {
            return _orcamentoBll.MarcarMensagemComoLida(IdOrcamentoCotacao,idUsuarioDestinatario);
        }

    }
}
