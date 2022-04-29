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
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll;
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEIndicadorVendedorBll;
        private readonly Usuario.UsuarioBll _usuarioBll;
        private readonly OrcamentoCotacao.OrcamentoCotacaoBll _orcamentoCotacaoBll;
        private readonly InfraBanco.ContextoBdProvider _contextoBdProvider;
        private readonly OrcamentoCotacaoOpcaoBll _orcamentoCotacaoOpcaoBll;

        public OrcamentoCotacaoBll(
            OrcamentoBll orcamentoBll, 
            IOptions<ConfigOrcamentoCotacao> appSettings,
            OrcamentistaEIndicadorBll orcamentistaEIndicadorBll, 
            Usuario.UsuarioBll usuarioBll,
            OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll, 
            PedidoPrepedidoApiBll pedidoPrepedidoApiBll,
            OrcamentoCotacao.OrcamentoCotacaoBll orcamentoCotacaoBll,
            InfraBanco.ContextoBdProvider contextoBdProvider, 
            OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll
            )
        {
            _orcamentoBll = orcamentoBll;
            _pedidoPrepedidoApiBll = pedidoPrepedidoApiBll;
            _orcamentoCotacaoBll = orcamentoCotacaoBll;
            _contextoBdProvider = contextoBdProvider;
            _orcamentoCotacaoOpcaoBll = orcamentoCotacaoOpcaoBll;
            _orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
            _usuarioBll = usuarioBll;
            _orcamentistaEIndicadorVendedorBll = orcamentistaEIndicadorVendedorBll;
            _appSettings = appSettings.Value;
        }

        public List<OrcamentoCotacaoListaDto> PorFiltro(TorcamentoFiltro tOrcamentoFiltro, UsuarioLogin usuarioLogin)
        {
            tOrcamentoFiltro.TipoUsuario = usuarioLogin.TipoUsuario;
            tOrcamentoFiltro.Apelido = usuarioLogin.Nome;
            tOrcamentoFiltro.IdUsuario = usuarioLogin.Id;

            if (tOrcamentoFiltro.TipoUsuario.HasValue)
            {
                //VÊ TUDO
                //if (tOrcamentoFiltro.TipoUsuario.Value == (int)Constantes.TipoUsuario.VENDEDOR)

                //VÊ TUDO somente dele e vendedores parceiros
                if (tOrcamentoFiltro.TipoUsuario.Value == (int)Constantes.TipoUsuario.PARCEIRO)
                {
                    tOrcamentoFiltro.Vendedor = usuarioLogin.VendedorResponsavel;
                    tOrcamentoFiltro.Parceiro = usuarioLogin.IdParceiro;
                }

                //VÊ somente suas vendas
                if (tOrcamentoFiltro.TipoUsuario.Value == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    tOrcamentoFiltro.VendedorParceiro = usuarioLogin.Nome;
                }
            }

            if (tOrcamentoFiltro.Origem == "ORCAMENTOS")
            {
                var orcamentoCotacaoListaDto = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro()
                {
                    Tusuario = true,
                    LimitarData = true,
                    Loja = tOrcamentoFiltro.Loja
                });

                List<OrcamentoCotacaoListaDto> lista = new List<OrcamentoCotacaoListaDto>();
                if (orcamentoCotacaoListaDto != null)
                {
                    var vendedores = _usuarioBll.PorFiltro(new TusuarioFiltro { });
                    var parceiros = _orcamentistaEIndicadorBll.BuscarParceiros(new TorcamentistaEindicadorFiltro { });
                    var vendParceiros = _orcamentistaEIndicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro { });

                    orcamentoCotacaoListaDto.ForEach(x => lista.Add(new OrcamentoCotacaoListaDto()
                    {
                        NumeroOrcamento = x.Id.ToString(),
                        NumPedido = x.IdPedido,
                        Cliente_Obra = $"{x.NomeCliente} - {x.NomeObra}",
                        Vendedor = vendedores.FirstOrDefault(v=>v.Id == x.IdVendedor)?.Nome,
                        Parceiro = parceiros.FirstOrDefault(v => v.IdIndicador == x.IdIndicador)?.Apelido,
                        VendedorParceiro = vendParceiros.FirstOrDefault(v => v.Id == x.IdIndicadorVendedor)?.Nome,
                        Valor = "0",
                        Status = x.StatusNome,
                        VistoEm = "",
                        Mensagem = _orcamentoBll.ObterListaMensagemPendente(x.Id, x.Tusuarios.Id).Result.Any() ? "Sim" : "Não",
                        DtCadastro = x.DataCadastro,
                        DtExpiracao = x.Validade,
                        DtInicio = tOrcamentoFiltro.DtInicio,
                        DtFim = tOrcamentoFiltro.DtFim
                    }));
                }

                return lista;
            }
            else if (tOrcamentoFiltro.Origem == "PENDENTES") //PrePedido/Em Aprovação [tOrcamentos]
            {
                var lista = _orcamentoBll.OrcamentoPorFiltro(tOrcamentoFiltro);

                //foreach (var item in lista)
                //    item.Status = TcfgOrcamentoStatus.ObterStatus(item.Status);

                return lista;
            }
            else //if (tOrcamentoFiltro.Origem == "PEDIDOS")
            {
                var lista = _pedidoPrepedidoApiBll.ListarPedidos(tOrcamentoFiltro);

                foreach (var item in lista)
                    item.Status = TcfgPedidoStatus.ObterStatus(item.Status);

                return lista;
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

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var tOrcamentoCotacao = MontarTorcamentoCotacao(orcamento, usuarioLogado);

                    var retorno = _orcamentoCotacaoBll.InserirComTransacao(tOrcamentoCotacao, dbGravacao);

                    if(tOrcamentoCotacao.Id == 0) throw new ArgumentException("Ops! Não gerou Id!");

                    var opcoes = _orcamentoCotacaoOpcaoBll.CadastrarOrcamentoCotacaoOpcoesComTransacao(orcamento.ListaOrcamentoCotacaoDto, tOrcamentoCotacao.Id,
                        usuarioLogado, dbGravacao, orcamento.Loja);

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
                AceiteWhatsApp = orcamento.ConcordaWhatsapp, //AceiteWhatsApp
                IdTipoUsuarioContextoCadastro = (int)usuarioLogado.TipoUsuario, //IdTipoUsuarioContextoCadastro
                IdUsuarioCadastro = usuarioLogado.Id,
                DataCadastro = DateTime.Now.Date.Date,
                DataHoraCadastro = DateTime.Now,
                DataUltStatus = DateTime.Now.Date,
                DataHoraUltStatus = DateTime.Now,
                Status = 1,
                StEtgImediata = orcamento.EntregaImediata ? 1 : 0,
                PrevisaoEntregaData = orcamento.DataEntregaImediata
            };

            if (!string.IsNullOrEmpty(orcamento.Vendedor))
            {
                var vendedor = _usuarioBll.PorFiltro(new TusuarioFiltro() { usuario = orcamento.Vendedor }).FirstOrDefault();

                if (vendedor == null) throw new ArgumentException("Vendedor não encontrado!");

                torcamentoCotacao.IdVendedor = vendedor.Id;//IdVendedor
            }
            if (!string.IsNullOrEmpty(orcamento.Parceiro))
            {
                if(usuarioLogado.TipoUsuario != (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var torcamentista = _orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { apelido = orcamento.Parceiro, acessoHabilitado = 1 });

                    if (torcamentista == null) throw new ArgumentException("Parceiro não encontrado!");

                    torcamentoCotacao.IdIndicador = torcamentista.IdIndicador;//IdIndicador
                }

                if (usuarioLogado.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var torcamentista = _orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { idParceiro = int.Parse(orcamento.Parceiro), acessoHabilitado = 1 });

                    if (torcamentista == null) throw new ArgumentException("Parceiro não encontrado!");

                    torcamentoCotacao.IdIndicador = torcamentista.IdIndicador;//IdIndicador
                }
            }

            if (!string.IsNullOrEmpty(orcamento.VendedorParceiro))
            {
                if (usuarioLogado.TipoUsuario != (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var vendedoresParceiro = _orcamentistaEIndicadorVendedorBll.BuscarVendedoresParceiro(orcamento.Parceiro);
                    if (vendedoresParceiro == null) throw new ArgumentException("Nenhum vendedor do parceiro encontrado!");

                    torcamentoCotacao.IdIndicadorVendedor = vendedoresParceiro //IdIndicadorVendedor
                        .Where(x => x.Nome == orcamento.VendedorParceiro)
                        .FirstOrDefault().Id;
                }

                if (usuarioLogado.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var vendedoresParceiro = _orcamentistaEIndicadorVendedorBll.BuscarVendedoresParceiroPorId(int.Parse(orcamento.Parceiro));
                    if (vendedoresParceiro == null) throw new ArgumentException("Nenhum vendedor do parceiro encontrado!");

                    torcamentoCotacao.IdIndicadorVendedor = vendedoresParceiro //IdIndicadorVendedor
                        .Where(x => x.Email.ToUpper() == orcamento.VendedorParceiro)
                        .FirstOrDefault().Id;
                }


            }

            return torcamentoCotacao;
        }
    }
}
