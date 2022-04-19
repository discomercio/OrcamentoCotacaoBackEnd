using ClassesBase;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arquivo
{
    public class ArquivoBll : BaseData<TorcamentoCotacaoArquivos, TorcamentoCotacaoArquivosFiltro>
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        public ArquivoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoArquivos Atualizar(TorcamentoCotacaoArquivos obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    db.TorcamentoCotacaoArquivos.Update(obj);
                    db.SaveChanges();
                    return obj;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TorcamentoCotacaoArquivos Inserir(TorcamentoCotacaoArquivos obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    db.TorcamentoCotacaoArquivos.Add(obj);
                    db.SaveChanges();
                    db.transacao.Commit();
                    return obj;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Editar(TorcamentoCotacaoArquivos obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var arquivo = db.TorcamentoCotacaoArquivos.First(x => x.Id == obj.Id);
                    arquivo.Nome = obj.Nome;
                    arquivo.Descricao = obj.Descricao;
                    db.SaveChanges();
                    db.transacao.Commit();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<TorcamentoCotacaoArquivos> PorFiltro(TorcamentoCotacaoArquivosFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = (from orcamentoCotacaoArquivos in db.TorcamentoCotacaoArquivos
                                 select orcamentoCotacaoArquivos);

                    if (obj.id.HasValue)
                    {
                        saida = saida.Where(x => x.Id == obj.id);
                    }

                    return saida.ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public List<TorcamentoCotacaoArquivos> ObterEstrutura()
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    return (from orcamentoCotacaoArquivos in db.TorcamentoCotacaoArquivos
                     select orcamentoCotacaoArquivos)
                    .ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TorcamentoCotacaoArquivos ObterArquivoPorID(Guid id)
        {
            return PorFiltro(new TorcamentoCotacaoArquivosFiltro() { id = id }).FirstOrDefault();
        }

        public bool Excluir(TorcamentoCotacaoArquivos obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var objeto = db.TorcamentoCotacaoArquivos.FirstOrDefault(x => x.Id == obj.Id);

                    if (objeto != null)
                    {
                        db.TorcamentoCotacaoArquivos.Remove(objeto);
                        db.SaveChanges();
                        db.transacao.Commit();
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public TorcamentoCotacaoArquivos InserirComTransacao(TorcamentoCotacaoArquivos model, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoArquivos> PorFilroComTransacao(TorcamentoCotacaoArquivosFiltro obj, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }
    }
}
