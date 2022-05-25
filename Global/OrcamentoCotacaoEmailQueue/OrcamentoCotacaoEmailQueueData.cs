using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
