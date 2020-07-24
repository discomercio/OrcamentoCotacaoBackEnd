using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dto.Produto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrepedidoBusiness.Dto.ClienteCadastro;
using InfraBanco.Constantes;
using PrepedidoBusiness.Utils;
using InfraBanco.Modelos;
using PrepedidoBusiness.Bll.ClienteBll;
using PrepedidoBusiness.Dto.Cep;
using InfraBanco;

namespace PrepedidoBusiness.Bll.PrepedidoBll
{
    public class ValidacoesPrepedidoBll
    {
        private readonly CoeficienteBll coeficienteBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly ClienteBll.ClienteBll clienteBll;
        private readonly CepBll cepBll;
        private readonly IBancoNFeMunicipio bancoNFeMunicipio;

        public ValidacoesPrepedidoBll(CoeficienteBll coeficienteBll, InfraBanco.ContextoBdProvider contextoProvider,
            ClienteBll.ClienteBll clienteBll, CepBll cepBll, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            this.coeficienteBll = coeficienteBll;
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
            this.cepBll = cepBll;
            this.bancoNFeMunicipio = bancoNFeMunicipio;
        }

        //vamos validar os produtos que foram enviados
        public async Task MontarProdutosParaComparacao(PrePedidoDto prepedido,
                    string siglaFormaPagto, int qtdeParcelas, string loja, List<string> lstErros, float perc_limite_RA,
                    decimal limiteArredondamento)
        {
            List<PrepedidoProdutoDtoPrepedido> lstProdutosParaComparacao = new List<PrepedidoProdutoDtoPrepedido>();
            List<List<CoeficienteDto>> lstCoeficienteDtoArclube = new List<List<CoeficienteDto>>();
            List<CoeficienteDto> coefDtoArclube = new List<CoeficienteDto>();

            List<string> lstFornec = new List<string>();
            lstFornec = prepedido.ListaProdutos.Select(x => x.Fabricante).Distinct().ToList();

            List<CoeficienteDto> lstCoeficiente = new List<CoeficienteDto>();
            //precisa verificar se a forma de pagto é diferente de av para não dar erro na validação

            //buscar coeficiente 
            //vamos alterar para montar uma lista de coeficiente para avista
            lstCoeficiente = (await MontarListaCoeficiente(lstFornec, qtdeParcelas, siglaFormaPagto)).ToList();
            //lstCoeficiente =
            //    (await BuscarListaCoeficientesFornecedores(lstFornec, qtdeParcelas, siglaFormaPagto)).ToList();

            //validar se os coeficientes estão ok
            ValidarCustoFinancFornecCoeficiente(prepedido.ListaProdutos, lstCoeficiente, lstErros);


            if (lstErros.Count == 0)
            {
                //estamos verificando se o produto é composto
                List<PrepedidoProdutoDtoPrepedido> lstProdutosCompare =
                (await BuscarProdutos(prepedido.ListaProdutos, loja, lstErros)).ToList();

                //vamos montar calcular a lista de produtos
                if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                    CalcularProdutoSemCoeficiente(lstProdutosCompare);
                else
                    CalcularProdutoComCoeficiente(lstProdutosCompare, lstCoeficiente);

                ConfrontarProdutos(prepedido, lstProdutosCompare, lstErros, limiteArredondamento);
                ConfrontarTotaisEPercentualMaxRA(prepedido, lstErros, perc_limite_RA);
            }
        }

        private async Task<IEnumerable<CoeficienteDto>> MontarListaCoeficiente(List<string> lstFornec,
            int qtdeParcelas, string siglaFormaPagto)
        {
            List<CoeficienteDto> lstcoefDto = new List<CoeficienteDto>();

            if (siglaFormaPagto == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
            {
                //vamos montar com o coeficiente = 1
                lstcoefDto = BuscarListaCoeficientesFornecedoresAVista(lstFornec, qtdeParcelas);
            }
            else
            {
                //vamos montar normalmente
                lstcoefDto =
                    (await BuscarListaCoeficientesFornecedores(lstFornec, qtdeParcelas, siglaFormaPagto)).ToList();
            }

            return lstcoefDto;
        }

        private void CalcularProdutoSemCoeficiente(List<PrepedidoProdutoDtoPrepedido> lstProdutos)
        {
            lstProdutos.ForEach(y =>
                {
                    //vamos calcular o preco_lista com o coeficiente
                    y.Preco = y.Preco;
                    y.VlLista = Math.Round((decimal)y.Preco * 1, 2);
                    y.VlUnitario = Math.Round(y.VlLista * (decimal)(1 - y.Desconto / 100), 2);
                    y.TotalItem = Math.Round((decimal)(y.VlUnitario * y.Qtde), 2);
                    y.TotalItemRA = Math.Round((decimal)(y.VlLista * y.Qtde), 2);
                    y.CustoFinancFornecCoeficiente = 1;
                });
        }

        private void CalcularProdutoComCoeficiente(List<PrepedidoProdutoDtoPrepedido> lstProdutos,
            List<CoeficienteDto> lstCoeficiente)
        {
            lstCoeficiente.ForEach(x =>
            {
                lstProdutos.ForEach(y =>
                {
                    if (x.Fabricante == y.Fabricante)
                    {
                        //vamos calcular o preco_lista com o coeficiente
                        y.Preco = y.Preco;
                        y.VlLista = Math.Round(((decimal)y.Preco * (decimal)x.Coeficiente), 2);
                        y.VlUnitario = Math.Round(y.VlLista * (decimal)(1 - y.Desconto / 100), 2);
                        y.TotalItem = Math.Round((decimal)(y.VlUnitario * y.Qtde), 2);
                        y.TotalItemRA = Math.Round((decimal)(y.VlLista * y.Qtde), 2);
                        y.CustoFinancFornecCoeficiente = x.Coeficiente;
                    }
                });
            });
        }

        public void ValidarCustoFinancFornecCoeficiente(List<PrepedidoProdutoDtoPrepedido> lstProdutos,
            List<CoeficienteDto> lstCoeficiente, List<string> lstErros)
        {
            lstProdutos.ForEach(x =>
            {
                if (lstCoeficiente.Any(l => l.Fabricante == x.Fabricante))
                {
                    lstCoeficiente.ForEach(y =>
                    {
                        if (y.Fabricante == x.Fabricante && y.Coeficiente != x.CustoFinancFornecCoeficiente)
                            lstErros.Add("Coeficiente do fabricante (" + x.Fabricante + ") esta incorreto!");
                    });
                }
                else
                {
                    lstErros.Add("Fabricante cód.(" + x.Fabricante + ") não possui cadastro de coeficiente!");
                }
            });
        }

        public async Task<IEnumerable<CoeficienteDto>> BuscarListaCoeficientesFornecedores(List<string> lstFornecedores, int qtdeParcelas, string siglaFP)
        {
            List<CoeficienteDto> lstcoefDto = new List<CoeficienteDto>();

            var lstcoeficientesTask = coeficienteBll.BuscarListaCoeficientesFornecedores(lstFornecedores);
            if (lstcoeficientesTask != null)
            {
                foreach (var i in await lstcoeficientesTask)
                {
                    //lstcoefDto = new List<CoeficienteDto>();
                    foreach (var y in i)
                    {
                        if (y.TipoParcela == siglaFP && y.QtdeParcelas == qtdeParcelas)
                        {
                            lstcoefDto.Add(new CoeficienteDto()
                            {
                                Fabricante = y.Fabricante,
                                TipoParcela = y.TipoParcela,
                                QtdeParcelas = y.QtdeParcelas,
                                Coeficiente = y.Coeficiente
                            });
                        }
                    }
                }
            }

            return lstcoefDto;
        }

        public List<CoeficienteDto> BuscarListaCoeficientesFornecedoresAVista(List<string> lstFornecedores, int qtdeParcelas)
        {
            List<CoeficienteDto> lstcoefDto = new List<CoeficienteDto>();

            if (lstFornecedores != null)
            {
                foreach (var i in lstFornecedores)
                {
                    lstcoefDto.Add(new CoeficienteDto()
                    {
                        Fabricante = i,
                        TipoParcela = Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA,
                        QtdeParcelas = 1,
                        Coeficiente = 1
                    });
                }
            }

            return lstcoefDto;
        }

        private async Task<IEnumerable<PrepedidoProdutoDtoPrepedido>> BuscarProdutos(List<PrepedidoProdutoDtoPrepedido> lstProdutos,
            string loja, List<string> lstErros)
        {


            var db = contextoProvider.GetContextoLeitura();
            List<PrepedidoProdutoDtoPrepedido> lsProdutosCompare = new List<PrepedidoProdutoDtoPrepedido>();

            foreach (var x in lstProdutos)
            {
                //vamos verificar se o cód do produto é composto
                if (await VerificarProdutoComposto(x, loja, lstErros))
                {
                    PrepedidoProdutoDtoPrepedido produto = await (from c in db.TprodutoLojas
                                                                  where c.Produto == x.NumProduto &&
                                                                        c.Fabricante == x.Fabricante &&
                                                                        c.Vendavel == "S" &&
                                                                        c.Loja == loja
                                                                  select new PrepedidoProdutoDtoPrepedido
                                                                  {
                                                                      Fabricante = c.Fabricante,
                                                                      NumProduto = c.Produto,
                                                                      Preco = c.Preco_Lista,
                                                                      Desconto = x.Desconto,
                                                                      Qtde = x.Qtde
                                                                  }).FirstOrDefaultAsync();

                    if (produto != null)
                    {
                        lsProdutosCompare.Add(produto);
                    }
                    else
                    {
                        lstErros.Add("Produto cód.(" + x.NumProduto + ") do fabricante cód.(" + x.Fabricante + ") não existe!");
                    }
                }
            }

            return lsProdutosCompare;
        }

        private async Task<bool> VerificarProdutoComposto(PrepedidoProdutoDtoPrepedido produto, string loja, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();


            var prodCompostoTask = await (from c in db.TecProdutoCompostos
                                          where c.Produto_Composto == produto.NumProduto &&
                                          c.Fabricante_Composto == produto.Fabricante
                                          select c).FirstOrDefaultAsync();

            if (prodCompostoTask != null)
            {
                lstErros.Add("Produto cód.(" + produto.NumProduto + ") do fabricante cód.(" + produto.Fabricante + ") " +
                    "é um produto composto. Para cadastrar produtos compostos é necessário enviar os produtos individualmente!");

                return false;
            }

            return true;
        }
        private void ConfrontarProdutos(PrePedidoDto prepedido,
            List<PrepedidoProdutoDtoPrepedido> lstProdutosCompare, List<string> lstErros,
            decimal limiteArredondamento)
        {
            prepedido.ListaProdutos.ForEach(x =>
            {
                lstProdutosCompare.ForEach(y =>
               {
                   if (x.NumProduto == y.NumProduto && x.Fabricante == y.Fabricante)
                   {
                       //vamos confrontar os valores
                       if (x.Preco.HasValue && y.Preco.HasValue && Math.Abs(x.Preco.Value - y.Preco.Value) > limiteArredondamento)
                           lstErros.Add($"Preço do fabricante (Preco_Fabricante {x.Preco} x {y.Preco}) está incorreto!");

                       if (x.VlLista != y.VlLista)
                           lstErros.Add($"Custo financeiro preço lista base (CustoFinancFornecPrecoListaBase " +
                               $"{string.Format("{0:c}", x.VlLista)} x {string.Format("{0:c}", y.VlLista)}) esta incorreto!");

                       //veio da Unis e vamos validar
                       if (x.Preco_NF != null)
                       {
                           //validar se permite RA
                           if (prepedido.PermiteRAStatus == 1)
                           {
                               if (x.Preco_NF != x.Preco_Lista)
                                   lstErros.Add($"Preço de nota fiscal (Preco_NF {string.Format("{0:c}", x.Preco_NF)} x {string.Format("{0:c}", x.Preco_Lista)}) está incorreto!");
                           }
                           else
                           {
                               if (x.Preco_NF != x.VlUnitario)
                                   lstErros.Add($"Preço de nota fiscal (Preco_NF {string.Format("{0:c}", x.Preco_NF)} x {x.VlUnitario}) está incorreto!");
                           }

                       }

                       if (Math.Abs(x.VlUnitario - y.VlUnitario) > limiteArredondamento)
                           lstErros.Add($"Preço do fabricante (Preco_Venda {x.VlUnitario} x {y.VlUnitario}) está incorreto!");
                   }
               });
            });
        }

        private void ConfrontarTotaisEPercentualMaxRA(PrePedidoDto prepedido, List<string> lstErros,
            float perc_limite_RA)
        {
            decimal totalCompare = 0;
            decimal totalRaCompare = 0;

            prepedido.ListaProdutos.ForEach(x =>
            {
                totalCompare += Math.Round((decimal)(x.VlUnitario * x.Qtde), 2);
                totalRaCompare += Math.Round((decimal)(x.Preco_Lista * x.Qtde), 2);
            });

            if (totalCompare != (decimal)prepedido.VlTotalDestePedido)
                lstErros.Add("Os valores totais estão divergindo!");

            if (prepedido.PermiteRAStatus == 1)
            {
                if (totalRaCompare != (decimal)prepedido.ValorTotalDestePedidoComRA)
                    lstErros.Add("Os valores totais de RA estão divergindo!");

                //vamos verificar o valor de RA
                decimal ra = totalRaCompare - totalCompare;
                decimal perc = Math.Round((decimal)(perc_limite_RA / 100), 2);
                decimal percentual = Math.Round(perc * (decimal)prepedido.VlTotalDestePedido, 2);

                if (ra > percentual)
                    lstErros.Add("O valor total de RA excede o limite permitido!");
            }
        }


        public async Task<bool> ValidarEnderecoEntrega(PrePedidoDto prepedido, List<string> lstErros)
        {
            bool retorno = true;

            if (prepedido.EnderecoEntrega.OutroEndereco)
            {
                await ValidarDadosEnderecoEntrega(prepedido, lstErros, contextoProvider);

                if (prepedido.DadosCliente.Tipo == Constantes.ID_PJ)
                    await ValidarDadosPessoaEnderecoEntrega(prepedido, lstErros);

                if (lstErros.Count != 0)
                {
                    retorno = false;
                }                    
            }

            return retorno;
        }

        private async Task ValidarDadosEnderecoEntrega(PrePedidoDto prePedido, List<string> lstErros,
            ContextoBdProvider contextoProvider)
        {

            if (prePedido.EnderecoEntrega.OutroEndereco)
            {
                //verificar se a justificativa esta correta "ListarComboJustificaEndereco"
                List<EnderecoEntregaJustificativaDto> lstJustificativas =
                    (await clienteBll.ListarComboJustificaEndereco(prePedido.DadosCliente.Indicador_Orcamentista)).ToList();

                bool achouJustifivcativa = false;
                lstJustificativas.ForEach(x =>
                {
                    if (prePedido.EnderecoEntrega.EndEtg_cod_justificativa == x.EndEtg_cod_justificativa)
                    {
                        achouJustifivcativa = true;
                    }
                });

                if (achouJustifivcativa)
                {
                    if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_cod_justificativa))
                        lstErros.Add("SELECIONE A JUSTIFICATIVA DO ENDEREÇO DE ENTREGA!");

                    if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_endereco))
                        lstErros.Add("PREENCHA O ENDEREÇO DE ENTREGA.");

                    if (prePedido.EnderecoEntrega.EndEtg_endereco.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                        lstErros.Add("ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " + prePedido.EnderecoEntrega.EndEtg_endereco.Length +
                            " CARACTERES<br>TAMANHO MÁXIMO: " + Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");

                    if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_endereco_numero))
                        lstErros.Add("PREENCHA O NÚMERO DO ENDEREÇO DE ENTREGA.");

                    if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_bairro))
                        lstErros.Add("PREENCHA O BAIRRO DO ENDEREÇO DE ENTREGA.");

                    if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_cidade))
                        lstErros.Add("PREENCHA A CIDADE DO ENDEREÇO DE ENTREGA.");

                    if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_uf) || !Util.VerificaUf(prePedido.EnderecoEntrega.EndEtg_uf))
                        lstErros.Add("UF INVÁLIDA NO ENDEREÇO DE ENTREGA.");

                    if (!Util.VerificaCep(prePedido.EnderecoEntrega.EndEtg_cep))
                        lstErros.Add("CEP INVÁLIDO NO ENDEREÇO DE ENTREGA.");

                    if (lstErros.Count == 0)
                    {
                        //vamos comparar endereço
                        string cepSoDigito = prePedido.EnderecoEntrega.EndEtg_cep.Replace(".", "").Replace("-", "");
                        List<CepDto> lstCepDto = (await cepBll.BuscarPorCep(cepSoDigito)).ToList();

                        if (lstCepDto.Count == 0)
                        {
                            lstErros.Add("Endereço Entrega: cep inválido!");
                        }
                        else
                        {
                            CepDto cep = new CepDto()
                            {
                                Cep = prePedido.EnderecoEntrega.EndEtg_cep,
                                Endereco = prePedido.EnderecoEntrega.EndEtg_endereco,
                                Bairro = prePedido.EnderecoEntrega.EndEtg_bairro,
                                Cidade = prePedido.EnderecoEntrega.EndEtg_cidade,
                                Uf = prePedido.EnderecoEntrega.EndEtg_uf
                            };
                            await ValidacoesClienteBll.VerificarEndereco(cep, lstCepDto, lstErros, contextoProvider,
                                bancoNFeMunicipio);
                        }

                        await ValidacoesClienteBll.ConsisteMunicipioIBGE(prePedido.EnderecoEntrega.EndEtg_cidade,
                            prePedido.EnderecoEntrega.EndEtg_uf, lstErros, contextoProvider, bancoNFeMunicipio, true);
                    }
                }
                else
                {
                    lstErros.Add("Código da justficativa inválida!");
                }
            }
        }

        public bool ValidarDetalhesPrepedido(DetalhesDtoPrepedido detalhesPrepedido, List<string> lstErros)
        {
            bool retorno = true;

            if (byte.Parse(detalhesPrepedido.EntregaImediata) !=
                (byte)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO &&
                byte.Parse(detalhesPrepedido.EntregaImediata) !=
                (byte)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM &&
                byte.Parse(detalhesPrepedido.EntregaImediata) !=
                (byte)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO_DEFINIDO &&
                byte.Parse(detalhesPrepedido.EntregaImediata) !=
                (byte)Constantes.EntregaImediata.COD_ETG_IMEDIATA_ST_INICIAL)
            {
                lstErros.Add("Valor de Entrega Imediata inválida!");
                retorno = false;
            }

            if (byte.Parse(detalhesPrepedido.EntregaImediata) ==
                (byte)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO)//Não
            {
                if (detalhesPrepedido.EntregaImediataData.Value.Date <= DateTime.Now.Date)
                {
                    lstErros.Add("Favor informar a data de 'Entrega Imediata' posterior a data atual!");
                    retorno = false;
                }
            }

            return retorno;
        }

        private async Task ValidarDadosPessoaEnderecoEntrega(PrePedidoDto prePedido, List<string> lstErros)
        {
            var cliente = await clienteBll.BuscarCliente(prePedido.DadosCliente.Cnpj_Cpf,
                prePedido.DadosCliente.Indicador_Orcamentista);

            //incluir a flag blnUsarMemorizacaoCompletaEnderecos
            //incluir verificação para saber se o tipo da pessoa é PF ou PJ para validar corretamente

            if (prePedido.EnderecoEntrega.EndEtg_tipo_pessoa != Constantes.ID_PJ &&
                prePedido.EnderecoEntrega.EndEtg_tipo_pessoa != Constantes.ID_PF)
            {
                lstErros.Add("Endereço de Entrega: Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!");
            }
            else if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_nome))
            {
                lstErros.Add("Endereço de Entrega: Preencha o nome/razão social no endereço de entrega!");
            }
            else
            {
                if (prePedido.EnderecoEntrega.EndEtg_tipo_pessoa == Constantes.ID_PJ)
                {
                    ValidarDadosPessoaEnderecoEntrega_PJ(prePedido.EnderecoEntrega, lstErros);
                    ValidarDadosPessoaEnderecoEntrega_PJ_Tel(prePedido.EnderecoEntrega, lstErros);
                }
                if (prePedido.EnderecoEntrega.EndEtg_tipo_pessoa == Constantes.ID_PF)
                {
                    ValidarDadosPessoaEnderecoEntrega_PF(prePedido.EnderecoEntrega, lstErros);
                    ValidarDadosPessoaEnderecoEntrega_PF_Tel(prePedido.EnderecoEntrega, lstErros);
                }
            }

            if (lstErros.Count == 0)
            {
                if (!string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_ie))
                {
                    ValidacoesClienteBll.VerificarInscricaoEstadualValida(
                        prePedido.EnderecoEntrega.EndEtg_ie, prePedido.EnderecoEntrega.EndEtg_uf, lstErros);
                }
            }


        }

        private void ValidarDadosPessoaEnderecoEntrega_PJ(EnderecoEntregaDtoClienteCadastro endEtg, List<string> lstErros)
        {
            if (endEtg.EndEtg_tipo_pessoa == Constantes.ID_PJ)
            {
                if (!Util.ValidaCNPJ(endEtg.EndEtg_cnpj_cpf))
                {
                    lstErros.Add("Endereço de entrega: CNPJ inválido!");
                }

                if (endEtg.EndEtg_produtor_rural_status == 1)
                {
                    lstErros.Add("Endereço de entrega: Se tipo pessoa é PJ, não pode ser Produtor Rural!");
                }

                if (endEtg.EndEtg_contribuinte_icms_status !=
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                    endEtg.EndEtg_contribuinte_icms_status !=
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                    endEtg.EndEtg_contribuinte_icms_status !=
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    lstErros.Add("Endereço de entrega: valor de contribuinte do ICMS inválido!");
                }

                if (endEtg.EndEtg_contribuinte_icms_status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL)
                {
                    lstErros.Add("Endereço de entrega: valor de contribuinte do ICMS inválido!");
                }

                if (string.IsNullOrEmpty(endEtg.EndEtg_cnpj_cpf) ||
                    !Util.ValidaCNPJ(endEtg.EndEtg_cnpj_cpf))
                    lstErros.Add("Endereço de entrega: CNPJ inválido!");

                if (string.IsNullOrEmpty(endEtg.EndEtg_contribuinte_icms_status.ToString()))
                    lstErros.Add("Endereço de entrega: selecione o tipo de contribuinte de ICMS!");

                if (endEtg.EndEtg_contribuinte_icms_status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                    string.IsNullOrEmpty(endEtg.EndEtg_ie))
                    lstErros.Add("Endereço de entrega: se o cliente é contribuinte do ICMS a inscrição " +
                        "estadual deve ser preenchida!");

                if (endEtg.EndEtg_contribuinte_icms_status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                    endEtg.EndEtg_ie.IndexOf("ISEN") > -1)
                    lstErros.Add("Endereço de entrega: se cliente é não contribuinte do ICMS, " +
                        "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                if (endEtg.EndEtg_contribuinte_icms_status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                    endEtg.EndEtg_ie.IndexOf("ISEN") > -1)
                    lstErros.Add("Endereço de entrega: se cliente é contribuinte do ICMS, " +
                        "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                if (!string.IsNullOrEmpty(endEtg.EndEtg_ie))
                    ValidacoesClienteBll.VerificarInscricaoEstadualValida(endEtg.EndEtg_ie,
                        endEtg.EndEtg_uf, lstErros);
            }
        }

        private void ValidarDadosPessoaEnderecoEntrega_PJ_Tel(EnderecoEntregaDtoClienteCadastro endEtg, List<string> lstErros)
        {
            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com) || !string.IsNullOrEmpty(endEtg.EndEtg_ddd_com))
            {
                if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_com).Length < 6)
                    lstErros.Add("Endereço de entrega: telefone comercial inválido!");

                if (string.IsNullOrEmpty(endEtg.EndEtg_ddd_com))
                {
                    lstErros.Add("Endereço de entrega: preencha o ddd do telefone comercial!");
                }
                else
                {
                    if (endEtg.EndEtg_ddd_com.Length != 2)
                        lstErros.Add("Endereço de entrega: ddd do telefone comercial inválido!");
                }
            }

            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com_2) || !string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2))
            {
                if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_com_2).Length < 6)
                    lstErros.Add("Endereço de entrega: telefone comercial 2 inválido!");

                if (string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2))
                {
                    lstErros.Add("Endereço de entrega: preencha o ddd do telefone comercial 2!");
                }
                else
                {
                    if (endEtg.EndEtg_ddd_com_2.Length != 2)
                        lstErros.Add("Endereço de entrega: ddd do telefone comercial 2 inválido!");
                }
            }

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_res) || !string.IsNullOrEmpty(endEtg.EndEtg_tel_res))
                lstErros.Add("Endereço de entrega: se tipo pessoa PJ, não pode conter telefone residencial!");

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_cel) || !string.IsNullOrEmpty(endEtg.EndEtg_tel_cel))
                lstErros.Add("Endereço de entrega: se tipo pessoa PJ, não pode conter telefone celular!");
        }

        private void ValidarDadosPessoaEnderecoEntrega_PF(EnderecoEntregaDtoClienteCadastro endEtg, List<string> lstErros)
        {
            if (endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF)
            {
                if (endEtg.EndEtg_produtor_rural_status !=
                    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO &&
                    endEtg.EndEtg_produtor_rural_status !=
                    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                {
                    lstErros.Add("Endereço de entrega: valor de produtor rural inválido!");
                }

                if (string.IsNullOrEmpty(endEtg.EndEtg_cnpj_cpf) ||
                    !Util.ValidaCPF(endEtg.EndEtg_cnpj_cpf))
                    lstErros.Add("Endereço de entrega: CPF inválido!");

                if (string.IsNullOrEmpty(endEtg.EndEtg_produtor_rural_status.ToString()))
                    lstErros.Add("Endereço de entrega: informe se o cliente é produtor rural ou não!");

                if (endEtg.EndEtg_produtor_rural_status ==
                    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM &&
                    endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF)
                {
                    if (endEtg.EndEtg_contribuinte_icms_status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        endEtg.EndEtg_contribuinte_icms_status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                    {
                        lstErros.Add("Endereço de entrega: valor de contribuinte do ICMS inválido!");
                    }

                    if (endEtg.EndEtg_contribuinte_icms_status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                        lstErros.Add("Endereço de entrega: para ser cadastrado como Produtor Rural, " +
                            "é necessário ser contribuinte do ICMS e possuir nº de IE!");

                    if (string.IsNullOrEmpty(endEtg.EndEtg_contribuinte_icms_status.ToString()))
                        lstErros.Add("Endereço de entrega: informe se o cliente é contribuinte do ICMS, " +
                            "não contribuinte ou isento!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        string.IsNullOrEmpty(endEtg.EndEtg_ie))
                        lstErros.Add("Endereço de entrega: se o cliente é contribuinte do ICMS a " +
                            "inscrição estadual deve ser preenchida!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                        endEtg.EndEtg_ie.IndexOf("ISEN") > -1)
                        lstErros.Add("Endereço de entrega: se cliente é não contribuinte do ICMS, " +
                            "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        endEtg.EndEtg_ie.IndexOf("ISEN") > -1)
                        lstErros.Add("Endereço de entrega: se cliente é contribuinte do ICMS, " +
                            "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO &&
                        !string.IsNullOrEmpty(endEtg.EndEtg_ie))
                        lstErros.Add("Endereço de entrega: se o Contribuinte ICMS é isento, " +
                            "o campo IE deve ser vazio!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        endEtg.EndEtg_produtor_rural_status != (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                        lstErros.Add("Endereço de entrega: se cliente é contribuinte do ICMS, ele dever ser Produtor Rural!");

                }
            }
        }

        private void ValidarDadosPessoaEnderecoEntrega_PF_Tel(EnderecoEntregaDtoClienteCadastro endEtg, List<string> lstErros)
        {
            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_res) || !string.IsNullOrEmpty(endEtg.EndEtg_ddd_res))
            {
                if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_res).Length < 6)
                    lstErros.Add("Endereço de entrega: telefone residencial inválido.");

                if (string.IsNullOrEmpty(endEtg.EndEtg_ddd_res))
                {
                    lstErros.Add("Endereço de entrega: preencha o ddd residencial.");
                }
                else
                {
                    if (endEtg.EndEtg_ddd_res.Length != 2)
                        lstErros.Add("Endereço de entrega: ddd residencial inválido.");
                }

            }

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_cel) || !string.IsNullOrEmpty(endEtg.EndEtg_tel_cel))
            {
                if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_cel).Length < 6)
                    lstErros.Add("Endereço de entrega: telefone celular inválido.");

                if (string.IsNullOrEmpty(endEtg.EndEtg_ddd_cel))
                {
                    lstErros.Add("Endereço de entrega: preencha o ddd do celular.");
                }
                else
                {
                    if (endEtg.EndEtg_ddd_cel.Length != 2)
                        lstErros.Add("Endereço de entrega: ddd do celular inválido.");
                }
            }

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2) || !string.IsNullOrEmpty(endEtg.EndEtg_tel_com_2))
            {
                lstErros.Add("Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial 2.");
            }
        }
    }
}
