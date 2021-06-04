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

        public decimal LimiteArredondamentoPrecoVendaOrcamentoItem { get; set; } = 0.1M;

        public class LimitePedidoMagento
        {
            public int LimitePedidosExatamenteIguais_Numero { get; set; } = 1;
            public int LimitePedidosExatamenteIguais_TempoSegundos { get; set; } = 10;
            public int LimitePedidosMesmoCpfCnpj_Numero { get; set; } = 10;
            public int LimitePedidosMesmoCpfCnpj_TempoSegundos { get; set; } = 3600;
            public int LimiteItens { get; set; } = 12;
        }

        public LimitePedidoMagento LimitePedidos { get; set; } = new LimitePedidoMagento();

        public string EndEtg_cod_justificativa { get; set; } = "007";   //Pedido Arclube

        public class IndicadorMagento
        {
            public string Indicador { get; set; } = "FRETE";
            public string Loja { get; set; } = "201";
        }

        public IndicadorMagento DadosIndicador { get; set; } = new IndicadorMagento();

        public class Magento
        {
            public string Op_av_forma_pagto { get; set; } = "6";
        }

        public class Markeplace
        {
            public string Op_pu_forma_pagto { get; set; } = "2";
            public int C_pu_vencto_apos { get; set; } = 30;
        }

        public class PagtoMagento
        {
            public Magento Magento { get; set; } = new Magento();
            public Markeplace Markeplace { get; set; } = new Markeplace();

        }

        public PagtoMagento FormaPagto { get; set; } = new PagtoMagento();

        public bool TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO { get; set; } = true;
    }
}
