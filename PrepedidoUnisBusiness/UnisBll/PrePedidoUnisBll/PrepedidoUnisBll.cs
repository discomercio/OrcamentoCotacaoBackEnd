using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll
{
    public class PrePedidoUnisBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public PrePedidoUnisBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        //a)	Validar se o Orçamentista enviado existe
        //b)	Validar dados do cliente
        //c)	Validar se Pré-Pedido já existe
        //d)	Validar Detalhes do Pré-Pedido:
        //i)	Entrega Imediata não: verificar se a foi informado a Data para entrega
        //ii)	Bem de uso comum: verificar se o valor esta correto conforme já é feito
        //iii)	Instalador instala
        //e)	Validar se a loja esta habilitada para produtos e-commerce
        //f)	Validar Endereço de entrega(Incluir validação dos novos campos no endereço de entrega)
        //g)	Validar Forma de pagamento
        //i)	Validar se tipo da opção de pagamento esta correta
        //h)	Validar a quantidade de parcelas
        //i)	Validar a quantidade de produtos na lista: 
        //i)	Fazer a busca de todos os produtos
        //ii)	Buscar os coeficientes para calcular os produtos conforme o tipo da forma de pagamento e quantidade de parcelas
        //iii)	Fazer a comparação de dados
        //j)	Validar quantidade permitida para cada item da lista de produtos
        //k)	Validar os valores de todos os produtos da lista
        //l)	Validar total do Pré-Pedido:
        //i)	Para casos que permitem RA será necessário verificar se o Preco_Lista esta diferente do valor total calculado com coeficiente para depois verificar se o valor de RA esta correto e se é permitido o valor de RA que foi enviado.
        //ii) Para todos os casos será necessário verificar se tem desconto aplicado em cada produto para fazer a comparação de valores e somar o total
        //iii)	Se permite RA, devemos somar a variável Preco_Lista para comparar o total
        //m)	Retorna lista de string:
        //i)	Sucesso: lista com 1 item sendo o número do Pré-Pedido
        //ii)	Falha: lista com erros
        public async Task<IEnumerable<string>> CadastrarPrepedidoUnis(PrePedidoUnisDto prePedidoUnis)
        {
            List<string> lstErros = new List<string>();

            var db = contextoProvider.GetContextoLeitura();

            //vamos verificar se orcamentista do cadastro existe
            if (await ValidacoesClienteUnisBll.ValidarOrcamentista(prePedidoUnis.DadosCliente.Indicador_Orcamentista,
                prePedidoUnis.DadosCliente.Loja, contextoProvider))
            {
                /*
                 * Precisa ser incluido a validação dos novos campos de memorização de endereço
                 * pois, precisamos verificar se teve alteração no cadastro do cliente quando ele gerou um novo prepedido
                 * antes de passar os dados para o dto do prepedido da Arclube, talvez necessite de alguma validação
                 * 
                 * Será necessário incluir essas rotinas na criação de um novo Prepedido
                 * 
                 * OBS: analisar bem o que devemos validar antes de mandar para a rotina de cadastro do Prepedido da Arclube
                 */
            }
            else
            {
                lstErros.Add("O Orçamentista não existe!");
            }

            return lstErros;
        }
    }
}
