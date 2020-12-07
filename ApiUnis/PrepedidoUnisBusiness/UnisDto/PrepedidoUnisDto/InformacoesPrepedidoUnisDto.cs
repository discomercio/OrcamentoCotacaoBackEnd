using Prepedido.Dados.DetalhesPrepedido;
using PrepedidoUnisBusiness.UnisDto.PedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto
{
    public class InformacoesPrepedidoUnisDto
    {
        /// <summary>
        /// Número do orçamento
        /// </summary>
        public string Orcamento { get; set; }
        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime Data { get; set; }
        /// <summary>
        /// Status do orçamento = "CAN", vazio ou nulo      
        /// </summary>
        public string St_orcamento { get; set; }
        /// <summary>
        /// Flag se virou pedido
        /// </summary>
        public bool St_virou_pedido { get; set; }
        public List<InformacoesPedidoUnisDto> LstInformacoesPedidoUnisDto { get; set; }

        public static List<InformacoesPrepedidoUnisDto> ListaInformacoesPrepedidoUnisDto_De_ListaInformacoesStatusPrepedidoRetornoDados(List<InformacoesStatusPrepedidoRetornoDados> lstDados)
        {
            List<InformacoesPrepedidoUnisDto> lstDto = new List<InformacoesPrepedidoUnisDto>();

            if (lstDados != null)
            {
                if (lstDados.Count > 0)
                {
                    foreach (var i in lstDados)
                    {
                        InformacoesPrepedidoUnisDto dto = new InformacoesPrepedidoUnisDto
                        {
                            Orcamento = i.Orcamento,
                            Data = i.Data,
                            St_orcamento = i.St_orcamento,
                            St_virou_pedido = i.St_virou_pedido,
                            LstInformacoesPedidoUnisDto = InformacoesPedidoUnisDto.ListaInformacoesPedidoUnisDto_De_InformacoesPedidoDados(i.LstInformacoesPedido)
                        };

                        lstDto.Add(dto);
                    }
                }
            }


            return lstDto;
        }
    }

    public class ListaInformacoesPrepedidoRetornoUnisDto
    {
        /// <summary>
        /// Armazena a lista
        /// </summary>
        public List<InformacoesPrepedidoUnisDto> ListaInformacoesPrepedidoRetorno { get; set; }
    }
}
