using Cliente;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Prepedido;
using Prepedido.Bll;
using Prepedido.Dados.DetalhesPrepedido;
using Prepedido.Dto;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
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
        private readonly PrepedidoBll prepedidoBll;
        private readonly ClienteBll clienteBll;

        public PrePedidoUnisBll(ConfiguracaoApiUnis configuracaoApiUnis, InfraBanco.ContextoBdProvider contextoProvider,
             PrepedidoBll prepedidoBll, ClienteBll clienteBll)
        {
            this.configuracaoApiUnis = configuracaoApiUnis;
            this.contextoProvider = contextoProvider;
            this.prepedidoBll = prepedidoBll;
            this.clienteBll = clienteBll;
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

            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                string prepedidosRepetidos = await PrepedidosRepetidos(prePedidoDados, dbgravacao);
                if (!string.IsNullOrEmpty(prepedidosRepetidos))
                {
                    retorno.ListaErros.Add(prepedidosRepetidos);
                    return retorno;
                }
            }

            //vamos cadastrar
            List<string> lstRet = (await prepedidoBll.CadastrarPrepedido(prePedidoDados,
                prePedidoUnis.Indicador_Orcamentista.ToUpper(),
                Convert.ToDecimal(configuracaoApiUnis.LimiteArredondamentoPrecoVendaOrcamentoItem), false,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__UNIS,
                configuracaoApiUnis.LimiteItens)).ToList();

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
            var ret = new PercentualVlPedidoRAResultadoUnisDto
            {
                PercentualVlPedidoRA = await prepedidoBll.ObtemPercentualVlPedidoRA()
            };
            return ret;
        }

        public async Task<string> PrepedidosRepetidos(Prepedido.Dados.DetalhesPrepedido.PrePedidoDados prePedidoDados, InfraBanco.ContextoBdGravacao dbgravacao)
        {
            PrepedidoRepetidoBll prepedidoRepetidoBll = new PrepedidoRepetidoBll(dbgravacao);

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
            int cancelado = configuracaoApiUnis.ParamBuscaListagemStatusPrepedido.CanceladoDias;
            DateTime? dataLimiteCan = cancelado > 0 ? DateTime.Now.AddDays(-cancelado) : (DateTime?)null;
            int pendentes = configuracaoApiUnis.ParamBuscaListagemStatusPrepedido.PendentesDias;
            DateTime? dataLimitePen = pendentes > 0 ? DateTime.Now.AddDays(-pendentes) : (DateTime?)null;
            int virouPedido = configuracaoApiUnis.ParamBuscaListagemStatusPrepedido.VirouPedidoDias;
            DateTime? dataLimiteVir = virouPedido > 0 ? DateTime.Now.AddDays(-virouPedido) : (DateTime?)null;

            //se tiver item => não filtra por data
            //se não tiver item e appsettings > 0 => filtra por data
            if (filtro.FiltrarPrepedidos != null && filtro.FiltrarPrepedidos.Count > 0)
            {
                dataLimiteCan = null;
                dataLimitePen = null;
                dataLimiteVir = null;
            }

            //vamos pegar a mero data
            DateTime? dataLimiteMenor = dataLimiteCan;
            if (dataLimitePen == null)
                dataLimiteMenor = null;
            if (dataLimiteVir == null)
                dataLimiteMenor = null;

            if (dataLimiteMenor.HasValue && dataLimitePen.HasValue && dataLimiteMenor.Value > dataLimitePen.Value)
                dataLimiteMenor = dataLimitePen;
            if (dataLimiteMenor.HasValue && dataLimiteVir.HasValue && dataLimiteMenor.Value > dataLimiteVir.Value)
                dataLimiteMenor = dataLimiteVir;

            //bucar a lista completa
            // vamos buscar os dados do prepedido no Global
            List<InformacoesStatusPrepedidoRetornoDados> lstStatusPrepedidoDados = await prepedidoBll.ListarStatusPrepedido(
                dataLimiteMenor,
                filtro.FiltrarPrepedidos, (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__UNIS);

            ListaInformacoesPrepedidoRetornoUnisDto lstInfoRetorno = new ListaInformacoesPrepedidoRetornoUnisDto
            {
                ListaInformacoesPrepedidoRetorno = new List<InformacoesPrepedidoUnisDto>()
            };
            if (lstStatusPrepedidoDados == null)
                return lstInfoRetorno;

            //vamos converter os dados
            List<InformacoesPrepedidoUnisDto> lstInfoPrepedidoUnisDto = InformacoesPrepedidoUnisDto.
                ListaInformacoesPrepedidoUnisDto_De_ListaInformacoesStatusPrepedidoRetornoDados(lstStatusPrepedidoDados);

            foreach (var i in lstInfoPrepedidoUnisDto)
            {
                bool adicionar = false;
                if (filtro.VirouPedido && i.St_virou_pedido)
                {
                    if (!dataLimiteVir.HasValue)
                        adicionar = true;
                    if (dataLimiteVir.HasValue && i.Data >= dataLimiteVir)
                        adicionar = true;
                }
                if (filtro.Pendentes && string.IsNullOrEmpty(i.St_orcamento) && !i.St_virou_pedido)
                {
                    if (!dataLimitePen.HasValue)
                        adicionar = true;
                    if (dataLimitePen.HasValue && i.Data >= dataLimitePen)
                        adicionar = true;
                }
                if (filtro.Cancelados && i.St_orcamento == InfraBanco.Constantes.Constantes.ST_ORCAMENTO_CANCELADO)
                {
                    if (!dataLimiteCan.HasValue)
                        adicionar = true;
                    if (dataLimiteCan.HasValue && i.Data >= dataLimiteCan)
                        adicionar = true;
                }
                if (adicionar)
                    lstInfoRetorno.ListaInformacoesPrepedidoRetorno.Add(i);
            }

            return lstInfoRetorno;
        }
    }
}

