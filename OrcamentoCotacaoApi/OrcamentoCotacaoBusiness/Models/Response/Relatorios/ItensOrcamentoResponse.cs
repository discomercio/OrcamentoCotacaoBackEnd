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
        public string Loja { get; set; }

        public int Orcamento { get; set; }

        public string PrePedido { get; set; }

        public string Pedido { get; set; }

        public string Status { get; set; }

        public string Vendedor { get; set; }

        public string Indicador { get; set; }

        public string IndicadorVendedor { get; set; }

        public string UsuarioCadastro { get; set; }

        public string IdCliente { get; set; }

        public string UF { get; set; }

        public string TipoCliente { get; set; }

        public string ContribuinteIcms { get; set; }

        public string EntregaImediata { get; set; }

        public DateTime? PrevisaoEntrega { get; set; }

        public string InstaladorInstala { get; set; }

        public int NumOpcaoOrcamento { get; set; }

        public string FormaPagtoAVista { get; set; }

        public string FormaPagtoAPrazo { get; set; }

        public int? QtdeParcelasFormaPagtoAPrazo { get; set; }

        public string OpcaoAprovada { get; set; }

        public string FormaPagtoOpcaoAprovada { get; set; }

        public string Fabricante { get; set; }

        public string Produto { get; set; }

        public int Qtde { get; set; }

        public string DescricaoProduto { get; set; }

        public string Categoria { get; set; }

        public decimal? PrecoListaUnitAVista { get; set; }

        public decimal? PrecoListaUnitAPrazo { get; set; }

        public decimal? PrecoNFUnitAVista { get; set; }

        public decimal? PrecoNFUnitAPrazo { get; set; }

        public decimal? DescontoAVista { get; set; }

        public decimal? DescontoAPrazo { get; set; }

        public decimal? DescSuperiorAVista { get; set; }

        public decimal? DescSuperiorAPrazo { get; set; }

        public decimal? Comissao { get; set; }

        public DateTime? DataCadastro { get; set; }

        public DateTime? Validade { get; set; }
    }
}
