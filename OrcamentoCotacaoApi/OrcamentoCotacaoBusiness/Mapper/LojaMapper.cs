using AutoMapper;
using InfraBanco.Modelos;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Mapper
{
    public class LojaMapper : Profile
    {
        public LojaMapper()
        {
            CreateMap<Tloja, LojaResponseViewModel>();
        }
    }
}
