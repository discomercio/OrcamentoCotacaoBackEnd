using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class BlocoNotasDtoPedido
    {
        public DateTime Dt_Hora_Cadastro { get; set; }
        public string Usuario { get; set; }
        public string Loja { get; set; }
        public string Mensagem { get; set; }
    }
}
