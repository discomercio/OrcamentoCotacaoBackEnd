using InfraIdentity;
using Microsoft.Extensions.Configuration;
using PrepedidoApi.Utils;
using UtilsGlobais;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class PublicoBll
    {
        private readonly IServicoAutenticacao _servicoAutenticacao;
        private readonly IConfiguration _configuration;
        private readonly PrepedidoBusiness.Bll.AcessoBll _acessoBll;

        public PublicoBll(
            IServicoAutenticacao servicoAutenticacao,
            IConfiguration configuration,
            PrepedidoBusiness.Bll.AcessoBll acessoBll
            )
        {
            _servicoAutenticacao = servicoAutenticacao;
            _servicoAutenticacao = servicoAutenticacao;
            _configuration = configuration;
            _acessoBll = acessoBll;
        }

        public string ObterTokenServico()
        {
            var appSettingsSection = _configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Configuracao>();

            var usuario = new UsuarioLogin { Apelido = "PEDREIRA", Senha = "0x0000000000000053f5bf29d54b1997" };

            var usuarioAuth = _servicoAutenticacao.ObterTokenAutenticacao(
                usuario, 
                appSettings.SegredoToken, 
                appSettings.ValidadeTokenMinutos,
                Autenticacao.RoleAcesso, 
                new ServicoAutenticacaoProvider(_acessoBll), 
                out bool unidade_negocio_desconhecida
                );

            return usuarioAuth.Token; 
        }
    }
}
