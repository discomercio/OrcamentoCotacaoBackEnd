using AutoMapper;
using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrcamentistaEindicador;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Interfaces;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Threading.Tasks;
using Usuario;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly IServicoAutenticacao servicoAutenticacao;
        private readonly IConfiguration configuration;
        private readonly OrcamentoCotacaoBusiness.Bll.AcessoBll _acessoBll;
        //private readonly IServicoDecodificarToken servicoDecodificarToken;
        //private readonly ILogger<AccountController> logger;

        private readonly ILogger<AccountController> _logger;
        //private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;
        private readonly UsuarioBll _usuarioBll;
        private readonly OrcamentistaEindicadorBll _orcamentistaEindicadorBll;
        private readonly IMapper _mapper;

        public AccountController(IServicoAutenticacao servicoAutenticacao, IConfiguration configuration, ILogger<AccountController> logger,
            IMapper mapper, OrcamentoCotacaoBusiness.Bll.AcessoBll acessoBll, ITokenService tokenService, UsuarioBll usuarioBll, OrcamentistaEindicadorBll orcamentistaEindicadorBll)
        //IServicoDecodificarToken servicoDecodificarToken, )
        {
            this.servicoAutenticacao = servicoAutenticacao;
            this.configuration = configuration;
            this._acessoBll = acessoBll;
            //this.servicoDecodificarToken = servicoDecodificarToken;
            this._tokenService = tokenService;
            this._usuarioBll = usuarioBll;
            this._orcamentistaEindicadorBll = orcamentistaEindicadorBll;
            this._logger = logger;
            this._mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<object>> Login(LoginRequestViewModel login)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<OrcamentoCotacaoApi.Utils.Configuracao>();
            string apelido = login.Login;
            string senha = login.Senha;
            UsuarioLogin objUsuarioLogin = new UsuarioLogin();
            string usuario = servicoAutenticacao.ObterTokenAutenticacao(apelido, senha, appSettings.SegredoToken, appSettings.ValidadeTokenMinutos, OrcamentoCotacaoApi.Utils.Autenticacao.RoleAcesso, new ServicoAutenticacaoProvider(_acessoBll, _usuarioBll, _orcamentistaEindicadorBll), out bool unidade_negocio_desconhecida, out objUsuarioLogin);

            if (usuario == null)
            {
                return BadRequest(new LoginResponseViewModel
                {
                    Authenticated = false,
                    Created = "",
                    Expiration = "",
                    AccessToken = "",
                    Message = "Usuário ou senha incorretos"
                });
            }

            _logger.LogInformation("Gerando token");
            var token = _tokenService.GenerateToken(objUsuarioLogin);

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(10000);


            var usuarioResponse = _mapper.Map<UsuarioResponseViewModel>(objUsuarioLogin);

            var retorno = new LoginResponseViewModel
            {
                Authenticated = true,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                Usuario = usuarioResponse,
                Message = "OK"
            };
            return Ok(retorno);


            //string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //string userAgent = Request.Headers["User-agent"];
            //if (!string.IsNullOrEmpty(token))
            //    await acessoBll.GravarSessaoComTransacao(ip, apelido, userAgent);

            //if (unidade_negocio_desconhecida)
            //{
            //    logger.LogWarning($"FazerLogin unidade_negocio_desconhecida apelido:{login.Login}");
            //    return Forbid();
            //}




            //if (token == null)
            //    return BadRequest(new { message = "Usuário ou senha incorreta." });


            //_logger.LogInformation("Validando usuario");
            //var usuario = await _usuarioService.Login(model.Login, model.Senha);

            //if (usuario == null)
            //{
            //    return BadRequest(new LoginResponseViewModel
            //    {
            //        Authenticated = false,
            //        Created = "",
            //        Expiration = "",
            //        AccessToken = "",
            //        Message = "Usuário ou senha incorretos"
            //    });
            //}

            //_logger.LogInformation("Gerando token");
            //var token = _tokenService.GenerateToken(usuario);

            //DateTime dataCriacao = DateTime.Now;
            //DateTime dataExpiracao = dataCriacao +
            //    TimeSpan.FromSeconds(10000);

            //var usuarioResponse = _mapper.Map<UsuarioResponseViewModel>(usuario);

            //var retorno = new LoginResponseViewModel
            //{
            //    Authenticated = true,
            //    Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
            //    Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
            //    AccessToken = token,
            //    Usuario = usuarioResponse,
            //    Message = "OK"
            //};
            //return Ok(retorno);
        }


        [HttpGet]
        [Route("permissoes")]
        public async Task<LoginResponseViewModel> BuscarPermissoes()
        {
            var login = User.Identity.Name;
            _logger.LogInformation("Buscando permissoes usuario");
            //var permissoes = await _usuarioService.BuscarPermissoes(login);

            return new LoginResponseViewModel();
        }
    }
}
