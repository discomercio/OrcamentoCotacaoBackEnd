using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace InfraIdentity
{
    public class SetupAutenticacao
    {
        public void ConfigurarToken(IServiceCollection services)
        {
            /*
             * 
             * nota: quem usar deve fazer o AddJwtBearer
             * 
             * */

            // configure DI for application services
            services.AddScoped<InfraIdentity.IServicoAutenticacao, InfraIdentity.ServicoAutenticacao>();
        }
    }
}
