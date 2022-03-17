using System.Security.Claims;

namespace OrcamentoCotacaoApi.Utils
{
    public static class ExtensionAutenticacao
    {
        public static string GetVendendor(this ClaimsPrincipal claims)
        {
            return claims.FindFirstValue("Vendendor");
        }
    }
}