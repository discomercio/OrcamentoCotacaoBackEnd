using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dados.DetalhesPrepedido
{
    public class DetalhesPrepedidoDados
    {
        public string Observacoes { get; set; }     //campo obs_1
        public string Obter_obs_1() => Observacoes;

        public string NumeroNF { get; set; }        //campo obs_2
        public string Obter_obs_2() => NumeroNF;

        public string EntregaImediata { get; set; }
        public DateTime? EntregaImediataData { get; set; }
        
        public short BemDeUso_Consumo { get; set; }
        public short InstaladorInstala { get; set; }
        public short? InstaladorInstalaIdTipoUsuarioContexto { get; set; }
        public int? InstaladorInstalaIdUsuarioUltAtualiz { get; set; }
        public string InstaladorInstalaUsuarioUltAtualiz { get; set; }
        public DateTime? InstaladorInstalaDtHrUltAtualiz { get; set; }
        public string GarantiaIndicadorTexto { get; set; }
        public byte GarantiaIndicador { get; set; }
        public short? GarantiaIndicadorIdTipoUsuarioContexto { get; set; }
        public int? GarantiaIndicadorIdUsuarioUltAtualiz { get; set; }
        public string GarantiaIndicadorUsuarioUltAtualiz { get; set; }
        public DateTime? GarantiaIndicadorDtHrUltAtualiz { get; set; }

        public string FormaDePagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public string PrevisaoEntregaTexto { get; set; }//para mostrar finalizado na tela
        public DateTime? PrevisaoEntrega { get; set; }
        public string PrevisaoEntregaUsuarioUltAtualiz { get; set; }
        public DateTime? PrevisaoEntregaDtHrUltAtualiz { get; set; }
        public short? PrevisaoEntregaIdTipoUsuarioContexto { get; set; }
        public int? PrevisaoEntregaIdUsuarioUltAtualiz { get; set; }
    }
}
