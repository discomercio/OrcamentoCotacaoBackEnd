using System;
using System.Collections.Generic;
using System.Text;
using Prepedido.Dados.DetalhesPrepedido;

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoProdutoDados
    {
        public PedidoCriacaoProdutoDados(string fabricante, string produto, short qtde, float? desc_Dado, decimal preco_Venda, decimal preco_Lista, decimal preco_NF, decimal custoFinancFornecPrecoListaBase_Conferencia, float custoFinancFornecCoeficiente_Conferencia, short? qtde_spe_usuario_aceitou)
        {
            Fabricante = fabricante ?? throw new ArgumentNullException(nameof(fabricante));
            Produto = produto ?? throw new ArgumentNullException(nameof(produto));
            Qtde = qtde;
            Desc_Dado = desc_Dado;
            Preco_Venda = preco_Venda;
            Preco_Lista = preco_Lista;
            Preco_NF = preco_NF;
            CustoFinancFornecPrecoListaBase_Conferencia = custoFinancFornecPrecoListaBase_Conferencia;
            CustoFinancFornecCoeficiente_Conferencia = custoFinancFornecCoeficiente_Conferencia;
            Qtde_spe_usuario_aceitou = qtde_spe_usuario_aceitou;
        }

        public string Fabricante { get; }
        public string Produto { get; }
        public short Qtde { get; }
        public float? Desc_Dado { get; }
        public decimal Preco_Venda { get; }
        public decimal Preco_Lista { get; }
        public decimal Preco_NF { get; }

        //estes campos são usados somente para conferência
        public decimal CustoFinancFornecPrecoListaBase_Conferencia { get; }
        public float CustoFinancFornecCoeficiente_Conferencia { get; }

        //usado para avisar se mudou o número de produtos não disponíveis em estoque
        //o usuário concordou em fazer o pedido com X unidades em estoque e aceita um pedido com Qtde_spe_aceita unidades sem presernça no estoque
        //se durante o processo outro pedido consumir esse estoque, devemos avisar o cliente que mudou a quantidade de produtos disponíveis para entrega
        //se null, não é verificado
        //cuidado ao buscar pela variável qtde_autorizada_sem_presenca; 
        //ela quer dizer a mesma coisa no ASP mas não é a variável que o usuário enviou; ao invés disso, no ASP ela é calculada
        public short? Qtde_spe_usuario_aceitou { get; set; }

        //informações
        public decimal TotalItem()
        {
            return Math.Round((Preco_Venda * Qtde), 2);
        }
        public decimal? TotalItemRA()
        {
            return Math.Round((Preco_NF * Qtde), 2);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "Estilo de código")]
        public static List<PrepedidoProdutoPrepedidoDados> PrepedidoProdutoPrepedidoDados_De_PedidoCriacaoProdutoDados(List<PedidoCriacaoProdutoDados> lstProdutoPedido)
        {

            List<PrepedidoProdutoPrepedidoDados> lstPrepedidoProduto = new List<PrepedidoProdutoPrepedidoDados>();

            foreach (var x in lstProdutoPedido)
            {
                PrepedidoProdutoPrepedidoDados produtoPrepedido = new PrepedidoProdutoPrepedidoDados();
                produtoPrepedido.Fabricante = x.Fabricante;
                produtoPrepedido.Produto = x.Produto;
                produtoPrepedido.CustoFinancFornecPrecoListaBase = x.CustoFinancFornecPrecoListaBase_Conferencia;
                produtoPrepedido.CustoFinancFornecCoeficiente = x.CustoFinancFornecCoeficiente_Conferencia;
                produtoPrepedido.Preco_Lista = x.Preco_Lista;
                produtoPrepedido.Preco_Venda = x.Preco_Venda;
                produtoPrepedido.Preco_NF = x.Preco_NF;
                produtoPrepedido.Qtde = x.Qtde;
                produtoPrepedido.TotalItem = x.TotalItem();
                produtoPrepedido.TotalItemRA = x.TotalItemRA() ?? 0;
                produtoPrepedido.Desc_Dado = x.Desc_Dado ?? 0;//não estava passando o desconto
                lstPrepedidoProduto.Add(produtoPrepedido);
            }


            return lstPrepedidoProduto;

        }
    }
}
