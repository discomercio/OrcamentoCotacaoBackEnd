using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoOpcaoPagto
{
    public class OrcamentoCotacaoOpcaoPagtoData : BaseData<TorcamentoCotacaoOpcaoPagto, TorcamentoCotacaoOpcaoPagtoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoOpcaoPagtoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacaoOpcaoPagto Atualizar(TorcamentoCotacaoOpcaoPagto obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentoCotacaoOpcaoPagto obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcaoPagto Inserir(TorcamentoCotacaoOpcaoPagto obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcaoPagto InserirComTransacao(TorcamentoCotacaoOpcaoPagto model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentoCotacaoOpcaoPagto> PorFilroComTransacao(TorcamentoCotacaoOpcaoPagtoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoOpcaoPagto> PorFiltro(TorcamentoCotacaoOpcaoPagtoFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoLeitura())
                {

                    var saida = from c in db.TorcamentoCotacaoOpcaoPgtos
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

                throw;
            }
        }
    }
}
