using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido
{
    public class DetalhesDtoPrepedido
    {
        public string Observacoes { get; set; }
        public string NumeroNF { get; set; }
        public string EntregaImediata { get; set; }
        public DateTime? EntregaImediataData { get; set; }
        public string BemDeUso_Consumo { get; set; }
        public string InstaladorInstala { get; set; }
        public string GarantiaIndicador { get; set; }
        public string FormaDePagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public string PrevisaoEntrega { get; set; }//para mostrar finalizado na tela

        public static DetalhesDtoPrepedido DetalhesDtoPrepedido_De_DetalhesPrepedidoDados(DetalhesPrepedidoDados origem)
        {
            if (origem == null) return null;
            return new DetalhesDtoPrepedido()
            {
                Observacoes = origem.Observacoes,
                NumeroNF = origem.NumeroNF,
                EntregaImediata = origem.EntregaImediata,
                EntregaImediataData = origem.EntregaImediataData,
                BemDeUso_Consumo = origem.BemDeUso_Consumo,
                InstaladorInstala = origem.InstaladorInstala,
                GarantiaIndicador = origem.GarantiaIndicador,
                FormaDePagamento = origem.FormaDePagamento,
                DescricaoFormaPagamento = origem.DescricaoFormaPagamento,
                PrevisaoEntrega = origem.PrevisaoEntrega
            };
        }
        public static DetalhesPrepedidoDados DetalhesPrepedidoDados_De_DetalhesDtoPrepedido(DetalhesDtoPrepedido origem)
        {
            if (origem == null) return null;
            return new DetalhesPrepedidoDados()
            {
                Observacoes = origem.Observacoes,
                NumeroNF = origem.NumeroNF,
                EntregaImediata = origem.EntregaImediata,
                EntregaImediataData = origem.EntregaImediataData,
                BemDeUso_Consumo = origem.BemDeUso_Consumo,
                InstaladorInstala = origem.InstaladorInstala,
                GarantiaIndicador = origem.GarantiaIndicador,
                FormaDePagamento = origem.FormaDePagamento,
                DescricaoFormaPagamento = origem.DescricaoFormaPagamento,
                PrevisaoEntrega = origem.PrevisaoEntrega
            };
        }
    }
}
