using FormaPagamento.Dados;
using System.Collections.Generic;

namespace PrepedidoBusiness.Dto.FormaPagto
{
    //public class ParcSemEntradaPrimPrestDto
    //{
    //    public short Id { get; set; }
    //    public string Descricao { get; set; }
    //    public int? Ordenacao { get; set; }
    //    public int QtdeMaxDias { get; set; }
    //    public static ParcSemEntradaPrimPrestDto ParcSemEntradaPrimPrestDto_De_TcfgPagtoMeioStatus(InfraBanco.Modelos.TcfgPagtoMeioStatus origem)
    //    {
    //        if (origem == null) return null;
    //        return new ParcSemEntradaPrimPrestDto()
    //        {
    //            Id = origem.TcfgPagtoMeio.Id,
    //            Descricao = origem.TcfgPagtoMeio.Descricao,
    //            Ordenacao = origem.TcfgPagtoMeio.Ordenacao,
    //            QtdeMaxDias = (short)origem.QtdeMaxDias
    //        };
    //    }
    //    public static List<ParcSemEntradaPrimPrestDto> ListaParcSemEntradaPrimPrestDto_De_TcfgPagtoMeioStatus(List<InfraBanco.Modelos.TcfgPagtoMeioStatus> listaBancoDados)
    //    {
    //        if (listaBancoDados == null) return null;
    //        var ret = new List<ParcSemEntradaPrimPrestDto>();
    //        if (listaBancoDados != null)
    //            foreach (var p in listaBancoDados)
    //                ret.Add(ParcSemEntradaPrimPrestDto_De_TcfgPagtoMeioStatus(p));
    //        return ret;
    //    }
    //}
}
