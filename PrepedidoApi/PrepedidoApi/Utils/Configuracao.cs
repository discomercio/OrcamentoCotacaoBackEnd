using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoApi.Utils
{
    public class Configuracao
    {
        public string SegredoToken { get; set; }
        public int ValidadeTokenMinutos { get; set; }
        public int LimiteItens { get; set; } = 12;
        public bool TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO { get; set; } = true;
    }
}
