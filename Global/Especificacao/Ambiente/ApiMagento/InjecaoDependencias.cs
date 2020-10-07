using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento
{
    internal static class InjecaoDependencias
    {
        private static string? tokenAcessoApiMagent_cache = null;
        public static string TokenAcessoApiMagento()
        {
            if (tokenAcessoApiMagent_cache == null)
            {
                //geramos um token de acesso válido
                var acessoMagentoBll = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<MagentoBusiness.MagentoBll.AcessoBll.AcessoMagentoBll>();
                var loginResultadoUnisDto = acessoMagentoBll.FazerLogin(new MagentoBusiness.MagentoDto.LoginMagentoDto()
                {
                    Usuario = "UsuarioApiMagento",
                    Senha = "123456"
                }, "local", "testes").Result;
                Assert.Empty(loginResultadoUnisDto.ListaErros);
                tokenAcessoApiMagent_cache = loginResultadoUnisDto.TokenAcesso;
            }
            return tokenAcessoApiMagent_cache;
        }

        public static void ConfigurarDependencias(IServiceCollection services)
        {
            global::ApiMagento.Startup.ConfigurarServicosComuns(services);
            services.AddTransient<global::ApiMagento.Controllers.AcessoMagentoController, global::ApiMagento.Controllers.AcessoMagentoController>();
            services.AddTransient<global::ApiMagento.Controllers.PedidoMagentoController, global::ApiMagento.Controllers.PedidoMagentoController>();

            services.AddTransient<Cep.IBancoNFeMunicipio, Testes.Utils.BancoTestes.TestesBancoNFeMunicipio>();
            services.AddSingleton<MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento>(c =>
            {
                var ret = new MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento
                {
                    SegredoToken = "appSettings.SegredoToken",
                    ValidadeTokenMinutos = 2628000,
                    ApelidoPerfilLiberaAcessoApiMagento = "USUARIOAPIMAGENTO"
                };

                //para nao dar erro...
                ret.LimitePedidos.LimitePrepedidosExatamenteIguais_Numero = 1000;
                return ret;
            });

            var key = Encoding.ASCII.GetBytes("appSettings.SegredoToken");

            //isto deveria ser passado para o SetupAutenticacao
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;

                x.SecurityTokenValidators.Clear();

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            new InfraIdentity.ApiMagento.SetupAutenticacaoApiMagento().ConfigurarTokenApiMagento(services);
        }
    }
}
