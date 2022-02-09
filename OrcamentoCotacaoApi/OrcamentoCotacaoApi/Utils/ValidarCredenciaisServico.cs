using InfraIdentity;
using OrcamentoCotacaoBusiness.Bll;
using PrepedidoBusiness.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Utils
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
            string msgErro;
            var usuario = acessoBll.ValidarUsuario(apelido, null, true, out msgErro);
            if (msgErro == null)
                return Task.FromResult(false);
            if (int.TryParse(msgErro, out int resultado))
                return Task.FromResult(false);

            //acesso liberado
            return Task.FromResult(true);
        }
    }
}
