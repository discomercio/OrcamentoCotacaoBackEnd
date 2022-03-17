using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentistaEindicador;
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
    public class ParceiroController : BaseController
    {
        private readonly OrcamentistaEIndicadorBll _orcamentistaEindicadorBll;
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll;
        private readonly ILogger<ParceiroController> _logger;
        private readonly IMapper _mapper;

        public ParceiroController(
            OrcamentistaEIndicadorVendedorBll orcamentistaEindicadorVendedorBll, 
            OrcamentistaEIndicadorBll orcamentistaEindicadorBll, 
            ILogger<ParceiroController> logger,
            IMapper mapper)
        {
            this._orcamentistaEindicadorBll = orcamentistaEindicadorBll;
            this._orcamentistaEindicadorVendedorBll = orcamentistaEindicadorVendedorBll;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<OrcamentistaIndicadorResponseViewModel> BuscarParceiros(string vendedorId, string loja)
        {
            _logger.LogInformation("Buscando lista de parceiros");
            var usuarios = _orcamentistaEindicadorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { vendedorId = vendedorId, loja = loja });

            return _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios); ;
        }

        [HttpGet]
        [Route("vendedores-parceiros")]
        public IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresDosParceiros(string apelidoParceiro)
        {
            _logger.LogInformation("Buscando lista de vendedores parceiros");
            var parceiro = _orcamentistaEindicadorBll
                .PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = apelidoParceiro }).FirstOrDefault();
            if (parceiro == null) return null;

            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = parceiro.Id });

            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);
        }

        [HttpGet]
        [Route("parceiros-por-vendedor")]
        public IEnumerable<OrcamentistaIndicadorResponseViewModel> BuscarParceirosByVendedor(string vendedor)
        {
            _logger.LogInformation("Buscando lista de parceiros por vendedor");
            var usuarios = _orcamentistaEindicadorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { vendedorId = vendedor }).ToList();

            return _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios);
        }
    }
}
