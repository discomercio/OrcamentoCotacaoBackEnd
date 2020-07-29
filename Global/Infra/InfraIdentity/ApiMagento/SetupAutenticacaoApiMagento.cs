using Microsoft.Extensions.DependencyInjection;

namespace InfraIdentity.ApiMagento
{
    public class SetupAutenticacaoApiMagento
    {
        public void ConfigurarTokenApiMagento(IServiceCollection services)
        {
            /*
             * 
             * nota: quem usar deve fazer o AddJwtBearer
             * 
             * */

            // configure DI for application services
            services.AddScoped<InfraIdentity.ApiMagento.IServicoAutenticacaoApiMagento, InfraIdentity.ApiMagento.ServicoAutenticacaoApiMagento>();
        }
    }
}
