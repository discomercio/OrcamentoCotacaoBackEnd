using Arquivo.Dto;
using InfraBanco.Modelos;
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

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArquivoController : BaseController
    {
        private readonly Arquivo.ArquivoBll arquivoBll;
        private readonly IOptions<Configuracoes> _appSettings;

        public ArquivoController(Arquivo.ArquivoBll arquivoBll, IOptions<Configuracoes> appSettings)
        {
            this.arquivoBll = arquivoBll;
            _appSettings = appSettings;
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            try
            {
                string caminho = Path.Combine(_appSettings.Value.PdfCaminho, $"{id}.pdf");
                FileInfo fileinfo = new FileInfo(caminho);
                byte[] byteArray = System.IO.File.ReadAllBytes(caminho);
                var arquivo = arquivoBll.ObterArquivoPorID(Guid.Parse(id));
                Response.Headers.Add("content-disposition", $"filename={arquivo.Nome}.pdf");
                Response.Headers.Add("content-length", fileinfo.Length.ToString());
                Response.Headers.Add("Expires", "0");
                Response.Headers.Add("Pragma", "Cache");
                Response.Headers.Add("Cache-Control", "private");
                Response.ContentType = "application/pdf";

                return await Task.FromResult(new FileContentResult(byteArray, "application/octet-stream"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        string calculaTamanho(long tamanhoBytes)
        {
            string sOut = "";
            decimal saida = 0;

            if (tamanhoBytes / 1024 <= 1024)
            {
                saida = (decimal)tamanhoBytes / 1024;
                sOut = $"{saida.ToString("F0")}kb";
            }
            else
            {
                saida = (decimal)tamanhoBytes / 1024 / 1024;
                sOut = $"{saida.ToString("F")}mb";
            }

            return sOut;
        }

        [HttpGet("ObterEstrutura")]
        public IActionResult ObterEstrutura()
        {
            try
            {
                List<TorcamentoCotacaoArquivos> lista = arquivoBll.ObterEstrutura();
                var root = lista.Where(x => x.Pai == null).FirstOrDefault();
                var data = new List<Child>{
                new Child
                {
                    data = new Data {
                        key = root.Id.ToString(),
                        name = root.Nome,
                        type = "Folder",
                        size = root.Tamanho,
                        descricao = root.Descricao
                    },
                    children = lista.Where(x => x.Pai == root.Id)
                                .Select(c => new Child
                                {
                                    data = new Data
                                    {
                                        key = c.Id.ToString(),
                                        name = c.Nome,
                                        type = c.Tipo,
                                        size = c.Tamanho,
                                        descricao = c.Descricao
                                    },
                                    children = lista.Where(x => x.Pai == c.Id)
                                                .Select(d => new Child
                                                {
                                                    data = new Data
                                                    {
                                                        key = d.Id.ToString(),
                                                        name = d.Nome,
                                                        type = d.Tipo,
                                                        size = d.Tamanho,
                                                        descricao = d.Descricao
                                                    },
                                                    children = lista.Where(x => x.Pai == d.Id)
                                                                .Select(e => new Child
                                                                {
                                                                    data = new Data
                                                                    {
                                                                        key = e.Id.ToString(),
                                                                        name = e.Nome,
                                                                        type = e.Tipo,
                                                                        size = e.Tamanho,
                                                                        descricao = e.Descricao
                                                                    },
                                                                }).ToList()
                                                }).ToList()
                                }).ToList()
                }
            };
                return Ok(JsonSerializer.Serialize(new { data = data }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
                var file = Path.Combine(_appSettings.Value.PdfCaminho, $"{idArquivo}.pdf");
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await arquivo.CopyToAsync(fileStream);
                }

                var tamanho = new FileInfo(file).Length;

                //await _arquivoService.SalvarArquivo(idArquivo, arquivo.FileName, idPai, descricao, tamanho);
                arquivoBll.Inserir(new TorcamentoCotacaoArquivos()
                {
                    Id = idArquivo,
                    Nome = arquivo.FileName,
                    Pai = !string.IsNullOrEmpty(idPai) ? Guid.Parse(idPai) : (Guid?)null,
                    Descricao = "",
                    Tamanho = calculaTamanho(tamanho),
                    Tipo = "File"
                });

                return Ok(new
                {
                    id = idArquivo,
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
        public IActionResult CriarPasta(string nome, string idPai)
        {
            Guid id = Guid.NewGuid();

            var saida = arquivoBll.Inserir(new TorcamentoCotacaoArquivos()
            {
                Id = Guid.NewGuid(),
                Nome = nome,
                Pai = !string.IsNullOrEmpty(idPai) ? Guid.Parse(idPai) : (Guid?)null,
                Descricao = nome,
                Tamanho = "",
                Tipo = "Folder"
            });

            return Ok(new
            {
                id = saida.Id,
                message = $"Pasta '{nome}' criada com sucesso."
            });
        }

        [HttpPut("Editar")]
        public IActionResult Editar(string id, [FromQuery] string nome, [FromQuery] string descricao)
        {
            var retorno = arquivoBll.Editar(new TorcamentoCotacaoArquivos
            {
                Id = Guid.Parse(id),
                Nome = nome,
                Descricao = descricao
            });

            if (retorno)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new
                {
                    message = $"Ocorreu um erro!"
                });
            }
        }

        [HttpPost("Excluir/{id}")]
        public IActionResult Excluir(string id)
        {
            var retorno = arquivoBll.Excluir(new TorcamentoCotacaoArquivos
            {
                Id = Guid.Parse(id)
            });

            if (retorno)
            {
                return Ok(retorno);
            }
            else
            {
                return BadRequest(new
                {
                    message = $"Erro ao excluir!"
                });
            }
        }
    }
}
