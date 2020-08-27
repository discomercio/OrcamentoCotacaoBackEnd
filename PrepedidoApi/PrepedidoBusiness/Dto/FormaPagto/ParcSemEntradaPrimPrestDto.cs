using Prepedido.Dados.FormaPagto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.FormaPagto
{
    public class ParcSemEntradaPrimPrestDto
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }
        public static ParcSemEntradaPrimPrestDto ParcSemEntradaPrimPrestDto_De_ParcSemEntradaPrimPrestDados(ParcSemEntradaPrimPrestDados origem)
        {
            if (origem == null) return null;
            return new ParcSemEntradaPrimPrestDto()
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                Ordenacao = origem.Ordenacao
            };
        }
        public static List<ParcSemEntradaPrimPrestDto> ListaParcSemEntradaPrimPrestDto_De_ParcSemEntradaPrimPrestDados(IEnumerable<ParcSemEntradaPrimPrestDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<ParcSemEntradaPrimPrestDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(ParcSemEntradaPrimPrestDto_De_ParcSemEntradaPrimPrestDados(p));
            return ret;
        }
    }
}
