using InfraBanco;
using InfraIdentity.ApiMagento;
using MagentoBusiness.MagentoDto;
using MagentoBusiness.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MagentoBusiness.MagentoBll.AcessoBll
{
    public class AcessoMagentoBll
    {
        private readonly ConfiguracaoApiMagento configuracaoApiMagento;
        private readonly IServicoAutenticacaoApiMagento servicoAutenticacaoApiMagento;
        private readonly ContextoBdProvider contextoProvider;

        public AcessoMagentoBll(ConfiguracaoApiMagento configuracaoApiMagento, 
            IServicoAutenticacaoApiMagento servicoAutenticacaoApiMagento, InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.configuracaoApiMagento = configuracaoApiMagento;
            this.servicoAutenticacaoApiMagento = servicoAutenticacaoApiMagento;
            this.contextoProvider = contextoProvider;
        }

        public async Task<LoginResultadoMagentoDto> FazerLogin(LoginMagentoDto login, string ip, string userAgent)
        {
            var autentica = await servicoAutenticacaoApiMagento.ObterTokenAutenticacaoApiMagento(login.Usuario, login.Senha,
                configuracaoApiMagento.SegredoToken, configuracaoApiMagento.ValidadeTokenMinutos,
                AutenticacaoApiMagento.RoleAcesso, new ServicoAutenticacaoProviderApiMagento(contextoProvider),
                ip, userAgent, configuracaoApiMagento.ApelidoPerfilLiberaAcessoApiMagento);
            var retorno = new LoginResultadoMagentoDto()
            {
                TokenAcesso = autentica.TokenAcesso,
                ListaErros = autentica.ListaErros
            };

            return retorno;
        }

        public async Task<LogoutResultadoMagentoDto> FazerLogout(LogoutMagentoDto logout, string usuario)
        {
            LogoutResultadoMagentoDto ret = new LogoutResultadoMagentoDto();
            ret.ListaErros = new List<string>();
            await (new ServicoAutenticacaoProviderApiMagento(contextoProvider).FazerLogout(usuario, ret));
            return ret;
        }

        public async Task<VersaoApiMagentoDto> VersaoApi()
        {
            VersaoApiMagentoDto ret = new VersaoApiMagentoDto()
            {
                Ambiente = configuracaoApiMagento.VersaoApi.Ambiente,
                Mensagem = configuracaoApiMagento.VersaoApi.Mensagem,
                VersaoApi = configuracaoApiMagento.VersaoApi.VersaoApi
            };

            //vamos ler a DataCompilacao.txt
            var caminhoCompleto = Path.Combine(AppContext.BaseDirectory, "DataCompilacao.txt");
            if (!File.Exists(caminhoCompleto))
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
