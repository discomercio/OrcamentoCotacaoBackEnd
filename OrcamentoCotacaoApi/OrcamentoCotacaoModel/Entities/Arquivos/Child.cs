using System.Collections.Generic;

namespace OrcamentoCotacao.Data.Entities.Arquivos
{
    public class Child
    {
        public Data data { get; set; }
        public List<Child> children { get; set; }
    }
}
