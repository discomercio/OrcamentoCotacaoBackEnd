using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Pedido.Dados.Criacao;
using Prepedido.Dados.FormaPagto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo40
{
    class Passo40 : PassoBase
    {
        public Passo40(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
            : base(pedido, retorno, pedidoCriacao)

        {
        }

        public async Task ExecutarAsync()
        {
            /*
            onde validamos alguns campos: 
            CustoFinancFornecPrecoListaBase_Conferencia: validado em Prepedido.ValidacoesPrepedidoBll.ConfrontarProdutos
            CustoFinancFornecCoeficiente_Conferencia: validado em Prepedido.ValidacoesPrepedidoBll.ValidarCustoFinancFornecCoeficiente
            Preco_Lista: validado em Prepedido.ValidacoesPrepedidoBll.ConfrontarProdutos
            */

            NumeroProdutos();
            await ListaProdutosFormaPagamento();
            await Usuario();
            Validar_qtde_max_venda();
        }

        private void NumeroProdutos()
        {
            if (Pedido.ListaProdutos.Count > Pedido.Configuracao.LimiteItens)
                Retorno.ListaErros.Add($"São permitidos no máximo {Pedido.Configuracao.LimiteItens} itens por pedido.");
            if (Pedido.ListaProdutos.Count == 0)
                Retorno.ListaErros.Add("Não há itens na lista de produtos!");
        }

        private async Task Usuario()
        {
            if (Pedido.Ambiente.ComIndicador)
            {
                if (string.IsNullOrEmpty(Pedido.Ambiente.Indicador))
                {
                    Retorno.ListaErros.Add("Informe quem é o indicador.");
                }

                //Não estou retornando a mensagem abaixo, pois o campos pedido.OpcaoPossuiRa é bool, 
                //sendo assim não tem como ser vazio
                //elseif rb_RA = "" then
                //    alerta = "Informe se o pedido possui RA ou não."
                //end if
            }


            //vamos validar o usuario e atribuir alguns valores da base de dados
            var db = Criacao.ContextoProvider.GetContextoLeitura();
            var tUsuario = await db.Tusuarios.Where(x => x.Usuario.ToUpper() == Pedido.Ambiente.Usuario.ToUpper())
                .Select(u => new { u.Loja, u.Vendedor_Externo })
                .FirstOrDefaultAsync();
            if (tUsuario == null)
            {
                Retorno.ListaErros.Add("Usuário não encontrado.");
                return;
            }

            //vamos validar o vendedor externo
            if (tUsuario.Vendedor_Externo != 0)
            {
                if (string.IsNullOrEmpty(tUsuario.Loja))
                {
                    Retorno.ListaErros.Add("Não foi especificada a loja que fez a indicação.");
                }
                else
                {

                    var tLoja = await db.Tlojas.Where(x => x.Loja == tUsuario.Loja).CountAsync();
                    if (tLoja == 0)
                    {
                        Retorno.ListaErros.Add("Loja " + tUsuario.Loja + " não está cadastrada.");
                    }
                }
            }
        }

        private async Task ListaProdutosFormaPagamento()
        {

            /* 13- valida o tipo de parcelamento "AV", "CE", "SE" */
            /* 14- valida a quantidade de parcela */
            FormaPagtoDados formasPagto = await Criacao.FormaPagtoBll.ObterFormaPagto(Pedido.Ambiente.Indicador, Pedido.Cliente.Tipo.ParaString(),
                Pedido.Configuracao.SistemaResponsavelCadastro);

            UtilsGlobais.Util.ValidarTipoCustoFinanceiroFornecedor(Retorno.ListaErros, Criacao.Execucao.C_custoFinancFornecTipoParcelamento,
                Criacao.Execucao.C_custoFinancFornecQtdeParcelas);

            Criacao.ValidacoesFormaPagtoBll.ValidarFormaPagto(Pedido.FormaPagtoCriacao, Retorno.ListaErros,
                Pedido.Configuracao.LimiteArredondamento, Pedido.Configuracao.MaxErroArredondamento, Criacao.Execucao.C_custoFinancFornecTipoParcelamento, formasPagto,
                Criacao.Execucao.TOrcamentista_Permite_RA_Status, Pedido.Valor.Vl_total_NF, Pedido.Valor.Vl_total);

            //vamos fazer a validação de Especificacao/Especificacao/Pedido/Passo40/FormaPagamentoProdutos.feature
            ValidarProdutosComFormaPagto(Retorno.ListaErros);

            //validar os produtos
            Prepedido.Dados.DetalhesPrepedido.PrePedidoDados prepedido = PedidoCriacaoDados.PrePedidoDadosDePedidoCriacaoDados(
                Pedido, Criacao.Execucao.Id_cliente);
            await Criacao.ValidacoesPrepedidoBll.MontarProdutosParaComparacao(prepedido,
                        Criacao.Execucao.C_custoFinancFornecTipoParcelamento, Criacao.Execucao.C_custoFinancFornecQtdeParcelas,
                        Pedido.Ambiente.Loja, Retorno.ListaErros, (decimal)Criacao.Execucao.Perc_limite_RA_sem_desagio, Pedido.Configuracao.LimiteArredondamento,
                        Prepedido.ValidacoesPrepedidoBll.AmbienteValidacao.PedidoValidacao);

            //se tiver erro vamos retornar
            if (Retorno.ListaErros.Count > 0)
                return;

            //CONSISTÊNCIA PARA VALOR ZERADO
            ConsisteProdutosValorZerados(Pedido.ListaProdutos, Retorno.ListaErros);
        }

        private void ConsisteProdutosValorZerados(List<PedidoCriacaoProdutoDados> lstProdutos, List<string> lstErros)
        {
            foreach (var x in lstProdutos)
            {
                if (string.IsNullOrWhiteSpace(x.Fabricante))
                    lstErros.Add("Lista de produtos: produto '" + x.Produto + "' está sem fabricante!");
                if (string.IsNullOrWhiteSpace(x.Produto))
                    lstErros.Add("Lista de produtos: algum produto está vazio!");
                if (x.Qtde <= 0)
                    lstErros.Add("Lista de produtos: produto '" + x.Produto + "' está com quantidade inválida!");

                if (x.Preco_Venda <= 0)
                    lstErros.Add("Produto '" + x.Produto + "' está com valor de venda zerado!");
                if (x.Preco_NF <= 0)
                    lstErros.Add("Produto '" + x.Produto + "' está com preço zerado!");
            }
        }

        private void ValidarProdutosComFormaPagto(List<string> lstErros)
        {
            if (Pedido.FormaPagtoCriacao.Rb_forma_pagto != Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                var db = Criacao.ContextoProvider.GetContextoLeitura();

                foreach (var prod in Pedido.ListaProdutos)
                {
                    var custoFinancFornec = (from c in Criacao.Execucao.TabelasBanco.TpercentualCustoFinanceiroFornecedors_Coeficiente
                                             where c.Fabricante == prod.Fabricante &&
                                                 c.Tipo_Parcelamento == Criacao.Execucao.C_custoFinancFornecTipoParcelamento &&
                                                 c.Qtde_Parcelas == Criacao.Execucao.C_custoFinancFornecQtdeParcelas
                                             select c).FirstOrDefault();

                    if (custoFinancFornec == null)
                        lstErros.Add("Opção de parcelamento não disponível para fornecedor " + prod.Fabricante + ": " +
                            Prepedido.PrepedidoBll.DecodificaCustoFinanFornecQtdeParcelas(Criacao.Execucao.C_custoFinancFornecTipoParcelamento, (short)Criacao.Execucao.C_custoFinancFornecQtdeParcelas) + " parcela(s).");


                    TprodutoLoja prodLoja = (from c in Criacao.Execucao.TabelasBanco.TprodutoLoja_Include_Tprodtuo_Tfabricante_Validado
                                             where c.Tproduto.Produto == prod.Produto &&
                                             c.Tproduto.Fabricante == prod.Fabricante &&
                                             c.Loja == Pedido.Ambiente.Loja
                                             select c).FirstOrDefault();

                    if (prodLoja == null)
                        lstErros.Add("Produto " + prod.Produto + " não localizado para a loja " + Pedido.Ambiente.Loja + ".");

                }

            }
        }


        private void Validar_qtde_max_venda()
        {
            foreach (var prod in Pedido.ListaProdutos)
            {
                TprodutoLoja prodLoja = (from c in Criacao.Execucao.TabelasBanco.TprodutoLoja_Include_Tprodtuo_Tfabricante_Validado
                                         where c.Tproduto.Produto == prod.Produto &&
                                         c.Tproduto.Fabricante == prod.Fabricante &&
                                         c.Loja == Pedido.Ambiente.Loja
                                         select c).FirstOrDefault();

                if (prodLoja == null)
                {
                    Retorno.ListaErros.Add("Produto " + prod.Produto + " não localizado para a loja " + Pedido.Ambiente.Loja + ".");
                    continue;
                }
                if(prodLoja.Qtde_Max_Venda.HasValue && prodLoja.Qtde_Max_Venda < prod.Qtde)
                {
                    Retorno.ListaErros.Add($"Quantidade do Produto {prod.Produto} excede o máximo permitido. Solicitado: {prod.Qtde}, Qtde_Max_Venda:{prodLoja.Qtde_Max_Venda}.");
                }

            }
        }
    }
}
