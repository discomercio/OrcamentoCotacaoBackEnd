using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TusuarioFiltro : IFilter
    {
        public int id;
        public string usuario { get; set; }
        public string loja { get; set; }
        public string senha { get; set; }
        public bool? bloqueado { get; set; }        
        public bool? vendedor_loja { get; set; }
        public bool? vendedor_externo { get; set; }
    }
}
