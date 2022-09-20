using FormaPagamento.Dados;
using System.Collections.Generic;

namespace Prepedido.Dto
{
    public class ParcComEntPrestacaoDto
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }


        public static ParcComEntPrestacaoDto ParcComEntPrestacaoDto_De_ParcComEntPrestacaoDados(ParcComEntPrestacaoDados origem)
        {
            if (origem == null) return null;
            return new ParcComEntPrestacaoDto()
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                Ordenacao = origem.Ordenacao
            };
        }
        public static List<ParcComEntPrestacaoDto> ListaParcComEntPrestacaoDto_De_ParcComEntPrestacaoDados(IEnumerable<ParcComEntPrestacaoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<ParcComEntPrestacaoDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(ParcComEntPrestacaoDto_De_ParcComEntPrestacaoDados(p));
            return ret;
        }
    }
}