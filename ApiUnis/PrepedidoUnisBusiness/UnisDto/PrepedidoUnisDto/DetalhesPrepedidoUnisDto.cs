using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.ComponentModel.DataAnnotations;

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
        public short St_Entrega_Imediata { get; set; }

        public DateTime? PrevisaoEntregaData { get; set; }

        /// <summary>
        /// BemDeUso_Consumo : COD_ST_BEM_USO_CONSUMO_NAO = 0, COD_ST_BEM_USO_CONSUMO_SIM = 1
        /// </summary>
        [Required]
        public short BemDeUso_Consumo { get; set; }

        /// <summary>
        /// InstaladorInstala: COD_INSTALADOR_INSTALA_NAO_DEFINIDO = 0, COD_INSTALADOR_INSTALA_NAO = 1, COD_INSTALADOR_INSTALA_SIM = 2
        /// </summary>
        [Required]
        public short InstaladorInstala { get; set; }

        [MaxLength(500)]
        public string Obs_1 { get; set; }

        public static DetalhesPrePedidoUnisDto DetalhesPrePedidoUnisDtoDeDetalhesPrePedidoDto(DetalhesDtoPrepedido detalhesPrePedidoDto)
        {
            var ret = new DetalhesPrePedidoUnisDto()
            {
                St_Entrega_Imediata = short.Parse(detalhesPrePedidoDto.EntregaImediata),
                PrevisaoEntregaData = detalhesPrePedidoDto.EntregaImediataData,
                BemDeUso_Consumo = short.Parse(detalhesPrePedidoDto.BemDeUso_Consumo),
                InstaladorInstala = short.Parse(detalhesPrePedidoDto.InstaladorInstala),
                Obs_1 = detalhesPrePedidoDto.Observacoes
            };
            return ret;
        }

        public static DetalhesDtoPrepedido DetalhesPrePedidoDtoDeDetalhesPrePedidoUnisDto(DetalhesPrePedidoUnisDto detalhesPrePedidoUnisDto)
        {
            var ret = new DetalhesDtoPrepedido()
            {
                EntregaImediata = detalhesPrePedidoUnisDto.St_Entrega_Imediata.ToString(),
                EntregaImediataData = detalhesPrePedidoUnisDto.PrevisaoEntregaData,
                BemDeUso_Consumo = detalhesPrePedidoUnisDto.BemDeUso_Consumo ==
                    (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO ? "NÃO" : "SIM",
                InstaladorInstala = detalhesPrePedidoUnisDto.InstaladorInstala ==
                    (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO ? "NÃO" : "SIM",
                Observacoes = detalhesPrePedidoUnisDto.Obs_1
            };
            return ret;
        }

        public static Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados DetalhesPrepedidoDadosDeDetalhesPrePedidoUnisDto(DetalhesPrePedidoUnisDto detalhesPrePedidoUnisDto)
        {
            var ret = new Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados()
            {
                EntregaImediata = detalhesPrePedidoUnisDto.St_Entrega_Imediata.ToString(),
                EntregaImediataData = detalhesPrePedidoUnisDto.PrevisaoEntregaData,
                BemDeUso_Consumo = detalhesPrePedidoUnisDto.BemDeUso_Consumo,
                InstaladorInstala = detalhesPrePedidoUnisDto.InstaladorInstala,
                Observacoes = detalhesPrePedidoUnisDto.Obs_1
            };
            return ret;
        }
    }
}
