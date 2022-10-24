using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Operacao
{
    public class OperacaoBll : BaseBLL<Toperacao, ToperacaoFiltro>
    {

        private OperacaoData _data { get; set; }

        public OperacaoBll(ContextoBdProvider contextoBdProvider) : base(new OperacaoData(contextoBdProvider))
        {
            _data = new OperacaoData(contextoBdProvider);
        }
    }
}
