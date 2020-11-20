using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.Loja.Loja_Bll
{
    internal static class InjecaoDependencias
    {
        public static void ConfigurarDependencias(IServiceCollection services)
        {
            services.AddTransient<global::Loja.Bll.PedidoBll.PedidoBll, global::Loja.Bll.PedidoBll.PedidoBll>();
            services.AddTransient<global::Loja.Bll.ProdutoBll.ProdutoBll, global::Loja.Bll.ProdutoBll.ProdutoBll>();
        }
    }
}

