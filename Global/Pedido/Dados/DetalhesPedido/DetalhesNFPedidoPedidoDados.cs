using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.DetalhesPedido
{
    public class DetalhesNFPedidoPedidoDados
    {
        public string Observacoes { get; set; }
        public string ConstaNaNF { get; set; }
        public string XPed { get; set; }
        public string NumeroNF { get; set; }
        public string NFSimples { get; set; }
        public string EntregaImediata { get; set; }
        public short StBemUsoConsumo { get; set; }
        public short InstaladorInstala { get; set; }
        public string GarantiaIndicadorStatus { get; set; }
        public string PrevisaoEntrega { get; set; }
    }
}
