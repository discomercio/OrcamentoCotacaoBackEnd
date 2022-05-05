using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Mensagem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class MensagemOrcamentoCotacaoBll
    {
        private readonly MensagemBll _bll;

        public MensagemOrcamentoCotacaoBll(MensagemBll bll)
        {
            _bll = bll;
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            return await _bll.ObterListaMensagem(IdOrcamentoCotacao);
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagemPendente(int IdOrcamentoCotacao, int IdUsuarioDestinatario)
        {
            return await _bll.ObterListaMensagemPendente(IdOrcamentoCotacao, IdUsuarioDestinatario);
        }

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            return _bll.EnviarMensagem(orcamentoCotacaoMensagem);
        }

        public bool MarcarMensagemComoLida(int IdOrcamentoCotacao, int idUsuarioDestinatario)
        {
            return _bll.MarcarMensagemComoLida(IdOrcamentoCotacao, idUsuarioDestinatario);
        }

    }
}
