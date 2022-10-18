using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TcfgTipoPermissaoEdicaoCadastro : IModel
    {
        public short Id { get; set; }
        public string Sigla { get; set; }
        public string Descricao { get; set; }

        public List<TProdutoCatalogoPropriedade> TprodutoCatalogoPropriedades { get; set; }
    }
}
