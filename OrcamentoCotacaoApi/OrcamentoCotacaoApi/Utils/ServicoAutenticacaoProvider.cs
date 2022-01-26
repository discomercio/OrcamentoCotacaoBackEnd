using InfraIdentity;
using Loja;
using OrcamentistaEindicador;
using OrcamentistaEIndicadorVendedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuario;
using UtilsGlobais;

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
        private readonly OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll;
        private readonly LojaBll lojaBll;

        public ServicoAutenticacaoProvider(OrcamentoCotacaoBusiness.Bll.AcessoBll acessoBll, UsuarioBll usuarioBll,
            OrcamentistaEindicadorBll orcamentistaEindicadorBll, OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll,
            LojaBll lojaBll)
        {
            this.acessoBll = acessoBll;
            this.usuarioBll = usuarioBll;
            this.orcamentistaEindicadorBll = orcamentistaEindicadorBll;
            this.orcamentistaEIndicadorVendedorBll = orcamentistaEIndicadorVendedorBll;
            this.lojaBll = lojaBll;
        }

        //retorna null se nao exisitr (ou se a senha estiver errada)
        public async Task<InfraIdentity.UsuarioLogin> ObterUsuario(string apelido, string senha)
        {
            //trabalhamos sempre com maiúsuculas
            apelido = apelido.ToUpper().Trim();

            int idErro;

            senha = Util.codificaDado(senha, false);

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
            {
                //Buscar Parceiros e depois buscar vendedores-parceiros
                var parceiro = orcamentistaEindicadorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = apelido, datastamp = senha }).FirstOrDefault();
                if (parceiro != null)
                {
                    //var loja = await acessoBll.BuscarLojaUsuario(apelido);
                    //var unidade_negocio = await acessoBll.Buscar_unidade_negocio(loja);
                    var loja = lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = parceiro.Loja });
                    UsuarioLogin usuario = new UsuarioLogin
                    {
                        Apelido = apelido,
                        Nome = parceiro.Razao_Social_Nome,
                        Email = "",
                        Loja = parceiro.Loja,
                        Unidade_negocio = loja.FirstOrDefault().Unidade_Negocio,
                        VendedorResponsavel = parceiro.Vendedor
                    };
                    return usuario;
                }
                else
                {
                    //Buscar vendedores-parceiros
                    var vendedorParceiro = orcamentistaEIndicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { email = apelido, senha = senha }).FirstOrDefault();
                    if (vendedorParceiro != null)
                    {
                        var parceiroValidacao = await orcamentistaEindicadorBll.ValidarParceiro(vendedorParceiro.IdIndicador, null, true);

                        if (parceiroValidacao != null)
                        {
                            //var loja = await acessoBll.BuscarLojaUsuario(apelido);
                            //var unidade_negocio = await acessoBll.Buscar_unidade_negocio(loja);
                            var loja = lojaBll.PorFiltro(new InfraBanco.Modelos.Filtros.TlojaFiltro() { Loja = vendedorParceiro.Loja });
                            UsuarioLogin usuario = new UsuarioLogin
                            {
                                Apelido = apelido,
                                Nome = vendedorParceiro.Nome,
                                Email = vendedorParceiro.Email,
                                Loja = vendedorParceiro.Loja,
                                Unidade_negocio = loja.FirstOrDefault().Unidade_Negocio,
                                VendedorResponsavel = vendedorParceiro.VendedorResponsavel,
                                IdParceiro = parceiroValidacao.Apelido
                            };
                            return usuario;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }

            }
            else
            {
                var usuarioInterno = usuarioBll.PorFiltro(new InfraBanco.Modelos.Filtros.TusuarioFiltro() { usuario = apelido });

                var loja = await acessoBll.BuscarLojaUsuario(apelido);
                var unidade_negocio = await acessoBll.Buscar_unidade_negocio(loja);
                UsuarioLogin usuario = new UsuarioLogin
                {
                    Apelido = apelido,
                    Nome = usuarioInterno.FirstOrDefault().Nome,
                    Email = usuarioInterno.FirstOrDefault().Email,
                    Loja = string.Join(",", loja.Select(x => x.Loja)),
                    Unidade_negocio = unidade_negocio,
                    VendedorResponsavel = null
                };
                return usuario;
            }
        }
    }
}
