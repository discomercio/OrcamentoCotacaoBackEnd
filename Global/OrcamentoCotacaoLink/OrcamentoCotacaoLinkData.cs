using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OrcamentoCotacaoLink
{
    public class OrcamentoCotacaoLinkData : BaseData<TorcamentoCotacaoLink, TorcamentoCotacaoLinkFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoLinkData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoLink Atualizar(TorcamentoCotacaoLink obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                db.TorcamentoCotacaoLink.Update(obj);
                db.SaveChanges();
                db.transacao.Commit();
                return obj;
            }
        }

        public TorcamentoCotacaoLink InserirComTransacao(TorcamentoCotacaoLink obj, ContextoBdGravacao contextoBdGravacao)
        {

            contextoBdGravacao.TorcamentoCotacaoLink.Add(obj);
            contextoBdGravacao.SaveChanges();
            return obj;
        }

        public TorcamentoCotacaoLink Inserir(TorcamentoCotacaoLink obj)
        {
            
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    db.TorcamentoCotacaoLink.Add(obj);
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

        bool BaseData<TorcamentoCotacaoLink, TorcamentoCotacaoLinkFiltro>.Excluir(TorcamentoCotacaoLink obj)
        {
            throw new NotImplementedException();
        }

        TorcamentoCotacaoLink BaseData<TorcamentoCotacaoLink, TorcamentoCotacaoLinkFiltro>.InserirComTransacao(TorcamentoCotacaoLink model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        List<TorcamentoCotacaoLink> BaseData<TorcamentoCotacaoLink, TorcamentoCotacaoLinkFiltro>.PorFilroComTransacao(TorcamentoCotacaoLinkFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoLink> PorFiltro(TorcamentoCotacaoLinkFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoLeitura())
                {

                    var saida = from c in db.TorcamentoCotacaoLink
                                select c;

                    if (obj.IdOrcamentoCotacao != 0)
                    {
                        saida = saida.Where(x => x.IdOrcamentoCotacao == obj.IdOrcamentoCotacao);
                    }

                    return saida.ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public TorcamentoCotacaoLink AtualizarComTransacao(TorcamentoCotacaoLink model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TorcamentoCotacaoLink obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }
    }
}
