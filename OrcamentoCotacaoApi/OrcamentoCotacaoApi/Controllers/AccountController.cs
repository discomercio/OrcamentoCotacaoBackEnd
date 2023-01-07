using InfraBanco.Constantes;
using InfraIdentity;
using Loja;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrcamentistaEindicador;
using OrcamentistaEIndicadorVendedor;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.Login;
using OrcamentoCotacaoBusiness.Models.Response.Login;
using Prepedido.Dto;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Usuario;
using UtilsGlobais;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(ResourceFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
    public class AccountController : BaseController
    {
        private readonly IServicoAutenticacao _servicoAutenticacao;
        private readonly IConfiguration _configuration;
        private readonly OrcamentoCotacaoBusiness.Bll.AcessoBll _acessoBll;
        private readonly ILogger<AccountController> _logger;
        private readonly UsuarioBll _usuarioBll;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEindicadorBll;
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll;
        private readonly LojaBll _lojaBll;

        public AccountController(
            IServicoAutenticacao servicoAutenticacao, 
            IConfiguration configuration, 
            ILogger<AccountController> logger,
            OrcamentoCotacaoBusiness.Bll.AcessoBll acessoBll, 
            ITokenService tokenService, 
            UsuarioBll usuarioBll, 
            OrcamentistaEIndicadorBll orcamentistaEindicadorBll,
            OrcamentistaEIndicadorVendedorBll orcamentistaEindicadorVendedorBll, 
            LojaBll lojaBll)
        {
            _servicoAutenticacao = servicoAutenticacao;
            _configuration = configuration;
            _acessoBll = acessoBll;
            _usuarioBll = usuarioBll;
            _orcamentistaEindicadorBll = orcamentistaEindicadorBll;
            _orcamentistaEindicadorVendedorBll = orcamentistaEindicadorVendedorBll;
            _lojaBll = lojaBll;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginRequest login)
        {
            try
            {
                login.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
                login.IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _logger.LogInformation($"CorrelationId => [{login.CorrelationId}]. AccountController/Login/POST - Request => [{JsonSerializer.Serialize(login.Usuario)}].");

                var appSettingsSection = _configuration.GetSection("AppSettings");
                var appSettings = appSettingsSection.Get<Configuracao>();
                string apelido = login.Usuario.ToUpper();
                string senha = login.Senha;

                var objUsuarioLogin = new UsuarioLogin()
                {
                    Apelido = apelido,
                    Senha = senha
                };

                objUsuarioLogin = _servicoAutenticacao.ObterTokenAutenticacao(
                    objUsuarioLogin, 
                    appSettings.SegredoToken, 
                    appSettings.ValidadeTokenMinutos,
                    appSettings.BloqueioUsuarioLoginAmbiente,
                    Autenticacao.RoleAcesso, 
                    new ServicoAutenticacaoProvider(_acessoBll, _usuarioBll, _orcamentistaEindicadorBll, _orcamentistaEindicadorVendedorBll, _lojaBll),
                    login.IP,
                    out bool unidade_negocio_desconhecida);

                var response = new LoginResponse();
                response.Sucesso = false;
                response.Created = "";
                response.Expiration = "";
                response.AccessToken = "";

                if (objUsuarioLogin == null || objUsuarioLogin.IdErro == int.Parse(Constantes.ERR_SENHA_INVALIDA))
                {
                    response.Mensagem = "Usuário ou senha incorretos";
                    _logger.LogInformation($"CorrelationId => [{login.CorrelationId}]. AccountController/Login/POST - {response.Mensagem}. Response => [{JsonSerializer.Serialize(objUsuarioLogin)}].");
                    return Ok(response);
                }

                if (objUsuarioLogin.IdErro == int.Parse(Constantes.ERR_USUARIO_BLOQUEADO))
                {
                    response.Mensagem = "Usuario bloqueado";
                    _logger.LogInformation($"CorrelationId => [{login.CorrelationId}]. AccountController/Login/POST - {response.Mensagem}. Response => [{JsonSerializer.Serialize(objUsuarioLogin)}]");
                    return Ok(response);
                }

                if (objUsuarioLogin.IdErro == int.Parse(Constantes.ERR_ACESSO_INSUFICIENTE))
                {
                    response.Mensagem = "Usuário não habilitado para acesso ao sistema";
                    _logger.LogInformation($"CorrelationId => [{login.CorrelationId}]. AccountController/Login/POST - {response.Mensagem}. Response => [{JsonSerializer.Serialize(objUsuarioLogin)}]");
                    return Ok(response);
                }

                if (objUsuarioLogin.IdErro == int.Parse(Constantes.ERR_USUARIO_INATIVO))
                {
                    response.Mensagem = "Usuário Inativo";
                    _logger.LogInformation($"CorrelationId => [{login.CorrelationId}]. AccountController/Login/POST - {response.Mensagem}. Response => [{JsonSerializer.Serialize(objUsuarioLogin)}]");
                    return Ok(response);
                }

                if (objUsuarioLogin.Token == null)
                {
                    response.Mensagem = "Usuário ou senha incorretos";
                    _logger.LogInformation($"CorrelationId => [{login.CorrelationId}]. AccountController/Login/POST - {response.Mensagem}. Response => [{JsonSerializer.Serialize(objUsuarioLogin)}].");
                    return Ok(response);
                }

                _logger.LogInformation($"CorrelationId => [{login.CorrelationId}]. AccountController/Login/POST - Gerando token de autenticação.");

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(10000);

                if (objUsuarioLogin.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR &&
                    !objUsuarioLogin.Permissoes.Contains(((int)Constantes.ePermissoes.ACESSO_AO_MODULO_100100).ToString()))
                {
                    response.Mensagem = "Usuário não possui acesso ao Módulo";
                    _logger.LogInformation($"CorrelationId => [{login.CorrelationId}]. AccountController/Login/POST - {response.Mensagem}. Response => [{JsonSerializer.Serialize(objUsuarioLogin)}]");
                    return Ok(response);
                }

                response.Sucesso = true;
                response.Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss");
                response.Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss");
                response.AccessToken = objUsuarioLogin.Token;

                var ret = new
                {
                    Usuario = objUsuarioLogin,
                    Sucesso = response.Sucesso,
                    Created = response.Created,
                    Expiration = response.Expiration,
                    AccessToken = response.AccessToken
                };

                _logger.LogInformation($"CorrelationId => [{login.CorrelationId}]. AccountController/Login/POST - Response => [{JsonSerializer.Serialize(ret)}].");

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("expiracao")]
        public async Task<ActionResult<object>> VerificarExpiracao(ExpiracaoSenhaRequestViewModel request)
        {
            try
            {
                var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

                _logger.LogInformation($"CorrelationId => [{correlationId}]. AccountController/VerificarExpiracao/POST - Request => [{JsonSerializer.Serialize(request)}].");

                var response = await _acessoBll.VerificarExpiracao(new AtualizarSenhaDto()
                {
                    TipoUsuario = request.TipoUsuario,
                    Apelido = request.Apelido
                });

                _logger.LogInformation($"CorrelationId => [{correlationId}]. AccountController/VerificarExpiracao/POST - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("AtualzarSenha")]
        public async Task<ActionResult<object>> AtualzarSenha(AtualizarSenhaRequestViewModel request)
        {
            try
            {
                var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

                _logger.LogInformation($"CorrelationId => [{correlationId}]. AccountController/AtualzarSenha/POST - Request => [{JsonSerializer.Serialize(request.Apelido)}].");

                var response = await _acessoBll.AtualizarSenhaAsync(new AtualizarSenhaDto()
                {
                    TipoUsuario = request.TipoUsuario,
                    Apelido = request.Apelido,
                    Senha = request.Senha,
                    NovaSenha = request.NovaSenha,
                    ConfirmacaoSenha = request.ConfirmacaoSenha
                });

                _logger.LogInformation($"CorrelationId => [{correlationId}]. AccountController/AtualzarSenha/POST - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}