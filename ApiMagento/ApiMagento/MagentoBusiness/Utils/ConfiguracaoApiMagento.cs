using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.Utils
{
    public class ConfiguracaoApiMagento
    {
        public string SegredoToken { get; set; }
        public int ValidadeTokenMinutos { get; set; }

        public string ApelidoPerfilLiberaAcessoApiMagento { get; set; }

        public class VersaoApiMagento
        {
            public string VersaoApi { get; set; }
            public string Ambiente { get; set; }
            public string Mensagem { get; set; }
        }

        public VersaoApiMagento VersaoApi { get; set; }

        public double LimiteArredondamentoPrecoVendaOrcamentoItem { get; set; } = 0.1;

        public class LimitePedidoMagento
        {
            public int LimitePrepedidosExatamenteIguais_Numero { get; set; } = 1;
            public int LimitePrepedidosExatamenteIguais_TempoSegundos { get; set; } = 10;
            public int LimitePrepedidosMesmoCpfCnpj_Numero { get; set; } = 10;
            public int LimitePrepedidosMesmoCpfCnpj_TempoSegundos { get; set; } = 3600;
        }

        public LimitePedidoMagento LimitePrepedidos { get; set; } = new LimitePedidoMagento();
    }
}
