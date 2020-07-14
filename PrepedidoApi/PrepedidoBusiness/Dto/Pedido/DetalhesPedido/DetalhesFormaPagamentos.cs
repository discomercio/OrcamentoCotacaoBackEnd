using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Pedido.DetalhesPedido
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

    }
}
