using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentistaEindicador;
using OrcamentistaEIndicadorVendedor;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
    [TypeFilter(typeof(ControleDelayFilter))]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
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
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                VendedorId = vendedorId,
                Loja = loja
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ParceiroController/BuscarParceiros/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var usuarios = _orcamentistaEindicadorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { vendedorId = vendedorId, loja = loja });

            var response = _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ParceiroController/BuscarParceiros/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return response;
        }

        [HttpGet]
        [Route("vendedores-parceiros")]
        public IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresDosParceiros(string apelidoParceiro)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                ApelidoParceiro = apelidoParceiro
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ParceiroController/BuscarVendedoresDosParceiros/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var parceiro = _orcamentistaEindicadorBll
                .PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = apelidoParceiro }).FirstOrDefault();
            if (parceiro == null) return null;

            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = parceiro.IdIndicador });

            var response = _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ParceiroController/BuscarVendedoresDosParceiros/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return response;
        }

        [HttpGet]
        [Route("parceiros-por-vendedor")]
        public IEnumerable<OrcamentistaIndicadorResponseViewModel> BuscarParceirosByVendedor(string vendedor)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Vendedor = vendedor
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ParceiroController/BuscarParceirosByVendedor/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var usuarios = _orcamentistaEindicadorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { vendedorId = vendedor }).ToList();

            var response = _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ParceiroController/BuscarParceirosByVendedor/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return response;
        }
    }
}