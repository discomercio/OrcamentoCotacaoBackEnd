using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

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

        public bool Excluir(TorcamentoCotacaoOpcaoItemAtomico obj)
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

        public List<TorcamentoCotacaoOpcaoItemAtomico> PorFiltro(TorcamentoCotacaoOpcaoItemAtomicoFiltro obj)
        {
            throw new NotImplementedException();
        }
    }
}
