using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InfraBanco.ContextoBdGravacao;

namespace Cfg.CfgOrcamentoCotacaoEndpointFilter
{
    public class TcfgOrcamentoCotacaoEndpointFilterData : BaseData<TcfgOrcamentoCotacaoEndpointFilter, TcfgOrcamentoCotacaoEndpointFilterFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public TcfgOrcamentoCotacaoEndpointFilterData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TcfgOrcamentoCotacaoEndpointFilter Atualizar(TcfgOrcamentoCotacaoEndpointFilter obj)
        {
            throw new NotImplementedException();
        }

        public TcfgOrcamentoCotacaoEndpointFilter AtualizarComTransacao(TcfgOrcamentoCotacaoEndpointFilter model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcfgOrcamentoCotacaoEndpointFilter obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TcfgOrcamentoCotacaoEndpointFilter obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TcfgOrcamentoCotacaoEndpointFilter Inserir(TcfgOrcamentoCotacaoEndpointFilter obj)
        {
            throw new NotImplementedException();
        }

        public TcfgOrcamentoCotacaoEndpointFilter InserirComTransacao(TcfgOrcamentoCotacaoEndpointFilter model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TcfgOrcamentoCotacaoEndpointFilter.Add(model);
            contextoBdGravacao.SaveChanges();

            return model;
        }

        public List<TcfgOrcamentoCotacaoEndpointFilter> PorFilroComTransacao(TcfgOrcamentoCotacaoEndpointFilterFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgOrcamentoCotacaoEndpointFilter> PorFiltro(TcfgOrcamentoCotacaoEndpointFilterFiltro obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
            {
                var query = from c in db.TcfgOrcamentoCotacaoEndpointFilter
                            select c;

                return query.ToList();
            }
        }
    }
}
