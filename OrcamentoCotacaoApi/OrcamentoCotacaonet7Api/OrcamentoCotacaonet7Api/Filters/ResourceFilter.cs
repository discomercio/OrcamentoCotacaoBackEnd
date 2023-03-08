using Microsoft.AspNetCore.Mvc.Filters;
using System;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaonet7Api.Filters
{
    public class ResourceFilter : IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;

            if (!headers.ContainsKey(HttpHeader.CorrelationIdHeader))
                headers.Add(HttpHeader.CorrelationIdHeader, Guid.NewGuid().ToString());
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}