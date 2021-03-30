using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MagentoBusiness.MagentoBll.AcessoBll;
using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using InfraIdentity.ApiMagento;
using MagentoBusiness.MagentoDto;
using MagentoBusiness.MagentoDto.MarketplaceDto;
using MagentoBusiness.MagentoBll.MagentoBll;
using Microsoft.Extensions.Logging;

namespace ApiMagento.Controllers
{
    [Route("api/pedidoMagento")]
    [ApiController]
    public class PedidoMagentoController : Controller
    {
        private readonly IServicoValidarTokenApiMagento servicoValidarTokenApiMagento;
        private readonly PedidoMagentoBll pedidoMagentoBll;
        private readonly ObterCodigoMarketplaceBll obterCodigoMarketplaceBll;
        private readonly ILogger<PedidoMagentoController> logger;

        public PedidoMagentoController(IServicoValidarTokenApiMagento servicoValidarTokenApiMagento,
            PedidoMagentoBll pedidoMagentoBll,
            MagentoBusiness.MagentoBll.MagentoBll.ObterCodigoMarketplaceBll obterCodigoMarketplaceBll,
            ILogger<PedidoMagentoController> logger)
        {
            this.servicoValidarTokenApiMagento = servicoValidarTokenApiMagento;
            this.pedidoMagentoBll = pedidoMagentoBll;
            this.obterCodigoMarketplaceBll = obterCodigoMarketplaceBll;
            this.logger = logger;
        }

        /// <summary>
        /// Rotina para cadastrar Pedido Magento
        /// </summary>
        /// <param name="pedido">PedidoMagentoDto</param>
        /// <returns>Não retorna dados</returns>
        [AllowAnonymous]
        [HttpPost("cadastrarPedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<PedidoResultadoMagentoDto>> CadastrarPedido(PedidoMagentoDto pedido)
        {
            logger.LogInformation($"CadastrarPedido início - pedido: {System.Text.Json.JsonSerializer.Serialize(pedido)}");

            //em 210115 estava demorando 8,1 segundos
            //em 210120 estava demorando de 10,0 a 11,5 segundos de casa com vpn 
            //em 210126 estava demorando de 13 segundos de casa com vpn, passou para 3,9 por causa do PedidoJaCadastradoDesdeData
            //em 210127 estava demorando de 6,9 segundos de casa com vpn
            //em 210203 estava demorando de 1,3 segundos de casa com vpn - 50 queries ao banco
            //em 210219 estava demorando de 1,5 segundos de casa com vpn
            //em 210305 estava demorando de 1,8 a 32, segundos de casa com vpn - 93 queries - umas 20 a 30 são de endereco_confrontacao
            if (!servicoValidarTokenApiMagento.ValidarToken(pedido.TokenAcesso, out string? usuario))
                return Unauthorized();
            if (string.IsNullOrEmpty(usuario))
                return Unauthorized();

            string apelido = usuario;

            var ret = await pedidoMagentoBll.CadastrarPedidoMagento(pedido, apelido);
            logger.LogInformation($"CadastrarPedido fim - pedido: {System.Text.Json.JsonSerializer.Serialize(pedido)}");
            logger.LogInformation($"CadastrarPedido fim - ret: {System.Text.Json.JsonSerializer.Serialize(ret)}");
            return Ok(ret);
        }

        /// <summary>
        /// Chamada para informar como obter o código do marketplace a partir dos campos do pedido.
        /// No nomento da implementação, sujeito a revisão.
        /// Observar o conteúdo do campo descricao_parametro, ele explica o formato dos dados.
        /// Lembrar do caso da LEROY_MERLIN, codigo 017, a busca deve ser feita em outro campo.
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <returns>MarketplaceResultadoDto</returns>
        [AllowAnonymous]
        [HttpPost("obterCodigoMarketplace")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<MarketplaceResultadoDto>> ObterCodigoMarketplace(string tokenAcesso)
        {
            logger.LogInformation($"ObterCodigoMarketplace início - tokenAcesso: {tokenAcesso}");

            if (!servicoValidarTokenApiMagento.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            var ret = await obterCodigoMarketplaceBll.ObterCodigoMarketplace();
            logger.LogInformation($"ObterCodigoMarketplace fim - tokenAcesso: {tokenAcesso}");
            logger.LogInformation($"ObterCodigoMarketplace fim - ret: {System.Text.Json.JsonSerializer.Serialize(ret)}");
            return Ok(ret);
        }

        /// <summary>
        /// Chamada para alterar o status do pedido cadastrado.
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="pedido_bs_x_ac">Este é o número do pedido no magento (no ASP, é C_numero_magento; precisa ter 9 dígitos)</param>
        /// <param name="statusPedido">APROVADO = 0 (pagamento confirmado),
        /// <br/> NAO_APROVADO = 1 (pedido cadastrado e pagamento não confirmado)</param>
        /// <returns>StatusPedidoResultadoMagentoDto</returns>
        [AllowAnonymous]
        [HttpPost("alterarStatusPedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<StatusPedidoResultadoMagentoDto>> AlterarSatatusPedido(string tokenAcesso, string pedido_bs_x_ac, int statusPedido)
        {
            StatusPedidoResultadoMagentoDto ret = new StatusPedidoResultadoMagentoDto();
            return Ok(await Task.FromResult(ret));
        }
    }
}