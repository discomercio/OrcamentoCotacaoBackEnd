using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.UI.Models.Home
{
    public class HomeViewModel
    {
        public string? LojaAtivaId { get; set; }
        public string? LojaAtivaNome { get; set; }
        public bool ErroChavearLoja { get; set; }
        public string? LojaTentandoChavearId { get; set; }
    }
}
