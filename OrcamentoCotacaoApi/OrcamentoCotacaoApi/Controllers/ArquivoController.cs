using Arquivo.Requests;
using Microsoft.AspNetCore.Authorization;
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

            //_logger.LogWarning("ENDPOINT: ObterEstrutura: LogWarning");
            _logger.LogInformation($"ArquivoController/ObterEstrutura/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoObterEstrutura(request);

            _logger.LogInformation($"ArquivoController/ObterEstrutura/GET - Response => [{JsonSerializer.Serialize(response)}].");

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

            _logger.LogInformation($"ArquivoController/Download/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoDownload(request);

            _logger.LogInformation($"ArquivoController/Download/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("Excluir/{id}")]
        public async Task<IActionResult> Excluir(string id)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var request = new ArquivoExcluirRequest()
            {
                Id = id,
                CaminhoArquivo = _appSettings.Value.PdfCaminho,
                CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"ArquivoController/Excluir/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoExcluir(request);

            _logger.LogInformation($"ArquivoController/Excluir/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar(
            string id,
            [FromQuery] string nome,
            [FromQuery] string descricao)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var request = new ArquivoEditarRequest()
            {
                Id = id,
                Nome = nome,
                Descricao = descricao,
                CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"ArquivoController/Editar/PUT - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoEditar(request);

            _logger.LogInformation($"ArquivoController/Editar/PUT - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }
        
        [HttpPost("CriarPasta")]
        public async Task<IActionResult> CriarPasta(ArquivoCriarPastaRequest request)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.Usuario = LoggedUser.Apelido;

            _logger.LogInformation($"ArquivoController/CriarPasta/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _arquivoBll.ArquivoCriarPasta(request);

            _logger.LogInformation($"ArquivoController/CriarPasta/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("Upload/{idPai}")]
        public async Task<IActionResult> Upload(string idPai)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var request = new ArquivoUploadRequest()
            {
                IdPai = idPai,
                CaminhoArquivo = _appSettings.Value.PdfCaminho,
                //Arquivo = Request.Form.Files[0],
                CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"ArquivoController/Upload/POST - Request => [{JsonSerializer.Serialize(request)}].");

            request.Arquivo = Request.Form.Files[0];

            var response = await _arquivoBll.ArquivoUpload(request);

            _logger.LogInformation($"ArquivoController/Upload/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }
    }
}