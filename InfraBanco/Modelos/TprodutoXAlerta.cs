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
        //[Key]
        [Column("fabricante")]
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        //[Key]
        [Column("produto")]
        [Required]
        [MaxLength(8)]
        //[ForeignKey("Tproduto")]
        public string Produto { get; set; }

        public Tproduto Tproduto { get; set; }

        //[Key]
        [Column("id_alerta")]
        [Required]
        [MaxLength(12)]
        
        public string Id_Alerta { get; set; }

        [ForeignKey("Id_Alerta")]
        public TalertaProduto TalertaProduto { get; set; }

        [Column("excluido_status")]
        [Required]
        public short Excluido_Status { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("usuario_cadastro")]
        [Required]
        [MaxLength(10)]
        public string Usuario_Cadastro { get; set; }
        
        

    }
}