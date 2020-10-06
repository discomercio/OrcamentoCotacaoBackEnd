using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string Descricao { get; set; }
        public string Descricao_html { get; set; }
        public float Qtde_solicitada { get; set; }
        public float Qtde_estoque { get; set; }
        public float Qtde_estoque_vendido { get; set; }
        public float Qtde_estoque_sem_presenca { get; set; }
        public float Qtde_estoque_global { get; set; }
    }
}
