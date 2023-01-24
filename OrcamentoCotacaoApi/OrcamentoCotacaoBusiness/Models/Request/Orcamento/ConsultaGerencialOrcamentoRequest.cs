using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UtilsGlobais.RequestResponse;

namespace OrcamentoCotacaoBusiness.Models.Request.Orcamento
{
    public class ConsultaGerencialOrcamentoRequest: RequestBase
    {
        [JsonProperty("lojas")]
        public List<string> Lojas { get; set; }

        [JsonProperty("vendedor")]
        public int IdVendedor { get; set; }

        [JsonProperty("comParceiro")]
        public bool? ComParceiro { get; set; }

        [JsonProperty("idParceiro")]
        public int IdParceiro { get; set; }

        [JsonProperty("idVendedorParceiro")]
        public int IdVendedorParceiro { get; set; }

        [JsonProperty("fabricante")]
        public string Fabricante { get; set; }

        [JsonProperty("grupo")]
        public string Grupo { get; set; }

        [JsonProperty("dataCriacaoInicio")]
        public DateTime? DataCricaoInicio { get; set; }

        [JsonProperty("dataCriacaoFim")]
        public DateTime? DataCriacaoFim { get; set; }

        [JsonProperty("dataCorrente")]
        public DateTime? DataCorrente { get; set; }

        [JsonProperty("pagina")]
        public int Pagina { get; set; }

        [JsonProperty("qtdeItensPagina")]
        public int QtdeItensPagina { get; set; }

        [JsonProperty("ordenacaoAscendente")]
        public bool Ascendente { get; set; }

        [JsonProperty("nomeColunaOrdenacao")]
        public string NomeColunaOrdenacao { get; set; }

        [JsonProperty("mensagemPendente")]
        public bool MensagemPendente { get; set; }

        [JsonProperty("expirado")]
        public bool Expirado { get; set; }

        [JsonProperty("nomeLista")]
        public string NomeLista { get; set; }
    }
}
