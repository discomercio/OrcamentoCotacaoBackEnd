using InfraBanco;
using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using Pedido.Criacao.Execucao;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava17
{
    class Grava17 : PassoBaseGravacao
    {
        public Grava17(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        public async Task ExecutarAsync()
        {
            //monta a lista de ProdutoGravacao
            MontarProdutoGravacao();
            await MontarDescontos();
        }

        private void MontarProdutoGravacao()
        {
            //cria a lista de produtos com as variaveis auxiliares
            List<ProdutoGravacao> listaProdutoGravacao = ProdutoGravacao.ListaProdutoGravacao(Pedido.ListaProdutos);
            Gravacao.ProdutoGravacaoLista = listaProdutoGravacao;
        }

        private async Task MontarDescontos()
        {
            //Passo17: descontos
            // loja/PedidoNovoConfirma.asp
            //região do 
            //if desc_dado_arredondado > perc_comissao_e_desconto_a_utilizar then
            //até
            //v_desconto(UBound(v_desconto)) = Trim("" & rs("id"))

            decimal limiteArredondamentoValores = 0.01M; //1 centavo
            double limiteArredondamentoPorcentagens = 0.01; //2 casas decimais
            List<string> v_desconto = new List<string>();

            foreach (var linha_pedido in Gravacao.ProdutoGravacaoLista)
            {
                //'	VERIFICA CADA UM DOS PRODUTOS SELECIONADOS

                //só pode ter um e tem que ter um
                var rsProduto = (from p in Execucao.TabelasBanco.TprodutoLoja_Include_Tprodtuo_Tfabricante_Validado
                                 where p.Fabricante == linha_pedido.Pedido.Fabricante && p.Produto == linha_pedido.Pedido.Produto && p.Loja == Pedido.Ambiente.Loja
                                 select p).First();



                var preco_lista = rsProduto.Preco_Lista ?? 0;
                if (Math.Abs(linha_pedido.Pedido.CustoFinancFornecPrecoListaBase_Conferencia - preco_lista) > limiteArredondamentoValores)
                {
                    Retorno.ListaErros.Add($"Produto {linha_pedido.Pedido.Produto} do fabricante {linha_pedido.Pedido.Fabricante} está com " +
                        $"CustoFinancFornecPrecoListaBase_Conferencia diferente do preco_lista de {preco_lista}");
                }

                float coeficiente;
                if (Pedido.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
                {
                    coeficiente = 1;
                }
                else
                {
                    var custoFinancFornec = (from c in Criacao.Execucao.TabelasBanco.TpercentualCustoFinanceiroFornecedors_Coeficiente
                                             where c.Fabricante == linha_pedido.Pedido.Fabricante &&
                                                 c.Tipo_Parcelamento == Criacao.Execucao.C_custoFinancFornecTipoParcelamento &&
                                                 c.Qtde_Parcelas == Criacao.Execucao.C_custoFinancFornecQtdeParcelas
                                             select c).FirstOrDefault();
                    if (custoFinancFornec == null)
                    {
                        coeficiente = 1;
                        Retorno.ListaErros.Add("Opção de parcelamento não disponível para fornecedor " + linha_pedido.Pedido.Fabricante + ": " +
                            Prepedido.Bll.PrepedidoBll.DecodificaCustoFinanFornecQtdeParcelas(Criacao.Execucao.C_custoFinancFornecTipoParcelamento, Criacao.Execucao.C_custoFinancFornecQtdeParcelas) + " parcela(s).");
                    }
                    else
                    {
                        coeficiente = custoFinancFornec.Coeficiente;
                        preco_lista = Convert.ToDecimal(coeficiente) * preco_lista;
                    }
                }

                var custoFinancFornecCoeficiente = coeficiente;
                if (Math.Abs(linha_pedido.Pedido.CustoFinancFornecCoeficiente_Conferencia - custoFinancFornecCoeficiente) > limiteArredondamentoPorcentagens)
                {
                    Retorno.ListaErros.Add($"Produto {linha_pedido.Pedido.Produto} do fabricante {linha_pedido.Pedido.Fabricante} está com " +
                        $"custoFinancFornecCoeficiente {linha_pedido.Pedido.CustoFinancFornecCoeficiente_Conferencia} diferente do custoFinancFornecCoeficiente de {custoFinancFornecCoeficiente}");
                }

                /*

                if .preco_lista = 0 then 
                    .desc_dado = 0
                    desc_dado_arredondado = 0
                else
                    .desc_dado = 100*(.preco_lista-.preco_venda)/.preco_lista
                    desc_dado_arredondado = converte_numero(formata_perc_desc(.desc_dado))
                    end if

                function formata_perc_desc(byval valor)
	                formata_perc_desc=formata_numero(valor, 1)
                end function
                function formata_numero(byval valor, byval decimais)

                quer dizer, arredondamos o desconto com 1 casa decimal
                */

                double desc_dado = 0;
                double desc_dado_arredondado = 0;
                if (preco_lista != 0)
                {
                    desc_dado = (double)(100 * (preco_lista - linha_pedido.Pedido.Preco_Venda) / preco_lista);
                    desc_dado_arredondado = Math.Round(desc_dado, 1);
                }

                //validações
                if (Math.Abs(linha_pedido.Pedido.Preco_Lista - preco_lista) > limiteArredondamentoValores)
                {
                    Retorno.ListaErros.Add($"Produto {linha_pedido.Pedido.Produto} do fabricante {linha_pedido.Pedido.Fabricante} está com " +
                        $"preco_lista {linha_pedido.Pedido.Preco_Lista} diferente do preco_lista com coeficiente de {preco_lista}");
                }
                if (Math.Abs((linha_pedido.Pedido.Desc_Dado ?? 0) - desc_dado) > limiteArredondamentoPorcentagens)
                {
                    Retorno.ListaErros.Add($"Produto {linha_pedido.Pedido.Produto} do fabricante {linha_pedido.Pedido.Fabricante} está com " +
                        $"desc_dado {linha_pedido.Pedido.Desc_Dado} diferente do desconto calculado de {desc_dado}");
                }



                //verifica se precisa de uma autorização para o desconto
                if (desc_dado_arredondado > Execucao.Perc_comissao_e_desconto_a_utilizar)
                {
                    /*
                                s = "SELECT " & _
                                        "*" & _
                                    " FROM t_DESCONTO" & _
                                    " WHERE" & _
                                        " (usado_status=0)" & _
                                        " AND (cancelado_status=0)" & _
                                        " AND (id_cliente='" & cliente_selecionado & "')" & _
                                        " AND (fabricante='" & .fabricante & "')" & _
                                        " AND (produto='" & .produto & "')" & _
                                        " AND (loja='" & loja & "')" & _
                                        " AND (data >= " & bd_formata_data_hora(Now-converte_min_to_dec(TIMEOUT_DESCONTO_EM_MIN)) & ")" & _
                                    " ORDER BY" & _
                                        " data DESC"
                                        */
                    var descontos = await (from d in ContextoBdGravacao.Tdesconto
                                           where d.Usado_status == 0
                                              && d.Cancelado_status == 0
                                              && d.Id_cliente == Pedido.Cliente.Id_cliente
                                              && d.Fabricante == linha_pedido.Pedido.Fabricante
                                              && d.Produto == linha_pedido.Pedido.Produto
                                              && d.Loja == Pedido.Ambiente.Loja
                                              && d.Data >= Gravacao.DataHoraCriacao.AddMinutes(-1 * Constantes.TIMEOUT_DESCONTO_EM_MIN)
                                           orderby d.Data descending
                                           select new { d.Id, d.Desc_max, d.Autorizador, d.Supervisor_autorizador }).ToListAsync();

                    if (descontos.Any())
                    {
                        var desconto = descontos.First();
                        if (desconto.Desc_max.HasValue && desc_dado <= (float)(desconto.Desc_max.Value))
                        {
                            linha_pedido.Abaixo_min_status = true;
                            linha_pedido.Abaixo_min_autorizacao = desconto.Id;
                            linha_pedido.Abaixo_min_autorizador = desconto.Autorizador;
                            linha_pedido.Abaixo_min_superv_autorizador = desconto.Supervisor_autorizador;
                            v_desconto.Add(desconto.Id);
                        }
                        else
                        {
                            Retorno.ListaErros.Add($"Produto {linha_pedido.Pedido.Produto} do fabricante {linha_pedido.Pedido.Fabricante}: desconto de {desc_dado_arredondado}% excede o máximo autorizado de {desconto.Desc_max}.");
                        }
                    }
                    else
                    {
                        Retorno.ListaErros.Add($"Produto {linha_pedido.Pedido.Produto} do fabricante {linha_pedido.Pedido.Fabricante}: desconto de {desc_dado_arredondado}% excede o máximo permitido de {Execucao.Perc_comissao_e_desconto_a_utilizar}.");
                    }
                }

            }

            Gravacao.V_desconto = v_desconto;
        }
    }
}
