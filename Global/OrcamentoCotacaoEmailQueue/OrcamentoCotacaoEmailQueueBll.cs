using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cfg.CfgOrcamentoCotacaoEmailTemplate;

namespace OrcamentoCotacaoEmailQueue
{
    public class OrcamentoCotacaoEmailQueueBll : BaseBLL<TorcamentoCotacaoEmailQueue, TorcamentoCotacaoEmailQueueFiltro>
    {
        private readonly CfgOrcamentoCotacaoEmailTemplateBll cfgOrcamentoCotacaoEmailTemplateBll;

        private OrcamentoCotacaoEmailQueueData _data { get; set; }

        public OrcamentoCotacaoEmailQueueBll(ContextoBdProvider contextoBdProvider, CfgOrcamentoCotacaoEmailTemplateBll _cfgOrcamentoCotacaoEmailTemplateBll) : base(new OrcamentoCotacaoEmailQueueData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoEmailQueueData(contextoBdProvider);
            cfgOrcamentoCotacaoEmailTemplateBll = _cfgOrcamentoCotacaoEmailTemplateBll;
        }

        public bool InserirQueueComTemplateEHTML(int IdCfgOrcamentoCotacaoEmailTemplates, TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueue, string[] tagHtml)
        {
            try
            {
                List<TcfgOrcamentoCotacaoEmailTemplate> tcfgOrcamentoCotacaoEmailTemplates = cfgOrcamentoCotacaoEmailTemplateBll.PorFiltro(new TcfgOrcamentoCotacaoEmailTemplateFiltro() { Id = IdCfgOrcamentoCotacaoEmailTemplates });
                string emailTemplateBody = tcfgOrcamentoCotacaoEmailTemplates[0].EmailTemplateBody;

                orcamentoCotacaoEmailQueue.Subject = tcfgOrcamentoCotacaoEmailTemplates[0].EmailTemplateSubject;
                emailTemplateBody = emailTemplateBody.Replace("{Cliente}", tagHtml[0]);
                emailTemplateBody = emailTemplateBody.Replace("{DadosEmpresa}", tagHtml[1]);
                orcamentoCotacaoEmailQueue.Body = emailTemplateBody;
                _data.Inserir(orcamentoCotacaoEmailQueue);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
