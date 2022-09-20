using Newtonsoft.Json;
using PrepedidoBusiness.Dto.ClienteCadastro;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class AprovarOrcamentoRequestViewModel : IViewModelRequest
    {
        [JsonProperty("idOrcamento")]
        public int IdOrcamento { get; set; }

        [JsonProperty("idOpcao")]
        public int IdOpcao { get; set; }

        [JsonProperty("idFormaPagto")]
        public int IdFormaPagto { get; set; }

        [JsonProperty("clienteCadastroDto")]
        public ClienteCadastroDto ClienteCadastroDto { get; set; }
    }
}
