﻿using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamento
{
    public class CadastroOrcamentoOpcaoFormaPagtoResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public List<TorcamentoCotacaoOpcaoPagto> TorcamentoCotacaoOpcaoPagtos { get; set; }
    }
}
