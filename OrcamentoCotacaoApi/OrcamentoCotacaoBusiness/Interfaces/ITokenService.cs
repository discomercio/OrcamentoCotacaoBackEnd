using InfraIdentity;
using System.IdentityModel.Tokens.Jwt;

namespace OrcamentoCotacaoBusiness.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UsuarioLogin user);
        bool ValidateToken(string authToken);
        JwtSecurityToken DecodeToken(string authToken);
    }
}
