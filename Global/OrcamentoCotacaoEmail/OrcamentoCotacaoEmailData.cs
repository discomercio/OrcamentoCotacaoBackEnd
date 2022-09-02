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

namespace OrcamentoCotacaoEmail
{
    public class OrcamentoCotacaoEmailData : BaseData<TorcamentoCotacaoEmail, TorcamentoCotacaoEmailFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoEmailData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoEmail Atualizar(TorcamentoCotacaoEmail obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoEmail AtualizarComTransacao(TorcamentoCotacaoEmail model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentoCotacaoEmail obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TorcamentoCotacaoEmail obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoEmail Inserir(TorcamentoCotacaoEmail obj)
        {

            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    db.TorcamentoCotacaoEmail.Add(obj);                    
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

        public TorcamentoCotacaoEmail InserirComTransacao(TorcamentoCotacaoEmail model, ContextoBdGravacao contextoBdGravacao)
        {
            try
            {
                contextoBdGravacao.TorcamentoCotacaoEmail.Add(model);
                contextoBdGravacao.SaveChanges();
                return model;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TorcamentoCotacaoEmail> PorFilroComTransacao(TorcamentoCotacaoEmailFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoEmail> PorFiltro(TorcamentoCotacaoEmailFiltro obj)
        {
            throw new NotImplementedException();
        }
    }
}
