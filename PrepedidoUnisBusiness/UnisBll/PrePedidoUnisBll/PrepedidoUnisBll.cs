using InfraBanco.Constantes;
using InfraBanco.Modelos;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoBusiness.Bll.ClienteBll;
using PrepedidoBusiness.Bll.PrepedidoBll;
using PrepedidoBusiness.Dto.ClienteCadastro;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoUnisBusiness.UnisBll.PrePedidoUnisBll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll
{
    public class PrePedidoUnisBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;
        private readonly PrepedidoBll prepedidoBll;
        private readonly ClienteBll clienteBll;

        public PrePedidoUnisBll(InfraBanco.ContextoBdProvider contextoProvider,
            InfraBanco.ContextoCepProvider contextoCepProvider, PrepedidoBll prepedidoBll, ClienteBll clienteBll)
        {
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
            this.prepedidoBll = prepedidoBll;
            this.clienteBll = clienteBll;
        }

        public async Task<PrePedidoResultadoUnisDto> CadastrarPrepedidoUnis(PrePedidoUnisDto prePedidoUnis)
        {
            PrePedidoResultadoUnisDto retorno = new PrePedidoResultadoUnisDto();

            var db = contextoProvider.GetContextoLeitura();

            //BUSCAR DADOS DO CLIENTE para incluir no dto de dados do cliente
            var clienteArclube = await clienteBll.BuscarCliente(prePedidoUnis.Cnpj_Cpf,
                prePedidoUnis.Indicador_Orcamentista);

            if (clienteArclube == null)
            {
                retorno.ListaErros.Add("Cliente não localizado");
                return retorno;
            }

            //a)	Validar se o Orçamentista enviado existe
            TorcamentistaEindicador orcamentista =
                await ValidacoesClienteUnisBll.ValidarBuscarOrcamentista(prePedidoUnis.Indicador_Orcamentista,
                contextoProvider);

            if (orcamentista == null)
            {
                retorno.ListaErros.Add("O Orçamentista não existe!");
                return retorno;
            }

            //Vamos passar os dados de "EnderecoCadastralPrepedidoUnisDto" para DadosCadastroClienteDto
            DadosClienteCadastroDto dadosClienteArclube =
                DadosClienteCadastroUnisDto.DadosClienteCadastroDtoDeEnderecoCadastralClientePrepedidoUnisDto(
                    prePedidoUnis.EnderecoCadastralCliente, prePedidoUnis.Indicador_Orcamentista,
                    clienteArclube.DadosCliente.Loja, clienteArclube.DadosCliente.Sexo,
                    (DateTime)clienteArclube.DadosCliente.Nascimento, clienteArclube.DadosCliente.Id);

            List<PrepedidoProdutoDtoPrepedido> lstProdutosArclube = new List<PrepedidoProdutoDtoPrepedido>();
            prePedidoUnis.ListaProdutos.ForEach(x =>
            {
                var ret = PrePedidoProdutoPrePedidoUnisDto.
                PrepedidoProdutoDtoPrepedidoDePrePedidoProdutoPrePedidoUnisDto(x,
                Convert.ToInt16(prePedidoUnis.PermiteRAStatus));

                lstProdutosArclube.Add(ret);
            });


            //vamos cadastrar
            //A validação dos dados será feita no cadastro do prepedido
            List<string> lstRet = (await prepedidoBll.CadastrarPrepedido(PrePedidoUnisDto.
                PrePedidoDtoDePrePedidoUnisDto(prePedidoUnis, dadosClienteArclube, lstProdutosArclube),
                dadosClienteArclube.Indicador_Orcamentista)).ToList();

            if(lstRet.Count > 0)
            {
                if (lstRet.Count > 1)
                {
                    retorno.ListaErros = lstRet;
                    return retorno;
                }
                if(lstRet[0].Length > Constantes.TAM_MAX_ID_ORCAMENTO)
                {
                    retorno.ListaErros = lstRet;
                    return retorno;
                }
                retorno.IdPrePedidoCadastrado = lstRet[0];    
            }
           

            return retorno;
        }

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
            bool retorno = false;

            if (await prepedidoBll.ValidarOrcamentistaIndicador(apelido))
                retorno = Convert.ToBoolean(await prepedidoBll.Obter_Permite_RA_Status(apelido));

            return retorno;
        }

        public async Task<decimal> ObtemPercentualVlPedidoRA()
        {
            return await prepedidoBll.ObtemPercentualVlPedidoRA();
        }
    }
}
