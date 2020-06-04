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
            apelido = apelido.ToUpper().Trim();

            int idErro;

            var dadosCliente = await acessoBll.ValidarUsuario(apelido, senha, false);
            //caso usuário com senha expirada ou bloqueado, retornamos um número 
            if (!string.IsNullOrEmpty(dadosCliente))
            {
                if(int.TryParse(dadosCliente, out idErro))
                {
                    //temos um problema e precisamos mandar algum valor para avisar que é senha expirada ou usuario bloqueado para mostrar na tela
                    UsuarioLogin usuario = new UsuarioLogin { IdErro = idErro };
                    return usuario;
                }                
            }
            

            if (string.IsNullOrEmpty(dadosCliente))
                return null;
            else
            {
                var loja = await acessoBll.BuscarLojaUsuario(apelido);
                UsuarioLogin usuario = new UsuarioLogin { Apelido = apelido, Nome = dadosCliente.ToString(), Loja = loja};
                return usuario;
            }
        }
    }
}
