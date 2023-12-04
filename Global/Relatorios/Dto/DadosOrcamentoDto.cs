namespace Relatorios.Dto
{
    public class DadosOrcamentoDto
    {
        public string Loja { get; set; }
        public int Orcamento { get; set; }
        public string Status { get; set; }
        public string Prepedido { get; set; }
        public string Pedido { get; set; }
        public string Vendedor { get; set; }
        public string Parceiro { get; set; }
        public string VendedorParceiro { get; set; }
        public string IdCliente { get; set; }
        public string UsuarioCadastro { get; set; }
        public string UF { get; set; }
        public string TipoCliente { get; set; }
        public string ContribuinteIcms { get; set; }
        public int QtdeMsgPendente { get; set; }
        public string EntregaImediata { get; set; }
        public string PrevisaoEntrega { get; set; }
        public string InstaladorInstala { get; set; }
        public List<OpcaoDadosOrcamento> ListaOpcoes { get; set; }
        public short? OpcaoAprovada { get; set; }
        public float? ComissaoOpcaoAprovada { get; set; }
        public string DescMedioOpcaoAprovada { get; set; }
        public string FormaPagtoOpcaoAprovada { get; set; }
        public string ValorFormaPagtoOpcaoAprovada { get; set; }
        public int? QtdeParcelasFormaOpcaoAprovada { get; set; }
        public string StatusDescSuperiorOpcaoAprovada { get; set; }
        public string Criacao { get; set; }
        public string Expiracao { get; set; }
    }

    public class OpcaoDadosOrcamento
    {
        public float? ComissaoOpcao { get; set; }
        public string DescMedioAVistaOpcao { get; set; }
        public string DescMedioAPrazoOpcao { get; set; }
        public string FormaPagtoAVistaOpcao { get; set; }
        public string ValorFormaPagtoAVistaOpcao { get; set; }
        public string StatusDescSuperiorAVistaOpcao { get; set; }
        public string FormaPagtoAPrazoOpcao { get; set; }
        public string ValorFormaPagtoAPrazoOpcao { get; set; }
        public int? QtdeParcelasFormaPagtoAPrazoOpcao { get; set; }
        public string StatusDescSuperiorAPrazoOpcao { get; set; }
    }
}
