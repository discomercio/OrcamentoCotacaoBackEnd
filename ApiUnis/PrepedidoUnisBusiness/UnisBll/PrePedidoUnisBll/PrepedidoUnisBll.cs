﻿using Cliente;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Prepedido;
using Prepedido.Dados.DetalhesPrepedido;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoBusiness.Dto.ClienteCadastro;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto;
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
        private readonly Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll;

        public PrePedidoUnisBll(ConfiguracaoApiUnis configuracaoApiUnis, InfraBanco.ContextoBdProvider contextoProvider,
            InfraBanco.ContextoCepProvider contextoCepProvider, PrepedidoBll prepedidoBll, ClienteBll clienteBll,
            Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll)
        {
            this.configuracaoApiUnis = configuracaoApiUnis;
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
            this.prepedidoBll = prepedidoBll;
            this.clienteBll = clienteBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
        }

        public async Task<PrePedidoResultadoUnisDto> CadastrarPrepedidoUnis(PrePedidoUnisDto prePedidoUnis)
        {
            PrePedidoResultadoUnisDto retorno = new PrePedidoResultadoUnisDto();

            var db = contextoProvider.GetContextoLeitura();

            //orcamentista sempre está em maiusculas
            prePedidoUnis.Indicador_Orcamentista = prePedidoUnis.Indicador_Orcamentista?.ToUpper();

            //BUSCAR DADOS DO CLIENTE para incluir no dto de dados do cliente
            Cliente.Dados.ClienteCadastroDados clienteCadastroDados = await clienteBll.BuscarCliente(prePedidoUnis.Cnpj_Cpf, prePedidoUnis.Indicador_Orcamentista);
            if (clienteCadastroDados == null)
            {
                retorno.ListaErros.Add("Cliente não localizado");
                return retorno;
            }
            ClienteCadastroDto clienteArclube = ClienteCadastroDto.ClienteCadastroDto_De_ClienteCadastroDados(clienteCadastroDados);


            if (!string.IsNullOrEmpty(prePedidoUnis.Cnpj_Cpf))
            {
                //vamos comparar os campos para saber se o cliente é o mesmo de dados cadastrais
                if (UtilsGlobais.Util.SoDigitosCpf_Cnpj(prePedidoUnis.Cnpj_Cpf) !=
                    UtilsGlobais.Util.SoDigitosCpf_Cnpj(prePedidoUnis.EnderecoCadastralCliente.Endereco_cnpj_cpf))
                {
                    retorno.ListaErros.Add("O CPF/CNPJ do cliente está divergindo do cadastro!");
                    return retorno;
                }
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

            if (orcamentista.Permite_RA_Status != Convert.ToInt16(prePedidoUnis.PermiteRAStatus))
            {
                retorno.ListaErros.Add("Permite RA status divergente do cadastro do indicador/orçamentista!");
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
            PrePedidoDados prePedidoDados = PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prePedidoDto);
            List<string> lstRet = (await prepedidoBll.CadastrarPrepedido(prePedidoDados,
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

        public async Task<PermiteRaStatusResultadoUnisDto> Obter_Permite_RA_Status(string apelido)
        {
            if (!await prepedidoBll.ValidarOrcamentistaIndicador(apelido))
                return null;

            bool retorno = Convert.ToBoolean(await prepedidoBll.Obter_Permite_RA_Status(apelido));
            var ret = new PermiteRaStatusResultadoUnisDto()
            {
                PermiteRaStatus = retorno
            };
            return ret;
        }

        public async Task<PercentualVlPedidoRAResultadoUnisDto> ObtemPercentualVlPedidoRA()
        {
            var ret = new PercentualVlPedidoRAResultadoUnisDto();
            ret.PercentualVlPedidoRA = await prepedidoBll.ObtemPercentualVlPedidoRA();
            return ret;
        }

        public async Task<string> PrepedidosRepetidos(PrePedidoDto prePedidoDto)
        {
            Prepedido.PrepedidoRepetidoBll prepedidoRepetidoBll = new Prepedido.PrepedidoRepetidoBll(contextoProvider);

            //repetição totalmente igual
            var repetidos = await prepedidoRepetidoBll.PrepedidoJaCadastradoDesdeData(PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prePedidoDto), DateTime.Now.AddSeconds(-1 * configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosExatamenteIguais_TempoSegundos));
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