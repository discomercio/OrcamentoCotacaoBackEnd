﻿using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TprodutoCatalogoFiltro : IFilter
    {
        public int IdCliente { get; set; }
        public string TipoUsuario { get; set; }
        public string Usuario { get; set; }
        public string Id { get; set; }
        public string Produto { get; set; }
    }
}
