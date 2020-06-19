using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoApiUnisBusiness;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoUnisBusiness.UnisBll.AcessoBll;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/clienteUnis")]
    [ApiController]
    public class ClienteUnisController : Controller
    {
        private readonly ClienteUnisBll clienteUnisBll;
        private readonly IServicoValidarTokenApiUnis servicoValidarTokenApiUnis;

        public ClienteUnisController(ClienteUnisBll clienteUnisBll, IServicoValidarTokenApiUnis servicoValidarTokenApiUnis)
        {
            this.clienteUnisBll = clienteUnisBll;
            this.servicoValidarTokenApiUnis = servicoValidarTokenApiUnis;
        }


        /// <summary>
        /// Rotina para cadastrar um novo cliente
        /// </summary>
        /// <param name="clienteDto">ClienteCadastroUnisDto</param>
        /// <returns>ClienteCadastroResultadoUnisDto</returns>
        [AllowAnonymous]
        [HttpPost("cadastrarCliente")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<ClienteCadastroResultadoUnisDto>> CadastrarCliente(ClienteCadastroUnisDto clienteDto)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(clienteDto.TokenAcesso, out string usuario))
                return Unauthorized();

            ClienteCadastroResultadoUnisDto retorno;
            //todo: retornar a estrutura certa
            retorno = await clienteUnisBll.CadastrarClienteUnis(clienteDto);

            return Ok(retorno);
        }

        /// <summary>
        /// Rotina para buscar dados do cliente por CPF ou CNPJ
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="cnpj_cpf_cliente">CPF ou CNPJ, somente dígitos</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("buscarCliente")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<ClienteBuscaRetornoUnisDto>> BuscarCliente(string tokenAcesso, 
            string cnpj_cpf_cliente)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out string usuario))
                return Unauthorized();

            var dadosCliente = await clienteUnisBll.BuscarCliente(cnpj_cpf_cliente, "");

            if (dadosCliente == null)
                return NoContent();
            return Ok(dadosCliente);
        }

    }
}