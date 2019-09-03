using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PrepedidoBusiness.Dtos.ClienteCadastro;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class ClienteController : ControllerBase
    {
        private readonly PrepedidoBusiness.Bll.ClienteBll clienteBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;

        public ClienteController(PrepedidoBusiness.Bll.ClienteBll clienteBll, InfraIdentity.IServicoDecodificarToken servicoDecodificarToken)
        {
            this.clienteBll = clienteBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarCliente/{cnpj_cpf}")]
        public async Task<IActionResult> BuscarCliente(string cnpj_cpf)
        {
            //para testar: http://localhost:60877/api/cliente/buscarCliente/{cnpj_cpf}
            var dadosCliente = await clienteBll.BuscarCliente(cnpj_cpf);
            if (dadosCliente == null)
                return NoContent();
            return Ok(dadosCliente);

        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("atualizarCliente")]
        public async Task<IActionResult> AtualizarCliente(DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var retorno = await clienteBll.AtualizarCliente(apelido, dadosClienteCadastroDto);
            if (retorno == null)
                return NoContent();
            return Ok(retorno);

        }

    }
}