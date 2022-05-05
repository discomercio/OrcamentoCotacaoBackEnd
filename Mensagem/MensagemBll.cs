using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mensagem
{
    public class MensagemBll 
    {
        private readonly MensagemData _data;

        public MensagemBll(MensagemData data)
        {
            _data = data;
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            return await _data.ObterListaMensagem(IdOrcamentoCotacao);
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagemPendente(int IdOrcamentoCotacao, int IdUsuarioDestinatario)
        {
            return await _data.ObterListaMensagemPendente(IdOrcamentoCotacao, IdUsuarioDestinatario);
        }

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            return _data.EnviarMensagem(orcamentoCotacaoMensagem);
        }

        public bool MarcarMensagemComoLida(int IdOrcamentoCotacao, int IdUsuarioDestinatario)
        {
            return _data.MarcarMensagemComoLida(IdOrcamentoCotacao, IdUsuarioDestinatario);
        }

    }
}
