using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace OrcamentoCotacaoMensagemStatus
{
    public class OrcamentoCotacaoMensagemStatusBll 
    {
        private readonly OrcamentoCotacaoMensagemStatusData _data;

        public OrcamentoCotacaoMensagemStatusBll(OrcamentoCotacaoMensagemStatusData data)
        {
            _data = data;
        }

    }
}
