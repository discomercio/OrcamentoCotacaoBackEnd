using Loja.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioAcessoBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public UsuarioAcessoBll(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
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

