using FormaPagamento.Dados;
using System.Collections.Generic;

namespace PrepedidoBusiness.Dto.FormaPagto
{
    public class ParcUnicaDto
    {
        public short Id { get; set; }

        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }
        public static ParcUnicaDto ParcUnicaDto_De_ParcUnicaDados(ParcUnicaDados origem)
        {
            if (origem == null) return null;
            return new ParcUnicaDto()
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                Ordenacao = origem.Ordenacao
            };
        }
        public static List<ParcUnicaDto> ListaParcUnicaDto_De_ParcUnicaDados(IEnumerable<ParcUnicaDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<ParcUnicaDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(ParcUnicaDto_De_ParcUnicaDados(p));
            return ret;
        }
    }
}
