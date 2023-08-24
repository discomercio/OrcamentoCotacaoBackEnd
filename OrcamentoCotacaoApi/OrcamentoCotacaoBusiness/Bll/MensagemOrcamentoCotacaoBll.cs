using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using OrcamentoCotacaoMensagem;
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
using OrcamentoCotacaoBusiness.Models.Response.Mensagem;
using Prepedido.Bll;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class MensagemOrcamentoCotacaoBll
    {
        private readonly OrcamentoCotacaoMensagemBll _bll;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll;
        private readonly LojaBll _lojaBll;
        private readonly CfgUnidadeNegocioBll _cfgUnidadeNegocioBll;
        private readonly InfraBanco.ContextoBdProvider _contextoBdProvider;
        private readonly OrcamentoCotacaoEmail.OrcamentoCotacaoEmailBll _orcamentoCotacaoEmailBll;
        private readonly CfgUnidadeNegocioParametroBll _cfgUnidadeNegocioParametroBll;
        private readonly OrcamentoCotacao.OrcamentoCotacaoBll _orcamentoCotacaoBll;
        private readonly OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll _orcamentoCotacaoEmailQueueBll;
        private readonly OrcamentoCotacaoLink.OrcamentoCotacaoLinkBll _orcamentoCotacaoLinkBll;
        private readonly UsuarioBll _usuarioBll;

        public MensagemOrcamentoCotacaoBll(OrcamentoCotacaoMensagemBll bll, OrcamentistaEIndicadorBll orcamentistaEIndicadorBll, LojaBll lojaBll, CfgUnidadeNegocioBll cfgUnidadeNegocioBll,
            CfgUnidadeNegocioParametroBll cfgUnidadeNegocioParametroBll, OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll orcamentoCotacaoEmailQueueBll,
            OrcamentoCotacao.OrcamentoCotacaoBll orcamentoCotacaoBll, OrcamentoCotacaoLink.OrcamentoCotacaoLinkBll orcamentoCotacaoLinkBll, OrcamentoCotacaoEmail.OrcamentoCotacaoEmailBll orcamentoCotacaoEmailBll, InfraBanco.ContextoBdProvider contextoBdProvider,
            UsuarioBll _usuarioBll)
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
            this._usuarioBll = _usuarioBll;
        }

        public async Task<List<TorcamentoCotacaoMensagemFiltro>> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            return await _bll.ObterListaMensagem(IdOrcamentoCotacao);
        }

        public async Task<List<TorcamentoCotacaoMensagemFiltro>> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            return await _bll.ObterListaMensagemPendente(IdOrcamentoCotacao);
        }

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem, int IdUsuarioLogado)
        {
            var saida = false;

            TorcamentoCotacaoMensagem outroParticipante = null;

            if (IdUsuarioLogado == 0)
                outroParticipante = _bll.ObterDadosOutroParticipante(
                    orcamentoCotacaoMensagem.IdOrcamentoCotacao,
                    orcamentoCotacaoMensagem.IdUsuarioDestinatario
                    );

            try
            {
                using (var contextoBdGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {

                    if (IdUsuarioLogado != 0)
                    {
                        var orcamentoCotacaoLink = _orcamentoCotacaoLinkBll.PorFiltro(new TorcamentoCotacaoLinkFiltro() { IdOrcamentoCotacao = orcamentoCotacaoMensagem.IdOrcamentoCotacao, Status = 1 }).FirstOrDefault();

                        if (orcamentoCotacaoLink != null)
                        {
                            TorcamentoCotacaoEmailQueue torcamentoCotacaoEmailQueue = EnviarEmail(orcamentoCotacaoMensagem.IdOrcamentoCotacao, contextoBdGravacao);
                            _bll.EnviarMensagem(orcamentoCotacaoMensagem, contextoBdGravacao, torcamentoCotacaoEmailQueue);
                        }
                        else
                        {
                            _bll.EnviarMensagem(orcamentoCotacaoMensagem, contextoBdGravacao);
                        }

                        saida = true;
                    }
                    else
                    {
                        _bll.EnviarMensagem(orcamentoCotacaoMensagem,
                            contextoBdGravacao,
                            null,
                            outroParticipante);

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

        private TorcamentoCotacaoEmailQueue EnviarEmail(int IdOrcamentoCotacao, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {

            TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueueModel = new InfraBanco.Modelos.TorcamentoCotacaoEmailQueue();

            var orcamento = _orcamentoCotacaoBll.PorFiltroComTransacao(new TorcamentoCotacaoFiltro() { Id = IdOrcamentoCotacao }, contextoBdGravacao).FirstOrDefault();
            var orcamentoCotacaoLink = _orcamentoCotacaoLinkBll.PorFiltroComTransacao(new TorcamentoCotacaoLinkFiltro() { IdOrcamentoCotacao = IdOrcamentoCotacao, Status = 1 }, contextoBdGravacao).FirstOrDefault();

            var loja = _lojaBll.PorFiltroComTransacao(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = orcamento.Loja }, contextoBdGravacao);

            var tcfgUnidadeNegocio = _cfgUnidadeNegocioBll.PorFiltroComTransacao(new TcfgUnidadeNegocioFiltro() { Sigla = loja[0].Unidade_Negocio }, contextoBdGravacao);
            var tcfgUnidadeNegocioParametros = _cfgUnidadeNegocioParametroBll.PorFiltroComTransacao(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id }, contextoBdGravacao);
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

            var torcamentoCotacaoEmailQueue = _orcamentoCotacaoEmailQueueBll.InserirQueueComTemplateEHTMLComTransacao(Int32.Parse(template), orcamentoCotacaoEmailQueueModel, tagHtml, contextoBdGravacao);

            if (torcamentoCotacaoEmailQueue.Id == 0)
            {
                throw new ArgumentException("Não foi possível cadastrar o orçamento. Problema no envio de e-mail!");
            }

            return torcamentoCotacaoEmailQueue;

        }

        public bool MarcarLida(int IdOrcamentoCotacao, int idUsuarioRemetente)
        {
            return _bll.MarcarLida(IdOrcamentoCotacao, idUsuarioRemetente);
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

        public RemetenteDestinatarioResponseViewModel CriarRemetenteUsuarioInterno(TorcamentoCotacao orcamento, int idUsuario, DateTime dataMaxTrocaMsg)
        {
            RemetenteDestinatarioResponseViewModel response = new RemetenteDestinatarioResponseViewModel();
            response.IdOrcamentoCotacao = orcamento.Id;
            response.Status = orcamento.Status;
            response.Validade = orcamento.Validade;
            response.IdTipoUsuarioContextoDestinatario = (int)Constantes.TipoUsuario.CLIENTE;
            response.DataMaxTrocaMsg = dataMaxTrocaMsg;

            setarDonoOrcamento(orcamento, idUsuario, response);

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
                orcamento.IdIndicadorVendedor != 0
                )
            {
                response.IdDonoOrcamento = orcamento.IdIndicadorVendedor;
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

        public ListaQuantidadeMensagemPendenteResponse ObterQuantidadeMensagemPendentePorLojas(UsuarioLogin usuario)
        {
            var response = new ListaQuantidadeMensagemPendenteResponse();
            response.Sucesso = false;

            var lojasUnidades = _usuarioBll.BuscarLojasEUnidadesNegocio(usuario);
            if (lojasUnidades.ListaLojaEUnidadeNegocio == null)
            {
                response.Mensagem = "Falha ao lista de quantidade de mensagem pendente por loja!";
                return response;
            }

            response.ListaQtdeMensagemPendente = new List<QuantidadeMensagemPendenteResponse>();

            var unidades = lojasUnidades.ListaLojaEUnidadeNegocio.DistinctBy(x => x.UnidadeNegocio).Select(x => x.UnidadeNegocio);
            foreach (var unidade in unidades)
            {
                var tcfgUnidadeNegocio = _cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { Sigla = unidade });
                var periodoMaxConsulta = _cfgUnidadeNegocioParametroBll
                    .PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id, IdCfgParametro = 20 }).FirstOrDefault();

                DateTime dataInicio = DateTime.Now.AddDays(-int.Parse(periodoMaxConsulta.Valor));

                var lojasPorUnidade = lojasUnidades.ListaLojaEUnidadeNegocio.Where(x => x.UnidadeNegocio == unidade).Select(x => x.Loja).ToList();
                var retorno = _bll.ObterQuantidadeMensagemPendentePorLojas(usuario.Id, (int)usuario.TipoUsuario, lojasPorUnidade, dataInicio).ToList();
                if (retorno.Count() == 0)
                {
                    response.Sucesso = true;
                    //response.ListaQtdeMensagemPendente = null;
                    return response;
                }
                var lojasDistinct = retorno.Select(x => (string)x.GetType().GetProperty("loja").GetValue(x, null)).Distinct().ToList();
                
                foreach( var x in lojasDistinct )
                {
                    var qtde = retorno.Where(y => (string)y.GetType().GetProperty("loja").GetValue(y, null) == x).Distinct().Count();
                    var qtdeMsgELoja = new QuantidadeMensagemPendenteResponse();
                    qtdeMsgELoja.Qtde = qtde;
                    qtdeMsgELoja.Loja = x;
                    response.ListaQtdeMensagemPendente.Add(qtdeMsgELoja);
                }
            }

            response.Sucesso = true;

            return response;
        }
    }
}
