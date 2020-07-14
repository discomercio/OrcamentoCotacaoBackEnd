using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_X_ALERTA")]
    public class TprodutoXAlerta
    {
        [Column("fabricante")]
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Column("produto")]
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; }

        public Tproduto Tproduto { get; set; }

        [Column("id_alerta")]
        [Required]
        [MaxLength(12)]
        public string Id_Alerta { get; set; }

        [ForeignKey("Id_Alerta")]
        public TalertaProduto TalertaProduto { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }
    }
}