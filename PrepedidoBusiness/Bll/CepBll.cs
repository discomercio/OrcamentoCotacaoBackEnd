using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Bll
{
    public class CepBll
    {
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;

        public CepBll(InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoCepProvider = contextoCepProvider;
        }
    }
}
