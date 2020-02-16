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

        public class Loja
        {
            public string Nome { get; set; }
            public string Id { get; set; }
        }
        public async Task<List<Loja>> Loja_troca_rapida_monta_itens_select(string strUsuario, string id_default)
        {
            var ret = new List<Loja>();

            if (string.IsNullOrWhiteSpace(strUsuario))
                return ret;

            var query = from usuarioXloja in contextoProvider.GetContextoLeitura().TusuarioXLojas
                        where usuarioXloja.Usuario == strUsuario
                        select new Loja() { Id = usuarioXloja.Tloja.Loja, Nome = usuarioXloja.Tloja.Nome };
            var lista = await query.ToListAsync();

            //'	LEMBRE-SE: O USUÁRIO QUE TEM PERMISSÃO DE ACESSO A TODAS AS LOJAS PODE
            //'	ACESSAR UMA LOJA QUE NÃO ESTÁ CADASTRADA EM t_USUARIO_X_LOJA
            if (!string.IsNullOrWhiteSpace(id_default))
            {
                var lojaDefaultQuery = from loja in contextoProvider.GetContextoLeitura().Tlojas
                                       where loja.Loja == id_default
                                       select new Loja() { Id = loja.Loja, Nome = loja.Nome };
                var lojaDefault = await lojaDefaultQuery.FirstOrDefaultAsync();
                if (lojaDefault != null)
                    lista.Add(lojaDefault);
            }
            return lista;
        }
    }
}

