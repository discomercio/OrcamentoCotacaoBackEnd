﻿using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;

namespace OrcamentoCotacaoBusiness.Dto.Pedido.DetalhesPedido
{
    public class DetalhesNFPedidoDtoPedido
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