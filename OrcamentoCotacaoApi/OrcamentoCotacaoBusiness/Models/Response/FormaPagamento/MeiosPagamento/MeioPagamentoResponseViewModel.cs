using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.FormaPagamento.MeiosPagamento
{
    public class MeioPagamentoResponseViewModel
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }
        public int? QtdeMaxParcelas { get; set; }
        public int? QtdeMaxDias { get; set; }

        public static MeioPagamentoResponseViewModel MeioPagamentoResponseViewModel_De_TcfgPagtoMeioStatus(InfraBanco.Modelos.TcfgPagtoMeioStatus origem)
        {
            if (origem == null) return null;
            return new MeioPagamentoResponseViewModel()
            {
                Id = origem.TcfgPagtoMeio.Id,
                Descricao = origem.TcfgPagtoMeio.Descricao,
                Ordenacao = origem.TcfgPagtoMeio.Ordenacao,
                QtdeMaxParcelas = origem.QtdeMaxParcelas,
                QtdeMaxDias = origem.QtdeMaxDias
            };
        }
        public static List<MeioPagamentoResponseViewModel> ListaMeioPagamentoResponseViewModel_De_TcfgPagtoMeioStatus(List<InfraBanco.Modelos.TcfgPagtoMeioStatus> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<MeioPagamentoResponseViewModel>();
            if (listaBancoDados != null)
                foreach (var item in listaBancoDados)
                    ret.Add(MeioPagamentoResponseViewModel_De_TcfgPagtoMeioStatus(item));
            return ret;
        }
    }
}
