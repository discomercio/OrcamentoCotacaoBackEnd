using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
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

        public OrcamentistaEIndicadorVendedorBll OrcamentistaEindicadorVendedorBll => _orcamentistaEindicadorVendedorBll;

        [HttpGet]
        [Route("vendedores-parceiros")]
        public IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresDosParceiros(string parceiro)
        {
            _logger.LogInformation("Buscando lista de vendedores parceiros");

            var vendedoresParceiros = _orcamentistaEindicadorVendedorBll.BuscarVendedoresParceiro(parceiro);

            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(vendedoresParceiros);
        }
    }
}
