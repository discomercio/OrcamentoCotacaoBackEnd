using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class TpedidoEnderecoConfrontacaoDados
    {
        public TpedidoEnderecoConfrontacaoDados(Tpedido pedido, string tipoEndreco)
        {
            Pedido = pedido;
            TipoEndreco = tipoEndreco;
        }

        public Tpedido Pedido { get; set; }
        public string TipoEndreco { get; set; }
    }
}
