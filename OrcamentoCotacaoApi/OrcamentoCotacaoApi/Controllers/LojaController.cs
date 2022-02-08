using AutoMapper;
using Loja;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class LojaController : BaseController
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IMapper _mapper;
        private readonly LojaBll _lojaBll;

        public LojaController(ILogger<UsuarioController> logger, IMapper mapper, LojaBll loja)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._lojaBll = loja;
        }

        [HttpGet]
        [Route("buscarlojas")]
        public IEnumerable<LojaResponseViewModel> BuscarLojas()
        {
            _logger.LogInformation("Buscando lista de lojas");
            var lojas = _lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Page = 1, RecordsPerPage = int.MaxValue });

            return _mapper.Map<List<LojaResponseViewModel>>(lojas);
        }
    }
}
