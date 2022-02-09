using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Loja
{
    public class LojaBll : BaseBLL<Tloja, TlojaFiltro>
    {
        public LojaBll(ContextoBdProvider contextoBdProvider) : base(new LojaData(contextoBdProvider))
        {

        }
    }
}
