using AutoMapper;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentistaEIndicadorVendedorBll
    {
        private readonly OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll;
        private readonly IMapper _mapper;
        private readonly Cfg.CfgOperacao.CfgOperacaoBll _cfgOperacaoBll;
        private readonly InfraBanco.ContextoBdProvider _contextoBdProvider;

        public OrcamentistaEIndicadorVendedorBll(
            OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll,
            IMapper _mapper,
             OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll,
             Cfg.CfgOperacao.CfgOperacaoBll cfgOperacaoBll,
             InfraBanco.ContextoBdProvider contextoBdProvider
            )
        {
            this._orcamentistaEindicadorVendedorBll = _orcamentistaEindicadorVendedorBll;
            this._mapper = _mapper;
            this._orcamentistaEIndicadorBll = _orcamentistaEIndicadorBll;
            _cfgOperacaoBll = cfgOperacaoBll;
            _contextoBdProvider = contextoBdProvider;
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

                var result = _orcamentistaEindicadorVendedorBll
                    .InserirComTransacao(objOrcamentistaEIndicadorVendedor, dbGravacao);

                if(result == null) throw new ArgumentException("Falha ao cadastrar novo usuário.");

                var cfgOperacao = _cfgOperacaoBll.PorFiltro(new TcfgOperacaoFiltro() { Id = 18 }).FirstOrDefault();
                if (cfgOperacao == null) return null;

                string omitirCampos = "|UsuarioCadastro|DataCadastro|UsuarioUltimaAlteracao|DataUltimaAlteracao|DataUltimaAlteracaoSenha|Datastamp|Senha|QtdeConsecutivaFalhaLogin|StLoginBloqueadoAutomatico|DataHoraBloqueadoAutomatico|EnderecoIpBloqueadoAutomatico|";
                string log = UtilsGlobais.Util.MontaLog(result, "", omitirCampos);

                var tLogV2 = UtilsGlobais.Util.GravaLogV2ComTransacao(dbGravacao, log, (short)usuarioLogado.TipoUsuario, usuarioLogado.Id, model.Loja, null, null, null,
                    InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO,
                    cfgOperacao.Id, ip);

                var ret = _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(result);

                dbGravacao.SaveChanges();
                dbGravacao.transacao.Commit();
                return ret;
            }

        }
    }
}
