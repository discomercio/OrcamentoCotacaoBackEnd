using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfraIdentity
{
    public interface IServicoDecodificarToken
    {
        string ObterApelidoOrcamentista(ClaimsPrincipal User);
    }

    public class ServicoDecodificarToken : IServicoDecodificarToken
    {
        public string ObterApelidoOrcamentista(ClaimsPrincipal User)
        {
            if (User == null)
                return null;
            var aux = User.Claims.FirstOrDefault(r => r.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if(aux == null)
                return null;
            string apelido = aux.Value;
            return apelido;
        }
    }
}


