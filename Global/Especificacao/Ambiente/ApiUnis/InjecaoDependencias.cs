using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis
{
    internal static class InjecaoDependencias
    {
        private static string? tokenAcessoApiUni_cache = null;
        public static string TokenAcessoApiUnis()
        {
            if (tokenAcessoApiUni_cache == null)
            {
                //geramos um token de acesso válido
                var acessoUnisBll = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<PrepedidoUnisBusiness.UnisBll.AcessoBll.AcessoUnisBll>();
                PrepedidoApiUnisBusiness.UnisDto.AcessoDto.LoginResultadoUnisDto loginResultadoUnisDto = acessoUnisBll.FazerLogin(new PrepedidoApiUnisBusiness.UnisDto.AcessoDto.LoginUnisDto()
                {
                    Usuario = "UsuarioApiUnis",
                    Senha = "123456"
                }, "local", "testes").Result;
                Assert.Empty(loginResultadoUnisDto.ListaErros);
                tokenAcessoApiUni_cache = loginResultadoUnisDto.TokenAcesso;
            }
            return tokenAcessoApiUni_cache;
        }

        public static void ConfigurarDependencias(IServiceCollection services)
        {
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll>();

            services.AddTransient<PrepedidoAPIUnis.Controllers.PrepedidoUnisController, PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
            services.AddTransient<PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll, PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll>();
            services.AddTransient<PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll, PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll.ProdutoUnisBll, PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll.ProdutoUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.AcessoBll.AcessoUnisBll, PrepedidoUnisBusiness.UnisBll.AcessoBll.AcessoUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll, PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll.FormaPagtoUnisBll, PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll.FormaPagtoUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.CoeficienteUnisBll.CoeficienteUnisBll, PrepedidoUnisBusiness.UnisBll.CoeficienteUnisBll.CoeficienteUnisBll>();

            //Bll's da Arclube
            services.AddTransient<PrepedidoBusiness.Bll.CepBll, PrepedidoBusiness.Bll.CepBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CepPrepedidoBll, PrepedidoBusiness.Bll.CepPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ClienteBll.ClienteBll, PrepedidoBusiness.Bll.ClienteBll.ClienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll, PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoBll.ValidacoesFormaPagtoBll, PrepedidoBusiness.Bll.FormaPagtoBll.ValidacoesFormaPagtoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CoeficienteBll, PrepedidoBusiness.Bll.CoeficienteBll>();
            services.AddTransient<Produto.ProdutoGeralBll, Produto.ProdutoGeralBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ProdutoPrepedidoBll, PrepedidoBusiness.Bll.ProdutoPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.ValidacoesPrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.ValidacoesPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll>();

            services.AddTransient<PrepedidoBusiness.UtilsNfe.IBancoNFeMunicipio, Testes.Utils.BancoTestes.TestesBancoNFeMunicipio>();

            services.AddTransient<InfraIdentity.ApiUnis.IServicoAutenticacaoApiUnis, InfraIdentity.ApiUnis.ServicoAutenticacaoApiUnis>();
            //como singleton para melhorar a performance
            services.AddSingleton<PrepedidoUnisBusiness.UnisBll.AcessoBll.IServicoValidarTokenApiUnis, PrepedidoUnisBusiness.UnisBll.AcessoBll.ServicoValidarTokenApiUnis>();

            services.AddSingleton<PrepedidoUnisBusiness.Utils.ConfiguracaoApiUnis>(c =>
            {
                var ret = new PrepedidoUnisBusiness.Utils.ConfiguracaoApiUnis
                {
                    SegredoToken = "appSettings.SegredoToken",
                    ValidadeTokenMinutos = 2628000,
                    ApelidoPerfilLiberaAcessoApiUnis = "APIUNIS"
                };

                //para nao dar erro...
                ret.LimitePrepedidos.LimitePrepedidosExatamenteIguais_Numero = 1000;
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

            new InfraIdentity.ApiUnis.SetupAutenticacaoApiUnis().ConfigurarTokenApiUnis(services);
        }
    }
}
