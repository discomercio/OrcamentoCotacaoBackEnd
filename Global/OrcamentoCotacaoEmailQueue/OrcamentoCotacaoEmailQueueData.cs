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

namespace OrcamentoCotacaoEmailQueue
{
    public class OrcamentoCotacaoEmailQueueData : BaseData<TorcamentoCotacaoEmailQueue, TorcamentoCotacaoEmailQueueFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoEmailQueueData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoEmailQueue Atualizar(TorcamentoCotacaoEmailQueue obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                db.TorcamentoCotacaoEmailQueue.Update(obj);
                db.SaveChanges();
                db.transacao.Commit();
                return obj;
            }
        }

        public TorcamentoCotacaoEmailQueue AtualizarComTransacao(TorcamentoCotacaoEmailQueue model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        //public List<TcfgUnidadeNegocioParametro> GetCfgUnidadeNegocioParametros(string nomeLoja)
        //{

        //    var idCfgUnidadeNegocio = GetCfgUnidadeNegocio(nomeLoja);

        //    List<TcfgUnidadeNegocioParametro> lista = null;

        //    using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
        //    {
        //        var negocioParametros = from u in db.TcfgUnidadeNegocio
        //                                join p in db.TcfgUnidadeNegocioParametro on u.Id equals p.IdCfgUnidadeNegocio into pl
        //                                from parametro in pl
        //                                .Where(x => x.IdCfgUnidadeNegocio == idCfgUnidadeNegocio)
        //                                select new TcfgUnidadeNegocioParametro
        //                                {
        //                                    Id = parametro.Id,
        //                                    IdCfgUnidadeNegocio = parametro.IdCfgUnidadeNegocio,
        //                                    IdCfgParametro = parametro.IdCfgParametro,
        //                                    Valor = parametro.Valor
        //                                };

        //        lista = negocioParametros.ToList();

        //    }

        //    return lista;
        //}

        //private int GetCfgUnidadeNegocio(string nomeLoja)
        //{

        //    List<TcfgUnidadeNegocio> lista = null;

        //    using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
        //    {
        //        var cfgUnidadeNegocios = from u in db.TcfgUnidadeNegocio
        //                                .Where(x => x.NomeCurto == nomeLoja)
        //                                 select new TcfgUnidadeNegocio
        //                                 {
        //                                     Id = u.Id
        //                                 };

        //        lista = cfgUnidadeNegocios.ToList();

        //    }

        //    var IdCfgUnidadeNegocio = lista[0].Id;

        //    return IdCfgUnidadeNegocio;
        //}



        //public bool AdicionarQueue(
        //            string emailTemplateBody,
        //            TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueue)
        //{
        //    var saida = false;


        //    return saida;

        //}

        public bool Excluir(TorcamentoCotacaoEmailQueue obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TorcamentoCotacaoEmailQueue obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoEmailQueue Inserir(TorcamentoCotacaoEmailQueue obj)
        {
            
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    db.TorcamentoCotacaoEmailQueue.Add(obj);
                    //db.TorcamentoCotacaoEmailQueue.Add(
                    //    new InfraBanco.Modelos.TorcamentoCotacaoEmailQueue
                    //    {

                    //        IdCfgUnidadeNegocio = obj.IdCfgUnidadeNegocio,
                    //        From = obj.From,
                    //        FromDisplayName = obj.FromDisplayName,
                    //        To = obj.To,
                    //        Cc = obj.Cc,
                    //        Bcc = obj.Bcc,
                    //        Subject = obj.Subject,
                    //        Body = obj.Body,
                    //        Sent = obj.Sent,
                    //        DateSent = obj.DateSent,
                    //        DateScheduled = obj.DateScheduled,
                    //        DateCreated = DateTime.Now,
                    //        Status = obj.Status,
                    //        AttemptsQty = obj.AttemptsQty,
                    //        DateLastAttempt = obj.DateLastAttempt,
                    //        ErrorMsgLastAttempt = obj.ErrorMsgLastAttempt,
                    //        Attachment = obj.ErrorMsgLastAttempt
                    //    }); ;

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

        public TorcamentoCotacaoEmailQueue InserirComTransacao(TorcamentoCotacaoEmailQueue model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoEmailQueue> PorFilroComTransacao(TorcamentoCotacaoEmailQueueFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoEmailQueue> PorFiltro(TorcamentoCotacaoEmailQueueFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var parametros = from L in db.TorcamentoCotacaoEmailQueue
                                     select L;

                    if (obj.Sent.HasValue)
                    {
                        if (!obj.Sent.Value)
                            parametros = parametros.Where(x => x.Sent == obj.Sent || x.Sent == null);
                        else
                            parametros = parametros.Where(x => x.Sent == obj.Sent);
                    }

                    parametros = parametros.Where(x => x.DateScheduled < DateTime.Now || x.DateScheduled == null);

                    if (obj.Page.HasValue || obj.RecordsPerPage.HasValue)
                    {
                        parametros = parametros.Skip(obj.RecordsPerPage.Value * (obj.Page.Value - 1)).Take(obj.RecordsPerPage.Value);
                    }

                    parametros = parametros.OrderBy(x => x.DateScheduled);

                    return parametros.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
