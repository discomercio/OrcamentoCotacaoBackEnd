using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.UtilsMagento
{
    public class ConfiguracaoApiMagento
    {
        public string SegredoToken { get; set; } = "um segredo para o token";
        public int ValidadeTokenMinutos { get; set; }

        public string ApelidoPerfilLiberaAcessoApiMagento { get; set; } = "";

        public class VersaoApiMagento
        {
            public string VersaoApi { get; set; } = "";
            public string Ambiente { get; set; } = "compilação";
            public string Mensagem { get; set; } = "mensagem durante a compilação";
        }

        public VersaoApiMagento VersaoApi { get; set; } = new VersaoApiMagento();

        public double LimiteArredondamentoPrecoVendaOrcamentoItem { get; set; } = 0.1;

        public class LimitePedidoMagento
        {
            public int LimitePrepedidosExatamenteIguais_Numero { get; set; } = 1;
            public int LimitePrepedidosExatamenteIguais_TempoSegundos { get; set; } = 10;
            public int LimitePrepedidosMesmoCpfCnpj_Numero { get; set; } = 10;
            public int LimitePrepedidosMesmoCpfCnpj_TempoSegundos { get; set; } = 3600;
        }

        public LimitePedidoMagento LimitePedidos { get; set; } = new LimitePedidoMagento();

        public string EndEtg_cod_justificativa { get; set; } = "007";   //Pedido Arclube

        public class OrcamentistaMagento
        {
            public string Orcamentista { get; set; } = "FRETE";
            public string Loja { get; set; } = "201";
        }

        public OrcamentistaMagento DadosOrcamentista { get; set; } = new OrcamentistaMagento();

        public class Magento
        {
            public string Op_av_forma_pagto { get; set; } = "6";
        }

        public class Markeplace
        {
            public string Op_pu_forma_pagto { get; set; } = "2";
            public int C_pu_vencto_apos { get; set; }
        }

        public class PagtoMagento
        {
            public Magento Magento { get; set; } = new Magento();
            public Markeplace Markeplace { get; set; } = new Markeplace();

        }

        public PagtoMagento FormaPagto { get; set; } = new PagtoMagento();
    }
}
