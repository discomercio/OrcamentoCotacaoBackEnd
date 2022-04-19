using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

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

        public bool Excluir(TorcamentoCotacaoOpcaoItemAtomicoCustoFin obj)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> PorFiltro(TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro obj)
        {
            throw new NotImplementedException();
        }
    }
}
