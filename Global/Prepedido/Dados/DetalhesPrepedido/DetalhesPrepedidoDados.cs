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
        public string GarantiaIndicador { get; set; }
        public string FormaDePagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public string PrevisaoEntrega { get; set; }//para mostrar finalizado na tela
    }
}
