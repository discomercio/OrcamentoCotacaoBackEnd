using InfraBanco.Modelos;
using System.Collections.Generic;

namespace FormaPagamento.Dados
{
    public class MeioPagamentoDados
    {
        public short Id { get; set; }
        public short? IdTipoParcela { get; set; }
        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }
        public int? QtdeMaxParcelas { get; set; }
        public int? QtdeMaxDias { get; set; }

        public static MeioPagamentoDados MeioPagamentoDados_De_TcfgPagtoMeioStatus(TcfgPagtoMeioStatus origem)
        {
            if (origem == null) return null;
            return new MeioPagamentoDados()
            {
                Id = origem.TcfgPagtoMeio.Id,
                Descricao = origem.TcfgPagtoMeio.Descricao,
                Ordenacao = origem.TcfgPagtoMeio.Ordenacao,
                QtdeMaxParcelas = origem.QtdeMaxParcelas,
                QtdeMaxDias = origem.QtdeMaxDias,
                IdTipoParcela = origem.IdCfgTipoParcela
            };
        }
        public static List<MeioPagamentoDados> ListaMeioPagamentoDados_De_TcfgPagtoMeioStatus(List<TcfgPagtoMeioStatus> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<MeioPagamentoDados>();
            if (listaBancoDados != null)
                foreach (var item in listaBancoDados)
                    ret.Add(MeioPagamentoDados_De_TcfgPagtoMeioStatus(item));
            return ret;
        }
    }
}
