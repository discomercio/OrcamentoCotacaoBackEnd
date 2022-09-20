using FormaPagamento.Dados;
using System.Collections.Generic;

namespace Prepedido.Dto
{
    public class ParcSemEntPrestacaoDto
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }

        public static ParcSemEntPrestacaoDto ParcSemEntPrestacaoDto_De_ParcSemEntPrestacaoDados(ParcSemEntPrestacaoDados origem)
        {
            if (origem == null) return null;
            return new ParcSemEntPrestacaoDto()
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                Ordenacao = origem.Ordenacao
            };
        }
        public static List<ParcSemEntPrestacaoDto> ListaParcSemEntPrestacaoDto_De_ParcSemEntPrestacaoDados(IEnumerable<ParcSemEntPrestacaoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<ParcSemEntPrestacaoDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(ParcSemEntPrestacaoDto_De_ParcSemEntPrestacaoDados(p));
            return ret;
        }
    }
}
