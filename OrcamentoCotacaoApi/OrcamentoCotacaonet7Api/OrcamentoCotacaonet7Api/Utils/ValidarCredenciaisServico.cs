using InfraIdentity;
using OrcamentoCotacaoBusiness.Bll;
using System.Threading.Tasks;

namespace OrcamentoCotacaonet7Api.Utils
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
            var usuario = acessoBll.ValidarUsuario(apelido, null, true, string.Empty, string.Empty, out msgErro);
            if (msgErro == null)
                return Task.FromResult(false);
            if (int.TryParse(msgErro, out int resultado))
                return Task.FromResult(false);

            //acesso liberado
            return Task.FromResult(true);
        }
    }
}
