using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TcfgPagtoFormaStatusFiltro : IFilter
    {
        public short IdCfgModulo { get; set; }
        public short IdCfgTipoPessoaCliente { get; set; }
        public short IdCfgTipoUsuarioPerfil { get; set; }
        public short IdCfgPagtoForma { get; set; }
        public byte? PedidoComIndicador { get; set; }
        public byte? Habilitado { get; set; }
        public bool IncluirTcfgPagtoForma { get; set; }
    }
}
