using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TprodutoCatalogoFiltro : IFilter
    {
        //int page, int pageItens, int idCliente, string tipoUsuario, string usuario
        public int IdCliente { get; set; }
        public string TipoUsuario { get; set; }
        public string Usuario { get; set; }
        public string Id { get; set; }
    }
}
