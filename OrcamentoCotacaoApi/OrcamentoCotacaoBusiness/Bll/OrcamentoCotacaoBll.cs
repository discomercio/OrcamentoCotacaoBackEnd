using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.Extensions.Options;
using Orcamento;
using Orcamento.Dto;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using PrepedidoBusiness.Bll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentoCotacaoBll
    {
        private readonly OrcamentoBll _orcamentoBll;
        private readonly PedidoPrepedidoApiBll _pedidoPrepedidoApiBll;
        private readonly ConfigOrcamentoCotacao _appSettings;
        private readonly OrcamentistaEIndicadorBll orcamentistaEIndicadorBll;
        private readonly OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll;
        private readonly Usuario.UsuarioBll usuarioBll;
        private readonly OrcamentoCotacao.OrcamentoCotacaoBll orcamentoCotacaoBll;
        private readonly InfraBanco.ContextoBdProvider contextoBdProvider;
        private readonly OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll;

        public OrcamentoCotacaoBll(OrcamentoBll orcamentoBll, IOptions<ConfigOrcamentoCotacao> appSettings,
            OrcamentistaEIndicadorBll orcamentistaEIndicadorBll, Usuario.UsuarioBll usuarioBll,
            OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll, PedidoPrepedidoApiBll pedidoPrepedidoApiBll,
            OrcamentoCotacao.OrcamentoCotacaoBll orcamentoCotacaoBll,
            InfraBanco.ContextoBdProvider contextoBdProvider, OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll)
        {
            _orcamentoBll = orcamentoBll;
            _pedidoPrepedidoApiBll = pedidoPrepedidoApiBll;
            this.orcamentoCotacaoBll = orcamentoCotacaoBll;
            this.contextoBdProvider = contextoBdProvider;
            this.orcamentoCotacaoOpcaoBll = orcamentoCotacaoOpcaoBll;
            this.orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
            this.usuarioBll = usuarioBll;
            this.orcamentistaEIndicadorVendedorBll = orcamentistaEIndicadorVendedorBll;
            _appSettings = appSettings.Value;

        }

        public List<OrcamentoCotacaoListaDto> PorFiltro(TorcamentoFiltro tOrcamentoFiltro)
        {
            if (tOrcamentoFiltro.Origem == "ORCAMENTOS")
            {
                var orcamentoCotacaoListaDto = orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro()
                {
                    Tusuario = true,
                    LimitarData = true,
                    Loja = tOrcamentoFiltro.Loja
                });

                List<OrcamentoCotacaoListaDto> retorno = new List<OrcamentoCotacaoListaDto>();
                if (orcamentoCotacaoListaDto != null)
                {
                    orcamentoCotacaoListaDto.ForEach(x => retorno.Add(new OrcamentoCotacaoListaDto()
                    {
                        NumeroOrcamento = x.IdOrcamento,
                        NumPedido = x.IdPedido,
                        Cliente_Obra = $"{x.NomeCliente} - {x.NomeObra}",
                        Vendedor = x.Tusuarios.Usuario,
                        Parceiro = "Parceiro",
                        VendedorParceiro = "VendedorParceiro",
                        Valor = "0",
                        Status = x.Status.ToString(),
                        VistoEm = "",
                        Mensagem = x.Status == 7 ? "Sim" : "Não",
                        DtCadastro = x.DataCadastro,
                        DtExpiracao = x.Validade,
                        DtInicio = tOrcamentoFiltro.DtInicio,
                        DtFim = tOrcamentoFiltro.DtFim
                    }));
                }
                return retorno;
                //return _orcamentoBll.OrcamentoCotacaoPorFiltro(tOrcamentoFiltro);
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

        public async Task<List<TcfgSelectItem>> ObterListaStatus(TorcamentoFiltro tOrcamentoFiltro)
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
            return _orcamentoBll.EnviarMensagem(orcamentoCotacaoMensagem);
        }


        public bool MarcarMensagemComoLida(int IdOrcamentoCotacao, int idUsuarioDestinatario)
        {
            return _orcamentoBll.MarcarMensagemComoLida(IdOrcamentoCotacao, idUsuarioDestinatario);
        }


        public void CadastrarOrcamentoCotacao(OrcamentoRequestViewModel orcamento, UsuarioLogin usuarioLogado)
        {

            //TODO: VALIDAR OrcamentoRequestViewModel
            if (orcamento.ListaOrcamentoCotacaoDto.Count <= 0) throw new ArgumentException("Necessário ter ao menos uma opção de orçamento!");


            using (var dbGravacao = contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var tOrcamentoCotacao = MontarTorcamentoCotacao(orcamento, usuarioLogado);

                    var retorno = orcamentoCotacaoBll.InserirComTransacao(tOrcamentoCotacao, dbGravacao);

                    if(tOrcamentoCotacao.Id == 0) throw new ArgumentException("Ops! Não gerou Id!");

                    var opcoes = orcamentoCotacaoOpcaoBll.CadastrarOrcamentoCotacaoOpcoesComTransacao(orcamento.ListaOrcamentoCotacaoDto, tOrcamentoCotacao.Id,
                        tOrcamentoCotacao.IdTipoUsuarioContextoCadastro, tOrcamentoCotacao.IdUsuarioCadastro, dbGravacao);

                    throw new ArgumentException("Ops! Não vamos salvar ainda...");

                    dbGravacao.transacao.Commit();
                }
                catch (Exception ex)
                {
                    dbGravacao.transacao.Rollback();
                    throw new ArgumentException("Falha ao gravar orçamento!");
                }


            }

            //retornar o Id do orçamento para incluir nos nos outros modelos
        }

        private TorcamentoCotacao MontarTorcamentoCotacao(OrcamentoRequestViewModel orcamento, UsuarioLogin usuarioLogado)
        {
            TorcamentoCotacao torcamentoCotacao = new TorcamentoCotacao()
            {
                Loja = orcamento.Loja, //Loja
                NomeCliente = orcamento.ClienteOrcamentoCotacaoDto.NomeCliente,//NomeCliente
                NomeObra = orcamento.ClienteOrcamentoCotacaoDto.NomeObra,//NomeObra
                Email = orcamento.ClienteOrcamentoCotacaoDto.Email, //Email
                Telefone = orcamento.ClienteOrcamentoCotacaoDto.Telefone, //Telefone
                UF = orcamento.ClienteOrcamentoCotacaoDto.Uf, //UF
                TipoCliente = orcamento.ClienteOrcamentoCotacaoDto.Tipo, //TipoCliente
                Validade = orcamento.Validade, //Validade
                Observacao = orcamento.ObservacoesGerais, //Observacao
                AceiteWhatsApp = orcamento.ConcordaWhatsapp ? 1 : 0, //AceiteWhatsApp
                IdTipoUsuarioContextoCadastro = (int)usuarioLogado.TipoUsuario, //IdTipoUsuarioContextoCadastro
                IdUsuarioCadastro = usuarioLogado.Id,
                DataCadastro = DateTime.Now.Date.Date,
                DataHoraCadastro = DateTime.Now,
                DataUltStatus = DateTime.Now.Date,
                DataHoraUltStatus = DateTime.Now,
                Status = 1
            };

            if (!string.IsNullOrEmpty(orcamento.Vendedor))
            {
                var vendedor = usuarioBll.PorFiltro(new TusuarioFiltro() { usuario = orcamento.Vendedor }).FirstOrDefault();

                if (vendedor == null) throw new ArgumentException("Vendedor não encontrado!");

                torcamentoCotacao.IdVendedor = vendedor.Id;//IdVendedor
            }
            if (!string.IsNullOrEmpty(orcamento.Parceiro))
            {
                var torcamentista = orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { apelido = orcamento.Parceiro });

                if (torcamentista == null) throw new ArgumentException("Parceiro não encontrado!");

                torcamentoCotacao.IdIndicador = torcamentista.IdIndicador;//IdIndicador
            }

            if (!string.IsNullOrEmpty(orcamento.VendedorParceiro))
            {
                var vendedoresParceiro = orcamentistaEIndicadorVendedorBll.BuscarVendedoresParceiro(orcamento.Parceiro);
                if (vendedoresParceiro == null) throw new ArgumentException("Nenhum vendedor do parceiro encontrado!");

                torcamentoCotacao.IdIndicadorVendedor = vendedoresParceiro //IdIndicadorVendedor
                    .Where(x => x.Nome == orcamento.VendedorParceiro)
                    .FirstOrDefault().Id;
            }

            return torcamentoCotacao;
        }
    }
}
