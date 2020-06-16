using InfraBanco;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.PrePedidoUnisBll
{
    public class ValidacoesPrePedidoUnisBll
    {
        public static async Task<bool> ValidarDadosPrePedidoUnis(PrePedidoUnisDto prePedidoUnis, string apelido, 
            string tipo_pessoa, List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            //g)	Validar Forma de pagamento
            await ValidarFormaPagtoCriacaoUnisBll.ValidarFormaPagto(prePedidoUnis.FormaPagtoCriacao, apelido, 
                tipo_pessoa, lstErros, contextoProvider);
            
            //i)	Validar a quantidade de produtos na lista: 
            //i)	Fazer a busca de todos os produtos
            //ii)	Buscar os coeficientes para calcular os produtos conforme o 
            //        tipo da forma de pagamento e quantidade de parcelas
            //iii)	Fazer a comparação de dados
            //j)	Validar quantidade permitida para cada item da lista de produtos
            //k)	Validar os valores de todos os produtos da lista
            
            return false;
        } 
    }
}
