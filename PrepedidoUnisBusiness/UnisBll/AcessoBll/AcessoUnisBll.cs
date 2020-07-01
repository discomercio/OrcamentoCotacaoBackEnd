using InfraBanco;
using InfraIdentity.ApiUnis;
using Microsoft.Extensions.Configuration;
using PrepedidoApiUnisBusiness.UnisDto.AcessoDto;
using PrepedidoUnisBusiness.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.AcessoBll
{
    public class AcessoUnisBll
    {
        private readonly ConfiguracaoApiUnis configuracaoApiUnis;
        private readonly IServicoAutenticacaoApiUnis servicoAutenticacaoApiUnis;
        private readonly ContextoBdProvider contextoProvider;

        public AcessoUnisBll(IConfiguration configuration, IServicoAutenticacaoApiUnis servicoAutenticacaoApiUnis, InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.servicoAutenticacaoApiUnis = servicoAutenticacaoApiUnis;
            this.contextoProvider = contextoProvider;

            var appSettingsSection = configuration.GetSection("AppSettings");
            configuracaoApiUnis = appSettingsSection.Get<ConfiguracaoApiUnis>();
        }
        public async Task<LoginResultadoUnisDto> FazerLogin(LoginUnisDto login, string ip, string userAgent)
        {
            var autentica = await servicoAutenticacaoApiUnis.ObterTokenAutenticacaoApiUnis(login.Usuario, login.Senha,
                configuracaoApiUnis.SegredoToken, configuracaoApiUnis.ValidadeTokenMinutos,
                AutenticacaoApiUnis.RoleAcesso, new ServicoAutenticacaoProviderApiUnis(contextoProvider),
                ip, userAgent);
            var retorno = new LoginResultadoUnisDto()
            {
                TokenAcesso = autentica.TokenAcesso,
                ListaErros = autentica.ListaErros
            };

            return retorno;
        }

        public async Task<LogoutResultadoUnisDto> FazerLogout(LogoutUnisDto logout, string usuario)
        {
            LogoutResultadoUnisDto ret = new LogoutResultadoUnisDto();
            ret.ListaErros = new List<string>();
            await (new ServicoAutenticacaoProviderApiUnis(contextoProvider).FazerLogout(usuario, ret));
            return ret;
        }

        public async Task<VersaoApiUnisDto> VersaoApi()
        {
            VersaoApiUnisDto ret = new VersaoApiUnisDto()
            {
                Ambiente = configuracaoApiUnis.VersaoApi.Ambiente,
                Mensagem = configuracaoApiUnis.VersaoApi.Mensagem,
                VersaoApi = configuracaoApiUnis.VersaoApi.VersaoApi
            };

            //vamos ler a DataCompilacao.txt
            var caminhoCompleto = Path.Combine(AppContext.BaseDirectory, "DataCompilacao.txt");
            if(!File.Exists(caminhoCompleto))
            {
                ret.Build = "DEBUG";
            }
            else
            {
                ret.Build = await File.ReadAllTextAsync(caminhoCompleto);
                ret.Build = ret.Build.Replace("\r", "");
                ret.Build = ret.Build.Replace("\n", "");
                ret.Build = ret.Build.Trim();
            }


            return ret;
        }

    }
}
