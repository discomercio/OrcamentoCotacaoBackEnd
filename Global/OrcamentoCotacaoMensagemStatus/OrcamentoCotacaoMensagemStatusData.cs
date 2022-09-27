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


        public TorcamentoCotacaoMensagemStatus InserirComTransacao(TorcamentoCotacaoMensagemStatus torcamentoCotacaoMensagemStatus, InfraBanco.ContextoBdGravacao contextoBdGravacao)
        {
            try
            {
                contextoBdGravacao.TorcamentoCotacaoMensagemStatus.Add(torcamentoCotacaoMensagemStatus);
                contextoBdGravacao.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return torcamentoCotacaoMensagemStatus;
        }

    }
}
