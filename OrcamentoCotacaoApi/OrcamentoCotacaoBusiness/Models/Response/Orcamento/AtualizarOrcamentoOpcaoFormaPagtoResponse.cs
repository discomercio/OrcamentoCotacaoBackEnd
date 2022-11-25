using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class AtualizarOrcamentoOpcaoFormaPagtoResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public List<TorcamentoCotacaoOpcaoPagto> TorcamentoCotacaoOpcaoPagtos { get; set; }
    }
}
