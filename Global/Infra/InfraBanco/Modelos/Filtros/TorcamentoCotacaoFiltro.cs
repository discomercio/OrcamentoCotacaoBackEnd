using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoFiltro : IFilter
    {
        public int Id { get; set; }
        public bool LimitarData { get; set; }

        public bool LimitarDataDashboard { get; set; }
        public bool Tusuario { get; set; }

        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
        public string Origem { get; set; }
        public string Loja { get; set; }
        public int? TipoUsuario { get; set; }
        public string Apelido { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string NumeroOrcamento { get; set; }
        public string Vendedor { get; set; }
        public int IdVendedor { get; set; }
        public int IdIndicador { get; set; }
        public int IdIndicadorVendedor { get; set; }
        public string Parceiro { get; set; }
        public string VendedorParceiro { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
        public int IdVendedor { get; set; }
        public int IdIndicador { get; set; }
        public int IdIndicadorVendedor { get; set; }
    }
}
