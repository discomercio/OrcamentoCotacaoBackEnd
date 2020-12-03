using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.UI.Models.Home
{
    public class HomeViewModel
    {
        public string LojaAtivaId { get; set; }
        public string LojaAtivaNome { get; set; }
        public bool ErroChavearLoja { get; set; }
        public string LojaTentandoChavearId { get; set; }
        public List<Bll.Bll.pedidoBll.CancelamentoAutomaticoBll.CancelamentoAutomaticoItem> CancelamentoAutomaticoItems { get; set; } = null;
        public Bll.Dto.PrepedidoDto.ResumoPrepedidoListaDto ResumoPrepedidoListaDto { get; set; } = null;
        public List<Bll.Dto.AvisosDto.AvisoDto> AvisoDto { get; set; }
    }
}
