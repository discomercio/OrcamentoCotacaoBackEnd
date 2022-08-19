using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoOpcaoItemAtomicoCustoFin
{
    public class OrcamentoCotacaoOpcaoItemAtomicoCustoFinData : BaseData<TorcamentoCotacaoOpcaoItemAtomicoCustoFin, TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;
        public OrcamentoCotacaoOpcaoItemAtomicoCustoFinData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoOpcaoItemAtomicoCustoFin Atualizar(TorcamentoCotacaoOpcaoItemAtomicoCustoFin obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcaoItemAtomicoCustoFin AtualizarComTransacao(TorcamentoCotacaoOpcaoItemAtomicoCustoFin model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Update(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public bool Excluir(TorcamentoCotacaoOpcaoItemAtomicoCustoFin obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TorcamentoCotacaoOpcaoItemAtomicoCustoFin obj, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TorcamentoCotacaoOpcaoItemAtomicoCustoFin.Remove(obj);
            contextoBdGravacao.SaveChanges();
        }

        public TorcamentoCotacaoOpcaoItemAtomicoCustoFin Inserir(TorcamentoCotacaoOpcaoItemAtomicoCustoFin obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcaoItemAtomicoCustoFin InserirComTransacao(TorcamentoCotacaoOpcaoItemAtomicoCustoFin model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> PorFilroComTransacao(TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            var saida = from c in contextoBdGravacao.TorcamentoCotacaoOpcaoItemAtomicoCustoFin
                        select c;

            if (saida == null) return null;

            if (obj.LstIdItemAtomico != null)
            {
                saida = saida.Where(x => obj.LstIdItemAtomico.Contains(x.IdItemAtomico));
            }
            if (obj.IdItemAtomico > 0)
            {
                saida = saida.Where(x => obj.IdItemAtomico == x.IdItemAtomico);
            }

            return saida.ToList();
        
        }

        public List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> PorFiltro(TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = from c in db.TorcamentoCotacaoOpcaoItemAtomicoCustoFin
                                select c;

                    if (saida == null) return null;

                    if(obj.LstIdItemAtomico != null)
                    {
                        saida = saida.Where(x => obj.LstIdItemAtomico.Contains(x.IdItemAtomico));  
                    }
                    if(obj.IdItemAtomico > 0)
                    {
                        saida = saida.Where(x => obj.IdItemAtomico == x.IdItemAtomico);
                    }
                    if(obj.IdOpcaoPagto > 0)
                    {
                        saida = saida.Where(x => obj.IdOpcaoPagto == x.IdOpcaoPagto);
                    }

                    return saida.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
