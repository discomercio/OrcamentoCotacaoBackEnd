using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Loja.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Loja.Modelos;
using Loja.Bll.Util;
using Microsoft.Extensions.Logging;

#nullable enable

namespace Loja.Bll.Bll.AcessoBll
{
    /*
     * nota: testado com o site no ISS configurado para usar a autenticação do Windows e negar o acesso anônimo
     * 
     * */

    public class AcessoRequirement : IAuthorizationRequirement
    {
        //sim, vazia....
    }

    public class AcessoAuthorizationHandlerBll : AuthorizationHandler<AcessoRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Configuracao configuracao;
        private readonly ClienteBll.ClienteBll clienteBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly ILogger<UsuarioLogado> logger;

        public AcessoAuthorizationHandlerBll(IHttpContextAccessor httpContextAccessor, Configuracao configuracao,
            ClienteBll.ClienteBll clienteBll, UsuarioAcessoBll usuarioAcessoBll, ILogger<UsuarioLogado> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuracao = configuracao;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AcessoRequirement requirement)
        {
            var usuarioLogado = new Loja.Bll.Bll.AcessoBll.UsuarioLogado(logger, context.User, httpContextAccessor.HttpContext.Session,
                clienteBll, usuarioAcessoBll, configuracao);
            if (!usuarioLogado.SessaoAtiva)
            {
                //sem login, ou sem sessão, conforme o cirtério do UsuarioLogado
                context.Fail();
                return Task.CompletedTask;
            }

            if (AutorizarPagina(context, usuarioLogado))
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }

        private bool AutorizarPagina(AuthorizationHandlerContext context, UsuarioLogado usuarioLogado)
        {
            if (context.User == null)
                return false;
            if (context.Resource == null)
                return true;

            //se for um GET, verificamos se exigimos digitar a senha
            //TODO: alguma páginas que são acessadar por ajax, como a do CEP, devem permitir o acesso
            if ((httpContextAccessor?.HttpContext?.Request?.Method?.ToLower() ?? "") == "get")
            {
                if (UsuarioLogado.ClaimsUsuario.ConfirmarSenhaPorGet(context.User.Claims, configuracao))
                {
                    return false;
                }
            }


            //TODO: corresponder páginas e controladores com as permissões do usuário
            //precisamos converter as permisões do usuário em nomes de controllers
            return true;
        }

    }

}
