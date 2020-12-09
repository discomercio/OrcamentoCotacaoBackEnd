using System;
using System.Collections.Generic;
using System.Text;

namespace Especificacao.Testes.Utils
{
    public class ConfiguracaoTestes
    {
        public string DiretorioLogs { get; set; } = "c:\\temp\\arclube_testes_log";
        public string DiretorioLogs_comentario { get; set; } = "deixar sem uma barra no final!";
        public string ArquivoLog { get; set; } = "log.txt";
        public bool SubstituirArquivos { get; set; } = true;
    }
}
