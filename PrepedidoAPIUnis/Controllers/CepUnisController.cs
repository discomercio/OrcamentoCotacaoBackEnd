using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PrepedidoAPIUnis.Controllers
{
    public class CepUnisController : Controller
    {
        private readonly PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll cepUnisBll;

        public CepUnisController(PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll cepUnisBll)
        {
            this.cepUnisBll = cepUnisBll;
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
        public async Task<IActionResult> BuscarCep(string tokenAcesso, string cep)
        {
            //todo: validar token
            //para buscar apenas por cep
            var ret = await cepUnisBll.BuscarCep(cep);

            return Ok(ret);
        }
    }
}