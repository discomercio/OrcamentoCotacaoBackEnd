using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using UtilsGlobais.Configs;
using UtilsGlobais.Exceptions;

namespace OrcamentoCotacaoApi.Filters
{
    public class ResourceFilter : IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;

            if (!headers.ContainsKey(HttpHeader.CorrelationIdHeader))
            {
                var response = new MensagemExceptionResponse()
                {
                    Sucesso = false,
                    Mensagem = "Não foi encontrado Header => [CorrelationId]."
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}