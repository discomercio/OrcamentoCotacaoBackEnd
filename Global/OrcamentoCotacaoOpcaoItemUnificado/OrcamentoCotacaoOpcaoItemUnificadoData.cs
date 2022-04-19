using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoOpcaoItemUnificado
{
    public class OrcamentoCotacaoOpcaoItemUnificadoData : BaseData<TorcamentoCotacaoItemUnificado, TorcamentoCotacaoOpcaoItemUnificadoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoOpcaoItemUnificadoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoItemUnificado Atualizar(TorcamentoCotacaoItemUnificado obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentoCotacaoItemUnificado obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoItemUnificado Inserir(TorcamentoCotacaoItemUnificado obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoItemUnificado InserirComTransacao(TorcamentoCotacaoItemUnificado model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentoCotacaoItemUnificado> PorFilroComTransacao(TorcamentoCotacaoOpcaoItemUnificadoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoItemUnificado> PorFiltro(TorcamentoCotacaoOpcaoItemUnificadoFiltro obj)
        {
            throw new NotImplementedException();
        }
    }
}
