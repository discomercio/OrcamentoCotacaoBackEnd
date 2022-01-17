using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacao.Data.Entities
{
    public class ProdutoEstoque
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public int Qtde { get; set; }
        public int QtdeUtilizada { get; set; }
        public short IdnfeEmitente { get; set; }
    }
}
