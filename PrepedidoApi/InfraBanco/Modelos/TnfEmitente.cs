using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_NFe_EMITENTE")]
    public class TnfEmitente
    {
        [Key]
        [Required]
        [Column("id")]
        public short Id { get; set; }

        [Column("st_ativo")]
        [Required]
        public byte St_Ativo { get; set; }

        [Column("apelido")]
        [MaxLength(20)]
        [Required]
        public string Apelido { get; set; }

        [Column("NFe_st_emitente_padrao")]
        [Required]
        public byte NFe_st_emitente_padrao { get; set; }

        [Column("NFe_T1_servidor_BD")]
        [MaxLength(160)]
        public string NFe_T1_servidor_BD { get; set; }

        [Column("NFe_T1_nome_BD")]
        [MaxLength(40)]
        public string NFe_T1_nome_BD { get; set; }

        [Column("NFe_T1_usuario_BD")]
        [MaxLength(40)]
        public string NFe_T1_usuario_BD { get; set; }

        [Column("NFe_T1_senha_BD")]
        [MaxLength(160)]
        public string NFe_T1_senha_BD { get; set; }

        [Column("st_habilitado_ctrl_estoque")]
        [Required]
        public byte St_Habilitado_Ctrl_Estoque { get; set; }
    }
}
