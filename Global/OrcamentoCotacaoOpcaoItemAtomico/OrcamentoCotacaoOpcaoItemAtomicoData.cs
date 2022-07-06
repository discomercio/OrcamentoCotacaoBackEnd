using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoOpcaoItemAtomico
{
    public class OrcamentoCotacaoOpcaoItemAtomicoData : BaseData<TorcamentoCotacaoOpcaoItemAtomico, TorcamentoCotacaoOpcaoItemAtomicoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoOpcaoItemAtomicoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoOpcaoItemAtomico Atualizar(TorcamentoCotacaoOpcaoItemAtomico obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcaoItemAtomico AtualizarComTransacao(TorcamentoCotacaoOpcaoItemAtomico model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Update(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public bool Excluir(TorcamentoCotacaoOpcaoItemAtomico obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TorcamentoCotacaoOpcaoItemAtomico obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcaoItemAtomico Inserir(TorcamentoCotacaoOpcaoItemAtomico obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcaoItemAtomico InserirComTransacao(TorcamentoCotacaoOpcaoItemAtomico model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentoCotacaoOpcaoItemAtomico> PorFilroComTransacao(TorcamentoCotacaoOpcaoItemAtomicoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            try
            {
                var saida = from c in contextoBdGravacao.TorcamentoCotacaoOpcaoItemAtomico
                            select c;

                if (obj.IdItemUnificado != 0)
                {
                    saida = saida.Where(x => x.IdItemUnificado == obj.IdItemUnificado);
                }

                return saida.ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TorcamentoCotacaoOpcaoItemAtomico> PorFiltro(TorcamentoCotacaoOpcaoItemAtomicoFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = from c in db.TorcamentoCotacaoOpcaoItemAtomico
                                select c;

                    if (saida == null) return null;

                    if (obj.IdItemUnificado != 0)
                    {
                        saida = saida.Where(x => x.IdItemUnificado == obj.IdItemUnificado);
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
