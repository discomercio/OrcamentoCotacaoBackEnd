using Loja.UI.Models.Comuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Loja.Bll.Bll.pedidoBll.CancelamentoAutomaticoBll;

#nullable enable
namespace Loja.UI.Models.Pedido
{
    public class CancelamentoAutomaticoViewModel
    {
        public CancelamentoAutomaticoViewModel(List<CancelamentoAutomaticoItem> cancelamentoAutomaticoItems, ListaLojasViewModel listaLojasViewModel)
        {
            CancelamentoAutomaticoItems = cancelamentoAutomaticoItems ?? throw new ArgumentNullException(nameof(cancelamentoAutomaticoItems));
            ListaLojasViewModel = listaLojasViewModel ?? throw new ArgumentNullException(nameof(listaLojasViewModel));
        }

        public List<CancelamentoAutomaticoItem> CancelamentoAutomaticoItems { get; }
        public ListaLojasViewModel ListaLojasViewModel { get; }
    }
}
