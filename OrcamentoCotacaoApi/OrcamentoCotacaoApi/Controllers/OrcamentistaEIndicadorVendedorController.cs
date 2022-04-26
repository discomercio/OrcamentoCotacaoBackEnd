using AutoMapper;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
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
        private readonly OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll;
        private readonly ILogger<OrcamentistaEIndicadorVendedorController> _logger;
        private readonly IMapper _mapper;
        private readonly OrcamentistaEIndicadorBll orcamentistaEIndicadorBll;

        public OrcamentistaEIndicadorVendedorController(
            OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll orcamentistaEindicadorVendedorBll,
            ILogger<OrcamentistaEIndicadorVendedorController> logger, IMapper mapper,
            OrcamentistaEIndicadorBll orcamentistaEIndicadorBll)
        {
            this._orcamentistaEindicadorVendedorBll = orcamentistaEindicadorVendedorBll;
            this._logger = logger;
            this._mapper = mapper;
            this.orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
        }

        [HttpGet]
        [Route("vendedores-parceiros")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresDosParceiros(string parceiro)
        {
            _logger.LogInformation("Buscando lista de vendedores parceiros");
            var torcamentista = orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = parceiro, acessoHabilitado = 1 });
            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = torcamentista.IdIndicador });

            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);
        }

        [HttpGet]
        [Route("vendedores-parceiros/{id}")]
        public async Task<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresDosParceirosPorId(int id)
        {
            _logger.LogInformation("Buscando um vendedor parceiro");
            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { id = id }).FirstOrDefault();

            if (usuarios == null) return null;

            OrcamentistaEIndicadorVendedorResponseViewModel ret = new OrcamentistaEIndicadorVendedorResponseViewModel()
            {
                Nome = usuarios.Nome,
                Email = usuarios.Email,
                IdIndicador = usuarios.IdIndicador,
                Ativo = usuarios.Ativo,
                Id = usuarios.Id,
                Telefone = usuarios.Telefone,
                Celular = usuarios.Celular,
                VendedorResponsavel = usuarios.VendedorResponsavel,
                Senha = usuarios.Datastamp
            };

            return ret;
        }

        [HttpPost]
        [Route("vendedores-parceiros")]
        public async Task<ActionResult<OrcamentistaEIndicadorVendedorResponseViewModel>> Post(UsuarioRequestViewModel model)
        {
            _logger.LogInformation("Inserindo vendedor parceiro");
            if (User.GetTipoUsuario() != InfraBanco.Constantes.Constantes.TipoUsuario.PARCEIRO)
            {
                return Unauthorized("Somente usuários do tipo parceiro");
            }
            var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
            try
            {
                var result = _orcamentistaEindicadorVendedorBll.Inserir(objOrcamentistaEIndicadorVendedor, model.Senha, User.GetParceiro(), User.GetVendedor());

                return _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(result);
            }
            catch (ArgumentException e)
            {
                return UnprocessableEntity(e);
            }
        }

        [HttpPut]
        [Route("vendedores-parceiros")]
        public async Task<OrcamentistaEIndicadorVendedorResponseViewModel> Put(UsuarioRequestViewModel model)
        {
            _logger.LogInformation("Altera vendedor parceiro");
            if (User.GetTipoUsuario() != InfraBanco.Constantes.Constantes.TipoUsuario.PARCEIRO)
            {
                this.Unauthorized("Somente usuários do tipo parceiro");
            }
            var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
            var result = _orcamentistaEindicadorVendedorBll.Atualizar(objOrcamentistaEIndicadorVendedor, model.Senha, User.GetParceiro(), User.GetVendedor());
            return _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(result); ;
        }
    }
}
