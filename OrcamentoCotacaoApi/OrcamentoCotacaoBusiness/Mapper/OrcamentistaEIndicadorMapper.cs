using AutoMapper;
using InfraBanco.Modelos;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Mapper
{
    public class OrcamentistaEIndicadorMapper : Profile
    {
        public OrcamentistaEIndicadorMapper()
        {
            CreateMap<TorcamentistaEindicador, OrcamentistaIndicadorResponseViewModel>();
        }

    }
}
