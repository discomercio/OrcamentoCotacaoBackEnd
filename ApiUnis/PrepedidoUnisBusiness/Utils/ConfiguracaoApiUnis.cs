using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.Utils
{
    public class ConfiguracaoApiUnis
    {
        public string SegredoToken { get; set; }
        public int ValidadeTokenMinutos { get; set; }

        public string ApelidoPerfilLiberaAcessoApiUnis { get; set; }

        public class VersaoApiUnis
        {
            public string VersaoApi { get; set; }
            public string Ambiente { get; set; }
            public string Mensagem { get; set; }
        }

        public VersaoApiUnis VersaoApi { get; set; }

        public double LimiteArredondamentoPrecoVendaOrcamentoItem { get; set; } = 0.1;

        public class LimitePrepedidosUnis
        {
            public int LimitePrepedidosExatamenteIguais_Numero { get; set; } = 1;
            public int LimitePrepedidosExatamenteIguais_TempoSegundos { get; set; } = 10;
            public int LimitePrepedidosMesmoCpfCnpj_Numero { get; set; } = 10;
            public int LimitePrepedidosMesmoCpfCnpj_TempoSegundos { get; set; } = 3600;
        }

        public LimitePrepedidosUnis LimitePrepedidos { get; set; } = new LimitePrepedidosUnis();

        public class PrepedidoListarStatusPrepedidoParametrizacao
        {
            public int CanceladoDias { get; set; } = 60;
            public int PendentesDias { get; set; } = 60;
            public int VirouPedidoDias { get; set; } = 60;
        }

        public PrepedidoListarStatusPrepedidoParametrizacao ParamBuscaListagemStatusPrepedido { get; set; } = new PrepedidoListarStatusPrepedidoParametrizacao();
    }
}
