using ClassesBase;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentistaEindicador
{
    public class OrcamentistaEIndicadorBll : BaseBLL<TorcamentistaEindicador, TorcamentistaEindicadorFiltro>
    {
        private OrcamentistaEIndicadorData _data { get; set; }
        public OrcamentistaEIndicadorBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentistaEIndicadorData(contextoBdProvider))
        {
            _data = new OrcamentistaEIndicadorData(contextoBdProvider);
        }

        public async Task<TorcamentistaEindicador> ValidarParceiro(string apelido, string senha_digitada_datastamp, bool somenteValidar)
        {
            return await _data.ValidarParceiro(apelido, senha_digitada_datastamp, somenteValidar);
        }

    }
}
