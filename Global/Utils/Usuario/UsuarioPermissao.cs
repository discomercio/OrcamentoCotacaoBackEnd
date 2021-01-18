using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG

namespace UtilsGlobais.Usuario
{
    public class UsuarioPermissao
    {
        private readonly List<int> listaPermissoes;
        public List<int> ObterListaPermissoes() => listaPermissoes;

        public bool Permitido(int permissao)
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
            //            SELECT DISTINCT id_operacao
            //FROM t_PERFIL
            //INNER JOIN t_PERFIL_ITEM ON t_PERFIL.id = t_PERFIL_ITEM.id_perfil
            //INNER JOIN t_PERFIL_X_USUARIO ON t_PERFIL.id = t_PERFIL_X_USUARIO.id_perfil
            //INNER JOIN t_OPERACAO ON(t_PERFIL_ITEM.id_operacao= t_OPERACAO.id)
            //WHERE(t_PERFIL_X_USUARIO.usuario = 'pragmatica') AND
            //      (t_PERFIL.st_inativo = 0) AND
            //      (t_OPERACAO.st_inativo = 0)
            //ORDER BY id_operacao

            /*
            var lstTask = (from operacao in db.Toperacaos
                           join perfilitem in db.TperfilItens on operacao.Id equals perfilitem.Id_operacao
                           join perfil in db.Tperfils on perfilitem.Id_perfil equals perfil.Id
                           join perfilusuario in db.TperfilUsuarios on perfil.Id equals perfilusuario.Id_perfil
                           where
                                perfilusuario.Usuario == usuario &&
                                perfil.St_inativo == 0 &&
                                operacao.St_inativo == 0
                           //nao precisamos ordenr
                           //orderby perfilitem.Id_operacao
                           select perfilitem.Id_operacao).Distinct();
            */

            var lstTask = (from perfil in db.Tperfils
                           join perfilitem in db.TperfilItens on perfil.Id equals perfilitem.Id_perfil
                           join perfilusuario in db.TperfilUsuarios on perfil.Id equals perfilusuario.Id_perfil
                           join operacao in db.Toperacaos on perfilitem.Id_operacao equals operacao.Id
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

#endif

