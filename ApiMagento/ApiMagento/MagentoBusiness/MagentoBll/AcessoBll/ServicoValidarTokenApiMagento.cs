using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MagentoBusiness.UtilsMagento;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MagentoBusiness.MagentoBll.AcessoBll
{
    public interface IServicoValidarTokenApiMagento
    {
        bool ValidarToken(string tokenAcesso, out string? usuario);
    }

    public class ServicoValidarTokenApiMagento:IServicoValidarTokenApiMagento
    {
        private readonly byte[] key;
        private readonly ILogger<ServicoValidarTokenApiMagento> logger;

        public ServicoValidarTokenApiMagento(ConfiguracaoApiMagento configuracaoApiUnis, ILogger<ServicoValidarTokenApiMagento> logger)
        {
            key = Encoding.ASCII.GetBytes(configuracaoApiUnis.SegredoToken);
            this.logger = logger;
        }

        public bool ValidarToken(string tokenAcesso, out string? usuario)
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
