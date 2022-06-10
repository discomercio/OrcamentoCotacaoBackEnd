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

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            return await _data.ObterListaMensagemPendente(IdOrcamentoCotacao);
        }

        public int ObterQuantidadeMensagemPendente(int idUsuarioRemetente)
        {
            return _data.ObterQuantidadeMensagemPendente(idUsuarioRemetente);
        }


        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            return _data.EnviarMensagem(orcamentoCotacaoMensagem);
        }

        public bool MarcarLida(int IdOrcamentoCotacao)
        {
            return _data.MarcarLida(IdOrcamentoCotacao);
        }

        public bool MarcarPendencia(int IdOrcamentoCotacao)
        {
            return _data.MarcarPendencia(IdOrcamentoCotacao);
        }

        public bool DesmarcarPendencia(int IdOrcamentoCotacao)
        {
            return _data.DesmarcarPendencia(IdOrcamentoCotacao);
        }

    }
}
