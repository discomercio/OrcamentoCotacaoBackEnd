using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PERFIL")]
    public class Tperfil
    {
        [Required]
        [MaxLength(12)]
        [Column("id")]
        //[Key]
        public string Id { get; set; }

        public TperfilUsuario TperfilUsuario { get; set; }
        public TperfilItem TperfilItem { get; set; }

        [Required]
        [MaxLength(12)]
        [Column("apelido")]
        public string Apelido { get; set; }

        [Required]
        [MaxLength(40)]
        [Column("descricao")]
        public string Descricao { get; set; }

        [Required]
        [Column("dt_cadastro")]
        public DateTime Dt_cadastro { get; set; }

        [MaxLength(10)]
        [Column("usuario_cadastro")]
        public string Usuario_cadastro { get; set; }

        [Required]
        [Column("dt_ult_atualizacao")]
        public DateTime Dt_ult_atualizacao { get; set; }

        [MaxLength(10)]
        [Column("usuario_ult_atualizacao")]
        public string Usuario_ult_atualizacao { get; set; }

        [Column("timestamp")]
        public byte Timestamp { get; }

        [Required]
        [Column("nivel_acesso_bloco_notas_pedido")]
        public short Nivel_acesso_bloco_notas_pedido { get; set; }

        [Required]
        [Column("nivel_acesso_chamado")]
        public short Nivel_acesso_chamado { get; set; }

        [Required]
        [Column("st_inativo")]
        public byte St_inativo { get; set; }
    }
}
