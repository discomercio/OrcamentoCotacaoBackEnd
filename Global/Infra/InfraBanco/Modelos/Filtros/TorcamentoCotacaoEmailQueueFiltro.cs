using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoEmailQueueFiltro : IFilter
    {
        public long? Id { get; set; }
        public bool? Sent { get; set; }
        public bool UseDateScheduled { get; set; }
    }
}