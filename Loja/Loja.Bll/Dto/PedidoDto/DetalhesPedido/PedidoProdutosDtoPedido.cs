﻿using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class PedidoProdutosDtoPedido
    {
        //todo: afazer: precisamos remover boa parte destas variáveis e usar as do bloco de baixo. Só vamos fazer isso quando normalizamormos os nomes dos campos no javascript/html
        public string Fabricante { get; set; }
        public string NumProduto { get; set; }
        public string Descricao { get; set; }
        public short? Qtde { get; set; }
        public short? Faltando { get; set; }
        public string CorFaltante { get; set; }
        public decimal? Preco { get; set; }
        public decimal? Preco_Lista { get; set; }
        public decimal VlLista { get; set; }
        public float? Desconto { get; set; }
        public decimal VlUnitario { get; set; }
        public decimal? VlTotalItem { get; set; }
        public decimal? VlTotalItemComRA { get; set; }
        public decimal? VlVenda { get; set; }
        public decimal? VlTotal { get; set; }
        public short? Qtde_estoque_total_disponivel { get; set; }
        public string Alertas { get; set; }


        //todo: afazer: acertar os nomes de variáveis para usar estes
        public string Produto { get; set; }
        public decimal CustoFinancFornecPrecoListaBase { get; set; }
        public decimal Preco_NF { get; set; }
        public float? Desc_Dado { get; set; }
        public decimal Preco_Venda { get; set; }
        public decimal TotalItem { get; set; }
        public decimal? TotalItemRA { get; set; }
        public float CustoFinancFornecCoeficiente { get; set; }


        public static List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> List_PedidoProdutoPedidoDados_De_PedidoProdutosDtoPedido(List<PedidoProdutosDtoPedido> origemLista)
        {
            if (origemLista == null) return null;
            var ret = new List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados>(0);
            foreach (var origem in origemLista)
            {
                ret.Add(new Pedido.Dados.Criacao.PedidoCriacaoProdutoDados(
                    fabricante: origem.Fabricante,
                    produto: origem.Produto,
                    qtde: origem.Qtde ?? 0,
                    qtde_spe_usuario_aceitou: origem.Qtde_estoque_total_disponivel,
                    preco_Lista: origem.Preco_Lista ?? 0,
                    custoFinancFornecPrecoListaBase_Conferencia: origem.CustoFinancFornecPrecoListaBase,
                    preco_NF: origem.Preco_NF,
                    desc_Dado: origem.Desc_Dado,
                    preco_Venda: origem.Preco_Venda,
                    custoFinancFornecCoeficiente_Conferencia: origem.CustoFinancFornecCoeficiente
                ));
            }
            return ret;
        }

        public static List<PedidoProdutosDtoPedido> ListaPedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(IEnumerable<PedidoProdutosPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<PedidoProdutosDtoPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(PedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(p));
            return ret;
        }
        public static PedidoProdutosDtoPedido PedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(PedidoProdutosPedidoDados origem)
        {
            if (origem == null) return null;
            return new PedidoProdutosDtoPedido()
            {
                Fabricante = origem.Fabricante,
                NumProduto = origem.Produto,
                Descricao = origem.Descricao,
                Qtde = origem.Qtde,
                Faltando = origem.Faltando,
                CorFaltante = origem.CorFaltante,
                Preco_Lista = origem.Preco_NF,
                VlLista = origem.Preco_Lista,
                Desconto = origem.Desc_Dado,
                VlVenda = origem.Preco_Venda ?? 0m,
                VlTotalItem = origem.VlTotalItem,
                VlTotalItemComRA = origem.VlTotalItemComRA,

                ////todo: verificar se isto fica...
                ////ests são os campos definitivos
                //Produto = origem.Produto,
                ////CustoFinancFornecPrecoListaBase = origem.CustoFinancFornecPrecoListaBase,
                //Preco_NF = origem.Preco_NF ?? 0,
                //Desc_Dado = origem.Desc_Dado,
                //Preco_Venda = origem.Preco_Venda ?? 0
                ////TotalItem = origem.TotalItem,
                ////TotalItemRA = origem.TotalItemRA,
                ////CustoFinancFornecCoeficiente = origem.CustoFinancFornecCoeficiente
            };
        }
    }
}
