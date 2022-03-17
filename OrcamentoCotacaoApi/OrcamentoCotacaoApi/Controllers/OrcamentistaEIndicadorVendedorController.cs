using AutoMapper;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentistaEIndicadorVendedor;
using OrcamentoCotacaoApi.Utils;
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
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll;
        private readonly ILogger<OrcamentistaEIndicadorVendedorController> _logger;
        private readonly IMapper _mapper;

        public OrcamentistaEIndicadorVendedorController(OrcamentistaEIndicadorVendedorBll orcamentistaEindicadorVendedorBll, ILogger<OrcamentistaEIndicadorVendedorController> logger,
            IMapper mapper)
        {
            this._orcamentistaEindicadorVendedorBll = orcamentistaEindicadorVendedorBll;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpGet]
        [Route("vendedores-parceiros")]
        public async Task<IEnumerable<OrcamentistaEIndicadorVendedorResponseViewModel>> BuscarVendedoresDosParceiros(string parceiro)
        {
                _logger.LogInformation("Buscando lista de vendedores parceiros");
            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { nomeVendedor = parceiro });

            return _mapper.Map<List<OrcamentistaEIndicadorVendedorResponseViewModel>>(usuarios);
        }


        [HttpGet]
        [Route("vendedores-parceiros/{id}")]
        public async Task<OrcamentistaEIndicadorVendedorResponseViewModel> BuscarVendedoresDosParceirosPorId(int id)
        {
            _logger.LogInformation("Buscando um vendedor parceiro");
            var usuarios = _orcamentistaEindicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { id = id });

            return _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(usuarios.FirstOrDefault());
        }

        [HttpPost]
        [Route("vendedores-parceiros")]
        public async Task<OrcamentistaEIndicadorVendedorResponseViewModel> Post(UsuarioRequestViewModel model)
        {

            _logger.LogInformation("Inserindo vendedor parceiro");
            var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
            var result = _orcamentistaEindicadorVendedorBll.Inserir(objOrcamentistaEIndicadorVendedor, model.Parceiro, User.GetVendendor());
            return _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(result); ;
        }
        [HttpPut]
        [Route("vendedores-parceiros")]
        public async Task<OrcamentistaEIndicadorVendedorResponseViewModel> Put(UsuarioRequestViewModel model)
        {
            _logger.LogInformation("Altera vendedor parceiro");
            var objOrcamentistaEIndicadorVendedor = _mapper.Map<TorcamentistaEIndicadorVendedor>(model);
            var result = _orcamentistaEindicadorVendedorBll.Atualizar(objOrcamentistaEIndicadorVendedor, model.Parceiro, User.GetVendendor());
            return _mapper.Map<OrcamentistaEIndicadorVendedorResponseViewModel>(result); ;
        }
    }
}
