using InfraBanco.Constantes;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class OrcamentoController : BaseController
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly OrcamentoCotacaoBll _orcamentoBll;

        public OrcamentoController(ILogger<OrcamentoController> logger, OrcamentoCotacaoBll orcamentoBll)
        {
            _logger = logger;
            _orcamentoBll = orcamentoBll;
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
        public IActionResult BuscarConfigValidade()
        {
            _logger.LogInformation("Buscando ConfigValidade");


            eUnidadeNegocio unidadeNegocio = (eUnidadeNegocio)Enum.Parse(typeof(eUnidadeNegocio), LoggedUser.Unidade_negocio);
            var saida = _orcamentoBll.BuscarConfigValidade(unidadeNegocio);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrcamentoRequestViewModel model)
        {
            _logger.LogInformation("Inserindo Orcamento");

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var id = _orcamentoBll.CadastrarOrcamentoCotacao(model, user);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var orcamentoCotacao = _orcamentoBll.PorFiltro(id, user);

            return Ok(orcamentoCotacao);
        }

        [HttpGet("buscarDadosParaMensageria")]
        public IActionResult BuscarDadosParaMensageria(int idOrcamento, bool usuarioInterno)
        {
            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var dados = _orcamentoBll.BuscarDadosParaMensageria(user, idOrcamento, usuarioInterno);
            return Ok(dados);
        }

        [HttpPost("atualizarOrcamentoOpcao")]
        public IActionResult AtualizarOrcamentoOpcao(OrcamentoOpcaoResponseViewModel opcao)
        {
            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            _orcamentoBll.AtualizarOrcamentoOpcao(opcao, user);
            return Ok();
        }

        [HttpPut("{id}/status/{idStatus}")]
        public IActionResult AtualizarStatus(int id,short idStatus)
        {
            return Ok(_orcamentoBll.AtualizarStatus(id, LoggedUser.Id, idStatus));            
        }

        [HttpPost("{id}/prorrogar")]
        public IActionResult ProrrogarOrcamento(int id)
        {
            eUnidadeNegocio e = (eUnidadeNegocio)Enum.Parse(typeof(eUnidadeNegocio), LoggedUser.Unidade_negocio);

            return Ok(_orcamentoBll.ProrrogarOrcamento(id, LoggedUser.Id, (int)e));
        }

        [HttpPost("atualizarDados")]
        public IActionResult AtualizarDados(OrcamentoResponseViewModel model)
        {
            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            _orcamentoBll.AtualizarDadosCadastraisOrcamento(model, user);
            return Ok();
        }
    }
}
