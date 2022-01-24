using InfraIdentity;
using OrcamentistaEindicador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuario;

namespace OrcamentoCotacaoApi.Utils
{
    /*
     * efetivamente faz o lofgin; quer dizer, efetivamente verifica a senha
     * */
    public class ServicoAutenticacaoProvider : InfraIdentity.IServicoAutenticacaoProvider
    {

        private readonly OrcamentoCotacaoBusiness.Bll.AcessoBll acessoBll;
        private readonly UsuarioBll usuarioBll;
        private readonly OrcamentistaEindicadorBll orcamentistaEindicadorBll;

        public ServicoAutenticacaoProvider(OrcamentoCotacaoBusiness.Bll.AcessoBll acessoBll, UsuarioBll usuarioBll, OrcamentistaEindicadorBll orcamentistaEindicadorBll)
        {
            this.acessoBll = acessoBll;
            this.usuarioBll = usuarioBll;
            this.orcamentistaEindicadorBll = orcamentistaEindicadorBll;
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
                if (int.TryParse(dadosCliente, out idErro))
                {
                    //temos um problema e precisamos mandar algum valor para avisar que é senha expirada ou usuario bloqueado para mostrar na tela
                    UsuarioLogin usuario = new UsuarioLogin { IdErro = idErro };
                    return usuario;
                }
            }


            if (string.IsNullOrEmpty(dadosCliente))
                return null;
            //Buscar Parceiros e depois buscar vendedores-parceiros
            else
            {
                var usuarioInterno = usuarioBll.PorFiltro(new InfraBanco.Modelos.Filtros.TusuarioFiltro() { usuario = apelido });

                var loja = await acessoBll.BuscarLojaUsuario(apelido);
                var unidade_negocio = await acessoBll.Buscar_unidade_negocio(loja);
                UsuarioLogin usuario = new UsuarioLogin { Apelido = apelido, Nome = usuarioInterno.FirstOrDefault().Nome, Email = usuarioInterno.FirstOrDefault().Email, Loja = string.Join(",", loja.Select(x => x.Loja)), Unidade_negocio = unidade_negocio };
                return usuario;
            }
        }
    }
}
