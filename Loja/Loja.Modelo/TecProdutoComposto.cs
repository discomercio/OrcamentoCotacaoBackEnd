using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("t_EC_PRODUTO_COMPOSTO")]
    public class TecProdutoComposto
    {
        [Key]
        [Column("fabricante_composto")]
        [MaxLength(4)]
        [Required]
        //[ForeignKey("TecProdutoCompostoItem")]
        public string Fabricante_Composto { get; set; }

        [Key]
        [Column("produto_composto")]
        [MaxLength(8)]
        [Required]
        //[ForeignKey("Tproduto")]
        public string Produto_Composto { get; set; }

        [Column("descricao")]
        [MaxLength(80)]
        public string Descricao { get; set; }

        [Column("dt_cadastro")]
        public DateTime Dt_Cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        [Required]
        public string Usuario_Cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        public DateTime Dt_Ult_Atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [MaxLength(10)]
        [Required]
        public string Usuario_Ult_Atualizacao { get; set; }

        public Tproduto Tproduto { get; set; }
        public TecProdutoCompostoItem TecProdutoCompostoItem { get; set; }
    }
}
