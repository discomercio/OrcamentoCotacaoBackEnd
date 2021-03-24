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

        /*
         * todo: ler as seguintes propriedades:
         * 
  "ID_PARAM_SITE": "ArtBS",
  "ID_PARAM_SITE_comentarios": [
    "ID_PARAM_SITE valores permitidos:",
    "PARÂMETRO DE FUNCIONAMENTO DO SITE (ARTVEN3 = BONSHOP; ARTVEN = FABRICANTE)",
    "COD_SITE_ARTVEN_BONSHOP = \"ArtBS\"",
    "COD_SITE_ASSISTENCIA_TECNICA = \"AssTec\""
  ],
  
    "conexao_comentarios": [
      "conexao: conexão padrão do sistema",
      "conexaoBS: conexão ao banco da BonShop ",
      "conexaoAT: conexão ao banco da Assitência Técnica ",
      "conexaoCep: conexão ao banco do CEP",
      "conexaoNfe: conexão ao banco de municípios do IBGE (está na tabela t_NFe_EMITENTE, verificar com hamilton se está correto)"
    ],
    "conexao": "server=ITS-DBDEV\\DESE;database=ARCLUBE_DIS;Uid=appAirClube;Pwd=appAirClube;",
    "conexaoBS": "server=ITS-DBDEV\\DESE;database=ARCLUBE_DIS;Uid=appAirClube;Pwd=appAirClube;",
    "conexaoAT": "server=ITS-DBDEV\\DESE;database=ARCLUBE_DIS;Uid=appAirClube;Pwd=appAirClube;",
    "conexaoCep": "server=ITS-DBDEV\\DESE;database=AirClub_CEP;Uid=CEP_Homologacao;Pwd=np8#A5d53-T82z;",
    "conexaoNfe": "server=ITS-DBDEV\\DESE;database=NFe_Dis_SP_Filial;Uid=NFe_homologacao;Pwd=L4-At2y9#vgu57;"
*/

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
            public int LimiteItens { get; set; } = 12;
        }

        public LimitePedidos LimitePedidosLoja { get; set; } = new LimitePedidos();
    }
}
