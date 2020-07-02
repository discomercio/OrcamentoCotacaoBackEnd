using InfraBanco.Constantes;
using InfraBanco.Modelos;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoBusiness.Bll.ClienteBll;
using PrepedidoBusiness.Bll.PrepedidoBll;
using PrepedidoBusiness.Dto.ClienteCadastro;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoUnisBusiness.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll
{
    public class PrePedidoUnisBll
    {
        public static class MensagensErro
        {
            public static string Orcamentista_nao_existe = "O Orçamentista não existe!";
        }

        private readonly ConfiguracaoApiUnis configuracaoApiUnis;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;
        private readonly PrepedidoBll prepedidoBll;
        private readonly ClienteBll clienteBll;

        public PrePedidoUnisBll(ConfiguracaoApiUnis configuracaoApiUnis, InfraBanco.ContextoBdProvider contextoProvider,
            InfraBanco.ContextoCepProvider contextoCepProvider, PrepedidoBll prepedidoBll, ClienteBll clienteBll)
        {
            this.configuracaoApiUnis = configuracaoApiUnis;
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
                retorno.ListaErros.Add(MensagensErro.Orcamentista_nao_existe);
                return retorno;
            }

            //Vamos passar os dados de "EnderecoCadastralPrepedidoUnisDto" para DadosCadastroClienteDto
            EnderecoCadastralClientePrepedidoDto endCadastralArclube =
                EnderecoCadastralClientePrepedidoUnisDto.EnderecoCadastralClientePrepedidoDtoDeEnderecoCadastralClientePrepedidoUnisDto(
                    prePedidoUnis.EnderecoCadastralCliente);

            List<PrepedidoProdutoDtoPrepedido> lstProdutosArclube = new List<PrepedidoProdutoDtoPrepedido>();
            prePedidoUnis.ListaProdutos.ForEach(x =>
            {
                var ret = PrePedidoProdutoPrePedidoUnisDto.
                PrepedidoProdutoDtoPrepedidoDePrePedidoProdutoPrePedidoUnisDto(x,
                Convert.ToInt16(prePedidoUnis.PermiteRAStatus));

                lstProdutosArclube.Add(ret);
            });

            //usar o formato padrão da BLL
            PrePedidoDto prePedidoDto = PrePedidoUnisDto.PrePedidoDtoDePrePedidoUnisDto(prePedidoUnis, endCadastralArclube, lstProdutosArclube, clienteArclube.DadosCliente);

            //verifica se já existe (ou se está no limite de repetições)
            string prepedidosRepetidos = await PrepedidosRepetidos(prePedidoDto);
            if (!string.IsNullOrEmpty(prepedidosRepetidos))
            {
                retorno.ListaErros.Add(prepedidosRepetidos);
                return retorno;
            }

            //vamos cadastrar
            //A validação dos dados será feita no cadastro do prepedido
            List<string> lstRet = (await prepedidoBll.CadastrarPrepedido(prePedidoDto,
                prePedidoUnis.Indicador_Orcamentista.ToUpper(),
                Convert.ToDecimal(configuracaoApiUnis.LimiteArredondamentoPrecoVendaOrcamentoItem), false,
                (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__UNIS)).ToList();

            if (lstRet.Count > 0)
            {
                if (lstRet.Count > 1)
                {
                    retorno.ListaErros = lstRet;
                    return retorno;
                }
                if (lstRet[0].Length > Constantes.TAM_MAX_ID_ORCAMENTO)
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

        public async Task<string> PrepedidosRepetidos(PrePedidoDto prePedidoDto)
        {
            PrepedidoRepetidoBll prepedidoRepetidoBll = new PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoRepetidoBll(contextoProvider);

            //repetição totalmente igual
            var repetidos = await prepedidoRepetidoBll.PrepedidoJaCadastradoDesdeData(prePedidoDto, DateTime.Now.AddSeconds(-1 * configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosExatamenteIguais_TempoSegundos));
            if (repetidos.Count >= configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosExatamenteIguais_Numero)
            {
                return $"Pré-pedido já foi cadastrado com os mesmos dados há menos de {configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosExatamenteIguais_TempoSegundos} segundos. " +
                    "Pré-pedidos existentes: " + String.Join(", ", repetidos);
            }

            //repetição por ID do cliente (CPF/CNPJ)
            repetidos = await prepedidoRepetidoBll.PrepedidoPorIdCLiente(prePedidoDto.DadosCliente.Id, DateTime.Now.AddSeconds(-1 * configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_TempoSegundos));
            if (repetidos.Count >= configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_Numero)
            {
                return $"Limite de pré-pedidos por CPF/CNPJ excedido, existem {configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_Numero} pré-pedidos há menos de {configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_TempoSegundos} segundos. " +
                    "Pré-pedidos para o mesmo CPF/CNPJ: " + String.Join(", ", repetidos);
            }

            return null;
        }
    }
}
