using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Util
{
    public class Configuracao
    {
        private readonly IConfiguration configuration;

        public Configuracao(IConfiguration configuration)
        {
            this.configuration = configuration;
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
