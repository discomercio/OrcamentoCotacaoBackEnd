using Interfaces;
using System;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoMensagemFiltro : IFilter
    {
        public int IdOrcamentoCotacao { get; set; }
        public int IdTipoUsuarioContextoRemetente { get; set; }
        public int IdUsuarioRemetente { get; set; }
        public int IdTipoUsuarioContextoDestinatario { get; set; }
        public int IdUsuarioDestinatario { get; set; }
        public String Mensagem { get; set; }
    }
}
