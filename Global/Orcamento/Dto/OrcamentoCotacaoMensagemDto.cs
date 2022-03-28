using System;

namespace Orcamento.Dto
{
    public class OrcamentoCotacaoMensagemDto
    {

        public int Id { get; set; }
        public int IdOrcamentoCotacao { get; set; }
        public int IdTipoUsuarioContextoRemetente { get; set; }
        public int IdUsuarioRemetente { get; set; }
        public int IdTipoUsuarioContextoDestinatario { get; set; }
        public int IdUsuarioDestinatario { get; set; }
        public bool Lida { get; set; }
        public DateTime DataLida { get; set; }
        public DateTime DataHoraLida { get; set; }
        public String Mensagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataHoraCadastro { get; set; }

    }
}
