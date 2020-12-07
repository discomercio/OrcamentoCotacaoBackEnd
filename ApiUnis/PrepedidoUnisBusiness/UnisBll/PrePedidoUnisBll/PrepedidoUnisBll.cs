using Cliente;
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

            Cliente.Dados.EnderecoCadastralClientePrepedidoDados endCadastralDados =
                EnderecoCadastralClientePrepedidoUnisDto
                .EnderecoCadastralClientePrepedidoDadosDeEnderecoCadastralClientePrepedidoUnisDto(prePedidoUnis.EnderecoCadastralCliente);

            List<PrepedidoProdutoPrepedidoDados> lstProdutosDados = new List<PrepedidoProdutoPrepedidoDados>();
            prePedidoUnis.ListaProdutos.ForEach(x =>
            {
                var ret = PrePedidoProdutoPrePedidoUnisDto.
                PrepedidoProdutoPrepedidoDadosDePrePedidoProdutoPrePedidoUnisDto(x,
                Convert.ToInt16(prePedidoUnis.PermiteRAStatus));

                lstProdutosDados.Add(ret);
            });

            Prepedido.Dados.DetalhesPrepedido.PrePedidoDados prePedidoDados =
                PrePedidoUnisDto.PrePedidoDadosDePrePedidoUnisDto(prePedidoUnis, endCadastralDados, lstProdutosDados, clienteCadastroDados.DadosCliente);

            string prepedidosRepetidos = await PrepedidosRepetidos(prePedidoDados);
            if (!string.IsNullOrEmpty(prepedidosRepetidos))
            {
                retorno.ListaErros.Add(prepedidosRepetidos);
                return retorno;
            }

            //vamos cadastrar
            List<string> lstRet = (await prepedidoBll.CadastrarPrepedido(prePedidoDados,
                prePedidoUnis.Indicador_Orcamentista.ToUpper(),
                Convert.ToDecimal(configuracaoApiUnis.LimiteArredondamentoPrecoVendaOrcamentoItem), false,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__UNIS)).ToList();

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

        public async Task<string> PrepedidosRepetidos(Prepedido.Dados.DetalhesPrepedido.PrePedidoDados prePedidoDados)
        {
            Prepedido.PrepedidoRepetidoBll prepedidoRepetidoBll = new Prepedido.PrepedidoRepetidoBll(contextoProvider);

            //repetição totalmente igual
            var repetidos = await prepedidoRepetidoBll.PrepedidoJaCadastradoDesdeData(prePedidoDados, DateTime.Now.AddSeconds(-1 * configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosExatamenteIguais_TempoSegundos));
            if (repetidos.Count >= configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosExatamenteIguais_Numero)
            {
                return $"Pré-pedido já foi cadastrado com os mesmos dados há menos de {configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosExatamenteIguais_TempoSegundos} segundos. " +
                    "Pré-pedidos existentes: " + String.Join(", ", repetidos);
            }

            //repetição por ID do cliente (CPF/CNPJ)
            repetidos = await prepedidoRepetidoBll.PrepedidoPorIdCLiente(prePedidoDados.DadosCliente.Id, DateTime.Now.AddSeconds(-1 * configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_TempoSegundos));
            if (repetidos.Count >= configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_Numero)
            {
                return $"Limite de pré-pedidos por CPF/CNPJ excedido, existem {configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_Numero} pré-pedidos há menos de {configuracaoApiUnis.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_TempoSegundos} segundos. " +
                    "Pré-pedidos para o mesmo CPF/CNPJ: " + String.Join(", ", repetidos);
            }

            return null;
        }

        public async Task<BuscarStatusPrepedidoRetornoUnisDto> BuscarStatusPrepedido(string orcamento)
        {
            BuscarStatusPrepedidoRetornoUnisDto ret = BuscarStatusPrepedidoRetornoUnisDto.BuscarStatusPrepedidoRetornoUnisDto_De_BuscarStatusPrepedidoRetornoDados(await prepedidoBll.BuscarStatusPrepedido(orcamento));

            return await Task.FromResult(ret);
        }

        public async Task<ListaInformacoesPrepedidoRetornoUnisDto> ListarStatusPrepedido(FiltroInfosStatusPrepedidosUnisDto filtro)
        {
            // vamos buscar os dados do prepedido no Global
            ListaInformacoesPrepedidoRetornoUnisDto lstInfoRetorno = new ListaInformacoesPrepedidoRetornoUnisDto();

            int cancelado = configuracaoApiUnis.ParamBuscaListagemStatusPrepedido.CanceladoDias;
            int pendentes = configuracaoApiUnis.ParamBuscaListagemStatusPrepedido.PendentesDias;
            int virouPedido = configuracaoApiUnis.ParamBuscaListagemStatusPrepedido.VirouPedidoDias;

            //bucar a lista completa
            List<InformacoesStatusPrepedidoRetornoDados> lstStatusPrepedidoDados = await prepedidoBll.ListarStatusPrepedido(filtro.FiltrarPrepedidos, (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__UNIS);

            if (lstStatusPrepedidoDados != null)
            {
                if (lstStatusPrepedidoDados.Count > 0)
                {
                    //vamos converter os dados
                    List<InformacoesPrepedidoUnisDto> lstInfoPrepedidoUnisDto = InformacoesPrepedidoUnisDto.
                        ListaInformacoesPrepedidoUnisDto_De_ListaInformacoesStatusPrepedidoRetornoDados(lstStatusPrepedidoDados);

                    if (lstInfoPrepedidoUnisDto != null)
                    {
                        if (lstInfoPrepedidoUnisDto.Count > 0)
                        {
                            lstInfoRetorno.ListaInformacoesPrepedidoRetorno = new List<InformacoesPrepedidoUnisDto>();
                            foreach (var i in lstInfoPrepedidoUnisDto)
                            {
                                if (filtro.VirouPedido)
                                {
                                    if (i.St_virou_pedido && i.Data >= DateTime.Now.AddDays(-virouPedido))
                                    {
                                        lstInfoRetorno.ListaInformacoesPrepedidoRetorno.Add(i);
                                    }
                                }
                                if (filtro.Pendentes)
                                {
                                    if (string.IsNullOrEmpty(i.St_orcamento) && !i.St_virou_pedido &&
                                        i.Data >= DateTime.Now.AddDays(-pendentes))
                                    {
                                        lstInfoRetorno.ListaInformacoesPrepedidoRetorno.Add(i);
                                    }
                                }
                                if (filtro.Cancelados)
                                {
                                    if (i.St_orcamento == InfraBanco.Constantes.Constantes.ST_ORCAMENTO_CANCELADO &&
                                        i.Data >= DateTime.Now.AddDays(-cancelado))
                                    {
                                        lstInfoRetorno.ListaInformacoesPrepedidoRetorno.Add(i);
                                    }
                                }
                            }
                        }
                    }

                    //if (filtro.Cancelados)
                    //{
                    //    lstInfoRetorno.ListaInformacoesPrepedidoRetorno.Add(lstInfoPrepedidoUnisDto
                    //   .Where(x => x.St_orcamento == Constantes.ST_ORCAMENTO_CANCELADO &&
                    //           x.Data >= DateTime.Now.AddDays(-configuracaoApiUnis.ParamBuscaListagemStatusPrepedido.CanceladoDias))
                    //    .Select(c => c).ToList());
                    //}
                    //if (filtro.Pendentes)
                    //{
                    //    lstInfoRetorno.ListaPrepedidosPendentesUnisDto = lstInfoPrepedidoUnisDto
                    //        .Where(x => !x.St_virou_pedido &&
                    //            x.Data >= DateTime.Now.AddDays(-configuracaoApiUnis.ParamBuscaListagemStatusPrepedido.PendentesDias))
                    //        .Select(c => c).ToList();
                    //}
                    //if (filtro.VirouPedido)
                    //{
                    //    lstInfoRetorno.ListaPrepedidosViraramPedidosUnisDto = lstInfoPrepedidoUnisDto
                    //        .Where(x => x.St_virou_pedido &&
                    //            x.Data >= DateTime.Now.AddDays(-configuracaoApiUnis.ParamBuscaListagemStatusPrepedido.VirouPedidoDias))
                    //        .Select(c => c).ToList();
                    //}
                }
            }

            return lstInfoRetorno;
        }
    }
}

