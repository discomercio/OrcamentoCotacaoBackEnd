using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MagentoBusiness.MagentoBll.AcessoBll;
using MagentoBusiness.MagentoBll.PedidoMagentoBll;
using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using InfraIdentity.ApiMagento;
using MagentoBusiness.MagentoDto;
using MagentoBusiness.MagentoDto.MarketplaceDto;

namespace ApiMagento.Controllers
{
    [Route("api/pedidoMagento")]
    [ApiController]
    public class PedidoMagentoController : Controller
    {
        private readonly IServicoValidarTokenApiMagento servicoValidarTokenApiMagento;
        private readonly PedidoMagentoBll pedidoMagentoBll;
        private readonly IServicoDecodificarTokenApiMagento servicoDecodificarTokenApiMagento;

        public PedidoMagentoController(IServicoValidarTokenApiMagento servicoValidarTokenApiMagento,
            PedidoMagentoBll pedidoMagentoBll, IServicoDecodificarTokenApiMagento servicoDecodificarTokenApiMagento)
        {
            this.servicoValidarTokenApiMagento = servicoValidarTokenApiMagento;
            this.pedidoMagentoBll = pedidoMagentoBll;
            this.servicoDecodificarTokenApiMagento = servicoDecodificarTokenApiMagento;
        }

        /// <summary>
        /// Rotina para cadastrar Pedido Magento
        /// </summary>
        /// <param name="pedido">PedidoMagentoDto</param>
        /// <returns>Não retorna dados</returns>
        [AllowAnonymous]
        [HttpPost("cadastrarPedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult<PedidoResultadoMagentoDto>> CadastrarPrepedido(PedidoMagentoDto pedido)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!servicoValidarTokenApiMagento.ValidarToken(pedido.TokenAcesso, out string usuario))
                return Unauthorized();

            //string apelido = servicoDecodificarTokenApiMagento.ObterApelidoOrcamentista(User);
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
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult<MarketplaceResultadoDto>> ObterCodigoMarketplace(string tokenAcesso)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!servicoValidarTokenApiMagento.ValidarToken(tokenAcesso, out _))
                return Unauthorized();
            
            var ret = await pedidoMagentoBll.ObterCodigoMarketplace();
            
            return Ok(ret);
        }
    }
}