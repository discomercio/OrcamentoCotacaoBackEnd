using ClassesBase;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Usuario
{
    public class UsuarioData : BaseData<Tusuario, TusuarioFiltro>
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        public UsuarioData(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }
        public Tusuario Atualizar(Tusuario obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(Tusuario obj)
        {
            throw new NotImplementedException();
        }

        public Tusuario Inserir(Tusuario obj)
        {
            throw new NotImplementedException();
        }

        public List<Tusuario> PorFiltro(TusuarioFiltro obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var usuario = from usr in db.Tusuarios
                              select usr;
                if (!string.IsNullOrEmpty(obj.usuario))
                {
                    usuario = usuario.Where(x => x.Usuario == obj.usuario);
                }
                if (!string.IsNullOrEmpty(obj.senha))
                {
                    usuario = usuario.Where(x => x.Senha == obj.senha);
                }
                if (obj.bloqueado.HasValue)
                {
                    usuario = usuario.Where(x => x.Bloqueado == (obj.bloqueado.Value ? 1 : 0));
                }
                return usuario.ToList();
            }
        }
    }
}
