using FormaPagamento;
using InfraBanco.Constantes;
using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Controllers;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
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
    [TypeFilter(typeof(ResourceFilter))]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    public class PrepedidoController : BaseController
    {
        private readonly ILogger<PrepedidoController> _logger;
        private readonly PrepedidoBll prepedidoBll;
        private readonly PrepedidoApiBll prepedidoApiBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly FormaPagtoBll formaPagtoBll;
        private readonly FormaPagtoPrepedidoBll formaPagtoPrepedidoBll;
        private readonly CoeficientePrepedidoBll coeficientePrepedidoBll;
        private readonly IConfiguration configuration;
        private readonly PermissaoBll permissaoBll;

        public PrepedidoController(
            ILogger<PrepedidoController> logger,
            PrepedidoBll prepedidoBll,
            Prepedido.Bll.PrepedidoApiBll prepedidoApiBll,
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            FormaPagtoBll formaPagtoBll,
            Prepedido.Bll.FormaPagtoPrepedidoBll formaPagtoPrepedidoBll,
            Prepedido.Bll.CoeficientePrepedidoBll coeficientePrepedidoBll,
            IConfiguration configuration,
            PermissaoBll permissaoBll)
        {
            this._logger = logger;
            this.prepedidoBll = prepedidoBll;
            this.prepedidoApiBll = prepedidoApiBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.formaPagtoBll = formaPagtoBll;
            this.formaPagtoPrepedidoBll = formaPagtoPrepedidoBll;
            this.coeficientePrepedidoBll = coeficientePrepedidoBll;
            this.configuration = configuration;
            this.permissaoBll = permissaoBll;
        }

        [HttpGet("listarNumerosPrepedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarNumerosPrepedidosCombo()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/ListarNumerosPrepedidosCombo/GET - Request => [{JsonSerializer.Serialize(request)}].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var response = await prepedidoBll.ListarNumerosPrepedidosCombo(apelido);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/ListarNumerosPrepedidosCombo/GET - Response => [{JsonSerializer.Serialize(response.Count())}].");

            return Ok(response);
        }

        [HttpGet("listarCpfCnpjPrepedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarCpfCnpjPrepedidosCombo()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/ListarCpfCnpjPrepedidosCombo/GET - Request => [{JsonSerializer.Serialize(request)}].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var response = await prepedidoBll.ListarCpfCnpjPrepedidosCombo(apelido);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/ListarCpfCnpjPrepedidosCombo/GET - Response => [{JsonSerializer.Serialize(response.Count())}].");

            return Ok(response);
        }

        [HttpGet("listarPrePedidos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarPrePedidos(
            int tipoBusca, 
            string clienteBusca, 
            string numeroPrePedido,
            DateTime? dataInicial, 
            DateTime? dataFinal)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                TipoBusca = tipoBusca,
                ClienteBusca = clienteBusca,
                NumeroPrePedido = numeroPrePedido,
                DataInicial = dataInicial,
                DataFinal = dataFinal
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/ListarPrePedidos/GET - Request => [{JsonSerializer.Serialize(request)}].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var lista = await prepedidoApiBll.ListarPrePedidos(
                apelido,
                (PrepedidoBll.TipoBuscaPrepedido)tipoBusca,
                clienteBusca, 
                numeroPrePedido, 
                dataInicial, 
                dataFinal);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/ListarPrePedidos/GET - Response => [{JsonSerializer.Serialize(lista.Count())}].");

            return Ok(lista);
        }

        [HttpPost("removerPrePedido/{numeroPrePedido}")]
        public async Task<IActionResult> RemoverPrePedido(string numeroPrePedido)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                NumeroPrePedido = numeroPrePedido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/RemoverPrePedido/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var permissao = this.ObterPermissaoPrePedido(numeroPrePedido);

            if (!permissao.CancelarPrePedido)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            bool ret = await prepedidoBll.RemoverPrePedido(numeroPrePedido, apelido);

            if (ret)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/RemoverPrePedido/POST - Response => [PrePedido removido.].");
                return Ok();
            }

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/RemoverPrePedido/POST - Response => [PrePedido não foi removido.].");
            return NotFound();
        }

        [HttpGet("buscarPrepedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarPrePedido(string numPrepedido)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                NumeroPrePedido = numPrepedido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarPrePedido/GET - Request => [{JsonSerializer.Serialize(request)}].");


            var permissao = this.ObterPermissaoPrePedido(numPrepedido);

            if (!permissao.VisualizarPrePedido)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var response = await prepedidoApiBll.BuscarPrePedido(apelido, numPrepedido);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarPrePedido/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet("obtemPercentualVlPedidoRA")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ObtemPercentualVlPedidoRA()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/ObtemPercentualVlPedidoRA/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await prepedidoBll.ObtemPercentualVlPedidoRA();

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarPrePedido/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet("obter_permite_ra_status")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Obter_Permite_RA_Status()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/Obter_Permite_RA_Status/GET - Request => [{JsonSerializer.Serialize(request)}].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var response = await prepedidoBll.Obter_Permite_RA_Status(apelido);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/Obter_Permite_RA_Status/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("cadastrarPrepedido")]
        public async Task<IActionResult> CadastrarPrepedido(PrePedidoDto prePedido)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                PrePedido = prePedido,
                IP = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/CadastrarPrepedido/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var permissao = this.ObterPermissaoInclusaoPrePedido();

            if (!permissao.IncluirPrePedido)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Configuracao>();
            //LIMITE_ARREDONDAMENTO_PRECO_VENDA_ORCAMENTO_ITEM fixo em 1 centavo

            var usuario = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            IEnumerable<string> ret = await prepedidoApiBll
                .CadastrarPrepedido(
                prePedido,
                apelido.Trim(),
                0.01M,
                appSettings.VerificarPrepedidoRepetido,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO,
                appSettings.LimiteItens,
                (Constantes.TipoUsuarioContexto)usuario.TipoUsuario,
                usuario.Id,
                request.IP);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/CadastrarPrepedido/POST - Response => [{JsonSerializer.Serialize(ret)}].");

            return Ok(ret);
        }

        [HttpPost("deletarPrepedido")]
        public async Task<IActionResult> DeletarPrePedido(PrePedidoDto prePedido)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                NumeroPrePedido = prePedido.NumeroPrePedido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/DeletarPrePedido/POST - Request => [{JsonSerializer.Serialize(request)}].");


            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            await prepedidoApiBll.DeletarOrcamentoExisteComTransacao(prePedido, apelido.Trim());

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/CadastrarPrepedido/POST - Response => [Não tem response].");

            return Ok();
        }

        [HttpGet("buscarFormasPagto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarFormasPagto(string tipo_pessoa)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                TipoPessoa = tipo_pessoa
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarFormasPagto/GET - Request => [{JsonSerializer.Serialize(request)}].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var response = await formaPagtoPrepedidoBll.ObterFormaPagto(apelido.Trim(), tipo_pessoa);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarFormasPagto/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("buscarCoeficiente")]
        public async Task<IActionResult> BuscarCoeficiente(
            List<Prepedido.Dto.PrepedidoProdutoDtoPrepedido> lstProdutos)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                LstProdutos = lstProdutos.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarCoeficiente/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await coeficientePrepedidoBll.BuscarListaCoeficientes(lstProdutos);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarCoeficiente/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("buscarCoeficienteFornecedores")]
        public async Task<IActionResult> BuscarCoeficienteFornecedores(List<string> lstFornecedores)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                LstFornecedores = lstFornecedores.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarCoeficienteFornecedores/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await coeficientePrepedidoBll.BuscarListaCoeficientesFornecedores(lstFornecedores);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarCoeficienteFornecedores/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet("buscarQtdeParcCartaoVisa")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarQtdeParcCartaoVisa()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarQtdeParcCartaoVisa/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await formaPagtoBll.BuscarQtdeParcCartaoVisa();

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PrepedidoController/BuscarQtdeParcCartaoVisa/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        private PermissaoPrePedidoResponse ObterPermissaoPrePedido(string IdPrePedido)
        {
            return this.permissaoBll.RetornarPermissaoPrePedido(new PermissaoPrePedidoRequest()
            {
                IdPrePedido = IdPrePedido,
                IdUsuario = LoggedUser.Id,
                PermissoesUsuario = LoggedUser.Permissoes,
                TipoUsuario = LoggedUser.TipoUsuario.Value,
                Usuario = LoggedUser.Apelido
            }).Result;
        }

        private PermissaoIncluirPrePedidoResponse ObterPermissaoInclusaoPrePedido()
        {
            return this.permissaoBll.RetornarPermissaoIncluirPrePedido(new PermissaoIncluirPrePedidoRequest()
            {
                PermissoesUsuario = LoggedUser.Permissoes
            }).Result;
        }
    }
}