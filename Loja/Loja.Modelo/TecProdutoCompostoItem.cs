﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("t_EC_PRODUTO_COMPOSTO_ITEM")]
    public class TecProdutoCompostoItem
    {
        [Key]
        [Column("fabricante_composto")]
        [Required]
        [MaxLength(4)]
        public string Fabricante_composto { get; set; }

        [Column("produto_composto")]
        [Required]
        [MaxLength(8)]
        public string Produto_composto { get; set; }

        [Column("fabricante_item")]
        [Required]
        [MaxLength(4)]
        public string Fabricante_item { get; set; }

        [Column("produto_item")]
        [Required]
        [MaxLength(8)]
        public string Produto_item { get; set; }

        [Column("qtde")]
        [Required]
        public short Qtde { get; set; }

        [Column("sequencia")]
        public short? Sequencia { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_cadastro { get; set; }

        [Column("usuario_cadastro")]
        [Required]
        [MaxLength(10)]
        public string Usuario_cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_ult_atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [Required]
        [MaxLength(10)]
        public string Usuario_ult_atualizacao { get; set; }

        [Column("excluido_status")]
        [Required]
        public short Excluido_status { get; set; }
    }
}
