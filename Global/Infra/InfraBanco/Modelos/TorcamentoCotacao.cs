using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacao : IModel
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Loja")]
        public string Loja { get; set; }

       [Column("NomeCliente")]
        public string NomeCliente { get; set; }

        [Column("NomeObra")]
        public string NomeObra { get; set; }

        [Column("IdVendedor")]
        public int IdVendedor { get; set; }

        [Column("IdIndicador")]
        public int? IdIndicador { get; set; }

        [Column("IdIndicadorVendedor")]
        public int? IdIndicadorVendedor { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Telefone")]
        public string Telefone { get; set; }

        [Column("AceiteWhatsApp")]
        public bool AceiteWhatsApp { get; set; }

        [Column("UF")]
        public string UF { get; set; }

        [Column("TipoCliente")]
        public string TipoCliente { get; set; }

        [Column("Validade")]
        public DateTime Validade { get; set; }

        [Column("ValidadeAnterior")]
        public DateTime? ValidadeAnterior { get; set; }

        [Column("QtdeRenovacao")]
        public byte QtdeRenovacao { get; set; }

        [Column("IdUsuarioUltRenovacao")]
        public int IdUsuarioUltRenovacao { get; set; }

        [Column("DataHoraUltRenovacao")]
        public DateTime? DataHoraUltRenovacao { get; set; }

        [Column("Observacao")]
        public string Observacao { get; set; }

        [Column("InstaladorInstalaStatus")]
        public int InstaladorInstalaStatus { get; set; }

        [Column("GarantiaIndicadorStatus")]
        public int GarantiaIndicadorStatus { get; set; }

        [Column("StEtgImediata")]
        public int StEtgImediata { get; set; }

        [Column("PrevisaoEntregaData")]
        public DateTime? PrevisaoEntregaData { get; set; }

        [Column("Status")]
        public Int16 Status { get; set; }

        [NotMapped]
        public string StatusNome { get; set; }

        [Column("IdTipoUsuarioContextoUltStatus")]
        public int IdTipoUsuarioContextoUltStatus { get; set; }

        [Column("IdUsuarioUltStatus")]
        public int? IdUsuarioUltStatus { get; set; }

        [Column("DataUltStatus")]
        public DateTime DataUltStatus { get; set; }

        [Column("DataHoraUltStatus")]
        public DateTime DataHoraUltStatus { get; set; }

        [Column("IdOrcamento")]
        public string IdOrcamento { get; set; }

        [Column("IdPedido")]
        public string IdPedido { get; set; }

        [Column("IdTipoUsuarioContextoCadastro")]
        public int IdTipoUsuarioContextoCadastro { get; set; }

        [Column("IdUsuarioCadastro")]
        public int IdUsuarioCadastro { get; set; }

        [Column("DataCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("DataHoraCadastro")]
        public DateTime DataHoraCadastro { get; set; }

        [Column("IdTipoUsuarioContextoUltAtualizacao")]
        public int? IdTipoUsuarioContextoUltAtualizacao { get; set; }

        [Column("IdUsuarioUltAtualizacao")]
        public int? IdTipoUsuarioContextoUltRenovacao { get; set; }

        [Column("DataHoraUltAtualizacao")]
        public int? IdUsuarioUltAtualizacao { get; set; }

        [Column("perc_max_comissao_padrao")]
        public DateTime? DataHoraUltAtualizacao { get; set; }

        [Column("perc_max_comissao_e_desconto_padrao")]
        public float? Perc_max_comissao_padrao { get; set; }

        [Column("perc_max_comissao_e_desconto_padrao")]
        public float? Perc_max_comissao_e_desconto_padrao { get; set; }

        [Column("ContribuinteIcms")]
        public byte ContribuinteIcms { get; set; }

        [Column("VersaoPoliticaCredito")]
        public string VersaoPoliticaCredito { get; set; }

        [Column("VersaoPoliticaPrivacidade")]
        public string VersaoPoliticaPrivacidade { get; set; }

        [Column("InstaladorInstalaIdTipoUsuarioContexto")]
        public int? InstaladorInstalaIdTipoUsuarioContexto { get; set; }

        [Column("InstaladorInstalaIdUsuarioUltAtualiz")]
        public int? InstaladorInstalaIdUsuarioUltAtualiz { get; set; }

        [Column("InstaladorInstalaDtHrUltAtualiz")]
        public DateTime? InstaladorInstalaDtHrUltAtualiz { get; set; }

        [Column("GarantiaIndicadorIdTipoUsuarioContexto")]
        public int? GarantiaIndicadorIdTipoUsuarioContexto { get; set; }

        [Column("GarantiaIndicadorIdUsuarioUltAtualiz")]
        public int? GarantiaIndicadorIdUsuarioUltAtualiz { get; set; }

        [Column("GarantiaIndicadorDtHrUltAtualiz")]
        public DateTime? GarantiaIndicadorDtHrUltAtualiz { get; set; }

        [Column("EtgImediataIdTipoUsuarioContexto")]
        public short? EtgImediataIdTipoUsuarioContexto { get; set; }

        [Column("EtgImediataIdUsuarioUltAtualiz")]
        public int? EtgImediataIdUsuarioUltAtualiz { get; set; }

        [Column("EtgImediataDtHrUltAtualiz")]
        public DateTime? EtgImediataDtHrUltAtualiz { get; set; }

        [Column("PrevisaoEntregaIdTipoUsuarioContexto")]
        public int? PrevisaoEntregaIdTipoUsuarioContexto { get; set; }

        [Column("PrevisaoEntregaIdUsuarioUltAtualiz")]
        public int? PrevisaoEntregaIdUsuarioUltAtualiz { get; set; }

        [Column("PrevisaoEntregaDtHrUltAtualiz")]
        public DateTime? PrevisaoEntregaDtHrUltAtualiz { get; set; }

        public virtual List<TorcamentoCotacaoMensagem> TorcamentoCotacaoMensagems { get; set; }

        public virtual List<TorcamentoCotacaoOpcao> TorcamentoCotacaoOpcaos { get; set; }

        public virtual List<TorcamentoCotacaoLink> TorcamentoCotacaoLinks { get; set; }

        public virtual TcfgOrcamentoCotacaoStatus TcfgOrcamentoCotacaoStatus { get; set; }

        public virtual Tusuario Tusuarios { get; set; }
    }
}
