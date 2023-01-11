using Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_EMAILSNDSVC_MENSAGEM")]
    public sealed class TemailSndsvcMensagem : IModel
    {
        public int id { get; set; }
        public int id_remetente { get; set; }
        public DateTime dt_cadastro { get; set; }
        public DateTime dt_hr_cadastro { get; set; }
        public string assunto { get; set; }
        public string corpo_mensagem { get; set; }
        public string destinatario_To { get; set; }
        public string destinatario_Cc { get; set; }
        public string destinatario_CCo { get; set; }
        public DateTime? dt_hr_agendamento_envio { get; set; }
        public int qtde_tentativas_realizadas { get; set; }
        public int st_enviado_sucesso { get; set; }
        public DateTime? dt_hr_enviado_sucesso { get; set; }
        public int st_falhou_em_definitivo { get; set; }
        public DateTime? dt_hr_falhou_em_definitivo { get; set; }
        public string resultado_ult_tentativa_envio { get; set; }
        public DateTime? dt_hr_ult_tentativa_envio { get; set; }
        public string msg_erro_ult_tentativa_envio { get; set; }
        public int st_processamento_mensagem { get; set; }
        public int st_envio_cancelado { get; set; }
        public DateTime? dt_hr_envio_cancelado { get; set; }
        public string usuario_envio_cancelado { get; set; }
        public string replyToMsg { get; set; }
        public int st_replyToMsg { get; set; }
    }
}