using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacao : IModel
    {
        public int Id { get; set; }

        
        public string Loja { get; set; }

       
        public string NomeCliente { get; set; }

        
        public string NomeObra { get; set; }

        
        public int IdVendedor { get; set; }

        public int? IdIndicador { get; set; }

        public int? IdIndicadorVendedor { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public bool AceiteWhatsApp { get; set; }

        public string UF { get; set; }

        public string TipoCliente { get; set; }

        public DateTime Validade { get; set; }

        public DateTime? ValidadeAnterior { get; set; }

        public byte QtdeRenovacao { get; set; }

        public int IdUsuarioUltRenovacao { get; set; }

        public DateTime? DataHoraUltRenovacao { get; set; }

        public string Observacao { get; set; }

        public int InstaladorInstalaStatus { get; set; }

        public int GarantiaIndicadorStatus { get; set; }

        public int StEtgImediata { get; set; }

        public DateTime? PrevisaoEntregaData { get; set; }

        public Int16 Status { get; set; }

        [NotMapped]
        public string StatusNome { get; set; }

        public int IdTipoUsuarioContextoUltStatus { get; set; }

        public int IdUsuarioUltStatus { get; set; }

        public DateTime DataUltStatus { get; set; }

        public DateTime DataHoraUltStatus { get; set; }

        public string IdOrcamento { get; set; }

        public string IdPedido { get; set; }

        public int IdTipoUsuarioContextoCadastro { get; set; }

        public int IdUsuarioCadastro { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataHoraCadastro { get; set; }

        public int IdTipoUsuarioContextoUltAtualizacao { get; set; }

        public int IdUsuarioUltAtualizacao { get; set; }

        public DateTime? DataHoraUltAtualizacao { get; set; }

        public float? Perc_max_comissao_padrao { get; set; }

        public float? Perc_max_comissao_e_desconto_padrao { get; set; }

        public byte ContribuinteIcms { get; set; }

        public string VersaoPoliticaCredito { get; set; }

        public string VersaoPoliticaPrivacidade { get; set; }

        public virtual List<TorcamentoCotacaoMensagem> TorcamentoCotacaoMensagems { get; set; }

        public virtual List<TorcamentoCotacaoOpcao> TorcamentoCotacaoOpcaos { get; set; }

        public virtual List<TorcamentoCotacaoLink> TorcamentoCotacaoLinks { get; set; }

        public virtual TcfgOrcamentoCotacaoStatus TcfgOrcamentoCotacaoStatus { get; set; }

        public virtual Tusuario Tusuarios { get; set; }
    }
}
