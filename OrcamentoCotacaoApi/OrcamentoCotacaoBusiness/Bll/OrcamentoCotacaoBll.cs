using Cfg.CfgUnidadeNegocio;
using Cfg.CfgUnidadeNegocioParametro;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Loja;
using Loja.Dados;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orcamento;
using Orcamento.Dto;
using OrcamentoCotacao.Dto;
using OrcamentoCotacaoBusiness.Dto;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoLink;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UtilsGlobais.Parametros;
using Microsoft.EntityFrameworkCore;
using Prepedido.Dados.DetalhesPrepedido;

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
        private readonly OrcamentoCotacaoEmail.OrcamentoCotacaoEmailBll _orcamentoCotacaoEmailBll;
        private readonly OrcamentoCotacaoLink.OrcamentoCotacaoLinkBll _orcamentoCotacaoLinkBll;
        private readonly LojaBll _lojaBll;
        private readonly CfgUnidadeNegocioBll _cfgUnidadeNegocioBll;
        private readonly CfgUnidadeNegocioParametroBll _cfgUnidadeNegocioParametroBll;
        private readonly FormaPagtoOrcamentoCotacaoBll _formaPagtoOrcamentoCotacaoBll;
        private readonly PublicoBll _publicoBll;
        private readonly ParametroOrcamentoCotacaoBll _parametroOrcamentoCotacaoBll;
        private readonly ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll;
        private readonly ClienteBll _clienteBll;
        private readonly ILogger<OrcamentoCotacaoBll> _logger;
        private readonly Prepedido.Bll.PrepedidoBll _prepedidoBll;

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
            OrcamentoCotacaoEmail.OrcamentoCotacaoEmailBll orcamentoCotacaoEmailBll,
            LojaBll lojaBll,
            CfgUnidadeNegocioBll cfgUnidadeNegocioBll,
            CfgUnidadeNegocioParametroBll cfgUnidadeNegocioParametroBll,
            FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll,
            OrcamentoCotacaoLinkBll orcamentoCotacaoLinkBll,
            PublicoBll publicoBll,
            ParametroOrcamentoCotacaoBll parametroOrcamentoCotacaoBll,
            ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll,
            ClienteBll clienteBll,
            ILogger<OrcamentoCotacaoBll> logger,
            Prepedido.Bll.PrepedidoBll _prepedidoBll
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
            _orcamentoCotacaoEmailBll = orcamentoCotacaoEmailBll;
            _orcamentoCotacaoLinkBll = orcamentoCotacaoLinkBll;
            _lojaBll = lojaBll;
            _cfgUnidadeNegocioBll = cfgUnidadeNegocioBll;
            _cfgUnidadeNegocioParametroBll = cfgUnidadeNegocioParametroBll;
            _formaPagtoOrcamentoCotacaoBll = formaPagtoOrcamentoCotacaoBll;
            _publicoBll = publicoBll;
            _parametroOrcamentoCotacaoBll = parametroOrcamentoCotacaoBll;
            this.produtoOrcamentoCotacaoBll = produtoOrcamentoCotacaoBll;
            _clienteBll = clienteBll;
            _logger = logger;
            this._prepedidoBll = _prepedidoBll;
        }

        public OrcamentoCotacaoDto PorGuid(string guid)
        {
            var orcamento = _orcamentoCotacaoBll.PorGuid(guid);

            if (orcamento != null)
            {
                var usuario = new UsuarioLogin { TipoUsuario = 4 }; //CLIENTE

                var loja = _lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = orcamento.loja });
                var tcfgUnidadeNegocio = _cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { Sigla = loja[0].Unidade_Negocio });

                orcamento.lojaViewModel = _lojaBll.BuscarLojaEstilo(orcamento.loja);

                //Parametros
                var prazoMaximoConsultaOrcamento = _cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id, IdCfgParametro = 24 });
                var condicoesGerais = _cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id, IdCfgParametro = 12 });

                orcamento.condicoesGerais = condicoesGerais[0].Valor;
                orcamento.prazoMaximoConsultaOrcamento = prazoMaximoConsultaOrcamento[0].Valor;
                orcamento.listaOpcoes = _orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro { IdOrcamentoCotacao = orcamento.id });
                orcamento.listaFormasPagto = _formaPagtoOrcamentoCotacaoBll.BuscarFormasPagamentos(orcamento.tipoCliente, (Constantes.TipoUsuario)usuario.TipoUsuario, orcamento.vendedor, byte.Parse(orcamento.idIndicador.HasValue ? "1" : "0"));
                orcamento.mensageria = BuscarDadosParaMensageria(usuario, orcamento.id, false);
                orcamento.token = _publicoBll.ObterTokenServico();


                if (!Validar(orcamento))
                {
                    orcamento = null;
                }

                return orcamento;
            }

            return null;
        }

        public bool Validar(OrcamentoCotacaoDto orcamentoCotacaoDto)
        {
            DateTime dataAtual = DateTime.Now;

            if (orcamentoCotacaoDto.statusOrcamentoCotacaoLink != 1)
                return false;

            // [2] Cancelado
            if (orcamentoCotacaoDto.status == 2)
                return false;

            // Expirado
            if (dataAtual > orcamentoCotacaoDto.validade)
                return false;

            // [3] Aprovado
            if (orcamentoCotacaoDto.status == 3)
            {

                DateTime dataCriacao = (DateTime)orcamentoCotacaoDto.dataCadastro;
                DateTime dataValidade = dataCriacao.AddDays(int.Parse(orcamentoCotacaoDto.prazoMaximoConsultaOrcamento));

                if (dataAtual > dataValidade)
                    return false;
            }

            return true;
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

        public OrcamentoResponseViewModel PorFiltro(int id, int tipoUsuario)
        {
            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro() { Id = id }).FirstOrDefault();
            if (orcamento == null) throw new Exception("Falha ao buscar Orçamento!");

            var opcao = _orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro() { IdOrcamentoCotacao = id });
            if (opcao.Count <= 0) throw new Exception("Falha ao buscar Opções do Orçamento!");

            string statusEmail;

            var orcamentoCotacaoEmail = _orcamentoCotacaoEmailBll.PorFiltro(new TorcamentoCotacaoEmailFiltro() { IdOrcamentoCotacao = id }).LastOrDefault();

            if (orcamentoCotacaoEmail == null)
            {
                statusEmail = "Erro no envio do email";
            }
            else
            {
                var orcamentoCotacaoEmailQueue = _orcamentoCotacaoEmailQueueBll.PorFiltro(new TorcamentoCotacaoEmailQueueFiltro() { Id = orcamentoCotacaoEmail.IdOrcamentoCotacaoEmailQueue }).FirstOrDefault();

                switch (orcamentoCotacaoEmailQueue.Status)
                {
                    case 0:
                        statusEmail = "Aguardando envio do email";
                        break;
                    case 2:
                        statusEmail = "Email enviado com sucesso";
                        break;
                    case 3:
                        statusEmail = "Erro no envio do email";
                        break;
                    case 4:
                        statusEmail = "Erro no envio do email";
                        break;
                    default:
                        statusEmail = "Erro no envio do email";
                        break;
                }

                var orcamentoCotacaoLink = _orcamentoCotacaoLinkBll.PorFiltro(new TorcamentoCotacaoLinkFiltro() { IdOrcamentoCotacao = id, Status = 3 }).LastOrDefault();

                if (orcamentoCotacaoLink != null)
                {
                    statusEmail = "Email recusado pelo cliente";
                }

                if (orcamentoCotacaoEmailQueue.AttemptsQty > 3)
                {
                    statusEmail = "Erro no envio do email";
                }

            }

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
                vendedorParceiro = tipoUsuario != (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO ?
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
                EntregaImediata = orcamento.StEtgImediata == (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO ? false : true,
                DataEntregaImediata = orcamento.PrevisaoEntregaData,
                Status = orcamento.Status,
                statusEmail = statusEmail,
                DataCadastro = orcamento.DataCadastro,
                IdIndicador = orcamento.IdIndicador,
                IdIndicadorVendedor = orcamento.IdIndicadorVendedor,
                IdVendedor = orcamento.IdVendedor,
                ClienteOrcamentoCotacaoDto = new ClienteOrcamentoCotacaoRequestViewModel()
                {
                    NomeCliente = orcamento.NomeCliente,
                    NomeObra = orcamento.NomeObra,
                    Email = orcamento.Email,
                    Telefone = orcamento.Telefone,
                    Tipo = orcamento.TipoCliente,
                    Uf = orcamento.UF,
                    ContribuinteICMS = orcamento.ContribuinteIcms
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

        public ValidadeResponseViewModel BuscarConfigValidade(string unidadeNegocio)
        {
            var parametros = _parametroOrcamentoCotacaoBll.ObterParametros(unidadeNegocio);
            if (parametros == null) throw new ArgumentException("Falha ao buscar configurações de orçamento!");

            return new ValidadeResponseViewModel
            {
                QtdeDiasValidade = int.Parse(parametros.QtdePadrao_DiasValidade),
                QtdeDiasProrrogacao = int.Parse(parametros.QtdePadrao_DiasProrrogacao),
                QtdeMaxProrrogacao = int.Parse(parametros.QtdeMaxProrrogacao),
                QtdeGlobalValidade = int.Parse(parametros.QtdeGlobal_Validade),
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


        private void ValidarClienteOrcamento(ClienteOrcamentoCotacaoRequestViewModel cliente)
        {
            if (cliente == null) throw new ArgumentNullException("Ops! Favor preencher os dados do cliente!");

            if (string.IsNullOrEmpty(cliente.NomeCliente))
                throw new ArgumentNullException("O nome do cliente é obrigatório!");

            if (cliente.NomeCliente.Length > 60)
                throw new ArgumentException("O nome do cliente excede a quantidade máxima de caracteres permitido!");

            if (!string.IsNullOrEmpty(cliente.NomeObra) && cliente.NomeObra.Length > 120)
                throw new ArgumentException("O nome da obra execede a quantidade máxima de caracteres permitido!");

            if (!new EmailAddressAttribute().IsValid(cliente.Email))
                throw new ArgumentException("E-mail inválido!");

            if (!string.IsNullOrEmpty(cliente.Telefone) && cliente.Telefone.Length > 15)
                throw new ArgumentException("Telefone inválido!");

            if (string.IsNullOrEmpty(cliente.Uf))
                throw new ArgumentException("Informe a UF de entrega!");

            if (!UtilsGlobais.Util.VerificaUf(cliente.Uf))
                throw new ArgumentException("Uf de entrega inválida!");

            if (string.IsNullOrEmpty(cliente.Tipo))
                throw new ArgumentException("Informe se o cliente é pessoa física ou jurídica!");

            if (cliente.Tipo.Length > 2)
                throw new ArgumentException("O tipo do cliente execede a quantidade máxima de caracteres permitido!");

            if (cliente.Tipo.ToUpper() != Constantes.ID_PF && cliente.Tipo.ToUpper() != Constantes.ID_PJ)
                throw new ArgumentException("Tipo do cliente inválido!");

            if (cliente.Tipo == Constantes.ID_PJ)
            {
                if (cliente.ContribuinteICMS == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL ||
                (cliente.ContribuinteICMS != (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                cliente.ContribuinteICMS != (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                cliente.ContribuinteICMS != (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO))
                    throw new ArgumentException("Contribuinte de ICMS inválido!");
            }

            if (cliente.Tipo == Constantes.ID_PF)
            {
                if (cliente.ContribuinteICMS != null && cliente.ContribuinteICMS > 0)
                    throw new ArgumentException("Cliente pessoa física não pode ter valor de contribuinte ICMS!");
            }
        }

        public int CadastrarOrcamentoCotacao(OrcamentoRequestViewModel orcamento, UsuarioLogin usuarioLogado)
        {
            //TODO: VALIDAR OrcamentoRequestViewModel
            //ValidarCredenciais campos obrigatórios
            ValidarClienteOrcamento(orcamento.ClienteOrcamentoCotacaoDto);

            if (orcamento.ListaOrcamentoCotacaoDto.Count <= 0) throw new ArgumentException("Necessário ter ao menos uma opção de orçamento!");

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var percentualMaxDescontoEComissao = _lojaBll.BuscarPercMaxPorLoja(orcamento.Loja);
                    if (percentualMaxDescontoEComissao == null) throw new ArgumentException("Falha ao tentar gravar orçamento!");

                    var tOrcamentoCotacao = MontarTorcamentoCotacao(orcamento, usuarioLogado, percentualMaxDescontoEComissao, 1);

                    var ocamentoCotacao = _orcamentoCotacaoBll.InserirComTransacao(tOrcamentoCotacao, dbGravacao);

                    if (tOrcamentoCotacao.Id == 0) throw new ArgumentException("Ops! Não gerou Id!");

                    var opcoes = _orcamentoCotacaoOpcaoBll.CadastrarOrcamentoCotacaoOpcoesComTransacao(orcamento.ListaOrcamentoCotacaoDto, tOrcamentoCotacao.Id,
                        usuarioLogado, dbGravacao, orcamento.Loja);

                    var guid = Guid.NewGuid();

                    AdicionarOrcamentoCotacaoLink(tOrcamentoCotacao, guid, dbGravacao);
                    AdicionarOrcamentoCotacaoEmailQueue(orcamento, guid, tOrcamentoCotacao.Id, dbGravacao);

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

        public MensagemDto ReenviarOrcamentoCotacao(int idOrcamentoCotacao)
        {

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var tOrcamento = _orcamentoCotacaoBll.PorFiltroComTransacao(new TorcamentoCotacaoFiltro() { Id = idOrcamentoCotacao }, dbGravacao).FirstOrDefault();

                    var tOrcamentoCotacaoLink = _orcamentoCotacaoLinkBll.PorFiltroComTransacao(new TorcamentoCotacaoLinkFiltro() { IdOrcamentoCotacao = idOrcamentoCotacao, Status = 1 }, dbGravacao).FirstOrDefault();
                    //var guid = Guid.NewGuid();

                    if (tOrcamentoCotacaoLink == null)
                    {
                        dbGravacao.transacao.Rollback();

                        return new MensagemDto
                        {
                            tipo = "WARN",
                            mensagem = "O endereço de e-mail necessita ser alterado, pois houve problema no envio."
                        };

                    }

                    //AdicionarOrcamentoCotacaoLink(tOrcamento, guid, dbGravacao);
                    AdicionarOrcamentoCotacaoEmailQueue(tOrcamento, tOrcamentoCotacaoLink.Guid, idOrcamentoCotacao, dbGravacao);

                    dbGravacao.transacao.Commit();

                    return new MensagemDto
                    {
                        tipo = "SUCCESS",
                        mensagem = "Orçamento reenviado."
                    };

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
            var orcamento = PorFiltro(opcao.IdOrcamentoCotacao, (int)usuarioLogado.TipoUsuario);

            bool temPermissao = ValidarPermissaoAtualizarOpcaoOrcamentoCotacao(orcamento, usuarioLogado);
            if (!temPermissao) throw new ArgumentException("Usuário não tem permissão para atualizar a opção de orçamento!");

            _orcamentoCotacaoOpcaoBll.AtualizarOrcamentoOpcao(opcao, usuarioLogado, orcamento);
        }

        public void AtualizarDadosCadastraisOrcamento(OrcamentoResponseViewModel orcamento, UsuarioLogin usuarioLogado)
        {
            var orcamentoAntigo = PorFiltro((int)orcamento.Id, (int)usuarioLogado.TipoUsuario);

            ValidarClienteOrcamento(orcamento.ClienteOrcamentoCotacaoDto);

            ValidarAtualizacaoDadosCadastraisOrcamentoCotacao(orcamento, orcamentoAntigo, usuarioLogado);

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var tOrcamento = _orcamentoCotacaoBll.PorFiltroComTransacao(new TorcamentoCotacaoFiltro() { Id = (int)orcamento.Id }, dbGravacao).FirstOrDefault();
                    if (tOrcamento == null) throw new Exception("Falha ao buscar Orçamento!");

                    bool alterouEmail = false;

                    if (orcamento.ClienteOrcamentoCotacaoDto.Email != tOrcamento.Email)
                        alterouEmail = true;


                    tOrcamento.NomeCliente = orcamento.ClienteOrcamentoCotacaoDto.NomeCliente;
                    tOrcamento.NomeObra = orcamento.ClienteOrcamentoCotacaoDto.NomeObra;
                    tOrcamento.UF = orcamento.ClienteOrcamentoCotacaoDto.Uf;
                    tOrcamento.Email = orcamento.ClienteOrcamentoCotacaoDto.Email;
                    tOrcamento.Telefone = orcamento.ClienteOrcamentoCotacaoDto.Telefone;
                    tOrcamento.StEtgImediata = orcamento.EntregaImediata ? (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM : (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO;
                    tOrcamento.PrevisaoEntregaData = orcamento.DataEntregaImediata?.Date;
                    tOrcamento.Observacao = orcamento.ObservacoesGerais;
                    tOrcamento.IdTipoUsuarioContextoUltAtualizacao = (int)usuarioLogado.TipoUsuario;
                    tOrcamento.IdUsuarioUltAtualizacao = usuarioLogado.Id;
                    tOrcamento.DataHoraUltAtualizacao = DateTime.Now;
                    tOrcamento.AceiteWhatsApp = orcamento.ConcordaWhatsapp;
                    tOrcamento.ContribuinteIcms = (byte)orcamento.ClienteOrcamentoCotacaoDto.ContribuinteICMS;

                    var retorno = _orcamentoCotacaoBll.AtualizarComTransacao(tOrcamento, dbGravacao);

                    bool atualizouLink = false;

                    if (alterouEmail)
                    {
                        AtualizarOrcamentoCotacaoLink(orcamento, dbGravacao);

                        atualizouLink = true;
                    }

                    dbGravacao.transacao.Commit();

                    if (atualizouLink)
                    {
                        GerarNovoLink(tOrcamento);
                    }

                }
                catch (Exception ex)
                {
                    dbGravacao.transacao.Rollback();
                    throw new ArgumentException("Falha ao atualizar orçamento!");
                }
            }
        }

        private void GerarNovoLink(TorcamentoCotacao tOrcamento)
        {
            var guid = Guid.NewGuid();

            using (var db = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    AdicionarOrcamentoCotacaoLink(tOrcamento, guid, db);
                    AdicionarOrcamentoCotacaoEmailQueue(tOrcamento, guid, tOrcamento.Id, db);
                    db.transacao.Commit();
                }
                catch
                {
                    db.transacao.Rollback();
                    throw new ArgumentException("Falha ao atualizar orçamento!");
                }
            }
        }

        private bool ValidarDonoOrcamentoCotacao(OrcamentoResponseViewModel orcamentoAntigo,
            UsuarioLogin usuarioLogado)
        {
            if (orcamentoAntigo.IdIndicadorVendedor != null)
                if (usuarioLogado.Id == orcamentoAntigo.IdIndicadorVendedor) return true;
                else return false;


            if (orcamentoAntigo.IdIndicador != null)
                if (usuarioLogado.Id == orcamentoAntigo.IdIndicador) return true;
                else return false;

            if (orcamentoAntigo.IdVendedor == usuarioLogado.Id) return true;

            return false;
        }

        public void ValidarAtualizacaoDadosCadastraisOrcamentoCotacao(OrcamentoResponseViewModel orcamento,
            OrcamentoResponseViewModel orcamentoAntigo, UsuarioLogin usuarioLogado)
        {
            foreach (var opcao in orcamentoAntigo.ListaOrcamentoCotacaoDto)
            {
                foreach (var item in opcao.ListaProdutos)
                {
                    if (item.IdOperacaoAlcadaDescontoSuperior != null)
                    {
                        if (orcamento.ClienteOrcamentoCotacaoDto.ContribuinteICMS !=
                            orcamentoAntigo.ClienteOrcamentoCotacaoDto.ContribuinteICMS)
                            throw new ArgumentException("O contribuinte de ICMS não pode ser alterado!");

                        if (orcamento.ClienteOrcamentoCotacaoDto.Uf.ToUpper() !=
                            orcamentoAntigo.ClienteOrcamentoCotacaoDto.Uf.ToUpper())
                            throw new ArgumentException("A UF de entrega não ser alterada!");
                    }
                }
            }

            if (orcamento.Validade.Date != orcamentoAntigo.Validade.Date)
                throw new ArgumentException("A validade do orçamento não pode ser alterada!");

            if (orcamento.ClienteOrcamentoCotacaoDto.Tipo.ToUpper() !=
                orcamentoAntigo.ClienteOrcamentoCotacaoDto.Tipo.ToUpper())
                throw new ArgumentException("O tipo do cliente não pode ser alterado!");

            if (orcamento.IdVendedor != orcamentoAntigo.IdVendedor &&
                orcamento.Vendedor.ToUpper() != orcamentoAntigo.Vendedor.ToUpper())
                throw new ArgumentException("O vendedor não pode ser alterado!");

            if (orcamento.IdIndicadorVendedor != orcamentoAntigo.IdIndicadorVendedor &&
                orcamento.VendedorParceiro?.ToUpper() != orcamentoAntigo.VendedorParceiro?.ToUpper())
                throw new ArgumentException("O Vendedor do parceiro não pode ser alterado!");

            if (usuarioLogado.TipoUsuario == (int?)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
            {
                if (orcamento.Parceiro != Constantes.SEM_INDICADOR && (orcamento.IdIndicador != orcamentoAntigo.IdIndicador &&
                    orcamento.Parceiro.ToUpper() != orcamentoAntigo.IdIndicador.ToString().ToUpper()))
                    throw new ArgumentException("O parceiro não pode ser alterado!");
            }
            else
            {
                if (orcamento.Parceiro != Constantes.SEM_INDICADOR && (orcamento.Parceiro?.ToUpper() != orcamentoAntigo.Parceiro?.ToUpper() ||
                orcamento.IdIndicador != orcamentoAntigo.IdIndicador))
                    throw new ArgumentException("O parceiro não pode ser alterado!");
            }

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

            if (!_orcamentoCotacaoLinkBll.InserirOrcamentoCotacaoLink(orcamentoCotacaoLinkModel, contextoBdGravacao))
            {
                throw new ArgumentException("Orçamento não cadastrado. Problemas ao gravar o Link!");
            }
        }

        public void AtualizarOrcamentoCotacaoLink(OrcamentoResponseViewModel orcamento, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            TorcamentoCotacaoLink orcamentoCotacaoLinkModel = new InfraBanco.Modelos.TorcamentoCotacaoLink();

            orcamentoCotacaoLinkModel.IdOrcamentoCotacao = unchecked((int)orcamento.Id);
            orcamentoCotacaoLinkModel.Status = 2;
            orcamentoCotacaoLinkModel.DataUltStatus = DateTime.Now;
            orcamentoCotacaoLinkModel.DataHoraUltStatus = DateTime.Now;

            try
            {
                _orcamentoCotacaoLinkBll.AtualizarComTransacao(orcamentoCotacaoLinkModel, contextoBdGravacao);

            }
            catch
            {
                throw new ArgumentException("Orçamento não reenviado. Problemas ao gravar o Link!");
            }

        }

        private void AdicionarOrcamentoCotacaoEmailQueue(OrcamentoRequestViewModel orcamento, Guid guid, int idOrcamentoCotacao,
            ContextoBdGravacao contextoBdGravacao)
        {

            TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueueModel = new InfraBanco.Modelos.TorcamentoCotacaoEmailQueue();

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
                    case 35:
                        template = item.Valor;
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
                        orcamento.Id.ToString(),
                        urlBaseFront,
                        logoEmpresa
                    };

            var torcamentoCotacaoEmailQueue = _orcamentoCotacaoEmailQueueBll.InserirQueueComTemplateEHTMLComTransacao(Int32.Parse(template),
                orcamentoCotacaoEmailQueueModel, tagHtml, contextoBdGravacao);

            if (torcamentoCotacaoEmailQueue.Id == 0)
            {
                throw new ArgumentException("Não foi possível cadastrar o orçamento. Problema no envio de e-mail!");
            }
            else
            {
                TorcamentoCotacaoEmail orcamentoCotacaoEmailModel = new InfraBanco.Modelos.TorcamentoCotacaoEmail();
                orcamentoCotacaoEmailModel.IdOrcamentoCotacao = idOrcamentoCotacao;
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

        private void AdicionarOrcamentoCotacaoEmailQueue(TorcamentoCotacao orcamento, Guid guid, int idOrcamentoCotacao,
                    ContextoBdGravacao contextoBdGravacao)
        {

            TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueueModel = new InfraBanco.Modelos.TorcamentoCotacaoEmailQueue();

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
                    case 35:
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
                        guid.ToString(),
                        orcamento.Id.ToString(),
                        urlBaseFront,
                        logoEmpresa
                    };

            var torcamentoCotacaoEmailQueue = _orcamentoCotacaoEmailQueueBll.InserirQueueComTemplateEHTMLComTransacao(Int32.Parse(template),
                orcamentoCotacaoEmailQueueModel, tagHtml, contextoBdGravacao);

            if (torcamentoCotacaoEmailQueue.Id == 0)
            {
                throw new ArgumentException("Não foi possível cadastrar o orçamento. Problema no envio de e-mail!");
            }
            else
            {
                TorcamentoCotacaoEmail orcamentoCotacaoEmailModel = new InfraBanco.Modelos.TorcamentoCotacaoEmail();
                orcamentoCotacaoEmailModel.IdOrcamentoCotacao = idOrcamentoCotacao;
                orcamentoCotacaoEmailModel.IdOrcamentoCotacaoEmailQueue = torcamentoCotacaoEmailQueue.Id;
                var torcamentoCotacaoEmail = _orcamentoCotacaoEmailBll.InserirComTransacao(orcamentoCotacaoEmailModel, contextoBdGravacao);
            }

        }

        private TorcamentoCotacao MontarTorcamentoCotacao(OrcamentoRequestViewModel orcamento, UsuarioLogin usuarioLogado,
            PercMaxDescEComissaoDados percMaxDescEComissaoDados, int status)
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
                IdTipoUsuarioContextoUltStatus = (int)usuarioLogado.TipoUsuario,
                IdUsuarioCadastro = usuarioLogado.Id,
                DataCadastro = DateTime.Now.Date.Date,
                DataHoraCadastro = DateTime.Now,
                DataUltStatus = DateTime.Now.Date,
                DataHoraUltStatus = DateTime.Now,
                Status = (short)status,
                StEtgImediata = orcamento.EntregaImediata ? (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM : (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO,
                PrevisaoEntregaData = orcamento.DataEntregaImediata?.Date,
                Perc_max_comissao_e_desconto_padrao = orcamento.ClienteOrcamentoCotacaoDto.Tipo == Constantes.ID_PF ?
                    percMaxDescEComissaoDados.PercMaxComissaoEDesconto : percMaxDescEComissaoDados.PercMaxComissaoEDescontoPJ,
                ContribuinteIcms = orcamento.ClienteOrcamentoCotacaoDto.ContribuinteICMS.HasValue ? (byte)orcamento.ClienteOrcamentoCotacaoDto.ContribuinteICMS : (byte)0
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

        public MensagemDto ProrrogarOrcamento(int id, int idUsuario, string lojaLogada, int? IdTipoUsuarioContextoUltAtualizacao)
        {
            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro { Id = id }).FirstOrDefault();
            var parametros = _parametroOrcamentoCotacaoBll.ObterParametros(lojaLogada);

            if (orcamento != null)
            {
                if (orcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.APROVADO ||
                    orcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.CANCELADO)
                    return new MensagemDto
                    {
                        tipo = "WARN",
                        mensagem = "Não é possível prorrogar, orçamentos aprovados ou cancelados!"
                    };

                if (orcamento.QtdeRenovacao >= byte.Parse(parametros.QtdeMaxProrrogacao))
                    return new MensagemDto
                    {
                        tipo = "WARN",
                        mensagem = $"Excedida a quantidade máxima! {parametros.QtdeMaxProrrogacao} vezes"
                    };

                orcamento.ValidadeAnterior = orcamento.Validade;
                orcamento.QtdeRenovacao += 1;
                orcamento.IdUsuarioUltRenovacao = idUsuario;
                orcamento.DataHoraUltRenovacao = DateTime.Now;
                orcamento.DataHoraUltAtualizacao = DateTime.Now;
                orcamento.IdUsuarioUltAtualizacao = idUsuario;
                orcamento.IdTipoUsuarioContextoUltAtualizacao = IdTipoUsuarioContextoUltAtualizacao.Value;

                if (DateTime.Now.AddDays(byte.Parse(parametros.QtdePadrao_DiasProrrogacao)).Date > orcamento.DataCadastro.AddDays(byte.Parse(parametros.QtdeGlobal_Validade)).Date)
                {
                    if (orcamento.DataCadastro.AddDays(byte.Parse(parametros.QtdeGlobal_Validade)).Date > DateTime.Now.Date)
                        orcamento.Validade = orcamento.DataCadastro.AddDays(byte.Parse(parametros.QtdeGlobal_Validade)).Date;

                    if (DateTime.Now.Date > orcamento.DataCadastro.AddDays(byte.Parse(parametros.QtdeGlobal_Validade)).Date)
                        return new MensagemDto
                        {
                            tipo = "WARN",
                            mensagem = $"Não é possível prorrogar o orçamento. Validade máxima permitida de {parametros.QtdeGlobal_Validade} dias."
                        };
                }
                else
                {
                    orcamento.Validade = DateTime.Now.AddDays(byte.Parse(parametros.QtdePadrao_DiasProrrogacao));
                }

                if (orcamento.Validade.Date == orcamento.ValidadeAnterior.Value.Date)
                    return new MensagemDto
                    {
                        tipo = "WARN",
                        mensagem = $"Orçamento já foi prorrogado para {orcamento.Validade.ToString("dd/MM/yyyy")}!"
                    };

                _orcamentoCotacaoBll.Atualizar(orcamento);

                return new MensagemDto
                {
                    tipo = "INFO",
                    mensagem = $"{orcamento.Validade.ToString("yyyy-MM-ddTHH:mm:ss")}|Prorrogado para: {orcamento.Validade.ToString("dd/MM/yyyy")}. {orcamento.QtdeRenovacao}ª vez."
                };
            }

            return null;
        }

        public List<TcfgUnidadeNegocioParametro> BuscarParametros(int idCfgParametro, string lojaLogada)
        {

            var loja = _lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = lojaLogada });
            var tcfgUnidadeNegocio = _cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { Sigla = loja[0].Unidade_Negocio });

            var tcfgUnidadeNegocioParametros = _cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id, IdCfgParametro = idCfgParametro });

            if (tcfgUnidadeNegocioParametros != null)
            {
                return tcfgUnidadeNegocioParametros;
            }

            return null;
        }


        public MensagemDto AtualizarStatus(int id, UsuarioLogin user, short idStatus)
        {
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var tOrcamento = _orcamentoCotacaoBll.PorFiltroComTransacao(new TorcamentoCotacaoFiltro() { Id = id }, dbGravacao).FirstOrDefault();
                    if (tOrcamento == null) throw new Exception("Falha ao buscar Orçamento!");

                    if (idStatus == 2)
                    {

                        if (tOrcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.APROVADO)
                            return new MensagemDto
                            {
                                tipo = "WARN",
                                mensagem = "Não é possível cancelar orçamentos aprovados!"
                            };
                    }

                    tOrcamento.Status = idStatus;
                    tOrcamento.DataHoraUltStatus = DateTime.Now;
                    tOrcamento.IdUsuarioUltStatus = user.Id;
                    tOrcamento.IdTipoUsuarioContextoUltStatus = (int)user.TipoUsuario;
                    tOrcamento.DataUltStatus = DateTime.Now;
                    tOrcamento.IdTipoUsuarioContextoUltAtualizacao = (int)user.TipoUsuario;
                    tOrcamento.IdUsuarioUltAtualizacao = user.Id;
                    tOrcamento.DataHoraUltAtualizacao = DateTime.Now;
                    tOrcamento.DataHoraUltStatus = DateTime.Now;

                    _orcamentoCotacaoBll.AtualizarComTransacao(tOrcamento, dbGravacao);

                    dbGravacao.transacao.Commit();
                }
                catch (Exception ex)
                {
                    dbGravacao.transacao.Rollback();
                    throw;
                }
            }

            return null;
        }

        public async Task<List<string>> AprovarOrcamento(AprovarOrcamentoRequestViewModel aprovarOrcamento,
            Constantes.TipoUsuarioContexto tipoUsuarioContexto, int idUsuarioUltAtualizacao)
        {
            if (aprovarOrcamento == null) return new List<string>() { "É necessário preencher o cadastro do cliente!" };

            var tCliente = await _clienteBll.BuscarTcliente(UtilsGlobais.Util.SoDigitosCpf_Cnpj(aprovarOrcamento.ClienteCadastroDto.DadosCliente.Cnpj_Cpf));

            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro() { Id = aprovarOrcamento.IdOrcamento }).FirstOrDefault();
            if (orcamento == null) return new List<string>() { "Falha ao buscar Orçamento!" };

            if (orcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.APROVADO ||
                orcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.CANCELADO)
                return new List<string>() { "Esse orçamento não pode ser aprovado!" };

            aprovarOrcamento.ClienteCadastroDto.DadosCliente.Perc_max_comissao_e_desconto_padrao = orcamento.Perc_max_comissao_e_desconto_padrao;
            aprovarOrcamento.ClienteCadastroDto.DadosCliente.Perc_max_comissao_padrao = orcamento.Perc_max_comissao_padrao;
            aprovarOrcamento.ClienteCadastroDto.DadosCliente.IdIndicadorVendedor = orcamento.IdIndicadorVendedor;
            aprovarOrcamento.ClienteCadastroDto.DadosCliente.IdOrcamentoCotacao = aprovarOrcamento.IdOrcamento;

            var clienteCadastroDados = Prepedido.Dto.ClienteCadastroDto
                .ClienteCadastroDados_De_ClienteCadastroDto(aprovarOrcamento.ClienteCadastroDto);

            _logger.LogInformation("Validando cadastro de cliente!");
            var erros = await _clienteBll.ValidarClienteOrcamentoCotacao(clienteCadastroDados);
            if (erros != null) return erros;

            _logger.LogInformation("Fim da validação do cadastro de cliente!");

            List<string> retorno = new List<string>();
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTO))
            {
                try
                {
                    if (tCliente == null)
                    {
                        tCliente = new Tcliente();
                        retorno = await _clienteBll.CadastrarClienteOrcamentoCotacao(clienteCadastroDados.DadosCliente,
                            dbGravacao, orcamento.Loja, tCliente);
                        if (retorno != null) return retorno;

                        _logger.LogInformation("Cliente cadastrado com sucesso!");
                    }

                    aprovarOrcamento.ClienteCadastroDto.DadosCliente.Id = tCliente.Id;
                    //passar o id do cliente para o modelo
                    //verificar os erros 
                    retorno = await CadastrarPrepedido(aprovarOrcamento, orcamento, dbGravacao);
                    //precisamos mudar isso, precisamos verificar se existe um número de orçamento válido
                    if (retorno.Count >= 1)
                    {
                        if (!retorno[0].Contains(Constantes.SUFIXO_ID_ORCAMENTO))
                            return retorno;
                    }

                    var tcfgStatus = _orcamentoCotacaoBll.BuscarStatusParaOrcamentoCotacaoComtransacao("APROVADO", dbGravacao);
                    //mudar os parametros para receber tipo do usuário
                    orcamento.DataHoraUltAtualizacao = DateTime.Now;
                    orcamento.Status = tcfgStatus.Id;
                    orcamento.DataHoraUltStatus = DateTime.Now;
                    orcamento.IdUsuarioUltStatus = idUsuarioUltAtualizacao;
                    orcamento.IdTipoUsuarioContextoUltStatus = (int)tipoUsuarioContexto;
                    orcamento.DataUltStatus = DateTime.Now;
                    orcamento.IdTipoUsuarioContextoUltAtualizacao = (int)tipoUsuarioContexto;
                    orcamento.IdUsuarioUltAtualizacao = idUsuarioUltAtualizacao;
                    orcamento.DataHoraUltAtualizacao = DateTime.Now;
                    orcamento.DataHoraUltStatus = DateTime.Now;
                    orcamento.VersaoPoliticaCredito = BuscarParametros(25, orcamento.Loja).FirstOrDefault().Valor;
                    orcamento.VersaoPoliticaPrivacidade = BuscarParametros(26, orcamento.Loja).FirstOrDefault().Valor;
                    _orcamentoCotacaoBll.AtualizarComTransacao(orcamento, dbGravacao);

                    var opcaoSelecionada = _orcamentoCotacaoOpcaoBll
                        .PorFiltroComTransacao(new TorcamentoCotacaoOpcaoFiltro() { Id = aprovarOrcamento.IdOpcao }, dbGravacao).FirstOrDefault();
                    if (opcaoSelecionada == null)
                        return new List<string>() { "Falha ao buscar opção selecionada para aprovação do orçamento!" };

                    //atualiza opcão
                    opcaoSelecionada.IdTipoUsuarioContextoUltAtualizacao = idUsuarioUltAtualizacao;
                    opcaoSelecionada.IdUsuarioUltAtualizacao = idUsuarioUltAtualizacao;
                    opcaoSelecionada.DataHoraUltAtualizacao = DateTime.Now;
                    var objOpcao = _orcamentoCotacaoOpcaoBll.AtualizarOpcaoComTransacao(opcaoSelecionada, dbGravacao);
                    if (objOpcao == null)
                        return new List<string>() { "Falha ao atualizar opção selecionada para aprovação do orçamento!" };

                    //atualiza forma pagto
                    var formaPagtoSelecionada = _formaPagtoOrcamentoCotacaoBll
                        .PorFiltroComTransacao(new TorcamentoCotacaoOpcaoPagtoFiltro() { Id = aprovarOrcamento.IdFormaPagto }, dbGravacao).FirstOrDefault();

                    if (formaPagtoSelecionada == null)
                        return new List<string>() { "Falha ao buscar forma de pagamento da opção selecionada para aprovação do orçamento!" };

                    formaPagtoSelecionada.IdTipoUsuarioContextoAprovado = (short?)idUsuarioUltAtualizacao;
                    formaPagtoSelecionada.IdUsuarioAprovado = idUsuarioUltAtualizacao;
                    formaPagtoSelecionada.DataAprovado = DateTime.Now.Date;
                    formaPagtoSelecionada.DataHoraAprovado = DateTime.Now;
                    formaPagtoSelecionada.Aprovado = true;
                    var objPagto = _formaPagtoOrcamentoCotacaoBll.AtualizarOpcaoPagtoComTransacao(formaPagtoSelecionada, dbGravacao);

                    await dbGravacao.SaveChangesAsync();
                    dbGravacao.transacao.Commit();

                    return null;
                }
                catch (Exception ex)
                {
                    dbGravacao.transacao.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<List<string>> CadastrarPrepedido(AprovarOrcamentoRequestViewModel aprovarOrcamento, TorcamentoCotacao orcamento,
            ContextoBdGravacao dbGravacao)
        {
            _logger.LogInformation("Iniciando criação de Pré-Pedido.");
            // criar prepedidoDto
            PrePedidoDto prepedido = new PrePedidoDto();
            prepedido.DadosCliente = aprovarOrcamento.ClienteCadastroDto.DadosCliente;
            prepedido.EnderecoCadastroClientePrepedido = new EnderecoCadastralClientePrepedidoDto();
            prepedido.EnderecoCadastroClientePrepedido =
                EnderecoCadastralClientePrepedidoDto
                .EnderecoCadastralClientePrepedidoDto_De_DadosClienteCadastroDto
                (aprovarOrcamento.ClienteCadastroDto.DadosCliente);

            prepedido.EnderecoEntrega = aprovarOrcamento.enderecoEntrega;

            var opcaoSelecionada = _orcamentoCotacaoOpcaoBll
                .PorFiltro(new TorcamentoCotacaoOpcaoFiltro() { Id = aprovarOrcamento.IdOpcao }).FirstOrDefault();
            if (opcaoSelecionada == null)
                return new List<string>() { "Falha ao buscar opção selecionada para aprovação do orçamento!" };

            var formaPagtoSelecionada = opcaoSelecionada.FormaPagto.Where(x => x.Id == aprovarOrcamento.IdFormaPagto).FirstOrDefault();
            if (formaPagtoSelecionada == null)
                return new List<string>() { "Falha ao buscar forma de pagamento selecionada da opção!" };

            prepedido.FormaPagtoCriacao = new FormaPagtoCriacaoDto();
            prepedido.FormaPagtoCriacao = await IncluirFormaPagtoCriacaoParaPrepedido(formaPagtoSelecionada);

            prepedido.ListaProdutos = await IncluirProdutosParaPrepedido(opcaoSelecionada.ListaProdutos, formaPagtoSelecionada.Id, (float)orcamento.Perc_max_comissao_e_desconto_padrao);
            if (prepedido.ListaProdutos == null)
                new List<string>() { "Falha ao buscar produtos atômicos da opção!" };

            prepedido.PercRT = opcaoSelecionada.PercRT;
            prepedido.VlTotalDestePedido = prepedido.ListaProdutos.Sum(x => x.Preco_Venda * (x.Qtde ?? 0));
            //passar esses dados abaixo
            prepedido.DadosCliente.IdOrcamentoCotacao = orcamento.Id;
            prepedido.DadosCliente.Perc_max_comissao_padrao = orcamento.Perc_max_comissao_padrao;
            prepedido.DadosCliente.Perc_max_comissao_e_desconto_padrao = orcamento.Perc_max_comissao_e_desconto_padrao;
            prepedido.DadosCliente.Vendedor = aprovarOrcamento.ClienteCadastroDto.DadosCliente.Vendedor;

            prepedido.DetalhesPrepedido = new DetalhesDtoPrepedido();
            prepedido.DetalhesPrepedido.EntregaImediata = orcamento.StEtgImediata.ToString();
            prepedido.DetalhesPrepedido.EntregaImediataData = orcamento.PrevisaoEntregaData.HasValue ? orcamento.PrevisaoEntregaData : null;

            string parceiro = null;
            if (!string.IsNullOrEmpty(prepedido.DadosCliente.Indicador_Orcamentista))
            {
                parceiro = prepedido.DadosCliente.Indicador_Orcamentista;
            }

            PrePedidoDados prePedidoDados = PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prepedido);
            return (await _prepedidoBll.CadastrarPrepedido(prePedidoDados, parceiro, 0.01M, false, 
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, 12, dbGravacao)).ToList();
        }

        public async Task<FormaPagtoCriacaoDto> IncluirFormaPagtoCriacaoParaPrepedido(FormaPagtoCriacaoResponseViewModel formaPagtoSelecionada)
        {
            return await Task.FromResult(new FormaPagtoCriacaoDto()
            {
                Op_av_forma_pagto = formaPagtoSelecionada.Av_forma_pagto.ToString(),
                Op_pu_forma_pagto = formaPagtoSelecionada.Pu_forma_pagto.ToString(),
                C_pu_valor = formaPagtoSelecionada.Pu_valor,
                C_pu_vencto_apos = formaPagtoSelecionada.Pu_vencto_apos,
                C_pc_qtde = formaPagtoSelecionada.Pc_qtde_parcelas,
                C_pc_valor = Math.Round(formaPagtoSelecionada.Pc_valor_parcela, 2),
                C_pc_maquineta_qtde = formaPagtoSelecionada.Pc_maquineta_qtde_parcelas,
                C_pc_maquineta_valor = formaPagtoSelecionada.Pc_maquineta_valor_parcela,
                Op_pce_entrada_forma_pagto = formaPagtoSelecionada.Pce_forma_pagto_entrada.ToString(),
                C_pce_entrada_valor = formaPagtoSelecionada.Pce_entrada_valor,
                Op_pce_prestacao_forma_pagto = formaPagtoSelecionada.Pce_forma_pagto_prestacao.ToString(),
                C_pce_prestacao_qtde = formaPagtoSelecionada.Pce_prestacao_qtde,
                C_pce_prestacao_valor = formaPagtoSelecionada.Pce_prestacao_valor,
                C_pce_prestacao_periodo = formaPagtoSelecionada.Pce_prestacao_periodo,
                Op_pse_prim_prest_forma_pagto = formaPagtoSelecionada.Pse_forma_pagto_prim_prest.ToString(),
                C_pse_prim_prest_valor = formaPagtoSelecionada.Pse_prim_prest_valor,
                C_pse_prim_prest_apos = formaPagtoSelecionada.Pse_prim_prest_apos,
                Op_pse_demais_prest_forma_pagto = formaPagtoSelecionada.Pse_forma_pagto_demais_prest.ToString(),
                C_pse_demais_prest_qtde = formaPagtoSelecionada.Pse_demais_prest_qtde,
                C_pse_demais_prest_valor = formaPagtoSelecionada.Pse_demais_prest_valor,
                C_pse_demais_prest_periodo = formaPagtoSelecionada.Pse_demais_prest_periodo,
                C_forma_pagto = formaPagtoSelecionada.Observacao,
                Tipo_parcelamento = (short)formaPagtoSelecionada.Tipo_parcelamento,
                Rb_forma_pagto = formaPagtoSelecionada.Tipo_parcelamento.ToString()
            });
        }

        public async Task<List<PrepedidoProdutoDtoPrepedido>> IncluirProdutosParaPrepedido(List<ProdutoOrcamentoOpcaoResponseViewModel> produtosOpcaoSelecionada, int idFormaPagto,
            float Perc_max_comissao_e_desconto_padrao)
        {
            var itensAtomicosFinOpcao = await produtoOrcamentoCotacaoBll
                        .BuscarTorcamentoCotacaoOpcaoItemAtomicosCustoFin(new TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro()
                        { IdOpcaoPagto = idFormaPagto, IncluirTorcamentoCotacaoOpcaoItemAtomico = true, IncluirTorcamentoCotacaoOpcaoPagto = true });
            if (itensAtomicosFinOpcao == null) return null;

            List<PrepedidoProdutoDtoPrepedido> prepedidoProdutos = new List<PrepedidoProdutoDtoPrepedido>();
            foreach (var item in produtosOpcaoSelecionada)
            {
                var itensAtomicosCustoFin = itensAtomicosFinOpcao.Where(x => x.TorcamentoCotacaoOpcaoItemAtomico.IdItemUnificado == item.IdItemUnificado);

                foreach (var itemAtomico in itensAtomicosCustoFin)
                {

                    var existe = prepedidoProdutos.Where(x => x.Fabricante == itemAtomico.TorcamentoCotacaoOpcaoItemAtomico.Fabricante &&
                    x.Produto == itemAtomico.TorcamentoCotacaoOpcaoItemAtomico.Produto).FirstOrDefault();
                    if (existe != null)
                    {
                        //preciso verificar se o desconto é maior para atribuir a maior alçada
                        if (itemAtomico.StatusDescontoSuperior)
                        {
                            if (itemAtomico.DescDado > existe.Desc_Dado)
                            {
                                existe.StatusDescontoSuperior = true;
                                existe.IdUsuarioDescontoSuperior = itemAtomico.IdUsuarioDescontoSuperior;
                                existe.DataHoraDescontoSuperior = itemAtomico.DataHoraDescontoSuperior;
                            }
                        }

                        existe.Qtde += itemAtomico.TorcamentoCotacaoOpcaoItemAtomico.Qtde;
                        existe.Desc_Dado = (existe.Desc_Dado + itemAtomico.DescDado) / existe.Qtde;//média
                        existe.Preco_Venda = Math.Round(existe.Preco_Lista * (decimal)(1 - existe.Desc_Dado / 100), 2);
                        existe.Preco_NF = existe.Preco_Venda;
                        existe.VlTotalItem = Math.Round((decimal)existe.Qtde * existe.Preco_Venda, 2);
                        existe.TotalItem = Math.Round((decimal)existe.Qtde * existe.Preco_Venda, 2);
                    }
                    else
                    {
                        PrepedidoProdutoDtoPrepedido produtoPrepedido = new PrepedidoProdutoDtoPrepedido();
                        produtoPrepedido.Fabricante = itemAtomico.TorcamentoCotacaoOpcaoItemAtomico.Fabricante;
                        produtoPrepedido.Produto = itemAtomico.TorcamentoCotacaoOpcaoItemAtomico.Produto;
                        produtoPrepedido.Descricao = itemAtomico.TorcamentoCotacaoOpcaoItemAtomico.Descricao;
                        produtoPrepedido.Obs = "";
                        produtoPrepedido.Qtde = itemAtomico.TorcamentoCotacaoOpcaoItemAtomico.Qtde;
                        produtoPrepedido.BlnTemRa = false;
                        produtoPrepedido.CustoFinancFornecPrecoListaBase = Math.Round(itemAtomico.CustoFinancFornecPrecoListaBase, 2);
                        produtoPrepedido.Preco_Lista = Math.Round(itemAtomico.PrecoLista, 2);
                        produtoPrepedido.Desc_Dado = itemAtomico.DescDado;
                        produtoPrepedido.Preco_Venda = Math.Round(itemAtomico.PrecoVenda, 2); //Math.Round(y.Preco_Lista * (decimal)(1 - y.Desc_Dado / 100), 2);
                        produtoPrepedido.VlTotalItem = Math.Round(item.Qtde * itemAtomico.PrecoVenda, 2);
                        produtoPrepedido.TotalItem = Math.Round(item.Qtde * itemAtomico.PrecoVenda, 2);
                        produtoPrepedido.Qtde_estoque_total_disponivel = 0;
                        produtoPrepedido.CustoFinancFornecCoeficiente = itemAtomico.TorcamentoCotacaoOpcaoPagto.Tipo_parcelamento == int.Parse(Constantes.COD_FORMA_PAGTO_A_VISTA) ?
                            int.Parse(Constantes.COD_FORMA_PAGTO_A_VISTA) : itemAtomico.CustoFinancFornecCoeficiente;
                        produtoPrepedido.Preco_NF = Math.Round(itemAtomico.PrecoNF, 2);
                        produtoPrepedido.StatusDescontoSuperior = itemAtomico.StatusDescontoSuperior;
                        produtoPrepedido.IdUsuarioDescontoSuperior = itemAtomico.IdUsuarioDescontoSuperior;
                        produtoPrepedido.DataHoraDescontoSuperior = itemAtomico.DataHoraDescontoSuperior;
                        prepedidoProdutos.Add(produtoPrepedido);
                    }
                }
            }



            //alçada
            foreach (var item in prepedidoProdutos)
            {
                if (item.StatusDescontoSuperior)
                {
                    if (item.Desc_Dado <= Perc_max_comissao_e_desconto_padrao)
                    {
                        item.StatusDescontoSuperior = false;
                        item.IdUsuarioDescontoSuperior = null;
                        item.DataHoraDescontoSuperior = null;
                    }
                    else
                    {
                        var produtos = itensAtomicosFinOpcao
                           .Where(x => x.TorcamentoCotacaoOpcaoItemAtomico.Produto == item.Produto &&
                                       x.StatusDescontoSuperior == true);
                        var maiorAlcada = produtos.Max(x => x.IdOperacaoAlcadaDescontoSuperior);
                        var p = produtos.Where(x => x.IdOperacaoAlcadaDescontoSuperior == maiorAlcada).FirstOrDefault();
                        item.StatusDescontoSuperior = true;
                        item.IdUsuarioDescontoSuperior = p.IdUsuarioDescontoSuperior;
                        item.DataHoraDescontoSuperior = p.DataHoraDescontoSuperior;
                    }
                }
            }

            return prepedidoProdutos;
        }
    }
}
