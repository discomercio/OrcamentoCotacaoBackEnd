using ClassesBase;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            var usuarioFiltrado = await (from u in db.Tusuario
                                         join uxl in db.TusuarioXLoja on u.Usuario equals uxl.Usuario
                                         join pu in db.TperfilUsuario on uxl.Usuario equals pu.Usuario
                                         join p in db.Tperfil on pu.Id_perfil equals p.Id
                                         join pi in db.TperfilItem on p.Id equals pi.Id_perfil
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
                var usuario = from usr in db.Tusuario
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
                if(obj.id > 0)
                {
                    usuario = usuario.Where(x => x.Id == obj.id);
                }
                return usuario.ToList();
            }
        }

        public Tusuario GetById(int id)
        {
            return PorFiltro(new TusuarioFiltro() { id = id }).FirstOrDefault();
        }

        public Tusuario InserirComTransacao(Tusuario model, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<Tusuario> PorFilroComTransacao(TusuarioFiltro obj, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<string> buscarPermissoes(string apelido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return (from o in db.Toperacao
                        join pi in db.TperfilItem on o.Id equals pi.Id_operacao
                        join p in db.Tperfil on pi.Id_perfil equals p.Id
                        join pu in db.TperfilUsuario on p.Id equals pu.Id_perfil
                        join u in db.Tusuario on pu.Usuario equals u.Usuario
                        where o.Modulo == "COTAC" && u.Usuario == apelido
                        select o.Id.ToString()).ToList();
            }
        }

        public List<TcfgTipoUsuarioContexto> BuscarTipoUsuarioContexto()
        {
            using(var db = contextoProvider.GetContextoLeitura())
            {
                return (from c in db.TcfgTipoUsuarioContexto
                        select c).ToList();
            }
        }

        public Tusuario AtualizarComTransacao(Tusuario model, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(Tusuario obj, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }
    }
}
