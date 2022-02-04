using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        public async Task<IEnumerable<OrcamentistaIndicadorResponseViewModel>> BuscarParceiros(string vendedorId)
        {
            _logger.LogInformation("Buscando lista de parceiros");
            var usuarios = _orcamentistaEindicadorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { vendedorId = vendedorId });
//GetParceiros(vendedorId, 1, 1);
            return _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios);
        }

        [HttpGet]
        [Route("parceiros-por-vendedor")]
        public async Task<IEnumerable<OrcamentistaIndicadorResponseViewModel>> BuscarParceirosByVendedor(string vendedor)
        {
            _logger.LogInformation("Buscando lista de parceiros por vendedor");
            var usuarios = _orcamentistaEindicadorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { vendedorId = vendedor }).ToList();

            return _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios);
        }
    }
}
