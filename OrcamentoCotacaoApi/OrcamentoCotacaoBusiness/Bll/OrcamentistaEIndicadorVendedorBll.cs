﻿using AutoMapper;
using Azure.Core;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.LoginHistorico;
using OrcamentoCotacaoBusiness.Models.Request.OrcamentistaIndicadorVendedor;
using OrcamentoCotacaoBusiness.Models.Request.Usuario;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.OrcamentistaIndicadorVendedor;
using System;
using System.Collections.Generic;
using System.Linq;
using UtilsGlobais;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentistaEIndicadorVendedorBll
    {
        private readonly OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll;
        private readonly IMapper _mapper;
        private readonly Cfg.CfgOperacao.CfgOperacaoBll _cfgOperacaoBll;
        private readonly InfraBanco.ContextoBdProvider _contextoBdProvider;
        private readonly LoginHistoricoBll _loginHistoricoBll;
        private readonly OrcamentoCotacao.OrcamentoCotacaoBll orcamentoCotacaoBll;

        public OrcamentistaEIndicadorVendedorBll(
            OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll,
            IMapper _mapper,
             OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll,
             Cfg.CfgOperacao.CfgOperacaoBll cfgOperacaoBll,
             InfraBanco.ContextoBdProvider contextoBdProvider,
             LoginHistoricoBll _loginHistoricoBll,
             OrcamentoCotacao.OrcamentoCotacaoBll orcamentoCotacaoBll
            )
        {
            this._orcamentistaEindicadorVendedorBll = _orcamentistaEindicadorVendedorBll;
            this._mapper = _mapper;
            this._orcamentistaEIndicadorBll = _orcamentistaEIndicadorBll;
            _cfgOperacaoBll = cfgOperacaoBll;
            _contextoBdProvider = contextoBdProvider;
            this._loginHistoricoBll = _loginHistoricoBll;
            this.orcamentoCotacaoBll = orcamentoCotacaoBll;
        }

        public List<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresParceiro(string apelidoParceiro)
        {
            var parceiro = _orcamentistaEIndicadorBll.BuscarParceiros(new TorcamentistaEindicadorFiltro() { apelido = apelidoParceiro }).FirstOrDefault();

            if (parceiro == null)
                return null;

            var vendedoresParceiro = _orcamentistaEindicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = parceiro.IdIndicador });
            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(vendedoresParceiro);
        }

        public List<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresParceiroPorId(int idParceiro)
        {
            var parceiro = _orcamentistaEIndicadorBll.BuscarParceiros(new TorcamentistaEindicadorFiltro() { idParceiro = idParceiro }).FirstOrDefault();

            if (parceiro == null)
                return null;

            var vendedoresParceiro = _orcamentistaEindicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = parceiro.IdIndicador });
            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(vendedoresParceiro);
        }

        public List<OrcamentistaEIndicadorVendedorResponseViewModel> PorFiltro(TorcamentistaEIndicadorVendedorFiltro filtro)
        {
            var vendedoresParceiro = _orcamentistaEindicadorVendedorBll.PorFiltro(filtro);
            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(vendedoresParceiro);
        }

        public OrcamentistaEIndicadorVendedorResponseViewModel Inserir(UsuarioRequestViewModel model,
            UsuarioLogin usuarioLogado, string ip)
        {
            var tOrcamentistaIndicador = _orcamentistaEIndicadorBll
                .BuscarParceiroPorApelido(new TorcamentistaEindicadorFiltro() { apelido = model.Parceiro });

            if (tOrcamentistaIndicador == null)
                throw new KeyNotFoundException();

            var tOrcamentistaIndicadorVendedor = _orcamentistaEindicadorVendedorBll
                .PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { email = model.Email }).FirstOrDefault();
            if (tOrcamentistaIndicadorVendedor != null)
                throw new ArgumentException("Email já cadastrado");

            var atualizacaoSenhaMensagem = _orcamentistaEindicadorVendedorBll.SenhaValida(model.Nome, model.Senha);

            if (atualizacaoSenhaMensagem.Length > 0)
                throw new ArgumentException(atualizacaoSenhaMensagem);

            string senha_codificada = UtilsGlobais.Util.codificaDado(model.Senha, false);

            if (string.IsNullOrEmpty(senha_codificada))
                throw new ArgumentException("Falha na codificação de senha.");

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);

                objOrcamentistaEIndicadorVendedor.Datastamp = senha_codificada;
                objOrcamentistaEIndicadorVendedor.IdIndicador = tOrcamentistaIndicador.IdIndicador;
                objOrcamentistaEIndicadorVendedor.UsuarioCadastro = tOrcamentistaIndicador.Apelido;
                objOrcamentistaEIndicadorVendedor.UsuarioUltimaAlteracao = tOrcamentistaIndicador.Apelido;
                objOrcamentistaEIndicadorVendedor.DataCadastro = DateTime.Now;
                objOrcamentistaEIndicadorVendedor.DataUltimaAlteracao = DateTime.Now;
                objOrcamentistaEIndicadorVendedor.DataUltimaAlteracaoSenha = null;
                objOrcamentistaEIndicadorVendedor.Senha = Util.GerarSenhaAleatoria();

                var retorno = _orcamentistaEindicadorVendedorBll
                    .InserirComTransacao(objOrcamentistaEIndicadorVendedor, dbGravacao);

                if (retorno == null) throw new ArgumentException("Falha ao cadastrar novo usuário.");

                var cfgOperacao = _cfgOperacaoBll.PorFiltro(new TcfgOperacaoFiltro() { Id = 18 }).FirstOrDefault();
                if (cfgOperacao == null) return null;

                string omitirCampos = "|UsuarioCadastro|DataCadastro|UsuarioUltimaAlteracao|DataUltimaAlteracao|DataUltimaAlteracaoSenha|Datastamp|Senha|QtdeConsecutivaFalhaLogin|StLoginBloqueadoAutomatico|DataHoraBloqueadoAutomatico|EnderecoIpBloqueadoAutomatico|datastamp|";
                string log = UtilsGlobais.Util.MontaLog(retorno, "", omitirCampos);

                var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogado.TipoUsuario, usuarioLogado.Id, model.Loja, null, null, null,
                    InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO,
                    cfgOperacao.Id, ip);

                var ret = _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(retorno);

                dbGravacao.SaveChanges();
                dbGravacao.transacao.Commit();
                return ret;
            }
        }

        public OrcamentistaEIndicadorVendedorResponseViewModel Atualizar(UsuarioRequestViewModel model,
            UsuarioLogin usuarioLogado, string ip, bool selecionaQualquerIndicadorLoja, string vendedor)
        {
            var tOrcamentistaIndicadorVendedor = _orcamentistaEindicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { id = (int)model.Id }).FirstOrDefault();
            if (tOrcamentistaIndicadorVendedor == null) throw new KeyNotFoundException();

            // Parceiro
            if ((int)usuarioLogado.TipoUsuario == 2)
                if (tOrcamentistaIndicadorVendedor.VendedorResponsavel != vendedor)
                    throw new ArgumentException("Não é permitido alterar um usuário de outro vendedor responsável");

            // Usuário Interno
            if ((int)usuarioLogado.TipoUsuario == 1 && !selecionaQualquerIndicadorLoja)
                if (tOrcamentistaIndicadorVendedor.VendedorResponsavel != vendedor)
                    throw new ArgumentException("Não é permitido alterar um usuário de outro vendedor responsável");

            var existeEmail = _orcamentistaEindicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { email = model.Email });

            if (existeEmail.Any() && existeEmail.FirstOrDefault().Id != model.Id)
                throw new ArgumentException("Email já cadastrado");

            var atualizacaoSenhaMensagem = _orcamentistaEindicadorVendedorBll.SenhaValida(model.Nome, model.Senha);

            if (atualizacaoSenhaMensagem.Length > 0)
                throw new ArgumentException(atualizacaoSenhaMensagem);

            string senha_codificada = UtilsGlobais.Util.codificaDado(model.Senha, false);
            string senhaBaseCodificada = UtilsGlobais.Util.codificaDado(tOrcamentistaIndicadorVendedor.Datastamp, false);
            tOrcamentistaIndicadorVendedor.Datastamp = senhaBaseCodificada;

            if (string.IsNullOrEmpty(senha_codificada))
                throw new ArgumentException("Falha na codificação de senha.");

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
                if(senhaBaseCodificada == senha_codificada)
                {
                    objOrcamentistaEIndicadorVendedor.DataUltimaAlteracaoSenha = tOrcamentistaIndicadorVendedor.DataUltimaAlteracaoSenha;
                }

                objOrcamentistaEIndicadorVendedor.Datastamp = senha_codificada;
                objOrcamentistaEIndicadorVendedor.Nome = model.Nome;
                objOrcamentistaEIndicadorVendedor.Celular = model.Celular;
                objOrcamentistaEIndicadorVendedor.Telefone = model.Telefone;
                objOrcamentistaEIndicadorVendedor.Email = model.Email;
                objOrcamentistaEIndicadorVendedor.Ativo = model.Ativo;
                objOrcamentistaEIndicadorVendedor.UsuarioUltimaAlteracao = tOrcamentistaIndicadorVendedor.UsuarioUltimaAlteracao;
                objOrcamentistaEIndicadorVendedor.DataUltimaAlteracao = DateTime.Now;
                objOrcamentistaEIndicadorVendedor.DataCadastro = tOrcamentistaIndicadorVendedor.DataCadastro;
                objOrcamentistaEIndicadorVendedor.IdIndicador = tOrcamentistaIndicadorVendedor.IdIndicador;
                objOrcamentistaEIndicadorVendedor.UsuarioCadastro = tOrcamentistaIndicadorVendedor.UsuarioCadastro;
                objOrcamentistaEIndicadorVendedor.StLoginBloqueadoAutomatico = model.StLoginBloqueadoAutomatico;
                objOrcamentistaEIndicadorVendedor.Senha = Util.GerarSenhaAleatoria();

                var retorno = _orcamentistaEindicadorVendedorBll.AtualizarComTransacao(objOrcamentistaEIndicadorVendedor, dbGravacao);

                if (retorno == null) throw new ArgumentException("Falha ao cadastrar novo usuário.");

                var cfgOperacao = _cfgOperacaoBll.PorFiltro(new TcfgOperacaoFiltro() { Id = 19 }).FirstOrDefault();
                if (cfgOperacao == null) return null;

                string campoAOmitir = "|Senha|datastamp|";
                string log = $"id={tOrcamentistaIndicadorVendedor.Id}; {UtilsGlobais.Util.MontalogComparacao(retorno, tOrcamentistaIndicadorVendedor, "", campoAOmitir)}";

                var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogado.TipoUsuario, usuarioLogado.Id, model.Loja, null, null, null,
                    InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO,
                    cfgOperacao.Id, ip);

                var ret = _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(retorno);

                dbGravacao.SaveChanges();
                dbGravacao.transacao.Commit();
                return ret;
            }
        }

        public OrcamentistaIndicadorVendedorDeleteResponse Deletar(OrcamentistaIndicadorVendedorDeleteRequest request)
        {
            var response = new OrcamentistaIndicadorVendedorDeleteResponse();
            response.Sucesso = false;

            var tOrcamentistaIndicadorVendedor = _orcamentistaEindicadorVendedorBll.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { id = request.IdIndicadorVendedor }).FirstOrDefault();
            if (tOrcamentistaIndicadorVendedor == null)
            {
                response.Mensagem = "Ops! Usuário não encontrado!";
                return response;
            }

            var loginHistoricoresponse = _loginHistoricoBll.PorFiltro(new LoginHistoricoRequest()
            {
                IdUsuario = request.IdIndicadorVendedor,
                SistemaResponsavel = (short?)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO
            });

            if (!loginHistoricoresponse.Sucesso)
            {
                response.Mensagem = loginHistoricoresponse.Mensagem;
                return response;
            }

            var logou = loginHistoricoresponse.LstLoginHistoricoResponse.Where(x => x.StSucesso == true).FirstOrDefault();
            if (logou != null)
            {
                response.Mensagem = "Exclusão não permitida. Usuário efetuou logon no Sistema alguma vez.";
                return response;
            }

            var existe = orcamentoCotacaoBll.PorFiltro(new TorcamentoCotacaoFiltro()
            {
                IdIndicadorVendedor = request.IdIndicadorVendedor
            });

            if (existe.Count > 0)
            {
                response.Mensagem = "Exclusão não permitida. Usuário está relacionado a algum orçamento.";
                return response;
            }

            using (var dbGravacao = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                _orcamentistaEindicadorVendedorBll.ExcluirComTransacao(tOrcamentistaIndicadorVendedor, dbGravacao);

                dbGravacao.SaveChanges();
                dbGravacao.transacao.Commit();

                response.Sucesso = true;

                return response;
            }
        }

        public List<TorcamentistaEIndicadorVendedor> BuscarVendedorParceirosPorParceiros(TorcamentistaEIndicadorVendedorFiltro obj)
        {
            return _orcamentistaEindicadorVendedorBll.BuscarVendedorParceirosPorParceiros(obj);
        }

        public ListaOrcamentistaVendedorResponse ListarOrcamentistaVendedor(UsuariosRequest request)
        {
            var response = new ListaOrcamentistaVendedorResponse();
            response.Sucesso = false;

            var filtro = new TorcamentistaEIndicadorVendedorFiltro();
            filtro.TipoUsuario = request.TipoUsuario;
            filtro.nomeVendedor= request.Vendedor;
            filtro.Parceiro = request.Parceiro;
            filtro.loja= request.Loja;
            filtro.Pesquisa = request.Pesquisa;
            filtro.ativo = request.Ativo;
            filtro.Pagina = request.Pagina;
            filtro.QtdeItensPagina = request.QtdeItensPagina;
            filtro.OrdenacaoAscendente = request.OrdenacaoAscendente;
            filtro.NomeColuna = request.NomeColuna;

            var objRetorno = _orcamentistaEindicadorVendedorBll.ListarOrcamentistaVendedor(filtro);
            if(objRetorno != null)
            {
                response.QtdeRegistros = (int)objRetorno.GetType().GetProperty("QtdeRegistros").GetValue(objRetorno, null);
                var lista = (List<TorcamentistaEIndicadorVendedor>)objRetorno.GetType().GetProperty("Lista").GetValue(objRetorno, null);
                response.ListaOrcamentistaVendedor = new List<OrcamentistaVendedorResponse>();
                foreach (var item in lista)
                {
                    var usuario = new OrcamentistaVendedorResponse();
                    usuario.Id = item.Id;
                    usuario.Nome = item.Nome;
                    usuario.Email = item.Email;
                    usuario.Parceiro = item.Parceiro;
                    usuario.Ativo = item.Ativo;
                    usuario.AtivoLabel = item.Ativo ? "Sim" : "Não";
                    usuario.VendedorResponsavel = item.VendedorResponsavel;
                    response.ListaOrcamentistaVendedor.Add(usuario);
                }
            }

            response.ListaOrcamentistaVendedor = response.ListaOrcamentistaVendedor.OrderBy(o => o.Nome).ToList();
            //fazer a busca
            response.Sucesso = true;
            return response;
        }
    }
}