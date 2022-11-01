using InfraBanco.Constantes;
using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrcamentoCotacaoBusiness.Dto.Prepedido.DetalhesPrepedido
{
    public class DetalhesDtoPrepedido
    {
        public string Observacoes { get; set; }
        public string NumeroNF { get; set; }
        public string EntregaImediata { get; set; }
        public DateTime? EntregaImediataData { get; set; }
        public string BemDeUso_Consumo { get; set; }
        public string InstaladorInstala { get; set; }
        public byte GarantiaIndicador { get; set; }
        public string FormaDePagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public DateTime? PrevisaoEntrega { get; set; }
        public string PrevisaoEntregaTexto { get; set; }//para mostrar finalizado na tela

        public static DetalhesDtoPrepedido DetalhesDtoPrepedido_De_DetalhesPrepedidoDados(DetalhesPrepedidoDados origem)
        {
            if (origem == null) return null;
            return new DetalhesDtoPrepedido()
            {
                Observacoes = origem.Observacoes,
                NumeroNF = origem.NumeroNF,
                EntregaImediata = origem.EntregaImediata,
                EntregaImediataData = origem.EntregaImediataData,
                BemDeUso_Consumo = origem.BemDeUso_Consumo == (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO ? "NÃO" : "SIM",
                InstaladorInstala = origem.InstaladorInstala == (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO ? "NÃO" : "SIM",
                GarantiaIndicador = origem.GarantiaIndicador,
                FormaDePagamento = origem.FormaDePagamento,
                DescricaoFormaPagamento = origem.DescricaoFormaPagamento,
                PrevisaoEntregaTexto = origem.PrevisaoEntregaTexto
            };
        }
        public static DetalhesPrepedidoDados DetalhesPrepedidoDados_De_DetalhesDtoPrepedido(DetalhesDtoPrepedido origem)
        {
            if (origem == null) return null;

            DetalhesPrepedidoDados ret = new DetalhesPrepedidoDados()
            {
                Observacoes = origem.Observacoes,
                NumeroNF = origem.NumeroNF,
                EntregaImediata = origem.EntregaImediata,
                EntregaImediataData = origem.EntregaImediataData,
                BemDeUso_Consumo = origem.BemDeUso_Consumo == Convert.ToString((short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM) ?
                    (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM :
                    (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO,
                InstaladorInstala = origem.InstaladorInstala == Convert.ToString((short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM) ?
                (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM :
                (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO,
                GarantiaIndicador = origem.GarantiaIndicador,
                FormaDePagamento = origem.FormaDePagamento,
                DescricaoFormaPagamento = origem.DescricaoFormaPagamento,
                PrevisaoEntrega = origem.PrevisaoEntrega
            };

            return ret;
        }
    }
}
