using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("t_USUARIO")]
    public class Tusuario
    {
        [Key]
        [Column("usuario")]
        [MaxLength(10)]
        [Required]
        public string Usuario { get; set; }

        [Column("nivel")]
        [MaxLength(1)]
        [Required]
        public string Nivel { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        [Required]
        public string Loja { get; set; }

        [Column("senha")]
        [MaxLength(10)]
        public string Senha { get; set; }

        [Column("nome")]
        [MaxLength(40)]
        public string Nome { get; set; }

        [Column("datastamp")]
        [MaxLength(32)]
        public string Datastamp { get; set; }

        [Column("bloqueado")]
        public short? Bloqueado { get; set; }

        [Column("dt_cadastro")]
        public DateTime Dt_Cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        public DateTime Dt_Ult_Atualizacao { get; set; }

        [Column("dt_ult_alteracao_senha")]
        public DateTime? Dt_Ult_Alteracao_Senha { get; set; }

        [Column("dt_ult_acesso")]
        public DateTime? Dt_Ult_Acesso { get; set; }

        [Column("vendedor_externo")]
        public short? Vendedor_Externo { get; set; }

        [Column("vendedor_loja")]
        public short Vendedor_Loja { get; set; }

        [Column("SessionCtrlTicket")]
        [MaxLength(64)]
        public string SessionCtrlTicket { get; set; }

        [Column("SessionCtrlLoja")]
        [MaxLength(3)]
        public string SessionCtrlLoja { get; set; }

        [Column("SessionCtrlModulo")]
        [MaxLength(5)]
        public string SessionCtrlModulo { get; set; }
        
        [Column("SessionCtrlDtHrLogon")]
        public DateTime? SessionCtrlDtHrLogon { get; set; }

        [Column("fin_email_remetente")]
        [MaxLength(80)]
        public string Fin_Email_Remetente { get; set; }

        [Column("fin_servidor_smtp")]
        [MaxLength(80)]
        public string Fin_Servidor_Smtp { get; set; }

        [Column("fin_usuario_smtp")]
        [MaxLength(80)]
        public string Fin_Usuario_Smtp { get; set; }

        [Column("fin_senha_smtp")]
        [MaxLength(80)]
        public string Fin_Senha_Smtp { get; set; }

        [Column("fin_display_name_remetente")]
        [MaxLength(80)]
        public string Fin_Display_Name_Remetente { get; set; }

        [Column("nome_iniciais_em_maiusculas")]
        [MaxLength(40)]
        public string Nome_Iniciais_Em_Maiusculas { get; set; }

        [Column("fin_servidor_smtp_porta")]
        public int? Fin_Servidor_Smtp_Porta { get; set; }

        [Column("email")]
        [MaxLength(60)]
        public string Email { get; set; }

        [Column("SessionTokenModuloCentral")]
        public Guid SessionTokenModuloCentral { get; set; }

        [Column("DtHrSessionTokenModuloCentral")]
        public DateTime DtHrSessionTokenModuloCentral { get; set; }

        [Column("SessionTokenModuloLoja")]
        public Guid SessionTokenModuloLoja { get; set; }

        [Column("DtHrSessionTokenModuloLoja")]
        public DateTime DtHrSessionTokenModuloLoja { get; set; }
    }
}
