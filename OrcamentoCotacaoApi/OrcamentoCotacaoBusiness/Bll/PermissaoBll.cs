using InfraBanco;
using InfraBanco.Constantes;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaoBusiness.Bll
{
    public sealed class PermissaoBll
    {
        private readonly ILogger<PermissaoBll> _logger;
        private readonly ContextoBdProvider _contextoProvider;

        public PermissaoBll(ILogger<PermissaoBll> logger, ContextoBdProvider contextoProvider)
        {
            _logger = logger;
            _contextoProvider = contextoProvider;
        }

        public async Task<PermissaoOrcamentoResponse> RetornarPermissaoOrcamento(PermissaoOrcamentoRequest request)
        {
            var response = new PermissaoOrcamentoResponse();

            if (request.TipoUsuario <= 0)
            {
                response.Sucesso = false;
                response.Mensagem = "Obrigatório o preenchimento do campo Tipo de Usuario.";
                return response;
            }

            if (string.IsNullOrEmpty(request.Usuario))
            {
                response.Sucesso = false;
                response.Mensagem = "Obrigatório o preenchimento do campo Usuário.";
                return response;
            }

            if (request.IdOrcamento <= 0)
            {
                response.Sucesso = false;
                response.Mensagem = "Obrigatório o preenchimento do campo IdOrçamento.";
                return response;
            }

            var idTipoUsuario = request.TipoUsuario;
            var usuario = request.Usuario.ToUpper().Trim();
            var idOrcamento = request.IdOrcamento;

            // Permissões
            var permissaoVisualizarOrcamentoConsultar = ValidaPermissao(request.PermissoesUsuario, ePermissao.VisualizarOrcamentoConsultar);
            var permissaoAcessoUniversalOrcamentoEditar = ValidaPermissao(request.PermissoesUsuario, ePermissao.AcessoUniversalOrcamentoEditar);
            var permissaoProrrogarVencimentoOrcamento = ValidaPermissao(request.PermissoesUsuario, ePermissao.ProrrogarVencimentoOrcamento);
            var permissaoAprovarOrcamento = ValidaPermissao(request.PermissoesUsuario, ePermissao.AprovarOrcamento);
            var permissaoDescontoSuperior1 = ValidaPermissao(request.PermissoesUsuario,ePermissao.DescontoSuperior1);
            var permissaoDescontoSuperior2 = ValidaPermissao(request.PermissoesUsuario, ePermissao.DescontoSuperior2);
            var permissaoDescontoSuperior3 = ValidaPermissao(request.PermissoesUsuario, ePermissao.DescontoSuperior3);

            // Orçamento
            var usuarioEnvolvidoOrcamento = UsuarioEnvolvidoOrcamento(idTipoUsuario, usuario, idOrcamento);

            if (usuarioEnvolvidoOrcamento || permissaoVisualizarOrcamentoConsultar)
            {
                var usuarioAcessaLoja = true; // ??

                // Loja
                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    var loja = ObterLojaPorOrcamento(request.IdOrcamento);

                    if (string.IsNullOrEmpty(loja))
                    {
                        response.Sucesso = false;
                        response.Mensagem = "Orçamento não esta relacionado com uma loja.";
                        return response;
                    }

                    usuarioAcessaLoja = UsuarioAcessaLoja(usuario, loja);
                    if (!usuarioAcessaLoja)
                    {
                        response.VizualizarOrcamento = false;

                        response.Sucesso = false;
                        response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
                        return response;
                    }
                }

                // Status Orçamento
                var statusOrcamentoEnviado = ObterStatusOrcamento(idOrcamento);

                // Orçamento Expirado
                var orcamentoExpirado = VerificarValidadeOrcamento(idOrcamento);

                // cenarios
                var cenario1 = usuarioEnvolvidoOrcamento;
                var cenario2 = (usuarioAcessaLoja && permissaoAcessoUniversalOrcamentoEditar);
                var cenario3 = (permissaoDescontoSuperior1 || permissaoDescontoSuperior2 || permissaoDescontoSuperior3);


                response.VizualizarOrcamento = true;
                response.ProrrogarOrcamento = (permissaoProrrogarVencimentoOrcamento && (cenario1 || cenario2) && statusOrcamentoEnviado);
                response.EditarOrcamento = ((cenario1 || cenario2) && (statusOrcamentoEnviado || !orcamentoExpirado));
                response.CancelarOrcamento = ((cenario1 || cenario2) && statusOrcamentoEnviado);
                response.ClonarOrcamento = true;
                response.NenhumaOpcaoOrcamento = false;
                response.ReenviarOrcamento = statusOrcamentoEnviado;
                response.DesabilitarBotoes = !statusOrcamentoEnviado;
                response.EditarOpcaoOrcamento = ((cenario1 || cenario2 || cenario3) && (statusOrcamentoEnviado || !orcamentoExpirado));
                response.AprovarOpcaoOrcamento = (permissaoAprovarOrcamento && (cenario1 || cenario2) && (statusOrcamentoEnviado || !orcamentoExpirado));

                if (!response.CancelarOrcamento && !response.ProrrogarOrcamento && !response.EditarOrcamento)
                {
                    response.NenhumaOpcaoOrcamento = true;
                }
            }
            else
            {
                response.VizualizarOrcamento = false;
                response.Sucesso = false;
                response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
            }

            return response;
        }

        private bool UsuarioEnvolvidoOrcamento(int idTipoUsuario, string usuario, int idOrcamento)
        {
            bool usuarioEnvolvidoOrcamento = false;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    usuarioEnvolvidoOrcamento = (from o in db.TorcamentoCotacao
                                                 join u in db.Tusuario
                                                      on o.IdVendedor equals u.Id
                                                 where
                                                      u.Usuario == usuario
                                                      && o.Id == idOrcamento
                                                 select o).Any();
                }

                if (idTipoUsuario == (int)Constantes.TipoUsuario.PARCEIRO)
                {
                    usuarioEnvolvidoOrcamento = (from o in db.TorcamentoCotacao
                                                 join u in db.TorcamentistaEindicador
                                                      on o.IdIndicador equals u.IdIndicador
                                                 where
                                                      u.Apelido == usuario
                                                      && o.Id == idOrcamento
                                                 select o).Any();
                }

                if (idTipoUsuario == (int)Constantes.TipoUsuario.PARCEIRO)
                {
                    usuarioEnvolvidoOrcamento = (from o in db.TorcamentoCotacao
                                                 join u in db.TorcamentistaEindicadorVendedor
                                                      on o.IdIndicadorVendedor equals u.Id
                                                 where
                                                      u.Email == usuario
                                                      && o.Id == idOrcamento
                                                 select o).Any();
                }
            }

            return usuarioEnvolvidoOrcamento;
        }

        private string ObterLojaPorOrcamento(int idOrcamento)
        {
            string loja = string.Empty;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                loja = (from o in db.TorcamentoCotacao
                        where
                             o.Id == idOrcamento
                        select o.Loja).FirstOrDefault();
            }

            return loja;
        }

        private bool UsuarioAcessaLoja(string usuario, string loja)
        {
            bool usuarioAcessaLoja = false;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                usuarioAcessaLoja = (from o in db.TusuarioXLoja
                                     where
                                          o.Usuario == usuario
                                          && o.Loja == loja
                                          && o.Excluido_Status == 0
                                     select o).Any();
            }

            return usuarioAcessaLoja;
        }

        private bool ObterStatusOrcamento(int idOrcamento)
        {
            int statusOrcamentoEnviado;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                statusOrcamentoEnviado = (from o in db.TorcamentoCotacao
                                          join s in db.TcfgOrcamentoCotacaoStatus on o.Status equals s.Id
                                          where
                                               o.Id == idOrcamento
                                          select o.Status).FirstOrDefault();
            }

            return statusOrcamentoEnviado == 1;
        }

        private bool VerificarValidadeOrcamento(int idOrcamento)
        {
            DateTime dataValidadeOrcamento;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                dataValidadeOrcamento = (from o in db.TorcamentoCotacao
                                        where o.Id == idOrcamento
                                        select o.Validade).FirstOrDefault();
            }

            return dataValidadeOrcamento.Date < DateTime.Now.Date;
        }

        private bool ValidaPermissao(List<string> permissoesUsuario, ePermissao permissao)
        {
            var idPermissao = (int)permissao;

            return permissoesUsuario.Contains(idPermissao.ToString());
        }
    }
}