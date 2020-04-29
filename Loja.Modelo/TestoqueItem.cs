//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


//desabilitar o warning CS8632: The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
//#nullable enable
//#nullable disable warnings
#pragma warning disable CS8632

namespace Loja.Modelos
{
    [Table("t_ESTOQUE_ITEM")]
    public class TestoqueItem
    {
        
        [Required]
        [Column("id_estoque")]
        [MaxLength(12)]
        public string Id_estoque { get; set; }

        public Testoque Testoque { get; set; }

        [Column("fabricante")]
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Column("produto")]
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; }

        [Column("qtde")]
        public short? Qtde { get; set; }

        [Column("preco_fabricante")]
        public decimal? Preco_fabricante { get; set; }

        [Column("qtde_utilizada")]
        public short? Qtde_utilizada { get; set; }

        [Column("data_ult_movimento")]
        [Required]
        public DateTime Data_ult_movimento { get; set; }

        [Column("sequencia")]
        public short? Sequencia { get; set; }

        [Column("timestamp")]
        public byte[]? Timestamp { get; }

        [Column("vl_custo2")]
        [Required]
        public decimal Vl_custo2 { get; set; }

        [Column("vl_BC_ICMS_ST")]
        [Required]
        public decimal Vl_BC_ICMS_ST { get; set; }

        [Column("vl_ICMS_ST")]
        [Required]
        public decimal Vl_ICMS_ST { get; set; }

        [Column("ncm")]
        [MaxLength(8)]
        public string Ncm { get; set; }

        [Column("cst")]
        [MaxLength(3)]
        public string Cst { get; set; }

        [Column("st_ncm_cst_herdado_tabela_produto")]
        [Required]
        public byte St_ncm_cst_herdado_tabela_produto { get; set; }

        [Column("ean")]
        [MaxLength(20)]
        public string Ean { get; set; }

        [Column("aliq_ipi")]
        public float? Aliq_ipi { get; set; }

        [Column("aliq_icms")]
        public float? Aliq_icms { get; set; }

        [Column("vl_ipi")]
        public decimal? Vl_ipi { get; set; }

        [Column("preco_origem")]
        public decimal? Preco_origem { get; set; }

        [Column("produto_xml")]
        [MaxLength(60)]
        public string Produto_xml { get; set; }


    }
}
