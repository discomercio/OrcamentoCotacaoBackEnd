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

        private readonly PrepedidoBusiness.Bll.AcessoBll acessoBll;

        public ServicoAutenticacaoProvider(PrepedidoBusiness.Bll.AcessoBll acessoBll)
        {
            this.acessoBll = acessoBll;
        }

        // usuários hardcoded por enquanto
        //private readonly List<InfraIdentity.UsuarioLogin> _users = new List<InfraIdentity.UsuarioLogin>
        //{
        //    new UsuarioLogin { Apelido = "teste", Nome= "Nome de teste" }
        //};

        //retorna null se nao exisitr (ou se a senha estiver errada)
        public async Task<InfraIdentity.UsuarioLogin> ObterUsuario(string apelido, string senha)
        {
            //gabriel, colocar aqui a chamada para a BLL de acesso (AcessoBll)
            //    a Bll só precisa retornar se fez o login e o nome do usuário que fez o login 
            //o apelido a gente ja sabe
            //var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            //validação de dados do usuário
            var dadosCliente = await acessoBll.ValidarUsuario(apelido, senha);
            if (string.IsNullOrEmpty(dadosCliente))
                return null;
            else
            {
                UsuarioLogin usuario = new UsuarioLogin { Apelido = apelido, Nome = dadosCliente.ToString()};
                //var user = _users.SingleOrDefault(x => x.Apelido == apelido);
                await Task.Run(() => { });
                //return user;
                return usuario;
            }
        }
    }
}
