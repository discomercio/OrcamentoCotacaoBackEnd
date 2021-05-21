using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_LOJA")]
    public class Tloja
    {
        [Column("loja")]
        [Key]
        [Required]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("nome")]
        [MaxLength(30)]
        public string Nome { get; set; }

        [Column("razao_social")]
        [MaxLength(60)]
        public string Razao_Social { get; set; }

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        [Column("comissao_indicacao")]
        public float? Comissao_indicacao { get; set; }

        [Column("perc_max_comissao")]
        public float Perc_Max_Comissao { get; set; }

        [Column("perc_max_comissao_e_desconto")]
        public float Perc_Max_Comissao_E_Desconto { get; set; }

        [Column("perc_max_comissao_e_desconto_nivel2")]
        public float Perc_Max_Comissao_E_Desconto_Nivel2 { get; set; }

        [Column("perc_max_comissao_e_desconto_nivel2_pj")]
        public float Perc_Max_Comissao_E_Desconto_Nivel2_Pj { get; set; }

        [Column("perc_max_comissao_e_desconto_pj")]
        public float Perc_Max_Comissao_E_Desconto_Pj { get; set; }
#endif

        [Column("unidade_negocio")]
        [MaxLength(5)]
        public string Unidade_Negocio { get; set; }

    }
}
