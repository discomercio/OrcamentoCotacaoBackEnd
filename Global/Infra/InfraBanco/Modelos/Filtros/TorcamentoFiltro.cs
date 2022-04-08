﻿using Interfaces;
using System;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoFiltro : IFilter
    {
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
        public string Origem { get; set; }
        public string Loja { get; set; }
        public int? TipoUsuario { get; set; }
        public string Apelido { get; set; }
        public string Status { get; set; }
        public string NumeroOrcamento { get; set; }
        public string Vendedor { get; set; }
        public string Parceiro { get; set; }
        public string VendedorParceiro { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
    }
}
