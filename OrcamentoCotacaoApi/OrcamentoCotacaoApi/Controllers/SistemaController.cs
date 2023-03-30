using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using OrcamentoCotacaoApi.Utils;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Reflection;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.AspNetCore.Hosting;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class SistemaController : BaseController
    {
        private readonly ILogger<SistemaController> _logger;
        private readonly IHostingEnvironment _env;

        public SistemaController(ILogger<SistemaController> logger, IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;
        }


        [HttpGet("versao")]
        public async Task<IActionResult> RetornarVersao()
        {
            _logger.LogInformation("RetornarVersao");

            try
            {

                Process currentProcess = Process.GetCurrentProcess();

                string assemblyPath = string.Empty;

                FileVersionInfo versionInfo;

                if (_env.IsDevelopment())
                {
                    assemblyPath = "bin/Debug/net7.0/OrcamentoCotacaoApi.dll";
                }
                else
                {
                    assemblyPath = "OrcamentoCotacaoApi.dll";
                }
                    
                versionInfo = FileVersionInfo.GetVersionInfo(assemblyPath);               
                
                return new ContentResult 
                { 
                    Content = versionInfo.FileVersion.ToString(), 
                    ContentType= "text/plain",
                    StatusCode= 200
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}