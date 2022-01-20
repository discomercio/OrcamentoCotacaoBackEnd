using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OrcamentoCotacaoBusiness.Models.Response
{

    public partial class FormaPagamentoResponseViewModel
    {
        [JsonProperty("ListaAvista")]
        public List<Parcela> ListaAvista { get; set; }

        [JsonProperty("ListaParcUnica")]
        public List<Parcela> ListaParcUnica { get; set; }

        [JsonProperty("ParcCartaoInternet")]
        public bool ParcCartaoInternet { get; set; }

        [JsonProperty("ParcCartaoMaquineta")]
        public bool ParcCartaoMaquineta { get; set; }

        [JsonProperty("ListaParcComEntrada")]
        public List<Parcela> ListaParcComEntrada { get; set; }

        [JsonProperty("ListaParcComEntPrestacao")]
        public List<Parcela> ListaParcComEntPrestacao { get; set; }

        [JsonProperty("ListaParcSemEntPrimPrest")]
        public object ListaParcSemEntPrimPrest { get; set; }

        [JsonProperty("ListaParcSemEntPrestacao")]
        public object ListaParcSemEntPrestacao { get; set; }
    }

    public partial class Parcela
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Descricao")]
        public string Descricao { get; set; }

        [JsonProperty("Ordenacao")]
        public long Ordenacao { get; set; }
    }
}
