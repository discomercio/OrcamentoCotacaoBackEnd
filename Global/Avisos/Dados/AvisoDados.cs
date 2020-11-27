using System;
using System.Collections.Generic;
using System.Text;

namespace Avisos.Dados
{
    public class AvisoDados
    {
        public string Id { get; set; }
        public string Mensagem { get; set; }
        public string Usuario { get; set; }
        public string Destinatario { get; set; }
        public DateTime Dt_ult_atualizacao { get; set; }
    }
}
