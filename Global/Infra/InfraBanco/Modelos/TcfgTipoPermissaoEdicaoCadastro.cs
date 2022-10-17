using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TcfgTipoPermissaoEdicaoCadastro : IModel
    {
        public int Id { get; set; }
        public string Sigla { get; set; }
        public string Descricao { get; set; }
    }
}
