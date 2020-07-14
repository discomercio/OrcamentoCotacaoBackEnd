using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ClienteDto
{
    public class RefBancariaDtoCliente
    {
        public string Banco { get; set; }
        public string BancoDescricao { get; set; }
        public string Agencia { get; set; }
        public string ContaBanco { get; set; }
        public string DddBanco { get; set; }
        public string TelefoneBanco { get; set; }
        public string ContatoBanco { get; set; }
        public int OrdemBanco { get; set; }
    }
}
