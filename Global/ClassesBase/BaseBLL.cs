using InfraBanco;
using Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassesBase
{
    public class BaseBLL<T, F> : IBusinessObject<T, F> where T : IModel where F : IFilter
    {
        public readonly BaseData<T, F> data;
        private readonly ILogger logger;

        public BaseBLL(BaseData<T,F> _data) //, ILogger logger
        {
            data = _data;
            //this.logger = logger;
        }

        public virtual void Atualizar(T model)
        {
            data.Atualizar(model);
        }

        public virtual void Excluir(T model)
        {
            data.Excluir(model);
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
    }
}
