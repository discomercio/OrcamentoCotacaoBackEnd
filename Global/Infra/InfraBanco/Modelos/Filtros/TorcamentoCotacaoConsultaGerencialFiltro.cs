using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoConsultaGerencialFiltro : IFilter
    {
        public List<string> Lojas { get; set; }

        public int IdVendedor { get; set; }

        public bool ComParceiro { get; set; }

        public int IdParceiro { get; set; }

        public int IdVendedorParceiro { get; set; }

        public string Fabricante { get; set; }

        public string Grupo { get; set; }

        public DateTime? DataCricaoInicio { get; set; }

        public DateTime? DataCriacaoFim { get; set; }

        public DateTime? DataCorrente { get; set; }

        public bool Ascendente { get; set; }

        public string NomeColunaOrdenacao { get; set; }

        public bool MensagemPendente { get; set; }

        public bool Expirado { get; set; }
    }
}
