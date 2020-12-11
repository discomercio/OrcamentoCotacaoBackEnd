using System;
using System.Collections.Generic;
using System.Text;


namespace Prepedido.Dados.DetalhesPrepedido
{
    public class InformacoesStatusPrepedidoRetornoDados
    {
        public string Orcamento { get; set; }
        public DateTime? Data { get; set; }
        public string St_orcamento { get; set; }
        public bool St_virou_pedido { get; set; }
        public List<InformacoesPedidoRetornoDados> LstInformacoesPedido { get; set; }
    }
}
