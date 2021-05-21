using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo30
{
    partial class Passo30
    {
        private async Task Cod_site_assistencia_tecnica()
        {
            /*
            Coisas válidas somente para ID_PARAM_SITE = COD_SITE_ASSISTENCIA_TECNICA 
            loja/PedidoNovo.asp, trecho do 
            <select id="pedBonshop" name="pedBonshop" style="width: 80px">
            que é o campo:
            rs("pedido_bs_x_at")=s_ped_bonshop
            */

            if (Pedido.Ambiente.Id_param_site != InfraBanco.Constantes.Constantes.Cod_site.COD_SITE_ASSISTENCIA_TECNICA)
            {
                if (!string.IsNullOrWhiteSpace(Pedido.Extra.Pedido_bs_x_at))
                    Retorno.ListaErros.Add($"Campo Pedido_bs_x_at somente pode ser informado se Id_param_site = COD_SITE_ASSISTENCIA_TECNICA");
                return;
            }

            /*
             * o campo Pedido.Extra.Pedido_bs_x_at pode ser vazio ou um pedido feito pelo mesmo cpf/cnpj que st_entrega = ST_ENTREGA_ENTREGUE 
             * */

            if (string.IsNullOrWhiteSpace(Pedido.Extra.Pedido_bs_x_at))
                return;

            //vamos verificar se é um dos permitidos
            var bd = Criacao.ContextoProvider.GetContextoLeitura();
            var sqlString = (from p in bd.Tpedidos
                             join c in bd.Tclientes on p.Id_Cliente equals c.Id
                             where c.Cnpj_Cpf == Pedido.Cliente.Cnpj_Cpf && p.St_Entrega == Constantes.ST_ENTREGA_ENTREGUE
                             select p.Pedido);
            var listaPedidos = await sqlString.ToListAsync();
            if (!listaPedidos.Contains(Pedido.Extra.Pedido_bs_x_at.Trim()))
            {
                Retorno.ListaErros.Add($"Campo Pedido_bs_x_at, se informado, precisa ser um Pedido do mesmo cliente com status St_Entrega = ST_ENTREGA_ENTREGUE");

            }

        }
    }
}
