using FormaPagamento.Dados;
using System.Collections.Generic;

namespace Prepedido.Dto
{
    public class ParcComEntradaDto
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }

        public static ParcComEntradaDto ParcComEntradaDto_De_ParcComEntradaDados(ParcComEntradaDados origem)
        {
            if (origem == null) return null;
            return new ParcComEntradaDto()
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                Ordenacao = origem.Ordenacao
            };
        }
        public static List<ParcComEntradaDto> ListaParcComEntradaDto_De_ParcComEntradaDados(IEnumerable<ParcComEntradaDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<ParcComEntradaDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(ParcComEntradaDto_De_ParcComEntradaDados(p));
            return ret;
        }
    }
}
