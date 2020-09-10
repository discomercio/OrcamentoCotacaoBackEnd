using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco;
using UtilsGlobais;
using Cep;
using Produto;
using Prepedido.Dados.DetalhesPrepedido;
using Produto.Dados;

namespace Prepedido
{
    public class ValidacoesPrepedidoBll
    {
        private readonly CoeficienteBll coeficienteBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly Cliente.ClienteBll clienteBll;
        private readonly CepBll cepBll;
        private readonly IBancoNFeMunicipio bancoNFeMunicipio;

        public ValidacoesPrepedidoBll(CoeficienteBll coeficienteBll, InfraBanco.ContextoBdProvider contextoProvider,
            Cliente.ClienteBll clienteBll, CepBll cepBll, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            this.coeficienteBll = coeficienteBll;
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
            this.cepBll = cepBll;
            this.bancoNFeMunicipio = bancoNFeMunicipio;
        }

        //vamos validar os produtos que foram enviados
        public async Task MontarProdutosParaComparacao(PrePedidoDados prepedido,
                    string siglaFormaPagto, int qtdeParcelas, string loja, List<string> lstErros, float perc_limite_RA,
                    decimal limiteArredondamento)
        {
            List<PrepedidoProdutoPrepedidoDados> lstProdutosParaComparacao = new List<PrepedidoProdutoPrepedidoDados>();
            List<List<CoeficienteDados>> lstCoeficienteDadosArclube = new List<List<CoeficienteDados>>();
            List<CoeficienteDados> coefDadosArclube = new List<CoeficienteDados>();

            List<string> lstFornec = new List<string>();
            lstFornec = prepedido.ListaProdutos.Select(x => x.Fabricante).Distinct().ToList();

            List<CoeficienteDados> lstCoeficiente = new List<CoeficienteDados>();
            //precisa verificar se a forma de pagto é diferente de av para não dar erro na validação

            //buscar coeficiente 
            //vamos alterar para montar uma lista de coeficiente para avista
            lstCoeficiente = (await MontarListaCoeficiente(lstFornec, qtdeParcelas, siglaFormaPagto)).ToList();

            //validar se os coeficientes estão ok
            ValidarCustoFinancFornecCoeficiente(prepedido.ListaProdutos, lstCoeficiente, lstErros);


            if (lstErros.Count == 0)
            {
                //estamos verificando se o produto é composto
                List<PrepedidoProdutoPrepedidoDados> lstProdutosCompare =
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

        public async Task<IEnumerable<CoeficienteDados>> MontarListaCoeficiente(List<string> lstFornec,
            int qtdeParcelas, string siglaFormaPagto)
        {
            List<CoeficienteDados> lstcoefDados = new List<CoeficienteDados>();

            if (siglaFormaPagto == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
            {
                //vamos montar com o coeficiente = 1
                lstcoefDados = BuscarListaCoeficientesFornecedoresAVista(lstFornec, qtdeParcelas);
            }
            else
            {
                //vamos montar normalmente
                lstcoefDados =
                    (await BuscarListaCoeficientesFornecedores(lstFornec, qtdeParcelas, siglaFormaPagto)).ToList();
            }

            return lstcoefDados;
        }

        private void CalcularProdutoSemCoeficiente(List<PrepedidoProdutoPrepedidoDados> lstProdutos)
        {
            lstProdutos.ForEach(y =>
                {
                    //vamos calcular o preco_lista com o coeficiente
                    y.NormalizacaoCampos_CustoFinancFornecPrecoListaBase = y.NormalizacaoCampos_CustoFinancFornecPrecoListaBase;
                    y.NormalizacaoCampos_Preco_Lista = Math.Round((decimal)y.NormalizacaoCampos_CustoFinancFornecPrecoListaBase * 1, 2);
                    y.NormalizacaoCampos_Preco_Venda = Math.Round(y.NormalizacaoCampos_Preco_Lista * (decimal)(1 - y.NormalizacaoCampos_Desc_Dado / 100), 2);
                    y.TotalItem = Math.Round((decimal)(y.NormalizacaoCampos_Preco_Venda * y.Qtde), 2);
                    y.TotalItemRA = Math.Round((decimal)(y.NormalizacaoCampos_Preco_Lista * y.Qtde), 2);
                    y.CustoFinancFornecCoeficiente = 1;
                });
        }

        private void CalcularProdutoComCoeficiente(List<PrepedidoProdutoPrepedidoDados> lstProdutos,
            List<CoeficienteDados> lstCoeficiente)
        {
            lstCoeficiente.ForEach(x =>
            {
                lstProdutos.ForEach(y =>
                {
                    if (x.Fabricante == y.Fabricante)
                    {
                        //vamos calcular o preco_lista com o coeficiente
                        y.NormalizacaoCampos_CustoFinancFornecPrecoListaBase = y.NormalizacaoCampos_CustoFinancFornecPrecoListaBase;
                        y.NormalizacaoCampos_Preco_Lista = Math.Round(((decimal)y.NormalizacaoCampos_CustoFinancFornecPrecoListaBase * (decimal)x.Coeficiente), 2);
                        y.NormalizacaoCampos_Preco_Venda = Math.Round(y.NormalizacaoCampos_Preco_Lista * (decimal)(1 - y.NormalizacaoCampos_Desc_Dado / 100), 2);
                        y.TotalItem = Math.Round((decimal)(y.NormalizacaoCampos_Preco_Venda * y.Qtde), 2);
                        y.TotalItemRA = Math.Round((decimal)(y.NormalizacaoCampos_Preco_Lista * y.Qtde), 2);
                        y.CustoFinancFornecCoeficiente = x.Coeficiente;
                    }
                });
            });
        }

        public void ValidarCustoFinancFornecCoeficiente(List<PrepedidoProdutoPrepedidoDados> lstProdutos,
            List<CoeficienteDados> lstCoeficiente, List<string> lstErros)
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

        public async Task<IEnumerable<CoeficienteDados>> BuscarListaCoeficientesFornecedores(List<string> lstFornecedores, int qtdeParcelas, string siglaFP)
        {
            List<CoeficienteDados> lstcoefDados = new List<CoeficienteDados>();

            var lstcoeficientesTask = coeficienteBll.BuscarListaCoeficientesFornecedores(lstFornecedores);
            if (lstcoeficientesTask != null)
            {
                foreach (var i in await lstcoeficientesTask)
                {
                    //lstcoefDados = new List<CoeficienteDados>();
                    foreach (var y in i)
                    {
                        if (y.TipoParcela == siglaFP && y.QtdeParcelas == qtdeParcelas)
                        {
                            lstcoefDados.Add(new CoeficienteDados()
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

            return lstcoefDados;
        }

        public List<CoeficienteDados> BuscarListaCoeficientesFornecedoresAVista(List<string> lstFornecedores, int qtdeParcelas)
        {
            List<CoeficienteDados> lstcoefDados = new List<CoeficienteDados>();

            if (lstFornecedores != null)
            {
                foreach (var i in lstFornecedores)
                {
                    lstcoefDados.Add(new CoeficienteDados()
                    {
                        Fabricante = i,
                        TipoParcela = Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA,
                        QtdeParcelas = 1,
                        Coeficiente = 1
                    });
                }
            }

            return lstcoefDados;
        }

        private async Task<IEnumerable<PrepedidoProdutoPrepedidoDados>> BuscarProdutos(List<PrepedidoProdutoPrepedidoDados> lstProdutos,
            string loja, List<string> lstErros)
        {


            var db = contextoProvider.GetContextoLeitura();
            List<PrepedidoProdutoPrepedidoDados> lsProdutosCompare = new List<PrepedidoProdutoPrepedidoDados>();

            foreach (var x in lstProdutos)
            {
                //vamos verificar se o cód do produto é composto
                if (await VerificarProdutoComposto(x, loja, lstErros))
                {
                    PrepedidoProdutoPrepedidoDados produto = await (from c in db.TprodutoLojas
                                                                  where c.Produto == x.NormalizacaoCampos_Produto &&
                                                                        c.Fabricante == x.Fabricante &&
                                                                        c.Vendavel == "S" &&
                                                                        c.Loja == loja
                                                                  select new PrepedidoProdutoPrepedidoDados
                                                                  {
                                                                      Fabricante = c.Fabricante,
                                                                      NormalizacaoCampos_Produto = c.Produto,
                                                                      NormalizacaoCampos_CustoFinancFornecPrecoListaBase = c.Preco_Lista,
                                                                      NormalizacaoCampos_Desc_Dado = x.NormalizacaoCampos_Desc_Dado,
                                                                      Qtde = x.Qtde
                                                                  }).FirstOrDefaultAsync();

                    if (produto != null)
                    {
                        lsProdutosCompare.Add(produto);
                    }
                    else
                    {
                        lstErros.Add("Produto cód.(" + x.NormalizacaoCampos_Produto + ") do fabricante cód.(" + x.Fabricante + ") não existe!");
                    }
                }
            }

            return lsProdutosCompare;
        }

        private async Task<bool> VerificarProdutoComposto(PrepedidoProdutoPrepedidoDados produto, string loja, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();


            var prodCompostoTask = await (from c in db.TecProdutoCompostos
                                          where c.Produto_Composto == produto.NormalizacaoCampos_Produto &&
                                          c.Fabricante_Composto == produto.Fabricante
                                          select c).FirstOrDefaultAsync();

            if (prodCompostoTask != null)
            {
                lstErros.Add("Produto cód.(" + produto.NormalizacaoCampos_Produto + ") do fabricante cód.(" + produto.Fabricante + ") " +
                    "é um produto composto. Para cadastrar produtos compostos é necessário enviar os produtos individualmente!");

                return false;
            }

            return true;
        }
        private void ConfrontarProdutos(PrePedidoDados prepedido,
            List<PrepedidoProdutoPrepedidoDados> lstProdutosCompare, List<string> lstErros,
            decimal limiteArredondamento)
        {
            prepedido.ListaProdutos.ForEach(x =>
            {
                lstProdutosCompare.ForEach(y =>
               {
                   if (x.NormalizacaoCampos_Produto == y.NormalizacaoCampos_Produto && x.Fabricante == y.Fabricante)
                   {
                       //vamos confrontar os valores
                       if (x.NormalizacaoCampos_CustoFinancFornecPrecoListaBase.HasValue && y.NormalizacaoCampos_CustoFinancFornecPrecoListaBase.HasValue && Math.Abs(x.NormalizacaoCampos_CustoFinancFornecPrecoListaBase.Value - y.NormalizacaoCampos_CustoFinancFornecPrecoListaBase.Value) > limiteArredondamento)
                           lstErros.Add($"Preço do fabricante (CustoFinancFornecPrecoListaBase {x.NormalizacaoCampos_CustoFinancFornecPrecoListaBase} x {y.NormalizacaoCampos_CustoFinancFornecPrecoListaBase}) está incorreto!");

                       if (x.NormalizacaoCampos_Preco_Lista != y.NormalizacaoCampos_Preco_Lista)
                           lstErros.Add($"Custo financeiro preço lista base (Preco_Lista " +
                               $"{string.Format("{0:c}", x.NormalizacaoCampos_Preco_Lista)} x {string.Format("{0:c}", y.NormalizacaoCampos_Preco_Lista)}) esta incorreto!");

                       //veio da Unis e vamos validar
                       if (x.Preco_NF != null)
                       {
                           if (prepedido.PermiteRAStatus != 1)
                           {
                               if (x.Preco_NF != x.NormalizacaoCampos_Preco_Venda)
                                   lstErros.Add($"Preço de nota fiscal (Preco_NF {string.Format("{0:c}", x.Preco_NF)} x {x.NormalizacaoCampos_Preco_Venda}) está incorreto!");
                           }                           

                       }

                       if (Math.Abs(x.NormalizacaoCampos_Preco_Venda - y.NormalizacaoCampos_Preco_Venda) > limiteArredondamento)
                           lstErros.Add($"Preço do fabricante (Preco_Venda {x.NormalizacaoCampos_Preco_Venda} x {y.NormalizacaoCampos_Preco_Venda}) está incorreto!");
                   }
               });
            });
        }

        private void ConfrontarTotaisEPercentualMaxRA(PrePedidoDados prepedido, List<string> lstErros,
            float perc_limite_RA)
        {
            decimal totalCompare = 0;
            decimal totalRaCompare = 0;

            prepedido.ListaProdutos.ForEach(x =>
            {
                totalCompare += Math.Round((decimal)(x.NormalizacaoCampos_Preco_Venda * x.Qtde), 2);
                totalRaCompare += Math.Round((decimal)(x.Preco_NF * x.Qtde), 2);
            });

            if (totalCompare != (decimal)prepedido.NormalizacaoCampos_Vl_total)
                lstErros.Add("Os valores totais estão divergindo!");

            if (prepedido.PermiteRAStatus == 1)
            {
                if (totalRaCompare != (decimal)prepedido.NormalizacaoCampos_Vl_total_NF)
                    lstErros.Add("Os valores totais de RA estão divergindo!");

                //vamos verificar o valor de RA
                decimal ra = totalRaCompare - totalCompare;
                decimal perc = Math.Round((decimal)(perc_limite_RA / 100), 2);
                decimal percentual = Math.Round(perc * (decimal)prepedido.NormalizacaoCampos_Vl_total, 2);

                if (ra > percentual)
                    lstErros.Add("O valor total de RA excede o limite permitido!");
            }
        }


        public async Task<bool> ValidarEnderecoEntrega(PrePedidoDados prepedido, List<string> lstErros)
        {
            bool retorno = true;

            if (prepedido.EnderecoEntrega.OutroEndereco)
            {
                await ValidarDadosEnderecoEntrega(prepedido, lstErros, contextoProvider);

                ValidarDadosPessoaEnderecoEntrega(prepedido, lstErros, false);

                if (lstErros.Count != 0)
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        private async Task ValidarDadosEnderecoEntrega(PrePedidoDados prePedido, List<string> lstErros,
            ContextoBdProvider contextoProvider)
        {

            if (prePedido.EnderecoEntrega.OutroEndereco)
            {
                //verificar se a justificativa esta correta "ListarComboJustificaEndereco"
                List<Cliente.Dados.EnderecoEntregaJustificativaDados> lstJustificativas =
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
                        List<Cep.Dados.CepDados> lstCepDados = (await cepBll.BuscarPorCep(cepSoDigito)).ToList();

                        if (lstCepDados.Count == 0)
                        {
                            lstErros.Add("Endereço Entrega: cep inválido!");
                        }
                        else
                        {
                            Cep.Dados.CepDados cep = new Cep.Dados.CepDados()
                            {
                                Cep = prePedido.EnderecoEntrega.EndEtg_cep,
                                Endereco = prePedido.EnderecoEntrega.EndEtg_endereco,
                                Bairro = prePedido.EnderecoEntrega.EndEtg_bairro,
                                Cidade = prePedido.EnderecoEntrega.EndEtg_cidade,
                                Uf = prePedido.EnderecoEntrega.EndEtg_uf
                            };
                            await Cliente.ValidacoesClienteBll.VerificarEndereco(cep, lstCepDados, lstErros, contextoProvider,
                                bancoNFeMunicipio);
                        }

                        await CepBll.ConsisteMunicipioIBGE(prePedido.EnderecoEntrega.EndEtg_cidade,
                            prePedido.EnderecoEntrega.EndEtg_uf, lstErros, contextoProvider, bancoNFeMunicipio, true);
                    }
                }
                else
                {
                    lstErros.Add("Código da justficativa inválida!");
                }
            }
        }

        public bool ValidarDetalhesPrepedido(DetalhesPrepedidoDados detalhesPrepedido, List<string> lstErros)
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

        private void ValidarDadosPessoaEnderecoEntrega(PrePedidoDados prePedido, List<string> lstErros,
            bool flagMsg_IE_Cadastro_PF)
        {
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

            if (prePedido.DadosCliente.Tipo == Constantes.ID_PF)
            {
                if (prePedido.EnderecoEntrega.EndEtg_tipo_pessoa != Constantes.ID_PF)
                {
                    lstErros.Add("Endereço de Entrega: se cliente é tipo PF, o tipo de pessoa do endereço de " +
                        "entrega deve ser PF.");
                }
            }

            if (lstErros.Count == 0)
            {
                //só validamos o IE se o cliente for PJ
                //Cliente PF aceita IE com estado do endereço de endereço de entrega diferente
                if (prePedido.DadosCliente.Tipo == Constantes.ID_PJ)
                {
                    if (!string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_ie))
                    {
                        Cliente.ValidacoesClienteBll.VerificarInscricaoEstadualValida(
                            prePedido.EnderecoEntrega.EndEtg_ie, prePedido.EnderecoEntrega.EndEtg_uf, lstErros, flagMsg_IE_Cadastro_PF);
                    }
                }
            }
        }

        private void ValidarDadosPessoaEnderecoEntrega_PJ(Cliente.Dados.EnderecoEntregaClienteCadastroDados endEtg, List<string> lstErros)
        {
            if (endEtg.EndEtg_tipo_pessoa == Constantes.ID_PJ)
            {
                if (string.IsNullOrEmpty(endEtg.EndEtg_cnpj_cpf) ||
                    !Util.ValidaCNPJ(endEtg.EndEtg_cnpj_cpf))
                    lstErros.Add("Endereço de entrega: CNPJ inválido!");

                if (endEtg.EndEtg_produtor_rural_status != 0)
                {
                    lstErros.Add("Endereço de entrega: Se tipo pessoa é PJ, não pode ser Produtor Rural!");
                }
                if (!string.IsNullOrEmpty(endEtg.EndEtg_rg))
                {
                    lstErros.Add("Endereço de entrega: Se tipo pessoa é PJ, não pode ter RG preenchido!");
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
            }
        }

        private void ValidarDadosPessoaEnderecoEntrega_PJ_Tel(Cliente.Dados.EnderecoEntregaClienteCadastroDados endEtg, List<string> lstErros)
        {
            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_ddd_com) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_ramal_com))
            {
                if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com) &&
                    string.IsNullOrEmpty(endEtg.EndEtg_ddd_com))
                {
                    lstErros.Add("Endereço de entrega: preencha o ddd do telefone comercial!");
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_tel_com) &&
                    !string.IsNullOrEmpty(endEtg.EndEtg_ddd_com))
                {
                    lstErros.Add("Endereço de entrega: preencha o telefone comercial!");
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_tel_com) &&
                    string.IsNullOrEmpty(endEtg.EndEtg_ddd_com) &&
                    !string.IsNullOrEmpty(endEtg.EndEtg_ramal_com))
                {
                    lstErros.Add("Endereço de entrega: Ramal do telefone comercial preenchido " +
                        "sem telefone comercial");
                }

                if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com) &&
                    !string.IsNullOrEmpty(endEtg.EndEtg_ddd_com))
                {
                    if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_com).Length < 6)
                        lstErros.Add("Endereço de entrega: telefone comercial inválido!");

                    if (endEtg.EndEtg_ddd_com.Length != 2)
                        lstErros.Add("Endereço de entrega: ddd do telefone comercial inválido!");
                }
            }

            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com_2) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_ramal_com_2))
            {
                if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com_2) &&
                    string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2))
                {
                    lstErros.Add("Endereço de entrega: preencha o ddd do telefone comercial 2!");
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_tel_com_2) &&
                    !string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2))
                {
                    lstErros.Add("Endereço de entrega: preencha o telefone comercial 2!");
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_tel_com_2) &&
                    string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2) &&
                    !string.IsNullOrEmpty(endEtg.EndEtg_ramal_com_2))
                {
                    lstErros.Add("Endereço de entrega: Ramal do telefone comercial 2 preenchido " +
                        "sem telefone comercial 2!");
                }

                if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com_2) &&
                    !string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2))
                {
                    if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_com_2).Length < 6)
                        lstErros.Add("Endereço de entrega: telefone comercial 2 inválido!");

                    if (endEtg.EndEtg_ddd_com_2.Length != 2)
                        lstErros.Add("Endereço de entrega: ddd do telefone comercial 2 inválido!");
                }
            }

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_res) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_tel_res))
                lstErros.Add("Endereço de entrega: se tipo pessoa PJ, não pode conter DDD residencial e telefone residencial!");

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_cel) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_tel_cel))
                lstErros.Add("Endereço de entrega: se tipo pessoa PJ, não pode conter DDD celular e telefone celular!");
        }

        private void ValidarDadosPessoaEnderecoEntrega_PF(Cliente.Dados.EnderecoEntregaClienteCadastroDados endEtg, List<string> lstErros)
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

                    
                }

                if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        endEtg.EndEtg_produtor_rural_status != (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                    lstErros.Add("Endereço de entrega: se cliente é contribuinte do ICMS, ele dever ser Produtor Rural!");

                if (endEtg.EndEtg_produtor_rural_status ==
                    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO &&
                    endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF)
                {
                    if (endEtg.EndEtg_contribuinte_icms_status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL)
                    {
                        lstErros.Add("Endereço de entrega: se cliente é não produtor rural, contribuinte do " +
                            "ICMS deve ter o valor inicial!");
                    }
                    if (!string.IsNullOrEmpty(endEtg.EndEtg_ie))
                    {
                        lstErros.Add("Endereço de entrega: se cliente é não produtor rural, o IE " +
                            "deve ser preenchido!");
                    }
                }
            }
        }

        private void ValidarDadosPessoaEnderecoEntrega_PF_Tel(Cliente.Dados.EnderecoEntregaClienteCadastroDados endEtg, List<string> lstErros)
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

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_com) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_tel_com) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_ramal_com))
            {
                lstErros.Add("Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial.");
            }
            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_tel_com_2) ||
                !string.IsNullOrEmpty(endEtg.EndEtg_ramal_com_2))
            {
                lstErros.Add("Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial 2.");
            }
        }
    }
}
