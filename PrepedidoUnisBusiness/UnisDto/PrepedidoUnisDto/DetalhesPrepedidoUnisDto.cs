﻿using PrepedidoBusiness.Dtos.Prepedido.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class DetalhesPrePedidoUnisDto
    {
        /// <summary>
        /// EntregaImediata: 
        ///     COD_ETG_IMEDIATA_ST_INICIAL = 0,
        ///     COD_ETG_IMEDIATA_NAO = 1,
        ///     COD_ETG_IMEDIATA_SIM = 2
        /// </summary>
        [Required]
        [MaxLength(1)]
        public string EntregaImediata { get; set; }
        
        public DateTime? EntregaImediataData { get; set; }

        /// <summary>
        /// BemDeUso_Consumo : COD_ST_BEM_USO_CONSUMO_NAO = 0, COD_ST_BEM_USO_CONSUMO_SIM = 1
        /// </summary>
        [Required]
        [MaxLength(1)]
        public string BemDeUso_Consumo { get; set; }

        /// <summary>
        /// InstaladorInstala: COD_INSTALADOR_INSTALA_NAO_DEFINIDO = 0, COD_INSTALADOR_INSTALA_NAO = 1, COD_INSTALADOR_INSTALA_SIM = 2
        /// </summary>
        [Required]
        [MaxLength(1)]
        public string InstaladorInstala { get; set; }

        public static DetalhesPrePedidoUnisDto DetalhesPrePedidoUnisDtoDeDetalhesPrePedidoDto(DetalhesDtoPrepedido detalhesPrePedidoDto)
        {
            var ret = new DetalhesPrePedidoUnisDto()
            {
                EntregaImediata = detalhesPrePedidoDto.EntregaImediata,
                EntregaImediataData = detalhesPrePedidoDto.EntregaImediataData,
                BemDeUso_Consumo = detalhesPrePedidoDto.BemDeUso_Consumo,
                InstaladorInstala = detalhesPrePedidoDto.InstaladorInstala
            };
            return ret;
        }

        public static DetalhesDtoPrepedido DetalhesPrePedidoDtoDeDetalhesPrePedidoUnisDto(DetalhesPrePedidoUnisDto  detalhesPrePedidoUnisDto)
        {
            var ret = new DetalhesDtoPrepedido()
            {
                EntregaImediata = detalhesPrePedidoUnisDto.EntregaImediata,
                EntregaImediataData = detalhesPrePedidoUnisDto.EntregaImediataData,
                BemDeUso_Consumo = detalhesPrePedidoUnisDto.BemDeUso_Consumo,
                InstaladorInstala = detalhesPrePedidoUnisDto.InstaladorInstala
            };
            return ret;
        }
    }
}
