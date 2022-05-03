using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoOpcaoItemUnificado
{
    public class OrcamentoCotacaoOpcaoItemUnificadoData : BaseData<TorcamentoCotacaoItemUnificado, TorcamentoCotacaoOpcaoItemUnificadoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoOpcaoItemUnificadoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoItemUnificado Atualizar(TorcamentoCotacaoItemUnificado obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentoCotacaoItemUnificado obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoItemUnificado Inserir(TorcamentoCotacaoItemUnificado obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoItemUnificado InserirComTransacao(TorcamentoCotacaoItemUnificado model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentoCotacaoItemUnificado> PorFilroComTransacao(TorcamentoCotacaoOpcaoItemUnificadoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoItemUnificado> PorFiltro(TorcamentoCotacaoOpcaoItemUnificadoFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = from c in db.TorcamentoCotacaoItemUnificados
                                select c;

                    if(obj.IdOpcao != 0)
                    {
                        saida = saida.Where(x => x.IdOrcamentoCotacaoOpcao == obj.IdOpcao);
                    }

                    return saida.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
