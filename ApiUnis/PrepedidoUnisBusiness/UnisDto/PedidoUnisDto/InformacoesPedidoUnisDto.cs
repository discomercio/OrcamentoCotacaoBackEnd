using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PedidoUnisDto
{
    public class InformacoesPedidoUnisDto
    {
        public string Pedido { get; set; }
        public string St_entrega { get; set; }
        public string DescricaoStatusEntrega { get; set; }
        public DateTime Entregue_data { get; set; }
        public DateTime Cancelado_data { get; set; }
        public short PedidoRecebidoStatus { get; set; }
        public DateTime PedidoRecebidoData { get; set; }
        public short Analise_credito { get; set; }
        public string DescricaoAnaliseCredito { get; set; }
        public DateTime Analise_credito_data { get; set; }
        public string St_pagto { get; set; }
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
                            //infosPedido.DescricaoStatusEntrega- filho.
                            Entregue_data = i.Entregue_data,
                            Cancelado_data = i.Cancelado_data,
                            PedidoRecebidoStatus = i.PedidoRecebidoStatus,
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
