﻿using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido
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
        public decimal? Preco_Lista { get; set; }
        public decimal VlLista { get; set; }
        public float? Desconto { get; set; }
        public decimal VlUnitario { get; set; }
        public decimal? VlTotalItem { get; set; }
        public decimal VlTotalRA { get; set; }
        public float? Comissao { get; set; }
        public decimal? TotalItemRA { get; set; }
        public decimal? TotalItem { get; set; }
        public short? Qtde_estoque_total_disponivel { get; set; }
        public float CustoFinancFornecCoeficiente { get; set; }//coeficiente do fabricante
        //incluimos esse campos apenas para validar o que esta sendo enviado pela API da Unis
        public decimal? Preco_NF { get; set; }

        private static PrepedidoProdutoDtoPrepedido PrepedidoProdutoDtoPrepedido_De_PrepedidoProdutoPrepedidoDados(PrepedidoProdutoPrepedidoDados origem)
        {
            if (origem == null) return null;
            return new PrepedidoProdutoDtoPrepedido()
            {
                Fabricante = origem.Fabricante,
                NumProduto = origem.NumProduto,
                Descricao = origem.Descricao,
                Obs = origem.Obs,
                Qtde = origem.Qtde,
                Permite_Ra_Status = origem.Permite_Ra_Status,
                BlnTemRa = origem.BlnTemRa,
                Preco = origem.CustoFinancFornecPrecoListaBase,
                Preco_Lista = origem.Preco_NF,
                VlLista = origem.Preco_Lista,
                Desconto = origem.Desc_Dado,
                VlUnitario = origem.Preco_Venda,
                VlTotalItem = origem.VlTotalItem,
                VlTotalRA = origem.VlTotalRA,
                Comissao = origem.Comissao,
                TotalItemRA = origem.TotalItemRA,
                TotalItem = origem.TotalItem,
                Qtde_estoque_total_disponivel = origem.Qtde_estoque_total_disponivel,
                CustoFinancFornecCoeficiente = origem.CustoFinancFornecCoeficiente,
                Preco_NF = origem.Preco_NF
            };
        }
        private static PrepedidoProdutoPrepedidoDados PrepedidoProdutoPrepedidoDados_De_PrepedidoProdutoDtoPrepedido(PrepedidoProdutoDtoPrepedido origem, short permiteRaStatus)
        {
            if (origem == null) return null;

            PrepedidoProdutoPrepedidoDados ret = new PrepedidoProdutoPrepedidoDados()
            {
                Fabricante = origem.Fabricante,
                NumProduto = origem.NumProduto,
                Descricao = origem.Descricao,
                Obs = origem.Obs,
                Qtde = origem.Qtde,
                Permite_Ra_Status = origem.Permite_Ra_Status,
                BlnTemRa = origem.BlnTemRa,
                CustoFinancFornecPrecoListaBase = origem.Preco ?? 0,
                //Preco_Lista = origem.Preco_NF,
                Preco_Lista = origem.VlLista,
                Desc_Dado = origem.Desconto ?? 0,
                Preco_Venda = origem.VlUnitario,
                VlTotalItem = origem.VlTotalItem ?? 0,
                VlTotalRA = origem.VlTotalRA,
                Comissao = origem.Comissao,
                TotalItemRA = origem.TotalItemRA ?? 0,
                TotalItem = origem.TotalItem ?? 0,
                Qtde_estoque_total_disponivel = origem.Qtde_estoque_total_disponivel,
                CustoFinancFornecCoeficiente = origem.CustoFinancFornecCoeficiente,
                Preco_NF = (permiteRaStatus == 1 ? origem.Preco_Lista : origem.VlUnitario) ?? 0
            };

            return ret;
        }
        public static List<PrepedidoProdutoDtoPrepedido> ListaPrepedidoProdutoDtoPrepedido_De_PrepedidoProdutoPrepedidoDados(IEnumerable<PrepedidoProdutoPrepedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<PrepedidoProdutoDtoPrepedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(PrepedidoProdutoDtoPrepedido_De_PrepedidoProdutoPrepedidoDados(p));
            return ret;
        }
        public static List<PrepedidoProdutoPrepedidoDados> ListaPrepedidoProdutoPrepedidoDados_De_PrepedidoProdutoDtoPrepedido(IEnumerable<PrepedidoProdutoDtoPrepedido> listaBancoDados, short permiteRaStatus)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<PrepedidoProdutoPrepedidoDados>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(PrepedidoProdutoPrepedidoDados_De_PrepedidoProdutoDtoPrepedido(p, permiteRaStatus));
            return ret;
        }
    }
}
