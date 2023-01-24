using AutoMapper;
using InfraBanco.Modelos;
using InfraIdentity;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Mapper
{
    public class UsuarioMapper : Profile
    {
        public UsuarioMapper()
        {
            CreateMap<Tusuario, UsuarioRequestViewModel>();
            CreateMap<Tusuario, UsuarioResponseViewModel>();
            CreateMap<TorcamentistaEIndicadorVendedor, UsuarioResponseViewModel>();
            CreateMap<UsuarioResponseViewModel, Tusuario>();
            CreateMap<UsuarioLogin, UsuarioResponseViewModel>();
            CreateMap<UsuarioResponseViewModel, UsuarioLogin>();
            CreateMap<Tusuario, UsuariosPorListaLojasResponse>();
        }
    }
}
