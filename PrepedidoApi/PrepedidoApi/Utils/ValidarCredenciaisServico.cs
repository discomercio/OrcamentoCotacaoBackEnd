using InfraIdentity;
using PrepedidoBusiness.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoApi.Utils
{
    public class ValidarCredenciaisServico : IValidarCredenciaisServico
    {
        private readonly AcessoBll acessoBll;

        public ValidarCredenciaisServico(AcessoBll acessoBll)
        {
            this.acessoBll = acessoBll;
        }

        public Task<bool> CredenciaisValidas(string apelido)
        {
            var validou = acessoBll.ValidarUsuario(apelido, null, true).Result;
            if (validou == null)
                return Task.FromResult(false);
            if (int.TryParse(validou, out int resultado))
                return Task.FromResult(false);

            //acesso liberado
            return Task.FromResult(true);
        }
    }
}
