using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
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
        public string PrevisaoEntrega { get; set; }
        //Campos para incluir
        //NumeroMagento = nº do pedido Magento
        //NumeroPedidoMarketplace
        //OrigemPedido = de qual loja esta vindo

        public static DetalhesNFPedidoDtoPedido DetalhesNFPedidoDtoPedido_De_DetalhesNFPedidoPedidoDados(DetalhesNFPedidoPedidoDados origem)
        {
            if (origem == null) return null;
            return new DetalhesNFPedidoDtoPedido()
            {
                Observacoes = origem.Observacoes,
                ConstaNaNF = origem.ConstaNaNF,
                XPed = origem.XPed,
                NumeroNF = origem.NumeroNF,
                NFSimples = origem.NFSimples,
                EntregaImediata = origem.EntregaImediata,
                StBemUsoConsumo = origem.StBemUsoConsumo,
                InstaladorInstala = origem.InstaladorInstala,
                GarantiaIndicadorStatus = origem.GarantiaIndicadorStatus,
                PrevisaoEntrega = origem.PrevisaoEntrega
            };
        }
    }
}
