﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoOpcaoPagtoFiltro : IFilter
    {
        public int IdOpcao { get; set; }
        public int Id { get; set; }

        public bool IncluirTorcamentoCotacaoOpcaoItemAtomicoCustoFin { get; set; }
    }
}
