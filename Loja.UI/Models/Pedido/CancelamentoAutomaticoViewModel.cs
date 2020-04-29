using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Loja.Bll.Bll.pedidoBll.CancelamentoAutomaticoBll;

namespace Loja.UI.Models.Pedido
{
    public class CancelamentoAutomaticoViewModel
    {
        public List<CancelamentoAutomaticoItem> cancelamentoAutomaticoItems { get; set; }
        public bool ConsultaUniversalPedidoOrcamento { get; set; }
        public bool MostrarLoja { get; set; }
        public List<Loja.Bll.Bll.AcessoBll.UsuarioAcessoBll.LojaPermtidaUsuario> LojasDisponiveis { get; set; }
    }
}
