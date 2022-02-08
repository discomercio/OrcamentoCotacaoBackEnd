using AutoMapper;
using InfraBanco.Modelos;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Mapper
{
    public class OrcamentistaEIndicadorVendedorMapper : Profile
    {
        public OrcamentistaEIndicadorVendedorMapper()
        {
            CreateMap<TorcamentistaEIndicadorVendedor, OrcamentistaEIndicadorVendedorResponseViewModel>();
        }
    }
}
