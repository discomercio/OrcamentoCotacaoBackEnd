using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public sealed class PermissaoPrePedidoRequest
    {
        public List<string> PermissoesUsuario { get; set; }
        public int TipoUsuario { get; set; }
        public string Usuario { get; set; }
        public string IdPrePedido { get; set; }
    }
}