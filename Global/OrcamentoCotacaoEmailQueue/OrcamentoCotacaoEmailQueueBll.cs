using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OrcamentoCotacaoEmailQueue
{
    public class OrcamentoCotacaoEmailQueueBll : BaseBLL<TorcamentoCotacaoEmailQueue, TorcamentoCotacaoEmailQueueFiltro>
    {
        private OrcamentoCotacaoEmailQueueData _data { get; set; }

        public OrcamentoCotacaoEmailQueueBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentoCotacaoEmailQueueData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoEmailQueueData(contextoBdProvider);
        }

        public List<TcfgOrcamentoCotacaoEmailTemplate> GetTemplateById(int Id)
        {
            return _data.GetTemplateById(Id);
        }

        public bool AdicionarQueue(int IdCfgOrcamentoCotacaoEmailTemplates, TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueue, string[] tagHtml)
        {
            List<TcfgOrcamentoCotacaoEmailTemplate> tcfgOrcamentoCotacaoEmailTemplates = GetTemplateById(IdCfgOrcamentoCotacaoEmailTemplates);
            string emailTemplateBody = tcfgOrcamentoCotacaoEmailTemplates[0].EmailTemplateBody;

            orcamentoCotacaoEmailQueue.Subject = tcfgOrcamentoCotacaoEmailTemplates[0].EmailTemplateSubject;
            emailTemplateBody = emailTemplateBody.Replace("{Cliente}", tagHtml[0]);
            emailTemplateBody = emailTemplateBody.Replace("{DadosEmpresa}", tagHtml[1]);

            return _data.AdicionarQueue(emailTemplateBody, orcamentoCotacaoEmailQueue);
        }

        public List<TcfgUnidadeNegocioParametro> GetCfgUnidadeNegocioParametros(string nomeLoja)
        {
            return _data.GetCfgUnidadeNegocioParametros(nomeLoja);
        }

    }
}
