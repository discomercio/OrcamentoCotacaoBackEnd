using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class ToperacaoFiltro : IFilter
    {
        public string Descricao { get; set; }
        public string Modulo { get; set; }
    }
}
