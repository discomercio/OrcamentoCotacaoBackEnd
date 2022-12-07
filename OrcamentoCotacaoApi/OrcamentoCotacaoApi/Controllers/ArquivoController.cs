using Arquivo.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoApi.Utils;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ArquivoController : BaseController
    {
        private readonly ILogger<ArquivoController> _logger;
        private readonly Arquivo.ArquivoBll _arquivoBll;
        private readonly IOptions<Configuracoes> _appSettings;

        public ArquivoController(
            ILogger<ArquivoController> logger,
            Arquivo.ArquivoBll arquivoBll,
            IOptions<Configuracoes> appSettings)
        {
            _logger = logger;
            _arquivoBll = arquivoBll;
            _appSettings = appSettings;
        }

        [HttpGet("ObterEstrutura")]
        public async Task<IActionResult> ObterEstrutura()
        {
            var request = new ArquivoObterEstruturaRequest()
            {
                CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/ObterEstrutura/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoObterEstrutura(request);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/ObterEstrutura/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            var request = new ArquivoDownloadRequest()
            {
                Id = id,
                CaminhoArquivo = _appSettings.Value.PdfCaminho,
                CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/Download/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoDownload(request);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/Download/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        //[HttpPost("Excluir")]
        //public async Task<IActionResult> Excluir(string id, string loja)
        [HttpPost("Excluir")]
        public async Task<IActionResult> Excluir(ArquivoExcluirRequest request)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            if (request == null) return BadRequest(new { message = "Objeto está vazio!" });

            request.CaminhoArquivo = _appSettings.Value.PdfCaminho;
            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.Usuario = LoggedUser.Apelido;
            request.IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/Excluir/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoExcluir(request, LoggedUser);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/Excluir/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        //[HttpPut("Editar")]
        //public async Task<IActionResult> Editar(
        //    string id,
        //    [FromQuery] string nome,
        //    [FromQuery] string descricao)
        [HttpPut("Editar")]
        public async Task<IActionResult> Editar(
            ArquivoEditarRequest request)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.Usuario = LoggedUser?.Apelido;
            request.IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //var request = new ArquivoEditarRequest()
            //{
            //    Id = id,
            //    Nome = nome,
            //    Descricao = descricao,
            //    CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
            //    Usuario = LoggedUser.Apelido,
            //    IP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
            //    Loja = ""
            //};

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/Editar/PUT - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoEditar(request, LoggedUser);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/Editar/PUT - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("CriarPasta")]
        public async Task<IActionResult> CriarPasta(ArquivoCriarPastaRequest request)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.Usuario = LoggedUser.Apelido;
            request.IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/CriarPasta/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoCriarPasta(request, LoggedUser);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/CriarPasta/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile arquivo, IFormCollection form)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var request = new ArquivoUploadRequest()
            {
                IdPai = form["idPai"].ToString(),
                CaminhoArquivo = _appSettings.Value.PdfCaminho,
                Arquivo = arquivo,
                CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
                Usuario = LoggedUser.Apelido,
                IP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                Loja = form["loja"].ToString()
            };

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/Upload/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoUpload(request, LoggedUser);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ArquivoController/Upload/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }
    }
}