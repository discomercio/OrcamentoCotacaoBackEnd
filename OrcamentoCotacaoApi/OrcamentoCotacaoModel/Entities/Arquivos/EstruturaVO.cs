using System;

namespace OrcamentoCotacao.Data.Entities.Arquivos
{
    public class EstruturaVO
    {
        public Guid id { get; set; }
        public string nome { get; set; }
        public string tamanho { get; set; }
        public string tipo { get; set; }
        public Guid pai { get; set; }
        public string descricao { get; set; }
    }
}
