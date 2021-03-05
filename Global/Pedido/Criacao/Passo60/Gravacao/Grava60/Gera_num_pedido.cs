using InfraBanco;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava60
{
    public static class Gera_num_pedido
    {

        public static async Task<string> Gera_num_pedido_pai(List<string> lstErros, ContextoBdGravacao contextoBdGravacao)
        {
            string s_num = "";

            s_num = await UtilsGlobais.Nsu.GerarNsu(contextoBdGravacao, InfraBanco.Constantes.Constantes.NSU_PEDIDO);

            if (string.IsNullOrEmpty(s_num))
            {
                lstErros.Add($"GerarNumeroPedido: erro ao gerar NSU.");
                return "";
            }

            {
                //descarta eventuais zeros à esquerda
                int n_descarte = 0;
                string s_descarte = "";
                n_descarte = s_num.Length - InfraBanco.Constantes.Constantes.TAM_MIN_NUM_PEDIDO;
                s_descarte = s_num.Substring(0, n_descarte);
                string teste = new String('0', n_descarte);

                if (s_descarte != teste)
                {
                    lstErros.Add($"GerarNumeroPedido: descarte não é totalmente zero: {s_num} deve ter no máximo {InfraBanco.Constantes.Constantes.TAM_MIN_NUM_PEDIDO} dógitos significativos");
                    return "";
                }

                s_num = s_num.Substring(n_descarte);
            }

            //obtém a letra para o sufixo do pedido de acordo c / o ano da geração do 
            //nsu(importante: fazer a leitura somente após gerar o nsu, pois a letra pode ter 
            //sido alterada devido à mudança de ano!!)
            var ret = await (from c in contextoBdGravacao.Tcontroles
                             where c.Id_Nsu == InfraBanco.Constantes.Constantes.NSU_PEDIDO
                             select new { c.Ano_Letra_Seq }).FirstOrDefaultAsync();

            var controle = ret;

            string numPedido = "";
            string s_letra_ano = "";
            if (controle == null)
                lstErros.Add("Não existe registro na tabela de controle com o id = '" +
                    InfraBanco.Constantes.Constantes.NSU_PEDIDO);
            else
                s_letra_ano = controle.Ano_Letra_Seq;

            numPedido = s_num + s_letra_ano;


            return numPedido;
        }

        public static string Gera_letra_pedido_filhote(int indice_pedido)
        {
            string s_letra;
            if (indice_pedido <= 0)
                return "";

            char letra = 'A';
            s_letra = (((int)letra - 1) + indice_pedido).ToString();
            return s_letra;
        }


    }
}
