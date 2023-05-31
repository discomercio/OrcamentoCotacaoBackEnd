using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class FormaPagtoRequestViewModel
    {
        public string TipoCliente { get; set; }
        public byte ComIndicacao { get; set; }
        public Constantes.TipoUsuario TipoUsuario { get; set; }
        public string Apelido { get; set; }

        public string ApelidoParceiro { get; set; }
    }
}
