﻿using Cfg.CfgUnidadeNegocio;
using Cfg.CfgUnidadeNegocioParametro;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Loja;
using Loja.Dados;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orcamento;
using Orcamento.Dto;
using OrcamentoCotacao;
using OrcamentoCotacao.Dto;
using OrcamentoCotacaoBusiness.Dto;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.Orcamento;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.Dashoard;
using OrcamentoCotacaoBusiness.Models.Response.Orcamento;
using OrcamentoCotacaoLink;
using Prepedido.Dados.DetalhesPrepedido;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais;
using UtilsGlobais.Parametros;

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
        private readonly Cfg.CfgOperacao.CfgOperacaoBll _cfgOperacaoBll;
        private readonly IConfiguration _configuration;
        private readonly CodigoDescricaoBll _codigoDescricaoBll;

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
            Prepedido.Bll.PrepedidoBll _prepedidoBll,
            Cfg.CfgOperacao.CfgOperacaoBll _cfgOperacaoBll,
            IConfiguration configuration,
            CodigoDescricaoBll _codigoDescricaoBll
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
            this._cfgOperacaoBll = _cfgOperacaoBll;
            _configuration = configuration;
            this._codigoDescricaoBll = _codigoDescricaoBll;
        }

        public OrcamentoCotacaoDto PorGuid(string guid)
        {
            if (!Guid.TryParse(guid, out _))
            {
                return null;
            }

            var orcamento = _orcamentoCotacaoBll.PorGuid(guid);

            if (orcamento != null)
            {
                var usuarioLogin = new UsuarioLogin() { TipoUsuario = (int)Constantes.TipoUsuario.CLIENTE }; //CLIENTE

                var usuario = _usuarioBll.PorFiltro(new TusuarioFiltro() { id = orcamento.idVendedor }).FirstOrDefault();
                var parceiro = orcamento.idIndicador != null ? _orcamentistaEIndicadorBll
                    .BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { idParceiro = (int)orcamento.idIndicador }) : null;
                if (orcamento.idIndicador != null && parceiro == null) throw new ArgumentException("Parceiro não encontrado!");

                string vendedorParceiro = null;
                if (orcamento.idIndicadorVendedor != null)
                {
                    var tVendedorParceiro = _orcamentistaEIndicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro()
                    {
                        id = (int)orcamento.idIndicadorVendedor
                    }).FirstOrDefault();
                    vendedorParceiro = usuarioLogin.TipoUsuario != (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO ?
                        tVendedorParceiro?.Nome : tVendedorParceiro.Email;
                }

                orcamento.usuarioCadastro = VerificarContextoCadastroOrcamento(orcamento.idTipoUsuarioContextoCadastro, usuario.Usuario, parceiro?.Apelido, vendedorParceiro);
                orcamento.amigavelUsuarioCadastro = BuscarCadastradoPorAmigavel(orcamento.idTipoUsuarioContextoCadastro, usuario.Nome_Iniciais_Em_Maiusculas, parceiro?.Razao_social_nome_iniciais_em_maiusculas, vendedorParceiro);
                orcamento.vendedor = usuario.Usuario;
                orcamento.nomeIniciaisEmMaiusculasVendedor = usuario.Nome_Iniciais_Em_Maiusculas;
                orcamento.parceiro = parceiro != null ? parceiro.Apelido : null;
                orcamento.razaoSocialNomeIniciaisEmMaiusculasParceiro = parceiro != null ? parceiro.Razao_social_nome_iniciais_em_maiusculas : null;
                orcamento.razaoSocialNomeIniciaisEmMaiusculasParceiro = parceiro != null ? parceiro.Razao_social_nome_iniciais_em_maiusculas : null;
                orcamento.vendedorParceiro = vendedorParceiro;

                var loja = _lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = orcamento.loja });
                var tcfgUnidadeNegocio = _cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { Sigla = loja[0].Unidade_Negocio });

                orcamento.lojaViewModel = _lojaBll.BuscarLojaEstilo(orcamento.loja);

                //Parametros
                var prazoMaximoConsultaOrcamento = _cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id, IdCfgParametro = 24 });
                var condicoesGerais = _cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id, IdCfgParametro = 12 });

                orcamento.condicoesGerais = condicoesGerais[0].Valor;
                orcamento.prazoMaximoConsultaOrcamento = prazoMaximoConsultaOrcamento[0].Valor;
                orcamento.listaOpcoes = _orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro { IdOrcamentoCotacao = orcamento.id }, true);

                string apelidoParceiro = null;
                string apelido = null;
                byte comIndicador = 0;
                if (orcamento.usuarioCadastro == orcamento.vendedor)
                {
                    apelido = orcamento.vendedor;
                    if (!string.IsNullOrEmpty(orcamento.parceiro))
                    {
                        comIndicador = 1;
                        apelidoParceiro = orcamento.parceiro;
                    }
                }
                if (orcamento.usuarioCadastro == orcamento.parceiro || orcamento.usuarioCadastro == orcamento.vendedorParceiro)
                {
                    comIndicador = 1;
                    apelido = orcamento.parceiro;
                    apelidoParceiro = orcamento.parceiro;
                }

                int contextoUsuario = orcamento.idTipoUsuarioContextoCadastro;

                orcamento.listaFormasPagto = _formaPagtoOrcamentoCotacaoBll.BuscarFormasPagamentos(orcamento.tipoCliente,
                    (Constantes.TipoUsuario)contextoUsuario, apelido, comIndicador, apelidoParceiro);
                orcamento.mensageria = BuscarDadosParaMensageria(usuarioLogin, orcamento.id, false);

                if (!Validar(orcamento))
                {
                    orcamento = null;
                }

                return orcamento;
            }

            return null;
        }

        public OrcamentoCotacaoDto ObterIdOrcamentoCotacao(string guid)
        {
            var orcamento = PorGuid(guid);

            if (orcamento != null)
            {
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
            if (orcamentoCotacaoDto.statusOrcamentoCotacaoLink != 1)
                return false;

            DateTime dataCriacao = (DateTime)orcamentoCotacaoDto.dataCadastro;
            DateTime dataMaximaConsulta = dataCriacao.AddDays(int.Parse(orcamentoCotacaoDto.prazoMaximoConsultaOrcamento));

            if (dataMaximaConsulta.Date <= DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }


        public List<DashoardResponse> Dashboard(TorcamentoFiltro tOrcamentoFiltro, UsuarioLogin usuarioLogin)
        {
            int idUsuario;

            using (var dbGravacao = _contextoBdProvider.GetContextoLeitura())
            {
                if (usuarioLogin.TipoUsuario == 1)
                {
                    var usuario = (from u in dbGravacao.Tusuario
                                   where u.Usuario == usuarioLogin.Apelido.ToUpper().Trim()
                                   select u).FirstOrDefault();

                    idUsuario = usuario.Id;
                }
                else if (usuarioLogin.TipoUsuario == 2)
                {
                    var orcamentista = (from u in dbGravacao.TorcamentistaEindicador
                                        where u.Apelido == usuarioLogin.Apelido.ToUpper().Trim()
                                        select u).FirstOrDefault();

                    idUsuario = orcamentista.IdIndicador;
                }
                else
                {
                    var vendedorParceiro = (from u in dbGravacao.TorcamentistaEindicadorVendedor
                                            where u.Email == usuarioLogin.Apelido.ToUpper().Trim()
                                            select u).FirstOrDefault();

                    idUsuario = vendedorParceiro.Id;
                }
            }

            var orcamentoCotacaoFiltro = new TorcamentoCotacaoFiltro
            {
                LimitarDataDashboard = true,
                Loja = tOrcamentoFiltro.Loja,
            };

            if (usuarioLogin.TipoUsuario == 1)
            {
                if (!tOrcamentoFiltro.PermissaoUniversal)
                {
                    orcamentoCotacaoFiltro.IdVendedor = idUsuario;
                }
            }
            else if (usuarioLogin.TipoUsuario == 2)
            {
                orcamentoCotacaoFiltro.IdIndicador = idUsuario;
            }
            else
            {
                orcamentoCotacaoFiltro.IdIndicadorVendedor = idUsuario;
            }

            orcamentoCotacaoFiltro.StatusId = tOrcamentoFiltro.StatusId;

            var orcamentoCotacaoListaDto = _orcamentoCotacaoBll.PorFiltro(orcamentoCotacaoFiltro);

            List<DashoardResponse> listaDashboard = new List<DashoardResponse>();
            if (orcamentoCotacaoListaDto != null)
            {
                var vendedores = _usuarioBll.PorFiltro(new TusuarioFiltro { });
                var parceiros = _orcamentistaEIndicadorBll.BuscarParceiros(new TorcamentistaEindicadorFiltro { });
                var vendParceiros = _orcamentistaEIndicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro { });

                orcamentoCotacaoListaDto.ForEach(x => listaDashboard.Add(new DashoardResponse()
                {
                    NumeroOrcamento = x.Id.ToString(),
                    Vendedor = vendedores.FirstOrDefault(v => v.Id == x.IdVendedor)?.Usuario,
                    Parceiro = parceiros.FirstOrDefault(v => v.IdIndicador == x.IdIndicador) == null ? "-" : parceiros.FirstOrDefault(v => v.IdIndicador == x.IdIndicador).Apelido,
                    VendedorParceiro = vendParceiros.FirstOrDefault(v => v.Id == x.IdIndicadorVendedor)?.Nome,
                    IdIndicadorVendedor = vendParceiros.FirstOrDefault(v => v.Id == x.IdIndicadorVendedor)?.Id,
                    DtExpiracao = x.Validade

                })); ;

            }

            return listaDashboard;
        }

        public OrcamentoCotacaoListaResponse PorFiltro(TorcamentoFiltro tOrcamentoFiltro, UsuarioLogin usuarioLogin)
        {
            var orcamentoCotacaoFiltro = new TorcamentoCotacaoFiltro
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

            if (!tOrcamentoFiltro.PermissaoUniversal)
            {
                if (tOrcamentoFiltro.TipoUsuario.HasValue)
                {
                    if (tOrcamentoFiltro.TipoUsuario.Value == (int)Constantes.TipoUsuario.VENDEDOR)
                    {
                        orcamentoCotacaoFiltro.IdVendedor = usuarioLogin.Id;
                        tOrcamentoFiltro.Vendedor = usuarioLogin.Apelido;
                    }

                    if (tOrcamentoFiltro.TipoUsuario.Value == (int)Constantes.TipoUsuario.PARCEIRO)
                    {
                        orcamentoCotacaoFiltro.IdIndicador = usuarioLogin.Id;
                        //tOrcamentoFiltro.Vendedor = usuarioLogin.VendedorResponsavel;
                        tOrcamentoFiltro.Parceiro = usuarioLogin.IdParceiro;
                        //orcamentoCotacaoFiltro.Vendedor = usuarioLogin.VendedorResponsavel;
                        orcamentoCotacaoFiltro.Parceiro = usuarioLogin.IdParceiro;
                    }

                    if (tOrcamentoFiltro.TipoUsuario.Value == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                    {
                        orcamentoCotacaoFiltro.IdIndicadorVendedor = usuarioLogin.Id;
                        tOrcamentoFiltro.VendedorParceiro = usuarioLogin.Nome;
                    }
                }
            }

            if (tOrcamentoFiltro.Origem == "ORCAMENTOS")
            {
                var response = new OrcamentoCotacaoListaResponse();

                if (!tOrcamentoFiltro.DtInicio.HasValue)
                {
                    response.Mensagem = "O campo 'Início da criação' é obrigatório!";
                    response.Sucesso = false;
                    return response;
                }
                if (!tOrcamentoFiltro.DtFim.HasValue)
                {
                    response.Mensagem = "O campo 'Fim da criação' é obrigatório!";
                    response.Sucesso = false;
                    return response;
                }

                var orcamentoCotacaoConusltaDto = _orcamentoCotacaoBll.ConsultaOrcamento(tOrcamentoFiltro);

                response.CorrelationId = orcamentoCotacaoConusltaDto.CorrelationId;
                response.Sucesso = orcamentoCotacaoConusltaDto.Sucesso;
                response.Mensagem = orcamentoCotacaoConusltaDto.Mensagem;
                response.Mensagens = orcamentoCotacaoConusltaDto.Mensagens;
                response.qtdeRegistros = orcamentoCotacaoConusltaDto.QtdeRegistros;

                if (orcamentoCotacaoConusltaDto.OrcamentoCotacaoLista.Count > 0)
                {
                    foreach (var OrcamentoCotacaoItem in orcamentoCotacaoConusltaDto.OrcamentoCotacaoLista)
                    {
                        response.orcamentoCotacaoListaDto.Add(new OrcamentoCotacaoListaDto()
                        {
                            NumeroOrcamento = OrcamentoCotacaoItem.NumeroOrcamento,
                            NumPedido = OrcamentoCotacaoItem.NumPedido,
                            Cliente_Obra = OrcamentoCotacaoItem.Cliente_Obra,
                            Vendedor = OrcamentoCotacaoItem.Vendedor,
                            Parceiro = OrcamentoCotacaoItem.Parceiro,
                            VendedorParceiro = OrcamentoCotacaoItem.VendedorParceiro,
                            Valor = OrcamentoCotacaoItem.Valor,
                            Status = OrcamentoCotacaoItem.Status,
                            IdStatus = OrcamentoCotacaoItem.IdStatus,
                            VistoEm = OrcamentoCotacaoItem.VistoEm,
                            Mensagem = OrcamentoCotacaoItem.Mensagem,
                            DtCadastro = OrcamentoCotacaoItem.DtCadastro,
                            DtExpiracao = OrcamentoCotacaoItem.DtExpiracao,
                            Orcamentista = OrcamentoCotacaoItem.Orcamentista,
                            Loja = OrcamentoCotacaoItem.Loja,
                            IdOrcamentoCotacao = OrcamentoCotacaoItem.IdOrcamentoCotacao,
                            IdIndicadorVendedor = OrcamentoCotacaoItem.IdIndicadorVendedor,
                            DtInicio = OrcamentoCotacaoItem.DtInicio,
                            DtFim = OrcamentoCotacaoItem.DtFim,
                            St_Orc_Virou_Pedido = OrcamentoCotacaoItem.St_Orc_Virou_Pedido,
                            IdVendedor = OrcamentoCotacaoItem.IdVendedor,
                            DataHoraCadastro = OrcamentoCotacaoItem.DataHoraCadastro,
                            NomeCliente = OrcamentoCotacaoItem.NomeCliente,
                            NomeObra = OrcamentoCotacaoItem.NomeObra
                        });
                    }
                }

                return response;
            }
            else if (tOrcamentoFiltro.Origem == "PENDENTES") //PrePedido/Em Aprovação [tOrcamentos]
            {
                var response = new OrcamentoCotacaoListaResponse();

                if (!tOrcamentoFiltro.DtInicio.HasValue)
                {
                    response.Mensagem = "O campo 'Início da criação' é obrigatório!";
                    response.Sucesso = false;
                    return response;
                }
                if (!tOrcamentoFiltro.DtFim.HasValue)
                {
                    response.Mensagem = "O campo 'Fim da criação' é obrigatório!";
                    response.Sucesso = false;
                    return response;
                }

                var orcamentoConusltaDto = _orcamentoBll.OrcamentoPorFiltro(tOrcamentoFiltro);

                response.CorrelationId = orcamentoConusltaDto.CorrelationId;
                response.Sucesso = orcamentoConusltaDto.Sucesso;
                response.Mensagem = orcamentoConusltaDto.Mensagem;
                response.Mensagens = orcamentoConusltaDto.Mensagens;
                response.qtdeRegistros = orcamentoConusltaDto.QtdeRegistros;

                if (orcamentoConusltaDto.OrcamentoCotacaoLista.Count > 0)
                {
                    foreach (var OrcamentoCotacaoItem in orcamentoConusltaDto.OrcamentoCotacaoLista)
                    {
                        response.orcamentoCotacaoListaDto.Add(new OrcamentoCotacaoListaDto()
                        {
                            NumeroOrcamento = OrcamentoCotacaoItem.NumeroOrcamento,
                            NumPedido = OrcamentoCotacaoItem.NumPedido,
                            Cliente_Obra = OrcamentoCotacaoItem.Cliente_Obra,
                            Vendedor = OrcamentoCotacaoItem.Vendedor,
                            Parceiro = OrcamentoCotacaoItem.Parceiro,
                            VendedorParceiro = OrcamentoCotacaoItem.VendedorParceiro,
                            Valor = OrcamentoCotacaoItem.Valor,
                            Status = OrcamentoCotacaoItem.Status,
                            IdStatus = OrcamentoCotacaoItem.IdStatus,
                            VistoEm = OrcamentoCotacaoItem.VistoEm,
                            Mensagem = OrcamentoCotacaoItem.Mensagem,
                            DtCadastro = OrcamentoCotacaoItem.DtCadastro,
                            DtExpiracao = OrcamentoCotacaoItem.DtExpiracao,
                            Orcamentista = OrcamentoCotacaoItem.Orcamentista,
                            Loja = OrcamentoCotacaoItem.Loja,
                            IdOrcamentoCotacao = OrcamentoCotacaoItem.IdOrcamentoCotacao,
                            IdIndicadorVendedor = OrcamentoCotacaoItem.IdIndicadorVendedor,
                            DtInicio = OrcamentoCotacaoItem.DtInicio,
                            DtFim = OrcamentoCotacaoItem.DtFim,
                            St_Orc_Virou_Pedido = OrcamentoCotacaoItem.St_Orc_Virou_Pedido,
                            IdVendedor = OrcamentoCotacaoItem.IdVendedor,
                            DataHoraCadastro = OrcamentoCotacaoItem.DataHoraCadastro,
                            NomeCliente = OrcamentoCotacaoItem.NomeCliente,
                            NomeObra = OrcamentoCotacaoItem.NomeObra
                        });
                    }
                }

                return response;
            }
            else
            {
                var response = new OrcamentoCotacaoListaResponse();

                if (!tOrcamentoFiltro.DtInicio.HasValue)
                {
                    response.Mensagem = "O campo 'Início da criação' é obrigatório!";
                    response.Sucesso = false;
                    return response;
                }
                if (!tOrcamentoFiltro.DtFim.HasValue)
                {
                    response.Mensagem = "O campo 'Fim da criação' é obrigatório!";
                    response.Sucesso = false;
                    return response;
                }

                var pedidoConsultaDto = _pedidoPrepedidoApiBll.ListarPedidos(tOrcamentoFiltro);

                response.CorrelationId = pedidoConsultaDto.CorrelationId;
                response.Sucesso = pedidoConsultaDto.Sucesso;
                response.Mensagem = pedidoConsultaDto.Mensagem;
                response.Mensagens = pedidoConsultaDto.Mensagens;
                response.qtdeRegistros = pedidoConsultaDto.QtdeRegistros;

                if (pedidoConsultaDto.OrcamentoCotacaoLista.Count > 0)
                {
                    foreach (var OrcamentoCotacaoItem in pedidoConsultaDto.OrcamentoCotacaoLista)
                    {
                        response.orcamentoCotacaoListaDto.Add(new OrcamentoCotacaoListaDto()
                        {
                            NumeroOrcamento = OrcamentoCotacaoItem.NumeroOrcamento,
                            NumPedido = OrcamentoCotacaoItem.NumPedido,
                            Cliente_Obra = OrcamentoCotacaoItem.Cliente_Obra,
                            Vendedor = OrcamentoCotacaoItem.Vendedor,
                            Parceiro = OrcamentoCotacaoItem.Parceiro,
                            VendedorParceiro = OrcamentoCotacaoItem.VendedorParceiro,
                            Valor = OrcamentoCotacaoItem.Valor,
                            Status = OrcamentoCotacaoItem.Status,
                            IdStatus = OrcamentoCotacaoItem.IdStatus,
                            VistoEm = OrcamentoCotacaoItem.VistoEm,
                            Mensagem = OrcamentoCotacaoItem.Mensagem,
                            DtCadastro = OrcamentoCotacaoItem.DtCadastro,
                            DtExpiracao = OrcamentoCotacaoItem.DtExpiracao,
                            Orcamentista = OrcamentoCotacaoItem.Orcamentista,
                            Loja = OrcamentoCotacaoItem.Loja,
                            IdOrcamentoCotacao = OrcamentoCotacaoItem.IdOrcamentoCotacao,
                            IdIndicadorVendedor = OrcamentoCotacaoItem.IdIndicadorVendedor,
                            DtInicio = OrcamentoCotacaoItem.DtInicio,
                            DtFim = OrcamentoCotacaoItem.DtFim,
                            St_Orc_Virou_Pedido = OrcamentoCotacaoItem.St_Orc_Virou_Pedido,
                            IdVendedor = OrcamentoCotacaoItem.IdVendedor,
                            DataHoraCadastro = OrcamentoCotacaoItem.DataHoraCadastro,
                            NomeCliente = OrcamentoCotacaoItem.NomeCliente,
                            NomeObra = OrcamentoCotacaoItem.NomeObra
                        });
                    }
                }

                return response;
            }
        }

        public OrcamentoResponseViewModel PorFiltro(int id, int tipoUsuario)
        {
            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro() { Id = id }).FirstOrDefault();
            if (orcamento == null) throw new Exception("Falha ao buscar Orçamento!");

            var opcao = _orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro() { IdOrcamentoCotacao = id }, false);
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

            if (opcao.Count <= 0)
            {
                throw new Exception("Falha ao buscar Opções do Orçamento!");
            }

            var usuario = _usuarioBll.PorFiltro(
                new TusuarioFiltro()
                {
                    id = orcamento.IdVendedor
                }).FirstOrDefault();

            var parceiro = orcamento.IdIndicador != null
                ? _orcamentistaEIndicadorBll.BuscarParceiroPorApelido(
                    new TorcamentistaEindicadorFiltro()
                    {
                        idParceiro = (int)orcamento.IdIndicador
                    })
                : null;

            string vendedorParceiro = null;

            if (orcamento.IdIndicadorVendedor != null)
            {
                var tVendedorParceiro = _orcamentistaEIndicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro()
                {
                    id = (int)orcamento.IdIndicadorVendedor
                }).FirstOrDefault();

                vendedorParceiro = tVendedorParceiro?.Nome;
            }

            OrcamentoResponseViewModel orcamentoResponse = new OrcamentoResponseViewModel()
            {
                Id = orcamento.Id,
                Vendedor = usuario.Usuario,
                NomeIniciaisEmMaiusculasVendedor = usuario.Nome_Iniciais_Em_Maiusculas,
                Parceiro = parceiro != null ? parceiro.Apelido : null,
                RazaoSocialNomeIniciaisEmMaiusculasParceiro = parceiro != null ? string.IsNullOrEmpty(parceiro.NomeFantasia) ? parceiro.Razao_social_nome_iniciais_em_maiusculas : parceiro.NomeFantasia : null,
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
                ClienteOrcamentoCotacaoDto = new CadastroOrcamentoClienteRequest()
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
                CadastradoPor = VerificarContextoCadastroOrcamento(orcamento.IdTipoUsuarioContextoCadastro, usuario.Usuario, parceiro?.Apelido, vendedorParceiro),
                AmigavelCadastradoPor = BuscarCadastradoPorAmigavel(orcamento.IdTipoUsuarioContextoCadastro, usuario.Nome_Iniciais_Em_Maiusculas, parceiro != null ? string.IsNullOrEmpty(parceiro.NomeFantasia) ? parceiro.Razao_social_nome_iniciais_em_maiusculas : parceiro.NomeFantasia : null, vendedorParceiro),
                InstaladorInstala = orcamento.InstaladorInstalaStatus
            };

            var loja = _lojaBll.PorFiltro(new TlojaFiltro() { Loja = orcamento.Loja });
            var tcfgUnidadeNegocio = _cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { Sigla = loja[0].Unidade_Negocio });
            var tcfgUnidadeNegocioParametros = _cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id });
            var tOrcamentoCotacaoLink = _orcamentoCotacaoLinkBll.PorFiltro(new TorcamentoCotacaoLinkFiltro() { IdOrcamentoCotacao = orcamento.Id, Status = 1 }).FirstOrDefault();

            var link = "{URL_Base_Front}#/publico/orcamento/{LinkOrcamento}";
            link = link.Replace("{URL_Base_Front}", tcfgUnidadeNegocioParametros.Where(x => x.IdCfgParametro == 2).FirstOrDefault().Valor);
            link = link.Replace("{LinkOrcamento}", tOrcamentoCotacaoLink.Guid.ToString());
            orcamentoResponse.Link = link;

            return orcamentoResponse;
        }

        public OrcamentoResponse PorFiltroNovo(int id, int tipoUsuario)
        {
            OrcamentoResponse response = new OrcamentoResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro() { Id = id }).FirstOrDefault();
            if (orcamento == null)
            {
                response.Mensagem = "Falha ao buscar Orçamento!";
                return response;
            }

            var opcao = _orcamentoCotacaoOpcaoBll.PorFiltro(new TorcamentoCotacaoOpcaoFiltro() { IdOrcamentoCotacao = id }, false);
            if (opcao.Count <= 0)
            {
                response.Mensagem = "Falha ao buscar Opções do Orçamento!";
                return response;
            }

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

            if (opcao.Count <= 0)
            {
                response.Mensagem = "Falha ao buscar Opções do Orçamento!";
                return response;
            }

            var usuario = _usuarioBll.PorFiltro(new TusuarioFiltro() { id = orcamento.IdVendedor }).FirstOrDefault();
            var parceiro = orcamento.IdIndicador != null ? _orcamentistaEIndicadorBll
                .BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { idParceiro = (int)orcamento.IdIndicador }) : null;

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

            response.Id = orcamento.Id;
            response.Vendedor = usuario.Usuario;
            response.NomeIniciaisEmMaiusculasVendedor = usuario.Nome_Iniciais_Em_Maiusculas;
            response.Parceiro = parceiro != null ? parceiro.Apelido : null;
            response.RazaoSocialNomeIniciaisEmMaiusculasParceiro = parceiro != null ? parceiro.Razao_social_nome_iniciais_em_maiusculas : null;
            response.VendedorParceiro = vendedorParceiro;
            response.Loja = orcamento.Loja;
            response.Validade = orcamento.Validade;
            response.QtdeRenovacao = orcamento.QtdeRenovacao;
            response.ConcordaWhatsapp = orcamento.AceiteWhatsApp;
            response.ObservacoesGerais = orcamento.Observacao;
            response.EntregaImediata = orcamento.StEtgImediata == (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO ? false : true;
            response.DataEntregaImediata = orcamento.PrevisaoEntregaData;
            response.Status = orcamento.Status;
            response.statusEmail = statusEmail;
            response.DataCadastro = orcamento.DataCadastro;
            response.IdIndicador = orcamento.IdIndicador;
            response.IdIndicadorVendedor = orcamento.IdIndicadorVendedor;
            response.IdVendedor = orcamento.IdVendedor;
            response.ClienteOrcamentoCotacaoDto = new OrcamentoClienteResponse()
            {
                NomeCliente = orcamento.NomeCliente,
                NomeObra = orcamento.NomeObra,
                Email = orcamento.Email,
                Telefone = orcamento.Telefone,
                Tipo = orcamento.TipoCliente,
                Uf = orcamento.UF,
                ContribuinteICMS = orcamento.ContribuinteIcms
            };
            response.ListaOrcamentoCotacaoDto = opcao;
            response.CadastradoPor = VerificarContextoCadastroOrcamento(orcamento.IdTipoUsuarioContextoCadastro,
                usuario.Usuario, parceiro?.Apelido, vendedorParceiro);
            response.AmigavelCadastradoPor = BuscarCadastradoPorAmigavel(orcamento.IdTipoUsuarioContextoCadastro,
                usuario.Nome_Iniciais_Em_Maiusculas, parceiro?.Razao_social_nome_iniciais_em_maiusculas, vendedorParceiro);
            response.InstaladorInstala = orcamento.InstaladorInstalaStatus;

            response.Sucesso = true;
            return response;
        }

        private string BuscarCadastradoPorAmigavel(int idTipoUsuarioContextoCadastro, string vendedor, string parceiro, string vendedorParceiro)
        {
            var lstTipoUsuariosContexto = _usuarioBll.BuscarTipoUsuarioContexto();
            if (lstTipoUsuariosContexto == null) return null;

            var filtrado = lstTipoUsuariosContexto.Where(x => x.Id == idTipoUsuarioContextoCadastro).FirstOrDefault();
            if (filtrado == null) return null;

            if (filtrado.Id == (short)Constantes.TipoUsuarioContexto.UsuarioInterno) { return vendedor; }
            if (filtrado.Id == (short)Constantes.TipoUsuarioContexto.Parceiro) { return parceiro; }
            if (filtrado.Id == (short)Constantes.TipoUsuarioContexto.VendedorParceiro) { return vendedorParceiro; }

            return null;
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

            var loja = _lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = orcamento.Loja }).FirstOrDefault();
            var tcfgUnidadeNegocio = _cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { Sigla = loja.Unidade_Negocio });

            var prazoMaximoConsultaOrcamento = _cfgUnidadeNegocioParametroBll.PorFiltro(new TcfgUnidadeNegocioParametroFiltro() { IdCfgUnidadeNegocio = tcfgUnidadeNegocio.FirstOrDefault().Id, IdCfgParametro = 24 }).FirstOrDefault();

            if (!usuarioIterno) return _mensagemBll.CriarRemetenteCliente(orcamento);

            var dataMaxTrocaMsg = orcamento.DataCadastro.AddDays(int.Parse(prazoMaximoConsultaOrcamento.Valor) - 7);

            var response = _mensagemBll.CriarRemetenteUsuarioInterno(orcamento, usuario.Id, dataMaxTrocaMsg);

            return response;
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
                MaxPeriodoConsultaFiltroPesquisa = parametros.MaxPeriodoConsultaFiltroPesquisa,
                MaxPeriodoConsulta_RelatorioGerencial = parametros.MaxPeriodoConsulta_RelatorioGerencial
            };
        }


        private string ValidarClienteOrcamento(CadastroOrcamentoClienteRequest cliente)
        {
            if (cliente == null) return "Ops! Favor preencher os dados do cliente!";

            if (string.IsNullOrEmpty(cliente.NomeCliente))
                return "O nome do cliente é obrigatório!";

            if (cliente.NomeCliente.Length > 60)
                return "O nome do cliente excede a quantidade máxima de caracteres permitido!";

            if (!string.IsNullOrEmpty(cliente.NomeObra) && cliente.NomeObra.Length > 120)
                return "O nome da obra execede a quantidade máxima de caracteres permitido!";

            if (!new EmailAddressAttribute().IsValid(cliente.Email)) return "E-mail inválido!";

            if (!string.IsNullOrEmpty(cliente.Telefone) && cliente.Telefone.Length > 15) return "Telefone inválido!";

            if (string.IsNullOrEmpty(cliente.Uf)) return "Informe a UF de entrega!";

            if (!UtilsGlobais.Util.VerificaUf(cliente.Uf)) return "Uf de entrega inválida!";

            if (string.IsNullOrEmpty(cliente.Tipo)) return "Informe se o cliente é pessoa física ou jurídica!";

            if (cliente.Tipo.Length > 2) return "O tipo do cliente execede a quantidade máxima de caracteres permitido!";

            if (cliente.Tipo.ToUpper() != Constantes.ID_PF && cliente.Tipo.ToUpper() != Constantes.ID_PJ)
                return "Tipo do cliente inválido!";

            if (cliente.Tipo == Constantes.ID_PJ)
            {
                if (cliente.ContribuinteICMS == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL ||
                (cliente.ContribuinteICMS != (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                cliente.ContribuinteICMS != (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                cliente.ContribuinteICMS != (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO))
                    return "Contribuinte de ICMS inválido!";
            }

            if (cliente.Tipo == Constantes.ID_PF)
            {
                if (cliente.ContribuinteICMS != null && cliente.ContribuinteICMS > 0)
                    return "Cliente pessoa física não pode ter valor de contribuinte ICMS!";
            }

            return null;
        }

        private string ValidarDetalhesOrcamento(CadastroOrcamentoRequest orcamento)
        {
            if (orcamento.EntregaImediata)
            {
                var param = BuscarParametros(37, orcamento.Loja).FirstOrDefault();
                if (param == null) return "Falha ao buscar parâmentro para validação!";//aletrar retorno para mensagem

                var dataAtual = DateTime.Now.Date;
                if (orcamento.DataEntregaImediata?.Date < dataAtual) return "A Data de entrega não pode ser menor que a data atual!";
                if (orcamento.DataEntregaImediata?.Date > dataAtual.AddDays(int.Parse(param.Valor)))
                    return "A Data de entrega ultrapassa o valor máximo permitido!";
            }

            if (!string.IsNullOrEmpty(orcamento.Parceiro) && orcamento.Parceiro != Constantes.SEM_INDICADOR)
            {
                string msgInstalador = "É necessário informar um valor válido para instalador instala!";
                if (orcamento.InstaladorInstala != (int)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO &&
                    orcamento.InstaladorInstala != (int)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM)
                    return msgInstalador;
                if (orcamento.InstaladorInstala == (int)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO_DEFINIDO)
                    return msgInstalador;
            }

            return null;
        }

        public CadastroOrcamentoResponse CadastrarOrcamentoCotacao(CadastroOrcamentoRequest orcamento, UsuarioLogin usuarioLogado)
        {
            var response = new CadastroOrcamentoResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Iniciando validações do orçamento. Orçamento => [{JsonSerializer.Serialize(orcamento)}].");

            _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Verificando quantidade de opções. Quantidade de opções => [{orcamento.ListaOrcamentoCotacaoDto.Count}].");
            if (orcamento.ListaOrcamentoCotacaoDto.Count <= 0)
            {
                response.Mensagem = "Necessário ter ao menos uma opção de orçamento!";
                return response;
            }

            _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Validar dados do cliente. Cliente => [{JsonSerializer.Serialize(orcamento.ClienteOrcamentoCotacaoDto)}].");
            response.Mensagem = ValidarClienteOrcamento(orcamento.ClienteOrcamentoCotacaoDto);
            if (!string.IsNullOrEmpty(response.Mensagem)) return response;

            _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Validar entrega imediata e instalador instala.");
            response.Mensagem = ValidarDetalhesOrcamento(orcamento);
            if (!string.IsNullOrEmpty(response.Mensagem)) return response;

            _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Abrindo transação.");
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Buscar percentual de desconto e comissão por loja. Loja => [{orcamento.Loja}].");
                    var percentualMaxDescontoEComissao = _lojaBll.BuscarPercMaxPorLoja(orcamento.Loja);
                    if (percentualMaxDescontoEComissao == null)
                    {
                        response.Mensagem = "Falha ao tentar gravar orçamento!";
                        return response;
                    }
                    _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Retorno da busca de percentual de desconto e comissão por loja. Response => [{JsonSerializer.Serialize(percentualMaxDescontoEComissao)}].");

                    var tOrcamentoCotacao = MontarTorcamentoCotacao(orcamento, usuarioLogado, percentualMaxDescontoEComissao, 1);
                    if (tOrcamentoCotacao == null)
                    {
                        response.Mensagem = "Falha ao tentar montar os dados do orçamento!";
                        return response;
                    }
                    _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Cadastrar orçamento cotação. Request => [{JsonSerializer.Serialize(tOrcamentoCotacao)}].");
                    tOrcamentoCotacao = _orcamentoCotacaoBll.InserirComTransacao(tOrcamentoCotacao, dbGravacao);
                    if (tOrcamentoCotacao.Id == 0)
                    {
                        response.Mensagem = "Ops! Falha ao cadastrar orçamento!";
                        return response;
                    }

                    //montar o log de t_orcamento_cotacao
                    //incluir dataannotations nas tabelas utilizadas no orçamento para conseguir capturar o nome real da coluna da tabela
                    string log = "";
                    string camposAOmitir = "|Id|Loja|ValidadeAnterior|QtdeRenovacao|IdUsuarioUltRenovacao|DataHoraUltRenovacao|Status|IdTipoUsuarioContextoUltStatus|IdUsuarioUltStatus|DataUltStatus|DataHoraUltStatus|IdOrcamento|IdPedido|IdTipoUsuarioContextoCadastro|IdUsuarioCadastro|DataCadastro|DataHoraCadastro|IdTipoUsuarioContextoUltAtualizacao|IdUsuarioUltAtualizacao|DataHoraUltAtualizacao|perc_max_comissao_padrao|perc_max_comissao_e_desconto_padrao|VersaoPoliticaCredito|VersaoPoliticaPrivacidade|InstaladorInstalaIdTipoUsuarioContexto|InstaladorInstalaIdUsuarioUltAtualiz|InstaladorInstalaDtHrUltAtualiz|GarantiaIndicadorIdTipoUsuarioContexto|GarantiaIndicadorIdUsuarioUltAtualiz|GarantiaIndicadorDtHrUltAtualiz|EtgImediataIdTipoUsuarioContexto|EtgImediataIdUsuarioUltAtualiz|EtgImediataDtHrUltAtualiz|PrevisaoEntregaIdTipoUsuarioContexto|PrevisaoEntregaIdUsuarioUltAtualiz|PrevisaoEntregaDtHrUltAtualiz|IdTipoUsuarioContextoUltRenovacao|GarantiaIndicadorStatus|";
                    log = UtilsGlobais.Util.MontaLog(tOrcamentoCotacao, log, camposAOmitir);


                    //colocar uma string de log para retornar o log montado do cadastro das opções 
                    var responseOpcoes = _orcamentoCotacaoOpcaoBll.CadastrarOrcamentoCotacaoOpcoesComTransacao(orcamento.ListaOrcamentoCotacaoDto,
                        tOrcamentoCotacao.Id, usuarioLogado, dbGravacao, orcamento.Loja, orcamento.CorrelationId);
                    if (!string.IsNullOrEmpty(responseOpcoes.Mensagem))
                    {
                        response.Mensagem = responseOpcoes.Mensagem;
                        return response;
                    }

                    log = $"{log}\r   {responseOpcoes.LogOperacao}";

                    var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = 1 }, dbGravacao).FirstOrDefault();
                    if (cfgOperacao == null)
                    {
                        response.Mensagem = "Ops! Falha ao cadastrar orçamento.";
                        return response;
                    }

                    var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogado.TipoUsuario, usuarioLogado.Id, orcamento.Loja, null, tOrcamentoCotacao.Id, null,
                        InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, orcamento.IP);

                    var guid = Guid.NewGuid();

                    _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Cadastrar link de orçamento cotação.");
                    response.Mensagem = AdicionarOrcamentoCotacaoLink(tOrcamentoCotacao, guid, dbGravacao);
                    if (!string.IsNullOrEmpty(response.Mensagem)) return response;

                    _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Cadastrar e-mail orçamento cotação.");
                    response.Mensagem = AdicionarOrcamentoCotacaoEmailQueue(orcamento, guid, tOrcamentoCotacao.Id, dbGravacao);
                    if (!string.IsNullOrEmpty(response.Mensagem)) return response;

                    dbGravacao.transacao.Commit();
                }
                catch (Exception ex)
                {
                    dbGravacao.transacao.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            _logger.LogInformation($"CorrelationId => [{orcamento.CorrelationId}]. {nomeMetodo}. Cadastro de orçamento cotação finalizado.");
            response.Sucesso = true;
            return response;
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

        public AtualizarOrcamentoOpcaoResponse AtualizarOrcamentoOpcao(AtualizarOrcamentoOpcaoRequest opcao, UsuarioLogin usuarioLogado)
        {
            var response = new AtualizarOrcamentoOpcaoResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Buscando orçamento cotação. Id => [{opcao.Id}]");
            var orcamentoResponse = PorFiltroNovo(opcao.IdOrcamentoCotacao, (int)usuarioLogado.TipoUsuario);
            if (!orcamentoResponse.Sucesso)
            {
                response.Mensagem = orcamentoResponse.Mensagem;
                return response;
            }
            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Retorno da busca de orçamento cotação. Response => [{JsonSerializer.Serialize(orcamentoResponse)}]");

            response = _orcamentoCotacaoOpcaoBll.AtualizarOrcamentoOpcao(opcao, usuarioLogado, orcamentoResponse);
            if (!string.IsNullOrEmpty(response.Mensagem))
            {
                response.Mensagem = response.Mensagem;
                return response;
            }
            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. {nomeMetodo}. Atualização da opção Id[{opcao.Id}] do orçamento Id[{opcao.IdOrcamentoCotacao}] finalizada!");

            response.Sucesso = true;
            return response;
        }

        public OrcamentoResponseViewModel AtualizarDadosCadastraisOrcamento(OrcamentoResponseViewModel orcamento,
            UsuarioLogin usuarioLogado, string ip)
        {
            _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Iniciando.");
            _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Buscando orçamento.");
            var orcamentoAntigo = PorFiltro((int)orcamento.Id, (int)usuarioLogado.TipoUsuario);
            if (orcamentoAntigo == null) orcamento.Erro = "Orçamento não encontrado!";

            if (!string.IsNullOrEmpty(orcamento.Erro)) return orcamento;

            _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Validar dados do cliente.");
            orcamento.Erro = ValidarClienteOrcamento(orcamento.ClienteOrcamentoCotacaoDto);
            if (!string.IsNullOrEmpty(orcamento.Erro)) return orcamento;

            _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Validar dados cadastrais do orçamento.");
            orcamento.Erro = ValidarAtualizacaoDadosCadastraisOrcamentoCotacao(orcamento, orcamentoAntigo, usuarioLogado);
            if (!string.IsNullOrEmpty(orcamento.Erro)) return orcamento;

            _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Abrindo transação.");
            var tOrcamentoAntigo = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro() { Id = (int)orcamento.Id }).FirstOrDefault();
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Buscando orçamento para atualizar.");
                    var tOrcamento = _orcamentoCotacaoBll.PorFiltroComTransacao(new TorcamentoCotacaoFiltro() { Id = (int)orcamento.Id }, dbGravacao).FirstOrDefault();
                    if (tOrcamento == null) orcamento.Erro = "Falha ao buscar Orçamento!";
                    if (!string.IsNullOrEmpty(orcamento.Erro)) return orcamento;

                    bool alterouEmail = false;

                    if (orcamento.ClienteOrcamentoCotacaoDto.Email != tOrcamento.Email)
                        alterouEmail = true;

                    tOrcamento.NomeCliente = orcamento.ClienteOrcamentoCotacaoDto.NomeCliente;
                    tOrcamento.NomeObra = orcamento.ClienteOrcamentoCotacaoDto.NomeObra;
                    tOrcamento.UF = orcamento.ClienteOrcamentoCotacaoDto.Uf;
                    tOrcamento.Email = orcamento.ClienteOrcamentoCotacaoDto.Email;
                    tOrcamento.Telefone = orcamento.ClienteOrcamentoCotacaoDto.Telefone;

                    if (tOrcamento.StEtgImediata != (orcamento.EntregaImediata ? (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM : (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO) ||
                        tOrcamento.PrevisaoEntregaData != orcamento.DataEntregaImediata)
                    {
                        tOrcamento.StEtgImediata = orcamento.EntregaImediata ? (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM : (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO;
                        if (tOrcamento.PrevisaoEntregaData != orcamento.DataEntregaImediata)
                        {
                            tOrcamento.PrevisaoEntregaData = orcamento.DataEntregaImediata?.Date;
                            tOrcamento.PrevisaoEntregaIdTipoUsuarioContexto = usuarioLogado.TipoUsuario;
                            tOrcamento.PrevisaoEntregaIdUsuarioUltAtualiz = usuarioLogado.Id;
                            tOrcamento.PrevisaoEntregaDtHrUltAtualiz = DateTime.Now;
                        }
                        tOrcamento.EtgImediataDtHrUltAtualiz = DateTime.Now;
                        tOrcamento.EtgImediataIdTipoUsuarioContexto = (short?)usuarioLogado.TipoUsuario;
                        tOrcamento.EtgImediataIdUsuarioUltAtualiz = usuarioLogado.Id;
                    }

                    tOrcamento.Observacao = orcamento.ObservacoesGerais;
                    tOrcamento.IdTipoUsuarioContextoUltAtualizacao = (int)usuarioLogado.TipoUsuario;
                    tOrcamento.IdUsuarioUltAtualizacao = usuarioLogado.Id;
                    tOrcamento.DataHoraUltAtualizacao = DateTime.Now;
                    tOrcamento.AceiteWhatsApp = orcamento.ConcordaWhatsapp;
                    tOrcamento.ContribuinteIcms = (byte)orcamento.ClienteOrcamentoCotacaoDto.ContribuinteICMS;

                    if (orcamento.InstaladorInstala != tOrcamento.InstaladorInstalaStatus)
                    {
                        tOrcamento.InstaladorInstalaStatus = orcamento.InstaladorInstala;
                        tOrcamento.InstaladorInstalaIdTipoUsuarioContexto = usuarioLogado.TipoUsuario;
                        tOrcamento.InstaladorInstalaIdUsuarioUltAtualiz = usuarioLogado.Id;
                        tOrcamento.InstaladorInstalaDtHrUltAtualiz = DateTime.Now;
                    }

                    _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Atualizando dados cadastrais.");
                    var retorno = _orcamentoCotacaoBll.AtualizarComTransacao(tOrcamento, dbGravacao);
                    if (retorno == null)
                    {
                        _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Falha ao atualizar dados cadastrais.");
                        orcamento.Erro = "Falha ao atualizar dados cadastrais!";
                        return orcamento;
                    }

                    string camposAOmitir = "|Id|Loja|ValidadeAnterior|QtdeRenovacao|IdUsuarioUltRenovacao|DataHoraUltRenovacao|Status|IdTipoUsuarioContextoUltStatus|IdUsuarioUltStatus|DataUltStatus|DataHoraUltStatus|IdOrcamento|IdPedido|IdTipoUsuarioContextoCadastro|IdUsuarioCadastro|DataCadastro|DataHoraCadastro|IdTipoUsuarioContextoUltAtualizacao|IdUsuarioUltAtualizacao|DataHoraUltAtualizacao|perc_max_comissao_padrao|perc_max_comissao_e_desconto_padrao|VersaoPoliticaCredito|VersaoPoliticaPrivacidade|InstaladorInstalaIdTipoUsuarioContexto|InstaladorInstalaIdUsuarioUltAtualiz|InstaladorInstalaDtHrUltAtualiz|GarantiaIndicadorIdTipoUsuarioContexto|GarantiaIndicadorIdUsuarioUltAtualiz|GarantiaIndicadorDtHrUltAtualiz|EtgImediataIdTipoUsuarioContexto|EtgImediataIdUsuarioUltAtualiz|EtgImediataDtHrUltAtualiz|PrevisaoEntregaIdTipoUsuarioContexto|PrevisaoEntregaIdUsuarioUltAtualiz|PrevisaoEntregaDtHrUltAtualiz|IdTipoUsuarioContextoUltRenovacao|";
                    string log = "";
                    log = UtilsGlobais.Util.MontalogComparacao(tOrcamento, tOrcamentoAntigo, log, camposAOmitir);
                    if (!string.IsNullOrEmpty(log)) log = $"id={tOrcamento.Id}; {log}";

                    var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = 2 }, dbGravacao).FirstOrDefault();
                    if (cfgOperacao == null)
                    {
                        orcamento.Erro = "Falha ao atualizar dados cadastrais!";
                        return orcamento;
                    }
                    var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogado.TipoUsuario, usuarioLogado.Id, tOrcamento.Loja, null, tOrcamento.Id, null,
                        Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);

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
                    _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Falha ao atualizar dados cadatrais do orçamento.");
                }
            }
            _logger.LogInformation($"Método Atualizar dados cadastrais de orçamento - Finalizando atualização de dados cadastrais.");
            return orcamento;
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

        public string ValidarAtualizacaoDadosCadastraisOrcamentoCotacao(OrcamentoResponseViewModel orcamento,
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
                            return "O contribuinte de ICMS não pode ser alterado!";

                        if (orcamento.ClienteOrcamentoCotacaoDto.Uf.ToUpper() !=
                            orcamentoAntigo.ClienteOrcamentoCotacaoDto.Uf.ToUpper())
                            return "A UF de entrega não ser alterada!";
                    }
                }
            }

            if (orcamento.Validade.Date != orcamentoAntigo.Validade.Date)
                return "A validade do orçamento não pode ser alterada!";

            if (orcamento.ClienteOrcamentoCotacaoDto.Tipo.ToUpper() !=
                orcamentoAntigo.ClienteOrcamentoCotacaoDto.Tipo.ToUpper())
                return "O tipo do cliente não pode ser alterado!";

            if (orcamento.IdVendedor != orcamentoAntigo.IdVendedor &&
                orcamento.Vendedor.ToUpper() != orcamentoAntigo.Vendedor.ToUpper())
                return "O vendedor não pode ser alterado!";

            if (orcamento.IdIndicadorVendedor != orcamentoAntigo.IdIndicadorVendedor &&
                orcamento.VendedorParceiro?.ToUpper() != orcamentoAntigo.VendedorParceiro?.ToUpper())
                return "O Vendedor do parceiro não pode ser alterado!";

            if (usuarioLogado.TipoUsuario == (int?)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
            {
                if (orcamento.Parceiro != Constantes.SEM_INDICADOR && (orcamento.IdIndicador != orcamentoAntigo.IdIndicador &&
                    orcamento.Parceiro.ToUpper() != orcamentoAntigo.IdIndicador.ToString().ToUpper()))
                    return "O parceiro não pode ser alterado!";
            }
            else
            {
                if (orcamento.Parceiro != Constantes.SEM_INDICADOR &&
                   (orcamento.Parceiro?.ToUpper() != orcamentoAntigo.Parceiro?.ToUpper() ||
                   orcamento.IdIndicador != orcamentoAntigo.IdIndicador))
                    return "O parceiro não pode ser alterado!";
            }

            if (orcamento.EntregaImediata)
            {
                var param = BuscarParametros(37, orcamento.Loja).FirstOrDefault();
                if (param == null) return "Falha ao buscar parâmentro para validação!";//aletrar retorno para mensagem

                var dataAtual = DateTime.Now.Date;
                if (orcamento.DataEntregaImediata?.Date < dataAtual) return "A Data de entrega não pode ser menor que a data atual!";
                if (orcamento.DataEntregaImediata?.Date > dataAtual.AddDays(int.Parse(param.Valor)))
                    return "A Data de entrega ultrapassa o valor máximo permitido!";
            }

            if (!string.IsNullOrEmpty(orcamento.Parceiro) && orcamento.Parceiro != Constantes.SEM_INDICADOR)
            {
                string msgInstalador = "É necessário informar um valor válido para instalador instala!";
                if (orcamento.InstaladorInstala != (int)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO &&
                    orcamento.InstaladorInstala != (int)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM)
                    return msgInstalador;
                if (orcamento.InstaladorInstala == (int)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO_DEFINIDO)
                    return msgInstalador;
            }

            return null;
        }

        public string AdicionarOrcamentoCotacaoLink(TorcamentoCotacao orcamento, Guid guid, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            var orcamentoCotacaoLinkModel = new TorcamentoCotacaoLink();
            orcamentoCotacaoLinkModel.IdOrcamentoCotacao = orcamento.Id;
            orcamentoCotacaoLinkModel.Guid = guid;
            orcamentoCotacaoLinkModel.Status = 1;
            orcamentoCotacaoLinkModel.IdTipoUsuarioContextoUltStatus = 1;
            orcamentoCotacaoLinkModel.IdUsuarioUltStatus = orcamento.IdUsuarioCadastro;

            orcamentoCotacaoLinkModel.DataUltStatus = DateTime.Now.Date;
            orcamentoCotacaoLinkModel.DataHoraUltStatus = DateTime.Now;

            orcamentoCotacaoLinkModel.IdTipoUsuarioContextoCadastro = (short)orcamento.IdTipoUsuarioContextoCadastro;
            orcamentoCotacaoLinkModel.IdUsuarioCadastro = orcamento.IdUsuarioCadastro;

            orcamentoCotacaoLinkModel.DataCadastro = DateTime.Now.Date;
            orcamentoCotacaoLinkModel.DataHoraCadastro = DateTime.Now;

            if (!_orcamentoCotacaoLinkBll.InserirOrcamentoCotacaoLink(orcamentoCotacaoLinkModel, contextoBdGravacao))
                return "Orçamento não cadastrado. Problemas ao gravar o Link!";

            return null;
        }

        public void AtualizarOrcamentoCotacaoLink(
            OrcamentoResponseViewModel orcamento,
            ContextoBdGravacao contextoBdGravacao)
        {
            var orcamentoCotacaoLinkModel = new TorcamentoCotacaoLink()
            {
                IdOrcamentoCotacao = unchecked((int)orcamento.Id),
                Status = 2,
                DataUltStatus = DateTime.Now.Date,
                DataHoraUltStatus = DateTime.Now
            };

            try
            {
                _orcamentoCotacaoLinkBll.AtualizarComTransacao(orcamentoCotacaoLinkModel, contextoBdGravacao);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Orçamento não reenviado. Problemas ao gravar o Link!");
            }
        }

        private string AdicionarOrcamentoCotacaoEmailQueue(CadastroOrcamentoRequest orcamento, Guid guid, int idOrcamentoCotacao,
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
                        idOrcamentoCotacao.ToString(),
                        urlBaseFront,
                        logoEmpresa
                    };

            var torcamentoCotacaoEmailQueue = _orcamentoCotacaoEmailQueueBll.InserirQueueComTemplateEHTMLComTransacao(Int32.Parse(template),
                orcamentoCotacaoEmailQueueModel, tagHtml, contextoBdGravacao);

            if (torcamentoCotacaoEmailQueue.Id == 0)
                return "Não foi possível cadastrar o orçamento. Problema no envio de e-mail!";
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
                    return "Não foi possível cadastrar o orçamento. Problema no envio de e-mail!";
                }

            }

            return null;
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

        private TorcamentoCotacao MontarTorcamentoCotacao(CadastroOrcamentoRequest orcamento, UsuarioLogin usuarioLogado,
            PercMaxDescEComissaoDados percMaxDescEComissaoDados, int status)
        {
            TorcamentoCotacao torcamentoCotacao = new TorcamentoCotacao();
            torcamentoCotacao.Loja = orcamento.Loja;
            torcamentoCotacao.NomeCliente = orcamento.ClienteOrcamentoCotacaoDto.NomeCliente;
            torcamentoCotacao.NomeObra = orcamento.ClienteOrcamentoCotacaoDto.NomeObra;
            torcamentoCotacao.Email = orcamento.ClienteOrcamentoCotacaoDto.Email;
            torcamentoCotacao.Telefone = orcamento.ClienteOrcamentoCotacaoDto.Telefone;
            torcamentoCotacao.UF = orcamento.ClienteOrcamentoCotacaoDto.Uf;
            torcamentoCotacao.TipoCliente = orcamento.ClienteOrcamentoCotacaoDto.Tipo;
            torcamentoCotacao.Validade = orcamento.Validade;
            torcamentoCotacao.Observacao = orcamento.ObservacoesGerais;
            torcamentoCotacao.AceiteWhatsApp = orcamento.ConcordaWhatsapp;
            torcamentoCotacao.IdTipoUsuarioContextoCadastro = (int)usuarioLogado.TipoUsuario;
            torcamentoCotacao.IdTipoUsuarioContextoUltStatus = (int)usuarioLogado.TipoUsuario;
            torcamentoCotacao.IdUsuarioCadastro = usuarioLogado.Id;

            torcamentoCotacao.DataCadastro = DateTime.Now.Date;
            torcamentoCotacao.DataHoraCadastro = DateTime.Now;
            torcamentoCotacao.DataUltStatus = DateTime.Now.Date;
            torcamentoCotacao.DataHoraUltStatus = DateTime.Now;

            torcamentoCotacao.Status = (short)status;
            torcamentoCotacao.IdUsuarioUltStatus = usuarioLogado.Id;
            torcamentoCotacao.StEtgImediata = orcamento.EntregaImediata ? (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM : (int)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO;
            torcamentoCotacao.PrevisaoEntregaData = orcamento.DataEntregaImediata?.Date;
            torcamentoCotacao.Perc_max_comissao_e_desconto_padrao = orcamento.ClienteOrcamentoCotacaoDto.Tipo == Constantes.ID_PF ?
                percMaxDescEComissaoDados.PercMaxComissaoEDesconto : percMaxDescEComissaoDados.PercMaxComissaoEDescontoPJ;
            torcamentoCotacao.ContribuinteIcms = orcamento.ClienteOrcamentoCotacaoDto.ContribuinteICMS.HasValue ?
                (byte)orcamento.ClienteOrcamentoCotacaoDto.ContribuinteICMS : (byte)0;
            torcamentoCotacao.InstaladorInstalaStatus = orcamento.InstaladorInstala;
            if (orcamento.InstaladorInstala != (int)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO_DEFINIDO)
            {
                torcamentoCotacao.InstaladorInstalaIdTipoUsuarioContexto = usuarioLogado.TipoUsuario;
                torcamentoCotacao.InstaladorInstalaIdUsuarioUltAtualiz = usuarioLogado.Id;
                torcamentoCotacao.InstaladorInstalaDtHrUltAtualiz = DateTime.Now;
            }
            torcamentoCotacao.GarantiaIndicadorStatus = 0;
            torcamentoCotacao.GarantiaIndicadorIdTipoUsuarioContexto = usuarioLogado.TipoUsuario;
            torcamentoCotacao.GarantiaIndicadorIdUsuarioUltAtualiz = usuarioLogado.Id;
            torcamentoCotacao.GarantiaIndicadorDtHrUltAtualiz = DateTime.Now;
            torcamentoCotacao.EtgImediataDtHrUltAtualiz = DateTime.Now;
            torcamentoCotacao.EtgImediataIdTipoUsuarioContexto = (short?)usuarioLogado.TipoUsuario;
            torcamentoCotacao.EtgImediataIdUsuarioUltAtualiz = usuarioLogado.Id;
            torcamentoCotacao.PrevisaoEntregaIdTipoUsuarioContexto = usuarioLogado.TipoUsuario;
            torcamentoCotacao.PrevisaoEntregaIdUsuarioUltAtualiz = usuarioLogado.Id;
            torcamentoCotacao.PrevisaoEntregaDtHrUltAtualiz = DateTime.Now;

            if (!string.IsNullOrEmpty(orcamento.Vendedor))
            {
                var vendedor = _usuarioBll.PorFiltro(new TusuarioFiltro() { usuario = orcamento.Vendedor }).FirstOrDefault();

                if (vendedor == null) return null;

                torcamentoCotacao.IdVendedor = vendedor.Id;
            }
            if (!string.IsNullOrEmpty(orcamento.Parceiro) && orcamento.Parceiro != Constantes.SEM_INDICADOR)
            {
                if (usuarioLogado.TipoUsuario != (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var torcamentista = _orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { apelido = orcamento.Parceiro });
                    if (torcamentista == null) return null;


                    torcamentoCotacao.IdIndicador = torcamentista.IdIndicador;
                }

                if (usuarioLogado.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var torcamentista = _orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { idParceiro = int.Parse(orcamento.Parceiro) });
                    if (torcamentista == null) return null;

                    torcamentoCotacao.IdIndicador = torcamentista.IdIndicador;
                }

                torcamentoCotacao.Perc_max_comissao_padrao = percMaxDescEComissaoDados.PercMaxComissao;
            }

            if (!string.IsNullOrEmpty(orcamento.VendedorParceiro))
            {
                if (usuarioLogado.TipoUsuario != (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var vendedoresParceiro = _orcamentistaEIndicadorVendedorBll.BuscarVendedoresParceiro(orcamento.Parceiro);
                    if (vendedoresParceiro == null) return null;

                    torcamentoCotacao.IdIndicadorVendedor = vendedoresParceiro
                        .Where(x => x.Nome == orcamento.VendedorParceiro)
                        .FirstOrDefault().Id;
                }

                if (usuarioLogado.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var vendedoresParceiro = _orcamentistaEIndicadorVendedorBll.BuscarVendedoresParceiroPorId(int.Parse(orcamento.Parceiro));
                    if (vendedoresParceiro == null) return null;

                    torcamentoCotacao.IdIndicadorVendedor = vendedoresParceiro
                        .Where(x => x.Email.ToUpper() == orcamento.VendedorParceiro)
                        .FirstOrDefault().Id;
                }

            }

            return torcamentoCotacao;
        }

        public MensagemDto ProrrogarOrcamento(int id, int idUsuario, string lojaLogada,
            int? IdTipoUsuarioContextoUltAtualizacao, string ip)
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


                var validadeAtual = orcamento.Validade;
                var validadeMaximaGlobal = orcamento.DataCadastro.AddDays(int.Parse(parametros.QtdeGlobal_Validade));
                if(orcamento.Validade.Date == validadeMaximaGlobal.Date)
                {
                    return new MensagemDto
                    {
                        tipo = "WARN",
                        mensagem = $"Não é possível prorrogar o orçamento. Validade máxima permitida de {parametros.QtdeGlobal_Validade} dias."
                    };
                }

                var validadeFutura = validadeAtual.AddDays(int.Parse(parametros.QtdePadrao_DiasProrrogacao));
                if(validadeFutura.Date > validadeMaximaGlobal.Date)
                {
                    validadeFutura = validadeMaximaGlobal;
                }

                orcamento.ValidadeAnterior = validadeAtual;
                orcamento.QtdeRenovacao += 1;
                orcamento.IdUsuarioUltRenovacao = idUsuario;
                orcamento.DataHoraUltRenovacao = DateTime.Now;
                orcamento.DataHoraUltAtualizacao = DateTime.Now;
                orcamento.IdUsuarioUltAtualizacao = idUsuario;
                orcamento.IdTipoUsuarioContextoUltAtualizacao = IdTipoUsuarioContextoUltAtualizacao.Value;
                orcamento.IdTipoUsuarioContextoUltRenovacao = IdTipoUsuarioContextoUltAtualizacao.Value;
                orcamento.Validade = validadeFutura;

                _orcamentoCotacaoBll.Atualizar(orcamento);

                string log = $"Validade:{orcamento.Validade.ToString("dd/MM/yyyy")} => ValidadeAnterior: {orcamento.ValidadeAnterior?.ToString("dd/MM/yyyy")}";

                var cfgOperacao = _cfgOperacaoBll.PorFiltro(new TcfgOperacaoFiltro() { Id = 4 }).FirstOrDefault();
                if (cfgOperacao == null)
                {
                    return new MensagemDto
                    {
                        tipo = "WARN",
                        mensagem = $"Falha ao montar log de operação."
                    };
                }

                var tLogV2 = UtilsGlobais.Util.GravaLogV2(_contextoBdProvider, log, (short)IdTipoUsuarioContextoUltAtualizacao.Value, idUsuario, orcamento.Loja, null, orcamento.Id, null,
                    Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);


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


        public MensagemDto AtualizarStatus(int id, UsuarioLogin user, short idStatus, string ip)
        {
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                try
                {
                    var tOrcamento = _orcamentoCotacaoBll.PorFiltroComTransacao(new TorcamentoCotacaoFiltro() { Id = id }, dbGravacao).FirstOrDefault();
                    if (tOrcamento == null) throw new Exception("Falha ao buscar Orçamento!");

                    if (idStatus == (short)Constantes.eCfgOrcamentoCotacaoStatus.CANCELADO)
                    {

                        if (tOrcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.APROVADO)
                            return new MensagemDto
                            {
                                tipo = "WARN",
                                mensagem = "Não é possível cancelar orçamentos aprovados!"
                            };
                    }

                    if (idStatus == (short)Constantes.eCfgOrcamentoCotacaoStatus.EXCLUIDO)
                    {

                        if (tOrcamento.Status == (short)Constantes.eCfgOrcamentoCotacaoStatus.APROVADO)
                            return new MensagemDto
                            {
                                tipo = "WARN",
                                mensagem = "Não é possível excluir orçamentos aprovados!"
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

                    if (idStatus == 2)
                    {
                        var cfgOperacao = _cfgOperacaoBll.PorFiltro(new TcfgOperacaoFiltro() { Id = 6 }).FirstOrDefault();
                        if (cfgOperacao == null)
                        {
                            return new MensagemDto
                            {
                                tipo = "WARN",
                                mensagem = $"Falha ao montar log de operação."
                            };
                        }

                        var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, "", (short)user.TipoUsuario, user.Id, tOrcamento.Loja, null, tOrcamento.Id, null,
                            Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);
                    }

                    if (idStatus == (short)Constantes.eCfgOrcamentoCotacaoStatus.EXCLUIDO)
                    {
                        var cfgOperacao = _cfgOperacaoBll.PorFiltro(new TcfgOperacaoFiltro() { Id = (int)Constantes.eCfgLogOperacao.ORCAMENTO_COTACAO_EXCLUSAO }).FirstOrDefault();
                        if (cfgOperacao == null)
                        {
                            return new MensagemDto
                            {
                                tipo = "WARN",
                                mensagem = $"Falha ao montar log de operação."
                            };
                        }
                        var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, "", (short)user.TipoUsuario, user.Id, tOrcamento.Loja, null, tOrcamento.Id, null,
                            Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);
                    }

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

        public async Task<List<string>> AprovarOrcamento(
            AprovarOrcamentoRequestViewModel aprovarOrcamento,
            Constantes.TipoUsuarioContexto tipoUsuarioContexto,
            int idUsuarioUltAtualizacao,
            string ip)
        {
            if (aprovarOrcamento == null)
            {
                return new List<string>() { "É necessário preencher o cadastro do cliente!" };
            }

            var tCliente = await _clienteBll
                .BuscarTcliente(UtilsGlobais.Util.SoDigitosCpf_Cnpj(aprovarOrcamento.ClienteCadastroDto.DadosCliente.Cnpj_Cpf));

            var orcamento = _orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro()
            {
                Id = aprovarOrcamento.IdOrcamento
            }).FirstOrDefault();

            if (orcamento == null)
            {
                return new List<string>() { "Falha ao buscar Orçamento!" };
            }

            if (orcamento.Status != (short)Constantes.eCfgOrcamentoCotacaoStatus.ENVIADO)
            {
                return new List<string>() { "Este orçamento está indisponível para aprovação." };
            }
            if (orcamento.Validade.Date < DateTime.Now.Date)
            {
                return new List<string>() { "Este orçamento está indisponível para aprovação." };
            }

            aprovarOrcamento.ClienteCadastroDto.DadosCliente.Perc_max_comissao_e_desconto_padrao = orcamento.Perc_max_comissao_e_desconto_padrao;
            aprovarOrcamento.ClienteCadastroDto.DadosCliente.Perc_max_comissao_padrao = orcamento.Perc_max_comissao_padrao;
            aprovarOrcamento.ClienteCadastroDto.DadosCliente.IdIndicadorVendedor = orcamento.IdIndicadorVendedor;
            aprovarOrcamento.ClienteCadastroDto.DadosCliente.IdOrcamentoCotacao = aprovarOrcamento.IdOrcamento;

            //endereço entrega
            aprovarOrcamento.enderecoEntrega.Etg_imediata_data = orcamento.EtgImediataDtHrUltAtualiz;
            aprovarOrcamento.enderecoEntrega.EtgImediataIdTipoUsuarioContexto = orcamento.EtgImediataIdTipoUsuarioContexto;
            aprovarOrcamento.enderecoEntrega.EtgImediataIdUsuarioUltAtualiz = orcamento.EtgImediataIdUsuarioUltAtualiz;
            aprovarOrcamento.enderecoEntrega.Etg_Imediata_Usuario = $"[{orcamento.EtgImediataIdTipoUsuarioContexto}] {orcamento.EtgImediataIdUsuarioUltAtualiz}";

            //previsão entrega


            var clienteCadastroDados = ClienteCadastroDto
                .ClienteCadastroDados_De_ClienteCadastroDto(aprovarOrcamento.ClienteCadastroDto);

            _logger.LogInformation("Validando cadastro de cliente!");
            var erros = await _clienteBll.ValidarClienteOrcamentoCotacao(clienteCadastroDados);

            if (erros != null)
            {
                return erros;
            }

            _logger.LogInformation("Fim da validação do cadastro de cliente!");

            var retorno = new List<string>();

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
                    retorno = await CadastrarPrepedido(aprovarOrcamento, orcamento, dbGravacao, tipoUsuarioContexto,
                        idUsuarioUltAtualizacao, ip, true);
                    //precisamos mudar isso, precisamos verificar se existe um número de orçamento válido ou adicionar alguma prop na classe
                    if (retorno.Count >= 1)
                    {
                        if (!retorno[0].Contains(Constantes.SUFIXO_ID_ORCAMENTO))
                            return retorno;
                    }

                    var tcfgStatus = _orcamentoCotacaoBll.BuscarStatusParaOrcamentoCotacaoComtransacao("APROVADO", dbGravacao);
                    orcamento.IdOrcamento = retorno[0];
                    orcamento.DataHoraUltAtualizacao = DateTime.Now;
                    orcamento.Status = tcfgStatus.Id;
                    orcamento.DataHoraUltStatus = DateTime.Now;
                    orcamento.IdUsuarioUltStatus = idUsuarioUltAtualizacao == (int)Constantes.TipoUsuarioContexto.Cliente ? null : (int?)idUsuarioUltAtualizacao;
                    orcamento.IdTipoUsuarioContextoUltStatus = (int)tipoUsuarioContexto;
                    orcamento.DataUltStatus = DateTime.Now.Date;
                    orcamento.IdTipoUsuarioContextoUltAtualizacao = (int?)tipoUsuarioContexto;
                    orcamento.IdUsuarioUltAtualizacao = idUsuarioUltAtualizacao == (int)Constantes.TipoUsuarioContexto.Cliente ? null : (int?)idUsuarioUltAtualizacao;
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
                    opcaoSelecionada.IdTipoUsuarioContextoUltAtualizacao = (int?)tipoUsuarioContexto;
                    opcaoSelecionada.IdUsuarioUltAtualizacao = idUsuarioUltAtualizacao == (int)Constantes.TipoUsuarioContexto.Cliente ? null : (int?)idUsuarioUltAtualizacao;
                    opcaoSelecionada.DataHoraUltAtualizacao = DateTime.Now;
                    var objOpcao = _orcamentoCotacaoOpcaoBll.AtualizarOpcaoComTransacao(opcaoSelecionada, dbGravacao);
                    if (objOpcao == null)
                        return new List<string>() { "Falha ao atualizar opção selecionada para aprovação do orçamento!" };

                    //atualiza forma pagto
                    var formaPagtoSelecionada = _formaPagtoOrcamentoCotacaoBll
                        .PorFiltroComTransacao(new TorcamentoCotacaoOpcaoPagtoFiltro() { Id = aprovarOrcamento.IdFormaPagto }, dbGravacao).FirstOrDefault();

                    if (formaPagtoSelecionada == null)
                        return new List<string>() { "Falha ao buscar forma de pagamento da opção selecionada para aprovação do orçamento!" };

                    formaPagtoSelecionada.IdTipoUsuarioContextoAprovado = idUsuarioUltAtualizacao == (int)Constantes.TipoUsuarioContexto.Cliente ? (short?)idUsuarioUltAtualizacao :
                       (short)tipoUsuarioContexto;
                    formaPagtoSelecionada.IdUsuarioAprovado = idUsuarioUltAtualizacao == (int)Constantes.TipoUsuarioContexto.Cliente ? null : (int?)idUsuarioUltAtualizacao;
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
                    ex.Data.Add("params", new { aprovarOrcamento, tipoUsuarioContexto, idUsuarioUltAtualizacao });
                    dbGravacao.transacao.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<List<string>> CadastrarPrepedido(
            AprovarOrcamentoRequestViewModel aprovarOrcamento,
            TorcamentoCotacao orcamento,
            ContextoBdGravacao dbGravacao,
            Constantes.TipoUsuarioContexto tipoUsuarioContexto,
            int idUsuarioUltAtualizacao,
            string ip,
            bool aprovandoOrcamentoCotacao)
        {
            _logger.LogInformation("Iniciando criação de Pré-Pedido.");

            // criar prepedidoDto
            var prepedido = new PrePedidoDto();
            prepedido.UsuarioCadastroId = tipoUsuarioContexto == Constantes.TipoUsuarioContexto.Cliente ? null : (int?)idUsuarioUltAtualizacao;
            prepedido.Usuario_cadastro =
                tipoUsuarioContexto == Constantes.TipoUsuarioContexto.Cliente ? $"[{idUsuarioUltAtualizacao}] {tipoUsuarioContexto}" :
                $"[{(int)tipoUsuarioContexto}] {idUsuarioUltAtualizacao}";
            prepedido.UsuarioCadastroIdTipoUsuarioContexto = (short?)tipoUsuarioContexto;
            prepedido.DadosCliente = new DadosClienteCadastroDto();
            prepedido.DadosCliente = aprovarOrcamento.ClienteCadastroDto.DadosCliente;
            prepedido.EnderecoCadastroClientePrepedido = new EnderecoCadastralClientePrepedidoDto();
            prepedido.EnderecoCadastroClientePrepedido = EnderecoCadastralClientePrepedidoDto
                .EnderecoCadastralClientePrepedidoDto_De_DadosClienteCadastroDto(aprovarOrcamento.ClienteCadastroDto.DadosCliente);
            prepedido.EnderecoEntrega = aprovarOrcamento.enderecoEntrega;


            var opcaoSelecionada = _orcamentoCotacaoOpcaoBll
                .PorFiltro(new TorcamentoCotacaoOpcaoFiltro() { Id = aprovarOrcamento.IdOpcao, IdFormaPagto = aprovarOrcamento.IdFormaPagto }, false).FirstOrDefault();

            if (opcaoSelecionada == null)
            {
                return new List<string>() { "Falha ao buscar opção selecionada para aprovação do orçamento!" };
            }

            var formaPagtoSelecionada = opcaoSelecionada.FormaPagto.Where(x => x.Id == aprovarOrcamento.IdFormaPagto).FirstOrDefault();

            if (formaPagtoSelecionada == null)
            {
                return new List<string>() { "Falha ao buscar forma de pagamento selecionada da opção!" };
            }

            prepedido.FormaPagtoCriacao = new FormaPagtoCriacaoDto();
            prepedido.FormaPagtoCriacao = await IncluirFormaPagtoCriacaoParaPrepedido(formaPagtoSelecionada);

            prepedido.ListaProdutos = await IncluirProdutosParaPrepedido(opcaoSelecionada.ListaProdutos, formaPagtoSelecionada.Id, (float)orcamento.Perc_max_comissao_e_desconto_padrao);

            if (prepedido.ListaProdutos == null)
            {
                new List<string>() { "Falha ao buscar produtos atômicos da opção!" };
            }

            prepedido.VlTotalDestePedido = Math.Round((decimal)prepedido.ListaProdutos.Sum(x => x.Preco_Venda * x.Qtde), 2);
            if (opcaoSelecionada.VlTotal != prepedido.VlTotalDestePedido)
            {
                decimal valorTotalDif = opcaoSelecionada.VlTotal - (decimal)prepedido.VlTotalDestePedido;
                prepedido.ListaProdutos = DistribuirDiferencaValores(prepedido.ListaProdutos, valorTotalDif);
                prepedido.VlTotalDestePedido = Math.Round((decimal)prepedido.ListaProdutos.Sum(x => x.Preco_Venda * x.Qtde), 2);
            }

            prepedido.PercRT = opcaoSelecionada.PercRT;

            prepedido.DadosCliente.IdOrcamentoCotacao = orcamento.Id;
            prepedido.DadosCliente.Perc_max_comissao_padrao = orcamento.Perc_max_comissao_padrao;
            prepedido.DadosCliente.Perc_max_comissao_e_desconto_padrao = orcamento.Perc_max_comissao_e_desconto_padrao;
            prepedido.DadosCliente.Vendedor = aprovarOrcamento.ClienteCadastroDto.DadosCliente.Vendedor;

            prepedido.EnderecoEntrega.EtgImediataIdTipoUsuarioContexto = orcamento.EtgImediataIdTipoUsuarioContexto;
            prepedido.EnderecoEntrega.EtgImediataIdUsuarioUltAtualiz = orcamento.EtgImediataIdUsuarioUltAtualiz;
            prepedido.EnderecoEntrega.Etg_Imediata_Usuario = aprovarOrcamento.enderecoEntrega.Etg_Imediata_Usuario;

            prepedido.DetalhesPrepedido = new DetalhesDtoPrepedido();
            prepedido.DetalhesPrepedido.EntregaImediata = orcamento.StEtgImediata.ToString();
            prepedido.DetalhesPrepedido.EntregaImediataData = orcamento.EtgImediataDtHrUltAtualiz.HasValue ? orcamento.EtgImediataDtHrUltAtualiz : null;
            prepedido.DetalhesPrepedido.PrevisaoEntregaData = orcamento.PrevisaoEntregaData;
            prepedido.DetalhesPrepedido.PrevisaoEntregaIdTipoUsuarioContexto = (short?)orcamento.PrevisaoEntregaIdTipoUsuarioContexto;
            prepedido.DetalhesPrepedido.PrevisaoEntregaIdUsuarioUltAtualiz = orcamento.PrevisaoEntregaIdUsuarioUltAtualiz;
            prepedido.DetalhesPrepedido.PrevisaoEntregaDtHrUltAtualiz = orcamento.PrevisaoEntregaDtHrUltAtualiz;
            prepedido.DetalhesPrepedido.PrevisaoEntregaUsuarioUltAtualiz = $"[{orcamento.PrevisaoEntregaIdTipoUsuarioContexto}] {orcamento.PrevisaoEntregaIdUsuarioUltAtualiz}";

            prepedido.DetalhesPrepedido.BemDeUso_Consumo = Convert.ToString((byte)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM);

            prepedido.DetalhesPrepedido.InstaladorInstala = orcamento.InstaladorInstalaStatus.ToString();
            prepedido.DetalhesPrepedido.InstaladorInstalaIdTipoUsuarioContexto = (short?)orcamento.InstaladorInstalaIdTipoUsuarioContexto;
            prepedido.DetalhesPrepedido.InstaladorInstalaIdUsuarioUltAtualiz = orcamento.InstaladorInstalaIdUsuarioUltAtualiz;
            prepedido.DetalhesPrepedido.InstaladorInstalaUsuarioUltAtualiz = $"[{orcamento.InstaladorInstalaIdTipoUsuarioContexto}] {orcamento.InstaladorInstalaIdUsuarioUltAtualiz}";
            prepedido.DetalhesPrepedido.InstaladorInstalaDtHrUltAtualiz = orcamento.InstaladorInstalaDtHrUltAtualiz;

            prepedido.DetalhesPrepedido.GarantiaIndicador = orcamento.GarantiaIndicadorStatus.ToString();
            prepedido.DetalhesPrepedido.GarantiaIndicadorIdTipoUsuarioContexto = (short?)orcamento.GarantiaIndicadorIdTipoUsuarioContexto;
            prepedido.DetalhesPrepedido.GarantiaIndicadorIdUsuarioUltAtualiz = orcamento.GarantiaIndicadorIdUsuarioUltAtualiz;
            prepedido.DetalhesPrepedido.GarantiaIndicadorUsuarioUltAtualiz = $"[{orcamento.GarantiaIndicadorIdTipoUsuarioContexto}] {orcamento.GarantiaIndicadorIdUsuarioUltAtualiz}";
            prepedido.DetalhesPrepedido.GarantiaIndicadorDtHrUltAtualiz = orcamento.GarantiaIndicadorDtHrUltAtualiz;
            prepedido.DetalhesPrepedido.Observacoes = orcamento.Observacao;

            string parceiro = null;
            if (!string.IsNullOrEmpty(prepedido.DadosCliente.Indicador_Orcamentista))
            {
                parceiro = prepedido.DadosCliente.Indicador_Orcamentista;
            }

            var appSettingsSection = _configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Configuracao>();

            //TODO: "appSettings.LimiteItens" deve vir de tabela de parametrização?
            PrePedidoDados prePedidoDados = PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prepedido);
            return (await _prepedidoBll
                .CadastrarPrepedido(prePedidoDados,
                parceiro,
                0.01M,
                false,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO,
                appSettings.LimiteItens,
                dbGravacao,
                ip, aprovandoOrcamentoCotacao)).ToList();
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

            List<PrepedidoProdutoDtoPrepedido> response = new List<PrepedidoProdutoDtoPrepedido>();

            List<PrepedidoProdutoDtoPrepedido> produtosDto = new List<PrepedidoProdutoDtoPrepedido>();

            foreach (var produto in produtosOpcaoSelecionada)
            {
                var itens = itensAtomicosFinOpcao.Where(x => x.TorcamentoCotacaoOpcaoItemAtomico.IdItemUnificado == produto.IdItemUnificado);
                foreach (var item in itens)
                {
                    PrepedidoProdutoDtoPrepedido produtoDto = new PrepedidoProdutoDtoPrepedido();
                    produtoDto.Fabricante = item.TorcamentoCotacaoOpcaoItemAtomico.Fabricante;
                    produtoDto.Produto = item.TorcamentoCotacaoOpcaoItemAtomico.Produto;
                    produtoDto.Descricao = item.TorcamentoCotacaoOpcaoItemAtomico.DescricaoHtml;
                    produtoDto.CustoFinancFornecPrecoListaBase = item.CustoFinancFornecPrecoListaBase;
                    produtoDto.CustoFinancFornecCoeficiente = item.CustoFinancFornecCoeficiente;
                    produtoDto.Preco_Lista = item.PrecoLista;
                    produtoDto.Preco_Venda = item.PrecoVenda;
                    produtoDto.Qtde = (short?)(item.TorcamentoCotacaoOpcaoItemAtomico.Qtde * produto.Qtde);
                    produtoDto.Desc_Dado = produto.DescDado;
                    produtosDto.Add(produtoDto);
                }
            }

            var produtosDtoDistinct = produtosDto.Select(x => x.Produto).Distinct();

            foreach (var produto in produtosDtoDistinct)
            {

                var itens = produtosDto.Where(x => x.Produto == produto);
                var produtoPrepedido = CalcularProdutoParaPrepedido(itens);

                produtoPrepedido.Fabricante = itens.First().Fabricante;
                produtoPrepedido.Produto = itens.First().Produto;
                produtoPrepedido.Descricao = itens.First().Descricao;
                produtoPrepedido.Obs = "";
                produtoPrepedido.BlnTemRa = false;
                produtoPrepedido.Qtde_estoque_total_disponivel = 0;

                if (Perc_max_comissao_e_desconto_padrao > 0)
                {
                    var produtosComAlcada = itensAtomicosFinOpcao.Where(x => x.TorcamentoCotacaoOpcaoItemAtomico.Produto == produto && x.StatusDescontoSuperior);
                    if (produtosComAlcada.Any())
                    {
                        var maiorDesconto = produtosComAlcada.Max(x => x.DescDado);
                        if (produtoPrepedido.Desc_Dado > Perc_max_comissao_e_desconto_padrao)
                        {
                            var idMaiorAlcada = produtosComAlcada.Max(x => x.IdOperacaoAlcadaDescontoSuperior);
                            var itemComMaiorAlcada = produtosComAlcada.Where(x => x.IdOperacaoAlcadaDescontoSuperior == idMaiorAlcada).FirstOrDefault();
                            produtoPrepedido.StatusDescontoSuperior = true;
                            produtoPrepedido.IdUsuarioDescontoSuperior = itemComMaiorAlcada.IdUsuarioDescontoSuperior;
                            produtoPrepedido.DataHoraDescontoSuperior = itemComMaiorAlcada.DataHoraDescontoSuperior;
                        }
                    }
                    else
                    {
                        produtoPrepedido.StatusDescontoSuperior = false;
                        produtoPrepedido.IdUsuarioDescontoSuperior = null;
                        produtoPrepedido.DataHoraDescontoSuperior = null;
                    }

                }

                response.Add(produtoPrepedido);
            }

            return response;
        }

        private PrepedidoProdutoDtoPrepedido CalcularProdutoParaPrepedido(IEnumerable<PrepedidoProdutoDtoPrepedido> itens)
        {
            PrepedidoProdutoDtoPrepedido response = new PrepedidoProdutoDtoPrepedido();

            var somaQtde = itens.Sum(x => x.Qtde);
            decimal precoVendaTotal = Math.Round((decimal)itens.Sum(x => x.Preco_Venda * x.Qtde), 4, MidpointRounding.AwayFromZero);
            decimal precoListaTotal = Math.Round((decimal)itens.Sum(x => x.Preco_Lista * x.Qtde), 2, MidpointRounding.AwayFromZero);
            decimal precoLista = Math.Round(itens.Select(x => x.Preco_Lista).First(), 2, MidpointRounding.AwayFromZero);
            decimal precoListaMedio = Math.Round(precoListaTotal / (int)somaQtde, 2, MidpointRounding.AwayFromZero);
            decimal precoVendaMedio = Math.Round(precoVendaTotal / (int)somaQtde, 2, MidpointRounding.AwayFromZero);
            var descontoMedio = 100 * (precoListaMedio - precoVendaMedio) / precoListaMedio;

            response.CustoFinancFornecPrecoListaBase = itens.First().CustoFinancFornecPrecoListaBase;
            response.CustoFinancFornecCoeficiente = itens.First().CustoFinancFornecCoeficiente;
            response.Preco_Lista = precoListaMedio;
            response.Qtde = (short?)somaQtde;
            response.Desc_Dado = (float)descontoMedio;
            response.Preco_Venda = precoVendaMedio;
            response.Preco_NF = precoVendaMedio;
            response.VlTotalItem = Math.Round(precoVendaTotal, 2);
            response.TotalItem = Math.Round(precoVendaTotal, 2);

            return response;
        }

        private List<PrepedidoProdutoDtoPrepedido> DistribuirDiferencaValores(List<PrepedidoProdutoDtoPrepedido> produtos, decimal valorTotalDif)
        {
            bool ajustou = false;
            var sinalAjuste = 0;
            if (valorTotalDif > 0)
            {
                sinalAjuste = 1;
            }
            else
            {
                sinalAjuste = -1;
            }

            foreach (var item in produtos)
            {
                if ((Convert.ToInt32(Math.Abs(valorTotalDif) * 100) % item.Qtde) == 0)
                {
                    item.Preco_Venda = Math.Round(item.Preco_Venda + (sinalAjuste * (Math.Abs(valorTotalDif) / (int)item.Qtde)), 2, MidpointRounding.AwayFromZero);
                    item.Preco_NF = item.Preco_Venda;
                    item.Desc_Dado = (float)(100 * (item.Preco_Lista - item.Preco_Venda) / item.Preco_Lista);
                    ajustou = true;
                    break;
                }
            }

            if (!ajustou)
            {
                decimal menorPrecoVendaDif = Math.Abs(valorTotalDif);
                int indice = -1;
                for (int i = 0; i < produtos.Count; i++)
                {
                    decimal precoVendaAux = Math.Round(produtos[i].Preco_Venda +
                        (sinalAjuste * (Math.Abs(valorTotalDif) / (int)produtos[i].Qtde)), 2, MidpointRounding.AwayFromZero);
                    decimal totalPrecoVenda = (decimal)produtos[i].VlTotalItem;
                    decimal novoTotalPrecoVenda = precoVendaAux * (short)produtos[i].Qtde;
                    decimal menorPrecoVendaDifAux = Math.Abs(totalPrecoVenda - novoTotalPrecoVenda);
                    if (menorPrecoVendaDifAux < menorPrecoVendaDif)
                    {
                        menorPrecoVendaDif = menorPrecoVendaDifAux;
                        indice = i;
                    }
                }

                if (indice > -1)
                {
                    produtos[indice].Preco_Venda = Math.Round(produtos[indice].Preco_Venda + (sinalAjuste * Math.Abs(menorPrecoVendaDif) / (short)produtos[indice].Qtde), 2, MidpointRounding.AwayFromZero);
                    produtos[indice].Preco_NF = produtos[indice].Preco_Venda;
                    produtos[indice].Desc_Dado = (float)(100 * (produtos[indice].Preco_Lista - produtos[indice].Preco_Venda) / produtos[indice].Preco_Lista);
                }
            }

            return produtos;
        }

        public ExcluirOrcamentoResponse ExcluirOrcamento(OrcamentoResponseViewModel orcamento, UsuarioLogin usuario, string ip)
        {
            var response = new ExcluirOrcamentoResponse();
            response.Sucesso = false;

            //alterar o status do orçamento para excluido
            var atualizaStatus = AtualizarStatus((int)orcamento.Id, usuario, (short)Constantes.eCfgOrcamentoCotacaoStatus.EXCLUIDO, ip);

            if (atualizaStatus != null)
            {
                response.Mensagem = atualizaStatus.mensagem;
                return response;
            }

            response.Sucesso = true;
            return response;

        }

        public ExcluirOrcamentoResponse AnularOrcamento(OrcamentoResponseViewModel orcamento, UsuarioLogin usuario, string ip)
        {
            var response = new ExcluirOrcamentoResponse();
            response.Sucesso = false;

            //vamos abrir a transação e fazer as modificações nas tabelas
            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTO))
            {
                //atualizar o orçamento cotação
                var tOrcamentoCotacao = _orcamentoCotacaoBll.PorFiltroComTransacao(new TorcamentoCotacaoFiltro() { Id = (int)orcamento.Id, StatusId = (short)Constantes.eCfgOrcamentoCotacaoStatus.APROVADO }, dbGravacao).FirstOrDefault();
                if (tOrcamentoCotacao == null)
                {
                    response.Mensagem = "Falha ao buscar Orçamento!";
                    return response;
                }
                string idOrcamento = tOrcamentoCotacao.IdOrcamento;
                var retornoOrcamento = AtualizarAnulacaoOrcamento(tOrcamentoCotacao, usuario, dbGravacao);
                if (!string.IsNullOrEmpty(retornoOrcamento.mensagem))
                {
                    response.Mensagem = retornoOrcamento.mensagem;
                    return response;
                }
                //atualizar o pré-pedido
                var retornoPrepedido = _prepedidoBll.AtualizarAnulacaoOrcamentoCotacaoPrepedido(idOrcamento, usuario.Id, (int)usuario.TipoUsuario, dbGravacao);
                //atualizar todos os pedidos que existir na tabela(procurar por pedido base que está no pré-pedido)
                if (!string.IsNullOrEmpty(retornoPrepedido))
                {
                    //retornou o pedido no orçamento, então vamos atualizar
                    _pedidoPrepedidoApiBll.AtualizarAnulacaoOrcamentoCotacaoPedido(retornoPrepedido, dbGravacao);
                }
                //registrar o log de operação
                var cfgOperacao = _cfgOperacaoBll.PorFiltroComTransacao(new TcfgOperacaoFiltro() { Id = (int)Constantes.eCfgLogOperacao.ORCAMENTO_COTACAO_ANULACAO }, dbGravacao).FirstOrDefault();
                if (cfgOperacao == null)
                {
                    response.Mensagem = "Ops! Falha ao anular orçamento aprovado.";
                    return response;
                }
                var pedido = string.Empty;

                var log = $"Orcamento: {orcamento.Id}; Pre-pedido: {idOrcamento};";

                if (!string.IsNullOrEmpty(retornoPrepedido))
                {
                    log = $"{log} Pedido: {retornoPrepedido}";
                }

                var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuario.TipoUsuario, usuario.Id, orcamento.Loja, pedido, (int)orcamento.Id, null,
                        InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO, cfgOperacao.Id, ip);

                dbGravacao.SaveChanges();
                dbGravacao.transacao.Commit();
            }

            response.Sucesso = true;
            return response;
        }

        public MensagemDto AtualizarAnulacaoOrcamento(TorcamentoCotacao tOrcamentoCotacao, UsuarioLogin usuario, ContextoBdGravacao dbGravacao)
        {
            var response = new MensagemDto();

            tOrcamentoCotacao.Status = (short)Constantes.eCfgOrcamentoCotacaoStatus.EXCLUIDO;
            tOrcamentoCotacao.IdOrcamento = null;
            tOrcamentoCotacao.IdPedido = null;
            tOrcamentoCotacao.IdTipoUsuarioContextoUltStatus = (int)usuario.TipoUsuario;
            tOrcamentoCotacao.IdUsuarioUltStatus = usuario.Id;
            tOrcamentoCotacao.DataUltStatus = DateTime.Now.Date;
            tOrcamentoCotacao.DataHoraUltStatus = DateTime.Now;

            var retorno = _orcamentoCotacaoBll.AtualizarComTransacao(tOrcamentoCotacao, dbGravacao);
            if (retorno == null)
            {
                response.mensagem = "Falha ao atualizar anulação do orçamento!";
                return response;
            }

            return response;
        }

        public bool VerificaPermissaoAnularOrcamento(int idOrcamento)
        {
            var pedidoBase = _prepedidoBll.BuscarPedidoPrepedidoPorIdOrcamento(idOrcamento);
            if (string.IsNullOrEmpty(pedidoBase)) return true;

            return _pedidoPrepedidoApiBll.BuscarPedidosParaAnular(pedidoBase);
        }
    }
}
