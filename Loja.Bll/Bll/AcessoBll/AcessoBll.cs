using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Loja.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Loja.Modelos;

namespace Loja.Bll.Bll.AcessoBll
{
    public class AcessoBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public AcessoBll(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public static class Roles
        {
            public const string PedidoController = "PedidoController";
            public const string ClienteController = "ClienteController";
            public const string CepController = "CepController";
        }

        //todo: retornar os roles corretos
        public static List<string> RolesDoUsuario()
        {
            var ret = new List<string>();
            ret.Add(Roles.PedidoController);
            ret.Add(Roles.ClienteController);
            ret.Add(Roles.CepController);
            return ret;
        }

        public async Task<Tusuario> LoginUsuario(string usuario, string senha, string loja)
        {
            //vamos ver se existe
            usuario = usuario.Trim().ToUpper();
            var existe = await (from u in contextoProvider.GetContextoLeitura().Tusuarios
                                where usuario == u.Usuario.Trim().ToUpper()
                                select u).FirstOrDefaultAsync();
            return existe;
        }
        public static async Task CriarSessao(ClienteBll.ClienteBll clienteBll, string usuario, ISession HttpContextSession)
        {
            string lstOperacoesPermitidas = await clienteBll.BuscaListaOperacoesPermitidas(usuario);
            HttpContextSession.SetString("lista_operacoes_permitidas", lstOperacoesPermitidas);
            HttpContextSession.SetString("usuario", usuario);
        }
        public static string ObterOperacoesPermitidas(ISession HttpContextSession)
        {
            return HttpContextSession.GetString("lista_operacoes_permitidas");
        }
        public static string ObterUsuario(ISession HttpContextSession)
        {
            return HttpContextSession.GetString("usuario");
        }
        public static bool operacao_permitida(int id_operacao, ISession HttpContextSession)
        {
            var permitidas = ObterOperacoesPermitidas(HttpContextSession);
            var s = id_operacao.ToString();

            if (string.IsNullOrWhiteSpace(s))
                return false;
            s = "|" + s + "|";

            if (permitidas.Contains(s))
                return true;

            return false;
        }


        public static bool AutorizarPagina(AuthorizationHandlerContext context)
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

            //nao tratamos, nada feito!
            return false;
        }

    }
}
