using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamento
{
    public class CadastroOrcamentoOpcaoProdutoUnificadoResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public List<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificados { get; set; }
    }
}
