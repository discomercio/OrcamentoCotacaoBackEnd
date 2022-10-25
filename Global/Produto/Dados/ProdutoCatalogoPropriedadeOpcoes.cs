using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Produto.Dados
{
    public class ProdutoCatalogoPropriedadeOpcoesDados
    {
        public int id { get; set; }
        public int id_produto_catalogo_propriedade { get; set; }
        public string valor { get; set; }
        public bool oculto { get; set; }
        public int ordem { get; set; }
        public DateTime dt_cadastro { get; set; }
        public string usuario_cadastro { get; set; }

    }
}








