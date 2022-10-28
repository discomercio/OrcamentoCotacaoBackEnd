using InfraBanco.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoCatalogoPropriedadeResponseViewModel : ResponseBase, IViewModelResponse
    {
        [JsonProperty("produtosCatalogo")]
        public List<TprodutoCatalogo> ProdutosCatalogo { get; set; }
    }
}
