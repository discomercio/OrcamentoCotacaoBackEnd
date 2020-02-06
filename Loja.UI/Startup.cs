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

            services.AddRazorPages();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession();

            //bll
            services.AddTransient<Bll.ProdutoBll.ProdutoBll, Bll.ProdutoBll.ProdutoBll>();
            services.AddTransient<Bll.ClienteBll.ClienteBll, Bll.ClienteBll.ClienteBll>();
            services.AddTransient<Bll.CepBll.CepBll, Bll.CepBll.CepBll>();
            services.AddTransient<Bll.PedidoBll.PedidoBll, Bll.PedidoBll.PedidoBll>();
            services.AddTransient<Bll.FormaPagtoBll.FormaPagtoBll, Bll.FormaPagtoBll.FormaPagtoBll>();
            services.AddTransient<Bll.CoeficienteBll.CoeficienteBll, Bll.CoeficienteBll.CoeficienteBll>();


            //ContextoProvider
            services.AddTransient<Data.ContextoBdProvider, Data.ContextoBdProvider>();
            services.AddTransient<Data.ContextoCepProvider, Data.ContextoCepProvider>();
            services.AddTransient<Data.ContextoNFeProvider, Data.ContextoNFeProvider>();

            //banco de dados
            services.AddDbContext<Loja.Data.ContextoBdBasico>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaohomologa"));
            });
            services.AddDbContext<Loja.Data.ContextoCepBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoCep"));
            });
            services.AddDbContext<Loja.Data.ContextoNFeBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoNfe"));
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
