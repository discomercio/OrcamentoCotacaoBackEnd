using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class Cl_ITEM_PEDIDO_NOVO
    {
        public Cl_ITEM_PEDIDO_NOVO(string fabricante, string produto, short qtde)
        {
            Fabricante = fabricante;
            Produto = produto;
            Qtde = qtde;
        }

        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public short Qtde { get; set; }
        public float Desc_Dado { get; set; }
        public decimal Preco_Venda { get; set; }
        public decimal Preco_NF { get; set; }
        public decimal Preco_fabricante { get; set; }
        public decimal Preco_lista { get; set; }
        public float Margem { get; set; }
        public float Desc_max { get; set; }
        public float Comissao { get; set; }
        public string Descricao { get; set; } = "";
        public string Descricao_html { get; set; } = "";
        public string Ean { get; set; } = "";
        public string Grupo { get; set; } = "";
        public float? Peso { get; set; }
        public short Qtde_volumes { get; set; }
        public short Abaixo_min_status { get; set; }
        public string Abaixo_min_autorizacao { get; set; } = "";
        public string Abaixo_min_autorizador { get; set; } = "";
        public short Sequencia { get; set; }
        public float Markup_fabricante { get; set; }
        public string Abaixo_min_superv_autorizador { get; set; } = "";
        public decimal Vl_custo2 { get; set; }
        public float CustoFinancFornecCoeficiente { get; set; }
        public decimal CustoFinancFornecPrecoListaBase { get; set; }
        public float Cubagem { get; set; }
        public string Ncm { get; set; } = "";
        public string Cst { get; set; } = "";
        public string Descontinuado { get; set; } = "";
        public short Qtde_estoque_total_disponivel { get; set; }
        public short Qtde_estoque_vendido { get; set; }
        public short Qtde_estoque_sem_presenca { get; set; }        
    }
}
