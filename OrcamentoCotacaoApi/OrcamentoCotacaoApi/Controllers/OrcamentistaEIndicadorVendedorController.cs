﻿using AutoMapper;
using Azure;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.OrcamentistaIndicadorVendedor;
using OrcamentoCotacaoBusiness.Models.Request.Usuario;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.OrcamentistaIndicadorVendedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaoApi.Controllers
{
    [TypeFilter(typeof(ControleDelayFilter))]
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
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEIndicadorVendedorBll;

        public OrcamentistaEIndicadorVendedorController(
            OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll orcamentistaEindicadorVendedorBll,
            ILogger<OrcamentistaEIndicadorVendedorController> logger, IMapper mapper,
            OrcamentistaEIndicadorBll orcamentistaEIndicadorBll, 
            OrcamentistaEIndicadorVendedorBll _orcamentistaEIndicadorVendedorBll)
        {
            this._orcamentistaEindicadorVendedorBll = orcamentistaEindicadorVendedorBll;
            this._logger = logger;
            this._mapper = mapper;
            this.orcamentistaEIndicadorBll = orcamentistaEIndicadorBll;
            this._orcamentistaEIndicadorVendedorBll = _orcamentistaEIndicadorVendedorBll;
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
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresDosParceirosPorApelidoELoja(string? apelido, string? loja)
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

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceirosPorApelidoELoja/GET - Response => [{JsonSerializer.Serialize(response)}].");

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

            var ret = new OrcamentistaEIndicadorVendedorResponseViewModel()
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
                Senha = usuarios.Datastamp,
                StLoginBloqueadoAutomatico = usuarios.StLoginBloqueadoAutomatico
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

            try
            {
                var usuarioLogado = LoggedUser;
                string ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var ret = _orcamentistaEIndicadorVendedorBll.Inserir(model, usuarioLogado, ip);

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
                //Novo
                var usuarioLogado = LoggedUser;
                string ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var selecionaQualquerIndicadorLoja = User.ValidaPermissao((int)ePermissao.SelecionarQualquerIndicadorDaLoja);
                var ret = _orcamentistaEIndicadorVendedorBll.Atualizar(model, usuarioLogado, ip, selecionaQualquerIndicadorLoja, User.GetVendedor());

                var response = new
                {
                    OrcamentistaEIndicadorVendedor = ret
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/vendedores-parceiros/PUT - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(ret);
                //Fim novo

                //var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
                //var result = _orcamentistaEindicadorVendedorBll.Atualizar(
                //    objOrcamentistaEIndicadorVendedor,
                //    model.Senha,
                //    model.Parceiro,
                //    User.GetVendedor(),
                //    User.GetTipoUsuario(),
                //    User.ValidaPermissao((int)ePermissao.SelecionarQualquerIndicadorDaLoja)
                //    );

                //var responsea = new
                //{
                //    OrcamentistaEIndicadorVendedor = result
                //};

                //_logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/vendedores-parceiros/PUT - Response => [{JsonSerializer.Serialize(response)}].");

                //return Ok(result);
            }
            catch (ArgumentException e)
            {
                return UnprocessableEntity(e);
            }
        }

        [HttpPost("delete")]
        public IActionResult Delete(OrcamentistaIndicadorVendedorDeleteRequest request)
        {
            try
            {
                request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
                request.Usuario = LoggedUser.Apelido;
                request.IP = HttpContext.Connection.RemoteIpAddress.ToString();

                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. OrcamentistaEIndicadorVendedorController/Delete - Request => [{JsonSerializer.Serialize(request)}].");

                var response = new OrcamentistaIndicadorVendedorDeleteResponse();

                if (!User.ValidaPermissao((int)ePermissao.CadastroVendedorParceiroIncluirEditar))
                {
                    response.Mensagem = "Não encontramos a permissão necessária para realizar atividade!";
                    response.Sucesso = false;
                    return Ok(response);
                }
                
                response = _orcamentistaEIndicadorVendedorBll.Deletar(request);

                return Ok(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("vendedores-parceiros-por-parceiros")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresDosParceirosPorParceiros(string[] parceiros)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Parceiros = parceiros
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceirosPorParceiros/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var result = new List<OrcamentistaEIndicadorVendedorResponseViewModel>();

            var usuarios = _orcamentistaEindicadorVendedorBll.BuscarVendedorParceirosPorParceiros(
                new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro()
                {
                    Parceiros = parceiros
                });

            result.AddRange(_mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios));

            var response = new
            {
                OrcamentistaEIndicadorVendedor = result.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OrcamentistaEIndicadorVendedorController/BuscarVendedoresDosParceirosPorParceiros/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return result;
        }

        [HttpPost]
        [Route("listar-orcamentista-vendedor")]
        public IActionResult ListarOrcamentistaVendedor(UsuariosRequest request)
        {
            try
            {
                request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
                request.Usuario = LoggedUser.Apelido;

                var response = new ListaOrcamentistaVendedorResponse();

                if (request.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                {
                    //verificar permissão do usuário
                    if (string.IsNullOrEmpty(request.Vendedor) && !User.ValidaPermissao((int)ePermissao.SelecionarQualquerIndicadorDaLoja))
                    {
                        response.Sucesso = false;
                        response.Mensagem = "Não encontramos a permissão necessária para realizar atividade!";
                        return Ok(response);
                    }
                }

                _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. OrcamentistaEIndicadorVendedorController/ListarOrcamentistaVendedor/POST - Request => [{JsonSerializer.Serialize(request)}].");


                response = _orcamentistaEIndicadorVendedorBll.ListarOrcamentistaVendedor(request);

                return Ok(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}