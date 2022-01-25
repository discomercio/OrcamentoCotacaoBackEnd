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

        public bool Excluir(TorcamentoCotacaoArquivos obj)
        {
            throw new NotImplementedException();
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
                    var saida = from orcamentoCotacaoArquivos in db.TorcamentoCotacaoArquivos
                                select orcamentoCotacaoArquivos;
                    //string query = "select id, nome, tamanho, tipo, pai, descricao from [dbo].[ORCAMENTO_COTACAO_ARQUIVOS]";
                    //var queryParameters = new DynamicParameters();

                    //saida = dbConn.Query<EstruturaVO>(
                    //    query,
                    //    queryParameters,
                    //    commandType: CommandType.Text
                    //    ).ToList();
                    return saida.ToList();
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
    }
}
