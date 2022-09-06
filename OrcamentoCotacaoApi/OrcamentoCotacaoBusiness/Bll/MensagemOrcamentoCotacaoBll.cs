using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Mensagem;
using InfraIdentity;
using OrcamentoCotacaoBusiness.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loja;
using Cfg.CfgUnidadeNegocio;
using Cfg.CfgUnidadeNegocioParametro;
using System.Linq;
using System.Text.Json;
using System;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class MensagemOrcamentoCotacaoBll
    {
        private readonly MensagemBll _bll;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll;
        private readonly LojaBll _lojaBll;
        private readonly CfgUnidadeNegocioBll _cfgUnidadeNegocioBll;
        private readonly InfraBanco.ContextoBdProvider _contextoBdProvider;
        private readonly OrcamentoCotacaoEmail.OrcamentoCotacaoEmailBll _orcamentoCotacaoEmailBll;
        private readonly CfgUnidadeNegocioParametroBll _cfgUnidadeNegocioParametroBll;
        private readonly OrcamentoCotacao.OrcamentoCotacaoBll _orcamentoCotacaoBll;
        private readonly OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll _orcamentoCotacaoEmailQueueBll;
        private readonly OrcamentoCotacaoLink.OrcamentoCotacaoLinkBll _orcamentoCotacaoLinkBll;

        public MensagemOrcamentoCotacaoBll(MensagemBll bll, OrcamentistaEIndicadorBll orcamentistaEIndicadorBll, LojaBll lojaBll, CfgUnidadeNegocioBll cfgUnidadeNegocioBll, 
            CfgUnidadeNegocioParametroBll cfgUnidadeNegocioParametroBll, OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll orcamentoCotacaoEmailQueueBll, 
            OrcamentoCotacao.OrcamentoCotacaoBll orcamentoCotacaoBll, OrcamentoCotacaoLink.OrcamentoCotacaoLinkBll orcamentoCotacaoLinkBll, OrcamentoCotacaoEmail.OrcamentoCotacaoEmailBll orcamentoCotacaoEmailBll, InfraBanco.ContextoBdProvider contextoBdProvider)
        {
            _bll = bll;
            _orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
            _lojaBll = lojaBll;
            _cfgUnidadeNegocioBll = cfgUnidadeNegocioBll;
            _cfgUnidadeNegocioParametroBll = cfgUnidadeNegocioParametroBll;
            _orcamentoCotacaoEmailQueueBll = orcamentoCotacaoEmailQueueBll;
            _orcamentoCotacaoBll = orcamentoCotacaoBll;
            _orcamentoCotacaoLinkBll = orcamentoCotacaoLinkBll;
            _orcamentoCotacaoEmailBll = orcamentoCotacaoEmailBll;
            _contextoBdProvider = contextoBdProvider;
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

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem, int IdUsuarioLogado)
        {
            var saida = false;

            try
            {
                using (var contextoBdGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {

                    if (IdUsuarioLogado != 0)
                    {
                        EnviarEmail(orcamentoCotacaoMensagem.IdOrcamentoCotacao, contextoBdGravacao);
                        _bll.EnviarMensagem(orcamentoCotacaoMensagem, contextoBdGravacao);
                        saida = true;
                    }
                    else
                    {
                        _bll.EnviarMensagem(orcamentoCotacaoMensagem, contextoBdGravacao);
                        saida = true;
                    }
                }
            }
            catch
            {
                saida = false;
                throw new ArgumentException("Não foi possível enviar a mensagem");
            }

            return saida;
        }

        private void EnviarEmail(int IdOrcamentoCotacao, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {

            TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueueModel = new InfraBanco.Modelos.TorcamentoCotacaoEmailQueue();

            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro() { Id = IdOrcamentoCotacao }).FirstOrDefault();
            var orcamentoCotacaoLink = _orcamentoCotacaoLinkBll.PorFiltro(new TorcamentoCotacaoLinkFiltro() { IdOrcamentoCotacao = IdOrcamentoCotacao }).FirstOrDefault();
            var loja = _lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = orcamento.Loja });

            var tcfgUnidadeNegocio = _cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { Sigla = loja[0].Unidade_Negocio });
            var tcfgUnidadeNegocioParametros = _cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id });
            var nomeEmpresa = "";
            var logoEmpresa = "";
            var urlBaseFront = "";
            var template = "";

            foreach (var item in tcfgUnidadeNegocioParametros)
            {
                switch (item.IdCfgParametro)
                {
                    case 2:
                        urlBaseFront = item.Valor;
                        break;
                    case 5:
                        orcamentoCotacaoEmailQueueModel.From = item.Valor;
                        break;
                    case 6:
                        orcamentoCotacaoEmailQueueModel.FromDisplayName = item.Valor;
                        nomeEmpresa = item.Valor;
                        break;
                    case 34:
                        logoEmpresa = item.Valor;
                        break;
                    case 36:
                        template = item.Valor;
                        break;
                }
            }
            
            orcamentoCotacaoEmailQueueModel.IdCfgUnidadeNegocio = tcfgUnidadeNegocioParametros[0].IdCfgUnidadeNegocio;
            orcamentoCotacaoEmailQueueModel.To = orcamento.Email;
            orcamentoCotacaoEmailQueueModel.Cc = "";
            orcamentoCotacaoEmailQueueModel.Bcc = "";

            string[] tagHtml = new string[] {
                        orcamento.NomeCliente,
                        nomeEmpresa,
                        orcamentoCotacaoLink.Guid.ToString(),
                        orcamento.Id.ToString(),
                        urlBaseFront,
                        logoEmpresa
                    };

            var torcamentoCotacaoEmailQueue = _orcamentoCotacaoEmailQueueBll.InserirQueueComTemplateEHTMLComTransacao(Int32.Parse(template), orcamentoCotacaoEmailQueueModel, tagHtml,contextoBdGravacao);
                       
            if (torcamentoCotacaoEmailQueue.Id == 0)
            {
                throw new ArgumentException("Não foi possível cadastrar o orçamento. Problema no envio de e-mail!");
            }
            else
            {
                TorcamentoCotacaoEmail orcamentoCotacaoEmailModel = new InfraBanco.Modelos.TorcamentoCotacaoEmail();
                orcamentoCotacaoEmailModel.IdOrcamentoCotacao = orcamento.Id;
                orcamentoCotacaoEmailModel.IdOrcamentoCotacaoEmailQueue = torcamentoCotacaoEmailQueue.Id;

                try
                {
                    var torcamentoCotacaoEmail = _orcamentoCotacaoEmailBll.InserirComTransacao(orcamentoCotacaoEmailModel, contextoBdGravacao);
                }
                catch
                {
                    throw new ArgumentException("Não foi possível cadastrar o orçamento. Problema no envio de e-mail!");
                }
                
            }


        }

        public bool MarcarLida(int IdOrcamentoCotacao, int idUsuarioRemetente)
        {
            return _bll.MarcarLida(IdOrcamentoCotacao,idUsuarioRemetente);
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
            response.Status = orcamento.Status;
            response.Validade = orcamento.Validade;
            response.IdTipoUsuarioContextoDestinatario = (int)Constantes.TipoUsuario.CLIENTE;

            setarDonoOrcamento(orcamento, idUsuario,response);

            if (orcamento.IdIndicadorVendedor != null &&
                orcamento.IdIndicadorVendedor == idUsuario)
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
