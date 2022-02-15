using System;

namespace Orcamento.Dto
{
    public class OrcamentoCotacaoListaDto
    {
        public string NumOrcamento { get; set; }
        public string NumPedido { get; set; }
        public string Cliente_Obra { get; set; }
        public string Vendedor { get; set; }
        public string Parceiro { get; set; }
        public string VendedorParceiro { get; set; }
        public string Valor { get; set; }
        public string Status { get; set; }
        public string VistoEm { get; set; }
        public string Mensagem { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime DtExpiracao { get; set; }
    }
}
