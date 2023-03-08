using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace OrcamentoCotacaonet7Api.Config
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerX(this IServiceCollection services)
        {

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Orçamento Cotação",
                    Version = "v1",
                    Description = "API para integração entre o front-end e os dados de Orçamento Cotação."
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement()
                                {
                                    {
                                        new OpenApiSecurityScheme
                                        {
                                            Reference = new OpenApiReference
                                            {
                                                Type = ReferenceType.SecurityScheme,
                                                Id = "Bearer"
                                            },
                                            Scheme = "oauth2",
                                            Name = "Bearer",
                                            In = ParameterLocation.Header,

                                        },
                                        new List<string>()
                                    }
                                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerX(this IApplicationBuilder builder, IConfiguration Configuration)
        {
            builder.UseSwagger();

            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orçamento Cotação V1");
                c.RoutePrefix = string.Empty;
            });

            return builder;
        }
    }
}
