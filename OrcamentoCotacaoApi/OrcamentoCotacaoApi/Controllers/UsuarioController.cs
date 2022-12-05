using AutoMapper;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentistaEindicador;
using OrcamentistaEIndicadorVendedor;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Usuario;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
    public class UsuarioController : BaseController
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly UsuarioBll _usuarioBll;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEIndicadorBLL;
        private readonly IMapper _mapper;
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEIndicadorVendedorBll;

        public UsuarioController(ILogger<UsuarioController> logger, UsuarioBll usuarioBll,
            OrcamentistaEIndicadorBll orcamentistaEIndicadorBLL, IMapper mapper,
            OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll)
        {
            _logger = logger;
            _usuarioBll = usuarioBll;
            _orcamentistaEIndicadorBLL = orcamentistaEIndicadorBLL;
            _mapper = mapper;
            _orcamentistaEIndicadorVendedorBll = orcamentistaEIndicadorVendedorBll;
        }

        [HttpGet]
        public async Task<ActionResult> Get() //IEnumerable<UsuarioResponseViewModel>
        {
            try
            {
                var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

                var request = new
                {
                    Usuario = LoggedUser.Apelido
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/Get/GET - Request => [{JsonSerializer.Serialize(request)}].");


                var usuarios = _usuarioBll.PorFiltro(new InfraBanco.Modelos.Filtros.TusuarioFiltro() { Page = 1, RecordsPerPage = 1 });//GetAll(1, 1);
                var retorno = _mapper.Map<List<UsuarioResponseViewModel>>(usuarios);

                _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/Get/GET - Response => [{JsonSerializer.Serialize(retorno)}].");

                return Ok(JsonSerializer.Serialize(new { data = retorno }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("vendedores")]
        public async Task<IEnumerable<UsuarioResponseViewModel>> BuscarVendedores(string loja)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Loja = loja
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/BuscarVendedores/GET - Request => [{JsonSerializer.Serialize(request)}].");

            string vendedorId = User.Identity.Name;

            var usuarios = await _usuarioBll.FiltrarPorPerfil(loja);

            var response = _mapper.Map<List<UsuarioResponseViewModel>>(usuarios);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/BuscarVendedores/GET - Response => [{JsonSerializer.Serialize(response.Count)}].");

            return response;
        }


        //[HttpGet]
        //[Route("vendedores-parceiros")]
        //public async Task<IEnumerable<UsuarioResponseViewModel>> BuscarVendedoresDosParceiros(string vendedorId, string parceiroId)
        //{
        //    _logger.LogInformation("Buscando lista de vendedores parceiros");
        //    var usuarios = await _usuarioService.GetVendedoresDoParceiro(vendedorId, parceiroId, 1, 1);

        //    return usuarios;
        //}

        [HttpGet]
        [Route("{id}")]
        public async Task<UsuarioResponseViewModel> Get(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id= id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/Get/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var usuario = _usuarioBll.GetById(id);
            var response = _mapper.Map<UsuarioResponseViewModel>(usuario);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/Get/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return response;
        }

        [HttpPost]
        public async Task<UsuarioResponseViewModel> Post(UsuarioRequestViewModel model)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                UsuarioRequestViewModel = model
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/Post/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var objUsuario = _mapper.Map<Tusuario>(model);
            var usuario = _usuarioBll.Inserir(objUsuario);// model, User.Identity.Name);
            var response = _mapper.Map<UsuarioResponseViewModel>(usuario);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/Post/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return response;
        }

        [HttpPut]
        public async Task<UsuarioResponseViewModel> Put(UsuarioRequestViewModel model)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                UsuarioRequestViewModel = model
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/Put/PUT - Request => [{JsonSerializer.Serialize(request)}].");

            var objUsuario = _mapper.Map<Tusuario>(model);
            var usuario = _usuarioBll.Atualizar(objUsuario);//model, User.Identity.Name);
            var response = _mapper.Map<UsuarioResponseViewModel>(usuario);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. UsuarioController/Put/PUT - Response => [{JsonSerializer.Serialize(response)}].");

            return response;
        }
    }
}