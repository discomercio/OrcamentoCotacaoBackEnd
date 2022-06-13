using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace UtilsGlobais.Usuario
{
    public class UsuarioPermissao
    {
        private readonly List<int> listaPermissoes;
        public List<int> ObterListaPermissoes() => listaPermissoes;

        public bool Operacao_permitida(int permissao)
        {
            return listaPermissoes.Contains(permissao);
        }

        //usamos uma factory porque cosntrutores não podem ser async
        public static async Task<UsuarioPermissao> ConstruirUsuarioPermissao(string usuario, InfraBanco.ContextoBdProvider contextoProvider)
        {
            var listaPermissoes = await BuscaListaOperacoesPermitidas(usuario, contextoProvider);
            return new UsuarioPermissao(listaPermissoes);
        }

        private UsuarioPermissao(List<int> listaPermissoes)
        {
            this.listaPermissoes = listaPermissoes ?? throw new ArgumentNullException(nameof(listaPermissoes));
        }

        private static async Task<List<int>> BuscaListaOperacoesPermitidas(string usuario, InfraBanco.ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lstTask = (from perfil in db.Tperfil
                           join perfilitem in db.TperfilItem on perfil.Id equals perfilitem.Id_perfil
                           join perfilusuario in db.TperfilUsuario on perfil.Id equals perfilusuario.Id_perfil
                           join operacao in db.Toperacao on perfilitem.Id_operacao equals operacao.Id
                           where
                                perfilusuario.Usuario == usuario &&
                                perfil.St_inativo == 0 &&
                                operacao.St_inativo == 0
                           //nao precisamos ordenr
                           //orderby perfilitem.Id_operacao
                           select perfilitem.Id_operacao).Distinct();

            return await lstTask.ToListAsync();
        }
    }
}
