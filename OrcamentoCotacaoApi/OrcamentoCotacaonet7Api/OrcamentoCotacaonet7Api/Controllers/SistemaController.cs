using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using OrcamentoCotacaonet7Api.Utils;
using System.Text.Json;
using Microsoft.AspNetCore.Http;


namespace OrcamentoCotacaonet7Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SistemaController : BaseController
    {
        private readonly ILogger<SistemaController> _logger;

        public SistemaController(ILogger<SistemaController> logger)
        {
            _logger = logger;
        }

        [HttpGet("cache")]
        public async Task<IActionResult> RetornarVersaoCache()
        {
            _logger.LogInformation("RetornarVersaoCache");

            try
            {               
                var versaoApi = JsonSerializer.Serialize(HttpContext.Session.GetString("versaoApi"));

                return Ok(versaoApi);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("versao")]
        public async Task<IActionResult> RetornarVersao()
        {
            _logger.LogInformation("RetornarVersao");

            try
            {
                var versaoApi = JsonSerializer.Serialize(System.IO.File.ReadAllText("version.txt"));

                return Ok(versaoApi);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}