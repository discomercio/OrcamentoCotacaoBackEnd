using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OrcamentoCotacaoApi.Utils;
using ProdutoCatalogo;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProdutoCatalogoController : ControllerBase
    {
        private readonly ProdutoCatalogoBll bll;
        private readonly IOptions<Configuracoes> _appSettings;

        public ProdutoCatalogoController(ProdutoCatalogoBll arquivoBll, IOptions<Configuracoes> appSettings)
        {
            this.bll = arquivoBll;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(int page, int pageItens, int idCliente)
        {
            var user = User.Identity.Name;
            var tipoUsuario = User.Claims.FirstOrDefault(x => x.Type == "TipoUsuario").Value;

            var saida = await bll.Listar(page, pageItens, idCliente, tipoUsuario, user);

            if (saida.Count > 0)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(string id)
        {
            var saida = await bll.ObterPorId(id);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpGet("{id}/detalhes")]
        public async Task<IActionResult> Detalhes(string id)
        {
            var saida = await bll.Detalhes(id);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(string id)
        {
            var saida = await bll.Excluir(id);

            if (saida)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpDelete("imagem")]
        public async Task<IActionResult> ExcluirImagem(string idProduto, string idImagem)
        {
            var saida = await bll.ExcluirImagem(idProduto, idImagem);

            if (saida)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpGet("{id}/itens")]
        public async Task<IActionResult> ObterListaItens(string id)
        {
            var saida = await bll.ObterListaItens(id);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(TprodutoCatalogo produto)
        {
            var saida = await bll.Atualizar(produto, User.Identity.Name);

            if (saida)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpPost("imagem")]
        public async Task<IActionResult> Upload(IFormFile arquivo, [FromForm] string idProdutoCalatogo)
        {
            try
            {
                if (arquivo.ContentType.Contains("png") || arquivo.ContentType.Contains("jpeg") || arquivo.ContentType.Contains("bmp"))
                {
                    Guid idArquivo = Guid.NewGuid();
                    var extensao = arquivo.FileName.Substring(arquivo.FileName.Length - 3, 3);
                    var file = Path.Combine(_appSettings.Value.ImgCaminho, $"{idArquivo}.{extensao}");
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await arquivo.CopyToAsync(fileStream);
                    }

                    await bll.SalvarArquivo($"{idArquivo}.{extensao}", idProdutoCalatogo, "1", "0");

                    return Ok(new
                    {
                        message = "Arquivo salvo com sucesso.",
                        file
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "Formato inválido. O arquivo deve ser imagem png, jpg ou bmp."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Criar(TprodutoCatalogo produtoCatalogo)
        {
            try
            {
                var usuario = User.Identity.Name;
                var saida = await bll.Criar(produtoCatalogo, usuario);

                if (saida)
                {
                    return Ok(new
                    {
                        message = "Produto criado com sucesso."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "Erro ao criar novo produto."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
