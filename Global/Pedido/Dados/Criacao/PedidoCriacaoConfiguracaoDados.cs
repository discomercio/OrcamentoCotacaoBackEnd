using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoConfiguracaoDados
    {
        public PedidoCriacaoConfiguracaoDados(decimal limiteArredondamento, decimal maxErroArredondamento)
        {
            LimiteArredondamento = limiteArredondamento;
            MaxErroArredondamento = maxErroArredondamento;
        }

        public decimal LimiteArredondamento { get; }
        public decimal MaxErroArredondamento { get; }
    }
}
