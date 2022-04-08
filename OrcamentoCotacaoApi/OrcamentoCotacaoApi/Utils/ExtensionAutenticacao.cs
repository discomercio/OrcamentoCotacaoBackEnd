using System;
using System.Security.Claims;
using static InfraBanco.Constantes.Constantes;

namespace OrcamentoCotacaoApi.Utils
{
    public static class ExtensionAutenticacao
    {
        public static string GetVendedor(this ClaimsPrincipal claims)
        {
            return claims.FindFirstValue("Vendedor");
        }
        public static TipoUsuario GetTipoUsuario(this ClaimsPrincipal claims)
        {
            var tipoUsuario = claims.FindFirstValue("TipoUsuario");
            TipoUsuario tipoUsuarioEnum;
            if (Enum.TryParse< TipoUsuario>(tipoUsuario, out tipoUsuarioEnum))
            {
                return tipoUsuarioEnum;
            }
            return TipoUsuario.NAO_IDENTIFICADO;
        }
        public static string GetParceiro(this ClaimsPrincipal claims)
        {
            return claims.FindFirstValue("Parceiro");
        }
    }
}