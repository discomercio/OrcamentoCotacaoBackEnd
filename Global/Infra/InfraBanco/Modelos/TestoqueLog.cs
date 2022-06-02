using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_ESTOQUE_LOG")]
    public class TestoqueLog
    {
        [Required]
        [Column("data")]
        public DateTime data { get; set; }

        [Required]
        [Column("data_hora")]
        public DateTime Data_hora { get; set; }

        [Required]
        [Column("usuario")]
        [MaxLength(10)]
        public string Usuario { get; set; }

        [Required]
        [Column("fabricante")]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Required]
        [Column("produto")]
        [MaxLength(8)]
        public string Produto { get; set; }

        [Required]
        [Column("qtde_solicitada")]
        public short Qtde_solicitada { get; set; }

        [Required]
        [Column("qtde_atendida")]
        public short Qtde_atendida { get; set; }

        [Required]
        [Column("operacao")]
        [MaxLength(3)]
        public string Operacao { get; set; }

        [Column("cod_estoque_origem")]
        [MaxLength(3)]
        public string Cod_estoque_origem { get; set; }

        [Column("cod_estoque_destino")]
        [MaxLength(3)]
        public string Cod_estoque_destino { get; set; }

        [Column("loja_estoque_origem")]
        [MaxLength(3)]
        public string Loja_estoque_origem { get; set; }

        [Column("loja_estoque_destino")]
        [MaxLength(3)]
        public string Loja_estoque_destino { get; set; }

        [Column("pedido_estoque_origem")]
        [MaxLength(9)]
        public string Pedido_estoque_origem { get; set; }

        [Column("pedido_estoque_destino")]
        [MaxLength(9)]
        public string Pedido_estoque_destino { get; set; }

        [Column("documento")]
        [MaxLength(30)]
        public string Documento { get; set; }

        [Column("complemento")]
        [MaxLength(80)]
        public string Complemento { get; set; }

        [Column("id_ordem_servico")]
        [MaxLength(12)]
        public string Id_ordem_servico { get; set; }

        [Required]
        [Column("id_nfe_emitente")]
        public short Id_nfe_emitente { get; set; }

    }
}
