using Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_EMAILSNDSVC_REMETENTE")]
    public sealed class TemailLsndsvcRemetente : IModel
    {
        public int id { get; set; }
        public string email_remetente { get; set; }
        public string display_name_remetente { get; set; }
        public string servidor_smtp { get; set; }
        public string servidor_smtp_porta { get; set; }
        public string usuario_smtp { get; set; }
        public string senha_smtp { get; set; }
        public string replyTo { get; set; }
        public short st_habilita_ssl { get; set; }
        public string resultado_ult_tentativa_envio { get; set; }
        public DateTime? dt_hr_ult_tentativa_envio { get; set; }
        public int ult_id_mensagem { get; set; }
        public short st_envio_mensagem_habilitado { get; set; }
        public short st_envia_falha { get; set; }
    }
}