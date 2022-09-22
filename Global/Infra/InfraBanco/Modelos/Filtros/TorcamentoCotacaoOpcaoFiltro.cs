using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoOpcaoFiltro : IFilter
    {
        public int Id { get; set; }
        public int IdOrcamentoCotacao { get; set; }
    }
}
