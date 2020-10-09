using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.UtilsMagento
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

        public LimitePedidoMagento LimitePedidos { get; set; } = new LimitePedidoMagento();
        
        public class OrcamentistaMagento
        {
            public string Orcamentista { get; set; }
            public string Loja { get; set; }
        }

        public OrcamentistaMagento DadosOrcamentista { get; set; } = new OrcamentistaMagento();

        public class Magento
        {
            public string Op_av_forma_pagto { get; set; }
        }

        public class Markeplace
        {
            public string Op_pu_forma_pagto { get; set; }
            public int C_pu_vencto_apos { get; set; }
        }

        public class PagtoMagento
        {
            public Magento Magento { get; set; }
            public Markeplace Markeplace { get; set; }
            
        }

        public PagtoMagento FormaPagto { get; set; }
    }
}
