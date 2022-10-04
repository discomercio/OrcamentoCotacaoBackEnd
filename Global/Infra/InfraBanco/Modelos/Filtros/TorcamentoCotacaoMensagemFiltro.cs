using Interfaces;
using System;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoMensagemFiltro : IFilter
    {

        public int Id { get; set; }
        public int IdOrcamentoCotacao { get; set; }        
        public short IdTipoUsuarioContextoRemetente { get; set; }
        public int IdUsuarioRemetente { get; set; }
        public short IdTipoUsuarioContextoDestinatario { get; set; }
        public int IdUsuarioDestinatario { get; set; }       
        public bool Lida { get; set; } = false;
        public DateTime? DataLida { get; set; }
        public DateTime? DataHoraLida { get; set; }        
        public String Mensagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataHoraCadastro { get; set; }        
        public long? IdOrcamentoCotacaoEmailQueue { get; set; }
        public bool? PendenciaTratada { get; set; }
        public DateTime? DataPendenciaTratada { get; set; }
        public DateTime? DataHoraPendenciaTratada { get; set; }
        
    }
}
