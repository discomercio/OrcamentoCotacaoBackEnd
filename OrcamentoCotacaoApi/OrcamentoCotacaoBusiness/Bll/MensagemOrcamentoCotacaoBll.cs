using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Mensagem;
using OrcamentoCotacaoBusiness.Models.Response;
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

        public RemetenteDestinatarioResponseViewModel CriarRemetenteCliente(TorcamentoCotacao orcamento)
        {
            RemetenteDestinatarioResponseViewModel response = new RemetenteDestinatarioResponseViewModel();
            response.IdOrcamentoCotacao = orcamento.Id;
            response.IdTipoUsuarioContextoRemetente = (int)Constantes.TipoUsuario.CLIENTE;

            if (orcamento.IdIndicadorVendedor != null)
            {
                response.IdUsuarioDestinatario = (int)orcamento.IdIndicadorVendedor;
                response.IdTipoUsuarioContextoDestinatario = (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO;
            }
            else if (orcamento.IdIndicador != null)
            {
                response.IdUsuarioDestinatario = (int)orcamento.IdIndicador;
                response.IdTipoUsuarioContextoDestinatario = (int)Constantes.TipoUsuario.PARCEIRO;
            }
            else
            {
                response.IdUsuarioDestinatario = (int)orcamento.IdVendedor;
                response.IdTipoUsuarioContextoDestinatario = (int)Constantes.TipoUsuario.VENDEDOR;
            }

            return response;
        }

        public RemetenteDestinatarioResponseViewModel CriarRemetenteUsuarioInterno(TorcamentoCotacao orcamento, int idUsuario)
        {
            RemetenteDestinatarioResponseViewModel response = new RemetenteDestinatarioResponseViewModel();
            response.IdOrcamentoCotacao = orcamento.Id;
            response.IdTipoUsuarioContextoDestinatario = (int)Constantes.TipoUsuario.CLIENTE;

            if (orcamento.IdIndicadorVendedor != null && orcamento.IdIndicadorVendedor == idUsuario)
            {
                response.IdUsuarioRemetente = (int)orcamento.IdIndicadorVendedor;
                response.IdTipoUsuarioContextoRemetente = (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO;
                return response;
            }

            if (orcamento.IdIndicador != null && orcamento.IdIndicador == idUsuario)
            {
                response.IdUsuarioRemetente = (int)orcamento.IdIndicador;
                response.IdTipoUsuarioContextoRemetente = (int)Constantes.TipoUsuario.PARCEIRO;
                return response;
            }

            if (orcamento.IdVendedor == idUsuario)
            {
                response.IdUsuarioRemetente = (int)orcamento.IdVendedor;
                response.IdTipoUsuarioContextoRemetente = (int)Constantes.TipoUsuario.VENDEDOR;
                return response;
            }

            return null;
        }
    }
}
