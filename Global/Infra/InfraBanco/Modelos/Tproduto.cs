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
        [Column("fabricante")]
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        public Tfabricante Tfabricante { get; set; }

        [Column("produto")]
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; }

        [Column("descricao")]
        [MaxLength(120)]
        public string Descricao { get; set; }

        [Column("ean")]
        [MaxLength(14)]
        public string Ean { get; set; }

        [Column("grupo")]
        [MaxLength(4)]
        public string Grupo { get; set; }

        [Column("preco_fabricante", TypeName = "money")]
        public decimal? Preco_Fabricante { get; set; }

        [Column("peso")]
        public float? Peso { get; set; }

        [Column("qtde_volumes")]
        public short? Qtde_Volumes { get; set; }

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

        [Column("descontinuado")]
        [MaxLength(1)]
        public string Descontinuado { get; set; }

        [Column("subgrupo")]
        [MaxLength(10)]
        public string Subgrupo { get; set; }

        [Column("excluido_status")]
        public short? Excluido_status { get; set; }

        [Column("potencia_BTU")]
        public int? PotenciaBtu { get; set; }

        [Column("ciclo")]
        [MaxLength(5)]
        public string Ciclo { get; set; }

        public ICollection<TprodutoLoja> TprodutoLoja { get; set; }
    }
}