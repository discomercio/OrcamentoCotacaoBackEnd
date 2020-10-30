using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.Util;

namespace Loja.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Configuracao, Configuracao>();
            Configuracao configuracao = new Configuracao(Configuration);

            services.AddHttpContextAccessor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = configuracao.ExpiracaoCookieMinutos;
                    options.SlidingExpiration = configuracao.ExpiracaoMovel;
                    options.LoginPath = new PathString("/Acesso/Login");
                    options.AccessDeniedPath = new PathString("/Acesso/AcessoNegado");
                });

            //exigimos a autenticação de todo mundo!
            services.AddTransient<IAuthorizationHandler, AcessoAuthorizationHandlerBll>();

            services.AddHttpContextAccessor();
            services.AddAuthorization(options =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                       .RequireAuthenticatedUser()
                       .AddRequirements(new AcessoRequirement())
                       .Build();
                options.DefaultPolicy = policy;
                options.FallbackPolicy = policy;
            });

            //precisa para permitir rodar fora da raiz e usar o iframe com o SiteColors
            services.AddAntiforgery((r) => { r.SuppressXFrameOptionsHeader = true; });


            services.AddRazorPages();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = configuracao.ExpiracaoCookieMinutos;
            });

            //bll
            services.AddTransient<Bll.ProdutoBll.ProdutoBll, Bll.ProdutoBll.ProdutoBll>();
            services.AddTransient<Loja.Bll.Bll.pedidoBll.CancelamentoAutomaticoBll, Loja.Bll.Bll.pedidoBll.CancelamentoAutomaticoBll>();
            services.AddTransient<Bll.ClienteBll.ClienteBll, Bll.ClienteBll.ClienteBll>();
            services.AddTransient<Bll.CepBll.CepBll, Bll.CepBll.CepBll>();
            services.AddTransient<Bll.PedidoBll.PedidoBll, Bll.PedidoBll.PedidoBll>();
            services.AddTransient<Loja.Bll.Bll.PrepedidoBll.PrepedidoBll, Loja.Bll.Bll.PrepedidoBll.PrepedidoBll>();
            services.AddTransient<Loja.Bll.PedidoBll.PedidoLogBll, Loja.Bll.PedidoBll.PedidoLogBll>();
            services.AddTransient<Bll.FormaPagtoBll.FormaPagtoBll, Bll.FormaPagtoBll.FormaPagtoBll>();
            services.AddTransient<Bll.CoeficienteBll.CoeficienteBll, Bll.CoeficienteBll.CoeficienteBll>();
            services.AddTransient<Loja.Bll.Bll.AcessoBll.UsuarioAcessoBll, Loja.Bll.Bll.AcessoBll.UsuarioAcessoBll>();
            services.AddTransient<Loja.Bll.Bll.AcessoBll.AcessoAuthorizationHandlerBll, Loja.Bll.Bll.AcessoBll.AcessoAuthorizationHandlerBll>();
            services.AddTransient<Pedido.EfetivaPedidoBll, Pedido.EfetivaPedidoBll>();
            services.AddTransient<Pedido.MontarLogPedidoBll, Pedido.MontarLogPedidoBll>();
            services.AddTransient<Pedido.PedidoCriacao, Pedido.PedidoCriacao>();
            services.AddTransient<Pedido.PedidoBll, Pedido.PedidoBll>();

            services.AddTransient<Loja.Bll.Bll.AcessoBll.SiteColorsBll, Loja.Bll.Bll.AcessoBll.SiteColorsBll>();


            //ContextoProvider
            services.AddTransient<InfraBanco.ContextoBdProvider, InfraBanco.ContextoBdProvider>();
            services.AddTransient<InfraBanco.ContextoCepProvider, InfraBanco.ContextoCepProvider>();
            services.AddTransient<InfraBanco.ContextoNFeProvider, InfraBanco.ContextoNFeProvider>();
            //services.AddTransient<Data.LojaContextoBdProvider, Data.LojaContextoBdProvider>();
            //services.AddTransient<Data.LojaContextoCepProvider, Data.LojaContextoCepProvider>();
            //services.AddTransient<Data.LojaContextoNFeProvider, Data.LojaContextoNFeProvider>();

            //banco de dados
            //banco de dados
            string conexaoBasica = Configuration.GetConnectionString("conexao");
            services.AddDbContext<InfraBanco.ContextoBdBasico>(options =>
            {
                options.UseSqlServer(conexaoBasica);
                options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<InfraBanco.ContextoCepBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoCep"));
                options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<InfraBanco.ContextoNFeBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoNfe"));
                options.EnableSensitiveDataLogging();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            Configuracao configuracao = new Configuracao(Configuration);
            if (env.IsDevelopment())
            {
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(configuracao.Diretorios.RaizSiteLojaMvc))
                    app.UsePathBase(configuracao.Diretorios.RaizSiteLojaMvc);
            }

            app.UseRouting();

            app.UseDefaultFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            app.UseCookiePolicy(cookiePolicyOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
