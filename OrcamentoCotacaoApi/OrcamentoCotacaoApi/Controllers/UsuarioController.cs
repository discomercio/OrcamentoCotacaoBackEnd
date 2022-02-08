using AutoMapper;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentistaEindicador;
using OrcamentistaEIndicadorVendedor;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Usuario;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsuarioController : BaseController
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly UsuarioBll _usuarioBll;
        private readonly OrcamentistaEindicadorBll _orcamentistaEIndicadorBLL;
        private readonly IMapper _mapper;
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEIndicadorVendedorBll;

        public UsuarioController(ILogger<UsuarioController> logger, UsuarioBll usuarioBll,
            OrcamentistaEindicadorBll orcamentistaEIndicadorBLL, IMapper mapper,
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
                _logger.LogInformation("Buscando lista de usuários");
                var usuarios = _usuarioBll.PorFiltro(new InfraBanco.Modelos.Filtros.TusuarioFiltro() { Page = 1, RecordsPerPage = 1 });//GetAll(1, 1);
                var retorno = _mapper.Map<List<UsuarioResponseViewModel>>(usuarios);

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
            string vendedorId = User.Identity.Name;
            _logger.LogInformation("Buscando lista de vendedores");

            var usuarios = await _usuarioBll.FiltrarPorPerfil(loja);

            return _mapper.Map<List<UsuarioResponseViewModel>>(usuarios);
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
        public async Task<UsuarioResponseViewModel> Get(string id)
        {
            _logger.LogInformation("Buscando lista de usuários");
            var usuario = _usuarioBll.GetById(id);
            return _mapper.Map<UsuarioResponseViewModel>(usuario);
        }

        [HttpPost]
        public async Task<UsuarioResponseViewModel> Post(UsuarioRequestViewModel model)
        {
            _logger.LogInformation("Inserindo usuário");
            var objUsuario = _mapper.Map<Tusuario>(model);
            var usuario = _usuarioBll.Inserir(objUsuario);// model, User.Identity.Name);
            return _mapper.Map<UsuarioResponseViewModel>(usuario);
        }

        [HttpPut]
        public async Task<UsuarioResponseViewModel> Put(UsuarioRequestViewModel model)
        {
            _logger.LogInformation("Alterando usuário");
            var objUsuario = _mapper.Map<Tusuario>(model);
            var usuario = _usuarioBll.Atualizar(objUsuario);//model, User.Identity.Name);
            return _mapper.Map<UsuarioResponseViewModel>(usuario);
        }
    }
}
