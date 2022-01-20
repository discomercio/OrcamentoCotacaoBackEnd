using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OrcamentoCotacaoBusiness.Bll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArquivoController : ControllerBase
    {
        private readonly ArquivoBLL arquivoBll;
        private readonly IConfiguration configuration;

        public ArquivoController(IConfiguration configuration, OrcamentoCotacaoBusiness.Bll.ArquivoBLL arquivoBll)
        {
            this.arquivoBll = arquivoBll;
            this.configuration = configuration;
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            //string caminho = Path.Combine(configuration.GetValue(configuration<string>("PdfCaminho")), $"{id}.pdf");
            //FileInfo fileinfo = new FileInfo(caminho);
            //byte[] byteArray = System.IO.File.ReadAllBytes(caminho);
            //var arquivo = await arquivoBll.ObterArquivoPorID(id);
            byte[] byteArray = new byte[0];
            Response.Headers.Add("content-disposition", $"filename=teste.pdf"); //{arquivo.name}
            Response.Headers.Add("content-length", 0.ToString());//fileinfo.Length.ToString());
            Response.Headers.Add("Expires", "0");
            Response.Headers.Add("Pragma", "Cache");
            Response.Headers.Add("Cache-Control", "private");
            Response.ContentType = "application/pdf";

            return await Task.FromResult(new FileContentResult(byteArray, "application/octet-stream"));
        }

        [HttpGet("ObterEstrutura")]
        public IActionResult ObterEstrutura()
        {
            try
            {
                //var data = _arquivoService.ObterEstrutura();

                return Ok(JsonSerializer.Serialize(new { })); //data= data 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            //return Ok();
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile arquivo, [FromForm] string idPai, [FromForm] string descricao)
        {
            Guid idArquivo = Guid.NewGuid();

            if (!arquivo.ContentType.Equals("application/pdf") && !arquivo.ContentType.Equals("pdf"))
            {
                return BadRequest(new
                {
                    message = "Formato inválido. O arquivo deve ser no formato PDF."
                });
            }

            try
            {
                var file = Path.Combine(configuration.GetValue<string>("PdfCaminho"), $"{idArquivo}.pdf");
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await arquivo.CopyToAsync(fileStream);
                }

                var tamanho = new FileInfo(file).Length;

                //await _arquivoService.SalvarArquivo(idArquivo, arquivo.FileName, idPai, descricao, tamanho);

                return Ok(new
                {
                    message = "Arquivo salvo com sucesso.",
                    file
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CriarPasta")]
        public async Task<IActionResult> CriarPasta(string nome, string idPai)
        {
            Guid id = Guid.NewGuid();

            //await _arquivoService.CriarPasta(id, nome, idPai);

            return Ok(new
            {
                message = $"Pasta '{nome}' criada com sucesso."
            });
        }
    }
}
