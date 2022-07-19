using System.IdentityModel.Tokens.Jwt;

namespace InfraIdentity
{
    public interface ITokenService
    {
        string GenerateToken(UsuarioLogin user);
        bool ValidateToken(string authToken);
        JwtSecurityToken DecodeToken(string authToken);
    }
}
