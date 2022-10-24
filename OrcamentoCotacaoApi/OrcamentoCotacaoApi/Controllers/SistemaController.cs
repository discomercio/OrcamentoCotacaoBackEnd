using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using System;
using System.IO;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using OrcamentoCotacaoApi.Utils;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SistemaController : BaseController
    {
        private readonly ILogger<SistemaController> _logger;
        private readonly IOptions<Configuracoes> _appSettings;

        public SistemaController(ILogger<SistemaController> logger, IOptions<Configuracoes> appSettings)
        {
            _appSettings = appSettings;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("versao")]
        public async Task<IActionResult> RetornarVersao()
        {
            try
            {
                string versaoApi = _appSettings.Value.VersaoApi;
                
                return Ok(versaoApi);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}