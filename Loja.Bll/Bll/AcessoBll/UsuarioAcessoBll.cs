using Loja.Bll.Util;
using Loja.Data;
using Loja.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioAcessoBll
    {
        private readonly ContextoBdProvider contextoProvider;
        private readonly ClienteBll.ClienteBll clienteBll;

        public UsuarioAcessoBll(ContextoBdProvider contextoProvider, ClienteBll.ClienteBll clienteBll)
        {
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
        }

        public class LoginUsuarioRetorno
        {
            public Tusuario tusuario;
            public bool sucesso;
            public bool deslogouLoginAnterior;
        }
        public async Task<LoginUsuarioRetorno> LoginUsuario(string usuario, string senha, string loja, ISession HttpContextSession, Configuracao configuracao)
        {
            var ret = new LoginUsuarioRetorno();
            ret.sucesso = false;
            ret.deslogouLoginAnterior = false;
            ret.tusuario = null;

            //vamos ver se existe
            usuario = usuario.Trim().ToUpper();
            var existe = await (from u in contextoProvider.GetContextoLeitura().Tusuarios
                                where usuario == u.Usuario.Trim().ToUpper()
                                select u).FirstOrDefaultAsync();
            if (existe == null)
                return ret;

            ret.tusuario = existe;
            ret.sucesso = true;

            //cria a session
            await UsuarioLogado.CriarSessao(usuario, HttpContextSession, clienteBll, this, configuracao);
            ret.deslogouLoginAnterior = new UsuarioSessoes().DeslogarLoginAnterior(usuario, HttpContextSession);
            return ret;
        }

        public class LojaPermtidaUsuario
        {
            public string Nome { get; set; }
            public string Id { get; set; }
        }
        public async Task<List<LojaPermtidaUsuario>> Loja_troca_rapida_monta_itens_select_a_partir_banco(string strUsuario, string id_default)
        {
            var ret = new List<LojaPermtidaUsuario>();

            if (string.IsNullOrWhiteSpace(strUsuario))
                return ret;

            var query = from usuarioXloja in contextoProvider.GetContextoLeitura().TusuarioXLojas
                        where usuarioXloja.Usuario == strUsuario
                        select new LojaPermtidaUsuario() { Id = usuarioXloja.Tloja.Loja, Nome = usuarioXloja.Tloja.Nome };
            var lista = await query.ToListAsync();

            //'	LEMBRE-SE: O USUÁRIO QUE TEM PERMISSÃO DE ACESSO A TODAS AS LOJAS PODE
            //'	ACESSAR UMA LOJA QUE NÃO ESTÁ CADASTRADA EM t_USUARIO_X_LOJA
            if (!string.IsNullOrWhiteSpace(id_default))
            {
                var lojaDefaultQuery = from loja in contextoProvider.GetContextoLeitura().Tlojas
                                       where loja.Loja == id_default
                                       select new LojaPermtidaUsuario() { Id = loja.Loja, Nome = loja.Nome };
                var lojaDefault = await lojaDefaultQuery.FirstOrDefaultAsync();
                if (lojaDefault != null)
                    lista.Add(lojaDefault);
            }
            return lista;
        }
    }
}

