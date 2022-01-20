using AutoMapper;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentistaEindicador;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuario;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly UsuarioBll _usuarioBll;
        private readonly OrcamentistaEindicadorBll _orcamentistaEIndicadorBLL;
        private readonly IMapper _mapper;

        public UsuarioController(ILogger<UsuarioController> logger, UsuarioBll usuarioBll,
            OrcamentistaEindicadorBll orcamentistaEIndicadorBLL, IMapper mapper)
        {
            _logger = logger;
            _usuarioBll = usuarioBll;
            orcamentistaEIndicadorBLL = _orcamentistaEIndicadorBLL;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UsuarioResponseViewModel>> Get()
        {
            _logger.LogInformation("Buscando lista de usuários");
            var usuarios = _usuarioBll.PorFiltro(new InfraBanco.Modelos.Filtros.TusuarioFiltro() { Page = 1, RecordsPerPage = 1 });//GetAll(1, 1);
            return _mapper.Map<List<UsuarioResponseViewModel>>(usuarios);
        }

        [HttpGet]
        [Route("parceiros")]
        public async Task<IEnumerable<UsuarioResponseViewModel>> BuscarParceiros(string vendedorId)
        {
            _logger.LogInformation("Buscando lista de parceiros");
            var usuarios = await _orcamentistaEIndicadorBLL.GetParceiros(vendedorId, 1, 1);

            return _mapper.Map<List<UsuarioResponseViewModel>>(usuarios);
        }

        [HttpGet]
        [Route("parceiros-por-vendedor")]
        public async Task<IEnumerable<UsuarioResponseViewModel>> BuscarParceirosByVendedor(string vendedor)
        {
            _logger.LogInformation("Buscando lista de parceiros por vendedor");
            var usuarios = await _orcamentistaEIndicadorBLL.GetParceirosByVendedor(vendedor);

            return _mapper.Map<List<UsuarioResponseViewModel>>(usuarios);
        }

        [HttpGet]
        [Route("vendedores")]
        public async Task<IEnumerable<UsuarioResponseViewModel>> BuscarVendedores()
        {
            string vendedorId = User.Identity.Name;
            _logger.LogInformation("Buscando lista de vendedores");
            var usuarios = _usuarioBll.GetVendedores(vendedorId, 1, 1);

            return _mapper.Map<List<UsuarioResponseViewModel>>(usuarios);
        }

        [HttpGet]
        [Route("vendedores-parceiros")]
        public async Task<IEnumerable<UsuarioResponseViewModel>> BuscarVendedoresDosParceiros(string parceiro)
        {
            _logger.LogInformation("Buscando lista de vendedores parceiros");
            var usuarios = _orcamentistaEIndicadorBLL.GetVendedoresDoParceiro(parceiro);

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
