using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PedidoUnisDto
{
    public class InformacoesPedidoUnisDto
    {
        /// <summary>
        /// Número do pedido
        /// </summary>
        public string Pedido { get; set; }
        /// <summary>
        /// ESP = Esperar Mercadoria, 
        /// SPL = Split Possível, 
        /// SEP = Separar Mercadoria, 
        /// AET = A Entregar, 
        /// ETG = Entregue, 
        /// CAN = Cancelado
        /// </summary>
        public string St_entrega { get; set; }
        /// <summary>
        /// Esperar Mercadoria, 
        /// Split Possível, 
        /// Separar Mercadoria, 
        /// A Entregar, 
        /// Entregue, 
        /// Cancelado
        /// </summary>
        public string DescricaoStatusEntrega { get; set; }
        /// <summary>
        /// Data em que o pedido foi coletado pela transportadora
        /// </summary>
        public DateTime? Entregue_data { get; set; }
        /// <summary>
        /// Data de cancelamento do pedido
        /// </summary>
        public DateTime? Cancelado_data { get; set; }
        /// <summary>
        /// Flag para informar se o pedido foi recebido pelo cliente
        /// </summary>
        public bool PedidoRecebidoStatus { get; set; }
        /// <summary>
        /// Data de recebimento do pedido pelo cliente
        /// </summary>
        public DateTime? PedidoRecebidoData { get; set; }

        /// <summary>
        /// Aguardando Análise Inicial = 0, 
        /// Pendente = 1, 
        /// Crédito OK = 2, 
        /// Pendente Endereço = 6, 
        /// Crédito OK (depósito aguardando desbloqueio) = 7, 
        /// Pendente Vendas = 8, 
        /// Crédito OK (aguardando depósito) = 9, 
        /// Pedido Sem Análise de Crédito = 10
        /// </summary>
        //nota: o texto PEND_CARTAO nunca exite no banco de dados
        public short Analise_credito { get; set; }
        /// <summary>
        /// Aguardando Análise Inicial, 
        /// Pendente, 
        /// Crédito OK, 
        /// Pendente Endereço, 
        /// Crédito OK (depósito aguardando desbloqueio), 
        /// Pendente Vendas, 
        /// Crédito OK (aguardando depósito), 
        /// Pedido Sem Análise de Crédito
        /// </summary>
        public string DescricaoAnaliseCredito { get; set; }
        /// <summary>
        /// Data da análise de crédito
        /// </summary>
        public DateTime? Analise_credito_data { get; set; }
        /// <summary>
        /// Pago = S, 
        /// Não-Pago = N, 
        /// Pago Parcial = P
        /// </summary>
        public string St_pagto { get; set; }
        /// <summary>
        /// Pago, 
        /// Não-Pago, 
        /// Pago Parcial
        /// </summary>
        public string DescricaoStatusPagto { get; set; }


        public static List<InformacoesPedidoUnisDto> ListaInformacoesPedidoUnisDto_De_InformacoesPedidoDados(List<InformacoesPedidoRetornoDados> lstDados)
        {
            List<InformacoesPedidoUnisDto> lstDto = new List<InformacoesPedidoUnisDto>();

            if (lstDados != null)
            {
                if (lstDados.Count > 0)
                {
                    foreach (var i in lstDados)
                    {
                        InformacoesPedidoUnisDto info = new InformacoesPedidoUnisDto
                        {
                            Pedido = i.Pedido,
                            St_entrega = i.St_entrega,
                            DescricaoStatusEntrega = i.DescricaoStatusEntrega,
                            Entregue_data = i.Entregue_data,
                            Cancelado_data = i.Cancelado_data,
                            PedidoRecebidoStatus = Convert.ToBoolean(i.PedidoRecebidoStatus),
                            PedidoRecebidoData = i.PedidoRecebidoData,
                            Analise_credito = i.Analise_credito,
                            DescricaoAnaliseCredito = i.DescricaoAnaliseCredito,
                            Analise_credito_data = i.Analise_credito_data,
                            St_pagto = i.St_pagto,
                            DescricaoStatusPagto = i.DescricaoStatusPagto
                        };

                        lstDto.Add(info);
                    }

                }
            }

            return lstDto;
        }
    }

    
}
