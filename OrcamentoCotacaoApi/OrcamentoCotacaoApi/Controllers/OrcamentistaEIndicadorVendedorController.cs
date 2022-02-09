using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentistaEIndicadorVendedor;
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
    public class OrcamentistaEIndicadorVendedorController : BaseController
    {
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll;
        private readonly ILogger<UsuarioController> _logger;
        private readonly IMapper _mapper;

        public OrcamentistaEIndicadorVendedorController(OrcamentistaEIndicadorVendedorBll orcamentistaEindicadorVendedorBll, ILogger<UsuarioController> logger,
            IMapper mapper)
        {
            this._orcamentistaEindicadorVendedorBll = orcamentistaEindicadorVendedorBll;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpGet]
        [Route("vendedores-parceiros")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresDosParceiros(string parceiro)
        {
            _logger.LogInformation("Buscando lista de vendedores parceiros");
            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = parceiro });

            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);
        }
    }
}
