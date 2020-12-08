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
        /// Obs: Caso não seja informado nenhum Pré-pedido nessa lista, retornará todos os Pré-pedidos
        /// </summary>
        public List<string> FiltrarPrepedidos { get; set; }


        /// <summary>
        /// VirouPedido = Filtro para buscar pré-pedidos que viraram pedidos
        /// </summary>
        [Required]
        public bool VirouPedido { get; set; }

        /// <summary>
        /// Pendentes = Filtro para buscar pré-pedidos que estão pendentes
        /// </summary>
        [Required]
        public bool Pendentes { get; set; }

        /// <summary>
        /// Cancelados = Filtro para buscar os pedidos cancelados
        /// </summary>
        [Required]
        public bool Cancelados { get; set; }
    }
}
