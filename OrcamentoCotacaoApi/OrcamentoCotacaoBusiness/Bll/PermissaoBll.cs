using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
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
            var orcamento = ObterOrcamentoPorIdOrcamento(idOrcamento);

            if (orcamento == null)
            {
                response.VisualizarOrcamento = false;
                response.Sucesso = false;
                response.Mensagem = "Orçamento não encontrado.";
                return response;
            }

            // Loja 
            var loja = ObterLojaPorOrcamento(idOrcamento);

            if (string.IsNullOrEmpty(loja))
            {
                response.VisualizarOrcamento = false;
                response.Sucesso = false;
                response.Mensagem = "Orçamento não esta relacionado com uma loja.";
                return response;
            }

            // Prazo orçamento encerrado
            var MaxPrazoConsultaOrcamentoEncerrado = 18;
            var prazoOrcamentoEncerrado = VerificarUnidadeNegocioParametro(
                orcamento.Loja,
                orcamento.DataCadastro,
                MaxPrazoConsultaOrcamentoEncerrado);

            if (prazoOrcamentoEncerrado)
            {
                response.VisualizarOrcamento = false;
                response.Sucesso = false;
                response.Mensagem = "Orçamento não esta mais disponível para visualização.";
                return response;
            }

            // Envolvido com orçamento
            var usuarioEnvolvidoOrcamento = UsuarioEnvolvidoOrcamento(idTipoUsuario, usuario, idOrcamento);

            // Permissões
            var permissaoVisualizarOrcamentoConsultar = ValidaPermissao(request.PermissoesUsuario, ePermissao.AcessoUniversalOrcamentoPedidoPrepedidoConsultar);
            var permissaoAcessoUniversalOrcamentoEditar = ValidaPermissao(request.PermissoesUsuario, ePermissao.AcessoUniversalOrcamentoEditar);
            var permissaoDescontoSuperior1 = ValidaPermissao(request.PermissoesUsuario, ePermissao.DescontoSuperior1);
            var permissaoDescontoSuperior2 = ValidaPermissao(request.PermissoesUsuario, ePermissao.DescontoSuperior2);
            var permissaoDescontoSuperior3 = ValidaPermissao(request.PermissoesUsuario, ePermissao.DescontoSuperior3);
            var permissaoDesconto = (permissaoDescontoSuperior1 || permissaoDescontoSuperior2 || permissaoDescontoSuperior3);

            if (usuarioEnvolvidoOrcamento 
                || permissaoVisualizarOrcamentoConsultar 
                || permissaoAcessoUniversalOrcamentoEditar
                || permissaoDesconto)
            {
                // Status Orçamento
                var statusOrcamentoEnviado = ObterStatusOrcamento(orcamento);

                // Orçamento Expirado
                var orcamentoExpirado = VerificarValidadeOrcamento(orcamento);

                // Permissões
                var permissaoProrrogarVencimentoOrcamento = ValidaPermissao(request.PermissoesUsuario, ePermissao.ProrrogarVencimentoOrcamento);
                var permissaoAprovarOrcamento = ValidaPermissao(request.PermissoesUsuario, ePermissao.AprovarOrcamento);

                if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    var usuarioAcessaLoja = UsuarioAcessaLoja(usuario, loja);
                    if (!usuarioAcessaLoja)
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
                    response.EditarOpcaoOrcamento = true;
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
            var idUsuario = request.IdUsuario;
            var usuario = request.Usuario.ToUpper().Trim();
            var idPrePedido = request.IdPrePedido;

            // Pedido
            var prePedido = ObterPrePedidoPorIdPrePedido(idPrePedido);

            if (prePedido == null)
            {
                response.VisualizarPrePedido = false;
                response.Sucesso = false;
                response.Mensagem = "Pré Pedido não encontrado.";
                return response;
            }

            // Prazo pre pedido encerrado
            var MaxPrazoConsultaOrcamentoEncerrado = 18;
            var prazoPedidoEncerrado = VerificarUnidadeNegocioParametro(
                prePedido.Loja,
                prePedido.Data.Value,
                MaxPrazoConsultaOrcamentoEncerrado);

            if (prazoPedidoEncerrado)
            {
                response.VisualizarPrePedido = false;
                response.Sucesso = false;
                response.Mensagem = "Pré pedido não esta mais disponível para visualização.";
                return response;
            }

            var permissaoVisualizarPrePedidoConsultar = ValidaPermissao(request.PermissoesUsuario, ePermissao.VisualizarPrePedidoConsultar);

            if (!permissaoVisualizarPrePedidoConsultar)
            {
                response.VisualizarPrePedido = false;
                response.Sucesso = false;
                response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
                return response;
            }

            var usuarioEnvolvidoOrcamento = UsuarioEnvolvidoPrePedido(idTipoUsuario, idUsuario, usuario, idPrePedido);
            var permissaoVisualizarOrcamentoConsultar = ValidaPermissao(request.PermissoesUsuario, ePermissao.AcessoUniversalOrcamentoPedidoPrepedidoConsultar);
            var permissaoAcessoUniversalOrcamentoEditar = ValidaPermissao(request.PermissoesUsuario, ePermissao.AcessoUniversalOrcamentoEditar);

            if (usuarioEnvolvidoOrcamento 
                || permissaoVisualizarOrcamentoConsultar
                || permissaoAcessoUniversalOrcamentoEditar)
            {
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
                response.Mensagem = "Obrigatório o preenchimento do campo IdPedido/IdPrePedido.";
                return response;
            }

            var idTipoUsuario = request.TipoUsuario;
            var idUsuario = request.IdUsuario;
            var usuario = request.Usuario.ToUpper().Trim();

            if (request.IdPedido.ToUpper().Contains("Z"))
            {
                var idPrePedido = request.IdPedido;

                var prePedido = ObterPrePedidoPorIdPrePedido(idPrePedido);

                if (prePedido != null)
                {
                    response.PrePedidoVirouPedido = true;
                    response.IdPedido = prePedido.Pedido;
                }
            }

            var idPedido = response.PrePedidoVirouPedido ? response.IdPedido : request.IdPedido;

            // Pedido
            var pedido = ObterPedidoPorIdPedido(idPedido);

            if (pedido == null)
            {
                response.VisualizarPedido = false;
                response.Sucesso = false;
                response.Mensagem = "Pedido não encontrado.";
                return response;
            }

            // Prazo pedido encerrado
            var MaxPrazoConsultaOrcamentoEncerrado = 19;

            DateTime data;

            if (pedido.Cancelado_Data.HasValue)
            {
                data = pedido.Cancelado_Data.Value;
            }
            else if (pedido.Entregue_Data.HasValue)
            {
                data = pedido.Entregue_Data.Value;
            }
            else
            {
                data = DateTime.Now;
            }

            var prazoPedidoEncerrado = VerificarUnidadeNegocioParametro(
                pedido.Loja,
                data,
                MaxPrazoConsultaOrcamentoEncerrado);

            if (prazoPedidoEncerrado)
            {
                response.VisualizarPedido = false;
                response.Sucesso = false;
                response.Mensagem = "Pedido não esta mais disponível para visualização.";
                return response;
            }

            var usuarioEnvolvido = UsuarioEnvolvidoPedido(idTipoUsuario, idUsuario, usuario, idPedido);

            var permissaoVisualizarOrcamentoConsultar = ValidaPermissao(request.PermissoesUsuario, ePermissao.AcessoUniversalOrcamentoPedidoPrepedidoConsultar);

            if (usuarioEnvolvido || permissaoVisualizarOrcamentoConsultar)
            {
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

        public async Task<PermissaoIncluirPrePedidoResponse> RetornarPermissaoIncluirPrePedido(PermissaoIncluirPrePedidoRequest request)
        {
            var response = new PermissaoIncluirPrePedidoResponse();

            var permissaoIncluirPrePedido = ValidaPermissao(request.PermissoesUsuario, ePermissao.IncluirPrePedido);

            if (!permissaoIncluirPrePedido)
            {
                response.IncluirPrePedido = false;
                response.Sucesso = false;
                response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
                return response;
            }

            response.IncluirPrePedido = true;
            response.Sucesso = true;
            response.Mensagem = string.Empty;
            return response;
        }

        private TorcamentoCotacao ObterOrcamentoPorIdOrcamento(int idOrcamento)
        {
            TorcamentoCotacao orcamentoCotacao = null;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                orcamentoCotacao = (from o in db.TorcamentoCotacao
                                    where
                                        o.Id == idOrcamento
                                    select o).FirstOrDefault();
            }

            return orcamentoCotacao;
        }

        private Torcamento ObterPrePedidoPorIdPrePedido(string idPrePedido)
        {
            Torcamento orcamento = null;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                orcamento = (from o in db.Torcamento
                             where
                                 o.Orcamento == idPrePedido
                             select o).FirstOrDefault();
            }

            return orcamento;
        }

        private Tpedido ObterPedidoPorIdPedido(string idPedido)
        {
            Tpedido pedido = null;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                pedido = (from o in db.Tpedido
                          where
                              o.Pedido == idPedido
                          select o).FirstOrDefault();
            }

            return pedido;
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

        private bool UsuarioEnvolvidoPrePedido(int idTipoUsuario, int idUsuario, string usuario, string idPrePedido)
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
                    usuarioEnvolvido = (from o in db.Torcamento
                                        where
                                             o.IdIndicadorVendedor == idUsuario
                                             && o.Orcamento == idPrePedido
                                        select o).Any();
                }
            }

            return usuarioEnvolvido;
        }

        private bool UsuarioEnvolvidoPedido(int idTipoUsuario, int idUsuario, string usuario, string idPedido)
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
                    usuarioEnvolvido = (from o in db.Tpedido
                                        where
                                             o.IdIndicadorVendedor == idUsuario
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

        private bool VerificarUnidadeNegocioParametro(string loja, DateTime dataCadastro, int maxPrazoConsulta)
        {
            var valor = string.Empty;
            TimeSpan result;

            using (var db = _contextoProvider.GetContextoLeitura())
            {
                valor = (from o in db.Tloja
                         join u in db.TcfgUnidadeNegocio
                              on o.Unidade_Negocio equals u.Sigla
                         join p in db.TcfgUnidadeNegocioParametro
                              on u.Id equals p.IdCfgUnidadeNegocio
                         where
                              o.Loja == loja
                              && p.IdCfgParametro == maxPrazoConsulta
                         select p.Valor).FirstOrDefault();

                result = DateTime.Now.Date.Subtract(dataCadastro.Date);
            }

            return result.Days > Convert.ToInt32(valor);
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

        private StatusOrcamento ObterStatusOrcamento(TorcamentoCotacao orcamento)
        {
            return (StatusOrcamento)orcamento.Status;
        }
        
        private bool VerificarValidadeOrcamento(TorcamentoCotacao orcamento)
        {
            return orcamento.Validade < DateTime.Now.Date;
        }
    }
}