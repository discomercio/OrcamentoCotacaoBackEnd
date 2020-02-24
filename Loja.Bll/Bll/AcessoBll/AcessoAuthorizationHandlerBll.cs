using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Loja.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Loja.Modelos;
using Loja.Bll.Util;

namespace Loja.Bll.Bll.AcessoBll
{
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

        public AcessoAuthorizationHandlerBll(IHttpContextAccessor httpContextAccessor, Configuracao configuracao,
            ClienteBll.ClienteBll clienteBll, UsuarioAcessoBll usuarioAcessoBll)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuracao = configuracao;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
        }

        public static class Roles
        {
            public const string PedidoController = "PedidoController";
            public const string ClienteController = "ClienteController";
            public const string CepController = "CepController";
            public const string ProdutosController = "ProdutosController";
        }

        //todo: retornar os roles corretos
        public static List<string> RolesDoUsuario()
        {
            var ret = new List<string>();
            ret.Add(Roles.PedidoController);
            ret.Add(Roles.ClienteController);
            ret.Add(Roles.CepController);
            ret.Add(Roles.ProdutosController);

            return ret;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AcessoRequirement requirement)
        {
            var usuarioLogado = new Loja.Bll.Bll.AcessoBll.UsuarioLogado(context.User, httpContextAccessor.HttpContext.Session, 
                clienteBll, usuarioAcessoBll, configuracao);
            if (!usuarioLogado.SessaoAtiva)
            {
                //sem login, ou sem sessão, conforme o cirtério do UsuarioLogado
                context.Fail();
                return Task.CompletedTask;
            }
            if(usuarioLogado.SessaoDeslogadaForcado)
            {
                //não permitimos!
                context.Fail();
                return Task.CompletedTask;
            }
            if (AutorizarPagina(context))
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }

        private bool AutorizarPagina(AuthorizationHandlerContext context)
        {
            if (context.User == null)
                return false;
            if (context.Resource == null)
                return true;

            //todo: por enquanto, autorizamos todo mundo
            return true;


            //todo: afazer: atualizar periodicamente as permissões do banco (quer dizer, ler elas novamente)
            //mas não é aqui onde devemos fazer...

            //testar por controller
            //todo: colocar as autorizacoes confrme vem do banco

            if (context.Resource.ToString().Contains("HomeController"))
            {
                //sempre pode
                return true;
            }

            if (context.Resource.ToString().Contains("PedidoController"))
            {
                if (context.User.IsInRole(Roles.PedidoController))
                    return true;
                return false;
            }

            if (context.Resource.ToString().Contains("ClienteController"))
            {
                if (context.User.IsInRole(Roles.ClienteController))
                    return true;
                return false;
            }

            if (context.Resource.ToString().Contains("CepController"))
            {
                if (context.User.IsInRole(Roles.CepController))
                    return true;
                return false;
            }

            if (context.Resource.ToString().Contains("ProdutosController"))
            {
                if (context.User.IsInRole(Roles.ProdutosController))
                    return true;
                return false;
            }

            //nao tratamos, nada feito!
            return false;
        }

    }

}
