using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoUnisBusiness.UnisBll.AcessoBll;
using PrepedidoUnisBusiness.UnisDto.CepUnisDto;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <response code="204">Sem informações para o CEP especificado</response>
        [AllowAnonymous]
        [HttpGet("buscarCep")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<CepUnisDto>> BuscarCep(string tokenAcesso, string cep)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            //para buscar apenas por cep
            CepUnisDto ret = await cepUnisBll.BuscarCep(cep);

            return Ok(ret);
        }


        /// <summary>
        /// Rotina para buscar estados e municípios do IBGE
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="uf">Estado, opcional.</param>
        /// <param name="municipioParcial">Opcional. Retorna todas as cidades que contenha esse texto.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("buscarUfs")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<UFeMunicipiosUnisDto>>> BuscarUfs(string tokenAcesso, string uf, string municipioParcial)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            IEnumerable<UFeMunicipiosUnisDto> ret = await cepUnisBll.BuscarUfs(uf, municipioParcial);

            return Ok(ret);
        }
    }
}