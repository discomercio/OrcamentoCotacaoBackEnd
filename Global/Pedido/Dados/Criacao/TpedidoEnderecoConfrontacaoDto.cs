using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class TpedidoEnderecoConfrontacaoDto
    {
        public Tpedido Pedido { get; set; }
        public string TipoEndreco { get; set; }
    }
}
