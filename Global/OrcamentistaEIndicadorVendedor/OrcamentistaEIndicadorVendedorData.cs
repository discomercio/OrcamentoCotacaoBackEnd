using ClassesBase;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrcamentistaEIndicadorVendedor
{
    public class OrcamentistaEIndicadorVendedorData : BaseData<TorcamentistaEIndicadorVendedor, TorcamentistaEIndicadorVendedorFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentistaEIndicadorVendedorData(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentistaEIndicadorVendedor Atualizar(TorcamentistaEIndicadorVendedor obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                db.TorcamentistaEIndicadorVendedor.Update(obj);
                db.SaveChanges();
                db.transacao.Commit();
                return obj;
            }
        }

        public TorcamentistaEIndicadorVendedor AtualizarComTransacao(TorcamentistaEIndicadorVendedor model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TorcamentistaEIndicadorVendedor.Update(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public bool Excluir(TorcamentistaEIndicadorVendedor obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TorcamentistaEIndicadorVendedor obj, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TorcamentistaEIndicadorVendedor.Remove(obj);
            contextoBdGravacao.SaveChanges();
        }

        public TorcamentistaEIndicadorVendedor Inserir(TorcamentistaEIndicadorVendedor obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                db.TorcamentistaEIndicadorVendedor.Add(obj);
                db.SaveChanges();
                db.transacao.Commit();
                return obj;
            }
        }

        public TorcamentistaEIndicadorVendedor InserirComTransacao(TorcamentistaEIndicadorVendedor model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TorcamentistaEIndicadorVendedor.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentistaEIndicadorVendedor> PorFilroComTransacao(TorcamentistaEIndicadorVendedorFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentistaEIndicadorVendedor> PorFiltro(TorcamentistaEIndicadorVendedorFiltro obj)
        {
            try
            {
                /*SELECT orc.Nome, orc.email, orc.nome, orc.IdParceiro, 
                   orc.ativo, toei.vendedor as IdVendedor FROM t_ORCAMENTISTA_E_INDICADOR_VENDEDOR orc 
                   INNER JOIN t_ORCAMENTISTA_E_INDICADOR toei ON toei.apelido = orc.IdParceiro 
                   WHERE orc.email = @login AND orc.senha = @senha*/
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var vendedorParceiro = from usr in db.TorcamentistaEIndicadorVendedor
                                           join par in db.TorcamentistaEindicador on usr.IdIndicador equals par.IdIndicador
                                           select new TorcamentistaEIndicadorVendedor()
                                           {
                                               Id = usr.Id,
                                               Nome = usr.Nome,
                                               Email = usr.Email,
                                               Datastamp = UtilsGlobais.SenhaBll.DecodificaSenha(usr.Datastamp),
                                               Senha = usr.Senha,
                                               IdIndicador = usr.IdIndicador,
                                               Telefone = usr.Telefone,
                                               Celular = usr.Celular,
                                               Ativo = usr.Ativo,
                                               UsuarioCadastro = usr.UsuarioCadastro,
                                               DataUltimaAlteracaoSenha = usr.DataUltimaAlteracaoSenha,
                                               UsuarioUltimaAlteracao = usr.UsuarioUltimaAlteracao,
                                               DataCadastro = usr.DataCadastro,
                                               DataUltimaAlteracao = usr.DataUltimaAlteracao,
                                               Loja = par.Loja,
                                               VendedorResponsavel = par.Vendedor,
                                               Parceiro = par.Apelido,
                                               StLoginBloqueadoAutomatico = usr.StLoginBloqueadoAutomatico
                                           };

                    if (obj.id > 0)
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.Id == obj.id);
                    }
                    if (!string.IsNullOrEmpty(obj.email))
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.Email.ToUpper() == obj.email.ToUpper());
                    }
                    if (!string.IsNullOrEmpty(obj.datastamp))
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.Datastamp == obj.datastamp);
                    }
                    if (!string.IsNullOrEmpty(obj.loja))
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.Loja == obj.loja);
                    }
                    if (obj.IdIndicador > 0)
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.IdIndicador == obj.IdIndicador);
                    }

                    if (!string.IsNullOrEmpty(obj.nomeVendedor))
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.VendedorResponsavel == obj.nomeVendedor);
                    }
                    if (obj.ativo != null)
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.Ativo == obj.ativo);
                    }

                    return vendedorParceiro.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TorcamentistaEIndicadorVendedor> BuscarVendedorParceirosPorParceiros(TorcamentistaEIndicadorVendedorFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var vendedorParceiro = from usr in db.TorcamentistaEIndicadorVendedor
                                           join par in db.TorcamentistaEindicador on usr.IdIndicador equals par.IdIndicador
                                           where
                                                par.Status == Constantes.ORCAMENTISTA_INDICADOR_STATUS_ATIVO
                                                && usr.Ativo == true
                                                && obj.Parceiros.Contains(par.Apelido)
                                           orderby usr.Nome
                                           select new TorcamentistaEIndicadorVendedor()
                                           {
                                               Id = usr.Id,
                                               Nome = usr.Nome,
                                               Email = usr.Email,
                                               Datastamp = UtilsGlobais.SenhaBll.DecodificaSenha(usr.Datastamp),
                                               Senha = usr.Senha,
                                               IdIndicador = usr.IdIndicador,
                                               Telefone = usr.Telefone,
                                               Celular = usr.Celular,
                                               Ativo = usr.Ativo,
                                               UsuarioCadastro = usr.UsuarioCadastro,
                                               DataUltimaAlteracaoSenha = usr.DataUltimaAlteracaoSenha,
                                               UsuarioUltimaAlteracao = usr.UsuarioUltimaAlteracao,
                                               DataCadastro = usr.DataCadastro,
                                               DataUltimaAlteracao = usr.DataUltimaAlteracao,
                                               Loja = par.Loja,
                                               VendedorResponsavel = par.Vendedor,
                                               Parceiro = par.Apelido,
                                               StLoginBloqueadoAutomatico = usr.StLoginBloqueadoAutomatico
                                           };

                    return vendedorParceiro.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Object ListarOrcamentistaVendedor(TorcamentistaEIndicadorVendedorFiltro obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var saida = from usr in db.TorcamentistaEIndicadorVendedor
                            join par in db.TorcamentistaEindicador on usr.IdIndicador equals par.IdIndicador
                            join ven in db.Tusuario on par.Vendedor equals ven.Usuario
                            select new TorcamentistaEIndicadorVendedor()
                            {
                                Id = usr.Id,
                                Nome = usr.Nome,
                                Email = usr.Email,
                                IdIndicador = usr.IdIndicador,
                                Ativo = usr.Ativo,
                                Loja = par.Loja,
                                IdVendedor = ven.Id,
                                VendedorResponsavel = par.Vendedor,
                                Parceiro = par.Apelido,
                                StLoginBloqueadoAutomatico = usr.StLoginBloqueadoAutomatico,
                                DataCadastro = usr.DataCadastro,
                                StringBusca = $"|{usr.Nome}|{usr.Email}|"
                            };

                if (obj.ativo != null)
                {
                    saida = saida.Where(x => x.Ativo == obj.ativo);
                }
                if (obj.Bloqueado != null)
                {
                    saida = saida.Where(x => x.StLoginBloqueadoAutomatico == (obj.Bloqueado == 1 ? true : false));
                }
                if (!string.IsNullOrEmpty(obj.loja))
                {
                    saida = saida.Where(x => x.Loja == obj.loja);
                }
                if (obj.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR && !string.IsNullOrEmpty(obj.nomeVendedor))
                {
                    saida = saida.Where(x => x.VendedorResponsavel == obj.nomeVendedor);
                }
                if (!string.IsNullOrEmpty(obj.Parceiro))
                {
                    saida = saida.Where(x => x.Parceiro == obj.Parceiro);
                }
                if (obj.Vendedores != null && obj.Vendedores.Count() > 0)
                {
                    saida = saida.Where(x => obj.Vendedores.Contains(x.IdVendedor));
                }
                if (obj.Parceiros != null && obj.Parceiros.Count() > 0)
                {
                    saida = saida.Where(x => obj.Parceiros.Contains(x.Parceiro));
                }



                if (!string.IsNullOrWhiteSpace(obj.NomeColuna))
                {
                    switch (obj.NomeColuna.ToUpper())
                    {
                        case "PARCEIRO":
                            if (obj.OrdenacaoAscendente)
                            {
                                saida = saida.OrderBy(o => o.Parceiro);
                            }
                            else
                            {
                                saida = saida.OrderByDescending(o => o.Parceiro);
                            }
                            break;
                        case "NOME":
                            if (obj.OrdenacaoAscendente)
                            {
                                saida = saida.OrderBy(o => o.Nome);
                            }
                            else
                            {
                                saida = saida.OrderByDescending(o => o.Nome);
                            }
                            break;

                        case "E-MAIL":
                            if (obj.OrdenacaoAscendente)
                            {
                                saida = saida.OrderBy(o => o.Email);
                            }
                            else
                            {
                                saida = saida.OrderByDescending(o => o.Email);
                            }
                            break;

                        case "RESPONSAVEL":
                            if (obj.OrdenacaoAscendente)
                            {
                                saida = saida.OrderBy(o => o.VendedorResponsavel);
                            }
                            else
                            {
                                saida = saida.OrderByDescending(o => o.VendedorResponsavel);
                            }
                            break;

                        case "ATIVO":
                            if (obj.OrdenacaoAscendente)
                            {
                                saida = saida.OrderBy(o => o.Ativo ? 1 : 0);
                            }
                            else
                            {
                                saida = saida.OrderByDescending(o => o.Ativo ? 1 : 0);
                            }
                            break;

                        case "BLOQUEADO":
                            if (obj.OrdenacaoAscendente)
                            {
                                saida = saida.OrderBy(o => o.StLoginBloqueadoAutomatico);
                            }
                            else
                            {
                                saida = saida.OrderByDescending(o => o.StLoginBloqueadoAutomatico);
                            }
                            break;

                        case "DATACRIACAO":
                            if (obj.OrdenacaoAscendente)
                            {
                                saida = saida.OrderBy(o => o.DataCadastro);
                            }
                            else
                            {
                                saida = saida.OrderByDescending(o => o.DataCadastro);
                            }
                            break;
                    }
                }
                else
                {
                    saida = saida.OrderBy(o => o.Nome);
                }

                var response = saida.ToList();

                var idsUsuario = response.Select(x => x.Id).ToList();
                var loginHisto = (from c in db.TloginHistorico
                                 where idsUsuario.Contains((int)c.IdUsuario) &&
                                       c.StSucesso == true
                                 select c).ToList();
                if (loginHisto != null)
                {
                    foreach (var i in response)
                    {
                        var login = loginHisto.Where(x => x.IdUsuario == i.Id).OrderByDescending(x => x.DataHora).FirstOrDefault();
                        if (login != null)
                        {
                            i.StSucesso = true;
                            i.UltimoLogin = login.DataHora;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(obj.NomeColuna))
                {
                    if (obj.NomeColuna.ToUpper() == "ULTIMOLOGIN")
                    {
                        if (obj.OrdenacaoAscendente)
                        {
                            response = response.OrderBy(o => o.UltimoLogin).ToList();
                        }
                        else
                        {
                            response = response.OrderByDescending(o => o.UltimoLogin).ToList();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(obj.Pesquisa))
                {
                    response = response.Where(x => x.StringBusca.ToLower().Contains(obj.Pesquisa.ToLower())).ToList();
                }

                var qtdeRegistros = response.Count;

                response = response.Skip(obj.Pagina * obj.QtdeItensPagina).Take(obj.QtdeItensPagina).ToList();

                var retorno = new
                {
                    QtdeRegistros = qtdeRegistros,
                    Lista = response
                };



                return retorno;
            }
        }
    }
}