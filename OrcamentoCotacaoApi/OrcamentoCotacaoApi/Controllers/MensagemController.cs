﻿using InfraIdentity;
using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MensagemController : BaseController
    {
        private readonly ILogger<MensagemController> _logger;
        private readonly MensagemOrcamentoCotacaoBll _mensagemBll;

        public MensagemController(ILogger<MensagemController> logger, MensagemOrcamentoCotacaoBll mensagemBll)
        {
            _logger = logger;
            _mensagemBll = mensagemBll;
        }

        [HttpGet()]
        public async Task<IActionResult> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("ObterListaMensagem");

            var saida = await _mensagemBll.ObterListaMensagem(IdOrcamentoCotacao);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("pendente")]
        public async Task<IActionResult> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("ObterListaMensagemPendente");

            var saida = await _mensagemBll.ObterListaMensagemPendente(IdOrcamentoCotacao);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpPost()]
        public async Task<IActionResult> EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            _logger.LogInformation("EnviarMensagem");

            var saida = _mensagemBll.EnviarMensagem(orcamentoCotacaoMensagem);

            if (saida)
            {
                return Ok(new
                {
                    message = "Mensagem criada com sucesso."
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Não foi possível criar a mensagem."
                });
            }
        }

        [HttpGet("pendente/quantidade")]
        public int ObterQuantidadeMensagemPendente()
        {
            _logger.LogInformation("ObterQuantidadeMensagemPendente");
            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var saida = _mensagemBll.ObterQuantidadeMensagemPendente(user.Id);

            return saida;
        }

        [HttpPut]
        [Route("marcar/lida")]
        public async Task<IActionResult> MarcarLida(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("MarcarLida");

            var saida = _mensagemBll.MarcarLida(IdOrcamentoCotacao);

            if (saida)
            {
                return Ok(new
                {
                    message = "Mensagens marcadas como lida"
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Não foi possível marcar como lida."
                });
            }
        }
        
        [HttpPut]
        [Route("marcar/pendencia")]
        public async Task<IActionResult> MarcarPendencia(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("MarcarPendencia");

            var saida = _mensagemBll.MarcarPendencia(IdOrcamentoCotacao);

            if (saida)
            {
                return Ok(new
                {
                    message = "Mensagens marcadas como pendência tratada."
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Mensagens marcadas como pendência tratada."
                });
            }
        }

        [HttpPut]
        [Route("desmarcar/pendencia")]
        public async Task<IActionResult> DesmarcarPendencia(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("DesmarcarPendencia");

            var saida = _mensagemBll.DesmarcarPendencia(IdOrcamentoCotacao);

            if (saida)
            {
                return Ok(new
                {
                    message = "Mensagens desmarcadas como pendência tratada."
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Mensagens desmarcadas como pendência tratada."
                });
            }
        }

    }
}
