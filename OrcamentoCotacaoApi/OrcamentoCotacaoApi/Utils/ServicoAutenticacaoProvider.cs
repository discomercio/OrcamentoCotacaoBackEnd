using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraIdentity;
using Loja;
using OrcamentistaEindicador;
using OrcamentistaEIndicadorVendedor;
using OrcamentoCotacaoBusiness.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuario;
using UtilsGlobais;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaoApi.Utils
{
    /*
     * efetivamente faz o lofgin; quer dizer, efetivamente verifica a senha
     * */
    public class ServicoAutenticacaoProvider : InfraIdentity.IServicoAutenticacaoProvider
    {

        private readonly OrcamentoCotacaoBusiness.Bll.AcessoBll acessoBll;
        private readonly UsuarioBll usuarioBll;
        private readonly OrcamentistaEIndicadorBll orcamentistaEindicadorBll;
        private readonly OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll;
        private readonly LojaBll lojaBll;

        public ServicoAutenticacaoProvider(OrcamentoCotacaoBusiness.Bll.AcessoBll acessoBll, UsuarioBll usuarioBll,
            OrcamentistaEIndicadorBll orcamentistaEindicadorBll, OrcamentistaEIndicadorVendedorBll orcamentistaEIndicadorVendedorBll,
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

            string msgErro;
            var dadosCliente = acessoBll.ValidarUsuario(apelido, senha, false, out msgErro);
            //caso usuário com senha expirada ou bloqueado, retornamos um número 
            if (!string.IsNullOrEmpty(msgErro))
            {
                if (int.TryParse(msgErro, out idErro))
                {
                    //temos um problema e precisamos mandar algum valor para avisar que é senha expirada ou usuario bloqueado para mostrar na tela
                    UsuarioLogin usuario = new UsuarioLogin { IdErro = idErro };
                    return usuario;
                }
            }


            if (dadosCliente == null)
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
                        VendedorResponsavel = parceiro.Vendedor,
                        IdParceiro = parceiro.Apelido,
                        Permissoes = new List<string>()
                        {
                            ((int)ePermissao.AcessoAoModulo).ToString(),
                            ((int)ePermissao.ParceiroIndicadorUsuarioMaster).ToString()
                        },
                        TipoUsuario = (int)Constantes.TipoUsuario.PARCEIRO
                        
                    };
                    return usuario;
                }
                else
                {
                    //Buscar vendedores-parceiros
                    var vendedorParceiro = orcamentistaEIndicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { email = apelido, datastamp = senha }).FirstOrDefault();
                    if (vendedorParceiro != null)
                    {
                        var parceiroValidacao = await orcamentistaEindicadorBll.ValidarParceiro(vendedorParceiro.VendedorResponsavel, null, true);

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
                                IdParceiro = parceiroValidacao.Apelido,
                                Permissoes = new List<string>()
                                {
                                    ((int)ePermissao.AcessoAoModulo).ToString()
                                },
                                TipoUsuario = (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO
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
                UsuarioLogin usuario = new UsuarioLogin();
                switch (dadosCliente.TipoUsuario)
                {
                    case (int)Constantes.TipoUsuario.VENDEDOR:
                    case (int)Constantes.TipoUsuario.GESTOR:
                        var loja = await acessoBll.BuscarLojaUsuario(apelido);
                        var unidade_negocio = await acessoBll.Buscar_unidade_negocio(loja);
                        var usuarioInterno = usuarioBll.PorFiltro(new InfraBanco.Modelos.Filtros.TusuarioFiltro() { usuario = apelido });
                        usuario = new UsuarioLogin
                        {
                            Apelido = apelido,
                            Nome = usuarioInterno.FirstOrDefault().Nome,
                            Email = usuarioInterno.FirstOrDefault().Email,
                            Loja = string.Join(",", loja.Select(x => x.Loja)),
                            Unidade_negocio = unidade_negocio,
                            VendedorResponsavel = null,
                            Permissoes = new List<string>()
                            {
                                ((int)ePermissao.AcessoAoModulo).ToString(),
                                ((int)ePermissao.AdministradorDoModulo).ToString()
                            },
                            TipoUsuario = (int)Constantes.TipoUsuario.VENDEDOR,
                            Id = usuarioInterno.FirstOrDefault().Id
                        };
                        return usuario;
                    //break;
                    case (int)Constantes.TipoUsuario.PARCEIRO:
                        var orcamentista = orcamentistaEindicadorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEindicadorFiltro() { apelido = apelido, acessoHabilitado = 1 });
                        var lojaOrcamentista = new List<TusuarioXLoja>();
                        lojaOrcamentista.Add(new TusuarioXLoja() { Loja = orcamentista.FirstOrDefault().Loja });
                        usuario = new UsuarioLogin
                        {
                            Apelido = apelido,
                            Nome = orcamentista.FirstOrDefault().Razao_Social_Nome,
                            //Email = orcamentista.FirstOrDefault().Email,
                            Loja = string.Join(",", lojaOrcamentista.Select(x => x.Loja)),
                            Unidade_negocio = acessoBll.Buscar_unidade_negocio(lojaOrcamentista).Result,
                            VendedorResponsavel = orcamentista.FirstOrDefault().Vendedor,
                            IdParceiro = orcamentista.FirstOrDefault().Apelido,
                            Permissoes = new List<string>()
                            {
                                ((int)ePermissao.AcessoAoModulo).ToString(),
                                ((int)ePermissao.ParceiroIndicadorUsuarioMaster).ToString()
                            },
                            TipoUsuario = (int)Constantes.TipoUsuario.PARCEIRO,
                            Id = orcamentista.FirstOrDefault().IdIndicador
                        };
                        break;
                    case (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO:
                        var orcamentistaVendedor = orcamentistaEIndicadorVendedorBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentistaEIndicadorVendedorFiltro() { email = apelido });
                        var lojaOrcamentistaVendedor = new List<TusuarioXLoja>();
                        lojaOrcamentistaVendedor.Add(new TusuarioXLoja() { Loja = orcamentistaVendedor.FirstOrDefault().Loja });
                        usuario = new UsuarioLogin
                        {
                            Apelido = apelido,
                            Nome = orcamentistaVendedor.FirstOrDefault().Nome,
                            Email = orcamentistaVendedor.FirstOrDefault().Email,
                            Loja = string.Join(",", lojaOrcamentistaVendedor.Select(x => x.Loja)),
                            Unidade_negocio = acessoBll.Buscar_unidade_negocio(lojaOrcamentistaVendedor).Result,
                            VendedorResponsavel = orcamentistaVendedor.FirstOrDefault().VendedorResponsavel,
                            IdParceiro = orcamentistaVendedor.FirstOrDefault().IdIndicador.ToString(),
                            Permissoes = new List<string>()
                            {
                                ((int)ePermissao.AcessoAoModulo).ToString()
                            },
                            TipoUsuario = (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO,
                            Id = orcamentistaVendedor.FirstOrDefault().Id
                        };
                        break;
                    default:
                        break;
                }



                return usuario;
            }
        }
    }
}
