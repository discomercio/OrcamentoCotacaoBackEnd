using System;
using System.Collections.Generic;
using System.Linq;
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

        public static bool ValidaPermissao(this ClaimsPrincipal claims, int permissao)
        {
            if (permissao == 0) return false;

            var claimsPermissoes = claims.FindFirstValue("Permissoes");
            return claimsPermissoes.Contains(permissao.ToString());
        }
    }
}