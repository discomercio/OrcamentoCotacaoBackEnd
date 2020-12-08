using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto
{
    public class FiltroInfosStatusPrepedidosUnisDto
    {
        [Required]
        public string TokenAcesso { get; set; }

        /// <summary>
        /// FiltrarPrepedidos = Lista de Pré-pedidos a serem buscados<br/>
        /// Caso não seja informado nenhum Pré-pedido nessa lista, retornará todos os Pré-pedidos.<br/>
        /// Os critérios serão todos processados através de AND.
        /// </summary>
        public List<string> FiltrarPrepedidos { get; set; }


        /// <summary>
        /// VirouPedido = Informar true para retornar pré-pedidos que viraram pedidos
        /// </summary>
        [Required]
        public bool VirouPedido { get; set; }

        /// <summary>
        /// Pendentes = Informar true para retornar pré-pedidos que estão pendentes
        /// </summary>
        [Required]
        public bool Pendentes { get; set; }

        /// <summary>
        /// Cancelados = Informar true para retornar pré-pedidos cancelados
        /// </summary>
        [Required]
        public bool Cancelados { get; set; }
    }
}
