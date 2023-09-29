using AutoMapper;
using OrcamentoCotacaoBusiness.Models.Request.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Mapper
{
    public class RelatoriosMapper : Profile
    {
        public RelatoriosMapper()
        {
            CreateMap<ItensOrcamentoRequest, Relatorios.Filtros.ItensOrcamentosFiltro>();
        }
    }
}
