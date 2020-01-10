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
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarCliente(string cnpj_cpf)
        {
            //para testar: http://localhost:60877/api/cliente/buscarCliente/{cnpj_cpf}
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var dadosCliente = await clienteBll.BuscarCliente(cnpj_cpf, apelido.Trim());
            if (dadosCliente == null)
                return NoContent();
            return Ok(dadosCliente);

        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("atualizarClienteparcial")]
        public async Task<IActionResult> AtualizarClienteParcial(DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            /*
             * somente os seguintes campos serão atualizados:
             * produtor rural
             * inscrição estadual
             * tipo de contibuinte ICMS
             * */
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var retorno = await clienteBll.AtualizarClienteParcial(apelido.Trim(), dadosClienteCadastroDto);
            if (retorno == null)
                return NoContent();
            return Ok(retorno);

        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarBancosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListaBancosCombo()
        {
            //para testar: http://localhost:60877/api/cliente/listarBancosCombo
            var listaBancos = await clienteBll.ListarBancosCombo();

            if (listaBancos == null)
                return BadRequest();

            return Ok(listaBancos);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("cadastrarCliente")]
        public async Task<IActionResult> CadastrarCliente(ClienteCadastroDto clienteDto)
        {
            //para testar: http://localhost:60877/api/cliente/cadastrarCliente
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var retorno = await clienteBll.CadastrarCliente(clienteDto, apelido.Trim());

            return Ok(retorno);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarComboJustificaEndereco")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarComboJustificaEndereco()
        {
            //para testar: http://localhost:60877/api/cliente/listarComboJustificaEndereco
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var retorno = await clienteBll.ListarComboJustificaEndereco(apelido.Trim());

            return Ok(retorno);

        }
    }
}