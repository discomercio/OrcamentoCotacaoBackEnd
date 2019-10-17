using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class CepController : Controller
    {
        private readonly PrepedidoBusiness.Bll.CepBll cepBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;

        public CepController(PrepedidoBusiness.Bll.CepBll cepBll, InfraIdentity.IServicoDecodificarToken servicoDecodificarToken)
        {
            this.cepBll = cepBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarCep")]
        public async Task<IActionResult> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            //para testar: http://localhost:60877/api/cep/buscarCep
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var ret = await cepBll.BuscarCep(cep, endereco, uf, cidade);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarUfs")]
        public async Task<IActionResult> BuscarUfs()
        {
            //para testar: http://localhost:60877/api/cep/buscarUfs
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var ret = await cepBll.BuscarUfs();

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarCepPorEndereco")]
        public async Task<IActionResult> BuscarCepPorEndereco(string endereco, string localidade, string uf)
        {
            //para testar: http://localhost:60877/api/cep/buscarCepPorEndereco
            //string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            //esse metodo esta buscando apenas 300 itens
            var ret = await cepBll.BuscarCepPorEndereco(endereco, localidade, uf);

            return Ok(ret);
        }
    }
}