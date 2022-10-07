using Newtonsoft.Json;
using OrcamentoCotacaoBusiness.Models.Request;
using System;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class OrcamentoResponseViewModel : IViewModelResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("vendedor")]
        public string Vendedor { get; set; }
        [JsonProperty("nomeIniciaisEmMaiusculasVendedor")]
        public string NomeIniciaisEmMaiusculasVendedor { get; set; }

        [JsonProperty("parceiro")]
        public string Parceiro { get; set; }

        [JsonProperty("razaoSocialNomeIniciaisEmMaiusculasParceiro")]
        public string RazaoSocialNomeIniciaisEmMaiusculasParceiro { get; set; }

        [JsonProperty("vendedorParceiro")]
        public string VendedorParceiro { get; set; }

        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("validade")]
        public DateTime Validade { get; set; }

        [JsonProperty("qtdeRenovacao")]
        public int QtdeRenovacao { get; set; }

        [JsonProperty("concordaWhatsapp")]
        public bool ConcordaWhatsapp { get; set; }

        [JsonProperty("observacoesGerais")]
        public string ObservacoesGerais { get; set; }

        [JsonProperty("entregaImediata")]
        public bool EntregaImediata { get; set; }

        [JsonProperty("dataEntregaImediata")]
        public DateTime? DataEntregaImediata { get; set; }

        [JsonProperty("clienteOrcamentoCotacaoDto")]
        public ClienteOrcamentoCotacaoRequestViewModel ClienteOrcamentoCotacaoDto { get; set; }

        [JsonProperty("listaOrcamentoCotacaoDto")]
        public List<OrcamentoOpcaoResponseViewModel> ListaOrcamentoCotacaoDto { get; set; }

        [JsonProperty("cadastradoPor")]
        public string CadastradoPor { get; set; }

        [JsonProperty("amigavelCadastradoPor")]
        public string AmigavelCadastradoPor { get; set; }

        [JsonProperty("dataCadastro")]
        public DateTime DataCadastro { get; set; }

        [JsonProperty("idIndicador")]
        public int? IdIndicador { get; set; }

        [JsonProperty("idIndicadorVendedor")]
        public int? IdIndicadorVendedor { get; set; }

        [JsonProperty("idVendedor")]
        public int IdVendedor { get; set; }

        [JsonProperty("status")]
        public short Status { get; set; }

        [JsonProperty("statusEmail")]
        public string statusEmail { get; set; }

        [JsonProperty("instaladorInstala")]
        public int InstaladorInstala { get; set; }

    }
}
