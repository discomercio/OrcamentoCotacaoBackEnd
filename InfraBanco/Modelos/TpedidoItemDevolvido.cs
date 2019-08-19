using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO_ITEM_DEVOLVIDO")]
    public class TpedidoItemDevolvido
    {
        [Column("id")]
        [Key]
        [Required]
        [MaxLength(12)]
        public string Id { get; set; }

        [Column("devolucao_data")]
        public DateTime? Devolucao_Data { get; set; }

        [Column("devolucao_hora")]
        [MaxLength(6)]
        public string Devolucao_Hora { get; set; }

        [Column("devolucao_usuario")]
        [MaxLength(10)]
        public string Devolucao_Usuario { get; set; }

        [Column("pedido")]
        [Required]
        [MaxLength(9)]
        public string Pedido { get; set; }

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

        [Column("desc_dado")]
        public float? Desc_Dado { get; set; }

        [Column("preco_venda")]
        [Required]
        public decimal Preco_Venda { get; set; }

        [Column("preco_fabricante")]
        public decimal? Preco_Fabricante { get; set; }

        [Column("preco_lista")]
        [Required]
        public decimal Preco_Lista { get; set; }

        [Column("margem")]
        public float? Margem { get; set; }

        [Column("desc_max")]
        public float? Desc_Max { get; set; }

        [Column("[comissao]")]
        public float? Comissao { get; set; }

        [Column("descricao")]
        [MaxLength(120)]
        public string Descricao { get; set; }

        [Column("ean")]
        [MaxLength(14)]
        public string Ean { get; set; }

        [Column("grupo")]
        [MaxLength(2)]
        public string Grupo { get; set; }

        [Column("peso")]
        public float? Peso { get; set; }

        [Column("qtde_volumes")]
        public short? Qtde_Volumes { get; set; }

        [Column("abaixo_min_status")]
        public short? Abaixo_Min_Status { get; set; }

        [Column("abaixo_min_autorizacao")]
        [MaxLength(12)]
        public string Abaixo_Min_Autorizacao { get; set; }

        [Column("abaixo_min_autorizador")]
        [MaxLength(10)]
        public string Abaixo_Min_Autorizador { get; set; }

        [Column("markup_fabricante")]
        public float? Markup_Fabricante { get; set; }

        [Column("motivo")]
        [MaxLength(80)]
        public string Motivo { get; set; }

        [Column("preco_NF")]
        public decimal? Preco_NF { get; set; }

        [Column("comissao_descontada")]
        [Required]
        public short Comissao_Descontada { get; set; }

        [Column("comissao_descontada_ult_op")]
        [MaxLength(1)]
        public string Comissao_Descontada_Ult_Op { get; set; }

        [Column("comissao_descontada_data")]
        public DateTime? Comissao_Descontada_Data { get; set; }

        [Column("comissao_descontada_usuario")]
        [MaxLength(10)]
        public string Comissao_Descontada_Usuario { get; set; }

        [Column("abaixo_min_superv_autorizador")]
        [MaxLength(10)]
        public string Abaixo_Min_Superv_Autorizador { get; set; }

        [Column("vl_custo2")]
        [Required]
        public decimal Vl_Custo2 { get; set; }

        [Column("descricao_html")]
        [MaxLength(4000)]
        public string Descricao_Html { get; set; }

        [Column("custoFinancFornecCoeficiente")]
        [Required]
        public float CustoFinancFornecCoeficiente { get; set; }

        [Column("custoFinancFornecPrecoListaBase")]
        [Required]
        public decimal CustoFinancFornecPrecoListaBase { get; set; }

        [Column("cubagem")]
        [Required]
        public float Cubagem { get; set; }

        [Column("ncm")]
        [MaxLength(8)]
        public string Ncm { get; set; }

        [Column("cst")]
        [MaxLength(3)]
        public string Cst { get; set; }

        [Column("id_nfe_emitente")]
        [Required]
        public short Id_Nfe_Emitente { get; set; }

        [Column("NFe_serie_NF")]
        [Required]
        public int NFe_Serie_NF { get; set; }

        [Column("NFe_numero_NF")]
        [Required]
        public int NFe_Numero_NF { get; set; }

        [Column("dt_hr_anotacao_numero_NF")]
        public DateTime? Dt_Hr_Anotacao_Numero_NF { get; set; }

        [Column("usuario_anotacao_numero_NF")]
        [MaxLength(10)]
        public string Usuario_Anotacao_Numero_NF { get; set; }

        [Column("separacao_rel_nsu")]
        [Required]
        public DateTime Separacao_Rel_Nsu { get; set; }

        [Column("separacao_data")]
        public DateTime? Separacao_Data { get; set; }

        [Column("separacao_data_hora")]
        public DateTime? Separacao_Data_Hora { get; set; }

        [Column("separacao_deposito_zona_id")]
        [Required]
        public int Separacao_Deposito_Zona_Id { get; set; }

        [Column("descontinuado")]
        [MaxLength(1)]
        public string Descontinuado { get; set; }
    }
}