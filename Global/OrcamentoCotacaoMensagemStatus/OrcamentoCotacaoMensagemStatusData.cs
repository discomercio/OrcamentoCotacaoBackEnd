using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OrcamentoCotacaoMensagemStatus
{
    public class OrcamentoCotacaoMensagemStatusData
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoMensagemStatusData(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }       

    }
}
