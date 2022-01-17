using OrcamentoCotacao.Data.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacao.Data.Entities
{
    public class Arquivo : IIdentityEntity
    {
        public Guid Id { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime? DataUltimaAlteracao { get; set; }
    }
}
