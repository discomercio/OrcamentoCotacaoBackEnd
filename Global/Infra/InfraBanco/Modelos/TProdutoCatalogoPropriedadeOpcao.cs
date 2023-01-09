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
        [Column("id")]
        public int id { get; set; }

        [Column("id_produto_catalogo_propriedade")]
        public int id_produto_catalogo_propriedade { get; set; }

        [Column("valor")]
        public string valor { get; set; }

        [Column("oculto")]
        public bool oculto { get; set; }

        [Column("ordem")]
        public int ordem { get; set; }

        [Column("dt_cadastro")]
        public DateTime dt_cadastro { get; set; }

        [Column("usuario_cadastro")]
        public string usuario_cadastro { get; set; }

        public TProdutoCatalogoPropriedade TprodutoCatalogoPropriedade { get; set; }

        public List<TprodutoCatalogoItem> TprodutoCatalogoItems { get; set; }
    }
}
