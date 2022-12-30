using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Dto.Acesso;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais;
using UtilsGlobais.Configs;

namespace PrepedidoApi.Controllers
{
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    [TypeFilter(typeof(ResourceFilter))]
    [ApiController]
    [Route("api/[controller]")]
    public class AcessoController : ControllerBase
    {
        private readonly IServicoAutenticacao servicoAutenticacao;
        private readonly IConfiguration configuration;
        private readonly Prepedido.Bll.AcessoBll acessoBll;
        private readonly IServicoDecodificarToken servicoDecodificarToken;
        private readonly ILogger<AcessoController> _logger;

        public AcessoController(
            IServicoAutenticacao servicoAutenticacao, 
            IConfiguration configuration, 
            Prepedido.Bll.AcessoBll acessoBll,
            IServicoDecodificarToken servicoDecodificarToken, 
            ILogger<AcessoController> logger)
        {
            this.servicoAutenticacao = servicoAutenticacao;
            this.configuration = configuration;
            this.acessoBll = acessoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this._logger = logger;
        }

        [HttpGet("renovarToken")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult RenovarToken()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. AcessoController/RenovarToken/GET - Request => [Não tem request].");

            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Configuracao>();
            string apelido = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.NameIdentifier).Value.Trim();
            string nome = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Name).Value;
            string loja = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Surname).Value;
            string unidade_negocio = User.Claims.FirstOrDefault(r => r.Type == "unidade_negocio").Value;

            UsuarioLogin objUsuarioLogin = new UsuarioLogin()
            {
                Apelido = apelido,
                Nome = nome,
                Loja = loja,
                Unidade_negocio = unidade_negocio
            };

            objUsuarioLogin = servicoAutenticacao.RenovarTokenAutenticacao(
                objUsuarioLogin, 
                appSettings.SegredoToken, 
                appSettings.ValidadeTokenMinutos,
                Autenticacao.RoleAcesso, 
                out bool unidade_negocio_desconhecida);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. AcessoController/RenovarToken/GET - Response => [{JsonSerializer.Serialize(objUsuarioLogin.Apelido)}].");

            if (unidade_negocio_desconhecida)
            {
                _logger.LogWarning($"RenovarToken unidade_negocio_desconhecida apelido:{apelido}");
                return Forbid();
            }

            if (objUsuarioLogin.Token == null)
                return BadRequest(new { message = "Erro no sistema de autenticação. O usuário pode ter sido editado no banco de dados." });

            return Ok(objUsuarioLogin.Token);
        }

        //este é o único que permite acesso anônimo
        //erro 403: t_LOJA.unidade_negocio desconhecida
        [AllowAnonymous]
        [HttpPost("fazerLogin")]
        public async Task<IActionResult> FazerLogin(LoginDto login)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. AcessoController/FazerLogin/POST - Request => [{login.Apelido}].");

            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Configuracao>();
            string apelido = login.Apelido;
            string senha = login.Senha;
            UsuarioLogin objUsuarioLogin = new UsuarioLogin()
            {
                Apelido = apelido,
                Senha = senha
            };

            objUsuarioLogin = servicoAutenticacao.ObterTokenAutenticacao(
                objUsuarioLogin, 
                appSettings.SegredoToken, 
                appSettings.ValidadeTokenMinutos,
                appSettings.BloqueioUsuarioLoginAmbiente,
                Autenticacao.RoleAcesso, 
                new OrcamentoCotacaoBusiness.PrePedido.Utils.ServicoAutenticacaoProvider(acessoBll),
                string.Empty,
                out bool unidade_negocio_desconhecida);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. AcessoController/FazerLogin/POST - Response => [{JsonSerializer.Serialize(objUsuarioLogin.Apelido)}].");

            if (unidade_negocio_desconhecida)
            {
                _logger.LogWarning($"FazerLogin unidade_negocio_desconhecida apelido:{login.Apelido}");
                return Forbid();
            }

            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string userAgent = Request.Headers["User-agent"];

            if (!string.IsNullOrEmpty(objUsuarioLogin.Token))
                await acessoBll.GravarSessaoComTransacao(ip, apelido, userAgent);

            if (objUsuarioLogin.Token == null)
                return BadRequest(new { message = "Usuário ou senha incorreta." });

            return Ok(objUsuarioLogin.Token);
        }

        [HttpGet("fazerLogout")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> FazerLogout()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. AcessoController/FazerLogout/GET - Request => [Não tem request].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            await acessoBll.FazerLogout(apelido.Trim());

            _logger.LogInformation($"CorrelationId => [{correlationId}]. AcessoController/RenovarToken/GET - Response => [Não tem response].");

            return Ok();
        }

        [HttpPost("alterarSenha")]
        [AllowAnonymous]
        public async Task<IActionResult> AlterarSenha(Prepedido.Dto.AlterarSenhaDto alterarSenhaDto)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. AcessoController/AlterarSenha/POST - Request => [{alterarSenhaDto.Apelido}].");

            var retorno = "";

            if (!string.IsNullOrEmpty(alterarSenhaDto.Apelido))
            {
                retorno = await acessoBll.AlterarSenha(alterarSenhaDto);
            }

            retorno = Newtonsoft.Json.JsonConvert.SerializeObject(retorno);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. AcessoController/AlterarSenha/POST - Response => [{retorno}].");

            return Ok(retorno);
        }
    }
}