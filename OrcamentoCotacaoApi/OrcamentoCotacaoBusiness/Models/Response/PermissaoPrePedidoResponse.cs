namespace OrcamentoCotacaoBusiness.Models.Response
{
    public sealed class PermissaoPrePedidoResponse : ResponseBase
    {
        public bool VisualizarPrePedido { get; set; }
        public bool CancelarPrePedido { get; set; }
    }
}