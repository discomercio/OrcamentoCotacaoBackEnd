using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginHistorico
{
    public class LoginHistoricoData : BaseData<TloginHistorico, TloginHistoricoFiltro>
    {
        private readonly ContextoBdProvider contextoBdProvider;

        public LoginHistoricoData(ContextoBdProvider contextoBdProvider)
        {
            this.contextoBdProvider = contextoBdProvider;
        }

        public TloginHistorico Atualizar(TloginHistorico obj)
        {
            throw new NotImplementedException();
        }

        public TloginHistorico AtualizarComTransacao(TloginHistorico model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TloginHistorico obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TloginHistorico obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TloginHistorico Inserir(TloginHistorico obj)
        {
            throw new NotImplementedException();
        }

        public TloginHistorico InserirComTransacao(TloginHistorico model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TloginHistorico> PorFilroComTransacao(TloginHistoricoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TloginHistorico> PorFiltro(TloginHistoricoFiltro obj)
        {
            using(var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var saida = from c in db.TloginHistorico
                            select c;

                if(obj.IdUsuario > 0)
                {
                    saida = saida.Where(x => x.IdUsuario == obj.IdUsuario);
                }

                return saida.ToList();
            }
            throw new NotImplementedException();
        }
    }
}
