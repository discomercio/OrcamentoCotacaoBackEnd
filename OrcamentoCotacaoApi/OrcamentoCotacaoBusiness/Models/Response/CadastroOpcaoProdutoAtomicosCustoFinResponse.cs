using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class CadastroOpcaoProdutoAtomicosCustoFinResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoOpcaoItemAtomicoCustoFins { get; set; }
    }
}
