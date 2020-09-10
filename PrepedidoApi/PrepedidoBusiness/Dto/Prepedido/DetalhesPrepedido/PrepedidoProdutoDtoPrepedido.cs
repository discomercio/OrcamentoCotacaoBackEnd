using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido
{
    public class PrepedidoProdutoDtoPrepedido
    {
        public string Fabricante { get; set; }
        public string NormalizacaoCampos_Produto { get; set; }
        public string Descricao { get; set; }
        public string Obs { get; set; }
        public short? Qtde { get; set; }
        public short Permite_Ra_Status { get; set; }
        public bool BlnTemRa { get; set; }
        public decimal? NormalizacaoCampos_CustoFinancFornecPrecoListaBase { get; set; }
        //public decimal? Preco_Lista { get; set; }
        public decimal NormalizacaoCampos_Preco_Lista { get; set; }
        public float? NormalizacaoCampos_Desc_Dado { get; set; }
        public decimal NormalizacaoCampos_Preco_Venda { get; set; }
        public decimal? VlTotalItem { get; set; }
        public decimal VlTotalRA { get; set; }
        public float? Comissao { get; set; }
        public decimal? TotalItemRA { get; set; }
        public decimal? TotalItem { get; set; }
        public short? Qtde_estoque_total_disponivel { get; set; }
        public float CustoFinancFornecCoeficiente { get; set; }//coeficiente do fabricante
        //incluimos esse campos apenas para validar o que esta sendo enviado pela API da Unis
        public decimal? NormalizacaoCampos_Preco_NF { get; set; }

        public static PrepedidoProdutoDtoPrepedido PrepedidoProdutoDtoPrepedido_De_PrepedidoProdutoPrepedidoDados(PrepedidoProdutoPrepedidoDados origem)
        {
            if (origem == null) return null;
            return new PrepedidoProdutoDtoPrepedido()
            {
                Fabricante = origem.Fabricante,
                NormalizacaoCampos_Produto = origem.NormalizacaoCampos_Produto,
                Descricao = origem.Descricao,
                Obs = origem.Obs,
                Qtde = origem.Qtde,
                Permite_Ra_Status = origem.Permite_Ra_Status,
                BlnTemRa = origem.BlnTemRa,
                NormalizacaoCampos_CustoFinancFornecPrecoListaBase = origem.NormalizacaoCampos_CustoFinancFornecPrecoListaBase,
                NormalizacaoCampos_Preco_Lista = origem.NormalizacaoCampos_Preco_Lista,
                NormalizacaoCampos_Desc_Dado = origem.NormalizacaoCampos_Desc_Dado,
                NormalizacaoCampos_Preco_Venda = origem.NormalizacaoCampos_Preco_Venda,
                VlTotalItem = origem.VlTotalItem,
                VlTotalRA = origem.VlTotalRA,
                Comissao = origem.Comissao,
                TotalItemRA = origem.TotalItemRA,
                TotalItem = origem.TotalItem,
                Qtde_estoque_total_disponivel = origem.Qtde_estoque_total_disponivel,
                CustoFinancFornecCoeficiente = origem.CustoFinancFornecCoeficiente,
                NormalizacaoCampos_Preco_NF = origem.Preco_NF
            };
        }
        public static PrepedidoProdutoPrepedidoDados PrepedidoProdutoPrepedidoDados_De_PrepedidoProdutoDtoPrepedido(PrepedidoProdutoDtoPrepedido origem, short permiteRaStatus)
        {
            if (origem == null) return null;

            PrepedidoProdutoPrepedidoDados ret = new PrepedidoProdutoPrepedidoDados()
            {
                Fabricante = origem.Fabricante,
                NormalizacaoCampos_Produto = origem.NormalizacaoCampos_Produto,
                Descricao = origem.Descricao,
                Obs = origem.Obs,
                Qtde = origem.Qtde,
                Permite_Ra_Status = origem.Permite_Ra_Status,
                BlnTemRa = origem.BlnTemRa,
                NormalizacaoCampos_CustoFinancFornecPrecoListaBase = origem.NormalizacaoCampos_CustoFinancFornecPrecoListaBase,
                //Preco_Lista = origem.Preco_NF,
                NormalizacaoCampos_Preco_Lista = origem.NormalizacaoCampos_Preco_Lista,
                NormalizacaoCampos_Desc_Dado = origem.NormalizacaoCampos_Desc_Dado,
                NormalizacaoCampos_Preco_Venda = origem.NormalizacaoCampos_Preco_Venda,
                VlTotalItem = origem.VlTotalItem,
                VlTotalRA = origem.VlTotalRA,
                Comissao = origem.Comissao,
                TotalItemRA = origem.TotalItemRA,
                TotalItem = origem.TotalItem,
                Qtde_estoque_total_disponivel = origem.Qtde_estoque_total_disponivel,
                CustoFinancFornecCoeficiente = origem.CustoFinancFornecCoeficiente,
                Preco_NF = origem.NormalizacaoCampos_Preco_NF
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
