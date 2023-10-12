using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Request.Relatorios
{
    public class ItensOrcamentoRequest 
    {
        public string[] Lojas { get; set; }
        public string[] Status { get; set; }
        public string[] Vendedores { get; set; }
        public bool? ComIndicador { get; set; }
        public string[] Parceiros { get; set; }
        public string OpcoesOrcamento { get; set; }
        public string[] Fabricantes { get; set; }
        public string[] Categorias { get; set; }
        public string LojaLogada { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
        public DateTime? DtInicioExpiracao { get; set; }
        public DateTime? DtFimExpiracao { get; set; }

    }
}
