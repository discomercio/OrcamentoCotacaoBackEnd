using FormaPagamento.Dados;
using System.Collections.Generic;

namespace PrepedidoBusiness.Dto.FormaPagto
{
    public class ParcComEntPrestacaoDto
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }
        public int QtdeMaxParcelas { get; set; }
        public int QtdeMaxDias { get; set; }


        public static ParcComEntPrestacaoDto ParcComEntPrestacaoDto_De_TcfgPagtoMeioStatus(InfraBanco.Modelos.TcfgPagtoMeioStatus origem)
        {
            if (origem == null) return null;
            return new ParcComEntPrestacaoDto()
            {
                Id = origem.TcfgPagtoMeio.Id,
                Descricao = origem.TcfgPagtoMeio.Descricao,
                Ordenacao = origem.TcfgPagtoMeio.Ordenacao,
                QtdeMaxParcelas = (short)origem.QtdeMaxParcelas,
                QtdeMaxDias = (short)origem.QtdeMaxDias
            };
        }
        public static List<ParcComEntPrestacaoDto> ListaParcComEntPrestacaoDto_De_TcfgPagtoMeioStatus(List<InfraBanco.Modelos.TcfgPagtoMeioStatus> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<ParcComEntPrestacaoDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(ParcComEntPrestacaoDto_De_TcfgPagtoMeioStatus(p));
            return ret;
        }
    }
}