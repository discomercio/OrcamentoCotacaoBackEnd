using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OrcamentoCotacaoEmailQueue
{
    public class OrcamentoCotacaoEmailQueueData : BaseData<TorcamentoCotacaoEmailQueue, TorcamentoCotacaoEmailQueueFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoEmailQueueData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoEmailQueue Atualizar(TorcamentoCotacaoEmailQueue obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                db.TorcamentoCotacaoEmailQueue.Update(obj);
                db.SaveChanges();
                db.transacao.Commit();
                return obj;
            }
        }

        public bool Excluir(TorcamentoCotacaoEmailQueue obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoEmailQueue Inserir(TorcamentoCotacaoEmailQueue obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoEmailQueue InserirComTransacao(TorcamentoCotacaoEmailQueue model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoEmailQueue> PorFilroComTransacao(TorcamentoCotacaoEmailQueueFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoEmailQueue> PorFiltro(TorcamentoCotacaoEmailQueueFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var parametros = from L in db.TorcamentoCotacaoEmailQueue
                                     select L;

                    if (obj.Sent.HasValue)
                    {
                        parametros = parametros.Where(x => x.Sent == obj.Sent);
                    }

                    parametros = parametros.Where(x => x.DateScheduled < DateTime.Now);

                    if (obj.Page.HasValue || obj.RecordsPerPage.HasValue)
                    {
                        parametros = parametros.Skip(obj.RecordsPerPage.Value * (obj.Page.Value - 1)).Take(obj.RecordsPerPage.Value);
                    }

                    parametros = parametros.OrderBy(x => x.DateScheduled);

                    return parametros.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
