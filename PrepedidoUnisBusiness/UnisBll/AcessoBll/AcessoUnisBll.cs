using InfraBanco;
using InfraIdentity.ApiUnis;
using Microsoft.Extensions.Configuration;
using PrepedidoApiUnisBusiness.UnisDto.AcessoDto;
using PrepedidoUnisBusiness.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.AcessoBll
{
    public class AcessoUnisBll
    {
        private readonly IConfiguration configuration;
        private readonly IServicoAutenticacaoApiUnis servicoAutenticacaoApiUnis;
        private readonly ContextoBdProvider contextoProvider;

        public AcessoUnisBll(IConfiguration configuration, IServicoAutenticacaoApiUnis servicoAutenticacaoApiUnis, InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.configuration = configuration;
            this.servicoAutenticacaoApiUnis = servicoAutenticacaoApiUnis;
            this.contextoProvider = contextoProvider;
        }
        public async Task<LoginResultadoUnisDto> FazerLogin(LoginUnisDto login, string ip, string userAgent)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<ConfiguracaoApiUnis>();
            var autentica = await servicoAutenticacaoApiUnis.ObterTokenAutenticacaoApiUnis(login.Usuario, login.Senha, 
                appSettings.SegredoToken, appSettings.ValidadeTokenMinutos,
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
    }
}
