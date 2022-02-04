using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Loja
{
    public class LojaBll //: BaseData<Tloja, TlojaFiltro>
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        public LojaBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public List<Tloja> PorFiltro(TlojaFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var lojas = from L in db.Tlojas
                                orderby L.Nome
                                select L;

                    //if (!string.IsNullOrEmpty(obj.Loja))
                    //{
                    //    lojas = lojas.Where(x => x.Loja == obj.Loja);
                    //}

                    return lojas.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
