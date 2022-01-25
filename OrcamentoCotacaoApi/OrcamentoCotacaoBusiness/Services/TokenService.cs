using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.Extensions.Configuration;
using OrcamentoCotacaoBusiness.Interfaces;
using InfraIdentity;

namespace OrcamentoCotacaoBusiness.Services
{
    public class TokenService : ITokenService
    {
        private readonly byte[] _appSecret;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration config, ILogger<TokenService> logger)
        {
            _logger = logger;
            _appSecret = Encoding.ASCII.GetBytes(config["AppSecret"]);
        }

        public string GenerateToken(UsuarioLogin usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim("Email", !string.IsNullOrEmpty(usuario.Email)? usuario.Email:""),
                    new Claim("TipoUsuario", !string.IsNullOrEmpty(usuario.TipoUsuario)?usuario.TipoUsuario:""),
                    new Claim("Parceiro", !string.IsNullOrEmpty(usuario.IdParceiro)?usuario.IdParceiro:""),
                    new Claim("Vendedor", !string.IsNullOrEmpty(usuario.IdVendedor)?usuario.IdVendedor:""),
                    new Claim("Lojas", !string.IsNullOrEmpty(usuario.Loja)?usuario.Loja:""),
                    new Claim("UnidadeNegocio", !string.IsNullOrEmpty(usuario.Unidade_negocio)?usuario.Unidade_negocio:"")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_appSecret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string authToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();

                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation("[invalid token]: {0}", e);
                return false;
            }
        }

        public JwtSecurityToken DecodeToken(string authToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(authToken);
                return token;
            }
            catch (Exception e)
            {
                _logger.LogInformation("[invalid token]: {0}", e);
                return null;
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Validate expiration in the generated token
                ValidateAudience = false, // Validate audiance in the generated token
                ValidateIssuer = false,   // Validate issuer in the generated token
                //ValidIssuer = "Sample",
                //ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(_appSecret) // The same key as the one that generate the token
            };
        }
    }
}
