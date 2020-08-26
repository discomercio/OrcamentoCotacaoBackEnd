using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PrepedidoBusiness.Dto.ClienteCadastro;
using InfraBanco.Constantes;
using PrepedidoBusiness.Bll;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class ClienteController : ControllerBase
    {
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly ClientePrepedidoBll clientePrepedidoBll;

        public ClienteController(InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            ClientePrepedidoBll clientePrepedidoBll)
        {
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.clientePrepedidoBll = clientePrepedidoBll;
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
            ClienteCadastroDto dadosCliente = await clientePrepedidoBll.BuscarCliente(cnpj_cpf, apelido.Trim());

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
            List<string> retorno = await clientePrepedidoBll.AtualizarClienteParcial(apelido.Trim(), dadosClienteCadastroDto);
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
            IEnumerable<ListaBancoDto> listaBancos = await clientePrepedidoBll.ListarBancosCombo();

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

            IEnumerable<string> retorno = await clientePrepedidoBll.CadastrarCliente(clienteDto, apelido.Trim());

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
            IEnumerable<EnderecoEntregaJustificativaDto> retorno = await clientePrepedidoBll.ListarComboJustificaEndereco(apelido.Trim());

            return Ok(retorno);

        }
    }
}