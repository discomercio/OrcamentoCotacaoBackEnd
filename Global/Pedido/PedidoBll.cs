using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Pedido
{
    public class PedidoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public PedidoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        // MÉTODOS NOVOS SENDO MOVIDO
        public async Task<PercentualMaxDescEComissao> ObterPercentualMaxDescEComissao(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            var ret = from c in db.Tlojas
                      where c.Loja == loja
                      select new PercentualMaxDescEComissao
                      {
                          PercMaxComissao = c.Perc_Max_Comissao,
                          PercMaxComissaoEDesc = c.Perc_Max_Comissao_E_Desconto,
                          PercMaxComissaoEDescNivel2 = c.Perc_Max_Comissao_E_Desconto_Nivel2,
                          PercMaxComissaoEDescPJ = c.Perc_Max_Comissao_E_Desconto_Pj,
                          PercMaxComissaoEDescNivel2PJ = c.Perc_Max_Comissao_E_Desconto_Nivel2_Pj
                      };

            return await ret.FirstOrDefaultAsync();
        }
    }
}
