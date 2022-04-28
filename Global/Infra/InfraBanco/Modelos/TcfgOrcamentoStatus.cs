using System.Collections.Generic;
using System.Linq;

namespace InfraBanco.Modelos
{
    public class TcfgOrcamentoStatus
    {
        public static List<TcfgSelectItem> ObterLista()
        {
            List<TcfgSelectItem> lista = new List<TcfgSelectItem>();

            lista.Add(new TcfgSelectItem { Id = "ESP", Value = "Em espera" });
            lista.Add(new TcfgSelectItem { Id = "SPL", Value = "Split Possível" });
            lista.Add(new TcfgSelectItem { Id = "SEP", Value = "Separar Mercadoria" });
            lista.Add(new TcfgSelectItem { Id = "AET", Value = "A entregar" });
            lista.Add(new TcfgSelectItem { Id = "ETG", Value = "Entregue" });
            lista.Add(new TcfgSelectItem { Id = "CAN", Value = "Cancelado" });

            return lista;
        }

        public static string ObterStatus(string status)
        {
            if (!string.IsNullOrEmpty(status))
                return ObterLista().FirstOrDefault(x => x.Id == status).Value;
            else
                return "";
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
