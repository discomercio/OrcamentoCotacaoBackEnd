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
                    Usuario = "USRAPUN",
                    Senha = "123456"
                }, "local", "testes").Result;
                Assert.Empty(loginResultadoUnisDto.ListaErros);
                tokenAcessoApiUni_cache = loginResultadoUnisDto.TokenAcesso;
            }
            return tokenAcessoApiUni_cache;
        }

        public static void ConfigurarDependencias(IServiceCollection services)
        {
            services.AddTransient<Prepedido.PrepedidoBll, Prepedido.PrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoApiBll, PrepedidoBusiness.Bll.PrepedidoApiBll>();

            services.AddTransient<PrepedidoAPIUnis.Controllers.PrepedidoUnisController, PrepedidoAPIUnis.Controllers.PrepedidoUnisController>();
            services.AddTransient<PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll, PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll>();
            services.AddTransient<PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll, PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll.ProdutoUnisBll, PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll.ProdutoUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.AcessoBll.AcessoUnisBll, PrepedidoUnisBusiness.UnisBll.AcessoBll.AcessoUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll, PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll.FormaPagtoUnisBll, PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll.FormaPagtoUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.CoeficienteUnisBll.CoeficienteUnisBll, PrepedidoUnisBusiness.UnisBll.CoeficienteUnisBll.CoeficienteUnisBll>();

            //Bll's da Arclube
            services.AddTransient<Cep.CepBll, Cep.CepBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CepPrepedidoBll, PrepedidoBusiness.Bll.CepPrepedidoBll>();
            services.AddTransient<Cliente.ClienteBll, Cliente.ClienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ClientePrepedidoBll, PrepedidoBusiness.Bll.ClientePrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoPrepedidoBll, PrepedidoBusiness.Bll.FormaPagtoPrepedidoBll>();
            services.AddTransient<Prepedido.FormaPagto.FormaPagtoBll, Prepedido.FormaPagto.FormaPagtoBll>();
            services.AddTransient<Prepedido.FormaPagto.ValidacoesFormaPagtoBll, Prepedido.FormaPagto.ValidacoesFormaPagtoBll>();
            services.AddTransient<Produto.CoeficienteBll, Produto.CoeficienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CoeficientePrepedidoBll, PrepedidoBusiness.Bll.CoeficientePrepedidoBll>();
            services.AddTransient<Produto.ProdutoGeralBll, Produto.ProdutoGeralBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ProdutoPrepedidoBll, PrepedidoBusiness.Bll.ProdutoPrepedidoBll>();
            services.AddTransient<Prepedido.ValidacoesPrepedidoBll, Prepedido.ValidacoesPrepedidoBll>();
            services.AddTransient<Prepedido.MontarLogPrepedidoBll, Prepedido.MontarLogPrepedidoBll>();
            services.AddTransient<Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll, Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll>();

            services.AddTransient<Cep.IBancoNFeMunicipio, Testes.Utils.BancoTestes.TestesBancoNFeMunicipio>();

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
                ret.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_Numero = 1000;
                ret.LimiteItens = 12;
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
