using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    //todo: classe não usada, remover
    public class Cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO
    {
        public Cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO(string fabricante, string produto, string descricao, string descricao_html, float qtde_solicitada, float qtde_estoque, float qtde_estoque_vendido, float qtde_estoque_sem_presenca, float qtde_estoque_global)
        {
            Fabricante = fabricante;
            Produto = produto;
            Descricao = descricao;
            Descricao_html = descricao_html;
            Qtde_solicitada = qtde_solicitada;
            Qtde_estoque = qtde_estoque;
            Qtde_estoque_vendido = qtde_estoque_vendido;
            Qtde_estoque_sem_presenca = qtde_estoque_sem_presenca;
            Qtde_estoque_global = qtde_estoque_global;
        }

        public string Fabricante { get; }
        public string Produto { get; }
        public string Descricao { get; }
        public string Descricao_html { get; }
        public float Qtde_solicitada { get; }
        public float Qtde_estoque { get; }
        public float Qtde_estoque_vendido { get; }
        public float Qtde_estoque_sem_presenca { get; }
        public float Qtde_estoque_global { get; }
    }
}
