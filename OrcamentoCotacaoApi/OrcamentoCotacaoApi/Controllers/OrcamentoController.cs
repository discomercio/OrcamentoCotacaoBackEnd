using DocumentFormat.OpenXml.Spreadsheet;
using InfraBanco.Constantes;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.Orcamento;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.Orcamento;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaoApi.Controllers
{
    [TypeFilter(typeof(ControleDelayFilter))]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
    public class OrcamentoController : BaseController
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly OrcamentoCotacaoBll _orcamentoBll;
        private readonly PermissaoBll _permissaoBll;

        public OrcamentoController(
            ILogger<OrcamentoController> logger,
            OrcamentoCotacaoBll orcamentoBll,
            PermissaoBll permissaoBll)
        {
            _logger = logger;
            _orcamentoBll = orcamentoBll;
            _permissaoBll = permissaoBll;
        }

        [HttpPost("porfiltro")]
        public IActionResult PorFiltro(TorcamentoFiltro filtro)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Filtro = filtro,
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/PorFiltro/GET - Request => [{JsonSerializer.Serialize(request)}].");

            filtro.PermissaoUniversal = User.ValidaPermissao((int)ePermissao.AcessoUniversalOrcamentoPedidoPrepedidoConsultar);

            var saida = _orcamentoBll.PorFiltro(filtro, LoggedUser);

            if (saida != null)
            {
                var response = new
                {
                    QtdOrcamentos = saida.orcamentoCotacaoListaDto.Count,
                    QtdeRegistros = saida.qtdeRegistros
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/PorFiltro/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/PorFiltro/GET - Response => [Não tem response].");
                return NoContent();
            }
        }

        [HttpGet("status")]
        public async Task<IActionResult> ObterListaStatus(string origem, string lojaLogada)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Origem = origem,
                LojaLogada = lojaLogada
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/ObterListaStatus/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = await _orcamentoBll.ObterListaStatus(new TorcamentoFiltro { Origem = origem, Loja = lojaLogada });

            if (saida != null)
            {
                var response = new
                {
                    Orcamentos = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/ObterListaStatus/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/ObterListaStatus/GET - Response => [Não tem response].");
                return NoContent();
            }
        }

        [HttpGet("validade")]
        public async Task<IActionResult> BuscarConfigValidade(string lojaLogada)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                LojaLogada = lojaLogada
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/BuscarConfigValidade/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = await Task.FromResult(_orcamentoBll.BuscarConfigValidade(lojaLogada));

            if (saida != null)
            {
                var response = new
                {
                    ConfigValidade = saida
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/BuscarConfigValidade/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/BuscarConfigValidade/GET - Response => [Não tem response].");
                return NoContent();
            }
        }

        [HttpPost]
        public IActionResult Post(CadastroOrcamentoRequest model)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Model = model,
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/Post/POST - Request => [{JsonSerializer.Serialize(request)}].");

            model.CorrelationId = correlationId;
            model.Usuario = LoggedUser.Apelido;
            model.IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var response = _orcamentoBll.CadastrarOrcamentoCotacao(model, user);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/Post/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id,
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/Get/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var permissao = this.ObterPermissaoOrcamento(id);

            if (!permissao.VisualizarOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var orcamentoCotacao = _orcamentoBll.PorFiltro(id, (int)user.TipoUsuario);

            var response = new
            {
                OrcamentoCotacao = orcamentoCotacao.Id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/Get/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(orcamentoCotacao);
        }

        [HttpGet("buscarDadosParaMensageria")]
        public IActionResult BuscarDadosParaMensageria(int idOrcamento, bool usuarioInterno)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdOrcamento = idOrcamento,
                UsuarioInterno = usuarioInterno
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/BuscarDadosParaMensageria/GET - Request => [{JsonSerializer.Serialize(request)}].");


            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var dados = _orcamentoBll.BuscarDadosParaMensageria(user, idOrcamento, usuarioInterno);

            var response = new
            {
                DadosParaMensageria = dados
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/BuscarDadosParaMensageria/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(dados);
        }

        [HttpPut("atualizarOrcamentoOpcao")]
        public IActionResult AtualizarOrcamentoOpcao(AtualizarOrcamentoOpcaoRequest opcao)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                AtualizarOrcamentoOpcaoRequest = opcao
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/AtualizarOrcamentoOpcao/PUT - Request => [{JsonSerializer.Serialize(request)}].");


            var permissao = this.ObterPermissaoOrcamento(opcao.IdOrcamentoCotacao);

            if (!permissao.EditarOpcaoOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            opcao.CorrelationId = correlationId;
            opcao.Usuario = LoggedUser.Apelido;
            opcao.IP = HttpContext.Connection.RemoteIpAddress.ToString();

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var response = _orcamentoBll.AtualizarOrcamentoOpcao(opcao, user);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/AtualizarOrcamentoOpcao/PUT - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPut("{id}/status/{idStatus}")]
        public IActionResult AtualizarStatus(int id, short idStatus)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id,
                IdStatus = idStatus,
                IP = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/AtualizarStatus/PUT - Request => [{JsonSerializer.Serialize(request)}].");


            if (idStatus == 2) //Cancelar
            {
                var permissao = this.ObterPermissaoOrcamento(id);

                if (!permissao.CancelarOrcamento)
                    return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });
            }

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var response = _orcamentoBll.AtualizarStatus(id, user, idStatus, request.IP);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/AtualizarStatus/PUT - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPut("{id}/reenviar")]
        public IActionResult ReenviarOrcamento(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/ReenviarOrcamento/PUT - Request => [{JsonSerializer.Serialize(request)}].");

            var permissao = this.ObterPermissaoOrcamento(id);

            if (!permissao.ReenviarOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var response = _orcamentoBll.ReenviarOrcamentoCotacao(id);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/ReenviarOrcamento/PUT - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("{id}/prorrogar")]
        public IActionResult ProrrogarOrcamento(int id, string lojaLogada)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id,
                LojaLogada = lojaLogada,
                IP = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/ProrrogarOrcamento/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var permissao = this.ObterPermissaoOrcamento(id);

            if (!permissao.ProrrogarOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var response = _orcamentoBll.ProrrogarOrcamento(id, LoggedUser.Id, lojaLogada, LoggedUser.TipoUsuario, request.IP);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/ProrrogarOrcamento/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet("parametros")]
        public IActionResult BuscarParametros(int idCfgParametro, string lojaLogada)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdCfgParametro = idCfgParametro,
                LojaLogada = lojaLogada
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/BuscarParametros/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = _orcamentoBll.BuscarParametros(idCfgParametro, lojaLogada);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/BuscarParametros/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPost("atualizarDados")]
        public IActionResult AtualizarDados(OrcamentoResponseViewModel model)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                OrcamentoResponseViewModel = model,
                IP = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/AtualizarDados/POST - Request => [{JsonSerializer.Serialize(request)}].");


            var permissao = this.ObterPermissaoOrcamento((int)model.Id);

            if (!permissao.EditarOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var retorno = _orcamentoBll.AtualizarDadosCadastraisOrcamento(model, user, request.IP);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/AtualizarDados/GET - Response => [{JsonSerializer.Serialize(retorno)}].");

            return Ok(retorno);
        }

        private PermissaoOrcamentoResponse ObterPermissaoOrcamento(int IdOrcamento)
        {
            return _permissaoBll.RetornarPermissaoOrcamento(new PermissaoOrcamentoRequest()
            {
                IdOrcamento = IdOrcamento,
                PermissoesUsuario = LoggedUser.Permissoes,
                TipoUsuario = LoggedUser.TipoUsuario.Value,
                Usuario = LoggedUser.Apelido
            }).Result;
        }

        [HttpPost("aprovarOrcamento")]
        public async Task<IActionResult> AprovarOrcamento(AprovarOrcamentoRequestViewModel aprovarOrcamento)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Orcamento = aprovarOrcamento.IdOrcamento,
                OpcaoAprovada = aprovarOrcamento.OpcaoSequencia,
                PagamentoAprovado = aprovarOrcamento.PagtoAprovadoTexto,
                IdPagamentoAprovado = aprovarOrcamento.IdFormaPagto,
                IP = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/AprovarOrcamento/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var orcamento = _orcamentoBll.PorFiltro(aprovarOrcamento.IdOrcamento, (int)user.TipoUsuario);

            if (orcamento == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/AprovarOrcamento/GET - Response => [Não tem response].");
                return BadRequest(new { message = "Acesso negado." });
            }


            if (orcamento.Id != aprovarOrcamento.IdOrcamento)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/AprovarOrcamento/GET - Response => [Não tem response].");
                return BadRequest(new { message = "Acesso negado." });
            }

            var retorno = await _orcamentoBll.AprovarOrcamento(
                aprovarOrcamento,
                (Constantes.TipoUsuarioContexto)user.TipoUsuario,
                user.Id, request.IP);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/AprovarOrcamento/GET - Response => [Retorna lista de mensagens].");

            return Ok(retorno);
        }

        [HttpPost("excluir")]
        public IActionResult ExcluirOrcamento(OrcamentoResponseViewModel model)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                OrcamentoResponseViewModel = model,
                IP = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/ExcluirOrcamento/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = new ExcluirOrcamentoResponse();
            response.Sucesso = false;

            var permissao = this.ObterPermissaoOrcamento((int)model.Id);
            if (permissao != null && !permissao.ExcluirOrcamento)
            {
               response.Mensagem = "Não encontramos a permissão necessária para realizar atividade!";
                return Ok(response);
            }

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            response = _orcamentoBll.ExcluirOrcamento(model, user, request.IP);

            return Ok(response);
        }

        [HttpPost("anular")]
        public IActionResult AnularOrcamento(OrcamentoResponseViewModel model)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                OrcamentoResponseViewModel = model,
                IP = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentoController/AnularOrcamento/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = new ExcluirOrcamentoResponse();
            response.Sucesso = false;

            var permissao = this.ObterPermissaoOrcamento((int)model.Id);
            if (permissao != null && !permissao.AnularOrcamentoAprovado)
            {
                response.Mensagem = "Este orçamento não pode ser anulado!";
                return Ok(response);
            }

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            response = _orcamentoBll.AnularOrcamento(model, user, request.IP);

            return Ok(response);
        }

        [HttpPost("relatorioItensOrcamento")]
        public IActionResult RelatorioItensOrcamento()
        {
            return Ok();
        }
    }
}