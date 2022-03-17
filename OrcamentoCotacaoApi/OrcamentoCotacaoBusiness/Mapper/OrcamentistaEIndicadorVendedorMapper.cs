using AutoMapper;
using InfraBanco.Modelos;
using OrcamentoCotacaoBusiness.Models.Request;
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
            CreateMap<UsuarioRequestViewModel, TorcamentistaEIndicadorVendedor>();
            CreateMap<TorcamentistaEIndicadorVendedor, UsuarioResponseViewModel>();
        }
    }
}
