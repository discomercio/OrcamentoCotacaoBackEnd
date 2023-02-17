using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using System;
using System.IO;
using System.Threading.Tasks;
using UtilsGlobais.Configs;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
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
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Page = page,
                PageItens = pageItens,
                IdCliente = idCliente
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Listar/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");


            if (!User.ValidaPermissao((int)ePermissao.CatalogoCaradastrarIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

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
            {
                var response = new
                {
                    ProdutoCatalogo = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Listar/GET - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Listar/GET - Response => [Não tem response].");
                return NoContent();
            }
        }

        [HttpGet("ativos")]
        public async Task<IActionResult> ListarAtivos(int page, int pageItens, int idCliente)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Page = page,
                PageItens = pageItens,
                IdCliente = idCliente
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ListarAtivos/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");


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
            {
                var response = new
                {
                    ProdutoCatalogo = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ListarAtivos/GET - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ListarAtivos/GET - Response => [Não tem response].");
                return NoContent();
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(string id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterPorId/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");

            var saida = _bll.PorFiltro(new InfraBanco.Modelos.Filtros.TprodutoCatalogoFiltro() { Id = id });

            if (saida != null && saida.Count > 0)
            {
                var response = new
                {
                    ProdutoCatalogo = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterPorId/GET - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterPorId/GET - Response => [Não tem response].");
                return NoContent();
            }
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> ObterPorCodigo(string codigo)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Codigo = codigo
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterPorCodigo/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");

            var saida = _bll.PorFiltro(new InfraBanco.Modelos.Filtros.TprodutoCatalogoFiltro() { Produto = codigo });

            if (saida != null && saida.Count > 0)
            {
                var response = new
                {
                    ProdutoCatalogo = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterPorCodigo/GET - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterPorCodigo/GET - Response => [Não tem response].");
                return NoContent();
            }
        }

        [HttpGet("{id}/detalhes")]
        public async Task<IActionResult> Detalhes(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Detalhes/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");

            var saida = _bll.Detalhes(id);

            if (saida != null)
            {
                var response = new
                {
                    ProdutoCatalogo = saida.Nome
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Detalhes/GET - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Detalhes/GET - Response => [Não tem response].");
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Excluir/DELETE - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");

            var retorno = _bll.Excluir(id, _appSettings.Value.ImgCaminho);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Excluir/DELETE - Response => [Produto Catalogo Removido.].");

            if(!string.IsNullOrEmpty(retorno))
            {
                return BadRequest(new { message = retorno });
            }
            
            return Ok(true);
        }

        [HttpDelete("imagem")]
        public async Task<IActionResult> ExcluirImagem(int idProduto, int idImagem)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdProduto = idProduto,
                IdImagem = idImagem
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ExcluirImagem/DELETE - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");

            var retorno = _bll.ExcluirImagem(idProduto, idImagem, _appSettings.Value.ImgCaminho);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ExcluirImagem/DELETE - Response => [{retorno}].");

            if (retorno != null)
                return BadRequest(new { message = retorno });

            return Ok();
        }

        [HttpGet("{id}/itens")]
        public async Task<IActionResult> ObterListaItens(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterListaItens/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");

            var saida = _bll.ObterListaItens(id);

            if (saida != null)
            {
                var response = new
                {
                    ListaItens = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterListaItens/GET - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterListaItens/GET - Response => [Não tem response].");
                return NotFound();
            }
        }

        [HttpGet("{id}/imagem")]
        public async Task<IActionResult> ObterImagem(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterImagem/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");

            var saida = _bll.ObterListaImagensPorId(id);

            if (saida != null)
            {
                var response = new
                {
                    ListaImagens = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterImagem/GET - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterImagem/GET - Response => [Não tem response].");
                return NotFound();
            }
        }

        [HttpGet("imagem/{produto}")]
        public async Task<IActionResult> ObterDadosImagemPorProduto(string produto)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Produto = produto
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterDadosImagemPorProduto/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");

            var saida = _bll.ObterDadosImagemPorProduto(produto);

            if (saida != null)
            {
                var response = new
                {
                    DadosImagemPorProduto = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterDadosImagemPorProduto/GET - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterDadosImagemPorProduto/GET - Response => [Não tem response].");
                return NotFound();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(IFormFile arquivo, IFormCollection form)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Atualizar/PUT - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");

            if (!User.ValidaPermissao((int)ePermissao.CatalogoCaradastrarIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var tProduto = JsonConvert.DeserializeObject<TprodutoCatalogo>(form["produto"]);
            //var lojaLogada = form["loja"];
            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            tProduto.UsuarioEdicao = LoggedUser.Apelido;

            _logger.LogInformation("Atualizar - Request: {0}", System.Text.Json.JsonSerializer.Serialize(tProduto));

            var retorno = await _bll.Atualizar(tProduto, arquivo, _appSettings.Value.ImgCaminho, ip, LoggedUser);

            if (retorno != null)
            {
                _logger.LogInformation("Atualizar - Response: {0}", retorno);
                return BadRequest(new { message = retorno });
            }

            var response = new
            {
                Dados = retorno
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Atualizar/PUT - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

            return Ok();
        }

        [HttpPost("imagem")]
        public async Task<IActionResult> Upload(IFormFile arquivo, [FromForm] int idProdutoCalatogo)
        {
            try
            {
                var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

                var request = new
                {
                    Usuario = LoggedUser.Apelido,
                    IdProdutoCalatogo = idProdutoCalatogo
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/imagem/POST - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");


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

                    _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/imagem/POST - Response => [Arquivo salvo com sucesso.].");

                    return Ok(new
                    {
                        message = "Arquivo salvo com sucesso.",
                        file
                    });
                }
                else
                {
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/imagem/POST - Response => [Formato inválido. O arquivo deve ser imagem png, jpg ou bmp.].");

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
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Arquivo = arquivo?.Name
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Criar/POST - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");


            if (!User.ValidaPermissao((int)ePermissao.CatalogoCaradastrarIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            try
            {
                var usuario = LoggedUser.Apelido;

                var tProduto = JsonConvert.DeserializeObject<TprodutoCatalogo>(form["produto"]);
                var lojaLogada = form["loja"];
                var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _logger.LogInformation("Criar - Request: {0}", System.Text.Json.JsonSerializer.Serialize(tProduto));

                var retorno = await _bll.Criar(tProduto, usuario, arquivo, _appSettings.Value.ImgCaminho, LoggedUser, ip);

                if (retorno != null)
                {
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Criar/POST - Response => [{retorno}].");

                    _logger.LogInformation("Atualizar - Response: {0}", retorno);
                    return BadRequest(new { message = retorno });
                }

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/Criar/POST - Response => [Produto criado com sucesso.].");

                return Ok(new { message = "Produto criado com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("listar-produtos-propriedades")]
        public async Task<IActionResult> ListarProdutosPropriedadesAtivos(ProdutoCalculadoraVrfRequestViewModel request)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var requests = new
            {
                Usuario = LoggedUser.Apelido,
                ProdutoCalculadoraVrfRequestViewModel = request
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ListarProdutosPropriedadesAtivos/POST - Request => [{System.Text.Json.JsonSerializer.Serialize(requests)}].");

            var saida = await _produtoOrcamentoCotacaoBll.ListarProdutoCatalogoParaVisualizacao(request.propriedadeOculta, request.propriedadeOcultaItem);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ListarProdutosPropriedadesAtivos/POST - Response => [{saida.Count}].");

            return Ok(saida);
        }

        [HttpGet("buscarDataTypes")]
        public async Task<IActionResult> BuscarDataTypes()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var requests = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/buscarDataTypes/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(requests)}].");

            var saida = await _bll.BuscarDataTypes();

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/buscarDataTypes/GET - Response => [{saida.Count}].");

            return Ok(saida);
        }

        [HttpGet("buscarTipoPropriedades")]
        public async Task<IActionResult> BuscarTipoPropriedades()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var requests = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/BuscarTipoPropriedades/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(requests)}].");

            var saida = await _bll.BuscarTipoPropriedades();

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/BuscarTipoPropriedades/GET - Response => [{saida.Count}].");

            return Ok(saida);
        }

        [HttpPost("propriedades")]
        public async Task<IActionResult> GravarPropriedade(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var requests = new
            {
                Usuario = LoggedUser.Apelido,
                ProdutoCatalogoPropriedade = produtoCatalogoPropriedade,
                IP = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/GravarPropriedade/POST - Request => [{System.Text.Json.JsonSerializer.Serialize(requests)}].");

            if (!User.ValidaPermissao((int)ePermissao.CatalogoPropriedadeIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            //var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            var saida = await _bll.GravarPropriedadesProdutos(produtoCatalogoPropriedade, LoggedUser, requests.IP);

            if (!string.IsNullOrEmpty(saida)) return BadRequest(new { message = saida });

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/GravarPropriedade/POST - Response => [{saida}].");

            return Ok();
        }

        [HttpGet("propriedades/{id}")]
        public async Task<IActionResult> ObterListaPropriedadesProdutos(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var requests = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterListaPropriedadesProdutos/GET - Request => [{System.Text.Json.JsonSerializer.Serialize(requests)}].");

            var ret = await _bll.ObterListaPropriedadesProdutos(id);

            if (ret == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterListaPropriedadesProdutos/GET - Response => [Não tem response.].");
                return NoContent();
            }
            else
            {
                var response = new
                {
                    PropriedadesProdutos = ret.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterListaPropriedadesProdutos/GET - Response => [{System.Text.Json.JsonSerializer.Serialize(response)}].");

                return Ok(ret);
            }
        }

        [HttpPut("propriedades")]
        public async Task<IActionResult> AtualizarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                ProdutoCatalogoPropriedade = produtoCatalogoPropriedade
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/AtualizarPropriedadesProdutos/PUT - Request => [{System.Text.Json.JsonSerializer.Serialize(request)}].");


            if (!User.ValidaPermissao((int)ePermissao.CatalogoPropriedadeIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            try
            {
                string ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var saida = await _bll.AtualizarPropriedadesProdutos(produtoCatalogoPropriedade, LoggedUser, ip);

                if (!string.IsNullOrEmpty(saida.Mensagem) || saida.ProdutosCatalogo?.Count > 0) return Ok(saida);

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/AtualizarPropriedadesProdutos/PUT - Response => [{System.Text.Json.JsonSerializer.Serialize(saida)}].");

                return Ok(saida);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObterPropriedadesUtilizadosPorProdutos/{idPropriedade}")]
        public async Task<IActionResult> ObterPropriedadesUtilizadosPorProdutos(int idPropriedade)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ObterPropriedadesUtilizadosPorProdutos/GET - Request => [{idPropriedade}].");

            var response = await _bll.ObterPropriedadesUtilizadosPorProdutos(idPropriedade);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ArquivoController/ObterPropriedadesUtilizadosPorProdutos/GET - Response => [{response}].");

            return Ok(response);
        }

        [HttpPost("ExcluirPropriedades/{idPropriedade}")]
        public async Task<IActionResult> ExcluirPropriedades(int idPropriedade)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoCatalogoController/ExcluirPropriedades/POST - Request => [{idPropriedade}].");

            if (!User.ValidaPermissao((int)ePermissao.CatalogoPropriedadeIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            string ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var response = await _bll.ExcluirPropriedades(idPropriedade, LoggedUser, ip);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ArquivoController/ExcluirPropriedades/POST - Response => [{response}].");

            return Ok(response);
        }
    }
}