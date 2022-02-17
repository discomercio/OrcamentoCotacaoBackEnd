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

            var saida = bll.PorFiltro(new InfraBanco.Modelos.Filtros.TprodutoCatalogoFiltro()
            {
                Page = page,
                RecordsPerPage = pageItens,
                IdCliente = idCliente,
                TipoUsuario = tipoUsuario,
                Usuario = user
            });

            if (saida.Count > 0)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(string id)
        {
            var saida = bll.PorFiltro(new InfraBanco.Modelos.Filtros.TprodutoCatalogoFiltro() { Id = id });// ObterPorId(id);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpGet("{id}/detalhes")]
        public async Task<IActionResult> Detalhes(int id)
        {
            var saida = bll.Detalhes(id);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            bll.Excluir(new TprodutoCatalogo() { Id = id });

            //if (saida)
            return Ok(true);
            //else
            //    return NotFound();
        }

        [HttpDelete("imagem")]
        public async Task<IActionResult> ExcluirImagem(int idProduto, int idImagem)
        {
            var saida = bll.ExcluirImagem(idProduto, idImagem);

            if (saida)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpGet("{id}/itens")]
        public async Task<IActionResult> ObterListaItens(int id)
        {
            var saida = bll.ObterListaItens(id);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(TprodutoCatalogo produto)
        {
           bll.Atualizar(produto);

            //if (saida)
                return Ok(true);
            //else
            //    return NotFound();
        }

        [HttpPost("imagem")]
        public async Task<IActionResult> Upload(IFormFile arquivo, [FromForm] int idProdutoCalatogo)
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

                    bll.SalvarArquivo($"{idArquivo}.{extensao}", idProdutoCalatogo, 1, "0");

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
                var saida = bll.Criar(produtoCatalogo, usuario);

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
