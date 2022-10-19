using Arquivo.Dto;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OrcamentoCotacaoApi.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static OrcamentoCotacaoBusiness.Enums.Enums;
using Microsoft.Extensions.Logging;
using Arquivo.Requests;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            var request = new ArquivoDownloadRequest()
            {
                Id = id,
                Caminho = _appSettings.Value.PdfCaminho
            };

            var response = await _arquivoBll.ArquivoDownload(request);

            Response.Headers.Add("content-disposition", $"filename={response.Nome}.pdf");
            Response.Headers.Add("content-length", response.FileLength);
            Response.Headers.Add("Expires", "0");
            Response.Headers.Add("Pragma", "Cache");
            Response.Headers.Add("Cache-Control", "private");
            Response.ContentType = "application/pdf";

            return await Task.FromResult(new FileContentResult(response.ByteArray, "application/octet-stream"));
        }

        [HttpGet("ObterEstrutura")]
        public async Task<IActionResult> ObterEstrutura()
        {
            var request = new ArquivoObterEstruturaRequest();

            var response = await _arquivoBll.ArquivoObterEstrutura(request);

            return Ok(JsonSerializer.Serialize(new { data = response.Childs }));
            //return Ok(response);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(
            IFormFile arquivo,
            [FromForm] string idPai,
            [FromForm] string descricao)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var request = new ArquivoUploadRequest()
            {
                Arquivo = arquivo,
                Caminho = _appSettings.Value.PdfCaminho,
                IdPai = idPai
            };

            var response = await _arquivoBll.ArquivoUpload(request);

            return Ok(response);
        }

        [HttpPost("CriarPasta")]
        public async Task<IActionResult> CriarPasta(
            string nome, 
            string idPai)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var request = new ArquivoCriarPastaRequest()
            {
                IdPai = idPai,
                Nome = nome
            };

            var response = await _arquivoBll.ArquivoCriarPasta(request);

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
                Descricao = descricao
            };

            var response = await _arquivoBll.ArquivoEditar(request);

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
                Caminho = _appSettings.Value.PdfCaminho
            };

            var response = await _arquivoBll.ArquivoExcluir(request);

            return Ok(response);
        }
    }
}