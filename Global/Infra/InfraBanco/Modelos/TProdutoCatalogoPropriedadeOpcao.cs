using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TProdutoCatalogoPropriedadeOpcao : IModel
    {  
        public int id { get; set; }
        public int id_produto_catalogo_propriedade { get; set; }
        public string valor { get; set; }
        public bool oculto { get; set; }
        public int ordem { get; set; }
        public DateTime dt_cadastro { get; set; }
        public string usuario_cadastro { get; set; }

        public TProdutoCatalogoPropriedade TprodutoCatalogoPropriedade { get; set; }

        public List<TprodutoCatalogoItem> TprodutoCatalogoItems { get; set; }
    }
}
