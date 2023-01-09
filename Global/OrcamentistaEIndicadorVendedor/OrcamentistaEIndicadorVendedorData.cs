using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            throw new NotImplementedException();
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
                                               Parceiro = par.Apelido
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
                    if(obj.ativo != null)
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
    }
}
