using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;
using NuGet.Protocol;
using System;
using System.Net;
using System.Threading.Tasks;
using UtilsGlobais.Configs;
using UtilsGlobais.Exceptions;

namespace OrcamentoCotacaoApi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async void OnException(ExceptionContext context)
        {
            var headers = context.HttpContext.Request.Headers;
            var correlationId = headers[HttpHeader.CorrelationIdHeader];
            var correlationIdParsed = Guid.TryParse(correlationId, out var guid) ? guid : Guid.NewGuid();

            var exception = context.Exception;

            var response = new MensagemExceptionResponse();
            response.Sucesso = false;

            if (exception is DomainException)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                _logger.LogInformation($"CorrelationId => [{correlationIdParsed}] / EXCEPTION: [{exception}] / INNEREXCEPTION: [{exception?.InnerException}].");
            }
            else
            {
                if (context.HttpContext.Request.Method == "POST" || context.HttpContext.Request.Method == "PUT")
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    object param = await Task.FromResult(context.Exception.Data.Values.ToJson());
                    if (param.ToString().Length > 2)
                    {
                        _logger.LogError($"CorrelationId => [{correlationIdParsed}] / PARAMETROS: {param} / EXCEPTION: [{exception}] / INNEREXCEPTION: [{exception?.InnerException}].");
                    }
                    else
                    {
                        _logger.LogError($"CorrelationId => [{correlationIdParsed}] / EXCEPTION: [{exception}] / INNEREXCEPTION: [{exception?.InnerException}].");
                    }

                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError($"CorrelationId => [{correlationIdParsed}] / EXCEPTION: [{exception}] / INNEREXCEPTION: [{exception?.InnerException}].");
                }

            }

            response.Mensagem = $"Erro inesperado! Favor entrar em contato com o suporte técnico. (Código: [{response.StatusCode}]).";

            context.ExceptionHandled = true;
            context.Result = new ObjectResult(response);
            context.HttpContext.Response.StatusCode = response.StatusCode;
        }
    }
}