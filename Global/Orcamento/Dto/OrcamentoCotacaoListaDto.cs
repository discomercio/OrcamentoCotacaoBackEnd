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
        public int IdStatus { get; set; }
        public string VistoEm { get; set; }
        public string Mensagem { get; set; }
        public DateTime? DtCadastro { get; set; }
        public DateTime? DtExpiracao { get; set; }
        public string Orcamentista { get; set; }
        public string Loja { get; set; }
        public int? IdOrcamentoCotacao { get; set; }
        public int? IdIndicadorVendedor { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
        public short? St_Orc_Virou_Pedido { get; set; }
        public int IdVendedor { get; set; }
        public DateTime DataHoraCadastro { get; set; }
        public string NomeCliente { get; set; }
        public string NomeObra { get; set; }
        public string St_Entrega { get; set; }
    }
}