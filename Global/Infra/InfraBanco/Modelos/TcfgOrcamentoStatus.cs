using System.Collections.Generic;
using System.Linq;

namespace InfraBanco.Modelos
{
    public class TcfgOrcamentoStatus
    {
        public static List<TcfgSelectItem> ObterLista()
        {
            List<TcfgSelectItem> status = new List<TcfgSelectItem>();

            //status.Add(new TcfgSelectItem { Id = "ESP", Value = "Em espera" });
            //status.Add(new TcfgSelectItem { Id = "SPL", Value = "Split Possível" });
            //status.Add(new TcfgSelectItem { Id = "SEP", Value = "Separar Mercadoria" });
            //status.Add(new TcfgSelectItem { Id = "AET", Value = "A entregar" });
            //status.Add(new TcfgSelectItem { Id = "ETG", Value = "Entregue" });
            status.Add(new TcfgSelectItem { Id = "CAN", Value = "Cancelado" });

            return status;
        }

        public static List<TcfgSelectItem> ObterLista(List<string> listaStatus)
        {
            List<TcfgSelectItem> status = new List<TcfgSelectItem>();
            TcfgSelectItem item = null;

            foreach (var s in listaStatus)
            {
                item = ObterLista().FirstOrDefault(x => x.Id == s);

                if (item != null)
                {
                    status.Add(item);
                }
            }

            return status;
        }
    }
}
