using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TLogV2 : IModel
    {
        public long Id { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataHora { get; set; }
        public short? IdTipoUsuarioContexto { get; set; }
        public int? IdUsuario { get; set; }
        public string Loja { get; set; }
        public string Pedido { get; set; }
        public int? IdOrcamentoCotacao { get; set; }
        public string IdCliente { get; set; }
        public int? SistemaResponsavel { get; set; }
        public int? IdOperacao { get; set; }
        public string Complemento { get; set; }
        public string IP { get; set; }
    }
}
