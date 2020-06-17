using InfraBanco.Modelos;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoBusiness.Dtos.Prepedido.DetalhesPrepedido;
using PrepedidoUnisBusiness.UnisBll.PrePedidoUnisBll;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll
{
    public class PrePedidoUnisBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;
        private readonly PrepedidoBusiness.Bll.PrepedidoBll prepedidoBll;

        public PrePedidoUnisBll(InfraBanco.ContextoBdProvider contextoProvider,
            InfraBanco.ContextoCepProvider contextoCepProvider, PrepedidoBusiness.Bll.PrepedidoBll prepedidoBll)
        {
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
            this.prepedidoBll = prepedidoBll;
        }



        //c)	Validar se Pré-Pedido já existe
        //d)	Validar Detalhes do Pré-Pedido:
        //i)	Entrega Imediata não: verificar se a foi informado a Data para entrega
        //ii)	Bem de uso comum: verificar se o valor esta correto conforme já é feito
        //iii)	Instalador instala
        //e)	Validar se a loja esta habilitada para produtos e-commerce
        //f)	Validar Endereço de entrega(Incluir validação dos novos campos no endereço de entrega)




        //m)	Retorna lista de string:
        //i)	Sucesso: lista com 1 item sendo o número do Pré-Pedido
        //ii)	Falha: lista com erros
        public async Task<IEnumerable<string>> CadastrarPrepedidoUnis(PrePedidoUnisDto prePedidoUnis)
        {
            List<string> lstErros = new List<string>();

            var db = contextoProvider.GetContextoLeitura();

            PrepedidoBusiness.Bll.ClienteBll clienteArclubeBll = new PrepedidoBusiness.Bll.ClienteBll(contextoProvider, contextoCepProvider);
            //BUSCAR DADOS DO CLIENTE para incluir no dto de dados do cliente
            var clienteArclube = await clienteArclubeBll.BuscarCliente(prePedidoUnis.Cnpj_Cpf,
                prePedidoUnis.Indicador_Orcamentista);

            if (clienteArclube == null)
            {
                lstErros.Add("Cliente não localizado");
                return lstErros;
            }
            DadosClienteCadastroUnisDto clienteUnis = DadosClienteCadastroUnisDto.DadosClienteCadastroUnisDtoDeDadosClienteCadastroDto(clienteArclube.DadosCliente);

            //a)	Validar se o Orçamentista enviado existe
            TorcamentistaEindicador orcamentista =
                await ValidacoesClienteUnisBll.ValidarBuscarOrcamentista(clienteUnis.Indicador_Orcamentista,
                contextoProvider);

            if (orcamentista != null)
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

                if (await ValidacoesPrePedidoUnisBll.ValidarDadosPrePedidoUnis(prePedidoUnis,
                    clienteUnis.Indicador_Orcamentista, clienteUnis.Tipo, lstErros, contextoProvider))
                {

                }

                /* b)	Validar dados do cliente
                 *  Na validação do cadastro do cliente precisamos verificar se teve alteração nos dados do cliente,
                 *  analisando os novos campos que foram incluídos
                 *  OBS: não iremos alterar de forma automática as alterações do cadastro do cliente, 
                 *  como os novos campo ref. a alteração de cadastro será sempre mostrado na tela com os 
                 *  dados do cadastro do cliente e na criação do prepedido o cliente poderá fazer alterações no cadastro, 
                 *  talvez pegar os dados sempre desses novos campos para fazer a validação do cliente no Prepedido.
                 *  Para isso devemos incluir uma rotina que sempre irá validar os dados do cliente ao criar um novo prepedido
                 *  
                 *  
                 */

                //g)	Validar Forma de pagamento
                //i)	Validar se tipo da opção de pagamento esta correta
                //h)	Validar a quantidade de parcelas: verificar se esta dentro do permitido.
                //i)	Validar a quantidade de produtos na lista: verificar se esta dentro do permitido 
                //i)	Fazer a busca de todos os produtos
                //ii)	Buscar os coeficientes para calcular os produtos conforme o 
                //        tipo da forma de pagamento e quantidade de parcelas
                //iii)	Fazer a comparação de dados
                //j)	Validar quantidade permitida para cada item da lista de produtos
                //k)	Validar os valores de todos os produtos da lista

                /* Será necessário validar os valores dos produtos que estão na lista de produtos 
                 * antes de passar para o dto para cadastro de prepedido.
                 * 
                 * 
                 */


                //l)	Validar total do Pré-Pedido:
                //i)	Para casos que permitem RA será necessário verificar se o Preco_Lista 
                //  esta diferente do valor total calculado com coeficiente para depois verificar 
                //  se o valor de RA esta correto e se é permitido o valor de RA que foi enviado.
                //ii) Para todos os casos será necessário verificar se tem desconto aplicado em cada produto para fazer a comparação de valores e somar o total
                //iii)	Se permite RA, devemos somar a variável Preco_Lista para comparar o total

            }
            else
            {
                lstErros.Add("O Orçamentista não existe!");
            }

            return lstErros;
        }


        //public async Task DeletarOrcamentoExisteComTransacao(string orcamentista, string numeroPrepedido)
        //{
        //    PrePedidoDto prePedido = new PrePedidoDto();
        //    //vamos buscar o prepedido

        //    if (await prepedidoBll.ValidarOrcamentistaIndicador(orcamentista))
        //    {
        //        prePedido.NumeroPrePedido = numeroPrepedido.Trim();

        //        using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
        //        {
        //            await prepedidoBll.DeletarOrcamentoExiste(dbgravacao, prePedido, orcamentista);
        //            dbgravacao.transacao.Commit();
        //        }
        //    }
        //}

        public async Task<bool> CancelarPrePedido(string orcamentista, string numeroPrepedido)
        {
            bool retorno = false;

            PrePedidoDto prePedido = new PrePedidoDto();
            //vamos buscar o prepedido

            if (await prepedidoBll.ValidarOrcamentistaIndicador(orcamentista))
            {
                prePedido.NumeroPrePedido = numeroPrepedido.Trim();
                {
                    retorno = await prepedidoBll.RemoverPrePedido(numeroPrepedido, orcamentista);                    
                }
            }

            return retorno;
        }

        public async Task<bool> Obter_Permite_RA_Status(string apelido)
        {
            return false;
        }

        public async Task<decimal> ObtemPercentualVlPedidoRA()
        {
            return 0M;
        }
    }
}
