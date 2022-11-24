using AutoMapper;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Mapper
{
    public class FormaPagtoMapper : Profile
    {
        public FormaPagtoMapper()
        {
            CreateMap<FormaPagtoCriacaoResponseViewModel, FormaPagtoCriacaoRequest>();
        }
    }
}
