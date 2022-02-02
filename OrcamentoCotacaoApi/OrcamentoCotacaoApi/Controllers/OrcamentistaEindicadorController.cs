﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentistaEindicador;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrcamentistaEindicadorController : BaseController
    {
        private readonly OrcamentistaEindicadorBll _orcamentistaEindicadorBll;
        private readonly ILogger<UsuarioController> _logger;
        private readonly IMapper _mapper;

        public OrcamentistaEindicadorController(OrcamentistaEindicadorBll orcamentistaEindicadorBll, ILogger<UsuarioController> logger,
            IMapper mapper)
        {
            this._orcamentistaEindicadorBll = orcamentistaEindicadorBll;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpGet]
        [Route("BuscarParceiros")]
        public async Task<IEnumerable<OrcamentistaIndicadorResponseViewModel>> BuscarParceiros(string vendedorId, string loja)
        {
            _logger.LogInformation("Buscando lista de parceiros");
            var usuarios = _orcamentistaEindicadorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { vendedorId = vendedorId, loja = loja });

            return _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios); ;
        }

        //[HttpGet]
        //[Route("OrcamentistaEIndicador")]
        //public async Task<IEnumerable<UsuarioResponseViewModel>> BuscarParceirosByVendedor(string vendedor)
        //{
        //    _logger.LogInformation("Buscando lista de parceiros por vendedor");
        //    var usuarios = await _orcamentistaEindicadorBll.GetParceirosByVendedor(vendedor);

        //    return _mapper.Map<List<UsuarioResponseViewModel>>(usuarios);
        //}
    }
}
