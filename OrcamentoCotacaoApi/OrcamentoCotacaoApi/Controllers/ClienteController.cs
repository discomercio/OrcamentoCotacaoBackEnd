using Azure;
using InfraBanco.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Controllers;
using OrcamentoCotacaoApi.Filters;
using Prepedido.Bll;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais;
using UtilsGlobais.Configs;

namespace PrepedidoApi.Controllers
{
    [TypeFilter(typeof(ControleDelayFilter))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    [TypeFilter(typeof(ResourceFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ClienteController : BaseController
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly ClientePrepedidoBll clientePrepedidoBll;

        public ClienteController(
            ILogger<ClienteController> logger,
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            ClientePrepedidoBll clientePrepedidoBll)
        {
            this._logger = logger;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.clientePrepedidoBll = clientePrepedidoBll;
        }

        [HttpGet("buscarCliente/{cnpj_cpf}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarCliente(string cnpj_cpf)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido, CnpjCpf = cnpj_cpf };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/BuscarCliente/GET - Request => [{JsonSerializer.Serialize(request)}].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            ClienteCadastroDto dadosCliente = await clientePrepedidoBll.BuscarCliente(cnpj_cpf, apelido.Trim());

            if (dadosCliente == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/BuscarCliente/GET - Response => [Não tem response].");
                return NoContent();
            }

            var response = new
            {
                DadosCliente = dadosCliente.DadosCliente,
                RefBancaria = dadosCliente.RefBancaria.Count,
                RefComercial = dadosCliente.RefComercial.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/BuscarCliente/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(dadosCliente);
        }

        [HttpPost("atualizarClienteparcial")]
        public async Task<IActionResult> AtualizarClienteParcial(DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido, Dto = dadosClienteCadastroDto };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/AtualizarClienteParcial/POST - Request => [{JsonSerializer.Serialize(request)}].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            List<string> retorno = await clientePrepedidoBll.AtualizarClienteParcial(apelido.Trim(), dadosClienteCadastroDto,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS);

            if (retorno == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/AtualizarClienteParcial/POST - Response => [Não tem response].");
                return NoContent();
            }

            var response = new
            {
                Retorno = retorno.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/AtualizarClienteParcial/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(retorno);
        }

        [HttpGet("listarBancosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListaBancosCombo()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/ListaBancosCombo/GET - Request => [{JsonSerializer.Serialize(request)}].");

            IEnumerable<ListaBancoDto> listaBancos = await clientePrepedidoBll.ListarBancosCombo();

            if (listaBancos == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/ListaBancosCombo/GET - Response => [Não tem response].");
                return BadRequest();
            }

            var response = new
            {
                ListaBancos = listaBancos.Count()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/ListaBancosCombo/GET  - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(listaBancos);
        }

        [HttpPost("cadastrarCliente")]
        public async Task<IActionResult> CadastrarCliente(ClienteCadastroDto clienteDto)
        {
            try
            {
                var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

                var request = new
                {
                    Usuario = LoggedUser.Apelido,
                    DadosCliente = clienteDto.DadosCliente,
                    RefBancaria = clienteDto.RefBancaria.Count,
                    RefComercial = clienteDto.RefComercial.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/CadastrarCliente/POST - Request => [{JsonSerializer.Serialize(request)}].");

                string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

                var cliente = await clientePrepedidoBll.CadastrarCliente(clienteDto, apelido.Trim());

                var response = new
                {
                    Clientes = cliente
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/CadastrarCliente/POST - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(cliente);
            }
            catch (Exception e)
            {
                e.Data.Add("params", clienteDto);
                throw e;
            }
        }

        [HttpGet("listarComboJustificaEndereco")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarComboJustificaEndereco()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/ListarComboJustificaEndereco/GET - Request => [{JsonSerializer.Serialize(request)}].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var justificaEnderecos = await clientePrepedidoBll.ListarComboJustificaEndereco(apelido.Trim());

            var response = new
            {
                JustificaEnderecos = justificaEnderecos.Count()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ClienteController/ListarComboJustificaEndereco/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(justificaEnderecos);
        }
    }
}