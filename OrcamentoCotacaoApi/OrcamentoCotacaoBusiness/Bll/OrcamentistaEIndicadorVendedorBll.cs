﻿using AutoMapper;
using OrcamentistaEindicador;
using OrcamentoCotacaoBusiness.Models.Response;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentistaEIndicadorVendedorBll
    {
        private readonly OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll;
        private readonly IMapper _mapper;
        public OrcamentistaEIndicadorVendedorBll(
            OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll, 
            IMapper _mapper,
             OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll
            )
        {
            this._orcamentistaEindicadorVendedorBll = _orcamentistaEindicadorVendedorBll;
            this._mapper = _mapper;
            this._orcamentistaEIndicadorBll = _orcamentistaEIndicadorBll;
        }

        public List<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresParceiro(string apelidoParceiro)
        {
            var parceiro = _orcamentistaEIndicadorBll
                .BuscarParceiros(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = apelidoParceiro, acessoHabilitado = 1 }).FirstOrDefault();
          
            if (parceiro == null) 
                return null;

            var vendedoresParceiro = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = parceiro.IdIndicador });
            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(vendedoresParceiro);
        }

        public List<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresParceiroPorId(int idParceiro)
        {
            var parceiro = _orcamentistaEIndicadorBll
                .BuscarParceiros(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { idParceiro = idParceiro, acessoHabilitado = 1 }).FirstOrDefault();

            if (parceiro == null)
                return null;

            var vendedoresParceiro = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = parceiro.IdIndicador });
            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(vendedoresParceiro);
        }
    }
}
