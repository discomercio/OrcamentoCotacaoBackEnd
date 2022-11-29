using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamento
{
    public class CadastroOrcamentoOpcaoProdutoAtomicoCustoFinResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoOpcaoItemAtomicoCustoFins { get; set; }
    }
}
