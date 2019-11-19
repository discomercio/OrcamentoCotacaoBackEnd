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

        //retorna null se nao exisitr (ou se a senha estiver errada)
        public async Task<InfraIdentity.UsuarioLogin> ObterUsuario(string apelido, string senha)
        {
            //trabalhamos sempre com maiúsuculas
            apelido = apelido.ToUpperInvariant();
            senha= senha.ToUpperInvariant();


            var dadosCliente = await acessoBll.ValidarUsuario(apelido, senha);
            var loja = await acessoBll.BuscarLojaUsuario(apelido);

            if (string.IsNullOrEmpty(dadosCliente))
                return null;
            else
            {               
                UsuarioLogin usuario = new UsuarioLogin { Apelido = apelido, Nome = dadosCliente.ToString(), Loja = loja};
                return usuario;
            }
        }
    }
}
