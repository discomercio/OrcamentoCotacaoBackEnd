using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
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

        public void OnException(ExceptionContext context)
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
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            response.Mensagem = $"Erro inesperado! Favor entrar em contato com o suporte técnico. (Código: [{response.StatusCode}]).";

            _logger.LogInformation($"EXCEPTION: {exception} / INNEREXCEPTION: {exception?.InnerException} / CorrelationId => [{correlationIdParsed}].");

            context.ExceptionHandled = true;
            context.Result = new ObjectResult(response);
            context.HttpContext.Response.StatusCode = response.StatusCode;
        }
    }
}