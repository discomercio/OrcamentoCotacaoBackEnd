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

namespace ApiMagento.Controllers
{
    [Route("api/pedidoMagento")]
    [ApiController]
    public class PedidoMagentoController : Controller
    {
        private readonly IServicoValidarTokenApiMagento servicoValidarTokenApiMagento;
        private readonly PedidoMagentoBll pedidoMagentoBll;
        private readonly ObterCodigoMarketplaceBll obterCodigoMarketplaceBll;

        public PedidoMagentoController(IServicoValidarTokenApiMagento servicoValidarTokenApiMagento,
            PedidoMagentoBll pedidoMagentoBll, MagentoBusiness.MagentoBll.MagentoBll.ObterCodigoMarketplaceBll obterCodigoMarketplaceBll)
        {
            this.servicoValidarTokenApiMagento = servicoValidarTokenApiMagento;
            this.pedidoMagentoBll = pedidoMagentoBll;
            this.obterCodigoMarketplaceBll = obterCodigoMarketplaceBll;
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
            //em 210115 estava demorando 8,1 segundos
            //em 210120 estava demorando de 10,0 a 11,5 segundos de casa com vpn 
            if (!servicoValidarTokenApiMagento.ValidarToken(pedido.TokenAcesso, out string? usuario))
                return Unauthorized();
            if (string.IsNullOrEmpty(usuario))
                return Unauthorized();

            string apelido = usuario;

            var ret = await pedidoMagentoBll.CadastrarPedidoMagento(pedido, apelido);
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
            if (!servicoValidarTokenApiMagento.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            var ret = await obterCodigoMarketplaceBll.ObterCodigoMarketplace();

            return Ok(ret);
        }
    }
}