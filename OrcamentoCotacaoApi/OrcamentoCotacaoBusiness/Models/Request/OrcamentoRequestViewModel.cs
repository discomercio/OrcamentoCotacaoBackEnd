using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class OrcamentoRequestViewModel : IViewModelRequest
    {
        [JsonProperty("vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("parceiro")]
        public string Parceiro { get; set; }

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
        public List<OrcamentoOpcaoRequestViewModel> ListaOrcamentoCotacaoDto { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}
