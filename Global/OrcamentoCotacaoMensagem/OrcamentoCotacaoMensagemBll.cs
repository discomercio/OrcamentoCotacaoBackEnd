using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrcamentoCotacaoMensagemStatus;
using System.Linq;


namespace OrcamentoCotacaoMensagem
{
    public class OrcamentoCotacaoMensagemBll 
    {
        private readonly OrcamentoCotacaoMensagemData _data;
        private readonly OrcamentoCotacaoMensagemStatusBll _orcamentoCotacaoMensagemStatusBll;

        public OrcamentoCotacaoMensagemBll(OrcamentoCotacaoMensagemData data, OrcamentoCotacaoMensagemStatusBll orcamentoCotacaoMensagemStatusBll)
        {
            _data = data;
            _orcamentoCotacaoMensagemStatusBll = orcamentoCotacaoMensagemStatusBll;
        }

        public async Task<List<TorcamentoCotacaoMensagemFiltro>> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            return await _data.ObterListaMensagem(IdOrcamentoCotacao);
        }

        public async Task<List<TorcamentoCotacaoMensagemFiltro>> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            return await _data.ObterListaMensagemPendente(IdOrcamentoCotacao);
        }

        public int ObterQuantidadeMensagemPendente(int idUsuarioRemetente)
        {
            return _data.ObterQuantidadeMensagemPendente(idUsuarioRemetente);
        }


        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem,
            InfraBanco.ContextoBdGravacao contextoBdGravacao, 
            TorcamentoCotacaoEmailQueue torcamentoCotacaoEmailQueue = null,
            List<TorcamentoCotacaoMensagem> participantesMensagem = null 
            )
        {

            bool saida = false;

            var torcamentoCotacaoMensagem = _data.InserirComTransacao(orcamentoCotacaoMensagem, contextoBdGravacao, torcamentoCotacaoEmailQueue);                        
            
            if (torcamentoCotacaoMensagem != null)
            {
                var orcamentoCotacaoMensagemStatus = new TorcamentoCotacaoMensagemStatus();

                orcamentoCotacaoMensagemStatus.IdOrcamentoCotacaoMensagem = torcamentoCotacaoMensagem.Id;
                orcamentoCotacaoMensagemStatus.IdTipoUsuarioContexto = (short)orcamentoCotacaoMensagem.IdTipoUsuarioContextoDestinatario;
                orcamentoCotacaoMensagemStatus.IdUsuario = orcamentoCotacaoMensagem.IdUsuarioDestinatario;
                orcamentoCotacaoMensagemStatus.Lida = false;
                orcamentoCotacaoMensagemStatus.PendenciaTratada = false;

                var torcamentoCotacaoMensagemStatus = _orcamentoCotacaoMensagemStatusBll.InserirComTransacao(orcamentoCotacaoMensagemStatus, contextoBdGravacao);

                if (participantesMensagem != null && torcamentoCotacaoMensagemStatus != null)
                {

                    var participantesStatus = new TorcamentoCotacaoMensagemStatus();

                    foreach (var item in participantesMensagem)
                    {
                        participantesStatus.IdOrcamentoCotacaoMensagem = item.Id;
                        participantesStatus.IdTipoUsuarioContexto = item.IdTipoUsuarioContextoRemetente;
                        participantesStatus.IdUsuario = item.IdUsuarioRemetente;
                        participantesStatus.Lida = false;
                        participantesStatus.PendenciaTratada = false;

                        contextoBdGravacao.TorcamentoCotacaoMensagemStatus.Add(participantesStatus);
                        contextoBdGravacao.SaveChanges();
                    }                    
                    
                }

                if (torcamentoCotacaoMensagemStatus != null)
                {                    
                    contextoBdGravacao.transacao.Commit();

                    saida = true;
                }
            }

            return saida;            
        }

        public List<TorcamentoCotacaoMensagem> ObterParticipantes(int idORcamentoCotacao, int idDonoOrcamento)
        {
            return _data.ObterParticipantes(idORcamentoCotacao, idDonoOrcamento);
        }

        public bool MarcarLida(int IdOrcamentoCotacao, int idUsuarioRemetente)
        {
            if (idUsuarioRemetente >0)
            {
                return _data.MarcarLida(IdOrcamentoCotacao, idUsuarioRemetente);
            }
            else
            {
                return _data.MarcarLidaCliente(IdOrcamentoCotacao, idUsuarioRemetente);
            }            

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
