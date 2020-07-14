using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class DetalhesNFPedidoDtoPedido
    {
        public string Observacoes { get; set; }
        public string ConstaNaNF { get; set; }
        public string XPed { get; set; }
        public string NumeroNF { get; set; }
        public string NFSimples { get; set; }//acho que esse campo não é usado
        public string EntregaImediata { get; set; }
        public short StBemUsoConsumo { get; set; }
        public short InstaladorInstala { get; set; }
        public string GarantiaIndicadorStatus { get; set; }
        //Campos para incluir
        //NumeroMagento = nº do pedido Magento
        //NumeroPedidoMarketplace
        //OrigemPedido = de qual loja esta vindo


    }
}
