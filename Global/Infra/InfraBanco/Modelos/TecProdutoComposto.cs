using Interfaces;
using System.Collections.Generic;

namespace InfraBanco.Modelos
{
    public class TecProdutoComposto : IModel
    {
        public string Fabricante_Composto { get; set; }

        public string Produto_Composto { get; set; }

        ////fkProduto
        //public string Fabricante { get; set; }
        //public string Produto{ get; set; }
        //public Tproduto Tproduto { get; set; }

        public ICollection<TecProdutoCompostoItem> TecProdutoCompostoItems { get; set; }
    }
}
