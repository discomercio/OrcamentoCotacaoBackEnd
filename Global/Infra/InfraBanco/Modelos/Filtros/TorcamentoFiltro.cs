﻿using ClassesBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoFiltro : IFilter
    {
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
    }
}