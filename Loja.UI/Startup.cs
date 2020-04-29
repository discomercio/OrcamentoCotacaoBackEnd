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
            services.AddTransient<Loja.Bll.Bll.PedidoBll.EfetivaPedido.EfetivaPedidoBll, Loja.Bll.Bll.PedidoBll.EfetivaPedido.EfetivaPedidoBll>();


            //ContextoProvider
            services.AddTransient<Data.ContextoBdProvider, Data.ContextoBdProvider>();
            services.AddTransient<Data.ContextoCepProvider, Data.ContextoCepProvider>();
            services.AddTransient<Data.ContextoNFeProvider, Data.ContextoNFeProvider>();

            //banco de dados
            services.AddDbContext<Loja.Data.ContextoBdBasico>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaohomologa"));
                options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<Loja.Data.ContextoCepBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoCep"));
                options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<Loja.Data.ContextoNFeBd>(options =>
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
