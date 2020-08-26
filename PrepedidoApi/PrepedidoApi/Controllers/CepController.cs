using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoBusiness.Bll;
using PrepedidoBusiness.Dto.Cep;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class CepController : Controller
    {
        private readonly PrepedidoBusiness.Bll.CepPrepedidoBll cepPrepedidoBll;
        private readonly CepBll cepBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;

        public CepController(PrepedidoBusiness.Bll.CepPrepedidoBll cepPrepedidoBll, PrepedidoBusiness.Bll.CepBll cepBll, InfraIdentity.IServicoDecodificarToken servicoDecodificarToken)
        {
            this.cepPrepedidoBll = cepPrepedidoBll;
            this.cepBll = cepBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarCep")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            //para testar: http://localhost:60877/api/cep/buscarCep
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            IEnumerable<CepDto> ret = await cepPrepedidoBll.BuscarCep(cep, endereco, uf, cidade);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarUfs")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<string>>> BuscarUfs()
        {
            //para testar: http://localhost:60877/api/cep/buscarUfs
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            IEnumerable<string> ret = await cepBll.BuscarUfs();

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarCepPorEndereco")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCepPorEndereco(string endereco, string localidade, string uf)
        {
            //para testar: http://localhost:60877/api/cep/buscarCepPorEndereco
            //string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            //esse metodo esta buscando apenas 300 itens
            IEnumerable<CepDto> ret = await cepPrepedidoBll.BuscarCepPorEndereco(endereco, localidade, uf);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarLocalidades")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<string>>> BuscarLocalidades(string uf)
        {
            //para testar: http://localhost:60877/api/cep/buscarCidades

            IEnumerable<string> ret = await cepBll.BuscarLocalidades(uf);

            return Ok(ret);
        }
    }
}