using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoUnisBusiness.UnisBll.AcessoBll;
using PrepedidoUnisBusiness.UnisDto.CepUnisDto;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/prepedidoUnis")]
    [ApiController]
    public class CepUnisController : Controller
    {
        private readonly PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll cepUnisBll;
        private readonly IServicoValidarTokenApiUnis servicoValidarTokenApiUnis;

        public CepUnisController(PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll cepUnisBll, IServicoValidarTokenApiUnis servicoValidarTokenApiUnis)
        {
            this.cepUnisBll = cepUnisBll;
            this.servicoValidarTokenApiUnis = servicoValidarTokenApiUnis;
        }

        /// <summary>
        /// Rotina para busca de endereço por Cep
        /// </summary>
        /// <param name="tokenAcesso">utilizado para autenticar usuário</param>
        /// <param name="cep"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("buscarCep")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<CepUnisDto>> BuscarCep(string tokenAcesso, string cep)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out string usuario))
                return Unauthorized();

            //para buscar apenas por cep
            var ret = await cepUnisBll.BuscarCep(cep);

            return Ok(ret);
        }
    }
}