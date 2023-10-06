using InfraBanco;
using InfraBanco.Modelos.Filtros;
using Relatorios.Dto;
using Relatorios.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relatorios
{
    public class RelatoriosBll
    {
        private RelatoriosData _data { get; set; }

        public RelatoriosBll(ContextoRelatorioProvider contextoProvider)
        {
            _data = new RelatoriosData(contextoProvider);
        }

        public List<ItensOrcamentoDto> RelatorioItensOrcamento(ItensOrcamentosFiltro filtro)
        {
            return _data.RelatorioItensOrcamento(filtro);
        }

        public List<DadosOrcamentoDto> RelatorioDadosOrcamento(DadosOrcamentosFiltro filtro)
        {
            return _data.RelatorioDadosOrcamento(filtro);
        }
    }
}
