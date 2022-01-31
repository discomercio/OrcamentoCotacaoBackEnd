using InfraIdentity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    public class BaseController : ControllerBase
    {
        public static UsuarioLogin LoggedUser { get {
                return new UsuarioLogin();
            } }
    }
}
