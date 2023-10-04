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

        public string PrevisaoEntrega { get; set; }

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

        public string PrecoListaUnitAVista { get; set; }

        public string PrecoListaUnitAPrazo { get; set; }

        public string PrecoNFUnitAVista { get; set; }

        public string PrecoNFUnitAPrazo { get; set; }

        public string DescontoAVista { get; set; }

        public string DescontoAPrazo { get; set; }

        public string DescSuperiorAVista { get; set; }

        public string DescSuperiorAPrazo { get; set; }

        public string Comissao { get; set; }

        public string DataCadastro { get; set; }

        public string Validade { get; set; }
    }
}
