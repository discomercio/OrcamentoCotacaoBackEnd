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
                response.Mensagem = "Obrigatório o preenchimento do campo IdOrcamento.";
                return response;
            }

            var idTipoUsuario = request.TipoUsuario;
            var usuario = request.Usuario.ToUpper().Trim();
            var idOrcamento = request.IdOrcamento;

            // Orçamento
            var usuarioEnvolvidoOrcamento = UsuarioEnvolvidoOrcamento(idTipoUsuario, usuario, idOrcamento);

            if (usuarioEnvolvidoOrcamento)
            {
                // Status Orçamento
                var statusOrcamentoEnviado = ObterStatusOrcamento(idOrcamento);

                // Orçamento Expirado
                var orcamentoExpirado = VerificarValidadeOrcamento(idOrcamento);

                // Permissões
                var permissaoVisualizarOrcamentoConsultar = ValidaPermissao(request.PermissoesUsuario, ePermissao.VisualizarOrcamentoConsultar);
                var permissaoAcessoUniversalOrcamentoEditar = ValidaPermissao(request.PermissoesUsuario, ePermissao.AcessoUniversalOrcamentoEditar);
                var permissaoProrrogarVencimentoOrcamento = ValidaPermissao(request.PermissoesUsuario, ePermissao.ProrrogarVencimentoOrcamento);
                var permissaoAprovarOrcamento = ValidaPermissao(request.PermissoesUsuario, ePermissao.AprovarOrcamento);
                var permissaoDescontoSuperior1 = ValidaPermissao(request.PermissoesUsuario, ePermissao.DescontoSuperior1);
                var permissaoDescontoSuperior2 = ValidaPermissao(request.PermissoesUsuario, ePermissao.DescontoSuperior2);
                var permissaoDescontoSuperior3 = ValidaPermissao(request.PermissoesUsuario, ePermissao.DescontoSuperior3);

                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    var loja = ObterLojaPorOrcamento(request.IdOrcamento);

                    if (string.IsNullOrEmpty(loja))
                    {
                        response.VisualizarOrcamento = false;
                        response.Sucesso = false;
                        response.Mensagem = "Orçamento não esta relacionado com uma loja.";
                        return response;
                    }

                    var usuarioAcessaLoja = UsuarioAcessaLoja(usuario, loja);
                    if (!usuarioAcessaLoja && !permissaoVisualizarOrcamentoConsultar)
                    {
                        response.VisualizarOrcamento = false;
                        response.Sucesso = false;
                        response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
                        return response;
                    }
                }

                response.VisualizarOrcamento = true;
                response.ClonarOrcamento = true;

                if (statusOrcamentoEnviado == StatusOrcamento.Enviado)
                {
                    response.EditarOrcamento = true;
                    response.CancelarOrcamento = true;
                    response.ProrrogarOrcamento = permissaoProrrogarVencimentoOrcamento;
                    response.ReenviarOrcamento = true;

                    if (permissaoAcessoUniversalOrcamentoEditar || idTipoUsuario != (int)Constantes.TipoUsuario.VENDEDOR)
                    {
                        response.EditarOpcaoOrcamento = true;
                    }
                    else
                    {
                        response.EditarOpcaoOrcamento =
                            (permissaoDescontoSuperior1
                            || permissaoDescontoSuperior2
                            || permissaoDescontoSuperior3);
                    }

                    response.DesabilitarAprovarOpcaoOrcamento = !permissaoAprovarOrcamento;
                    response.MensagemOrcamento = true;

                    if (orcamentoExpirado)
                    {
                        response.EditarOrcamento = false;
                        response.ReenviarOrcamento = false;
                        response.EditarOpcaoOrcamento = false;
                        response.DesabilitarAprovarOpcaoOrcamento = true;
                        response.MensagemOrcamento = false;
                    }
                }
                else if (statusOrcamentoEnviado == StatusOrcamento.Aprovado
                        || statusOrcamentoEnviado == StatusOrcamento.Cancelado)
                {
                    response.ProrrogarOrcamento = false;
                    response.EditarOrcamento = false;
                    response.CancelarOrcamento = false;
                    response.ReenviarOrcamento = false;
                    response.DesabilitarBotoes = false;
                    response.EditarOpcaoOrcamento = false;
                    response.DesabilitarAprovarOpcaoOrcamento = true;
                }
            }
            else
            {
                response.VisualizarOrcamento = false;
                response.Sucesso = false;
                response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
            }

            response.NenhumaOpcaoOrcamento = false;
            response.DesabilitarBotoes = false;

            return response;
        }

        public async Task<PermissaoPrePedidoResponse> RetornarPermissaoPrePedido(PermissaoPrePedidoRequest request)
        {
            var response = new PermissaoPrePedidoResponse();

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

            if (string.IsNullOrEmpty(request.IdPrePedido))
            {
                response.Sucesso = false;
                response.Mensagem = "Obrigatório o preenchimento do campo IdPrePedido.";
                return response;
            }

            var idTipoUsuario = request.TipoUsuario;
            var usuario = request.Usuario.ToUpper().Trim();
            var idPrePedido = request.IdPrePedido;

            var usuarioEnvolvidoOrcamento = UsuarioEnvolvidoPrePedido(idTipoUsuario, usuario, idPrePedido);

            if (usuarioEnvolvidoOrcamento)
            {
                var permissaoVisualizarOrcamentoConsultar = ValidaPermissao(request.PermissoesUsuario, ePermissao.VisualizarOrcamentoConsultar);
                var permissaoVisualizarPrePedidoConsultar = ValidaPermissao(request.PermissoesUsuario, ePermissao.VisualizarPrePedidoConsultar);
                var permissaoCancelarPrePedido = ValidaPermissao(request.PermissoesUsuario, ePermissao.CancelarPrePedido);

                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    var loja = ObterLojaPorPrePedido(idPrePedido);

                    if (string.IsNullOrEmpty(loja))
                    {
                        response.VisualizarPrePedido = false;
                        response.Sucesso = false;
                        response.Mensagem = "Pre Pedido não esta relacionado com uma loja.";
                        return response;
                    }

                    var usuarioAcessaLoja = UsuarioAcessaLoja(usuario, loja);

                    if (!usuarioAcessaLoja && !permissaoVisualizarOrcamentoConsultar)
                    {
                        response.VisualizarPrePedido = false;
                        response.Sucesso = false;
                        response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
                        return response;
                    }
                }

                response.VisualizarPrePedido = permissaoVisualizarPrePedidoConsultar;
                response.CancelarPrePedido = permissaoCancelarPrePedido;
            }
            else
            {
                response.VisualizarPrePedido = false;
                response.Sucesso = false;
                response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
            }
            
            return response;
        }

        public async Task<PermissaoPedidoResponse> RetornarPermissaoPedido(PermissaoPedidoRequest request)
        {
            var response = new PermissaoPedidoResponse();

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

            if (string.IsNullOrEmpty(request.IdPedido))
            {
                response.Sucesso = false;
                response.Mensagem = "Obrigatório o preenchimento do campo IdPedido.";
                return response;
            }

            var idTipoUsuario = request.TipoUsuario;
            var usuario = request.Usuario.ToUpper().Trim();
            var idPedido = request.IdPedido;

            var usuarioEnvolvido = UsuarioEnvolvidoPedido(idTipoUsuario, usuario, idPedido);

            if (usuarioEnvolvido)
            {
                var permissaoVisualizarOrcamentoConsultar = ValidaPermissao(request.PermissoesUsuario, ePermissao.VisualizarOrcamentoConsultar);
                var permissaoConsultarPedido = ValidaPermissao(request.PermissoesUsuario, ePermissao.ConsultarPedido);

                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    var loja = ObterLojaPorPedido(idPedido);

                    if (string.IsNullOrEmpty(loja))
                    {
                        response.VisualizarPedido = false;
                        response.Sucesso = false;
                        response.Mensagem = "Pedido não esta relacionado com uma loja.";
                        return response;
                    }

                    var usuarioAcessaLoja = UsuarioAcessaLoja(usuario, loja);

                    if (!usuarioAcessaLoja && !permissaoVisualizarOrcamentoConsultar)
                    {
                        response.VisualizarPedido = false;
                        response.Sucesso = false;
                        response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
                        return response;
                    }
                }

                response.VisualizarPedido = permissaoConsultarPedido;
            }
            else
            {
                response.VisualizarPedido = false;
                response.Sucesso = false;
                response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
            }
            
            return response;
        }

        private bool ValidaPermissao(List<string> permissoesUsuario, ePermissao permissao)
        {
            var idPermissao = (int)permissao;

            return permissoesUsuario.Contains(idPermissao.ToString());
        }

        private bool UsuarioEnvolvidoOrcamento(int idTipoUsuario, string usuario, int idOrcamento)
        {
            bool usuarioEnvolvido = false;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    usuarioEnvolvido = (from o in db.TorcamentoCotacao
                                        join u in db.Tusuario
                                             on o.IdVendedor equals u.Id
                                        where
                                             u.Usuario == usuario
                                             && o.Id == idOrcamento
                                        select o).Any();
                }

                if (idTipoUsuario == (int)Constantes.TipoUsuario.PARCEIRO)
                {
                    usuarioEnvolvido = (from o in db.TorcamentoCotacao
                                        join u in db.TorcamentistaEindicador
                                             on o.IdIndicador equals u.IdIndicador
                                        where
                                             u.Apelido == usuario
                                             && o.Id == idOrcamento
                                        select o).Any();
                }

                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    usuarioEnvolvido = (from o in db.TorcamentoCotacao
                                        join u in db.TorcamentistaEindicadorVendedor
                                             on o.IdIndicadorVendedor equals u.Id
                                        where
                                             u.Email == usuario
                                             && o.Id == idOrcamento
                                        select o).Any();
                }
            }

            return usuarioEnvolvido;
        }

        private bool UsuarioEnvolvidoPrePedido(int idTipoUsuario, string usuario, string idPrePedido)
        {
            bool usuarioEnvolvido = false;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    usuarioEnvolvido = (from o in db.Torcamento
                                        where
                                             o.Vendedor == usuario
                                             && o.Orcamento == idPrePedido
                                        select o).Any();
                }

                if (idTipoUsuario == (int)Constantes.TipoUsuario.PARCEIRO)
                {
                    usuarioEnvolvido = (from o in db.Torcamento
                                        where
                                             o.Orcamentista == usuario
                                             && o.Orcamento == idPrePedido
                                        select o).Any();
                }

                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var idIndicadorVendedor = 0;

                    if (!int.TryParse(usuario, out idIndicadorVendedor))
                    {
                        throw new Exception("Problemas na conversão de Vendedor do parceiro.");
                    }

                    usuarioEnvolvido = (from o in db.Torcamento
                                        where
                                             o.IdIndicadorVendedor == idIndicadorVendedor
                                             && o.Orcamento == idPrePedido
                                        select o).Any();
                }
            }

            return usuarioEnvolvido;
        }

        private bool UsuarioEnvolvidoPedido(int idTipoUsuario, string usuario, string idPedido)
        {
            bool usuarioEnvolvido = false;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    usuarioEnvolvido = (from o in db.Tpedido
                                        where
                                             o.Vendedor == usuario
                                             && o.Pedido == idPedido
                                        select o).Any();
                }

                if (idTipoUsuario == (int)Constantes.TipoUsuario.PARCEIRO)
                {
                    usuarioEnvolvido = (from o in db.Tpedido
                                        where
                                             o.Orcamentista == usuario
                                             && o.Pedido == idPedido
                                        select o).Any();
                }

                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                {
                    var idIndicadorVendedor = 0;

                    if (!int.TryParse(usuario, out idIndicadorVendedor))
                    {
                        throw new Exception("Problemas na conversão de Vendedor do parceiro.");
                    }

                    usuarioEnvolvido = (from o in db.Tpedido
                                        where
                                             o.IdIndicadorVendedor == idIndicadorVendedor
                                             && o.Pedido == idPedido
                                        select o).Any();
                }
            }

            return usuarioEnvolvido;
        }

        private string ObterLojaPorPrePedido(string idPrePedido)
        {
            string loja = string.Empty;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                loja = (from o in db.Torcamento
                        where
                             o.Orcamento == idPrePedido
                        select o.Loja).FirstOrDefault();
            }

            return loja;
        }

        private string ObterLojaPorPedido(string idPedido)
        {
            string loja = string.Empty;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                loja = (from o in db.Tpedido
                        where
                             o.Pedido == idPedido
                        select o.Loja).FirstOrDefault();
            }

            return loja;
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

        private StatusOrcamento ObterStatusOrcamento(int idOrcamento)
        {
            int statusOrcamentoEnviado;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                statusOrcamentoEnviado = (from o in db.TorcamentoCotacao
                                          where
                                               o.Id == idOrcamento
                                          select o.Status).FirstOrDefault();
            }

            return (StatusOrcamento)statusOrcamentoEnviado;
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
    }
}