using AutoMapper;
using InfraBanco.Modelos;
using InfraIdentity;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Mapper
{
    public class UsuarioMapper :Profile
    {
        public UsuarioMapper()
        {
            CreateMap<Tusuario, UsuarioRequestViewModel>();
            //CreateMap<UsuarioRequestViewModel, Usuario>();
            CreateMap<UsuarioLogin, UsuarioResponseViewModel>();
            CreateMap<UsuarioResponseViewModel, UsuarioLogin>();

            //CreateMap<Usuario, UsuarioResponseViewModel>();

            //CreateMap<Usuario, LoginResponseViewModel>();
        }
    }
}
