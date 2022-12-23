using Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_LOGIN_HISTORICO")]
    public sealed class TloginHistorico : IModel
    {
        public Int64 Id { get; set; }
        public DateTime DataHora { get; set; }
        public int IdTipoUsuarioContexto { get; set; }
        public int IdUsuario { get; set; }
        public bool StSucesso { get; set; }
        public string Ip { get; set; }
        public int sistema_responsavel { get; set; }
        public string Login { get; set; }
        public string Motivo { get; set; }
    }
}