using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class OrcamentoRequestViewModel : IViewModelRequest
    {
        [JsonProperty("Vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("Parceiro")]
        public string Parceiro { get; set; }

        [JsonProperty("VendedorParceiro")]
        public string VendedorParceiro { get; set; }

        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("Validade")]
        public DateTime Validade { get; set; }

        [JsonProperty("qtdeRenovacao")]
        public int QtdeRenovacao { get; set; }

        [JsonProperty("ConcordaWhatsapp")]
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
        public List<OrcamentoOpcaoRequestViewModel> ListaOrcamentoCotacaoDto { get; set; }

        [JsonProperty("Id")]
        public long Id { get; set; }
    }
}
