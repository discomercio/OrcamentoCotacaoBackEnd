using InfraIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoApi.Utils
{
    /*
     * efetivamente faz o lofgin; quer dizer, efetivamente verifica a senha
     * */
    public class ServicoAutenticacaoProvider : InfraIdentity.IServicoAutenticacaoProvider
    {
        // usuários hardcoded por enquanto
        private readonly List<InfraIdentity.UsuarioLogin> _users = new List<InfraIdentity.UsuarioLogin>
        {
            new UsuarioLogin { Apelido = "teste", Nome= "Nome de teste" }
        };

        //retorna null se nao exisitr (ou se a senha estiver errada)
        public async Task<InfraIdentity.UsuarioLogin> ObterUsuario(string apelido, string senha)
        {
            //var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            var user = _users.SingleOrDefault(x => x.Apelido == apelido);
            await Task.Run(() => { });
            return user;
        }
    }
}
