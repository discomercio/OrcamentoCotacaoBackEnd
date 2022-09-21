using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Dto.Pedido.DetalhesPedido
{
    public class DetalhesFormaPagamentos
    {
        public List<string> FormaPagto { get; set; }
        public string InfosAnaliseCredito { get; set; }
        public string StatusPagto { get; set; }
        public string CorStatusPagto { get; set; }
        public decimal VlTotalFamilia { get; set; }
        public decimal VlPago { get; set; }
        public decimal VlDevolucao { get; set; }
        public decimal? VlPerdas { get; set; }
        public decimal? SaldoAPagar { get; set; }
        public string AnaliseCredito { get; set; }
        public string CorAnalise { get; set; }
        public DateTime? DataColeta { get; set; }
        public string Transportadora { get; set; }
        public decimal VlFrete { get; set; }

        public static DetalhesFormaPagamentos DetalhesFormaPagamentos_De_DetalhesFormaPagamentosDados(DetalhesFormaPagamentosDados origem)
        {
            if (origem == null) return null;
            return new DetalhesFormaPagamentos()
            {
                FormaPagto = origem.FormaPagto,
                InfosAnaliseCredito = origem.InfosAnaliseCredito,
                StatusPagto = origem.StatusPagto,
                CorStatusPagto = origem.CorStatusPagto,
                VlTotalFamilia = origem.VlTotalFamilia,
                VlPago = origem.VlPago,
                VlDevolucao = origem.VlDevolucao,
                VlPerdas = origem.VlPerdas,
                SaldoAPagar = origem.SaldoAPagar,
                AnaliseCredito = origem.AnaliseCredito,
                CorAnalise = origem.CorAnalise,
                DataColeta = origem.DataColeta,
                Transportadora = origem.Transportadora,
                VlFrete = origem.VlFrete
            };
        }
    }
}
