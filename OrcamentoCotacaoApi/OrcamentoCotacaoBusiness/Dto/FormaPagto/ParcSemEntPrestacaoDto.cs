using FormaPagamento.Dados;
using System.Collections.Generic;

namespace PrepedidoBusiness.Dto.FormaPagto
{
    //public class ParcSemEntPrestacaoDto
    //{
    //    public short Id { get; set; }
    //    public string Descricao { get; set; }
    //    public int? Ordenacao { get; set; }
    //    public int QtdeMaxParcelas { get; set; }
    //    public int QtdeMaxDias { get; set; }

    //    public static ParcSemEntPrestacaoDto ParcSemEntPrestacaoDto_De_TcfgPagtoMeioStatus(InfraBanco.Modelos.TcfgPagtoMeioStatus origem)
    //    {
    //        if (origem == null) return null;
    //        return new ParcSemEntPrestacaoDto()
    //        {
    //            Id = origem.TcfgPagtoMeio.Id,
    //            Descricao = origem.TcfgPagtoMeio.Descricao,
    //            Ordenacao = origem.TcfgPagtoMeio.Ordenacao,
    //            QtdeMaxParcelas = (short)origem.QtdeMaxParcelas,
    //            QtdeMaxDias = (short)origem.QtdeMaxDias
    //        };
    //    }
    //    public static List<ParcSemEntPrestacaoDto> ListaParcSemEntPrestacaoDto_De_TcfgPagtoMeioStatus(List<InfraBanco.Modelos.TcfgPagtoMeioStatus> listaBancoDados)
    //    {
    //        if (listaBancoDados == null) return null;
    //        var ret = new List<ParcSemEntPrestacaoDto>();
    //        if (listaBancoDados != null)
    //            foreach (var p in listaBancoDados)
    //                ret.Add(ParcSemEntPrestacaoDto_De_TcfgPagtoMeioStatus(p));
    //        return ret;
    //    }
    //}
}
