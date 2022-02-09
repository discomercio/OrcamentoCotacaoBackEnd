using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CFG_PAGTO_FORMA_STATUS")]
    public class TcfgPagtoFormaStatus
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdCfgPagtoForma")]
        public short IdCfgPagtoForma { get; set; }

        [Column("IdCfgModulo")]
        public short IdCfgModulo { get; set; }

        [Column("IdCfgTipoUsuario")]
        public short IdCfgTipoUsuario { get; set; }

        [Column("IdCfgTipoPessoaCliente")]
        public short IdCfgTipoPessoaCliente { get; set; }

        [Column("PedidoComIndicador")]
        public byte? PedidoComIndicador { get; set; }

        [Column("Habilitado")]
        public short Habilitado { get; set; }

        [Column("DataHoraCadastro")]
        public DateTime DataHoraCadastro { get; set; }

        [Column("UsuarioCadastro")]
        [MaxLength(10)]
        [Required]
        public string UsuarioCadastro { get; set; }

        [Column("DataHoraUltAtualizacao")]
        public DateTime DataHoraUltAtualizacao { get; set; }

        [Column("UsuarioUltAtualizacao")]
        [MaxLength(10)]
        [Required]
        public string UsuarioUltAtualizacao { get; set; }

        public TcfgModulo TcfgModulo { get; set; }
        public TcfgPagtoForma TcfgPagtoForma { get; set; }
        public TcfgTipoPessoa TcfgTipoPessoa { get; set; }
        public TcfgTipoUsuario TcfgTipoUsuarios { get; set; }
    }
}
