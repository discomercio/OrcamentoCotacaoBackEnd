using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class PedidoResultadoMagentoDto
    {
        public string IdPedidoCadastrado { get; set; }
        public List<string> IdsPedidosFilhotes { get; set; } = new List<string>();
        public List<string> ListaErros { get; set; } = new List<string>();
    }
}
