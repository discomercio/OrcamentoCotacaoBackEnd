using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class MovimentoEstoqueDto
    {
        public short total_estoque_vendido { get; set; }
        public short total_estoque_sem_presenca { get; set; }
        public string s_log_item_autosplit { get; set; }
    }
}
