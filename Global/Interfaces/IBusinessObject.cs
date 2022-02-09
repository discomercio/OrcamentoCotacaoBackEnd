using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface IBusinessObject<T, F> where T : IModel where F : IFilter
    {
        //void Activate(I id);
        //void Activate(List<I> list);
        void Atualizar(T model);
        void Excluir(T model);
        void Inserir(T model);
        //List<T> List();
        List<T> PorFiltro(F model);
        //T GetById(I id);
    }
}
