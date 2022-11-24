using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class CadastroOpcaoProdutosAtomicosResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public List<TorcamentoCotacaoOpcaoItemAtomico> TorcamentoCotacaoOpcaoItemAtomicos { get; set; }
    }
}
