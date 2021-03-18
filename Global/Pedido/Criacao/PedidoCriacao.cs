using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Pedido.Dados.Criacao;
using Prepedido.Dados.FormaPagto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Produto.RegrasCrtlEstoque;
using Cliente.Dados;
using Cliente;
using Cep;

#nullable enable

namespace Pedido.Criacao
{

    public class PedidoCriacao
    {
        #region construtor
        public readonly PedidoBll PedidoBll;
        public readonly InfraBanco.ContextoBdProvider ContextoProvider;
        public readonly Prepedido.FormaPagto.ValidacoesFormaPagtoBll ValidacoesFormaPagtoBll;
        public readonly Prepedido.PrepedidoBll PrepedidoBll;
        public readonly Prepedido.FormaPagto.FormaPagtoBll FormaPagtoBll;
        public readonly Prepedido.ValidacoesPrepedidoBll ValidacoesPrepedidoBll;
        public readonly ClienteBll ClienteBll;
        public readonly CepBll CepBll;
        public readonly IBancoNFeMunicipio BancoNFeMunicipio;
        internal readonly Execucao.Execucao Execucao;
        internal readonly Execucao.Gravacao Gravacao;

        public PedidoCriacao(PedidoBll pedidoBll, InfraBanco.ContextoBdProvider contextoProvider,
            Prepedido.FormaPagto.ValidacoesFormaPagtoBll validacoesFormaPagtoBll, Prepedido.PrepedidoBll prepedidoBll,
            Prepedido.FormaPagto.FormaPagtoBll formaPagtoBll, Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll,
            ClienteBll clienteBll, CepBll cepBll, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            this.PedidoBll = pedidoBll;
            this.ContextoProvider = contextoProvider;
            this.ValidacoesFormaPagtoBll = validacoesFormaPagtoBll;
            this.PrepedidoBll = prepedidoBll;
            this.FormaPagtoBll = formaPagtoBll;
            this.ValidacoesPrepedidoBll = validacoesPrepedidoBll;
            this.ClienteBll = clienteBll;
            this.CepBll = cepBll;
            this.BancoNFeMunicipio = bancoNFeMunicipio;

            Execucao = new Execucao.Execucao(this);
            Gravacao = new Execucao.Gravacao();
        }
        #endregion


        //uma classe: Global/Pedido/PedidoBll/PedidoCriacao com a rotina CadastrarPrepedido, 
        //que retorna um PedidoCriacaoRetorno com o id do pedido, dos filhotes, 
        //as mensagens de erro e as mensagens de erro da validação dos 
        //dados cadastrais (quer dizer, duas listas de erro.) 
        //É que na loja o tratamento dos erros dos dados cadastrais vai ser diferente).
        public async Task<PedidoCriacaoRetornoDados> CadastrarPedido(PedidoCriacaoDados pedido)
        {
            PedidoCriacaoRetornoDados retorno = new PedidoCriacaoRetornoDados();

            /*
Fluxo no módulo loja:
05 - Passo05: ajustando dados (garantindo cpf/cnpj e telefones somente com dígitos, etc)
10 - Passo10: Escolher cliente já cadastrado
	Se o cliente não existir, ele deve ser cadastrado primeiro. (arquivo CLiente/FLuxoCadastroCliente - criar esse arquivo)
20 - Passo20: Confirmar (ou editar) dados cadastrais e informar endereço de entrega
	se editar dados cadastrais, salva na t_cliente
25 - Passo 25: somente na API. Validar dados cadastrais. Não existe na tela porque sempre se usa o atual do cliente.
30 - Passo30: Escolher indicador e RA e Modo de Seleção do CD 
40 - Passo40: Escolher produtos, quantidades e alterar valores e forma de pagamento
50 - Passo50: Informar observações (entrega imediata, instalador instala, etc) 
60 - Passo60: Salvar o pedido
*/

            //05 - Passo05: ajustando dados (garantindo cpf/cnpj e telefones somente com dígitos, etc)
            new Passo05.Passo05(pedido, retorno, this).Executar();

            //setup dados
            await Execucao.ConfigurarExecucaoInicial(pedido);

            var passo10 = new Criacao.Passo10.Passo10(pedido, retorno, this);
            passo10.Permissoes();
            //se tiver erro de permissao retorna imediatamente
            if (retorno.AlgumErro())
                return retorno;

            //setup adicional
            await Execucao.ConfigurarExecucaoComPermissaoOk(pedido, retorno);
            if (retorno.AlgumErro()) return retorno;

            //15 - Passo15: verificar a loja
            var db = ContextoProvider.GetContextoLeitura();
            var lojaExiste = db.Tlojas.Where(c => c.Loja.Contains(pedido.Ambiente.Loja))
                .Select(c => c.Loja).FirstOrDefault();
            if (string.IsNullOrEmpty(lojaExiste))
                retorno.ListaErrosValidacao.Add("Loja não existe!");

            await passo10.ValidarCliente();
            if (retorno.AlgumErro()) return retorno;

            //somente valida o endereço de entrega. Os dados cadastrais são validados no passo 10 (ou 25)
            //na API magento, por hora, nunca será feito
            await new Passo20.Passo20(pedido, retorno, this).ValidarEnderecoEntregaAsync();

            //passo 25 feito em Criacao.Passo10.Passo10.ValidarCliente

            await new Passo30.Passo30(pedido, retorno, this).ExecutarAsync();
            await new Passo40.Passo40(pedido, retorno, this).ExecutarAsync();
            if (retorno.AlgumErro()) return retorno;
            new Passo50.Passo50(pedido, retorno, this).Executar();

            await new Passo60.Passo60(pedido, retorno, this).ExecutarAsync();

            //se tiver algum erro, limpa os numeros de pedidos gerados
            if (retorno.AlgumErro())
            {
                retorno.RemoverPedidos();
            }
            return retorno;

            //return await CadastrarPedido_anterior(pedido, Execucao.UsuarioPermissao);
        }
    }
}
