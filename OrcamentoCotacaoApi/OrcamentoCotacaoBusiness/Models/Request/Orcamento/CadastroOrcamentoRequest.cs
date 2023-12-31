﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UtilsGlobais.RequestResponse;

namespace OrcamentoCotacaoBusiness.Models.Request.Orcamento
{
    public class CadastroOrcamentoRequest : RequestBase
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

        [JsonProperty("instaladorInstala")]
        public int InstaladorInstala { get; set; }

        [JsonProperty("clienteOrcamentoCotacaoDto")]
        public CadastroOrcamentoClienteRequest ClienteOrcamentoCotacaoDto { get; set; }

        [JsonProperty("listaOrcamentoCotacaoDto")]
        public List<CadastroOrcamentoOpcaoRequest> ListaOrcamentoCotacaoDto { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("versaoPoliticaCredito")]
        public string VersaoPoliticaCredito { get; set; }

        [JsonProperty("versaoPoliticaPrivacidade")]
        public string VersaoPoliticaPrivacidade { get; set; }
    }
}
