using AutoMapper;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
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
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Parceiro = parceiro
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceiros/GET - Request => [{JsonSerializer.Serialize(request)}].");
            var torcamentista = orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = parceiro, status = Constantes.ORCAMENTISTA_INDICADOR_STATUS_ATIVO });

            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = torcamentista.IdIndicador, ativo = true });

            if (usuarios != null) usuarios = usuarios.OrderBy(x => x.Nome).ToList();


            var result = _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);

            var response = new
            {
                OrcamentistaEIndicadorVendedor = result.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceiros/GET - Response => [{JsonSerializer.Serialize(response)}].");
            
            if (usuarios != null) usuarios = usuarios.OrderBy(x => x.Nome).ToList();

            return result;
        }

        [HttpGet]
        [Route("vendedores-parceiros-apelido-loja")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresDosParceirosPorApelidoELoja(string apelido, string loja)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Apelido = apelido,
                Loja = loja
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceirosPorApelidoELoja/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var torcamentista = orcamentistaEIndicadorBll.BuscarParceiroPorApelido(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = apelido, loja = loja });
            if (torcamentista == null) throw new ArgumentException("Parceiro não encontrado!");

            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { IdIndicador = torcamentista.IdIndicador, loja = loja });

            if (usuarios != null) usuarios = usuarios.OrderBy(x => x.Nome).ToList();

            var result = _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);

            var response = new
            {
                OrcamentistaEIndicadorVendedor = result.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceiros/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return result;
        }

        [HttpGet]
        [Route("vendedores-parceiros-vendedor-loja")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresPorVendedorELoja(string vendedor, string loja)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Vendedor = vendedor,
                Loja = loja
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresPorVendedorELoja/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { nomeVendedor = vendedor, loja = loja });

            if (usuarios != null) usuarios = usuarios.OrderBy(x => x.Nome).ToList();

            var result = _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);

            var response = new
            {
                OrcamentistaEIndicadorVendedor = result.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresPorVendedorELoja/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return result;
        }

        [HttpGet]
        [Route("vendedores-parceiros-loja")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresDosParceirosPorLoja(string loja)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Loja = loja
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceirosPorLoja/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { loja = loja });

            if (usuarios != null) usuarios = usuarios.OrderBy(x => x.Nome).ToList();

            var result = _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);

            var response = new
            {
                OrcamentistaEIndicadorVendedor = result.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceirosPorLoja/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return result;
        }

        [HttpGet]
        [Route("vendedores-parceiros/{id}")]
        public async Task<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresDosParceirosPorId(int id)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Id = id
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceirosPorId/GET - Request => [{JsonSerializer.Serialize(request)}].");

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

            var response = new
            {
                OrcamentistaEIndicadorVendedor = ret
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceirosPorId/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return ret;
        }

        [HttpPost]
        [Route("vendedores-parceiros")]
        public IActionResult Post(UsuarioRequestViewModel model)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                UsuarioRequest = model
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/vendedores-parceiros/POST - Request => [{JsonSerializer.Serialize(request)}].");

            if (!User.ValidaPermissao((int)ePermissao.CadastroVendedorParceiroIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
            try
            {
                var result = _orcamentistaEindicadorVendedorBll.Inserir(objOrcamentistaEIndicadorVendedor, model.Senha, User.GetParceiro(), User.GetVendedor());

                var ret = _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(result);

                var response = new
                {
                    OrcamentistaEIndicadorVendedor = ret
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/vendedores-parceiros/POST - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(ret);
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
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                UsuarioRequest = model
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/vendedores-parceiros-usuario-interno/POST - Request => [{JsonSerializer.Serialize(request)}].");

            if (!User.ValidaPermissao((int)ePermissao.CadastroVendedorParceiroIncluirEditar))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
            try
            {
                var result = _orcamentistaEindicadorVendedorBll.Inserir(objOrcamentistaEIndicadorVendedor, model.Senha, model.Parceiro, User.GetVendedor());

                var ret = _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(result);

                var response = new
                {
                    OrcamentistaEIndicadorVendedor = ret
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/vendedores-parceiros-usuario-interno/POST - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(ret);
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
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                UsuarioRequest = model
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/vendedores-parceiros/PUT - Request => [{JsonSerializer.Serialize(request)}].");

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

                var response = new
                {
                    OrcamentistaEIndicadorVendedor = result
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/vendedores-parceiros/PUT - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return UnprocessableEntity(e);
            }
        }
    }
}