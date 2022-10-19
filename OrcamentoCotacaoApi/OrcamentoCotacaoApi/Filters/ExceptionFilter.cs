using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace OrcamentoCotacaoApi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(
            ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            var exception = context.Exception;

            //var response = new MessageResponse();

            //if (exception is DomainException)
            //{
            //    response.Sucesso = false;
            //    response.Mensagem = exception.Message;
            //    response.StatusCode = 400;
            //}
            //else if (exception is DomainException)
            //{
            //    response.Sucesso = false;
            //    response.Mensagem = "Regras de negócio.";
            //    response.StatusCode = 404;
            //}
            //else
            //{
            //    response.Sucesso = false;
            //    response.Mensagem = exception.Message;
            //    response.StatusCode = 500;
            //}

            //_logger.LogDebug(exception.Message);
            //context.HttpContext.Response.StatusCode = response.StatusCode;
            //context.Result = new JsonResult(response) { StatusCode = response.StatusCode };

            context.HttpContext.Response.StatusCode = 500;
            context.Result = new ObjectResult(exception);
        }
    }
}