using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TcfgPagtoMeioStatusFiltro : IFilter
    {
        public short IdCfgModulo { get; set; }
        public short IdCfgTipoPessoaCliente { get; set; }
        public short IdCfgTipoUsuario { get; set; }
        public short IdCfgPagtoForma { get; set; }
        public byte? PedidoComIndicador { get; set; }
        public byte? Habilitado { get; set; }
        public short? IdTipoParcela { get; set; }
        public bool IncluirTcfgTipoParcela { get; set; }
        public bool IncluirTcfgPagtoMeio { get; set; }
        public bool IncluirTorcamentistaEIndicadorRestricaoFormaPagtos { get; set; }
        public string Apelido { get; set; }
        public string ApelidoParceiro { get; set; }
    }
}
