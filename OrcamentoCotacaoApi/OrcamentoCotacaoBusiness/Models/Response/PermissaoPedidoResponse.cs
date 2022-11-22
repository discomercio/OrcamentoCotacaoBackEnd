namespace OrcamentoCotacaoBusiness.Models.Response
{
    public sealed class PermissaoPedidoResponse : ResponseBase
    {
        public bool VisualizarPedido { get; set; }
        public bool PrePedidoVirouPedido { get; set; }
        public string IdPedido { get; set; }
    }
}