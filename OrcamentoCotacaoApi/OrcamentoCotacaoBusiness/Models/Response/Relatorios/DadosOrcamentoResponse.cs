using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.Relatorios
{
    public class DadosOrcamentoResponse
    {
        public string Loja { get; set; }
        public int Orcamento { get; set; }
        public string Status { get; set; }
        public string PrePedido { get; set; }
        public string Pedido { get; set; }
        public string Vendedor { get; set; }
        public string Indicador { get; set; }
        public string IndicadorVendedor { get; set; }
        public string IdCliente { get; set; }
        public string UsuarioCadastro { get; set; }
        public string UF { get; set; }
        public string TipoCliente { get; set; }
        public string ContribuinteIcms { get; set; }
        public int QtdeMsgPendente { get; set; }
        public string EntregaImediata { get; set; }
        public DateTime? PrevisaoEntrega { get; set; }
        public string InstaladorInstala { get; set; }
        public float? ComissaoOpcao1 { get; set; }
        public decimal? DescMedioAVistaOpcao1 { get; set; }
        public decimal? DescMedioAPrazoOpcao1 { get; set; }
        public string FormaPagtoAVistaOpcao1 { get; set; }
        public decimal? ValorFormaPagtoAVistaOpcao1 { get; set; }
        public string StatusDescSuperiorAVistaOpcao1 { get; set; }
        public string FormaPagtoAPrazoOpcao1 { get; set; }
        public decimal? ValorFormaPagtoAPrazoOpcao1 { get; set; }
        public int? QtdeParcelasFormaPagtoAPrazoOpcao1 { get; set; }
        public string StatusDescSuperiorAPrazoOpcao1 { get; set; }
        public float? ComissaoOpcao2 { get; set; }
        public decimal? DescMedioAVistaOpcao2 { get; set; }
        public decimal? DescMedioAPrazoOpcao2 { get; set; }
        public string FormaPagtoAVistaOpcao2 { get; set; }
        public decimal? ValorFormaPagtoAVistaOpcao2 { get; set; }
        public string StatusDescSuperiorAVistaOpcao2 { get; set; }
        public string FormaPagtoAPrazoOpcao2 { get; set; }
        public decimal? ValorFormaPagtoAPrazoOpcao2 { get; set; }
        public int? QtdeParcelasFormaPagtoAPrazoOpcao2 { get; set; }
        public string StatusDescSuperiorAPrazoOpcao2 { get; set; }
        public float? ComissaoOpcao3 { get; set; }
        public decimal? DescMedioAVistaOpcao3 { get; set; }
        public decimal? DescMedioAPrazoOpcao3 { get; set; }
        public string FormaPagtoAVistaOpcao3 { get; set; }
        public decimal? ValorFormaPagtoAVistaOpcao3 { get; set; }
        public string StatusDescSuperiorAVistaOpcao3 { get; set; }
        public string FormaPagtoAPrazoOpcao3 { get; set; }
        public decimal? ValorFormaPagtoAPrazoOpcao3 { get; set; }
        public int? QtdeParcelasFormaPagtoAPrazoOpcao3 { get; set; }
        public string StatusDescSuperiorAPrazoOpcao3 { get; set; }
        public short? OpcaoAprovada { get; set; }
        public float? ComissaoOpcaoAprovada { get; set; }
        public decimal? DescMedioOpcaoAprovada { get; set; }
        public string FormaPagtoOpcaoAprovada { get; set; }
        public decimal? ValorFormaPagtoOpcaoAprovada { get; set; }
        public int? QtdeParcelasFormaOpcaoAprovada { get; set; }
        public string StatusDescSuperiorOpcaoAprovada { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? Validade { get; set; }
    }
}
