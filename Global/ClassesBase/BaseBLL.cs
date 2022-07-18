using InfraBanco;
using Interfaces;
using System.Collections.Generic;

namespace ClassesBase
{
    public class BaseBLL<T, F> : IBusinessObject<T, F> where T : IModel where F : IFilter
    {
        public readonly BaseData<T, F> data;

        public BaseBLL(BaseData<T,F> _data) //, ILogger logger
        {
            data = _data;
        }

        public virtual void Atualizar(T model)
        {
            data.Atualizar(model);
        }

        public virtual T AtualizarComTransacao(T model, ContextoBdGravacao contextoBdGravacao)
        {
            return data.AtualizarComTransacao(model, contextoBdGravacao);
        }

        public virtual void Excluir(T model)
        {
            data.Excluir(model);
        }

        public virtual void ExcluirComTransacao(T model, ContextoBdGravacao contextoBdGravacao)
        {
            data.ExcluirComTransacao(model, contextoBdGravacao);
        }

        public virtual void Inserir(T model)
        {
            data.Inserir(model);
        }
        
        public virtual T InserirComTransacao(T model, ContextoBdGravacao contextoBdGravacao)
        {
            return data.InserirComTransacao(model, contextoBdGravacao);
        }

        public virtual List<T> PorFiltro(F filter)
        {
            return data.PorFiltro(filter);
        }

        public virtual List<T> PorFiltroComTransacao(F filter, ContextoBdGravacao contextoBdGravacao)
        {
            return data.PorFilroComTransacao(filter, contextoBdGravacao);
        }
    }
}
