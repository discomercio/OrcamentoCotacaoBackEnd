using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.LojaDto
{
    public class ObjetoSenhaDesconto
    {
        public string Id { get; set; }
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public decimal? Desc_Max { get; set; }
        public DateTime Data { get; set; }
        public string IdCliente { get; set; }
        public string Cpf_Cnpj { get; set; }
        public string Loja { get; set; }
        public string Autorizador { get; set; }
        public string supervisor_autorizador { get; set; }
    }
}
