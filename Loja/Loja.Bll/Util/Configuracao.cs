using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace Loja.Bll.Util
{
    public class Configuracao
    {
        private readonly IConfiguration configuration;

        public Configuracao(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Diretorios = new ClasseDiretorios(configuration);
        }

        public bool PermitirManterConectado
        {
            get
            {
                return configuration.GetSection("Acesso")?.GetValue<bool>("PermitirManterConectado") ?? true;
            }
        }
        public bool ExpiracaoMovel
        {
            get
            {
                return configuration.GetSection("Acesso")?.GetValue<bool>("ExpiracaoMovel") ?? true;
            }
        }
        public TimeSpan ExpiracaoCookieMinutos
        {
            get
            {
                return TimeSpan.FromMinutes(configuration.GetSection("Acesso")?.GetValue<int>("ExpiracaoCookieMinutos") ?? 14400);
            }
        }
        public long ForcarLoginPorGetMinutos
        {
            get
            {
                return configuration.GetSection("Acesso")?.GetValue<long>("ForcarLoginPorGetMinutos") ?? 10080;
            }
        }
        public long RecarregarPermissoesUsuarioMinutos
        {
            get
            {
                return configuration.GetSection("Acesso")?.GetValue<long>("RecarregarPermissoesUsuarioMinutos") ?? 10;
            }
        }

        public class ClasseDiretorios
        {
            private readonly IConfiguration configuration;
            public ClasseDiretorios(IConfiguration configuration)
            {
                this.configuration = configuration;
            }
            public string RaizSiteLojaMvc
            {
                get
                {
                    return configuration.GetSection("Diretorios")?.GetValue<string>("RaizSiteLojaMvc") ?? "";
                }
            }
            public string RaizSiteColorsLoja
            {
                get
                {
                    return configuration.GetSection("Diretorios")?.GetValue<string>("RaizSiteColorsLoja") ?? "";
                }
            }
        }
        public ClasseDiretorios Diretorios;


        public class LimitePedidos
        {
            public int LimitePedidosExatamenteIguais_Numero { get; set; } = 1;
            public int LimitePedidosExatamenteIguais_TempoSegundos { get; set; } = 10;
            public int LimitePedidosMesmoCpfCnpj_Numero { get; set; } = 10;
            public int LimitePedidosMesmoCpfCnpj_TempoSegundos { get; set; } = 3600;
        }

        public LimitePedidos LimitePedidosLoja { get; set; } = new LimitePedidos();
    }
}
