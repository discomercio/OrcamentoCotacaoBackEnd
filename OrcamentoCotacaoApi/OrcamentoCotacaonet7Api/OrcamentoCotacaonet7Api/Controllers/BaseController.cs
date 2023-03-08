using InfraIdentity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace OrcamentoCotacaonet7Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        public UsuarioLogin LoggedUser
        {
            get
            {
                var obj = (((ClaimsIdentity)User.Identity).Claims.Where(x => x.Type == "UsuarioLogin")).FirstOrDefault();
                if (obj != null)
                {
                    var usuarioLogin = JsonSerializer.Deserialize<UsuarioLogin>(obj.Value.ToString());
                    return usuarioLogin;
                }
                return new UsuarioLogin();
            }
        }
    }
}
