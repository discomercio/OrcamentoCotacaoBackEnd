using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class AtualizarOrcamentoOpcaoProdutoUnificadoResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public List<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificados { get; set; }
    }
}
