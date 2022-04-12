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

        public OrcamentoCotacaoBll(OrcamentoBll orcamentoBll, IOptions<ConfigOrcamentoCotacao> appSettings,
            OrcamentistaEIndicadorBll orcamentistaEIndicadorBll, Usuario.UsuarioBll usuarioBll,
            OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll, PedidoPrepedidoApiBll pedidoPrepedidoApiBll)
        {
            _orcamentoBll = orcamentoBll;
            _pedidoPrepedidoApiBll = pedidoPrepedidoApiBll;
            this.orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
            this.usuarioBll = usuarioBll;
            this.orcamentistaEIndicadorVendedorBll = orcamentistaEIndicadorVendedorBll;
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

        public async Task<List<TcfgOrcamentoCotacaoStatus>> ObterListaStatus(TorcamentoFiltro tOrcamentoFiltro)
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


        public void CadastrarOrcamento(OrcamentoRequestViewModel orcamento, UsuarioLogin usuarioLogado)
        {

            TorcamentoCotacao torcamentoCotacao = new TorcamentoCotacao()
            {
                Loja = orcamento.Loja, //Loja
                NomeCliente = orcamento.ClienteOrcamentoCotacaoDto.NomeCliente,//NomeCliente
                NomeObra = orcamento.ClienteOrcamentoCotacaoDto.NomeObra,//NomeObra
                Email = orcamento.ClienteOrcamentoCotacaoDto.Email, //Email
                Telefone = orcamento.ClienteOrcamentoCotacaoDto.Telefone, //Telefone
                UF = orcamento.ClienteOrcamentoCotacaoDto.Uf, //UF
                Tipo = orcamento.ClienteOrcamentoCotacaoDto.Tipo, //TipoCliente
                Validade = orcamento.Validade, //Validade
                Observacao = orcamento.ObservacoesGerais, //Observacao
                AceiteWhatsApp = orcamento.ConcordaWhatsapp ? 1 : 0, //AceiteWhatsApp
                IdTipoUsuarioContextoCadastro = (int)usuarioLogado.TipoUsuario, //IdTipoUsuarioContextoCadastro
                IdUsuarioCadastro = usuarioLogado.Id
            };

            if (!string.IsNullOrEmpty(orcamento.Vendedor))
            {
                var vendedor = usuarioBll.PorFiltro(new TusuarioFiltro() { usuario = orcamento.Vendedor }).FirstOrDefault();

                if (vendedor == null) throw new ArgumentException("Vendedor não encontrado!");

                torcamentoCotacao.IdVendedor = vendedor.Id;//IdVendedor
            }
            if (!string.IsNullOrEmpty(orcamento.Parceiro))
            {
                var parceiro = orcamentistaEIndicadorBll
                    .BuscarParceiros(new TorcamentistaEindicadorFiltro() { idParceiro = int.Parse(orcamento.Parceiro) }).FirstOrDefault();

                if (parceiro == null) throw new ArgumentException("Parceiro não encontrado!");

                torcamentoCotacao.IdIndicador = parceiro.IdIndicador;//IdIndicador
            }

            if (!string.IsNullOrEmpty(orcamento.VendedorParceiro))
            {
                var vendedoresParceiro = orcamentistaEIndicadorVendedorBll.BuscarVendedoresParceiro(orcamento.Parceiro);
                if (vendedoresParceiro == null) throw new ArgumentException("Nenhum vendedor do parceiro encontrado!");

                torcamentoCotacao.IdIndicadorVendedor = vendedoresParceiro //IdIndicadorVendedor
                    .Where(x => x.Nome == orcamento.VendedorParceiro)
                    .FirstOrDefault().Id;
            }

            //até aqui tudo certo espero...
            //vamos cadastrar o orçamento com os dados principais

            //retornar o Id do orçamento para incluir nos nos outros modelos
        }
    }
}
