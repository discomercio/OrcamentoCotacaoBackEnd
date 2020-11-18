using InfraBanco;
using InfraIdentity.ApiMagento;
using MagentoBusiness.MagentoDto;
using MagentoBusiness.UtilsMagento;
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
            var retorno = new LoginResultadoMagentoDto(autentica.TokenAcesso, autentica.ListaErros);
            return retorno;
        }

        public async Task<LogoutResultadoMagentoDto> FazerLogout(string usuario)
        {
            LogoutResultadoMagentoDto ret = new LogoutResultadoMagentoDto
            {
                ListaErros = new List<string>()
            };
            await (new ServicoAutenticacaoProviderApiMagento(contextoProvider).FazerLogout(usuario, ret));
            return ret;
        }

        public async Task<VersaoApiMagentoDto> VersaoApi()
        {
            string retBuild;
            //vamos ler a DataCompilacao.txt
            var caminhoCompleto = Path.Combine(AppContext.BaseDirectory, "DataCompilacao.txt");
            if (!File.Exists(caminhoCompleto))
            {
                retBuild = "DEBUG";
            }
            else
            {
                retBuild = await File.ReadAllTextAsync(caminhoCompleto);
                retBuild = retBuild.Replace("\r", "");
                retBuild = retBuild.Replace("\n", "");
                retBuild = retBuild.Trim();
            }

            VersaoApiMagentoDto ret = new VersaoApiMagentoDto(
                ambiente: configuracaoApiMagento.VersaoApi.Ambiente,
                mensagem: configuracaoApiMagento.VersaoApi.Mensagem,
                versaoApi: configuracaoApiMagento.VersaoApi.VersaoApi,
                build: retBuild
            );


            return ret;
        }
    }
}
