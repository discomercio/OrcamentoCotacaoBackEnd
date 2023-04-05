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
using Cep.Dados;

namespace Prepedido.Bll
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


        public enum AmbienteValidacao
        {
            PrepedidoValidacao, PedidoValidacao
        }

        //vamos validar os produtos que foram enviados
        public async Task MontarProdutosParaComparacao(PrePedidoDados prepedido,
                    string siglaFormaPagto, int qtdeParcelas, string loja, List<string> lstErros, decimal percVlPedidoRA,
                    decimal limiteArredondamento,
                    AmbienteValidacao ambienteValidacao)
        {


            List<PrepedidoProdutoPrepedidoDados> lstProdutosParaComparacao = new List<PrepedidoProdutoPrepedidoDados>();
            List<List<CoeficienteDados>> lstCoeficienteDadosArclube = new List<List<CoeficienteDados>>();
            List<CoeficienteDados> coefDadosArclube = new List<CoeficienteDados>();

            List<string> lstFornec = new List<string>();
            lstFornec = prepedido.ListaProdutos.Select(x => x.Fabricante).Distinct().ToList();

            List<CoeficienteDados> lstCoeficiente;
            //precisa verificar se a forma de pagto é diferente de av para não dar erro na validação

            //buscar coeficiente 
            //vamos alterar para montar uma lista de coeficiente para avista
            lstCoeficiente = (await MontarListaCoeficiente(lstFornec, qtdeParcelas, siglaFormaPagto)).ToList();

            //validar se os coeficientes estão ok
            ValidarCustoFinancFornecCoeficiente(prepedido.ListaProdutos, lstCoeficiente, lstErros);

            //valida valores zerados ou negativos
            ValidarPrecoQuantidadeZerada(prepedido.ListaProdutos, lstErros);

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
                ConfrontarTotaisEPercentualMaxRA(prepedido, lstErros, percVlPedidoRA, ambienteValidacao);
            }
        }

        public async Task<string> ValidarVendaCondicional(List<PrepedidoProdutoPrepedidoDados> listaProdutos)
        {
            string retorno = string.Empty;
            var proporcaoListaProdutos = await Util.BuscarRegistroParametro(Constantes.ID_PARAMETRO_VendaCondicionada_RegraProporcao_ListaProdutos, contextoProvider);
            var proporcaoPercentualMaximo = await Util.BuscarRegistroParametro(Constantes.ID_PARAMETRO_VendaCondicionada_RegraProporcao_PercentualMaximoPedido, contextoProvider);

            var vlTotalPrecoLista = 0m;
            var vlTotalVendaCondicionada = 0m;
            var qtdeProdutosCondicionados = 0;
            string produtosCondicionados = string.Empty;
            foreach (var item in listaProdutos)
            {
                vlTotalPrecoLista += (decimal)(item.Qtde * item.Preco_Lista);
                if (proporcaoListaProdutos.Campo_texto.IndexOf(item.Produto) > -1)
                {
                    qtdeProdutosCondicionados++;
                    vlTotalVendaCondicionada += (decimal)(item.Qtde * item.Preco_Lista);
                    if (!string.IsNullOrEmpty(produtosCondicionados)) produtosCondicionados += ", ";

                    produtosCondicionados += item.Produto;
                }
            }

            if (vlTotalVendaCondicionada > 0)
            {
                if (qtdeProdutosCondicionados > 0 &&
                    (vlTotalVendaCondicionada / vlTotalPrecoLista) > (decimal)(proporcaoPercentualMaximo.Campo_Real / 100))
                {
                    if (qtdeProdutosCondicionados > 1) retorno = $"Os produtos {produtosCondicionados} não podem ser vendidos neste pedido!";
                    else retorno = $"O produto {produtosCondicionados} não pode ser vendido neste pedido!";
                }
            }

            return retorno;
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
                    y.Preco_Lista = Math.Round((decimal)y.CustoFinancFornecPrecoListaBase * 1, 2);
                    y.Preco_Venda = Math.Round(y.Preco_Lista * (decimal)(1 - y.Desc_Dado / 100), 2);
                    y.TotalItem = Math.Round((decimal)(y.Preco_Venda * y.Qtde), 2);
                    y.TotalItemRA = Math.Round((decimal)(y.Preco_NF * y.Qtde), 2);
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
                        y.Preco_Lista = Math.Round(((decimal)y.CustoFinancFornecPrecoListaBase * (decimal)x.Coeficiente), 2);
                        y.Preco_Venda = Math.Round(y.Preco_Lista * (decimal)(1 - y.Desc_Dado / 100), 2);
                        y.TotalItem = Math.Round((decimal)(y.Preco_Venda * y.Qtde), 2);
                        y.TotalItemRA = Math.Round((decimal)(y.Preco_NF * y.Qtde), 2);
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
                            lstErros.Add("Coeficiente do fabricante (" + x.Fabricante + ") está incorreto!");
                    });
                }
                else
                {
                    lstErros.Add("Fabricante cód.(" + x.Fabricante + ") não possui cadastro de coeficiente!");
                }
            });
        }

        public void ValidarPrecoQuantidadeZerada(List<PrepedidoProdutoPrepedidoDados> lstProdutos, List<string> lstErros)
        {
            foreach (var p in lstProdutos)
            {
                if (p.Qtde <= 0)
                    lstErros.Add($"Produto {p.Fabricante} {p.Produto} com Qtde menor ou igual a zero!");
                if (p.Preco_Lista <= 0)
                    lstErros.Add($"Produto {p.Fabricante} {p.Produto} com Preco_Lista menor ou igual a zero!");
                if (p.Preco_NF <= 0)
                    lstErros.Add($"Produto {p.Fabricante} {p.Produto} com Preco_NF menor ou igual a zero!");
                if (p.Preco_Venda <= 0)
                    lstErros.Add($"Produto {p.Fabricante} {p.Produto} com Preco_Venda menor ou igual a zero!");
            }

            //# loja/PedidoNovoConsiste.asp
            //# alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": linha " & renumera_com_base1(Lbound(v_item),i) & " repete o mesmo produto da linha " & renumera_com_base1(Lbound(v_item),j) & "."
            var agrupados = (from p in lstProdutos group p by new { p.Fabricante, p.Produto } into g select g).ToList();
            var repetidos = (from p in agrupados where p.Count() > 1 select p.Key).ToList();
            //para saber a linha precisamos de um loop
            for (int ilinha = 0; ilinha < lstProdutos.Count(); ilinha++)
            {
                var esteproduto = lstProdutos[ilinha];
                if (!(from repetido in repetidos where repetido.Produto == esteproduto.Produto && repetido.Fabricante == esteproduto.Fabricante select repetido).Any())
                    continue;

                //está repetido sim
                for (int ilinha2 = ilinha + 1; ilinha2 < lstProdutos.Count(); ilinha2++)
                {
                    var esterepetido = lstProdutos[ilinha2];
                    if (esteproduto.Fabricante == esterepetido.Fabricante && esteproduto.Produto == esterepetido.Produto)
                    {
                        var msg = "Produto " + esteproduto.Produto + " do fabricante " + esteproduto.Fabricante
                            + ": linha " + (ilinha + 1).ToString() + " repete o mesmo produto da linha " + (ilinha2 + 1).ToString() + ".";
                        lstErros.Add(msg);
                    }
                }
            }
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

        public async Task<IEnumerable<PrepedidoProdutoPrepedidoDados>> BuscarProdutos(
            List<PrepedidoProdutoPrepedidoDados> lstProdutos,
            string loja, 
            List<string> lstErros)
        {
            var lsProdutosCompare = new List<PrepedidoProdutoPrepedidoDados>();

            using (var db = contextoProvider.GetContextoLeitura())
            {
                foreach (var x in lstProdutos)
                {
                    //vamos verificar se o cód do produto é composto
                    if (await VerificarProdutoComposto(x, loja, lstErros))
                    {
                        PrepedidoProdutoPrepedidoDados produto = await (from c in db.TprodutoLoja
                                                                        where c.Produto == x.Produto &&
                                                                              c.Fabricante == x.Fabricante &&
                                                                              c.Vendavel == "S" &&
                                                                              c.Loja == loja &&
                                                                              c.Excluido_status == 0
                                                                        select new PrepedidoProdutoPrepedidoDados
                                                                        {
                                                                            Fabricante = c.Fabricante,
                                                                            Produto = c.Produto,
                                                                            CustoFinancFornecPrecoListaBase = c.Preco_Lista ?? 0,
                                                                            Desc_Dado = x.Desc_Dado,
                                                                            Qtde = x.Qtde
                                                                        }).FirstOrDefaultAsync();

                        if (produto != null)
                        {
                            lsProdutosCompare.Add(produto);
                        }
                        else
                        {
                            lstErros.Add("Produto cód.(" + x.Produto + ") do fabricante cód.(" + x.Fabricante + ") não existe!");
                        }
                    }
                }
            }

            return lsProdutosCompare;
        }

        private async Task<bool> VerificarProdutoComposto(PrepedidoProdutoPrepedidoDados produto, string loja, List<string> lstErros)
        {
            TecProdutoComposto prodCompostoTask;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                prodCompostoTask = await (from c in db.TecProdutoComposto
                                              where c.Produto_Composto == produto.Produto &&
                                              c.Fabricante_Composto == produto.Fabricante
                                              select c).FirstOrDefaultAsync();
            }

            if (prodCompostoTask != null)
            {
                lstErros.Add("Produto cód.(" + produto.Produto + ") do fabricante cód.(" + produto.Fabricante + ") " +
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
                   if (x.Produto == y.Produto && x.Fabricante == y.Fabricante)
                   {
                       //vamos confrontar os valores
                       if (Math.Abs(x.CustoFinancFornecPrecoListaBase - y.CustoFinancFornecPrecoListaBase) > limiteArredondamento)
                           lstErros.Add($"Preço do fabricante (CustoFinancFornecPrecoListaBase {x.CustoFinancFornecPrecoListaBase} x {string.Format("{0:C}", y.CustoFinancFornecPrecoListaBase)}) está incorreto!");

                       if (x.Preco_Lista != y.Preco_Lista)
                           lstErros.Add($"Custo financeiro preço lista base (Preco_Lista " +
                               $"{string.Format("{0:c}", x.Preco_Lista)} x {string.Format("{0:c}", y.Preco_Lista)}) esta incorreto!");

                       //validar que Preco_NF não tenha RA
                       if (prepedido.PermiteRAStatus != 1)
                       {
                           if (x.Preco_NF != x.Preco_Venda)
                               lstErros.Add($"Preço de nota fiscal (Preco_NF {string.Format("{0:c}", x.Preco_NF)} x Preco_Venda {x.Preco_Venda}) está incorreto!");
                       }

                       if (Math.Abs(x.Preco_Venda - y.Preco_Venda) > limiteArredondamento)
                           lstErros.Add($"Preço do fabricante (Preco_Venda {x.Preco_Venda} x {y.Preco_Venda}) está incorreto!");
                   }
               });
            });
        }

        private void ConfrontarTotaisEPercentualMaxRA(PrePedidoDados prepedido, List<string> lstErros,
            decimal percVlPedidoRA, AmbienteValidacao ambienteValidacao)
        {
            decimal totalCompare = 0;
            decimal totalRaCompare = 0;

            prepedido.ListaProdutos.ForEach(x =>
            {
                var desc = Math.Round(1 - x.Desc_Dado / 100, 4);
                var precoVenda = Math.Round((decimal)(x.Preco_Lista * x.Qtde) * (decimal)desc, 2);
                totalCompare += precoVenda;
                totalRaCompare += Math.Round((decimal)(x.Preco_NF * (x.Qtde ?? 0)), 2);
            });

            if (totalCompare != (decimal)prepedido.Vl_total)
                lstErros.Add("Os valores totais estão divergindo!");

            if (prepedido.PermiteRAStatus == 1)
            {
                if (totalRaCompare != (decimal)prepedido.Vl_total_NF)
                    lstErros.Add("Os valores totais de RA estão divergindo!");

                if (ambienteValidacao == AmbienteValidacao.PrepedidoValidacao)
                {
                    //vamos verificar o valor de RA
                    decimal ra = totalRaCompare - totalCompare;
                    decimal perc = Math.Round((decimal)(percVlPedidoRA / 100), 2);
                    decimal percentual = Math.Round(perc * (decimal)prepedido.Vl_total, 2);

                    if (ra > percentual)
                        lstErros.Add("O valor total de RA excede o limite permitido!");
                }
            }
        }


        public async Task ValidarEnderecoEntrega(Cliente.Dados.EnderecoEntregaClienteCadastroDados endEntrega,
            List<string> lstErros, string orcamentista, string tipoCliente, bool usarLojaOrcamentista, string loja,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel tipoValidacaoEndereco)
        {

            if (endEntrega.OutroEndereco)
            {
                await ValidarDadosEnderecoEntrega(
                    endEntrega,
                    lstErros,
                    usarLojaOrcamentista,
                    loja,
                    orcamentista,
                    tipoValidacaoEndereco);

                ValidarDadosPessoaEnderecoEntrega(endEntrega, lstErros, false, tipoCliente);
                VerificarCaracteresInvalidosEnderecoEntregaClienteCadastro(endEntrega, lstErros);
            }
        }

        private void VerificarCaracteresInvalidosEnderecoEntregaClienteCadastro(
            Cliente.Dados.EnderecoEntregaClienteCadastroDados endEtg, List<string> lstErros)
        {
            string caracteres;
            if (UtilsGlobais.Util.IsTextoValido(endEtg.EndEtg_endereco, out caracteres).Length > 0)
                lstErros.Add("O CAMPO 'ENDEREÇO DE ENTREGA' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(endEtg.EndEtg_endereco_numero ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO NÚMERO DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(endEtg.EndEtg_endereco_complemento ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO COMPLEMENTO DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(endEtg.EndEtg_bairro ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO BAIRRO DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(endEtg.EndEtg_cidade ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO CIDADE DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(endEtg.EndEtg_nome ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO NOME DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            //vamos verificar se o endereço de entrega esta com os valores corretos
            if (endEtg.EndEtg_endereco.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                lstErros.Add("ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br> TAMANHO ATUAL: " +
                    endEtg.EndEtg_endereco.Length + " CARACTERES <br> TAMANHO MÁXIMO: " +
                    Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");
        }

        private async Task ValidarDadosEnderecoEntrega(
            Cliente.Dados.EnderecoEntregaClienteCadastroDados endEntrega,
            List<string> lstErros,
            bool usarLojaOrcamentista, 
            string loja, 
            string orcamentista,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel tipoValidacaoEndereco)
        {
            if (!endEntrega.OutroEndereco)
                return;

            if (string.IsNullOrEmpty(endEntrega.EndEtg_cod_justificativa))
                lstErros.Add("SELECIONE A JUSTIFICATIVA DO ENDEREÇO DE ENTREGA!");
            else
            {
                //verificar se a justificativa esta correta "ListarComboJustificaEndereco"
                List<Cliente.Dados.EnderecoEntregaJustificativaDados> lstJustificativas;
                if (usarLojaOrcamentista)
                {
                    orcamentista = orcamentista ?? "";
                    lstJustificativas = (await clienteBll.ListarComboJustificaEndereco(orcamentista)).ToList();
                }
                else
                {
                    loja = loja ?? "";
                    lstJustificativas = (await clienteBll.ListarComboJustificaEnderecoPorLoja(loja)).ToList();
                }
                if (!lstJustificativas.Where(r => r.EndEtg_cod_justificativa == endEntrega.EndEtg_cod_justificativa).Any())
                {
                    lstErros.Add("CÓDIGO DA JUSTFICATIVA INVÁLIDO!");
                }
            }


            if (string.IsNullOrEmpty(endEntrega.EndEtg_endereco))
                lstErros.Add("PREENCHA O ENDEREÇO DE ENTREGA.");

            if (endEntrega.EndEtg_endereco.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                lstErros.Add("ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " + endEntrega.EndEtg_endereco.Length +
                    " CARACTERES<br>TAMANHO MÁXIMO: " + Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");

            if (string.IsNullOrEmpty(endEntrega.EndEtg_endereco_numero))
                lstErros.Add("PREENCHA O NÚMERO DO ENDEREÇO DE ENTREGA.");

            if (string.IsNullOrEmpty(endEntrega.EndEtg_bairro))
                lstErros.Add("PREENCHA O BAIRRO DO ENDEREÇO DE ENTREGA.");

            if (string.IsNullOrEmpty(endEntrega.EndEtg_cidade))
                lstErros.Add("PREENCHA A CIDADE DO ENDEREÇO DE ENTREGA.");

            if (string.IsNullOrEmpty(endEntrega.EndEtg_uf) || !Util.VerificaUf(endEntrega.EndEtg_uf))
                lstErros.Add("UF INVÁLIDA NO ENDEREÇO DE ENTREGA.");

            if (string.IsNullOrEmpty(endEntrega.EndEtg_cep))
                lstErros.Add("INFORME O CEP DO ENDEREÇO DE ENTREGA.");
            else
            {
                if (!Util.VerificaCep(endEntrega.EndEtg_cep))
                    lstErros.Add("CEP INVÁLIDO NO ENDEREÇO DE ENTREGA.");
            }

            if (lstErros.Count == 0)
            {
                //vamos comparar endereço
                Cep.Dados.CepDados cep = new Cep.Dados.CepDados()
                {
                    Cep = endEntrega.EndEtg_cep,
                    Endereco = endEntrega.EndEtg_endereco,
                    Bairro = endEntrega.EndEtg_bairro,
                    Cidade = endEntrega.EndEtg_cidade,
                    Uf = endEntrega.EndEtg_uf
                };
                var msgErro = "Endereço Entrega: cep inválido!";
                await Cliente.ValidacoesClienteBll.VerificarEndereco(lstErros, cepBll, contextoProvider,
                    bancoNFeMunicipio, msgErro, cep, tipoValidacaoEndereco);

                await CepBll.ConsisteMunicipioIBGE(endEntrega.EndEtg_cidade,
                    endEntrega.EndEtg_uf, lstErros, contextoProvider, bancoNFeMunicipio, true);
            }
        }

        public static bool ValidarDetalhesPrepedido(DetalhesPrepedidoDados detalhesPrepedido, List<string> lstErros)
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
                if (detalhesPrepedido.PrevisaoEntrega == null ||
                    detalhesPrepedido.PrevisaoEntrega.Value.Date <= DateTime.Now.Date)
                {
                    lstErros.Add("Favor informar a data de 'Entrega Imediata' posterior a data atual!");
                    retorno = false;
                }
            }

            return retorno;
        }

        private void ValidarDadosPessoaEnderecoEntrega(Cliente.Dados.EnderecoEntregaClienteCadastroDados endEntrega, List<string> lstErros,
            bool flagMsg_IE_Cadastro_PF, string tipoCliente)
        {
            if (endEntrega.EndEtg_tipo_pessoa != Constantes.ID_PJ &&
                endEntrega.EndEtg_tipo_pessoa != Constantes.ID_PF)
            {
                lstErros.Add("Endereço de Entrega: Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!");
            }
            else if (string.IsNullOrEmpty(endEntrega.EndEtg_nome))
            {
                lstErros.Add("Endereço de Entrega: Preencha o nome/razão social no endereço de entrega!");
            }
            else
            {
                if (endEntrega.EndEtg_tipo_pessoa == Constantes.ID_PJ)
                {
                    ValidarDadosPessoaEnderecoEntrega_PJ(endEntrega, lstErros);
                    ValidarDadosPessoaEnderecoEntrega_PJ_Tel(endEntrega, lstErros);
                }
                if (endEntrega.EndEtg_tipo_pessoa == Constantes.ID_PF)
                {
                    ValidarDadosPessoaEnderecoEntrega_PF(endEntrega, lstErros);
                    ValidarDadosPessoaEnderecoEntrega_PF_Tel(endEntrega, lstErros);
                }
            }

            if (tipoCliente == Constantes.ID_PF)
            {
                if (endEntrega.EndEtg_tipo_pessoa != Constantes.ID_PF)
                {
                    lstErros.Add("Endereço de Entrega: se cliente é tipo PF, o tipo de pessoa do endereço de " +
                        "entrega deve ser PF.");
                }
            }

            if (lstErros.Count == 0)
            {
                //só validamos o IE se o cliente for PJ
                //Cliente PF aceita IE com estado do endereço de endereço de entrega diferente
                if (tipoCliente == Constantes.ID_PJ)
                {
                    if ((endEntrega.EndEtg_tipo_pessoa == Constantes.ID_PF && endEntrega.EndEtg_produtor_rural_status ==
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) ||
                    (endEntrega.EndEtg_tipo_pessoa == Constantes.ID_PJ &&
                    endEntrega.EndEtg_contribuinte_icms_status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) ||
                    (endEntrega.EndEtg_tipo_pessoa == Constantes.ID_PJ &&
                    endEntrega.EndEtg_contribuinte_icms_status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                    && !string.IsNullOrEmpty(endEntrega.EndEtg_ie))
                    {
                        Cliente.ValidacoesClienteBll.VerificarInscricaoEstadualValida(endEntrega.EndEtg_ie,
                        endEntrega.EndEtg_uf, lstErros, flagMsg_IE_Cadastro_PF);
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
                    !string.IsNullOrEmpty(endEtg.EndEtg_ie))
                {
                    if (endEtg.EndEtg_contribuinte_icms_status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                    endEtg.EndEtg_ie.IndexOf("ISEN") > -1)
                        lstErros.Add("Endereço de entrega: se cliente é contribuinte do ICMS, " +
                            "não pode ter o valor ISENTO no campo de Inscrição Estadual!");
                }

                if (endEtg.EndEtg_contribuinte_icms_status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO &&
                    !string.IsNullOrEmpty(endEtg.EndEtg_ie))
                    lstErros.Add("Endereço de entrega: se o Contribuinte ICMS é isento, " +
                        "o campo IE deve ser vazio!");

            }
        }

        private void ValidarDadosPessoaEnderecoEntrega_PJ_Tel(Cliente.Dados.EnderecoEntregaClienteCadastroDados endEtg, List<string> lstErros)
        {
            TelefonesEnderecoEntregaSomenteComDigitos(endEtg);

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
                    if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_com).Length < 6 ||
                        Util.Telefone_SoDigito(endEtg.EndEtg_tel_com).Length > 11)
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
                    if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_com_2).Length < 6 ||
                        Util.Telefone_SoDigito(endEtg.EndEtg_tel_com_2).Length > 11)
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
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO &&
                    endEtg.EndEtg_produtor_rural_status !=
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                {
                    lstErros.Add("Endereço de entrega: valor de produtor rural inválido!");
                }

                if (string.IsNullOrEmpty(endEtg.EndEtg_cnpj_cpf) ||
                    !Util.ValidaCPF(endEtg.EndEtg_cnpj_cpf))
                    lstErros.Add("Endereço de entrega: CPF inválido!");

                if (string.IsNullOrEmpty(endEtg.EndEtg_produtor_rural_status.ToString()))
                    lstErros.Add("Endereço de entrega: informe se o cliente é produtor rural ou não!");

                if (endEtg.EndEtg_produtor_rural_status ==
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM &&
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
                    if (!string.IsNullOrEmpty(endEtg.EndEtg_ie))
                    {
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

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO &&
                        !string.IsNullOrEmpty(endEtg.EndEtg_ie))
                        lstErros.Add("Endereço de entrega: se o Contribuinte ICMS é isento, " +
                            "o campo IE deve ser vazio!");


                }

                if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        endEtg.EndEtg_produtor_rural_status != (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                    lstErros.Add("Endereço de entrega: se cliente é contribuinte do ICMS, ele dever ser Produtor Rural!");

                if (endEtg.EndEtg_produtor_rural_status ==
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO &&
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
                            "não deve ser preenchido!");
                    }
                }
            }
        }

        //deixa somente dígitos nos telefones
        private void TelefonesEnderecoEntregaSomenteComDigitos(Cliente.Dados.EnderecoEntregaClienteCadastroDados endEtg)
        {
            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_res))
                endEtg.EndEtg_ddd_res = Util.Telefone_SoDigito(endEtg.EndEtg_ddd_res);

            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_res))
                endEtg.EndEtg_tel_res = Util.Telefone_SoDigito(endEtg.EndEtg_tel_res);

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_com))
                endEtg.EndEtg_ddd_com = Util.Telefone_SoDigito(endEtg.EndEtg_ddd_com);

            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com))
                endEtg.EndEtg_tel_com = Util.Telefone_SoDigito(endEtg.EndEtg_tel_com);

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ramal_com))
                endEtg.EndEtg_ramal_com = Util.Telefone_SoDigito(endEtg.EndEtg_ramal_com);

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_cel))
                endEtg.EndEtg_ddd_cel = Util.Telefone_SoDigito(endEtg.EndEtg_ddd_cel);

            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_cel))
                endEtg.EndEtg_tel_cel = Util.Telefone_SoDigito(endEtg.EndEtg_tel_cel);

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ddd_com_2))
                endEtg.EndEtg_ddd_com_2 = Util.Telefone_SoDigito(endEtg.EndEtg_ddd_com_2);

            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_com_2))
                endEtg.EndEtg_tel_com_2 = Util.Telefone_SoDigito(endEtg.EndEtg_tel_com_2);

            if (!string.IsNullOrEmpty(endEtg.EndEtg_ramal_com_2))
                endEtg.EndEtg_ramal_com_2 = Util.Telefone_SoDigito(endEtg.EndEtg_ramal_com_2);
        }


        private void ValidarDadosPessoaEnderecoEntrega_PF_Tel(Cliente.Dados.EnderecoEntregaClienteCadastroDados endEtg, List<string> lstErros)
        {
            TelefonesEnderecoEntregaSomenteComDigitos(endEtg);
            if (!string.IsNullOrEmpty(endEtg.EndEtg_tel_res) || !string.IsNullOrEmpty(endEtg.EndEtg_ddd_res))
            {
                if (string.IsNullOrEmpty(endEtg.EndEtg_tel_res))
                    lstErros.Add("Endereço de entrega: preencha o telfone residencial.");
                else if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_res).Length < 6 ||
                    Util.Telefone_SoDigito(endEtg.EndEtg_tel_res).Length > 11)
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
                if (string.IsNullOrEmpty(endEtg.EndEtg_tel_cel))
                    lstErros.Add("Endereço de entrega: preencha o telefone do celular.");
                else if (Util.Telefone_SoDigito(endEtg.EndEtg_tel_cel).Length < 6 ||
                    Util.Telefone_SoDigito(endEtg.EndEtg_tel_cel).Length > 11)
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
