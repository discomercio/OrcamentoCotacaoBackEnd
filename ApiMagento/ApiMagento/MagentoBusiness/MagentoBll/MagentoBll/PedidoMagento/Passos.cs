﻿using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static InfraBanco.Constantes.Constantes;

namespace MagentoBusiness.MagentoBll.MagentoBll.PedidoMagento
{
    static class Passos
    {
        public static void P30_InfPedido_MagentoPedidoStatus(PedidoMagentoDto pedidoMagento, List<string> listaErros)
        {
            //MagentoPedidoStatus deve ser aprovado ou aprovação pendente
            if (pedidoMagento.MagentoPedidoStatus != null)
            {
                if (pedidoMagento.MagentoPedidoStatus.Status == (int)MagentoPedidoStatusEnum.MAGENTO_PEDIDO_STATUS_APROVADO)
                    return;
                if (pedidoMagento.MagentoPedidoStatus.Status == (int)MagentoPedidoStatusEnum.MAGENTO_PEDIDO_STATUS_APROVACAO_PENDENTE)
                    return;
            }

            listaErros.Add($"O campo MagentoPedidoStatus somente pode ter os valores {(int)MagentoPedidoStatusEnum.MAGENTO_PEDIDO_STATUS_APROVADO} " +
                $"e {(int)MagentoPedidoStatusEnum.MAGENTO_PEDIDO_STATUS_APROVACAO_PENDENTE}, mas está com o valor {pedidoMagento?.MagentoPedidoStatus?.Status}");
        }

        public static void P35_Totais(PedidoMagentoDto pedidoMagento, List<string> listaErros, decimal limiteArredondamentoTotais)
        {
            /*
            P35_Totais: validações de PedidoTotaisMagentoDto
                Campos não validados: FreteBruto e DescontoFrete
                Campos com sua feature: Subtotal, DiscountAmount, BSellerInterest, GrandTotal
            */
            if (pedidoMagento.TotaisPedido == null)
            {
                listaErros.Add("A estrutura pedidoMagento.TotaisPedido é obrigatória.");
                return;
            }

            //BSellerInterest: nos casos em que seja diferente de zero, o pedido deve ser processado manualmente.
            if (pedidoMagento.TotaisPedido.BSellerInterest != 0)
            {
                listaErros.Add("TotaisPedido.BSellerInterest é diferente de zero. Nos casos em que seja diferente de zero, o pedido deve ser processado manualmente.");
            }

            /*
	        PedidoTotaisMagentoDto.DiscountAmount = 
		        soma de PedidoProdutoMagentoDto.DiscountAmount 
		        + soma de PedidoServicoMagentoDto.DiscountAmount 
		        + PedidoTotaisMagentoDto.DescontoFrete
		        dentro do arredondamento
            */
            {
                decimal discountAmountCalculado = (from l in pedidoMagento.ListaProdutos select l.DiscountAmount).Sum();
                discountAmountCalculado += (from l in pedidoMagento.ListaServicos select l.DiscountAmount).Sum();
                discountAmountCalculado += pedidoMagento.TotaisPedido.DescontoFrete;
                if (!IgualComArredondamento(pedidoMagento.TotaisPedido.DiscountAmount, discountAmountCalculado, limiteArredondamentoTotais))
                {
                    listaErros.Add($"TotaisPedido.DiscountAmount inconsistente. Recebido {pedidoMagento.TotaisPedido.DiscountAmount} mas deveria ser {discountAmountCalculado}.");
                }
            }

            /*
	            PedidoTotaisMagentoDto.Subtotal = 
		            soma de PedidoProdutoMagentoDto.Subtotal
		            + soma de PedidoServicoMagentoDto.Subtotal
            */
            {
                decimal subtotalCalculado = (from l in pedidoMagento.ListaProdutos select l.Subtotal).Sum();
                subtotalCalculado += (from l in pedidoMagento.ListaServicos select l.Subtotal).Sum();
                if (!IgualComArredondamento(pedidoMagento.TotaisPedido.Subtotal, subtotalCalculado, limiteArredondamentoTotais))
                {
                    listaErros.Add($"TotaisPedido.Subtotal inconsistente. Recebido {pedidoMagento.TotaisPedido.Subtotal} mas deveria ser {subtotalCalculado}.");
                }
            }

            /*
            PedidoTotaisMagentoDto.GrandTotal = 
                PedidoTotaisMagentoDto.Subtotal
                + PedidoTotaisMagentoDto.FreteBruto
                - PedidoTotaisMagentoDto.DiscountAmount
                dentro do arredondamento
            */
            {
                decimal grandTotalCalculado = pedidoMagento.TotaisPedido.Subtotal
                    + pedidoMagento.TotaisPedido.FreteBruto
                    - pedidoMagento.TotaisPedido.DiscountAmount;
                if (!IgualComArredondamento(pedidoMagento.TotaisPedido.GrandTotal, grandTotalCalculado, limiteArredondamentoTotais))
                {
                    listaErros.Add($"TotaisPedido.GrandTotal inconsistente. Recebido {pedidoMagento.TotaisPedido.GrandTotal} mas deveria ser {grandTotalCalculado}.");
                }
            }
        }

        internal static void P39_Servicos(List<PedidoServicoMagentoDto> listaServicos, List<string> listaErros, decimal limiteArredondamentoPorItem)
        {
            foreach (var linha in listaServicos)
            {
                //P39_Servicos: para cada linha, consistir Quantidade > 0, RowTotal = Subtotal - DiscountAmount dentro do arredondamento
                if (linha.Quantidade <= 0)
                    listaErros.Add($"O serviço {linha.Sku} está com Qtde menor ou igual a zero: {linha.Quantidade}");
                if (!IgualComArredondamento(linha.RowTotal, linha.Subtotal - linha.DiscountAmount, limiteArredondamentoPorItem))
                    listaErros.Add($"O serviço {linha.Sku} está com valor inválido, RowTotal != Subtotal - DiscountAmount ({linha.RowTotal} != {linha.Subtotal} - {linha.DiscountAmount})");
            }
        }

        //rotina muito parecida com a P39_Servicos
        internal static void P40_P05_LinhasProdutos(List<PedidoProdutoMagentoDto> listaProdutos, List<PedidoServicoMagentoDto> listaServicos,
            List<string> listaErros, decimal limiteArredondamentoPorItem)
        {
            foreach (var linha in listaProdutos)
            {
                //P39_Servicos: para cada linha, consistir Quantidade > 0, RowTotal = Subtotal - DiscountAmount dentro do arredondamento
                if (linha.Quantidade <= 0)
                    listaErros.Add($"O produto {linha.Sku} está com Qtde menor ou igual a zero: {linha.Quantidade}");
                if (!IgualComArredondamento(linha.RowTotal, linha.Subtotal - linha.DiscountAmount, limiteArredondamentoPorItem))
                    listaErros.Add($"O produto {linha.Sku} está com valor inválido, RowTotal != Subtotal - DiscountAmount ({linha.RowTotal} != {linha.Subtotal} - {linha.DiscountAmount})");
            }

            //verificar se existem produtos ou serviços repetidos
            VerificarProdutosRepetidos((from p in listaProdutos select p.Sku).ToList(), listaErros, "Produto", "produto");
            VerificarProdutosRepetidos((from p in listaServicos select p.Sku).ToList(), listaErros, "Serviço", "serviço");
        }

        internal static void VerificarProdutosRepetidos(List<string> lstProdutos, List<string> lstErros, string mensagem1, string mensagem2)
        {
            //# loja/PedidoNovoConsiste.asp
            //# alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": linha " & renumera_com_base1(Lbound(v_item),i) & " repete o mesmo produto da linha " & renumera_com_base1(Lbound(v_item),j) & "."
            var agrupados = (from p in lstProdutos group p by p into g select g).ToList();
            var repetidos = (from p in agrupados where p.Count() > 1 select p.Key).ToList();
            //para saber a linha precisamos de um loop
            for (int ilinha = 0; ilinha < lstProdutos.Count(); ilinha++)
            {
                var esteproduto = lstProdutos[ilinha];
                if (!(from repetido in repetidos where repetido == esteproduto select repetido).Any())
                    continue;

                //está repetido sim
                for (int ilinha2 = ilinha + 1; ilinha2 < lstProdutos.Count(); ilinha2++)
                {
                    var esterepetido = lstProdutos[ilinha2];
                    if (esteproduto == esterepetido)
                    {
                        var msg = mensagem1 + " com SKU " + esteproduto
                            + ": linha " + (ilinha + 1).ToString() + " repete o mesmo " + mensagem2 + " da linha " + (ilinha2 + 1).ToString() + ".";
                        lstErros.Add(msg);
                    }
                }
            }
        }



        public static bool IgualComArredondamento(decimal valor1, decimal valor2, decimal limiteArredondamento)
        {
            if (Math.Abs(valor1 - valor2) > limiteArredondamento)
                return false;
            return true;
        }


    }
}
