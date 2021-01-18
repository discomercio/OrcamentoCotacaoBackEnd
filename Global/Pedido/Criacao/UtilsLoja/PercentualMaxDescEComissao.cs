using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.UtilsLoja
{
    public class PercentualMaxDescEComissao
    {
        private PercentualMaxDescEComissao()
        {
        }

        //se não existir retorna tudo como 0 (em bdd.asp, rotina obtem_perc_max_comissao_e_desconto_por_loja)
        public float PercMaxComissao { get; set; } = 0;
        public float PercMaxComissaoEDesc { get; set; } = 0;
        public float PercMaxComissaoEDescPJ { get; set; } = 0;
        public float PercMaxComissaoEDescNivel2 { get; set; } = 0;
        public float PercMaxComissaoEDescNivel2PJ { get; set; } = 0;

        public static async Task<PercentualMaxDescEComissao> ObterPercentualMaxDescEComissao(string loja, InfraBanco.ContextoBdProvider contextoBdProvider)
        {
            var db = contextoBdProvider.GetContextoLeitura();

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

            var retvar = await ret.FirstOrDefaultAsync();
            if (retvar != null)
                return retvar;
            return new PercentualMaxDescEComissao();
        }


    }
}
