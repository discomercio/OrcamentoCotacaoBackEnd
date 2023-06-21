using AutoMapper;
using InfraBanco.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request.OrcamentistaIndicador;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
    [TypeFilter(typeof(ControleDelayFilter))]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
    public class OrcamentistaEindicadorController : BaseController
    {
        private readonly OrcamentistaEindicador.OrcamentistaEIndicadorBll _orcamentistaEindicadorGlobalBll;
        private readonly ILogger<UsuarioController> _logger;
        private readonly IMapper _mapper;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBll;

        public OrcamentistaEindicadorController(
            OrcamentistaEindicador.OrcamentistaEIndicadorBll orcamentistaEindicadorBll, 
            ILogger<UsuarioController> logger,
            IMapper mapper,
            OrcamentistaEIndicadorBll orcamentistaEIndicadorBll)
        {
            this._orcamentistaEindicadorGlobalBll = orcamentistaEindicadorBll;
            this._logger = logger;
            this._mapper = mapper;
            _orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
        }

        [HttpGet]
        [Route("BuscarParceiros")]
        public async Task<IEnumerable<OrcamentistaIndicadorResponseViewModel>> BuscarParceiros(string vendedorId, string loja)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                VendedorId = vendedorId,
                Loja = loja
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEindicadorController/BuscarParceiros/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var usuarios = _orcamentistaEindicadorGlobalBll.PorFiltro(
                new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { vendedorId = vendedorId, loja = loja, status = Constantes.ORCAMENTISTA_INDICADOR_STATUS_ATIVO });
            usuarios.Insert(0, _orcamentistaEindicadorGlobalBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = Constantes.SEM_INDICADOR, status = Constantes.ORCAMENTISTA_INDICADOR_STATUS_INATIVO }).FirstOrDefault());

            var result = _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios);

            var response = new
            {
                OrcamentistaIndicador = result.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEindicadorController/BuscarParceiros/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return result;
        }

        [HttpGet]
        [Route("BuscarParceirosPorLoja")]
        public async Task<IEnumerable<OrcamentistaIndicadorResponseViewModel>> BuscarParceiros(string loja)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Loja = loja
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEindicadorController/BuscarParceiros/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var usuarios = _orcamentistaEindicadorGlobalBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { loja = loja,  status = Constantes.ORCAMENTISTA_INDICADOR_STATUS_ATIVO });
            if (usuarios != null)
                usuarios = usuarios.OrderBy(o => o.Apelido).ToList();
            usuarios.Insert(0, _orcamentistaEindicadorGlobalBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = Constantes.SEM_INDICADOR, status = Constantes.ORCAMENTISTA_INDICADOR_STATUS_INATIVO }).FirstOrDefault());

            var result = _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios);

            var response = new
            {
                OrcamentistaIndicador = result.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEindicadorController/BuscarParceiros/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return result;
        }

        [HttpGet]
        [Route("parceiros-por-vendedor")]
        public async Task<IEnumerable<OrcamentistaIndicadorResponseViewModel>> BuscarParceirosByVendedor(string vendedor)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Vendedor = vendedor
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEindicadorController/BuscarParceirosByVendedor/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var usuarios = _orcamentistaEindicadorGlobalBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { vendedorId = vendedor }).ToList();

            var result = _mapper.Map<List<OrcamentistaIndicadorResponseViewModel>>(usuarios);

            var response = new
            {
                OrcamentistaIndicador = result.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEindicadorController/BuscarParceirosByVendedor/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return result;
        }

        [HttpGet]
        [Route("parceiro-por-apelido/{apelido}")]
        public async Task<OrcamentistaIndicadorResponseViewModel> BuscarParceiroPorApelido(string apelido)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Apelido = apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEindicadorController/BuscarParceiroPorApelido/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var usuario = _orcamentistaEindicadorGlobalBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = apelido }).FirstOrDefault();

            var result = _mapper.Map<OrcamentistaIndicadorResponseViewModel>(usuario);

            var response = new
            {
                OrcamentistaIndicador = result
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEindicadorController/BuscarParceiroPorApelido/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return result;
        }

        [HttpPost]
        [Route("buscarParceirosPorIdVendedor")]
        public async Task<IActionResult> BuscarParceirosPorIdVendedor(BuscarParceiroRequest request)
        {
            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.Usuario = LoggedUser.Apelido;
            request.IP = HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. OrcamentistaEindicadorController/BuscarParceirosPorIdVendedor/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var response = _orcamentistaEIndicadorBll.BuscarParceirosCombo(request);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. OrcamentistaEindicadorController/BuscarParceirosPorIdVendedor/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }
    }
}