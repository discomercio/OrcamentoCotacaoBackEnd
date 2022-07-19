using Cfg.CfgUnidadeNegocio;
using Cfg.CfgUnidadeNegocioParametro;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Loja;
using Microsoft.Extensions.Options;
using Orcamento;
using OrcamentoCotacaoLink;
using Orcamento.Dto;
using OrcamentoCotacao.Dto;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using PrepedidoBusiness.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using Loja.Dados;
using OrcamentoCotacaoBusiness.Dto;

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
        private readonly OrcamentoCotacaoLink.OrcamentoCotacaoLinkBll _orcamentoCotacaoLinkBll;
        private readonly LojaBll _lojaBll;
        private readonly CfgUnidadeNegocioBll _cfgUnidadeNegocioBll;
        private readonly CfgUnidadeNegocioParametroBll _cfgUnidadeNegocioParametroBll;
        private readonly FormaPagtoOrcamentoCotacaoBll _formaPagtoOrcamentoCotacaoBll;
        private readonly LojaOrcamentoCotacaoBll _lojaOrcamentoCotacaoBll;

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
            CfgUnidadeNegocioParametroBll cfgUnidadeNegocioParametroBll,
            FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll,
            OrcamentoCotacaoLinkBll orcamentoCotacaoLinkBll,
            LojaOrcamentoCotacaoBll _lojaOrcamentoCotacaoBll
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
            _orcamentoCotacaoLinkBll = orcamentoCotacaoLinkBll;
            this._lojaOrcamentoCotacaoBll = _lojaOrcamentoCotacaoBll;
            _lojaBll = lojaBll;
            _cfgUnidadeNegocioBll = cfgUnidadeNegocioBll;
            _cfgUnidadeNegocioParametroBll = cfgUnidadeNegocioParametroBll;
            _formaPagtoOrcamentoCotacaoBll = formaPagtoOrcamentoCotacaoBll;
        }

        public OrcamentoCotacaoDto PorGuid(string guid)
        {
            var orcamento = _orcamentoCotacaoBll.PorGuid(guid);

            if (orcamento != null)
            {
                UsuarioLogin usuario = new UsuarioLogin { TipoUsuario = 4 }; //CLIENTE

                orcamento.listaOpcoes = _orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro { IdOrcamentoCotacao = orcamento.id });
                orcamento.listaFormasPagto = _formaPagtoOrcamentoCotacaoBll.BuscarFormasPagamentos(orcamento.tipoCliente, (Constantes.TipoUsuario)usuario.TipoUsuario, orcamento.vendedor, byte.Parse(orcamento.idIndicador.HasValue ? "1" : "0"));
                orcamento.mensageria = BuscarDadosParaMensageria(usuario, orcamento.id, false);

                return orcamento;
            }

            return null;
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
                        VendedorParceiro = tOrcamentoFiltro.TipoUsuario.Value == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO ? vendParceiros.FirstOrDefault(v => v.Id == x.IdIndicadorVendedor)?.Email : vendParceiros.FirstOrDefault(v => v.Id == x.IdIndicadorVendedor)?.Nome,
                        Valor = "0",
                        Status = x.StatusNome,
                        VistoEm = "",
                        Mensagem = _mensagemBll.ObterListaMensagemPendente(x.Id).Result.Any() ? "Sim" : "Não",
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
        public OrcamentoResponseViewModel PorFiltro(int id, UsuarioLogin usuarioLogin)
        {
            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro() { Id = id }).FirstOrDefault();
            if (orcamento == null) throw new Exception("Falha ao buscar Orçamento!");

            var opcao = _orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro() { IdOrcamentoCotacao = id });
            if (opcao.Count <= 0) throw new Exception("Falha ao buscar Opções do Orçamento!");

            var usuario = _usuarioBll.PorFiltro(new TusuarioFiltro() { id = orcamento.IdVendedor }).FirstOrDefault().Usuario;
            var parceiro = orcamento.IdIndicador != null ? _orcamentistaEIndicadorBll
                .BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { idParceiro = (int)orcamento.IdIndicador, acessoHabilitado = 1 }).Apelido : null;

            string vendedorParceiro = null;
            if (orcamento.IdIndicadorVendedor != null)
            {
                var tVendedorParceiro = _orcamentistaEIndicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro()
                {
                    id = (int)orcamento.IdIndicadorVendedor
                }).FirstOrDefault();
                vendedorParceiro = (int)usuarioLogin.TipoUsuario != (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO ?
                    tVendedorParceiro?.Nome : tVendedorParceiro.Email;
            }
            

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
                Status = orcamento.Status,
                DataCadastro = orcamento.DataCadastro,
                IdIndicador = orcamento.IdIndicador,
                IdIndicadorVendedor = orcamento.IdIndicadorVendedor,
                ClienteOrcamentoCotacaoDto = new ClienteOrcamentoCotacaoRequestViewModel()
                {
                    NomeCliente = orcamento.NomeCliente,
                    NomeObra = orcamento.NomeObra,
                    Email = orcamento.Email,
                    Telefone = orcamento.Telefone,
                    Tipo = orcamento.TipoCliente,
                    Uf = orcamento.UF
                },
                ListaOrcamentoCotacaoDto = opcao,
                CadastradoPor = VerificarContextoCadastroOrcamento(orcamento.IdTipoUsuarioContextoCadastro, usuario, parceiro, vendedorParceiro)
            };

            return orcamentoResponse;
        }

        private string VerificarContextoCadastroOrcamento(int idTipoUsuarioContextoCadastro, string usuario, string parceiro,
            string vendedorParceiro)
        {
            var lstTipoUsuariosContexto = _usuarioBll.BuscarTipoUsuarioContexto();
            if (lstTipoUsuariosContexto == null) return null;

            var filtrado = lstTipoUsuariosContexto.Where(x => x.Id == idTipoUsuarioContextoCadastro).FirstOrDefault();
            if (filtrado == null) return null;

            if (filtrado.Id == (short)Constantes.TipoUsuarioContexto.UsuarioInterno) { return usuario; }
            if (filtrado.Id == (short)Constantes.TipoUsuarioContexto.Parceiro) { return parceiro; }
            if (filtrado.Id == (short)Constantes.TipoUsuarioContexto.VendedorParceiro) { return vendedorParceiro; }

            return null;
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

        public async Task<List<TorcamentoCotacaoMensagem>> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            return await _mensagemBll.ObterListaMensagemPendente(IdOrcamentoCotacao);
        }

        public bool EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem, int IdUsuarioLogado)
        {
            return _mensagemBll.EnviarMensagem(orcamentoCotacaoMensagem, IdUsuarioLogado);
        }


        public bool MarcarMensagemComoLida(int IdOrcamentoCotacao, int idUsuarioRemetente)
        {
            return _mensagemBll.MarcarLida(IdOrcamentoCotacao, idUsuarioRemetente);
        }

        public int CadastrarOrcamentoCotacao(OrcamentoRequestViewModel orcamento, UsuarioLogin usuarioLogado)
        {
            //TODO: VALIDAR OrcamentoRequestViewModel
            if (orcamento.ListaOrcamentoCotacaoDto.Count <= 0) throw new ArgumentException("Necessário ter ao menos uma opção de orçamento!");

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var percentualMaxDescontoEComissao = _lojaBll.BuscarPercMaxPorLoja(orcamento.Loja);
                    if (percentualMaxDescontoEComissao == null) throw new ArgumentException("Falha ao tentar gravar orçamento!");

                    var tOrcamentoCotacao = MontarTorcamentoCotacao(orcamento, usuarioLogado, percentualMaxDescontoEComissao);

                    var ocamentoCotacao = _orcamentoCotacaoBll.InserirComTransacao(tOrcamentoCotacao, dbGravacao);

                    if (tOrcamentoCotacao.Id == 0) throw new ArgumentException("Ops! Não gerou Id!");

                    var opcoes = _orcamentoCotacaoOpcaoBll.CadastrarOrcamentoCotacaoOpcoesComTransacao(orcamento.ListaOrcamentoCotacaoDto, tOrcamentoCotacao.Id,
                        usuarioLogado, dbGravacao, orcamento.Loja);

                    var guid = Guid.NewGuid();

                    AdicionarOrcamentoCotacaoLink(tOrcamentoCotacao, guid, dbGravacao);
                    AdicionarOrcamentoCotacaoEmailQueue(orcamento, guid);

                    dbGravacao.transacao.Commit();

                    return ocamentoCotacao.Id;
                }
                catch
                {
                    dbGravacao.transacao.Rollback();
                    throw new ArgumentException("Falha ao gravar orçamento!");
                }
            }
        }

        public void AtualizarOrcamentoOpcao(OrcamentoOpcaoResponseViewModel opcao, UsuarioLogin usuarioLogado)
        {
            var orcamento = PorFiltro(opcao.IdOrcamentoCotacao, usuarioLogado);

            bool temPermissao = ValidarPermissaoAtualizarOpcaoOrcamentoCotacao(orcamento, usuarioLogado);
            if (!temPermissao) throw new ArgumentException("Usuário não tem permissão para atualizar a opção de orçamento!");

            _orcamentoCotacaoOpcaoBll.AtualizarOrcamentoOpcao(opcao, usuarioLogado, orcamento);
        }

        private bool ValidarPermissaoAtualizarOpcaoOrcamentoCotacao(OrcamentoResponseViewModel orcamento, UsuarioLogin usuarioLogado)
        {

            if (orcamento.IdIndicadorVendedor != null)
            {
                var vendedoresParceiro = _orcamentistaEIndicadorVendedorBll.BuscarVendedoresParceiro(orcamento.Parceiro);
                if (vendedoresParceiro == null) throw new ArgumentException("Nenhum vendedor do parceiro encontrado!");

                var email = vendedoresParceiro //IdIndicadorVendedor
                    .Where(x => x.Id == orcamento.IdIndicadorVendedor)
                    .FirstOrDefault().Email;

                if (usuarioLogado.Apelido == email.ToUpper()) return true;
            }

            if (orcamento.IdIndicadorVendedor == null && orcamento.IdIndicador != null)
            {
                var parceiro = _orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { apelido = orcamento.Parceiro, acessoHabilitado = 1 });

                if (usuarioLogado.Apelido == parceiro.Apelido) return true;
            }

            

            if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_1) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_2) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_3))
                return true;

            if (orcamento.CadastradoPor.ToUpper() == usuarioLogado.Apelido.ToUpper()) return true;

                return false;
        }

        public void AdicionarOrcamentoCotacaoLink(TorcamentoCotacao orcamento, Guid guid, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            TorcamentoCotacaoLink orcamentoCotacaoLinkModel = new InfraBanco.Modelos.TorcamentoCotacaoLink();
            //orcamentoCotacaoLinkModel.DataHoraCadastro = System.DateTime.Now;

            orcamentoCotacaoLinkModel.IdOrcamentoCotacao = orcamento.Id;
            orcamentoCotacaoLinkModel.Guid = guid;
            orcamentoCotacaoLinkModel.Status = 1;
            orcamentoCotacaoLinkModel.IdTipoUsuarioContextoUltStatus = 1;
            orcamentoCotacaoLinkModel.IdUsuarioUltStatus = orcamento.IdUsuarioCadastro;
            orcamentoCotacaoLinkModel.DataUltStatus = orcamento.DataUltStatus;
            orcamentoCotacaoLinkModel.DataHoraUltStatus = orcamento.DataHoraUltStatus;
            orcamentoCotacaoLinkModel.IdTipoUsuarioContextoCadastro = (short)orcamento.IdTipoUsuarioContextoCadastro;
            orcamentoCotacaoLinkModel.IdUsuarioCadastro = orcamento.IdUsuarioCadastro;
            orcamentoCotacaoLinkModel.DataCadastro = orcamento.DataCadastro;
            orcamentoCotacaoLinkModel.DataHoraCadastro = orcamento.DataHoraCadastro;
            _orcamentoCotacaoLinkBll.InserirOrcamentoCotacaoLink(orcamentoCotacaoLinkModel, contextoBdGravacao);

        }

        private void AdicionarOrcamentoCotacaoEmailQueue(OrcamentoRequestViewModel orcamento, Guid guid)
        {

            TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueueModel = new InfraBanco.Modelos.TorcamentoCotacaoEmailQueue();

            var loja = _lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = orcamento.Loja });
            var tcfgUnidadeNegocio = _cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { Sigla = loja[0].Unidade_Negocio });
            var tcfgUnidadeNegocioParametros = _cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id });
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
                        nomeEmpresa,
                        guid.ToString(),
                        orcamento.Id.ToString()
                    };

            _orcamentoCotacaoEmailQueueBll.InserirQueueComTemplateEHTML(2, orcamentoCotacaoEmailQueueModel, tagHtml);

        }

        private TorcamentoCotacao MontarTorcamentoCotacao(OrcamentoRequestViewModel orcamento, UsuarioLogin usuarioLogado,
            PercMaxDescEComissaoDados percMaxDescEComissaoDados)
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
                PrevisaoEntregaData = orcamento.DataEntregaImediata,
                Perc_max_comissao_e_desconto_padrao = orcamento.ClienteOrcamentoCotacaoDto.Tipo == Constantes.ID_PF ?
                    percMaxDescEComissaoDados.PercMaxComissaoEDesconto : percMaxDescEComissaoDados.PercMaxComissaoEDescontoPJ
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

                torcamentoCotacao.Perc_max_comissao_padrao = percMaxDescEComissaoDados.PercMaxComissao;
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

        public MensagemDto ProrrogarOrcamento(int id, int idUsuario)
        {
            //TODO: DESFIXAR VARIAVEIS
            int QtdeDiasValidade = 15;
            int QtdeGlobalValidade = 30;
            int QtdeDiasProrrogacao = 5;
            int QtdeMaxProrrogacao = 2;

            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro { Id = id }).FirstOrDefault();

            if (orcamento != null)
            {
                if (orcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.APROVADO ||
                    orcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.CANCELADO)
                    return new MensagemDto
                    {
                        tipo = "WARN",
                        mensagem = "Não é possível prorrogar, orçamentos aprovados ou cancelados!"
                    };

                if (orcamento.QtdeRenovacao >= QtdeMaxProrrogacao)
                    return new MensagemDto
                    {
                        tipo = "WARN",
                        mensagem = $"Excedida a quantidade máxima! {QtdeMaxProrrogacao} vezes"
                    };

                if (DateTime.Now.AddDays(QtdeDiasProrrogacao) > DateTime.Now.AddDays(QtdeGlobalValidade))
                    orcamento.Validade = DateTime.Now.AddDays(QtdeGlobalValidade);
                else
                    orcamento.Validade = DateTime.Now.AddDays(QtdeDiasProrrogacao);

                orcamento.ValidadeAnterior = orcamento.Validade;
                orcamento.QtdeRenovacao += 1;
                orcamento.IdUsuarioUltRenovacao = idUsuario;
                orcamento.DataHoraUltRenovacao = DateTime.Now;

                _orcamentoCotacaoBll.Atualizar(orcamento);

                return new MensagemDto
                {
                    tipo = "INFO",
                    mensagem = $"Prorrogado para: {orcamento.Validade.ToString("dd/MM/yyyy")}. {orcamento.QtdeRenovacao}ª vez."
                };
            }

            return null;
        }

        public MensagemDto AprovarOrcamento(int id, int idUsuario)
        {
            //TODO: DESFIXAR VARIAVEIS
            int QtdeDiasValidade = 15;
            int QtdeGlobalValidade = 30;
            int QtdeDiasProrrogacao = 5;
            int QtdeMaxProrrogacao = 2;

            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro { Id = id }).FirstOrDefault();

            if (orcamento != null)
            {
                if (orcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.APROVADO ||
                    orcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.CANCELADO)
                    return new MensagemDto
                    {
                        tipo = "WARN",
                        mensagem = "Não é possível aprovar, orçamentos aprovados ou cancelados!"
                    };

                if (orcamento.Validade.Date < DateTime.Now.Date)
                    return new MensagemDto
                    {
                        tipo = "WARN",
                        mensagem = "Não é possível aprovar, orçamentos com validade expirada!"
                    };

                //orcamento.ValidadeAnterior = orcamento.Validade;
                //orcamento.QtdeRenovacao += 1;
                //orcamento.IdUsuarioUltRenovacao = idUsuario;
                //orcamento.DataHoraUltRenovacao = DateTime.Now;

                //_orcamentoCotacaoBll.Atualizar(orcamento);

                return new MensagemDto
                {
                    tipo = "INFO",
                    mensagem = String.Format("Prorrogado para: {0}. {1} {2}", orcamento.Validade.ToString("dd/MM/yyyy"), orcamento.QtdeRenovacao, orcamento.QtdeRenovacao == 1 ? "vez" : "vezes")
                };
            }

            return null;
        }
    }
}
