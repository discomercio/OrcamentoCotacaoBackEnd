using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrcamentoCotacaoApi.Controllers;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using Prepedido.PedidoVisualizacao;
using System;
using System.Threading.Tasks;
using UtilsGlobais;

namespace PrepedidoApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    public class PedidoController : BaseController
    {
        private readonly PedidoVisualizacaoBll pedidoBll;
        private readonly PedidoPrepedidoApiBll pedidoPrepedidoApiBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly PermissaoBll permissaoBll;

        public PedidoController(
            PedidoVisualizacaoBll pedidoBll,
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            PedidoPrepedidoApiBll pedidoPrepedidoApiBll,
            PermissaoBll permissaoBll)
        {
            this.pedidoBll = pedidoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.pedidoPrepedidoApiBll = pedidoPrepedidoApiBll;
            this.permissaoBll = permissaoBll;
        }

        [HttpGet("listarNumerosPedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarNumerosPedidosCombo()
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            return Ok(await pedidoBll.ListarNumerosPedidoCombo(apelido.Trim()));
        }

        [HttpGet("listarPedidos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarPedidos(string clienteBusca, int tipoBusca, string numPedido,
            DateTime? dataInicial, DateTime? dataFinal)
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var ret = await pedidoPrepedidoApiBll.ListarPedidos(
                apelido.Trim(),
                (PedidoVisualizacaoBll.TipoBuscaPedido)tipoBusca,
                clienteBusca,
                numPedido,
                dataInicial,
                dataFinal
                );

            return Ok(ret);
        }

        [HttpGet("listarCpfCnpjPedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarCpfCnpjPedidosCombo()
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            return Ok(await pedidoBll.ListarCpfCnpjPedidosCombo(apelido.Trim()));
        }

        [HttpGet("buscarPedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarPedido(string numPedido)
        {
            var permissao = this.ObterPermissaoPedido(numPedido);

            if (!permissao.VisualizarPedido)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            return Ok(await pedidoPrepedidoApiBll.BuscarPedido(apelido.Trim(), numPedido));
        }

        private PermissaoPedidoResponse ObterPermissaoPedido(string IdPedido)
        {
            return this.permissaoBll.RetornarPermissaoPedido(new PermissaoPedidoRequest()
            {
                IdPedido = IdPedido,
                IdUsuario = LoggedUser.Id,
                PermissoesUsuario = LoggedUser.Permissoes,
                TipoUsuario = LoggedUser.TipoUsuario.Value,
                Usuario = LoggedUser.Apelido
            }).Result;
        }
    }
}