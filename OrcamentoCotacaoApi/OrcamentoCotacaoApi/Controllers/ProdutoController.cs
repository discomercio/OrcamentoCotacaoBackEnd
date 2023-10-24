using Azure;
using InfraBanco.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.GrupoSubgrupoProduto;
using OrcamentoCotacaoBusiness.Models.Request.Produto;
using OrcamentoCotacaoBusiness.Models.Request.Propriedades;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaoApi.BaseController
{
    [TypeFilter(typeof(ControleDelayFilter))]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
    public class ProdutoController : Controllers.BaseController
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly ProdutoOrcamentoCotacaoBll _produtoBll;
        private readonly CoeficienteBll _coeficienteBll;

        public ProdutoController(
            ILogger<ProdutoController> logger,
            ProdutoOrcamentoCotacaoBll produtoBll,
            CoeficienteBll coeficienteBll)
        {
            _logger = logger;
            _produtoBll = produtoBll;
            _coeficienteBll = coeficienteBll;
        }

        [HttpPost("buscarProdutos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarProduto(ProdutosRequestViewModel produtosRequest)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                ProdutosRequest = produtosRequest
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/BuscarProduto/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _produtoBll.ListaProdutosCombo(produtosRequest);

            if (response == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/BuscarProduto/POST - Response => [Não tem response].");
                return NoContent();
            }
            else
            {
                var responses = new
                {
                    ProdutosSimples = response.ProdutosSimples.Count,
                    ProdutosCompostos = response.ProdutosCompostos.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/BuscarProduto/POST - Response => [{JsonSerializer.Serialize(responses)}].");

                return Ok(response);
            }
        }

        [HttpPost("buscarProdutosOrcamentoEdicao")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarProdutosOrcamentoEdicao(ProdutosOpcaoEdicaoResquest produtosRequest)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                ProdutosRequest = produtosRequest
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/BuscarProduto/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _produtoBll.ListarProdutosComboParaEdicao(produtosRequest);

            if (response == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/BuscarProduto/POST - Response => [Não tem response].");
                return NoContent();
            }
            else
            {
                var responses = new
                {
                    ProdutosSimples = response.ProdutosSimples.Count,
                    ProdutosCompostos = response.ProdutosCompostos.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/BuscarProduto/POST - Response => [{JsonSerializer.Serialize(responses)}].");

                return Ok(response);
            }
        }

        [HttpPost("buscarCoeficientes")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarCoeficientes(CoeficienteRequestViewModel coeficienteRequest)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                CoeficienteRequest = coeficienteRequest
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/BuscarCoeficientes/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _coeficienteBll.BuscarListaCoeficientesFabricantesHistoricoDistinct(coeficienteRequest);

            if (response == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/BuscarCoeficientes/POST - Response => [Não tem response].");
                return NoContent();
            }
            else
            {
                var responses = new
                {
                    ListaCoeficientes = response.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/BuscarCoeficientes/POST - Response => [{JsonSerializer.Serialize(responses)}].");

                return Ok(response);
            }
        }

        [HttpGet("propriedades")]
        public async Task<IActionResult> ObterListaPropriedadesProdutos()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterListaPropriedadesProdutos/GET - Request => [{JsonSerializer.Serialize(request)}].");

            if (!User.ValidaPermissao((int)ePermissao.CatalogoPropriedadeConsultar) &&
                !User.ValidaPermissao((int)ePermissao.CatalogoConsultar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var ret = await _produtoBll.ObterListaPropriedadesProdutos();

            if (ret == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesProdutos/GET - Response => [Não tem response].");
                return NoContent();
            }
            else
            {
                var response = new
                {
                    ListaPropriedadesProdutos = ret.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesProdutos/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(ret);
            }
        }

        [HttpPost("listar-propriedades")]
        public IActionResult ListarPropriedadesProdutos(PropriedadesRequest request)
        {
            try
            {
                request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
                request.Usuario = LoggedUser.Apelido;

                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ProdutoController/ListarPropriedadesProdutos/POST - Request => [{JsonSerializer.Serialize(request)}].");

                if (!User.ValidaPermissao((int)ePermissao.CatalogoPropriedadeConsultar) &&
                    !User.ValidaPermissao((int)ePermissao.CatalogoConsultar))
                    return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

                var response = _produtoBll.ListarPropriedadesProdutos(request);

                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}].  ProdutoController/ListarPropriedadesProdutos/POST - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet("propriedades/{id}")]
        public async Task<IActionResult> ObterListaPropriedadesProdutos(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterListaPropriedadesProdutos/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var ret = await _produtoBll.ObterListaPropriedadesProdutos(id);

            if (ret == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesProdutos/GET - Response => [Não tem response].");
                return NoContent();
            }
            else
            {
                var response = new
                {
                    ListaPropriedadesProdutos = ret.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesProdutos/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(ret);
            }
        }

        [HttpGet("itens/{idProdutoCatalogo}")]
        public async Task<IActionResult> ObterListaPropriedadesProdutosById(int idProdutoCatalogo)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdProdutoCatalogo = idProdutoCatalogo
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterListaPropriedadesProdutosById/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var ret = await _produtoBll.ObterListaPropriedadesProdutosById(idProdutoCatalogo);

            if (ret == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesProdutosById/GET - Response => [Não tem response].");
                return NoContent();
            }
            else
            {
                var response = new
                {
                    ListaPropriedadesProdutos = ret.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesProdutosById/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(ret);
            }
        }

        [HttpGet("opcoes/{IdProdutoCatalogoPropriedade}")]
        public async Task<IActionResult> ObterListaPropriedadesOpcoesProdutosById(int IdProdutoCatalogoPropriedade)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdProdutoCatalogoPropriedade = IdProdutoCatalogoPropriedade
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterListaPropriedadesOpcoesProdutosById/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var ret = await _produtoBll.ObterListaPropriedadesOpcoesProdutosById(IdProdutoCatalogoPropriedade);

            if (ret == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesOpcoesProdutosById/GET - Response => [Não tem response].");
                return NoContent();
            }
            else
            {
                var response = new
                {
                    ListaPropriedadesProdutos = ret.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesOpcoesProdutosById/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(ret);
            }
        }

        [HttpGet("opcoes")]
        public async Task<IActionResult> ObterListaPropriedadesOpcoes()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterListaPropriedadesOpcoes/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var ret = await _produtoBll.ObterListaPropriedadesOpcoes();

            if (ret == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesOpcoes/GET - Response => [Não tem response].");
                return NoContent();
            }
            else
            {
                var response = new
                {
                    ListaPropriedadesProdutos = ret.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaPropriedadesOpcoes/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(ret);
            }
        }

        [HttpGet("fabricantes")]
        public async Task<IActionResult> ObterListaFabricante()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterListaFabricante/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var ret = await _produtoBll.ObterListaFabricante();

            if (ret == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaFabricante/GET - Response => [Não tem response].");
                return NoContent();
            }
            else
            {
                var response = new
                {
                    ObterListaFabricante = ret.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaFabricante/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(ret);
            }
        }

        [HttpGet("listar-produtos-propriedades-ativos")]
        public async Task<IActionResult> ObterListaProdutosPropriedadesProdutosAtivos()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterListaProdutosPropriedadesProdutosAtivos/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = await _produtoBll.ObterListaProdutosPropriedadesProdutosAtivos();


            var response = new
            {
                ListaProdutosPropriedades = saida.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterListaProdutosPropriedadesProdutosAtivos/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(saida);
        }

        [HttpGet("listar-propriedades-opcoes-produtos-ativos")]
        public async Task<IActionResult> ObterPropriedadesEOpcoesProdutosAtivos()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterPropriedadesEOpcoesProdutosAtivos/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = await _produtoBll.ObterPropriedadesEOpcoesProdutosAtivos();

            var response = new
            {
                ProdutosAtivos = saida.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterPropriedadesEOpcoesProdutosAtivos/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(saida);
        }

        [HttpPost("buscar-produtos-opcoes-ativos")]
        public async Task<IActionResult> ObterPropriedadesEOpcoesProdutosPorProduto(ProdutosAtivosRequestViewModel obj)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                ProdutosAtivosRequestViewModel = obj
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterPropriedadesEOpcoesProdutosPorProduto/POST - Request => [{JsonSerializer.Serialize(request)}].");


            if (!User.ValidaPermissao((int)ePermissao.CatalogoConsultar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var retorno = await _produtoBll.BuscarProdutoCatalogoParaVisualizacao(obj.idProduto, obj.propriedadeOculta, obj.propriedadeOcultaItem);

            var response = new
            {
                ProdutoCatalogo = retorno
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterPropriedadesEOpcoesProdutosPorProduto/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(retorno);
        }

        [HttpGet("listar-produtos-ativos")]
        public async Task<IActionResult> ObterProdutosAtivos()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoController/ObterProdutosAtivos/GET - Request => [{JsonSerializer.Serialize(request)}].");

            if (!User.ValidaPermissao((int)ePermissao.CatalogoConsultar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var ret = await _produtoBll.ObterProdutosAtivos();

            var response = new
            {
                ProdutosAtivos = ret.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}].  ProdutoController/ObterProdutosAtivos/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(ret);
        }

        [HttpGet("buscarGruposProdutos")]
        public async Task<IActionResult> BuscarGruposProdutos()
        {
            var request = new
            {
                Usuario = LoggedUser.Apelido,
                CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
                Ip = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ProdutoController/BuscarGruposProdutos/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = _produtoBll.BuscarGruposProdutos();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}].  ProdutoController/BuscarGruposProdutos/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("buscarGruposSubgruposProdutos")]
        public IActionResult BuscarGruposSubgruposProdutos(GrupoSubgrupoProdutoRequest request)
        {
            request.Usuario = LoggedUser.Apelido;
            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.IP = HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ProdutoController/BuscarGruposSubgruposProdutos/GET");

            var response = _produtoBll.BuscarGrupoSubgrupoProduto((int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_NovoOrcto_Filtro_GrupoSubgrupo, request.Loja);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}].  ProdutoController/BuscarGruposProdutos/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("buscarGruposSubgruposProdutosPorLojas")]
        public IActionResult BuscarGruposSubgruposProdutosPorLojas(GrupoSubgrupoProdutoRequest request)
        {
            request.Usuario = LoggedUser.Apelido;
            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.IP = HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. ProdutoController/BuscarGruposSubgruposProdutos/GET");

            var response = _produtoBll.BuscarGrupoSubgrupoProdutoPorLojas((int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_NovoOrcto_Filtro_GrupoSubgrupo, request.Lojas);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}].  ProdutoController/BuscarGruposProdutos/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }
    }
}