using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.Dto.CoeficienteDto;
using Loja.Bll.Dto.FormaPagtoDto;
using Loja.Bll.Dto.LojaDto;
using Loja.Bll.Dto.PedidoDto;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;
using Loja.Bll.Dto.PrepedidoDto.DetalhesPrepedido;
using Loja.Bll.Dto.ProdutoDto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.UI.Models.Pedido
{
    public class ProdutosFormaPagtoViewModel
    {
        //
        public ProdutoComboDto ProdutoCombo { get; set; }
        //public DadosClienteCadastroDto DadosCliente { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCnpj { get; set; }
        public string TipoCliente { get; set; }
        public EnderecoEntregaDtoClienteCadastro EnderecoEntrega { get; set; }
        public List<PedidoProdutosDtoPedido> ListaProdutos { get; set; }
        public SelectList EnumFormaPagto { get; set; }
        public FormaPagtoDto FormaPagto { get; set; }
        public SelectList ListaFormaPagto { get; set; }
        public FormaPagtoCriacaoDto FormaPagtoCriacao { get; set; }
        public int CdSelecionadoId { get; set; }
        public int ComIndicacao { get; set; }//verificar essa var para saber se é com ou sem indicação              
        public SelectList PedBonshop { get; set; }
        public List<CoeficienteDto> ListaCoeficiente { get; set; }
        public short Permite_RA_Status { get; set; }
        public int QtdeParcVisa { get; set; }
        public PercentualMaximoDto PercMaxDescEComissao { get; set; }
        public decimal VlTotalDestePedido { get; set; }
        public decimal? TotalPedidoRALiquido { get; set; }
        public decimal? TotalPedidoRABruto { get; set; }
        public float? PercComissao { get; set; }
        public MeioPagtoPreferenciais MeiosPagtoPreferenciais { get; set; }
        public List<ObjetoSenhaDesconto> ListaObjetoSenhaDesconto { get; set; }

    }
}
