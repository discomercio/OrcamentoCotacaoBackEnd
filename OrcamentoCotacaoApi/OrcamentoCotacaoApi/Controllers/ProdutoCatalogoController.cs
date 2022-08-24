using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using Produto;
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
        private readonly ProdutoCatalogoOrcamentoCotacaoBll _bll;
        private readonly IOptions<Configuracoes> _appSettings;
        private readonly ProdutoOrcamentoCotacaoBll _produtoOrcamentoCotacaoBll;

        public ProdutoCatalogoController(ProdutoCatalogoOrcamentoCotacaoBll bll, IOptions<Configuracoes> appSettings,
            ProdutoOrcamentoCotacaoBll produtoOrcamentoCotacaoBll)
        {
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
            var saida = _bll.ExcluirImagem(idProduto, idImagem);

            if (saida)
                return Ok(saida);
            else
                return NotFound();
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

        [HttpPut]
        public async Task<IActionResult> Atualizar(TprodutoCatalogo produto)
        {
            produto.UsuarioEdicao = LoggedUser.Apelido;

            var saida = _bll.Atualizar(produto);

            if (saida)
                return Ok(true);
            else
                return BadRequest();
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

        [HttpPost]
        public async Task<IActionResult> Criar(TprodutoCatalogo produto)
        {
            var usuario = User.Identity.Name;
            var retorno = await _bll.Criar(produto, usuario, _appSettings.Value.ImgCaminho);
            if (retorno != null)
            {
                return BadRequest(new { message = "Erro ao criar novo produto." });
            }

            return Ok(new { message = "Produto criado com sucesso." });
        }

        [HttpGet("listar-produtos-propriedades/{propriedadeOculta}&{propriedadeOcultaItem}")]
        public async Task<IActionResult> ListarProdutosPropriedadesAtivos(bool propriedadeOculta, bool propriedadeOcultaItem)
        {
            var saida = await _produtoOrcamentoCotacaoBll.ListarProdutoCatalogoParaVisualizacao(propriedadeOculta, propriedadeOcultaItem);

            return Ok(saida);
        }
    }
}
