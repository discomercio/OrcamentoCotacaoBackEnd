using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Execucao.UtilsLoja
{
    public class PercentualMaxDescEComissao
    {
        private PercentualMaxDescEComissao()
        {
        }

        //se não existir retorna tudo como 0 (em bdd.asp, rotina obtem_perc_max_comissao_e_desconto_por_loja)
        public float Perc_Max_Comissao { get; set; } = 0;
        public float Perc_Max_Comissao_E_Desconto { get; set; } = 0;
        public float Perc_Max_Comissao_E_Desconto_Pj { get; set; } = 0;
        public float Perc_Max_Comissao_E_Desconto_Nivel2 { get; set; } = 0;
        public float Perc_Max_Comissao_E_Desconto_Nivel2_Pj { get; set; } = 0;

        public static async Task<PercentualMaxDescEComissao> ObterPercentualMaxDescEComissao(string loja, InfraBanco.ContextoBdProvider contextoBdProvider)
        {
            var db = contextoBdProvider.GetContextoLeitura();

            var ret = from c in db.Tlojas
                      where c.Loja == loja
                      select new PercentualMaxDescEComissao
                      {
                          Perc_Max_Comissao = c.Perc_Max_Comissao,
                          Perc_Max_Comissao_E_Desconto = c.Perc_Max_Comissao_E_Desconto,
                          Perc_Max_Comissao_E_Desconto_Nivel2 = c.Perc_Max_Comissao_E_Desconto_Nivel2,
                          Perc_Max_Comissao_E_Desconto_Pj = c.Perc_Max_Comissao_E_Desconto_Pj,
                          Perc_Max_Comissao_E_Desconto_Nivel2_Pj = c.Perc_Max_Comissao_E_Desconto_Nivel2_Pj
                      };

            var retvar = await ret.FirstOrDefaultAsync();
            if (retvar != null)
                return retvar;
            return new PercentualMaxDescEComissao();
        }


    }
}
