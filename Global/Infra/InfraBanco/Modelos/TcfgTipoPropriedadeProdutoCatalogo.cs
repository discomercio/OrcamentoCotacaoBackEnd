using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CFG_TIPO_PROPRIEDADE_PRODUTO_CATALOGO")]
    public class TcfgTipoPropriedadeProdutoCatalogo : IModel
    {
        public short Id { get; set; }
        public string Sigla { get; set; }
        public string Descricao { get; set; }

        public List<TProdutoCatalogoPropriedade> TprodutoCatalogoPropriedades { get; set; }
    }
}
