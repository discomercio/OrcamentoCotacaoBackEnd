using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TecProdutoComposto : IModel
    {
        public string Fabricante_Composto { get; set; }

        public string Produto_Composto { get; set; }

        public Tproduto Tproduto { get; set; }
        public ICollection<TecProdutoCompostoItem> TecProdutoCompostoItems { get; set; }
    }
}
