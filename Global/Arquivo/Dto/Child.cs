using System.Collections.Generic;

namespace Arquivo.Dto
{
    public class Child
    {
        public Data data { get; set; }
        public List<Child> children { get; set; }
    }
}
