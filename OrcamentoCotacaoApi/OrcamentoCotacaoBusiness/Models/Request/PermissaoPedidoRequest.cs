using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public sealed class PermissaoPedidoRequest
    {
        public List<string> PermissoesUsuario { get; set; }
        public int TipoUsuario { get; set; }
        public string Usuario { get; set; }
        public int IdPedido { get; set; }
    }
}