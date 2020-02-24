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
                return configuration.GetSection("Acesso")?.GetValue<bool>("PermitirManterConectado") ?? false;
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
                return TimeSpan.FromMinutes(configuration.GetSection("Acesso")?.GetValue<int>("ExpiracaoCookieMinutos") ?? 120);
            }
        }

    }
}
