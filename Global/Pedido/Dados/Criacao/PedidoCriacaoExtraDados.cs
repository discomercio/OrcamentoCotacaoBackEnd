using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoExtraDados
    {
        public PedidoCriacaoExtraDados(string? pedido_bs_x_at, string? nfe_Texto_Constar, string? nfe_XPed)
        {
            Pedido_bs_x_at = pedido_bs_x_at;
            Nfe_Texto_Constar = nfe_Texto_Constar;
            Nfe_XPed = nfe_XPed;
        }

        /*
* Documentação do banco:
Campo usado para vincular um pedido cadastrado na 'Assistência Técnica' com um pedido cadastrado na 'Bonshop'. 
Somente na versão 'Assistência Técnica' este campo é preenchido, sendo que o conteúdo armazenado se refere a um 
número de pedido da 'Bonshop'. A versão 'Bonshop' consulta no BD da 'Assistência Técnica' para obter informações 
(ex: bloco de notas) de pedidos que tenham originado demandas de assistência técnica
*/
        public string? Pedido_bs_x_at { get; }

        public string? Nfe_Texto_Constar { get; }   //campo nf_texto no asp
        public string? Nfe_XPed { get; } // campo num_pedido_compra no asp
    }
}
