using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.Relatorios
{
    public class ItensOrcamentoResponse
    {
        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("orcamento")]
        public int Orcamento { get; set; }

        [JsonProperty("prepedido")]
        public string Prepedido { get; set; }

        [JsonProperty("pedido")]
        public string Pedido { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("parceiro")]
        public string Parceiro { get; set; }

        [JsonProperty("vendedorParceiro")]
        public string VendedorParceiro { get; set; }

        [JsonProperty("usuarioCadastro")]
        public string UsuarioCadastro { get; set; }

        [JsonProperty("idCliente")]
        public string IdCliente { get; set; }

        [JsonProperty("uf")]
        public string UF { get; set; }

        [JsonProperty("tipoCliente")]
        public string TipoCliente { get; set; }

        [JsonProperty("contribuinteIcms")]
        public string ContribuinteIcms { get; set; }

        [JsonProperty("entregaImediata")]
        public string EntregaImediata { get; set; }

        [JsonProperty("previsaoEntrega")]
        public string PrevisaoEntrega { get; set; }

        [JsonProperty("instaladoInstala")]
        public string InstaladorInstala { get; set; }

        [JsonProperty("numOpcaoOrcamento")]
        public int NumOpcaoOrcamento { get; set; }

        [JsonProperty("formaPagtoAvista")]
        public string FormaPagtoAVista { get; set; }

        [JsonProperty("formaPagtoAprazo")]
        public string FormaPagtoAPrazo { get; set; }

        [JsonProperty("qtdeParcelasFormaPagtoAprazo")]
        public int? QtdeParcelasFormaPagtoAPrazo { get; set; }

        [JsonProperty("opcaoAprovada")]
        public string OpcaoAprovada { get; set; }

        [JsonProperty("formaPagtoOpcaoAprovada")]
        public string FormaPagtoOpcaoAprovada { get; set; }

        [JsonProperty("fabricante")]
        public string Fabricante { get; set; }

        [JsonProperty("produto")]
        public string Produto { get; set; }

        [JsonProperty("qtde")]
        public int Qtde { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("precoListaUnitAvista")]
        public string PrecoListaUnitAVista { get; set; }

        [JsonProperty("precoListaUniAprazo")]
        public string PrecoListaUnitAPrazo { get; set; }

        [JsonProperty("precoNFUniAvista")]
        public string PrecoNFUnitAVista { get; set; }

        [JsonProperty("precoNFUnitAprazo")]
        public string PrecoNFUnitAPrazo { get; set; }

        [JsonProperty("descontoAvista")]
        public string DescontoAVista { get; set; }

        [JsonProperty("descontoAprazo")]
        public string DescontoAPrazo { get; set; }

        [JsonProperty("descSuperiorAvista")]
        public string DescSuperiorAVista { get; set; }

        [JsonProperty("descSuperiorAprazo")]
        public string DescSuperiorAPrazo { get; set; }

        [JsonProperty("comissao")]
        public string Comissao { get; set; }

        [JsonProperty("criacao")]
        public string Criacao { get; set; }

        [JsonProperty("expiracao")]
        public string Expiracao { get; set; }
    }
}
