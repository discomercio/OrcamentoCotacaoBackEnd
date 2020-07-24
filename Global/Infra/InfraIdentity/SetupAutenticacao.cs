using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<InfraIdentity.IServicoDecodificarToken, InfraIdentity.ServicoDecodificarToken>();
        }
    }
}
