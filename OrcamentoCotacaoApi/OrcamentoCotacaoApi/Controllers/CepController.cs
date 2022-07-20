using Cep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoBusiness.Dto.Cep;
using System.Collections.Generic;
using System.Threading.Tasks;
using UtilsGlobais;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    public class CepController : Controller
    {
        private readonly PrepedidoBusiness.Bll.CepPrepedidoBll cepPrepedidoBll;
        private readonly CepBll cepBll;

        public CepController(
            PrepedidoBusiness.Bll.CepPrepedidoBll cepPrepedidoBll, 
            Cep.CepBll cepBll)
        {
            this.cepPrepedidoBll = cepPrepedidoBll;
            this.cepBll = cepBll;
        }

        [HttpGet("buscarCep")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            return Ok(await cepPrepedidoBll.BuscarCep(cep, endereco, uf, cidade));
        }

        [HttpGet("buscarUfs")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<string>>> BuscarUfs()
        {
            return Ok(await cepBll.BuscarUfs());
        }

        [HttpGet("buscarCepPorEndereco")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCepPorEndereco(string endereco, string localidade, string uf)
        {
            return Ok(await cepPrepedidoBll.BuscarCepPorEndereco(endereco, localidade, uf));
        }

        [HttpGet("buscarLocalidades")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<string>>> BuscarLocalidades(string uf)
        {
            return Ok(await cepBll.BuscarLocalidades(uf));
        }
    }
}