using System;

namespace Orcamento.Dto
{
    public class OrcamentoCotacaoListaDto
    {
        public string NumeroOrcamento { get; set; }
        public string NumPedido { get; set; }
        public string Cliente_Obra { get; set; }
        public string Vendedor { get; set; }
        public string Parceiro { get; set; }
        public string VendedorParceiro { get; set; }
        public string Valor { get; set; }
        public string Status { get; set; }
        public string VistoEm { get; set; }
        public string Mensagem { get; set; }
        public DateTime? DtCadastro { get; set; }
        public DateTime? DtExpiracao { get; set; }
        public string Orcamentista { get; set; }
        public string Loja { get; internal set; }
        public int? IdOrcamentoCotacao { get; set; }
        public int? IdIndicadorVendedor { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
        public short? St_Orc_Virou_Pedido { get; internal set; }
    }
}
