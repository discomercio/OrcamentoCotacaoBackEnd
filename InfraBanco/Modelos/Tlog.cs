using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_LOG")]
    public class Tlog
    {
        [Column("data")]
        [Required]
        [Key]//Não é chave no banco, mas precisando declarar um chave para o EF
        public DateTime Date { get; set; }

        [Column("usuario")]
        [MaxLength(20)]
        public string Usuario { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("pedido")]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Column("id_cliente")]
        [MaxLength(12)]
        public string Id_Cliente { get; set; }

        [Column("operacao")]
        [MaxLength(20)]
        public string Operacao { get; set; }

        [Column("complemento")]
        [MaxLength]
        public string Complemento { get; set; }

    }
}
