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

        public LimitePedidoMagento LimitePrepedidos { get; set; } = new LimitePedidoMagento();

        /* Afazer: criar a classe com dados 
         * LojaUsuario
         * Usuario
         */

        public class OrcamentistaMagento
        {
            public string Orcamentista { get; set; }
            public string Loja { get; set; }
            public string Vendedor { get; set; }
        }

        public OrcamentistaMagento DadosOrcamentista { get; set; }

        public class PagtoMagento
        {
            public string Op_pu_forma_pagto { get; set; }
            public int C_pu_vencto_apos { get; set; }
            public string Op_av_forma_pagto { get; set; }
        }

        public PagtoMagento FormaPagto { get; set; }
    }
}
