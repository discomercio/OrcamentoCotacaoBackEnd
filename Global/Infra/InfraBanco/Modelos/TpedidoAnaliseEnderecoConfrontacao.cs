using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG

namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO")]
    public class TpedidoAnaliseEnderecoConfrontacao
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("TpedidoAnaliseEndereco")]
        [Required]
        [Column("id_pedido_analise_endereco")]
        public int Id_pedido_analise_endereco { get; set; }

        public TpedidoAnaliseEndereco TpedidoAnaliseEndereco { get; set; }

        [Required]
        [Column("pedido")]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Required]
        [Column("id_cliente")]
        [MaxLength(12)]
        public string Id_cliente { get; set; }

        [Required]
        [Column("tipo_endereco")]
        [MaxLength(1)]
        public string Tipo_endereco { get; set; }

        [Column("endereco_logradouro")]
        [MaxLength(80)]
        public string Endereco_logradouro { get; set; }

        [Column("endereco_bairro")]
        [MaxLength(72)]
        public string Endereco_bairro { get; set; }

        [Column("endereco_cidade")]
        [MaxLength(60)]
        public string Endereco_cidade { get; set; }

        [Column("endereco_uf")]
        [MaxLength(2)]
        public string Endereco_uf { get; set; }

        [Column("endereco_cep")]
        [MaxLength(8)]
        public string Endereco_cep { get; set; }

        [Column("endereco_numero")]
        [MaxLength(20)]
        public string Endereco_numero { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_complemento { get; set; }

    }
}
#endif