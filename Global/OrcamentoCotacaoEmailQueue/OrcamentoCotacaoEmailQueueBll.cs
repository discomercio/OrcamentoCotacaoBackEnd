using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;

namespace OrcamentoCotacaoEmailQueue
{
    public class OrcamentoCotacaoEmailQueueBll : BaseBLL<TorcamentoCotacaoEmailQueue, TorcamentoCotacaoEmailQueueFiltro>
    {
        private OrcamentoCotacaoEmailQueueData _data { get; set; }

        public OrcamentoCotacaoEmailQueueBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentoCotacaoEmailQueueData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoEmailQueueData(contextoBdProvider);
        }
    }
}
