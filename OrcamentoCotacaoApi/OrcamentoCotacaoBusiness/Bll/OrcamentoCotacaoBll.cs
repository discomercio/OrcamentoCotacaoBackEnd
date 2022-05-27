using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.Extensions.Options;
using Orcamento;
using Loja;
using Orcamento.Dto;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using PrepedidoBusiness.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cfg.CfgUnidadeNegocioParametro;
using Cfg.CfgUnidadeNegocio;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentoCotacaoBll
    {
        private readonly OrcamentoBll _orcamentoBll;
        private readonly MensagemOrcamentoCotacaoBll _mensagemBll;
        private readonly PedidoPrepedidoApiBll _pedidoPrepedidoApiBll;
        private readonly ConfigOrcamentoCotacao _appSettings;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll;
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEIndicadorVendedorBll;
        private readonly Usuario.UsuarioBll _usuarioBll;
        private readonly OrcamentoCotacao.OrcamentoCotacaoBll _orcamentoCotacaoBll;
        private readonly InfraBanco.ContextoBdProvider _contextoBdProvider;
        private readonly OrcamentoCotacaoOpcaoBll _orcamentoCotacaoOpcaoBll;
        private readonly OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll _orcamentoCotacaoEmailQueueBll;
        private readonly LojaBll _lojaBll;
        private readonly CfgUnidadeNegocioBll cfgUnidadeNegocioBll;
        private readonly CfgUnidadeNegocioParametroBll cfgUnidadeNegocioParametroBll;

        public OrcamentoCotacaoBll(
            OrcamentoBll orcamentoBll,
            MensagemOrcamentoCotacaoBll mensagemBll,
            IOptions<ConfigOrcamentoCotacao> appSettings,
            OrcamentistaEIndicadorBll orcamentistaEIndicadorBll,
            Usuario.UsuarioBll usuarioBll,
            OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll,
            PedidoPrepedidoApiBll pedidoPrepedidoApiBll,
            OrcamentoCotacao.OrcamentoCotacaoBll orcamentoCotacaoBll,
            InfraBanco.ContextoBdProvider contextoBdProvider,
            OrcamentoCotacaoOpcaoBll orcamentoCotacaoOpcaoBll,
            OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll orcamentoCotacaoEmailQueueBll,
            LojaBll lojaBll,
            CfgUnidadeNegocioBll cfgUnidadeNegocioBll,
            CfgUnidadeNegocioParametroBll cfgUnidadeNegocioParametroBll
            )
        {
            _orcamentoBll = orcamentoBll;
            _mensagemBll = mensagemBll;
            _pedidoPrepedidoApiBll = pedidoPrepedidoApiBll;
            _orcamentoCotacaoBll = orcamentoCotacaoBll;
            _contextoBdProvider = contextoBdProvider;
            _orcamentoCotacaoOpcaoBll = orcamentoCotacaoOpcaoBll;
            _orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
            _usuarioBll = usuarioBll;
            _orcamentistaEIndicadorVendedorBll = orcamentistaEIndicadorVendedorBll;
            _appSettings = appSettings.Value;
            _orcamentoCotacaoEmailQueueBll = orcamentoCotacaoEmailQueueBll;
            _lojaBll = lojaBll;
            this.cfgUnidadeNegocioBll = cfgUnidadeNegocioBll;
            this.cfgUnidadeNegocioParametroBll = cfgUnidadeNegocioParametroBll;
        }

        public List<OrcamentoCotacaoListaDto> PorFiltro(TorcamentoFiltro tOrcamentoFiltro, UsuarioLogin usuarioLogin)
        {
            TorcamentoCotacaoFiltro orcamentoCotacaoFiltro = new TorcamentoCotacaoFiltro
            {
                Tusuario = true,
                LimitarData = true,
                Loja = tOrcamentoFiltro.Loja,
                TipoUsuario = usuarioLogin.TipoUsuario,
                Apelido = usuarioLogin.Nome
            };
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
                    orcamentoCotacaoFiltro.Vendedor = usuarioLogin.VendedorResponsavel;
                    orcamentoCotacaoFiltro.Parceiro = usuarioLogin.IdParceiro;
                }

                //VÊ somente suas vendas
                if (tOrcamentoFiltro.TipoUsuario.Value == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    tOrcamentoFiltro.VendedorParceiro = usuarioLogin.Nome;
                    orcamentoCotacaoFiltro.VendedorParceiro = usuarioLogin.Nome;
                }
            }

            if (tOrcamentoFiltro.Origem == "ORCAMENTOS")
            {
                var orcamentoCotacaoListaDto = _orcamentoCotacaoBll.PorFiltro(orcamentoCotacaoFiltro);

                List<OrcamentoCotacaoListaDto> lista = new List<OrcamentoCotacaoListaDto>();
                if (orcamentoCotacaoListaDto != null)
                {
                    var vendedores = _usuarioBll.PorFiltro(new TusuarioFiltro { });
                    var parceiros = _orcamentistaEIndicadorBll.BuscarParceiros(new TorcamentistaEindicadorFiltro { });
                    var vendParceiros = _orcamentistaEIndicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro { });

                    if (!String.IsNullOrEmpty(orcamentoCotacaoFiltro.Vendedor) && !String.IsNullOrEmpty(orcamentoCotacaoFiltro.Parceiro))
                    {
                        var idVendedor = vendedores.FirstOrDefault(v => v.Usuario == orcamentoCotacaoFiltro.Vendedor);
                        var idParceiro = parceiros.FirstOrDefault(p => p.Apelido == orcamentoCotacaoFiltro.Parceiro);

                        if (idVendedor != null && idParceiro != null)
                        {
                            orcamentoCotacaoListaDto = orcamentoCotacaoListaDto.Where(o =>
                                 o.IdVendedor == idVendedor.Id
                                 && (o.IdIndicador.HasValue && o.IdIndicador.Value == idParceiro.IdIndicador)
                             ).ToList();
                        }
                    }

                    orcamentoCotacaoListaDto.ForEach(x => lista.Add(new OrcamentoCotacaoListaDto()
                    {
                        NumeroOrcamento = x.Id.ToString(),
                        NumPedido = String.IsNullOrEmpty(x.IdPedido) ? "-" : x.IdPedido,
                        Cliente_Obra = !string.IsNullOrEmpty(x.NomeObra) ? $"{x.NomeCliente} - {x.NomeObra}" : x.NomeCliente,
                        Vendedor = vendedores.FirstOrDefault(v => v.Id == x.IdVendedor)?.Usuario,
                        Parceiro = parceiros.FirstOrDefault(v => v.IdIndicador == x.IdIndicador) == null ? "-" : parceiros.FirstOrDefault(v => v.IdIndicador == x.IdIndicador).Apelido,
                        VendedorParceiro = vendParceiros.FirstOrDefault(v => v.Id == x.IdIndicadorVendedor)?.Nome,
                        Valor = "0",
                        Status = x.StatusNome,
                        VistoEm = "",
                        Mensagem = _mensagemBll.ObterListaMensagemPendente(x.Id, x.Tusuarios.Id).Result.Any() ? "Sim" : "Não",
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
                return _orcamentoBll.OrcamentoPorFiltro(tOrcamentoFiltro);
            }
            else //if (tOrcamentoFiltro.Origem == "PEDIDOS")
            {
                var lista = _pedidoPrepedidoApiBll.ListarPedidos(tOrcamentoFiltro);

                foreach (var item in lista)
                    item.Status = TcfgPedidoStatus.ObterStatus(item.Status);

                return lista;
            }
        }
        public OrcamentoResponseViewModel PorFiltro(int id)
        {
            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro() { Id = id }).FirstOrDefault();
            if (orcamento == null) throw new Exception("Falha ao buscar Orçamento!");

            var opcao = _orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro() { IdOrcamentoCotacao = id });
            if (opcao.Count <= 0) throw new Exception("Falha ao buscar Opções do Orçamento!");

            var usuario = _usuarioBll.PorFiltro(new TusuarioFiltro() { id = orcamento.IdVendedor }).FirstOrDefault().Usuario;
            var parceiro = orcamento.IdIndicador != null ? _orcamentistaEIndicadorBll
                .BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { idParceiro = (int)orcamento.IdIndicador, acessoHabilitado = 1 }).Apelido : null;

            var vendedorParceiro = orcamento.IdIndicadorVendedor != null ? _orcamentistaEIndicadorVendedorBll
                .PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { id = (int)orcamento.IdIndicadorVendedor }).FirstOrDefault().Nome : null;
            OrcamentoResponseViewModel orcamentoResponse = new OrcamentoResponseViewModel()
            {
                Id = orcamento.Id,
                Vendedor = usuario,
                Parceiro = parceiro,
                VendedorParceiro = vendedorParceiro,
                Loja = orcamento.Loja,
                Validade = orcamento.Validade,
                QtdeRenovacao = orcamento.QtdeRenovacao,
                ConcordaWhatsapp = orcamento.AceiteWhatsApp,
                ObservacoesGerais = orcamento.Observacao,
                EntregaImediata = orcamento.StEtgImediata == 1 ? true : false,
                DataEntregaImediata = orcamento.PrevisaoEntregaData,
                ClienteOrcamentoCotacaoDto = new ClienteOrcamentoCotacaoRequestViewModel()
                {
                    NomeCliente = orcamento.NomeCliente,
                    NomeObra = orcamento.NomeObra,
                    Email = orcamento.Email,
                    Telefone = orcamento.Telefone,
                    Tipo = orcamento.TipoCliente,
                    Uf = orcamento.UF
                },
                ListaOrcamentoCotacaoDto = opcao
            };

            return orcamentoResponse;
        }

        public RemetenteDestinatarioResponseViewModel BuscarDadosParaMensageria(UsuarioLogin usuario, int id, bool usuarioIterno)
        {
            if (usuario.TipoUsuario == (int)Constantes.TipoUsuario.GESTOR) return null;

            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro() { Id = id }).FirstOrDefault();
            if (orcamento == null) throw new Exception("Falha ao buscar Orçamento!");

            if (!usuarioIterno) return _mensagemBll.CriarRemetenteCliente(orcamento);

            return _mensagemBll.CriarRemetenteUsuarioInterno(orcamento, usuario.Id);
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
            return await _mensagemBll.ObterListaMensagem(IdOrcamentoCotacao);
        }

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagemPendente(int IdOrcamentoCotacao, int IdUsuarioDestinatario)
        {
            return await _mensagemBll.ObterListaMensagemPendente(IdOrcamentoCotacao, IdUsuarioDestinatario);
        }

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            return _mensagemBll.EnviarMensagem(orcamentoCotacaoMensagem);
        }


        public bool MarcarMensagemComoLida(int IdOrcamentoCotacao, int idUsuarioDestinatario)
        {
            return _mensagemBll.MarcarMensagemComoLida(IdOrcamentoCotacao, idUsuarioDestinatario);
        }

        public int CadastrarOrcamentoCotacao(OrcamentoRequestViewModel orcamento, UsuarioLogin usuarioLogado)
        {
            //TODO: VALIDAR OrcamentoRequestViewModel
            if (orcamento.ListaOrcamentoCotacaoDto.Count <= 0) throw new ArgumentException("Necessário ter ao menos uma opção de orçamento!");

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var tOrcamentoCotacao = MontarTorcamentoCotacao(orcamento, usuarioLogado);

                    var ocamentoCotacao = _orcamentoCotacaoBll.InserirComTransacao(tOrcamentoCotacao, dbGravacao);

                    if (tOrcamentoCotacao.Id == 0) throw new ArgumentException("Ops! Não gerou Id!");

                    var opcoes = _orcamentoCotacaoOpcaoBll.CadastrarOrcamentoCotacaoOpcoesComTransacao(orcamento.ListaOrcamentoCotacaoDto, tOrcamentoCotacao.Id,
                        usuarioLogado, dbGravacao, orcamento.Loja);

                    AdicionarOrcamentoCotacaoEmailQueue(orcamento);

                    dbGravacao.transacao.Commit();

                    return ocamentoCotacao.Id;
                }
                catch (Exception ex)
                {
                    dbGravacao.transacao.Rollback();
                    throw new ArgumentException("Falha ao gravar orçamento!");
                }
            }

        }

        private void AdicionarOrcamentoCotacaoEmailQueue(OrcamentoRequestViewModel orcamento)
        {

            TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueueModel = new InfraBanco.Modelos.TorcamentoCotacaoEmailQueue();

            var loja = _lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = orcamento.Loja });
            var tcfgUnidadeNegocio = cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { NomeCurto = loja[0].Nome });
            var tcfgUnidadeNegocioParametros = cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id });
            var nomeEmpresa = "";

            foreach (var item in tcfgUnidadeNegocioParametros)
            {
                switch (item.IdCfgParametro)
                {
                    case 5:
                        orcamentoCotacaoEmailQueueModel.From = item.Valor;
                        break;
                    case 6:
                        orcamentoCotacaoEmailQueueModel.FromDisplayName = item.Valor;
                        nomeEmpresa = item.Valor;
                        break;
                }
            }

            orcamentoCotacaoEmailQueueModel.IdCfgUnidadeNegocio = tcfgUnidadeNegocioParametros[0].IdCfgUnidadeNegocio;
            orcamentoCotacaoEmailQueueModel.To = orcamento.ClienteOrcamentoCotacaoDto.Email;
            orcamentoCotacaoEmailQueueModel.Cc = "";
            orcamentoCotacaoEmailQueueModel.Bcc = "";

            string[] tagHtml = new string[] {
                        orcamento.ClienteOrcamentoCotacaoDto.NomeCliente,
                        nomeEmpresa
                    };

            _orcamentoCotacaoEmailQueueBll.InserirQueueComTemplateEHTML(2, orcamentoCotacaoEmailQueueModel, tagHtml);

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
            if (!string.IsNullOrEmpty(orcamento.Parceiro) && orcamento.Parceiro != Constantes.SEM_INDICADOR)
            {
                if (usuarioLogado.TipoUsuario != (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
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
