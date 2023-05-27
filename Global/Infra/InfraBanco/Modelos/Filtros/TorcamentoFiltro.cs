using Interfaces;
using System;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoFiltro //: IFilter
    {
        public string Origem { get; set; }
        public string Loja { get; set; }
        public int? TipoUsuario { get; set; }
        public string Apelido { get; set; }
        public int StatusId { get; set; }
        public string NumeroOrcamento { get; set; }
        public string Vendedor { get; set; }
        public string Parceiro { get; set; }
        public string VendedorParceiro { get; set; }
        public int IdUsuario { get; set; }
        public int IdVendedor { get; set; }
        public int IdIndicador { get; set; }
        //public int IdIndicadorVendedor { get; set; }
        public bool PermissaoUniversal { get; set; }


        public string[] Status { get; set; }
        public string Nome_numero { get; set; }
        public string[] Vendedores { get; set; }
        public string[] Parceiros { get; set; }
        public string[] VendedorParceiros { get; set; }
        public string Mensagem { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
        public DateTime? DtInicioExpiracao { get; set; }
        public DateTime? DtFimExpiracao { get; set; }
        public bool Exportar { get; set; }

        public int Pagina { get; set; }
        public int QtdeItensPagina { get; set; }
        public string NomeColunaOrdenacao { get; set; }
        public bool OrdenacaoAscendente { get; set; }

        public string IdBaseBusca { get; set; }
    }
}