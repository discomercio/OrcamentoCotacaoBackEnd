using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodigoDescricao
{
    public class CodigoDescricaoBll : BaseBLL<TcodigoDescricao, TcodigoDescricaoFiltro>
    {
        private CodigoDescricaoData _data { get; set; }

        public CodigoDescricaoBll(ContextoBdProvider contextoBdProvider) : base(new CodigoDescricaoData(contextoBdProvider))
        {
            _data = new CodigoDescricaoData(contextoBdProvider);
        }
    }
}
