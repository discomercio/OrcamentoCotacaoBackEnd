using Arquivo.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrcamentoCotacaoApi.Utils;
using System.Text.Json;
using System.Threading.Tasks;
using static OrcamentoCotacaoBusiness.Enums.Enums;

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

        [HttpGet("ObterEstrutura")]
        public async Task<IActionResult> ObterEstrutura()
        {
            var request = new ArquivoObterEstruturaRequest();

            var response = await _arquivoBll.ArquivoObterEstrutura(request);
            
            return Ok(response);
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
        
        [HttpPost("CriarPasta")]
        public async Task<IActionResult> CriarPasta(ArquivoCriarPastaRequest request)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var response = await _arquivoBll.ArquivoCriarPasta(request);

            return Ok(response);
        }

        [HttpPost("Upload/{idPai}")]
        public async Task<IActionResult> Upload(string idPai)
        {
            if (!User.ValidaPermissao((int)ePermissao.ArquivosDownloadIncluirEditarPastasArquivos))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var request = new ArquivoUploadRequest()
            {
                Arquivo = Request.Form.Files[0],
                Caminho = _appSettings.Value.PdfCaminho,
                IdPai = idPai
            };

            var response = await _arquivoBll.ArquivoUpload(request);

            return Ok(response);
        }
    }
}