using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.Orcamento;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
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
            _logger.LogInformation("Buscando lista de orçamentos");

            var saida = _orcamentoBll.PorFiltro(filtro, LoggedUser);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("status")]
        public async Task<IActionResult> ObterListaStatus(string origem, string lojaLogada)
        {
            _logger.LogInformation("Buscando status");

            var saida = await _orcamentoBll.ObterListaStatus(new TorcamentoFiltro { Origem = origem, Loja = lojaLogada });

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("validade")]
        public IActionResult BuscarConfigValidade(string lojaLogada)
        {
            _logger.LogInformation("Buscando ConfigValidade");

            var saida = _orcamentoBll.BuscarConfigValidade(lojaLogada);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpPost]
        public IActionResult Post(CadastroOrcamentoRequest model)
        {
            model.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            model.Usuario = LoggedUser.Apelido;

            _logger.LogInformation($"CorrelationId => [{model.CorrelationId}]. OrcamentoController/POST - Request => [{JsonSerializer.Serialize(model)}].");

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var response = _orcamentoBll.CadastrarOrcamentoCotacao(model, user);

            _logger.LogInformation($"CorrelationId => [{model.CorrelationId}]. ArquivoController/Download/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var permissao = this.ObterPermissaoOrcamento(id);

            if (!permissao.VisualizarOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var orcamentoCotacao = _orcamentoBll.PorFiltro(id, (int)user.TipoUsuario);

            return Ok(orcamentoCotacao);
        }

        [HttpGet("buscarDadosParaMensageria")]
        public IActionResult BuscarDadosParaMensageria(int idOrcamento, bool usuarioInterno)
        {
            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var dados = _orcamentoBll.BuscarDadosParaMensageria(user, idOrcamento, usuarioInterno);
            return Ok(dados);
        }

        [HttpPut("atualizarOrcamentoOpcao")]
        public IActionResult AtualizarOrcamentoOpcao(AtualizarOrcamentoOpcaoRequest opcao)
        {
            var permissao = this.ObterPermissaoOrcamento(opcao.IdOrcamentoCotacao);

            if (!permissao.EditarOpcaoOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            opcao.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            opcao.Usuario = LoggedUser.Apelido;

            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. OrcamentoController/POST - Request => [{JsonSerializer.Serialize(opcao)}].");

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var response = _orcamentoBll.AtualizarOrcamentoOpcao(opcao, user);
            _logger.LogInformation($"CorrelationId => [{opcao.CorrelationId}]. OrcamentoController/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpPut("{id}/status/{idStatus}")]
        public IActionResult AtualizarStatus(int id,short idStatus)
        {
            if (idStatus == 2) //Cancelar
            {
                var permissao = this.ObterPermissaoOrcamento(id);

                if (!permissao.CancelarOrcamento)
                    return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });
            }

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            return Ok(_orcamentoBll.AtualizarStatus(id, user, idStatus));            
        }

        [HttpPut("{id}/reenviar")]
        public IActionResult ReenviarOrcamento(int id)
        {
            var permissao = this.ObterPermissaoOrcamento(id);

            if (!permissao.ReenviarOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            return Ok(_orcamentoBll.ReenviarOrcamentoCotacao(id));
        }

        [HttpPost("{id}/prorrogar")]
        public IActionResult ProrrogarOrcamento(int id, string lojaLogada)
        {
            var permissao = this.ObterPermissaoOrcamento(id);

            if (!permissao.ProrrogarOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            return Ok(_orcamentoBll.ProrrogarOrcamento(id, LoggedUser.Id, lojaLogada, LoggedUser.TipoUsuario));
        }

        [HttpGet("parametros")]
        public IActionResult BuscarParametros(int idCfgParametro, string lojaLogada)
        {            
            return Ok(_orcamentoBll.BuscarParametros(idCfgParametro, lojaLogada));
        }

        [HttpPost("atualizarDados")]
        public IActionResult AtualizarDados(OrcamentoResponseViewModel model)
        {
            var permissao = this.ObterPermissaoOrcamento((int)model.Id);

            if (!permissao.EditarOrcamento)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var retorno = _orcamentoBll.AtualizarDadosCadastraisOrcamento(model, user);
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
    }
}