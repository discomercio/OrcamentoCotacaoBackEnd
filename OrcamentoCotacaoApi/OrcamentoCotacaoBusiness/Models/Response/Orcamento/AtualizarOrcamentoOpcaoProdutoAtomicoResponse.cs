using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamento
{
    public class AtualizarOrcamentoOpcaoProdutoAtomicoResponse: UtilsGlobais.RequestResponse.ResponseBase
    {
        public List<TorcamentoCotacaoOpcaoItemAtomico> TorcamentoCotacaoOpcaoItemAtomicos { get; set; }
    }
}
