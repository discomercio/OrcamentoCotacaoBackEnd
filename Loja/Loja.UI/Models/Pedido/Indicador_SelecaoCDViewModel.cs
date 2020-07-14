using Loja.Bll.Dto.IndicadorDto;
using Loja.Bll.Dto.LojaDto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.UI.Models.Pedido
{
    public class Indicador_SelecaoCDViewModel
    {
        public string NomeCliente { get; set; }
        public string IndicadorOriginal { get; set; }
        public List<IndicadorDto> ListaIndicadores { get; set; }
        public SelectList ListaCD { get; set; }
        public string ListaOperacoesPermitidas { get; set; }
        public SelectList PedBonshop { get; set; }
        public PercentualMaximoDto PercMaxPorLoja { get; set; }
        public int ComIndicacao { get; set; }//verificar essa var para saber se é com ou sem indicação
        public string LojaAtual { get; set; }
    }
}
