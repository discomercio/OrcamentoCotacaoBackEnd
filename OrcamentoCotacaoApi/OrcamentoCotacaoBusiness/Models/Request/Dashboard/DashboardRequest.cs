using UtilsGlobais.RequestResponse;

namespace OrcamentoCotacaoBusiness.Models.Request.Dashboard
{
    public sealed class DashboardRequest : RequestBase
    {
        public string Loja { get; set; }

        public int Status = 1;

        public string Origem = "DASHBOARD";
    }
}