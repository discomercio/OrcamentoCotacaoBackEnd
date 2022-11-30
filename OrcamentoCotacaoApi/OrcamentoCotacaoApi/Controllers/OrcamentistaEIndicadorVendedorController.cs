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
using static OrcamentoCotacaoBusiness.Enums.Enums;

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

            var torcamentista = orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = parceiro});
            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = torcamentista.IdIndicador});

            
            if (usuarios != null) usuarios = usuarios.OrderBy(x => x.Nome).ToList();

            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);
        }

        [HttpGet]
        [Route("vendedores-parceiros-apelido-loja")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresDosParceirosPorApelidoELoja(string apelido, string loja)
        {
            _logger.LogInformation("Buscando lista de vendedores parceiros");

            var torcamentista = orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = apelido, loja = loja });
            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = torcamentista.IdIndicador, loja = loja });


            if (usuarios != null) usuarios = usuarios.OrderBy(x => x.Nome).ToList();

            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);
        }


        [HttpGet]
        [Route("vendedores-parceiros-vendedor-loja")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresPorVendedorELoja(string vendedor, string loja)
        {
            _logger.LogInformation("Buscando lista de vendedores parceiros");
            
            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { nomeVendedor = vendedor, loja = loja });


            if (usuarios != null) usuarios = usuarios.OrderBy(x => x.Nome).ToList();

            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);
        }


        [HttpGet]
        [Route("vendedores-parceiros-loja")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresDosParceirosPorLoja(string loja)
        {
            _logger.LogInformation("Buscando lista de vendedores parceiros por loja");
                      
            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { loja = loja });

            if (usuarios != null) usuarios = usuarios.OrderBy(x => x.Nome).ToList();

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
                Parceiro = usuarios.Parceiro,
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
        public IActionResult Post(UsuarioRequestViewModel model)
        {
            _logger.LogInformation("Inserindo vendedor parceiro");

            if (!User.ValidaPermissao((int)ePermissao.CadastroVendedorParceiroIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
            try
            {
                var result = _orcamentistaEindicadorVendedorBll.Inserir(objOrcamentistaEIndicadorVendedor, model.Senha, User.GetParceiro(), User.GetVendedor());

                return Ok(_mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(result));
            }
            catch (ArgumentException e)
            {
                return UnprocessableEntity(e);
            }
        }

        [HttpPost]
        [Route("vendedores-parceiros-usuario-interno")]
        public IActionResult PostPorUsuarioInterno(UsuarioRequestViewModel model)
        {
            _logger.LogInformation("Inserindo vendedor parceiro");

            if (!User.ValidaPermissao((int)ePermissao.CadastroVendedorParceiroIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
            try
            {
                var result = _orcamentistaEindicadorVendedorBll.Inserir(objOrcamentistaEIndicadorVendedor, model.Senha, model.Parceiro, User.GetVendedor());

                return Ok(_mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(result));
            }
            catch (ArgumentException e)
            {
                return UnprocessableEntity(e);
            }
        }

        [HttpPut]
        [Route("vendedores-parceiros")]
        public IActionResult Put(UsuarioRequestViewModel model)
        {
            _logger.LogInformation("Altera vendedor parceiro");
            if (!User.ValidaPermissao((int)ePermissao.CadastroVendedorParceiroIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            try
            {
                var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
                var result = _orcamentistaEindicadorVendedorBll.Atualizar(
                    objOrcamentistaEIndicadorVendedor, 
                    model.Senha, 
                    model.Parceiro, 
                    User.GetVendedor(),
                    User.GetTipoUsuario(),
                    User.ValidaPermissao((int)ePermissao.SelecionarQualquerIndicadorDaLoja)
                    );
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return UnprocessableEntity(e);
            }
        }
    }
}