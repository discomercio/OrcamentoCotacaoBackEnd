using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfraIdentity.ApiUnis
{
    public interface IServicoDecodificarTokenApiUnis
    {
        string ObterUsuario(ClaimsPrincipal User);
    }

    public class ServicoDecodificarTokenApiUnis : IServicoDecodificarTokenApiUnis
    {
        public string ObterUsuario(ClaimsPrincipal User)
        {
            if (User == null)
                return null;
            var aux = User.Claims.FirstOrDefault(r => r.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if(aux == null)
                return null;
            string usuario = aux.Value;
            return usuario;
        }
    }
}


