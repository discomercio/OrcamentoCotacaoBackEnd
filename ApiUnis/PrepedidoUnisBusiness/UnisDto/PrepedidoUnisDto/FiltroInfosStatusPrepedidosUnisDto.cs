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
        /// FiltrarPrepedidos = Lista de Pré-pedidos a serem buscados
        /// </summary>
        [Required]
        public List<string> FiltrarPrepedidos { get; set; }
        [Required]

        /// <summary>
        /// VirouPedido = Filtro para buscar pré-pedidos que viraram pedidos
        /// </summary>
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
