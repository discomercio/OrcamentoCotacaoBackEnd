using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TecProdutoCompostoItem : IModel
    {
        public string Fabricante_composto { get; set; }
        public string Produto_composto { get; set; }
        public string Fabricante_item { get; set; }
        public string Produto_item { get; set; }
        public short Qtde { get; set; }
        public short? Sequencia { get; set; }
        public short Excluido_status { get; set; }

        public TecProdutoComposto TecProdutoComposto { get; set; }
    }
}
