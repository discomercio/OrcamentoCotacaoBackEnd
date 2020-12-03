using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class ProdutoPedidoDados : Produto.Dados.ProdutoDados
    {
        public short? QtdeSolicitada { get; set; }
        public List<int> Lst_empresa_selecionada { get; set; } = new List<int>();
    }
}
