using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public sealed class PermissaoIncluirPrePedidoRequest
    {
        public List<string> PermissoesUsuario { get; set; }
    }
}