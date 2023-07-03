using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;

namespace TesteEndpoint
{
    public class TesteEndpointBll : BaseBLL<TEndpoints, EndpointsFiltro>
    {
        private TesteEndpointData _data { get; set; }

        public TesteEndpointBll(ContextoBdProvider contextoBdProvider): base(new TesteEndpointData(contextoBdProvider))
        {
            _data = new TesteEndpointData(contextoBdProvider);
        }
}
}