﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PrepedidoDto.DetalhesPrepedido
{
    public class PrepedidoProdutoDtoPrepedido
    {
        public string Fabricante { get; set; }
        public string NumProduto { get; set; }
        public string Descricao { get; set; }
        public string Obs { get; set; }
        public short? Qtde { get; set; }
        public short Permite_Ra_Status { get; set; }
        public bool BlnTemRa { get; set; }
        public decimal? Preco { get; set; }
        public decimal VlLista { get; set; }
        public float? Desconto { get; set; }
        public decimal VlUnitario { get; set; }
        public decimal? VlTotalItem { get; set; }
        public decimal VlTotalRA { get; set; }
        public float? Comissao { get; set; }
        public decimal? TotalItemRA { get; set; }
        public decimal? TotalItem { get; set; }

        //verificar a necessidade dessa variavel
        public short? Qtde_estoque_total_disponivel { get; set; }
        //public short? Qtde_Max_Venda { get; set; }
    }
}
