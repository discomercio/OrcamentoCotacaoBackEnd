using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoMarketplaceDados
    {
        public PedidoCriacaoMarketplaceDados(string? pedido_bs_x_ac, string? marketplace_codigo_origem, string? pedido_bs_x_marketplace)
        {
            Pedido_bs_x_ac = pedido_bs_x_ac;
            Marketplace_codigo_origem = marketplace_codigo_origem;
            Pedido_bs_x_marketplace = pedido_bs_x_marketplace;
        }

        public string? Pedido_bs_x_ac { get; }
        public string? Marketplace_codigo_origem { get; }
        public string? Pedido_bs_x_marketplace { get; }

    }
}
