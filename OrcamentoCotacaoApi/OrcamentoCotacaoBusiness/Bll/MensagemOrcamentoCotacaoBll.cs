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
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll;

        public MensagemOrcamentoCotacaoBll(MensagemBll bll, OrcamentistaEIndicadorBll orcamentistaEIndicadorBll)
        {
            _bll = bll;
            _orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            return await _bll.ObterListaMensagem(IdOrcamentoCotacao);
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            return await _bll.ObterListaMensagemPendente(IdOrcamentoCotacao);
        }

        public int ObterQuantidadeMensagemPendente(int idUsuarioRemetente)
        {
            return _bll.ObterQuantidadeMensagemPendente(idUsuarioRemetente);
        }

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            return _bll.EnviarMensagem(orcamentoCotacaoMensagem);
        }

        public bool MarcarLida(int IdOrcamentoCotacao)
        {
            return _bll.MarcarLida(IdOrcamentoCotacao);
        }

        public bool MarcarPendencia(int IdOrcamentoCotacao)
        {
            return _bll.MarcarPendencia(IdOrcamentoCotacao);
        }

        public bool DesmarcarPendencia(int IdOrcamentoCotacao)
        {
            return _bll.DesmarcarPendencia(IdOrcamentoCotacao);
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

            setarDonoOrcamento(orcamento, idUsuario,response);

            if (orcamento.IdIndicadorVendedor != null &&
                orcamento.IdIndicadorVendedor == idUsuario)
            {
                response.IdUsuarioRemetente = (int)orcamento.IdIndicadorVendedor;
                response.IdTipoUsuarioContextoRemetente = (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO;
                return response;
            }
            if (orcamento.IdIndicador != null &&
                orcamento.IdIndicador == idUsuario)
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

        public void setarDonoOrcamento(TorcamentoCotacao orcamento, int idUsuarioLogado, RemetenteDestinatarioResponseViewModel response)
        {
            
            response.DonoOrcamento = false;

            if (orcamento.IdIndicador == null) orcamento.IdIndicador = 0;
            if (orcamento.IdIndicadorVendedor == null) orcamento.IdIndicadorVendedor = 0;

            if (orcamento.IdVendedor != 0 &&
                orcamento.IdIndicador != 0 &&
                orcamento.IdIndicadorVendedor !=0 
                )
            {                                
                response.IdDonoOrcamento =  orcamento.IdIndicadorVendedor;                
            }

            if (orcamento.IdVendedor != 0 &&
                orcamento.IdIndicador != 0 &&
                orcamento.IdIndicadorVendedor == 0
                )
            {
                //var orcamentistaEIndicadorBll = _orcamentistaEIndicadorBll.BuscarParceiros(new TorcamentistaEindicadorFiltro() { idParceiro = orcamento.Id}).FirstOrDefault();
                response.IdDonoOrcamento = orcamento.IdIndicador;
            }

            if (orcamento.IdVendedor != 0 &&
                 orcamento.IdIndicador == 0 &&
                 orcamento.IdIndicadorVendedor == 0 
                )
            {
                response.IdDonoOrcamento = orcamento.IdVendedor;
            }

            if (response.IdDonoOrcamento == idUsuarioLogado)
            {
                response.DonoOrcamento = true;
            }

        }

    }
}
