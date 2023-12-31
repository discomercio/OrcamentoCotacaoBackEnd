﻿using Microsoft.Extensions.DependencyInjection;

namespace InfraIdentity.ApiUnis
{
    public class SetupAutenticacaoApiUnis
    {
        public void ConfigurarTokenApiUnis(IServiceCollection services)
        {
            /*
             * 
             * nota: quem usar deve fazer o AddJwtBearer
             * 
             * */

            // configure DI for application services
            services.AddScoped<InfraIdentity.ApiUnis.IServicoAutenticacaoApiUnis, InfraIdentity.ApiUnis.ServicoAutenticacaoApiUnis>();
        }
    }
}
