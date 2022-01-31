using ClassesBase;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilsGlobais;

namespace Usuario
{
    public class UsuarioBll : BaseData<Tusuario, TusuarioFiltro>
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        public UsuarioBll(InfraBanco.ContextoBdProvider contextoProvider)
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
                if (obj.vendedor_loja.HasValue)
                {
                    usuario = usuario.Where(x => x.Vendedor_Loja == (obj.vendedor_loja.Value ? 1 : 0));
                }
                if (obj.vendedor_externo.HasValue)
                {
                    usuario = usuario.Where(x => x.Vendedor_Externo == (obj.vendedor_externo.Value ? 1 : 0));
                }
                return usuario.ToList();
            }
        }

        public Tusuario GetById(string id)
        {
            return PorFiltro(new TusuarioFiltro() { id = id }).FirstOrDefault();
        }

    }
}
