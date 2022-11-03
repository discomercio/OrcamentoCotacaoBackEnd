using InfraBanco.Constantes;
using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prepedido.Dto
{
    public class DetalhesDtoPrepedido
    {
        public string Observacoes { get; set; }
        public string NumeroNF { get; set; }
        public string EntregaImediata { get; set; }
        public DateTime? EntregaImediataData { get; set; }
        public string BemDeUso_Consumo { get; set; }
        public string InstaladorInstala { get; set; }
        public string GarantiaIndicador { get; set; }
        public short? GarantiaIndicadorIdTipoUsuarioContexto { get; set; }
        public string FormaDePagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public string PrevisaoEntrega { get; set; }//para mostrar finalizado na tela
        public DateTime? PrevisaoEntregaData { get; set; }
        public string PrevisaoEntregaUsuarioUltAtualiz { get; set; }
        public DateTime? PrevisaoEntregaDtHrUltAtualiz { get; set; }
        public short? PrevisaoEntregaIdTipoUsuarioContexto { get; set; }
        public int? PrevisaoEntregaIdUsuarioUltAtualiz { get; set; }
        public short? InstaladorInstalaIdTipoUsuarioContexto { get; set; }
        public int? InstaladorInstalaIdUsuarioUltAtualiz { get; set; }
        public string InstaladorInstalaUsuarioUltAtualiz { get; set; }
        public DateTime? InstaladorInstalaDtHrUltAtualiz { get; set; }
        public int? GarantiaIndicadorIdUsuarioUltAtualiz { get; set; }
        public string GarantiaIndicadorUsuarioUltAtualiz { get; set; }
        public DateTime? GarantiaIndicadorDtHrUltAtualiz { get; set; }


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
                GarantiaIndicador = origem.GarantiaIndicador.ToString(),
                FormaDePagamento = origem.FormaDePagamento,
                DescricaoFormaPagamento = origem.DescricaoFormaPagamento,
                PrevisaoEntrega = origem.PrevisaoEntregaTexto
            };
        }
        public static DetalhesPrepedidoDados DetalhesPrepedidoDados_De_DetalhesDtoPrepedido(DetalhesDtoPrepedido origem)
        {
            if (origem == null) return null;

            DetalhesPrepedidoDados ret = new DetalhesPrepedidoDados();

            ret.Observacoes = origem.Observacoes;
            ret.NumeroNF = origem.NumeroNF;
            ret.EntregaImediata = origem.EntregaImediata;
            ret.EntregaImediataData = origem.EntregaImediataData;
            ret.BemDeUso_Consumo = origem.BemDeUso_Consumo == Convert.ToString((short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM) ?
                (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM :
                (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO;
            ret.InstaladorInstala = origem.InstaladorInstala == Convert.ToString((short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM) ?
                (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM :
                (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO;
            ret.InstaladorInstalaIdTipoUsuarioContexto = origem.InstaladorInstalaIdTipoUsuarioContexto;//mesmo campo da T_ORCAMENTO_COTACAO
            ret.InstaladorInstalaIdUsuarioUltAtualiz = origem.InstaladorInstalaIdUsuarioUltAtualiz;//mesmo campo da T_ORCAMENTO_COTACAO
            ret.InstaladorInstalaUsuarioUltAtualiz = origem.InstaladorInstalaUsuarioUltAtualiz;//preencher com '[N] 999999' onde  N = InstaladorInstalaIdTipoUsuarioContexto  999999 = InstaladorInstalaIdUsuarioUltAtualiz Se tipo usuário = 4 preencher com 'Cliente'
            ret.InstaladorInstalaDtHrUltAtualiz = origem.InstaladorInstalaDtHrUltAtualiz;//mesmo campo da T_ORCAMENTO_COTACAO
            ret.GarantiaIndicadorTexto = origem.GarantiaIndicador;
            ret.GarantiaIndicadorIdTipoUsuarioContexto = origem.GarantiaIndicadorIdTipoUsuarioContexto;//mesmo campo da T_ORCAMENTO_COTACAO
            ret.GarantiaIndicadorIdUsuarioUltAtualiz = origem.GarantiaIndicadorIdUsuarioUltAtualiz;//mesmo campo da T_ORCAMENTO_COTACAO
            ret.GarantiaIndicadorUsuarioUltAtualiz = origem.GarantiaIndicadorUsuarioUltAtualiz;// preencher com '[N] 999999' onde  N = GarantiaIndicadorIdTipoUsuarioContexto  999999 = GarantiaIndicadorIdUsuarioUltAtualiz Se tipo usuário = 4 preencher com 'Cliente'
            ret.GarantiaIndicadorDtHrUltAtualiz = origem.GarantiaIndicadorDtHrUltAtualiz;//mesmo campo da T_ORCAMENTO_COTACAO
            ret.FormaDePagamento = origem.FormaDePagamento;
            ret.DescricaoFormaPagamento = origem.DescricaoFormaPagamento;
            ret.PrevisaoEntrega = origem.PrevisaoEntregaData;
            ret.PrevisaoEntregaIdTipoUsuarioContexto = origem.PrevisaoEntregaIdTipoUsuarioContexto;
            ret.PrevisaoEntregaIdUsuarioUltAtualiz = origem.PrevisaoEntregaIdUsuarioUltAtualiz;
            ret.PrevisaoEntregaDtHrUltAtualiz = origem.PrevisaoEntregaDtHrUltAtualiz;
            ret.PrevisaoEntregaUsuarioUltAtualiz = origem.PrevisaoEntregaUsuarioUltAtualiz;

            return ret;
        }
    }
}
