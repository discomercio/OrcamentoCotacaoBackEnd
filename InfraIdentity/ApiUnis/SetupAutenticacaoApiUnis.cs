using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
            services.AddScoped<InfraIdentity.ApiUnis.IServicoDecodificarTokenApiUnis, InfraIdentity.ApiUnis.ServicoDecodificarTokenApiUnis>();
        }
    }
}
