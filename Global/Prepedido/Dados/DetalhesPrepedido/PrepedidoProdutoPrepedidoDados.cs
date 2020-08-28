using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dados.DetalhesPrepedido
{
    public class PrepedidoProdutoPrepedidoDados
    {
        /* AFAZER: alterar os nomes dos campos para facilitar a identificação dos campos com a tabela
         * verificar se há necessidade de todos os campos que estão aqui.
         */

        public string Fabricante { get; set; }
        public string NumProduto { get; set; }
        public string Descricao { get; set; }
        public string Obs { get; set; }
        public short? Qtde { get; set; }
        //campos "Permite_Ra_Status" e "BlnTemRa" 
        //não devem ser uteis nos produtos, pois o Permite RA é com base no Orçamentista/Indicador
        public short Permite_Ra_Status { get; set; }
        public bool BlnTemRa { get; set; }

        //public decimal? Preco { get; set; }//preço fabricante
        //Preco_fabricante = Preco
        public decimal? Preco { get; set; }

        //esse campo deve ter o nome alterado, pois ele tem o mesmo nome de uma campo da tabela que recebe 
        //preco_lista para complementar a tabela de pedido
        //public decimal? Preco_Lista { get; set; }//preco fabricante x coeficiente e pode ser alterado manualmente
        //Preco_RA = Preco_Lista => coloquei esse nome pq ele só aparece se permite ra 
        public decimal? Preco_Lista { get; set; }

        //preço base com coeficiente
        //alterando o nome para Preco_Lista que é o correto
        //public decimal VlLista { get; set; }//preço fabricante x coeficiente 
        public decimal VlLista { get; set; }
        public float? Desconto { get; set; }

        //public decimal VlUnitario { get; set; }//preco fabricante x coeficiente com desconto
        //Preco_venda = VlUnitario
        public decimal VlUnitario { get; set; }

        public decimal? VlTotalItem { get; set; }
        public decimal VlTotalRA { get; set; }
        public float? Comissao { get; set; }
        public decimal? TotalItemRA { get; set; }
        public decimal? TotalItem { get; set; }
        public short? Qtde_estoque_total_disponivel { get; set; }
        public float CustoFinancFornecCoeficiente { get; set; }//coeficiente do fabricante
        //incluimos esse campos apenas para validar o que esta sendo enviado pela API da Unis
        public decimal? Preco_NF { get; set; }
    }
}
