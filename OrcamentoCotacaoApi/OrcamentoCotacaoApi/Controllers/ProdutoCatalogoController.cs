using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProdutoCatalogoController : BaseController
    {
        private readonly ILogger<ProdutoCatalogoController> _logger;
        private readonly ProdutoCatalogoOrcamentoCotacaoBll _bll;
        private readonly IOptions<Configuracoes> _appSettings;
        private readonly ProdutoOrcamentoCotacaoBll _produtoOrcamentoCotacaoBll;

        public ProdutoCatalogoController(
            ILogger<ProdutoCatalogoController> logger,
            ProdutoCatalogoOrcamentoCotacaoBll bll, 
            IOptions<Configuracoes> appSettings,
            ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll)
        {
            _logger = logger;
            _bll = bll;
            _appSettings = appSettings;
            _produtoOrcamentoCotacaoBll = produtoOrcamentoCotacaoBll;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(int page, int pageItens, int idCliente)
        {
            var saida = _bll.PorFiltro(new InfraBanco.Modelos.Filtros.TprodutoCatalogoFiltro()
            {
                Page = page,
                RecordsPerPage = pageItens,
                IdCliente = idCliente,
                TipoUsuario = LoggedUser.TipoUsuario?.ToString(),
                Usuario = LoggedUser.Apelido,
                IncluirImagem = true
            });

            if (saida.Count > 0)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("ativos")]
        public async Task<IActionResult> ListarAtivos(int page, int pageItens, int idCliente)
        {
            var saida = _bll.PorFiltro(new InfraBanco.Modelos.Filtros.TprodutoCatalogoFiltro()
            {
                Page = page,
                RecordsPerPage = pageItens,
                IdCliente = idCliente,
                TipoUsuario = LoggedUser.TipoUsuario?.ToString(),
                Usuario = LoggedUser.Apelido,
                Ativo = true
            });

            if (saida.Count > 0)
                return Ok(saida);
            else
                return NoContent();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(string id)
        {
            var saida = _bll.PorFiltro(new InfraBanco.Modelos.Filtros.TprodutoCatalogoFiltro() { Id = id });

            if (saida != null && saida.Count > 0)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> ObterPorCodigo(string codigo)
        {
            var saida = _bll.PorFiltro(new InfraBanco.Modelos.Filtros.TprodutoCatalogoFiltro() { Produto = codigo });

            if (saida != null && saida.Count > 0)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("{id}/detalhes")]
        public async Task<IActionResult> Detalhes(int id)
        {
            var saida = _bll.Detalhes(id);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            _bll.Excluir(id);

            //if (saida)
            return Ok(true);
            //else
            //    return NotFound();
        }

        [HttpDelete("imagem")]
        public async Task<IActionResult> ExcluirImagem(int idProduto, int idImagem)
        {
            var retorno = _bll.ExcluirImagem(idProduto, idImagem, _appSettings.Value.ImgCaminho);

            if (retorno != null)
                return BadRequest(new { message = retorno });

            return Ok();
        }

        [HttpGet("{id}/itens")]
        public async Task<IActionResult> ObterListaItens(int id)
        {
            var saida = _bll.ObterListaItens(id);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpGet("{id}/imagem")]
        public async Task<IActionResult> ObterImagem(int id)
        {
            var saida = _bll.ObterListaImagensPorId(id);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpGet("imagem/{produto}")]
        public async Task<IActionResult> ObterDadosImagemPorProduto(string produto)
        {
            var saida = _bll.ObterDadosImagemPorProduto(produto);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(IFormFile arquivo, IFormCollection form)
        {
            var tProduto = JsonConvert.DeserializeObject<TprodutoCatalogo>(form["produto"]);
            tProduto.UsuarioEdicao = LoggedUser.Apelido;

            _logger.LogInformation("Atualizar - Request: {0}", System.Text.Json.JsonSerializer.Serialize(tProduto));

            var retorno = await _bll.Atualizar(tProduto, arquivo, _appSettings.Value.ImgCaminho);

            if (retorno != null)
            {
                _logger.LogInformation("Atualizar - Response: {0}", retorno);
                return BadRequest(new { message = retorno });
            }
            
            return Ok();
        }

        [HttpPost("imagem")]
        public async Task<IActionResult> Upload(IFormFile arquivo, [FromForm] int idProdutoCalatogo)
        {
            try
            {
                if (arquivo.ContentType.Contains("png") || arquivo.ContentType.Contains("jpeg") || arquivo.ContentType.Contains("bmp") || arquivo.ContentType.Contains("jpg"))
                {
                    Guid idArquivo = Guid.NewGuid();
                    var extensao = arquivo.FileName.Substring(arquivo.FileName.Length - 3, 3);
                    var file = Path.Combine(_appSettings.Value.ImgCaminho, $"{idArquivo}.{extensao}");
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await arquivo.CopyToAsync(fileStream);
                    }

                    _bll.SalvarArquivo($"{idArquivo}.{extensao}", idProdutoCalatogo, 1, "0");

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

        [HttpPost("criar")]
        public async Task<IActionResult> Criar(IFormFile arquivo, IFormCollection form)
        {
            try
            {
                var usuario = LoggedUser.Apelido;

                var tProduto = JsonConvert.DeserializeObject<TprodutoCatalogo>(form["produto"]);

                _logger.LogInformation("Criar - Request: {0}", System.Text.Json.JsonSerializer.Serialize(tProduto));

                var retorno = await _bll.Criar(tProduto, usuario, arquivo, _appSettings.Value.ImgCaminho);

                if (retorno != null)
                {
                    _logger.LogInformation("Atualizar - Response: {0}", retorno);
                    return BadRequest(new { message = retorno });
                }

                return Ok(new { message = "Produto criado com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("listar-produtos-propriedades/{propriedadeOculta}&{propriedadeOcultaItem}")]
        public async Task<IActionResult> ListarProdutosPropriedadesAtivos(bool propriedadeOculta, bool propriedadeOcultaItem)
        {
            var saida = await _produtoOrcamentoCotacaoBll.ListarProdutoCatalogoParaVisualizacao(propriedadeOculta, propriedadeOcultaItem);

            return Ok(saida);
        }

        [HttpGet("buscarTiposPropriedades")]
        public async Task<IActionResult> BuscarTiposPropriedadesProdutoCatalago()
        {
            //var saida = await _bll.BuscarTiposPropriedadesProdutoCatalago();
            return Ok();
        }
    }
}
