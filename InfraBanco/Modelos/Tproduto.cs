using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO")]
    public class Tproduto
    {
        //        [Key]
        [Column("fabricante")]
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        //[ForeignKey("fabricante")]
        public Tfabricante Tfabricante { get; set; }

        //        [Key]
        [Column("produto")]
        [Required]
        [MaxLength(8)]
        //[ForeignKey("TprodutoLoja")]
        public string Produto { get; set; }

        public ICollection<TprodutoLoja> TprodutoLoja { get; set; }

        [Column("descricao")]
        [MaxLength(120)]
        public string Descricao { get; set; }

        [Column("ean")]
        [MaxLength(14)]
        public string Ean { get; set; }

        [Column("grupo")]
        [MaxLength(2)]
        public string Grupo { get; set; }

        [Column("preco_fabricante", TypeName = "money")]
        public decimal? Preco_Fabricante { get; set; }

        [Column("estoque_critico")]
        public short? Estoque_Critico { get; set; }

        [Column("peso")]
        public float? Peso { get; set; }

        [Column("qtde_volumes")]
        public short? Qtde_Volumes { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_Ult_Atualizacao { get; set; }

        [Column("excluido_status")]
        public short? Excluido_Status { get; set; }

        [Column("vl_custo2", TypeName = "money")]
        [Required]
        public decimal Vl_Custo2 { get; set; }

        [Column("descricao_html")]
        [MaxLength(4000)]
        public string Descricao_Html { get; set; }

        [Column("cubagem")]
        [Required]
        public float Cubagem { get; set; }

        [Column("ncm")]
        [MaxLength(8)]
        public string Ncm { get; set; }

        [Column("cst")]
        [MaxLength(3)]
        public string Cst { get; set; }

        [Column("perc_MVA_ST")]
        [Required]
        public float Perc_MVA_ST { get; set; }

        [Column("deposito_zona_id")]
        [Required]
        public int Deposito_Zona_Id { get; set; }

        [Column("deposito_zona_usuario_ult_atualiz")]
        [MaxLength(10)]
        public string Deposito_Zona_Usuario_Ult_Atualiz { get; set; }

        [Column("deposito_zona_dt_hr_ult_atualiz")]
        public DateTime? Deposito_Zona_Dt_Hr_Ult_Atualiz { get; set; }

        [Column("farol_qtde_comprada")]
        [Required]
        public int Farol_Qtde_Comprada { get; set; }

        [Column("farol_qtde_comprada_usuario_ult_atualiz")]
        [MaxLength(10)]
        public string Farol_Qtde_Comprada_Usuario_Ult_Atualiz { get; set; }

        [Column("farol_qtde_comprada_dt_hr_ult_atualiz")]
        public DateTime? Farol_Qtde_Comprada_Dt_Hr_Ult_Atualiz { get; set; }

        [Column("descontinuado")]
        [MaxLength(1)]
        public string Descontinuado { get; set; }

        [Column("potencia_BTU")]
        [Required]
        public int Potencia_BTU { get; set; }

        [Column("ciclo")]
        [MaxLength(5)]
        public string Ciclo { get; set; }

        [Column("posicao_mercado")]
        [MaxLength(10)]
        public string Posicao_Mercado { get; set; }



    }
}