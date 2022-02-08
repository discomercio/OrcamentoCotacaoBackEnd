using InfraBanco;
using Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        //public virtual void Activate(I id)
        //{
        //    data.Activate(id);
        //}
        //public virtual void Activate(List<I> list)
        //{
        //    data.Activate(list);
        //}
        public virtual void Atualizar(T model)
        {
            data.Atualizar(model);
          //  logger.LogDebug(JsonConvert.SerializeObject(model, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            //new LogActionsData().Insert(new LogAction { ContentData = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), eOperation = Enums.eOperationLog.Update, InsertedBy = model.InsertedBy, Origin = this.GetType().Name });
        }
        public virtual void Excluir(T model)
        {
            data.Excluir(model);
            //logger.LogDebug(JsonConvert.SerializeObject(model, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            //new LogActionsData().Insert(new LogAction { ContentData = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), eOperation = Enums.eOperationLog.Delete, InsertedBy = model.InsertedBy, Origin = this.GetType().Name });
        }
        //public virtual void Inactivate(I id)
        //{
        //    data.Inactivate(id);
        //}
        //public virtual void Inactivate(List<I> list)
        //{
        //    data.Inactivate(list);
        //}
        public virtual void Inserir(T model)
        {
            data.Inserir(model);
        //    logger.LogDebug(JsonConvert.SerializeObject(model, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            //new LogActionsData().Insert(new LogAction { ContentData = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), eOperation = Enums.eOperationLog.Insert, InsertedBy = model.InsertedBy, Origin = this.GetType().Name });
        }
        //public virtual List<T> List()
        //{
        //    return data.ListList();
        //}

        public virtual List<T> PorFiltro(F filter)
        {
            return data.PorFiltro(filter);
        }
        //public virtual T GetById(I id)
        //{
        //    return data.Por.GetById(id);
        //}
    }
}
