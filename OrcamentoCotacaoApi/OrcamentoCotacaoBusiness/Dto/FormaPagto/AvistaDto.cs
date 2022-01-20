using FormaPagamento.Dados;
using System.Collections.Generic;

namespace PrepedidoBusiness.Dto.FormaPagto
{
    public class AvistaDto
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }

        public static AvistaDto AvistaDto_De_AvistaDados(AvistaDados origem)
        {
            if (origem == null) return null;
            return new AvistaDto()
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                Ordenacao = origem.Ordenacao
            };
        }
        public static List<AvistaDto> ListaAvistaDto_De_AvistaDados(IEnumerable<AvistaDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<AvistaDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(AvistaDto_De_AvistaDados(p));
            return ret;
        }
    }
}
