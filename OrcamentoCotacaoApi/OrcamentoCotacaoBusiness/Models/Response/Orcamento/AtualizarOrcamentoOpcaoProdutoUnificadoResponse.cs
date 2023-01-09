using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamento
{
    public class AtualizarOrcamentoOpcaoProdutoUnificadoResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public List<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificados { get; set; }

        public string LogOperacao { get; set; }
    }
}
