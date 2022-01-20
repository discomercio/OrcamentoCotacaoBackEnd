using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orcamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrcamentoOpcaoController : ControllerBase
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly IMapper _mapper;
        private readonly OrcamentoOpcaoBll _orcamentoOpcaoBll;

        public OrcamentoOpcaoController(ILogger<OrcamentoController> logger, IMapper mapper, OrcamentoOpcaoBll OrcamentoOpcaoBll)
        {
            _logger = logger;
            _mapper = mapper;
            _orcamentoOpcaoBll = OrcamentoOpcaoBll;
        }

    }
}
