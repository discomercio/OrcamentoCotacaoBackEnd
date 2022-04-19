using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoOpcaoPagto
{
    public class OrcamentoCotacaoOpcaoPagtoData : BaseData<TorcamentoCotacaoOpcaoPagto, TorcamentoCotacaoOpcaoPagtoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoOpcaoPagtoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoOpcaoPagto Atualizar(TorcamentoCotacaoOpcaoPagto obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentoCotacaoOpcaoPagto obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcaoPagto Inserir(TorcamentoCotacaoOpcaoPagto obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcaoPagto InserirComTransacao(TorcamentoCotacaoOpcaoPagto model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentoCotacaoOpcaoPagto> PorFilroComTransacao(TorcamentoCotacaoOpcaoPagtoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoOpcaoPagto> PorFiltro(TorcamentoCotacaoOpcaoPagtoFiltro obj)
        {
            throw new NotImplementedException();
        }
    }
}
