using Microsoft.Extensions.DependencyInjection;
using OrcamentoCotacaoBusiness.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Utils
{
    public static class ExtensionMapper
    {
        public static void ApplicationMappersIoC(this IServiceCollection services, Type type)
        {
            services.AddAutoMapper(x =>
            {
                x.AddProfile(new UsuarioMapper());
                x.AddProfile(new OrcamentistaIndicadorMapper());
                x.AddProfile(new OrcamentistaEIndicadorVendedorMapper());
                x.AddProfile(new LojaMapper());
                x.AddProfile(new OrcamentoCotacaoOpcaoMapper());
                x.AddProfile(new ProdutoMapper());
                x.AddProfile(new FormaPagtoMapper());
            }, type);
        }
    }
}
