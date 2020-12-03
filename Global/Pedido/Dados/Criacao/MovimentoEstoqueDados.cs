using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class MovimentoEstoqueDados
    {
        public short Total_estoque_vendido { get; set; }
        public short Total_estoque_sem_presenca { get; set; }
        public string Slog_item_autosplit { get; set; } = "";
    }
}
