using FormaPagamento.Dados;
using Prepedido.Dto;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Dto.FormaPagto
{
    public class FormaPagtoDto
    {
        public List<AvistaDto> ListaAvista { get; set; }
        public List<ParcUnicaDto> ListaParcUnica { get; set; }
        public bool ParcCartaoInternet { get; set; }
        public bool ParcCartaoMaquineta { get; set; }
        public List<ParcComEntradaDto> ListaParcComEntrada { get; set; }
        public List<ParcComEntPrestacaoDto> ListaParcComEntPrestacao { get; set; }
        public List<ParcSemEntradaPrimPrestDto> ListaParcSemEntPrimPrest { get; set; }
        public List<ParcSemEntPrestacaoDto> ListaParcSemEntPrestacao { get; set; }

        public static FormaPagtoDto FormaPagtoDto_De_FormaPagtoDados(FormaPagtoDados origem)
        {
            if (origem == null) return null;
            return new FormaPagtoDto()
            {
                ListaAvista = AvistaDto.ListaAvistaDto_De_AvistaDados(origem.ListaAvista),
                ListaParcUnica = ParcUnicaDto.ListaParcUnicaDto_De_ParcUnicaDados(origem.ListaParcUnica),
                ParcCartaoInternet = origem.ParcCartaoInternet,
                ParcCartaoMaquineta = origem.ParcCartaoMaquineta,
                ListaParcComEntrada = ParcComEntradaDto.ListaParcComEntradaDto_De_ParcComEntradaDados(origem.ListaParcComEntrada),
                ListaParcComEntPrestacao = ParcComEntPrestacaoDto.ListaParcComEntPrestacaoDto_De_ParcComEntPrestacaoDados(origem.ListaParcComEntPrestacao),
                ListaParcSemEntPrimPrest = ParcSemEntradaPrimPrestDto.ListaParcSemEntradaPrimPrestDto_De_ParcSemEntradaPrimPrestDados(origem.ListaParcSemEntPrimPrest),
                ListaParcSemEntPrestacao = ParcSemEntPrestacaoDto.ListaParcSemEntPrestacaoDto_De_ParcSemEntPrestacaoDados(origem.ListaParcSemEntPrestacao)
            };
        }
    }
}

