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

        public async Task<List<Tusuario>> FiltrarPorPerfil(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            var usuarioFiltrado = await(from u in db.Tusuarios
                                  join uxl in db.TusuarioXLojas on u.Usuario equals uxl.Usuario
                                  join pu in db.TperfilUsuarios on uxl.Usuario equals pu.Usuario
                                  join p in db.Tperfils on pu.Id_perfil equals p.Id
                                  join pi in db.TperfilItens on p.Id equals pi.Id_perfil
                                  where uxl.Loja == loja &&
                                        pi.Id_operacao == Constantes.OP_LJA_CADASTRA_NOVO_PEDIDO &&
                                        u.Bloqueado == 0
                                  select u).Distinct().ToListAsync();

            return usuarioFiltrado;
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

                if (obj.Page.HasValue)
                {
                    usuario = usuario.Skip(obj.RecordsPerPage.Value * (obj.Page.Value - 1)).Take(obj.RecordsPerPage.Value);
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
