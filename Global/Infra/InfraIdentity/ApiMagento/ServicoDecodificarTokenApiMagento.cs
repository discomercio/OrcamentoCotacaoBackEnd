using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace InfraIdentity.ApiMagento
{
    public interface IServicoDecodificarTokenApiMagento
    {
        string ObterApelidoOrcamentista(ClaimsPrincipal User);
    }

    public class ServicoDecodificarTokenApiMagento : IServicoDecodificarTokenApiMagento
    {
        public string ObterApelidoOrcamentista(ClaimsPrincipal User)
        {
            if (User == null)
                return null;
            var aux = User.Claims.FirstOrDefault(r => r.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (aux == null)
                return null;
            string apelido = aux.Value;
            return apelido;
        }
    }
}
