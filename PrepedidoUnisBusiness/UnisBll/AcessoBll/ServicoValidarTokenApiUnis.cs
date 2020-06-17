using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.AcessoBll
{
    public interface IServicoValidarTokenApiUnis
    {
        bool ValidarToken(string tokenAcesso, out string usuario);
    }

    public class ServicoValidarTokenApiUnis : IServicoValidarTokenApiUnis
    {
        private byte[] key;
        private readonly ILogger<ServicoValidarTokenApiUnis> logger;

        public ServicoValidarTokenApiUnis(IConfiguration configuration, ILogger<ServicoValidarTokenApiUnis> logger)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<PrepedidoUnisBusiness.Utils.ConfiguracaoApiUnis>();
            key = Encoding.ASCII.GetBytes(appSettings.SegredoToken);
            this.logger = logger;
        }

        public bool ValidarToken(string tokenAcesso, out string usuario)
        {
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                var claims = handler.ValidateToken(tokenAcesso, validations, out var tokenSecure);
                var aux = claims.FindFirst(r => r.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                usuario = aux.Value;
                return true;
            }
            catch (Exception e)
            {
                //qualquer excecao é um erro de autorização
                logger.LogWarning(e, $"Erro na validação do TokenAcesso: {tokenAcesso}");
                usuario = null;
                return false;
            }
        }

    }
}


