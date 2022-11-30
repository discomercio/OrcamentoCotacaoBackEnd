using UtilsGlobais.RequestResponse;

namespace OrcamentoCotacaoBusiness.Models.Request.Login
{
    public class LoginRequest: RequestBase
    {
        public string Login { get; set; }

        public string Senha { get; set; }
    }
}