﻿using InfraBanco;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Pedido.Dados.Criacao;
using Produto.RegrasCrtlEstoque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Produto.Estoque.Estoque;
using Pedido.Criacao.Passo60.Gravacao.Grava80;


#nullable enable

namespace Pedido
{
    public class EfetivaPedidoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly PedidoBll pedidoBll;
        private readonly Prepedido.PrepedidoBll prepedidoBll;
        private readonly MontarLogPedidoBll montarLogPedidoBll;

        public EfetivaPedidoBll(ContextoBdProvider contextoProvider, PedidoBll pedidoBll, Prepedido.PrepedidoBll prepedidoBll,
            MontarLogPedidoBll montarLogPedidoBll)
        {
            this.contextoProvider = contextoProvider;
            this.pedidoBll = pedidoBll;
            this.prepedidoBll = prepedidoBll;
            this.montarLogPedidoBll = montarLogPedidoBll;
        }

        public async Task<string> EfetivarCadastroPedido(PedidoCriacaoDados pedido, List<int> vEmpresaAutoSplit, string usuario_atual,
            string c_custoFinancFornecTipoParcelamento, short c_custoFinancFornecQtdeParcelas, TtransportadoraCep? transportadora,
            List<Cl_ITEM_PEDIDO_NOVO> v_item, List<Cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO> v_spe, List<string> v_desconto,
            List<RegrasBll> lstRegras, float perc_limite_RA_sem_desagio,
            string loja_atual, float perc_desagio_RA, Tcliente cliente, List<string> lstErros,
            ContextoBdGravacao dbGravacao,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel Plataforma_Origem_Pedido)
        {
            bool blnUsarMemorizacaoCompletaEnderecos =
                await UtilsGlobais.Util.IsActivatedFlagPedidoUsarMemorizacaoCompletaEnderecos(contextoProvider);

            int indicePedido = 0;

            //Criando PedidoNovo
            Tpedido pedidonovo = new Tpedido();

            Tpedido pedidonovoTrocaId = pedidonovo;
            //pedidonovoTrocaId = new Tpedido();

            //necessário incluir essa variavel, pois ela é muito utilizada
            string operacao_origem = "";

            //Numero Pedido Temporario
            //vamos gerar o numero de pedido temporario
            //estamos gerando pedidos temporários aqui e não esta sendo alterado depois, 
            //pois é feito a alteração na montagem dos itens
            string id_pedido_temp_base = await GerarNumeroPedidoTemporario(lstErros, dbGravacao);

            foreach (var empresa in vEmpresaAutoSplit)
            {
                if (empresa != 0)
                {
                    indicePedido++;

                    if (indicePedido == 1)
                    {
                        pedidonovo.Pedido = id_pedido_temp_base;
                    }
                    else
                    {
                        pedidonovo.Pedido = id_pedido_temp_base + global::Pedido.Criacao.Passo60.Gravacao.Grava60.Gera_num_pedido.Gera_letra_pedido_filhote(indicePedido);
                    }

                    pedidonovo.Loja = pedido.Ambiente.Loja;
                    pedidonovo.Data = DateTime.Now.Date;
                    pedidonovo.Hora = DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                        DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                        DateTime.Now.Second.ToString().PadLeft(2, '0');

                    if (indicePedido == 1)
                    {
                        if (vEmpresaAutoSplit.Count > 1)
                        {
                            pedidonovo.St_Auto_Split = 1;
                        }

                        if (!string.IsNullOrEmpty(pedidonovo.St_Pagto) &&
                            pedidonovo.St_Pagto != InfraBanco.Constantes.Constantes.ST_PAGTO_NAO_PAGO)
                        {
                            pedidonovo.Dt_St_Pagto = DateTime.Now.Date;
                            pedidonovo.Dt_Hr_St_Pagto = DateTime.Now;
                            pedidonovo.Usuario_St_Pagto = usuario_atual;
                        }

                        pedidonovo.St_Pagto = InfraBanco.Constantes.Constantes.ST_PAGTO_NAO_PAGO;
                        pedidonovo.St_Recebido = !string.IsNullOrEmpty(pedidonovo.St_Recebido) ? pedidonovo.St_Recebido : "";
                        pedidonovo.Obs_1 = pedido.DetalhesPedido.Observacoes;
                        pedidonovo.Obs_2 = pedido.DetalhesPedido.NumeroNF;

                        //Monta forma de pagto do pedido
                        MontarFormaPagto(pedido, pedidonovo);

                        pedidonovo.Forma_Pagto = "";
                        pedidonovo.Vl_Total_Familia = pedido.Valor.Vl_total;
                        //Montamos a analise de crédito
                        await MontarAnaliseCredito(pedido, pedidonovo);

                        //CUSTO FINANCEIRO FORNECEDOR
                        pedidonovo.CustoFinancFornecTipoParcelamento = c_custoFinancFornecTipoParcelamento;
                        pedidonovo.CustoFinancFornecQtdeParcelas = c_custoFinancFornecQtdeParcelas;
                        decimal vl_total_nf = 0m;
                        decimal vl_total = 0m;
                        foreach (var x in pedido.ListaProdutos)
                        {
                            vl_total_nf += Math.Round((short)x.Qtde * x.Preco_NF, 2);
                            vl_total += Math.Round((short)x.Qtde * x.Preco_Venda, 2);
                        };
                        pedidonovo.Vl_Total_NF = vl_total_nf;
                        pedidonovo.Vl_Total_RA = vl_total_nf - vl_total;
                        pedidonovo.Perc_RT = pedido.Valor.Perc_RT;
                        pedidonovo.Perc_Desagio_RA = perc_desagio_RA;
                        pedidonovo.Perc_Limite_RA_Sem_Desagio = perc_limite_RA_sem_desagio;
                    }
                    else
                    {
                        //PEDIDO FILHOTE
                        MontarPedidoFilhote(pedidonovo);
                    }

                    //CAMPOS ARMAZENADOS TANTO NO PEDIDO - PAI QUANTO NO PEDIDO - FILHOTE
                    //aqui também esta sendo salvo alguns campos a mais 
                    MontarDetalhesPedido(pedidonovo, pedido, cliente, usuario_atual);

                    //Endereço de entrega
                    if (pedido.EnderecoEntrega.OutroEndereco == true)
                    {
                        //Monta endereço de entrega
                        MontarEndereçoEntrega(pedido, pedidonovo);
                        //como estou verificando cada campos para atribuir ao Tpedido estou comentando essa verificação
                        //if (blnUsarMemorizacaoCompletaEnderecos)
                        //{
                        //    MontarEndereçoEntrega(pedido, pedidonovo);
                        //}

                    }

                    //OBTENÇÃO DE TRANSPORTADORA QUE ATENDA AO CEP INFORMADO, SE HOUVER
                    if (transportadora != null && transportadora.Id != 0)
                    {
                        IncluirTransportadoraPedido(pedidonovo, transportadora, usuario_atual);
                    }

                    //01 / 02 / 2018: os pedidos do Arclube usam o RA para incluir o valor do frete e, 
                    //portanto, não devem ter deságio do RA
                    //necessário incluir essa variavel, pois ela é utilizada para saber 
                    //se o pedido magento é com indicador
                    bool blnMagentoPedidoComIndicador = false;
                    if (!string.IsNullOrEmpty(loja_atual) &&
                        loja_atual != InfraBanco.Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE &&
                        !blnMagentoPedidoComIndicador)
                    {
                        pedidonovo.Perc_Desagio_RA_Liquida = await UtilsGlobais.Util.VerificarSemDesagioRA(contextoProvider);
                    }

                    if (operacao_origem == InfraBanco.Constantes.Constantes.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO && blnMagentoPedidoComIndicador)
                    {
                        //campos do pedido magento á incluir
                    }

                    MontarEnderecoCadastralCliente(pedidonovo, pedido);

                    pedidonovo.Plataforma_Origem_Pedido = (int)Plataforma_Origem_Pedido;

                    pedidonovo.Id_Nfe_Emitente = (short)empresa;

                    string pedido_existe = await (from c in dbGravacao.Tpedidos
                                                  where c.Pedido == pedidonovo.Pedido
                                                  select c.Pedido).FirstOrDefaultAsync();

                    if (string.IsNullOrEmpty(pedido_existe))
                    {
                        dbGravacao.Add(pedidonovo);
                        await dbGravacao.SaveChangesAsync();
                    }
                    else
                    {
                        dbGravacao.Update(pedidonovo);
                        await dbGravacao.SaveChangesAsync();
                    }

                    //fazer a montagens dos itens
                    string log = "";
                    log = await Novo_MontarItens(lstRegras, v_item, id_pedido_temp_base, pedidonovoTrocaId, empresa, indicePedido,
                        usuario_atual, lstErros, pedidonovo, pedido, v_desconto, operacao_origem, cliente, dbGravacao);

                    //vamos montar o log do pedido
                    //verificar o que inserir para a inclusão dos itens
                    if (indicePedido == 1 && lstErros.Count == 0)
                        log = await montarLogPedidoBll.MontarLogPedido(pedidonovoTrocaId.Pedido, dbGravacao, pedido) + log;

                    if (!UtilsGlobais.Util.GravaLog(dbGravacao, usuario_atual, loja_atual, pedidonovoTrocaId.Pedido,
                                    pedidonovoTrocaId.Id_Cliente, InfraBanco.Constantes.Constantes.OP_LOG_PEDIDO_NOVO, log))
                        lstErros.Add("Falha ao gravar log.");
                }
            }

            //salvar Pedido na base
            pedidonovoTrocaId = pedidonovo;
            dbGravacao.Update(pedidonovoTrocaId);
            string retorno = pedidonovoTrocaId.Pedido;

            //salvando todas alterações
            await dbGravacao.SaveChangesAsync();

            return retorno;
        }

        private async Task<string> Novo_MontarItens(List<RegrasBll> lstRegras, List<Cl_ITEM_PEDIDO_NOVO> v_item, string id_pedido_temp,
            Tpedido pedidonovoTrocaId, int empresa, int indice_pedido, string usuario_atual, List<string> lstErros,
            Tpedido pedidonovo, PedidoCriacaoDados pedidoDados, List<string> v_desconto, string opercao_origem,
            Tcliente cliente, ContextoBdGravacao dbGravacao)
        {
            //OBS: vou remover um dos "PedidoCriacaoDados" para ver se ocorre erro ou se esta em duplicidade mesmo

            //mover e alterar o nome da classe para MovimentoEstoqueDados
            //já temos a sequencia de itens
            MovimentoEstoqueDados movimentoEstoque = new MovimentoEstoqueDados();
            //Tpedido pedidonovoTrocaId = new Tpedido();

            //string s_log_item_autosplit = "";
            int indice_item = 0;
            int sequencia = 0;

            string idPedidoBase = "";
            string s_log_cliente_indicador = "";
            decimal vl_total_RA_liquido = -1;

            //pegar a lista de regras
            foreach (var regra in lstRegras)
            {
                if (!string.IsNullOrEmpty(regra.Produto))
                {
                    int aux = 0;
                    foreach (var regra_UF_Pessoa_CD in regra.TwmsCdXUfXPessoaXCd)
                    {
                        aux++;
                        if (regra_UF_Pessoa_CD.Id_nfe_emitente == empresa &&
                            regra_UF_Pessoa_CD.Estoque_Qtde_Solicitado > 0 &&
                            regra_UF_Pessoa_CD.St_inativo == 0)
                        {
                            indice_item = -1;

                            foreach (var item in v_item)
                            {
                                if (item.Fabricante == regra_UF_Pessoa_CD.Estoque_Fabricante &&
                                    item.Produto == regra_UF_Pessoa_CD.Estoque_Produto)
                                {
                                    indice_item = aux;
                                    break;
                                }
                            }

                            if (indice_item >= 0)
                            {
                                sequencia++;


                                Cl_ITEM_PEDIDO_NOVO item = (from c in v_item
                                                            where c.Fabricante == regra.Fabricante &&
                                                                  c.Produto == regra.Produto
                                                            select c).FirstOrDefault();

                                TpedidoItem t_pedido_item = new TpedidoItem();
                                movimentoEstoque = await Novo_VerificarListaRegras(t_pedido_item, regra_UF_Pessoa_CD,
                                    item, empresa, usuario_atual, id_pedido_temp, lstErros, dbGravacao);

                                //aqui esta dando erro pois não devemos fazer isso no foreach e 
                                //sim conforme a regra que esta sendo utilizada
                                //foreach (var item in v_item)
                                //{
                                //    TpedidoItem t_pedido_item = new TpedidoItem();
                                //    movimentoEstoque = await Novo_VerificarListaRegras(t_pedido_item, regra_UF_Pessoa_CD,
                                //        item, empresa, usuario_atual, id_pedido_temp, lstErros, dbGravacao);
                                //}
                            }
                        }

                    }
                }
            }

            if (indice_pedido == 1)
            {
                // gerar num_pedido
                if (idPedidoBase == "")
                    idPedidoBase = await global::Pedido.Criacao.Passo60.Gravacao.Grava60.Gera_num_pedido.Gera_num_pedido_pai(lstErros, dbGravacao);

                if (string.IsNullOrEmpty(idPedidoBase))
                {
                    lstErros.Add(InfraBanco.Constantes.Constantes.ERR_FALHA_OPERACAO_GERAR_NSU);
                }
            }
            else
            {
                //Gera Pedido filhote
                idPedidoBase = idPedidoBase + InfraBanco.Constantes.Constantes.COD_SEPARADOR_FILHOTE +
                    global::Pedido.Criacao.Passo60.Gravacao.Grava60.Gera_num_pedido.Gera_letra_pedido_filhote(indice_pedido - 1);
            }

            //Log
            //afazer descomentar
            //vLogAutoSplit.Add(idPedidoBase + " (" + await UtilsGlobais.Util.ObterApelidoEmpresaNfeEmitentes(
            //    item, contextoProvider.GetContextoLeitura()) + ") " + movimentoEstoque.s_log_item_autosplit);

            if (idPedidoBase != "")
            {
                //vamos retornar o pedidonovoTrocaId pq ele foi preenchido aqui dentro mas esta retornando vazio
                pedidonovoTrocaId = await AlterarIdPedido(id_pedido_temp, idPedidoBase, pedidonovo, pedidonovoTrocaId, dbGravacao);
            }

            //Indicador
            if (indice_pedido == 1)
            {
                if (pedidonovo.Permite_RA_Status == 1)
                {
                    //indicador: se este pedido é com indicador e o cliente ainda 
                    //não tem um indicador no cadastro, então cadastra este.
                    CadastrarIndicador(pedidoDados, pedidonovo, pedidonovoTrocaId, dbGravacao, ref s_log_cliente_indicador, cliente.Id);

                }
            }

            //status de entrega
            string status_entrega = "";
            status_entrega = VerificarStatusEntrega(pedidonovoTrocaId, movimentoEstoque.Total_estoque_vendido,
                movimentoEstoque.Total_estoque_sem_presenca, dbGravacao);

            pedidonovoTrocaId.St_Entrega = status_entrega;

            if (pedidoDados.Valor.PedidoPossuiRa())
            {
                //calcula total ra liquido bd 
                vl_total_RA_liquido = await CalculaTotalRALiquidoBD(idPedidoBase, dbGravacao, lstErros);

                //alterando status do pedido cadastrado
                await AlterarStatusPedidoCadastrado(pedidonovoTrocaId, pedidonovo, pedidoDados,
                    idPedidoBase, indice_pedido, dbGravacao, vl_total_RA_liquido);

            }
            else
            {
                //significa que o vl_total_RA_liquido não será calculado então vamos colocar ele como 0.00
                pedidonovoTrocaId.Vl_Total_RA_Liquido = 0M;
            }

            //necessário incluir essa variavel blnMagentoPedidoComIndicador
            bool blnMagentoPedidoComIndicador = false;

            //Desconto
            if (indice_pedido == 1)
            {
                //senhas de autorização para desconto superior
                await VerificarSenhaDescontoSuperior(v_desconto, usuario_atual, blnMagentoPedidoComIndicador,
                    opercao_origem, dbGravacao, lstErros);
            }

            //VERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POSSÍVEL FRAUDE)
            //ENDEREÇO DO CADASTRO linha 2264
            if (indice_pedido == 1)
            {
                //Analisa endereço
                await AnalisarEndereco(pedidoDados, pedidonovoTrocaId, pedidonovo, cliente, idPedidoBase,
                    usuario_atual, lstErros, dbGravacao);
            }

            //Vamos criar o log do pedido e dos itens de pedido
            //Itens do pedido => dentro de um foreach vamos montar da seguinte forma
            /*1x003243(003) = qtdex / produto / (fabricante)
             * preco_lista=1523,61;
             * desc_dado=0;
             * preco_venda=1523,61;
             * preco_NF=1523,61;
             * custoFinancFornecCoeficiente=1;
             * custoFinancFornecPrecoListaBase=1523,61;
             */
            string log = "";
            log = montarLogPedidoBll.MontarCamposAInserirItensPedido(log, v_item, s_log_cliente_indicador);
            return log;
        }

        private async Task<Tpedido> AlterarIdPedido(string idPedidoBase_temporario, string idPedidoBase,
            Tpedido pedidonovo, Tpedido pedidonovoTrocaId, ContextoBdGravacao dbGravacao)
        {
            List<TpedidoItem> tpedidoItemset = await (from c in dbGravacao.TpedidoItems
                                                      where c.Pedido == idPedidoBase_temporario
                                                      select c).ToListAsync();

            //para alterar o valor da chave primária, precisamos excluir o existente e inserir novamente
            //excluimos o pedidonovo com o Id temporario
            dbGravacao.Remove(pedidonovo);
            await dbGravacao.SaveChangesAsync();

            //iremos passar os dados dos registros que foram removidos
            //esse objeto esta sendo instanciado fora do foreach, pois iremos salvar no final da rotina                            
            pedidonovoTrocaId = pedidonovo;
            pedidonovoTrocaId.Pedido = idPedidoBase;
            foreach (var itemset in tpedidoItemset)
            {
                //buscando o item para criar um novo com o Id_Pedido definitivo
                TestoqueMovimento testoqueMovto = await (from c in dbGravacao.TestoqueMovimentos
                                                         where c.Pedido == idPedidoBase_temporario &&
                                                               c.Fabricante == itemset.Fabricante &&
                                                               c.Produto == itemset.Produto
                                                         select c).FirstOrDefaultAsync();

                dbGravacao.Remove(testoqueMovto);
                await dbGravacao.SaveChangesAsync();

                //buscando o item para criar um novo com o Id_Pedido definitivo
                TestoqueLog testoqueLogOrigem = await (from c in dbGravacao.TestoqueLogs
                                                       where c.Pedido_estoque_origem == idPedidoBase_temporario &&
                                                             c.Fabricante == itemset.Fabricante &&
                                                             c.Produto == itemset.Produto
                                                       select c).FirstOrDefaultAsync();
                if (testoqueLogOrigem != null)
                {
                    dbGravacao.Remove(testoqueLogOrigem);
                    await dbGravacao.SaveChangesAsync();
                }

                //buscando o item para criar um novo com o Id_Pedido definitivo
                TestoqueLog testoqueLogDestino = await (from c in dbGravacao.TestoqueLogs
                                                        where c.Pedido_estoque_destino == idPedidoBase_temporario &&
                                                              c.Fabricante == itemset.Fabricante &&
                                                              c.Produto == itemset.Produto
                                                        select c).FirstOrDefaultAsync();
                if (testoqueLogDestino != null)
                {
                    dbGravacao.Remove(testoqueLogDestino);
                    await dbGravacao.SaveChangesAsync();
                }

                TpedidoItem itemsetTrocaId = new TpedidoItem();
                itemsetTrocaId = itemset;
                itemsetTrocaId.Pedido = idPedidoBase;
                dbGravacao.Add(itemsetTrocaId);

                TestoqueMovimento testoqueMovtoTrocaId = testoqueMovto;
                testoqueMovtoTrocaId.Pedido = idPedidoBase;
                dbGravacao.Add(testoqueMovtoTrocaId);

                if (testoqueLogOrigem != null)
                {
                    TestoqueLog testoqueLogTrocaId = testoqueLogOrigem;
                    testoqueLogTrocaId.Pedido_estoque_origem = idPedidoBase;
                    dbGravacao.Add(testoqueLogTrocaId);
                }

                if (testoqueLogDestino != null)
                {
                    TestoqueLog testoqueLog2TrocaId = testoqueLogDestino;
                    testoqueLog2TrocaId.Pedido_estoque_destino = idPedidoBase;
                    dbGravacao.Add(testoqueLog2TrocaId);
                }
            }

            //inserimos o pedidonovo com o Id definitivo
            dbGravacao.Add(pedidonovoTrocaId);
            await dbGravacao.SaveChangesAsync();

            return pedidonovoTrocaId;
        }

        private void IncluirTransportadoraPedido(Tpedido pedidonovo, TtransportadoraCep transportadoraCep,
            string usuario_atual)
        {
            if (!string.IsNullOrEmpty(transportadoraCep.Id.ToString()))
            {
                if (!string.IsNullOrEmpty(pedidonovo.EndEtg_Cep))
                {
                    pedidonovo.Transportadora_Id = transportadoraCep.Transportadora_id.ToString();
                    pedidonovo.Transportadora_Data = DateTime.Now;
                    pedidonovo.Transportadora_Usuario = usuario_atual;
                    pedidonovo.Transportadora_Selecao_Auto_Status =
                        InfraBanco.Constantes.Constantes.TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_S;
                    pedidonovo.Transportadora_Selecao_Auto_Cep = pedidonovo.EndEtg_Cep;
                    pedidonovo.Transportadora_Selecao_Auto_Transportadora = transportadoraCep.Transportadora_id.ToString(); ;
                    pedidonovo.Transportadora_Selecao_Auto_Tipo_Endereco =
                        InfraBanco.Constantes.Constantes.TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_ENTREGA;
                    pedidonovo.Transportadora_Selecao_Auto_Data_Hora = DateTime.Now;
                }
                else
                {
                    pedidonovo.Transportadora_Id = transportadoraCep.Transportadora_id.ToString();
                    pedidonovo.Transportadora_Data = DateTime.Now;
                    pedidonovo.Transportadora_Usuario = usuario_atual;
                    pedidonovo.Transportadora_Selecao_Auto_Status =
                        InfraBanco.Constantes.Constantes.TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_S;
                    pedidonovo.Transportadora_Selecao_Auto_Cep = pedidonovo.Endereco_cep;
                    pedidonovo.Transportadora_Selecao_Auto_Transportadora = transportadoraCep.Transportadora_id.ToString(); ;
                    pedidonovo.Transportadora_Selecao_Auto_Tipo_Endereco =
                        InfraBanco.Constantes.Constantes.TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_CLIENTE;
                    pedidonovo.Transportadora_Selecao_Auto_Data_Hora = DateTime.Now;

                }
            }

        }

        private static void MontarFormaPagto(PedidoCriacaoDados pedidoCriacao, Tpedido pedidonovo)
        {
            pedidonovo.Pu_Valor = pedidoCriacao.FormaPagtoCriacao.C_pu_valor == null ? 0M : pedidoCriacao.FormaPagtoCriacao.C_pu_valor; ;
            //pagto com entrada
            pedidonovo.Pce_Entrada_Valor = pedidoCriacao.FormaPagtoCriacao.C_pce_entrada_valor == null ?
                0M : pedidoCriacao.FormaPagtoCriacao.C_pce_entrada_valor;
            pedidonovo.Pce_Prestacao_Valor = pedidoCriacao.FormaPagtoCriacao.C_pce_prestacao_valor == null ?
                0M : pedidoCriacao.FormaPagtoCriacao.C_pce_prestacao_valor;
            //parcela sem entrada
            pedidonovo.Pse_Prim_Prest_Valor = pedidoCriacao.FormaPagtoCriacao.C_pse_prim_prest_valor == null ?
                0M : pedidoCriacao.FormaPagtoCriacao.C_pse_prim_prest_valor;
            pedidonovo.Pse_Demais_Prest_Valor = pedidoCriacao.FormaPagtoCriacao.C_pse_demais_prest_valor == null ?
                0M : pedidoCriacao.FormaPagtoCriacao.C_pse_demais_prest_valor;


            pedidonovo.Tipo_Parcelamento = short.Parse(pedidoCriacao.FormaPagtoCriacao.Rb_forma_pagto);
            if (pedidoCriacao.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                pedidonovo.Av_Forma_Pagto = short.Parse(pedidoCriacao.FormaPagtoCriacao.Op_av_forma_pagto);
                pedidonovo.CustoFinancFornecQtdeParcelas = (short)Prepedido.PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
                pedidonovo.Qtde_Parcelas = Prepedido.PrepedidoBll.ObterQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
            }

            if (pedidoCriacao.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                pedidonovo.Pu_Forma_Pagto = short.Parse(pedidoCriacao.FormaPagtoCriacao.Op_pu_forma_pagto);
                pedidonovo.Pu_Valor = pedidoCriacao.FormaPagtoCriacao.C_pu_valor;
                pedidonovo.Pu_Vencto_Apos = (short)(pedidoCriacao.FormaPagtoCriacao.C_pu_vencto_apos ?? 1);
                pedidonovo.CustoFinancFornecQtdeParcelas = (short)Prepedido.PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
                pedidonovo.Qtde_Parcelas = Prepedido.PrepedidoBll.ObterQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
            }

            if (pedidoCriacao.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            {
                pedidonovo.Qtde_Parcelas = Prepedido.PrepedidoBll.ObterQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao); ;
                pedidonovo.Pc_Qtde_Parcelas = (short)(pedidoCriacao.FormaPagtoCriacao.C_pc_qtde ?? 1);
                pedidonovo.Pc_Valor_Parcela = pedidoCriacao.FormaPagtoCriacao.C_pc_valor;
                pedidonovo.CustoFinancFornecQtdeParcelas = (short)Prepedido.PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
            }
            else
            {
                pedidonovo.Pc_Valor_Parcela = 0M;
            }

            if (pedidoCriacao.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                pedidonovo.Pc_Maquineta_Valor_Parcela = (pedidoCriacao.FormaPagtoCriacao.C_pc_maquineta_valor ?? 0);
                pedidonovo.Pc_Maquineta_Qtde_Parcelas = (short)(pedidoCriacao.FormaPagtoCriacao.C_pc_maquineta_qtde ?? 1);
                pedidonovo.Qtde_Parcelas = Prepedido.PrepedidoBll.ObterQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
                pedidonovo.CustoFinancFornecQtdeParcelas = (short)Prepedido.PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
            }
            if (pedidoCriacao.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                pedidonovo.Pce_Forma_Pagto_Entrada = short.Parse(pedidoCriacao.FormaPagtoCriacao.Op_pce_entrada_forma_pagto);
                pedidonovo.Pce_Forma_Pagto_Prestacao = short.Parse(pedidoCriacao.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto);
                pedidonovo.Pce_Entrada_Valor = pedidoCriacao.FormaPagtoCriacao.C_pce_entrada_valor;
                pedidonovo.Pce_Prestacao_Qtde = (short)(pedidoCriacao.FormaPagtoCriacao.C_pce_prestacao_qtde ?? 1);
                pedidonovo.Pce_Prestacao_Valor = pedidoCriacao.FormaPagtoCriacao.C_pce_prestacao_valor;
                if (pedidoCriacao.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "5" &&
                    pedidoCriacao.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "7")
                    pedidonovo.Pce_Prestacao_Periodo = (short)(pedidoCriacao.FormaPagtoCriacao.C_pce_prestacao_periodo ?? 1);
                pedidonovo.Qtde_Parcelas = Prepedido.PrepedidoBll.ObterQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
                pedidonovo.CustoFinancFornecQtdeParcelas = (short)Prepedido.PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
            }
            if (pedidoCriacao.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                pedidonovo.Pse_Forma_Pagto_Prim_Prest = short.Parse(pedidoCriacao.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto);
                pedidonovo.Pse_Forma_Pagto_Demais_Prest = short.Parse(pedidoCriacao.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto);
                pedidonovo.Pse_Prim_Prest_Valor = pedidoCriacao.FormaPagtoCriacao.C_pse_prim_prest_valor;
                pedidonovo.Pse_Prim_Prest_Apos = (short)(pedidoCriacao.FormaPagtoCriacao.C_pse_prim_prest_apos ?? 1);
                pedidonovo.Pse_Demais_Prest_Qtde = (short)(pedidoCriacao.FormaPagtoCriacao.C_pse_demais_prest_qtde ?? 1);
                pedidonovo.Pse_Demais_Prest_Valor = (decimal)(pedidoCriacao.FormaPagtoCriacao.C_pse_demais_prest_valor ?? 0);
                pedidonovo.Pse_Demais_Prest_Periodo = (short)(pedidoCriacao.FormaPagtoCriacao.C_pse_demais_prest_periodo ?? 1);
                pedidonovo.Qtde_Parcelas = Prepedido.PrepedidoBll.ObterQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
                pedidonovo.CustoFinancFornecQtdeParcelas = (short)Prepedido.PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(pedidoCriacao.FormaPagtoCriacao);
            }

            pedidonovo.Forma_Pagto = pedidoCriacao.FormaPagtoCriacao.C_forma_pagto;
        }

        private async Task MontarAnaliseCredito(PedidoCriacaoDados pedido, Tpedido pedidonovo)
        {
            decimal vl_aprov_auto_analise_credito = decimal.Parse(await pedidoBll.LeParametroControle(InfraBanco.Constantes.Constantes.ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO));

            decimal vl_total = 0m;
            foreach (var x in pedido.ListaProdutos)
                vl_total += Math.Round((short)x.Qtde * x.Preco_Venda, 2);

            if (vl_total <= vl_aprov_auto_analise_credito)
            {
                pedidonovo.Analise_Credito = short.Parse(InfraBanco.Constantes.Constantes.COD_AN_CREDITO_OK);
                pedidonovo.Analise_credito_Data = DateTime.Now;
                pedidonovo.Analise_Credito_Usuario = InfraBanco.Constantes.Constantes.ANALISE_CREDITO_USUARIO_AUTOMATICO;
            }
            else if (pedido.Ambiente.Loja == InfraBanco.Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE &&
                pedido.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA &&
                pedido.FormaPagtoCriacao.Op_av_forma_pagto == InfraBanco.Constantes.Constantes.ID_FORMA_PAGTO_DINHEIRO)
            {
                pedidonovo.Analise_Credito = short.Parse(InfraBanco.Constantes.Constantes.COD_AN_CREDITO_PENDENTE_VENDAS);
                pedidonovo.Analise_credito_Data = DateTime.Now;
                pedidonovo.Analise_Credito_Usuario = InfraBanco.Constantes.Constantes.ANALISE_CREDITO_USUARIO_AUTOMATICO;
            }
            else if (pedido.Ambiente.Loja == InfraBanco.Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE &&
                pedido.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA &&
                pedido.FormaPagtoCriacao.Op_av_forma_pagto == InfraBanco.Constantes.Constantes.ID_FORMA_PAGTO_BOLETO_AV)
            {
                pedidonovo.Analise_Credito = short.Parse(InfraBanco.Constantes.Constantes.COD_AN_CREDITO_PENDENTE_VENDAS);
                pedidonovo.Analise_Credito_Pendente_Vendas_Motivo = InfraBanco.Constantes.Constantes.AGUARDANDO_EMISSAO_BOLETO;//aguardando emissão do boleto
                pedidonovo.Analise_credito_Data = DateTime.Now;
                pedidonovo.Analise_Credito_Usuario = InfraBanco.Constantes.Constantes.ANALISE_CREDITO_USUARIO_AUTOMATICO;
            }
            else if (pedido.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA &&
                (pedido.FormaPagtoCriacao.Op_av_forma_pagto == InfraBanco.Constantes.Constantes.ID_FORMA_PAGTO_DEPOSITO ||
                 pedido.FormaPagtoCriacao.Op_av_forma_pagto == InfraBanco.Constantes.Constantes.ID_FORMA_PAGTO_BOLETO_AV))
            {
                pedidonovo.Analise_Credito = short.Parse(InfraBanco.Constantes.Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO);
                pedidonovo.Analise_credito_Data = DateTime.Now;
                pedidonovo.Analise_Credito_Usuario = InfraBanco.Constantes.Constantes.ANALISE_CREDITO_USUARIO_AUTOMATICO;
            }
            else if (pedido.Ambiente.Loja == InfraBanco.Constantes.Constantes.NUMERO_LOJA_TRANSFERENCIA ||
                pedido.Ambiente.Loja == InfraBanco.Constantes.Constantes.NUMERO_LOJA_KITS)
            {
                pedidonovo.Analise_Credito = short.Parse(InfraBanco.Constantes.Constantes.COD_AN_CREDITO_OK);
                pedidonovo.Analise_credito_Data = DateTime.Now;
                pedidonovo.Analise_Credito_Usuario = InfraBanco.Constantes.Constantes.ANALISE_CREDITO_USUARIO_AUTOMATICO;
            }
            else if (pedido.FormaPagtoCriacao.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                pedidonovo.Analise_Credito = short.Parse(InfraBanco.Constantes.Constantes.COD_AN_CREDITO_PENDENTE_VENDAS);
                pedidonovo.Analise_credito_Data = DateTime.Now;
                pedidonovo.Analise_Credito_Usuario = InfraBanco.Constantes.Constantes.ANALISE_CREDITO_USUARIO_AUTOMATICO;
            }
        }

        private void MontarPedidoFilhote(Tpedido pedidonovo)
        {
            pedidonovo.St_Auto_Split = 1;
            pedidonovo.Split_Status = 1;
            pedidonovo.Split_Data = DateTime.Now.Date;
            pedidonovo.Split_Hora = DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0');
            pedidonovo.Split_Usuario = InfraBanco.Constantes.Constantes.ID_USUARIO_SISTEMA;
            pedidonovo.St_Pagto = "";
            pedidonovo.St_Recebido = "";
            pedidonovo.Obs_1 = "";
            pedidonovo.Obs_2 = "";
            pedidonovo.Qtde_Parcelas = 0;
            pedidonovo.Forma_Pagto = "";
        }

        private void MontarEndereçoEntrega(PedidoCriacaoDados pedido, Tpedido pedidonovo)
        {
            if (pedidonovo != null)
            {
                if (!string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_cod_justificativa))
                {
                    pedidonovo.EndEtg_Endereco = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_endereco) ?
                        "" : pedido.EnderecoEntrega.EndEtg_endereco;
                    pedidonovo.EndEtg_Endereco_Numero = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_endereco_numero) ?
                        "" : pedido.EnderecoEntrega.EndEtg_endereco_numero;
                    pedidonovo.EndEtg_Endereco_Complemento = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_endereco_complemento) ?
                        "" : pedido.EnderecoEntrega.EndEtg_endereco_complemento;
                    pedidonovo.EndEtg_Bairro = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_bairro) ?
                        "" : pedido.EnderecoEntrega.EndEtg_bairro;
                    pedidonovo.EndEtg_Cidade = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_cidade) ?
                        "" : pedido.EnderecoEntrega.EndEtg_cidade;
                    pedidonovo.EndEtg_UF = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_uf) ?
                        "" : pedido.EnderecoEntrega.EndEtg_uf;
                    pedidonovo.EndEtg_Cep = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_cep) ?
                        "" : pedido.EnderecoEntrega.EndEtg_cep.Replace("-", "");
                    pedidonovo.EndEtg_Cod_Justificativa = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_cod_justificativa) ?
                        "" : pedido.EnderecoEntrega.EndEtg_cod_justificativa;
                    pedidonovo.EndEtg_email = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_email) ?
                        "" : pedido.EnderecoEntrega.EndEtg_email;
                    pedidonovo.EndEtg_email_xml = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_email_xml) ?
                        "" : pedido.EnderecoEntrega.EndEtg_email_xml;
                    pedidonovo.EndEtg_nome = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_nome) ?
                        "" : pedido.EnderecoEntrega.EndEtg_nome;
                    pedidonovo.EndEtg_ddd_res = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_ddd_res) ?
                        "" : pedido.EnderecoEntrega.EndEtg_ddd_res;
                    pedidonovo.EndEtg_tel_res = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_tel_res) ?
                        "" : pedido.EnderecoEntrega.EndEtg_tel_res;
                    pedidonovo.EndEtg_ddd_com = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_ddd_com) ?
                        "" : pedido.EnderecoEntrega.EndEtg_ddd_com;
                    pedidonovo.EndEtg_tel_com = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_tel_com) ?
                        "" : pedido.EnderecoEntrega.EndEtg_tel_com;
                    pedidonovo.EndEtg_ramal_com = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_ramal_com) ?
                        "" : pedido.EnderecoEntrega.EndEtg_ramal_com;
                    pedidonovo.EndEtg_ddd_cel = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_ddd_cel) ?
                        "" : pedido.EnderecoEntrega.EndEtg_ddd_cel;
                    pedidonovo.EndEtg_tel_cel = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_tel_cel) ?
                        "" : pedido.EnderecoEntrega.EndEtg_tel_cel;
                    pedidonovo.EndEtg_ddd_com_2 = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_ddd_com_2) ?
                        "" : pedido.EnderecoEntrega.EndEtg_ddd_com_2;
                    pedidonovo.EndEtg_tel_com_2 = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_tel_com_2) ?
                        "" : pedido.EnderecoEntrega.EndEtg_tel_com_2;
                    pedidonovo.EndEtg_ramal_com_2 = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_ramal_com_2) ?
                        "" : pedido.EnderecoEntrega.EndEtg_ramal_com_2;
                    pedidonovo.EndEtg_tipo_pessoa = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_tipo_pessoa) ?
                        "" : pedido.EnderecoEntrega.EndEtg_tipo_pessoa;
                    pedidonovo.EndEtg_cnpj_cpf = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_cnpj_cpf) ?
                        "" : UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedido.EnderecoEntrega.EndEtg_cnpj_cpf);
                    pedidonovo.EndEtg_contribuinte_icms_status = pedido.EnderecoEntrega.EndEtg_contribuinte_icms_status;
                    pedidonovo.EndEtg_produtor_rural_status = pedido.EnderecoEntrega.EndEtg_produtor_rural_status;
                    pedidonovo.EndEtg_ie = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_ie) ?
                        "" : pedido.EnderecoEntrega.EndEtg_ie;
                    pedidonovo.EndEtg_rg = string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_rg) ?
                        "" : pedido.EnderecoEntrega.EndEtg_rg;
                }
            }
        }

        private void MontarEnderecoCadastralCliente(Tpedido pedidonovo, PedidoCriacaoDados pedido)
        {
            var cliente = pedido.EnderecoCadastralCliente;
            pedidonovo.Endereco_memorizado_status = 1;
            pedidonovo.St_memorizacao_completa_enderecos = 1;
            pedidonovo.Endereco_tipo_pessoa = cliente.Endereco_tipo_pessoa;
            pedidonovo.Endereco_cnpj_cpf = cliente.Endereco_cnpj_cpf;
            pedidonovo.Endereco_nome = cliente.Endereco_nome;
            pedidonovo.Endereco_ie = string.IsNullOrEmpty(cliente.Endereco_ie) ? "" : cliente.Endereco_ie;
            pedidonovo.Endereco_rg = string.IsNullOrEmpty(cliente.Endereco_rg) ? "" : cliente.Endereco_rg;
            pedidonovo.Endereco_contribuinte_icms_status = cliente.Endereco_contribuinte_icms_status;
            pedidonovo.Endereco_produtor_rural_status = cliente.Endereco_produtor_rural_status;
            pedidonovo.Endereco_contato = string.IsNullOrEmpty(cliente.Endereco_contato) ? "" : cliente.Endereco_contato;
            pedidonovo.Endereco_email = cliente.Endereco_email;
            pedidonovo.Endereco_email_xml = string.IsNullOrEmpty(cliente.Endereco_email_xml) ? "" : cliente.Endereco_email_xml;
            pedidonovo.Endereco_ddd_res = string.IsNullOrEmpty(cliente.Endereco_ddd_res) ? "" : cliente.Endereco_ddd_res;
            pedidonovo.Endereco_tel_res = string.IsNullOrEmpty(cliente.Endereco_tel_res) ? "" : cliente.Endereco_tel_res;
            pedidonovo.Endereco_ddd_com = string.IsNullOrEmpty(cliente.Endereco_ddd_com) ? "" : cliente.Endereco_ddd_com;
            pedidonovo.Endereco_tel_com = string.IsNullOrEmpty(cliente.Endereco_tel_com) ? "" : cliente.Endereco_tel_com;
            pedidonovo.Endereco_ramal_com = string.IsNullOrEmpty(cliente.Endereco_ramal_com) ? "" : cliente.Endereco_ramal_com;
            pedidonovo.Endereco_ddd_cel = string.IsNullOrEmpty(cliente.Endereco_ddd_cel) ? "" : cliente.Endereco_ddd_cel;
            pedidonovo.Endereco_tel_cel = string.IsNullOrEmpty(cliente.Endereco_tel_cel) ? "" : cliente.Endereco_tel_cel;
            pedidonovo.Endereco_ddd_com_2 = string.IsNullOrEmpty(cliente.Endereco_ddd_com_2) ? "" : cliente.Endereco_ddd_com_2;
            pedidonovo.Endereco_tel_com_2 = string.IsNullOrEmpty(cliente.Endereco_tel_com_2) ? "" : cliente.Endereco_tel_com_2;
            pedidonovo.Endereco_ramal_com_2 = string.IsNullOrEmpty(cliente.Endereco_ramal_com_2) ? "" : cliente.Endereco_ramal_com_2;
            pedidonovo.Endereco_logradouro = cliente.Endereco_logradouro;
            pedidonovo.Endereco_bairro = cliente.Endereco_bairro;
            pedidonovo.Endereco_cidade = cliente.Endereco_cidade;
            pedidonovo.Endereco_uf = cliente.Endereco_uf;
            pedidonovo.Endereco_cep = cliente.Endereco_cep;
            pedidonovo.Endereco_numero = cliente.Endereco_numero;
            pedidonovo.Endereco_complemento = cliente.Endereco_complemento == null ? "" : cliente.Endereco_complemento;
        }

        private void MontarDetalhesPedido(Tpedido pedidonovo, PedidoCriacaoDados pedido, Tcliente cliente,
            string usuario_atual)
        {
            //campos armazenados tanto no pedido - pai quanto no pedido - filhote
            pedidonovo.Id_Cliente = cliente.Id;
            pedidonovo.Midia = cliente.Midia;
            pedidonovo.Servicos = "";

            //detalhes do pedido
            pedidonovo.Vendedor = usuario_atual;
            pedidonovo.Usuario_Cadastro = usuario_atual;
            pedidonovo.St_Entrega = "";

            if (pedido.DetalhesPedido.EntregaImediata != "")
            {
                pedidonovo.St_Etg_Imediata = short.Parse(pedido.DetalhesPedido.EntregaImediata);
                pedidonovo.Etg_Imediata_Data = DateTime.Now;
                pedidonovo.Etg_Imediata_Usuario = usuario_atual;
            }

            pedidonovo.StBemUsoConsumo = pedido.DetalhesPedido.BemDeUso_Consumo;
            pedidonovo.InstaladorInstalaStatus = pedido.DetalhesPedido.InstaladorInstala;
            pedidonovo.InstaladorInstalaUsuarioUltAtualiz = usuario_atual;
            pedidonovo.InstaladorInstalaDtHrUltAtualiz = DateTime.Now;


            //referente ao magento
            pedidonovo.Pedido_Bs_X_At = "";
            pedidonovo.Pedido_Bs_X_Ac = pedido.Marketplace.Pedido_bs_x_ac ?? "";  //s_pedido_ac id do pedido magento
            pedidonovo.Pedido_Bs_X_Marketplace = pedido.Marketplace.Pedido_bs_x_marketplace ?? ""; //num pedido_marketplace
            pedidonovo.Marketplace_codigo_origem = pedido.Marketplace.Marketplace_codigo_origem ?? ""; //s_origem_pedido

            //Nota Fiscal
            pedidonovo.Nfe_Texto_Constar = pedido.DetalhesPedido.Observacoes;
            //verificar, pois no PedidoNovoConsiste é possivel inserir o tetxto o 
            //número de "xPed" a variavel é "c_num_pedido_compra" 
            pedidonovo.Nfe_XPed = "";

            //Comissão
            pedidonovo.Venda_Externa = pedido.Ambiente.Venda_Externa ? (short)1 : (short)0;
            pedidonovo.Loja_Indicou = pedido.Ambiente.Venda_Externa ? pedido.Ambiente.Loja : "";
            pedidonovo.Comissao_Loja_Indicou = 0;//comissao_loja_indicou
            pedidonovo.Indicador = pedido.Ambiente.Indicador ?? "";
            //campo Orcamentista pode ser null
            pedidonovo.Orcamentista = string.IsNullOrWhiteSpace(pedido.Ambiente.Orcamentista) ? null : pedido.Ambiente.Orcamentista;

            //quero ver o pq nao esta sendo salvo corretamente
            pedidonovo.GarantiaIndicadorStatus = pedido.DetalhesPedido.GarantiaIndicador != "0" &&
               pedido.DetalhesPedido.GarantiaIndicador != null ?
               byte.Parse(InfraBanco.Constantes.Constantes.COD_GARANTIA_INDICADOR_STATUS__SIM) :
                 byte.Parse(InfraBanco.Constantes.Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO);

            pedidonovo.GarantiaIndicadorUsuarioUltAtualiz = usuario_atual;
            pedidonovo.GarantiaIndicadorDtHrUltAtualiz = DateTime.Now;

            pedidonovo.Obs_1 = string.IsNullOrWhiteSpace(pedido.DetalhesPedido.Observacoes) ? "" : pedido.DetalhesPedido.Observacoes;
            //afazer: verificar qual é no prepedido esse campo "ConstaNaNF"
            //pedidonovo.Obs_2 = string.IsNullOrWhiteSpace(pedido.DetalhesPedido.ConstaNaNF) ? "" : pedido.DetalhesPedido.ConstaNaNF;

            pedidonovo.Sistema_responsavel_atualizacao = (int)pedido.Configuracao.SistemaResponsavelCadastro;
            pedidonovo.Sistema_responsavel_cadastro = (int)pedido.Configuracao.SistemaResponsavelCadastro;

            //RA
            pedidonovo.Permite_RA_Status = (short)(pedido.Valor.PedidoPossuiRa() ? 1 : 0);
            pedidonovo.Opcao_Possui_RA = pedido.Valor.PedidoPossuiRa() ? "S" : "-";
        }

        private async Task<string> GerarNumeroPedidoTemporario(List<string> lstErros, ContextoBdGravacao contextoBdGravacao)
        {
            string numPedido = "";
            string s_num = "";
            string s_letra_ano = "";
            int n_descarte = 0;
            string s_descarte = "";
            //passar o db ContextoBdGravacao
            var dbgravacao = contextoBdGravacao;

            s_num = await UtilsGlobais.Nsu.GerarNsu(contextoBdGravacao, InfraBanco.Constantes.Constantes.NSU_PEDIDO_TEMPORARIO);

            if (!string.IsNullOrEmpty(s_num))
            {
                n_descarte = s_num.Length - InfraBanco.Constantes.Constantes.TAM_MIN_NUM_PEDIDO;
                s_descarte = s_num.Substring(0, n_descarte);
                string teste = new String('0', n_descarte);
                if (s_descarte != teste)
                    return numPedido;

                s_num = s_num.Substring(s_num.Length - n_descarte);

                //obtém a letra para o sufixo do pedido de acordo c / o ano da geração do 
                //nsu(importante: fazer a leitura somente após gerar o nsu, pois a letra pode ter 
                //sido alterada devido à mudança de ano!!)
                var ret = await (from c in dbgravacao.Tcontroles
                                 where c.Id_Nsu == InfraBanco.Constantes.Constantes.NSU_PEDIDO_TEMPORARIO
                                 select c).FirstOrDefaultAsync();

                var controle = ret;

                if (controle == null)
                    lstErros.Add("Não existe registro na tabela de controle com o id = '" +
                        InfraBanco.Constantes.Constantes.NSU_PEDIDO_TEMPORARIO);
                else
                    s_letra_ano = controle.Ano_Letra_Seq;

                numPedido = "T" + s_num + s_letra_ano;

            }

            return numPedido;
        }

        private async Task<decimal> CalculaTotalRALiquidoBD(string id_pedido, ContextoBdGravacao dbGravacao, List<string> lstErros)
        {
            float percentual_desagio_RA_liquido = 0;
            decimal vl_total = 0;
            decimal vl_total_RA_liquido = 0;

            id_pedido = id_pedido.Trim();
            id_pedido = Normaliza_num_pedido(id_pedido);

            //busca pedido
            Tpedido tpedido = await (from c in dbGravacao.Tpedidos
                                     where c.Pedido == id_pedido
                                     select c).FirstOrDefaultAsync();

            if (tpedido == null)
            {
                lstErros.Add("Pedido-base " + id_pedido + " não foi encontrado.");
                return 0;
            }

            percentual_desagio_RA_liquido = tpedido.Perc_Desagio_RA_Liquida;

            //obtém os valores totais de nf, ra e venda
            var vlTotalTask = await (from c in dbGravacao.TpedidoItems.Include(x => x.Tpedido)
                                     where c.Tpedido.St_Entrega != InfraBanco.Constantes.Constantes.ST_ENTREGA_CANCELADO &&
                                           c.Tpedido.Pedido.Contains(id_pedido)
                                     select new
                                     {
                                         vlTotalRA = c.Qtde * (c.Preco_Lista - c.Preco_Venda)
                                     }).ToListAsync();
            vl_total = vlTotalTask.Sum(x => x.vlTotalRA ?? 0);

            vl_total_RA_liquido = (vl_total - ((decimal)percentual_desagio_RA_liquido / 100) * vl_total);

            return vl_total_RA_liquido;
        }

        public string Normaliza_num_pedido(string id_pedido)
        {
            string s_num = "";
            string s_ano = "";
            string s_filhote = "";
            string c = "";

            int letra_numerica;

            string retorno = "";

            if (string.IsNullOrEmpty(id_pedido))
                return retorno;

            for (int i = 0; i <= id_pedido.Length; i++)
            {
                if (int.TryParse(id_pedido.Substring(i, 1), out letra_numerica))
                {
                    s_num += letra_numerica;
                }
                else
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(s_num))
            {
                return retorno;
            }

            letra_numerica = 0;
            int eNumero;
            for (int i = 0; i < id_pedido.Length; i++)
            {
                c = id_pedido.Substring(i, 1);

                if (!int.TryParse(c, out eNumero))
                {
                    if (string.IsNullOrEmpty(s_ano))
                        s_ano = c;
                    else if (string.IsNullOrEmpty(s_filhote))
                        s_filhote = c;
                }
            }

            if (string.IsNullOrEmpty(s_ano))
                return retorno;

            s_num = UtilsGlobais.Util.Normaliza_Codigo(s_num, InfraBanco.Constantes.Constantes.TAM_MIN_NUM_PEDIDO);

            retorno = s_num + s_ano;

            if (!string.IsNullOrEmpty(s_filhote))
                retorno += InfraBanco.Constantes.Constantes.COD_SEPARADOR_FILHOTE + s_filhote;

            return retorno;
        }

        private void CadastrarIndicador(PedidoCriacaoDados pedido, Tpedido pedidonovo, Tpedido pedidonovoTrocaId,
            ContextoBdGravacao dbGravacao, ref string s_log_cliente_indicador, string id_cliente)
        {
            if (pedido.Ambiente.ComIndicador)
            {
                if (!string.IsNullOrEmpty(pedidonovo.Indicador))
                {
                    pedidonovoTrocaId.Indicador = pedido.Ambiente.Indicador;

                    //alterando indicado do pedido cadastrado
                    dbGravacao.Update(pedidonovoTrocaId);
                    dbGravacao.SaveChanges();

                    s_log_cliente_indicador = "Cadastrado o indicador '" + pedido.Ambiente.Indicador +
                       "' no cliente id=" + id_cliente;
                }
            }
        }

        private async Task AlterarStatusPedidoCadastrado(Tpedido pedidonovoTrocaId, Tpedido pedidonovo, PedidoCriacaoDados pedido,
            string idPedidoBase, int indicePedido, ContextoBdGravacao dbGravacao, decimal vl_total_RA_liquido)
        {

            //RA
            if (indicePedido == 1)
            {
                pedidonovoTrocaId.Vl_Total_RA_Liquido = Math.Round(vl_total_RA_liquido, 2);
                pedidonovoTrocaId.Qtde_Parcelas_Desagio_RA = 0;

                if (pedidonovo.Vl_Total_RA != 0)
                    pedidonovoTrocaId.St_Tem_Desagio_RA = 1;
                else
                    pedidonovoTrocaId.St_Tem_Desagio_RA = 0;

                //alterando RA e desagio pedido cadastrado
                dbGravacao.Update(pedidonovoTrocaId);
                await dbGravacao.SaveChangesAsync();
            }

        }

        private async Task VerificarSenhaDescontoSuperior(List<string> vdesconto, string usuario_atual,
            bool blnMagentoPedidoComIndicador, string opercao_origem, ContextoBdGravacao dbGravacao, List<string> lstErros)
        {
            foreach (var d in vdesconto)
            {
                if (!string.IsNullOrEmpty(d))
                {
                    Tdesconto tdesconto = await (from c in dbGravacao.Tdescontos
                                                 where c.Usado_status == 0 &&
                                                       c.Cancelado_status == 0 &&
                                                       c.Id == d
                                                 select c).FirstOrDefaultAsync();

                    if (tdesconto == null)
                    {
                        lstErros.Add("Senha de autorização para desconto superior não encontrado.");
                        return;
                    }
                    else
                    {
                        tdesconto.Usado_status = 1;
                        tdesconto.Usado_data = DateTime.Now;
                        if (opercao_origem == InfraBanco.Constantes.Constantes.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO &&
                            blnMagentoPedidoComIndicador)
                        {
                            tdesconto.Vendedor = usuario_atual;
                        }

                        tdesconto.Usado_usuario = usuario_atual;

                        //alterando tabela de desconto
                        dbGravacao.Update(tdesconto);
                        await dbGravacao.SaveChangesAsync();
                    }
                }

            }
        }

        private string VerificarStatusEntrega(Tpedido pedidonovoTrocaId, decimal total_estoque_vendido,
            decimal total_estoque_sem_presenca, ContextoBdGravacao dbGravacao)
        {
            string status_entrega = "";

            if (total_estoque_vendido == 0)
                status_entrega = InfraBanco.Constantes.Constantes.ST_ENTREGA_ESPERAR;
            else if (total_estoque_sem_presenca == 0)
                status_entrega = InfraBanco.Constantes.Constantes.ST_ENTREGA_SEPARAR;
            else
                status_entrega = InfraBanco.Constantes.Constantes.ST_ENTREGA_SPLIT_POSSIVEL;

            //pedidonovoTrocaId.St_Entrega = status_entrega;

            //dbGravacao.Update(pedidonovoTrocaId);
            //await dbGravacao.SaveChangesAsync();

            return status_entrega;
        }

        private async Task AnalisarEndereco(PedidoCriacaoDados pedido, Tpedido pedidonovoTrocaId, Tpedido pedidonovo, Tcliente cliente,
            string idPedidoBase, string usuario_atual, List<string> lstErros, ContextoBdGravacao dbGravacao)
        {
            bool blnAnalisarEndereco = false;

            //essa variavel esta substituindo as 2 variaveis "blnAnalisarEndereco" "blnAnEnderecoCadClienteUsaEndParceiro"
            bool blnAnalisaEndereco_Com_blnUsaEndParcrceiro = false;

            if (lstErros.Count == 0)
            {
                //1) verifica se o endereço usado é o do parceiro
                //CompararEndereco do cadastro do cliente com o orçamentista aqui
                blnAnalisaEndereco_Com_blnUsaEndParcrceiro = await CompararEndereco_Cadastro_Parceiro_Cadastrar(pedido,
                    pedidonovoTrocaId, pedidonovo, idPedidoBase, lstErros, dbGravacao);
            }
            if (lstErros.Count == 0)
            {
                //2)verifica pedidos de outros clientes
                if (!blnAnalisaEndereco_Com_blnUsaEndParcrceiro)
                {
                    List<Cl_ANALISE_ENDERECO_CONFRONTACAO> vAnEndConfrontacao = new List<Cl_ANALISE_ENDERECO_CONFRONTACAO>();

                    List<TpedidoEnderecoConfrontacaoDados> lstTpedidoEndConfrontacao =
                        (await MontarListaEnderecoParaConfrotacaoEndParceiro(cliente, pedidonovo, dbGravacao)).ToList();

                    MontarListaParaConfrontacao(cliente, lstTpedidoEndConfrontacao, vAnEndConfrontacao);

                    foreach (var tPedidoEndConfrontacao in lstTpedidoEndConfrontacao)
                    {
                        if (Pedido.Criacao.Passo60.Gravacao.Grava80.CompararEndereco.IsEnderecoIgual(cliente.Endereco,
                            cliente.Endereco_Numero, cliente.Cep,
                            tPedidoEndConfrontacao.Pedido.Endereco_logradouro,
                            tPedidoEndConfrontacao.Pedido.Endereco_numero,
                            tPedidoEndConfrontacao.Pedido.Endereco_cep))
                        {
                            vAnEndConfrontacao.Add(Cl_ANALISE_ENDERECO_CONFRONTACAO.Construir(tPedidoEndConfrontacao));
                            if (vAnEndConfrontacao.Count >=
                                InfraBanco.Constantes.Constantes.MAX_AN_ENDERECO_QTDE_PEDIDOS_CADASTRAMENTO)
                            {
                                break;
                            }
                        }
                    }

                    //vou retornar um bool para substituir o "blnAnalisarEndereco"
                    blnAnalisarEndereco = await CadastrarAnaliseEndereco(vAnEndConfrontacao, pedidonovo, cliente, usuario_atual,
                        lstErros, dbGravacao);

                }

                if (lstErros.Count == 0)
                {
                    //endereço de entrega(se houver)
                    if (pedido.EnderecoEntrega.OutroEndereco)
                    {
                        blnAnalisarEndereco = await CompararEndereco_Entrega_Parceiro_OutrosClientes(pedidonovoTrocaId, cliente,
                            pedidonovo, idPedidoBase, usuario_atual, lstErros, dbGravacao);
                    }
                }
            }
            if (lstErros.Count == 0)
            {
                if (blnAnalisarEndereco)
                {
                    //analise_endereco_tratar_status
                    //na comparação de pedidos arclube - 119664N / meu - 119663N, no meu foi salvo como 0
                    //sendo assim, não entrou nesse bloco
                    pedidonovoTrocaId.Analise_Endereco_Tratar_Status = 1;
                    pedidonovoTrocaId = pedidonovo;

                    dbGravacao.Update(pedidonovoTrocaId);
                    await dbGravacao.SaveChangesAsync();
                }
            }
        }

        private async Task<bool> CompararEndereco_Cadastro_Parceiro_Cadastrar(PedidoCriacaoDados pedido, Tpedido pedidonovoTrocaId, Tpedido pedidonovo,
            string idPedidoBase, List<string> lstErros, ContextoBdGravacao dbGravacao)
        {
            bool blnAnalisaEndereco_ComUsaEndParcrceiro = false;
            int intNsuPai = 0;

            if (pedido.Ambiente.ComIndicador)
            {
                if (!string.IsNullOrEmpty(pedidonovoTrocaId.Indicador))
                {
                    //buscar orçamentista para comparar
                    TorcamentistaEindicador torcamentista = await prepedidoBll.BuscarTorcamentista(pedidonovoTrocaId.Indicador);

                    //verificar se o endereço é igual
                    //CompararEndereco do cadastro do cliente com o orçamentista aqui
                    if (Pedido.Criacao.Passo60.Gravacao.Grava80.CompararEndereco.IsEnderecoIgual(pedidonovoTrocaId.Endereco_logradouro,
                        pedidonovoTrocaId.Endereco_numero,
                        pedidonovo.Endereco_cep.Replace("-", ""),
                        torcamentista.Endereco,
                        torcamentista.Endereco_Numero,
                        torcamentista.Cep.Replace("-", "")))
                    {
                        blnAnalisaEndereco_ComUsaEndParcrceiro = true;

                        //gerar fin_gera_nsu
                        intNsuPai = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO,
                            dbGravacao);

                        if (intNsuPai == 0)
                        {
                            lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO(" + lstErros.Last() + ")");
                        }
                        else
                        {
                            TpedidoAnaliseEndereco tpedidoAnaliseEnd = new TpedidoAnaliseEndereco();
                            tpedidoAnaliseEnd.Id = intNsuPai;
                            tpedidoAnaliseEnd.Pedido = idPedidoBase;
                            tpedidoAnaliseEnd.Id_cliente = pedidonovoTrocaId.Id_Cliente;
                            tpedidoAnaliseEnd.Tipo_endereco = InfraBanco.Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE;
                            tpedidoAnaliseEnd.Endereco_logradouro = pedidonovoTrocaId.Endereco_logradouro;
                            tpedidoAnaliseEnd.Endereco_bairro = pedidonovoTrocaId.Endereco_bairro;
                            tpedidoAnaliseEnd.Endereco_cidade = pedidonovoTrocaId.Endereco_cidade;
                            tpedidoAnaliseEnd.Endereco_uf = pedidonovoTrocaId.Endereco_uf;
                            tpedidoAnaliseEnd.Endereco_cep = pedidonovoTrocaId.Endereco_cep;
                            tpedidoAnaliseEnd.Endereco_numero = pedidonovoTrocaId.Endereco_numero;
                            tpedidoAnaliseEnd.Endereco_complemento = pedidonovoTrocaId.Endereco_complemento;
                            tpedidoAnaliseEnd.Usuario_cadastro = pedidonovoTrocaId.Usuario_Cadastro;

                            dbGravacao.Add(tpedidoAnaliseEnd);
                            await dbGravacao.SaveChangesAsync();
                        }
                    }
                }
            }

            return blnAnalisaEndereco_ComUsaEndParcrceiro;
        }

        private async Task<IEnumerable<TpedidoEnderecoConfrontacaoDados>> MontarListaEnderecoParaConfrotacaoEndParceiro(
            Tcliente cliente, Tpedido pedidonovo, ContextoBdGravacao dbGravacao)
        {

            var tpedidoCli_St_0Task = await (from c in dbGravacao.Tpedidos.Include(x => x.Tcliente)
                                             where c.Endereco_memorizado_status == 0 &&
                                                   c.Tcliente.Id != cliente.Id &&
                                                   c.Tcliente.Cep == cliente.Cep.Replace("-", "").Trim()
                                             select new TpedidoEnderecoConfrontacaoDados
                                             (
                                                 c,
                                                 InfraBanco.Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE
                                             )).ToListAsync();


            var tpedido_St_1Task = await (from c in dbGravacao.Tpedidos
                                          where c.Endereco_memorizado_status == 1 &&
                                                c.Id_Cliente != cliente.Id &&
                                                c.Endereco_cep == cliente.Cep
                                          select new TpedidoEnderecoConfrontacaoDados
                                          (
                                              c,
                                              InfraBanco.Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE_MEMORIZADO
                                          )).ToListAsync();

            var tpedido_St_Entrega_1Task = await (from c in dbGravacao.Tpedidos
                                                  where c.St_End_Entrega == 1 &&
                                                        c.Id_Cliente != cliente.Id &&
                                                        c.EndEtg_Cep == pedidonovo.EndEtg_Cep
                                                  select new TpedidoEnderecoConfrontacaoDados
                                                  (
                                                      c,
                                                      InfraBanco.Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__END_ENTREGA
                                                  )).ToListAsync();


            var tpedidoUnion1 = tpedidoCli_St_0Task;
            var tpedidoUnion2 = tpedido_St_1Task;
            var tpedidoUnion3 = tpedido_St_Entrega_1Task;

            List<TpedidoEnderecoConfrontacaoDados> lstTpedidoEndConfrontacao = (tpedidoUnion1
                .Union(tpedidoUnion2)
                .Union(tpedidoUnion3)
                .Distinct().OrderByDescending(x => x.Pedido.Data_Hora)).ToList();

            return lstTpedidoEndConfrontacao;
        }

        private void MontarListaParaConfrontacao(Tcliente cliente,
            List<TpedidoEnderecoConfrontacaoDados> lstTpedidoEndConfrontacao,
            List<Cl_ANALISE_ENDERECO_CONFRONTACAO> vAnEndConfrontacao)
        {

            foreach (var tPedidoEndConfrontacao in lstTpedidoEndConfrontacao)
            {
                if (Pedido.Criacao.Passo60.Gravacao.Grava80.CompararEndereco.IsEnderecoIgual(cliente.Endereco,
                    cliente.Endereco_Numero, cliente.Cep,
                    tPedidoEndConfrontacao.Pedido.Endereco_logradouro,
                    tPedidoEndConfrontacao.Pedido.Endereco_numero,
                    tPedidoEndConfrontacao.Pedido.Endereco_cep))
                {
                    //if (vAnEndConfrontacao.Count != 0)
                    //{
                    vAnEndConfrontacao.Add(Cl_ANALISE_ENDERECO_CONFRONTACAO.Construir(tPedidoEndConfrontacao));

                    if (vAnEndConfrontacao.Count >=
                        InfraBanco.Constantes.Constantes.MAX_AN_ENDERECO_QTDE_PEDIDOS_CADASTRAMENTO)
                    {
                        break;
                    }
                }
            }
        }

        private async Task<bool> CadastrarAnaliseEndereco(List<Cl_ANALISE_ENDERECO_CONFRONTACAO> vAnEndConfrontacao,
            Tpedido pedidonovo, Tcliente cliente, string usuario_atual, List<string> lstErros, ContextoBdGravacao dbGravacao)
        {
            //vamos retornar essa variavel
            bool blnAnalisarEndereco = false;
            bool blnGravouRegPai = false;
            int intNsuPai = 0;
            int intNsu = 0;

            foreach (var i in vAnEndConfrontacao)
            {
                if (!string.IsNullOrEmpty(i.Pedido))
                {
                    blnAnalisarEndereco = true;
                    //já gravou o registro pai ?
                    if (!blnGravouRegPai)
                    {
                        blnGravouRegPai = true;
                        intNsuPai = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO,
                            dbGravacao);

                        if (intNsuPai == 0)
                        {
                            lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
                            return false;
                        }
                        else
                        {
                            TpedidoAnaliseEndereco tpedidoEndAnalise = new TpedidoAnaliseEndereco
                            {

                                Id = intNsuPai,
                                Pedido = pedidonovo.Pedido,
                                Id_cliente = cliente.Id,
                                Tipo_endereco = InfraBanco.Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE,
                                Endereco_logradouro = cliente.Endereco,
                                Endereco_bairro = cliente.Bairro,
                                Endereco_cidade = cliente.Cidade,
                                Endereco_cep = cliente.Cep,
                                Endereco_uf = cliente.Uf,
                                Endereco_numero = cliente.Endereco_Numero,
                                Endereco_complemento = cliente.Endereco_Complemento,
                                Dt_cadastro = DateTime.Now.Date,
                                Dt_hr_cadastro = DateTime.Now,
                                Usuario_cadastro = usuario_atual.ToUpper()
                            };

                            dbGravacao.Add(tpedidoEndAnalise);
                            await dbGravacao.SaveChangesAsync();
                        }
                    }

                    intNsu = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO,
                        dbGravacao);

                    if (intNsu == 0)
                    {
                        lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
                        return false;
                    }
                    else
                    {
                        TpedidoAnaliseEnderecoConfrontacao tpedidoAnaliseConfrontacao = new TpedidoAnaliseEnderecoConfrontacao
                        {
                            Id = intNsu,
                            Id_pedido_analise_endereco = intNsuPai,
                            Pedido = i.Pedido,
                            Id_cliente = cliente.Id,
                            Tipo_endereco = InfraBanco.Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE,
                            Endereco_logradouro = i.Endereco_logradouro,
                            Endereco_bairro = i.Endereco_bairro,
                            Endereco_cidade = i.Endereco_cidade,
                            Endereco_cep = i.Endereco_cep,
                            Endereco_uf = i.Endereco_uf,
                            Endereco_numero = i.Endereco_numero,
                            Endereco_complemento = i.Endereco_complemento
                        };

                        dbGravacao.Add(tpedidoAnaliseConfrontacao);
                        await dbGravacao.SaveChangesAsync();
                    }
                }
            }
            return blnAnalisarEndereco;
        }

        private async Task<bool> CompararEndereco_Entrega_Parceiro(Tpedido pedidonovoTrocaId, Tpedido pedidonovo,
            string idPedidoBase, string usuario_atual, List<string> lstErros, ContextoBdGravacao dbGravacao)
        {
            bool blnAnalisarEndereco = false;
            int intNsuPai = 0;
            int intNsu = 0;

            TorcamentistaEindicador torcamentista = await prepedidoBll.BuscarTorcamentista(pedidonovoTrocaId.Indicador);

            //verificar se o endereço é igual
            if (Pedido.Criacao.Passo60.Gravacao.Grava80.CompararEndereco.IsEnderecoIgual(pedidonovo.EndEtg_Endereco, pedidonovoTrocaId.EndEtg_Endereco_Numero,
                pedidonovoTrocaId.EndEtg_Cep.Replace("-", ""), torcamentista.Endereco,
                torcamentista.Endereco_Numero,
                torcamentista.Cep.Replace("-", "")))
            {
                //blnAnEnderecoEndEntregaUsaEndParceiro = true;
                blnAnalisarEndereco = true;

                intNsuPai = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO,
                            dbGravacao);

                if (intNsuPai == 0)
                {
                    lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
                    return false;
                }
                else
                {
                    intNsu = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO,
                            dbGravacao);

                    TpedidoAnaliseEndereco tpedidoAnaliseEnd = new TpedidoAnaliseEndereco();

                    tpedidoAnaliseEnd.Id = intNsuPai;
                    tpedidoAnaliseEnd.Pedido = idPedidoBase;
                    tpedidoAnaliseEnd.Id_cliente = pedidonovoTrocaId.Id_Cliente;
                    tpedidoAnaliseEnd.Tipo_endereco = InfraBanco.Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__END_ENTREGA;
                    tpedidoAnaliseEnd.Endereco_logradouro = pedidonovoTrocaId.EndEtg_Endereco;
                    tpedidoAnaliseEnd.Endereco_bairro = pedidonovoTrocaId.EndEtg_Bairro;
                    tpedidoAnaliseEnd.Endereco_cidade = pedidonovoTrocaId.EndEtg_Cidade;
                    tpedidoAnaliseEnd.Endereco_uf = pedidonovoTrocaId.EndEtg_UF;
                    tpedidoAnaliseEnd.Endereco_cep = pedidonovoTrocaId.EndEtg_Cep;
                    tpedidoAnaliseEnd.Endereco_numero = pedidonovoTrocaId.EndEtg_Endereco_Numero;
                    tpedidoAnaliseEnd.Endereco_complemento = pedidonovoTrocaId.EndEtg_Endereco_Complemento;
                    tpedidoAnaliseEnd.Usuario_cadastro = usuario_atual;

                    dbGravacao.Add(tpedidoAnaliseEnd);
                    //await dbGravacao.SaveChangesAsync();
                }

                if (lstErros.Count == 0)
                {
                    intNsu = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO,
                            dbGravacao);

                    if (intNsuPai == 0)
                    {
                        lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
                        return false;
                    }
                    else
                    {
                        TpedidoAnaliseEnderecoConfrontacao tpedidoAnaliseConfrontacao = new TpedidoAnaliseEnderecoConfrontacao
                        {
                            Id = intNsu,
                            Id_pedido_analise_endereco = intNsuPai,
                            Pedido = "",
                            Id_cliente = "",
                            Tipo_endereco = InfraBanco.Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__END_PARCEIRO,
                            Endereco_logradouro = torcamentista.Endereco,
                            Endereco_bairro = torcamentista.Bairro,
                            Endereco_cidade = torcamentista.Cidade,
                            Endereco_cep = torcamentista.Cep,
                            Endereco_uf = torcamentista.Uf,
                            Endereco_numero = torcamentista.Endereco_Numero,
                            Endereco_complemento = torcamentista.Endereco_Complemento
                        };

                        dbGravacao.Add(tpedidoAnaliseConfrontacao);
                        //await dbGravacao.SaveChangesAsync();
                    }
                }
            }

            return blnAnalisarEndereco;
        }

        private async Task<bool> CompararEndereco_Entrega_Parceiro_OutrosClientes(Tpedido pedidonovoTrocaId, Tcliente cliente,
            Tpedido pedidonovo, string idPedidoBase, string usuario_atual, List<string> lstErros, ContextoBdGravacao dbGravacao)
        {
            //vou retornar essa var
            bool blnAnalisarEndereco = false;
            bool blnAnEnderecoEndEntregaUsaEndParceiro = false;

            //1) verifica se o endereço usado é o do parceiro
            if (!string.IsNullOrEmpty(pedidonovoTrocaId.Indicador))
            {
                if (await CompararEndereco_Entrega_Parceiro(pedidonovoTrocaId, pedidonovo, idPedidoBase,
                    usuario_atual, lstErros, dbGravacao))
                {
                    blnAnEnderecoEndEntregaUsaEndParceiro = true;
                    blnAnalisarEndereco = true;
                }
            }

            //2)verifica pedidos de outros clientes
            if (lstErros.Count == 0)
            {
                if (!blnAnEnderecoEndEntregaUsaEndParceiro)
                {
                    //A lista será montada "MontarListaParaConfrontacao(cliente, lstTpedidoEndConfrontacao, vAnEndConfrontacao);"
                    List<Cl_ANALISE_ENDERECO_CONFRONTACAO> vAnEndConfrontacao = new List<Cl_ANALISE_ENDERECO_CONFRONTACAO>();

                    //nova chamada
                    List<TpedidoEnderecoConfrontacaoDados> lstTpedidoEndConfrontacao =
                        (await MontarListaEnderecoParaConfrotacaoEndParceiro(cliente, pedidonovo, dbGravacao)).ToList();

                    MontarListaParaConfrontacao(cliente, lstTpedidoEndConfrontacao, vAnEndConfrontacao);

                    blnAnalisarEndereco = await CadastrarAnaliseEndereco(vAnEndConfrontacao, pedidonovo, cliente, usuario_atual,
                        lstErros, dbGravacao);

                }
            }
            return blnAnalisarEndereco;
        }


        private async Task<MovimentoEstoqueDados> Novo_VerificarListaRegras(TpedidoItem t_pedido_item,
            t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD regra_UF_Pessoa_CD, Cl_ITEM_PEDIDO_NOVO item, int vEmpresaAutoSplit,
            string usuario_atual,
            string id_pedido_temp, List<string> lstErros, ContextoBdGravacao dbGravacao)
        {
            MovimentoEstoqueDados movtoEstoque = new MovimentoEstoqueDados();

            short qtde_spe = 0;

            await MontarTpedidoItemParaCadastrar(item, id_pedido_temp, t_pedido_item, dbGravacao);

            if (regra_UF_Pessoa_CD.Estoque_Qtde_Solicitado > regra_UF_Pessoa_CD.Estoque_Qtde)
            {
                qtde_spe = (short)(regra_UF_Pessoa_CD.Estoque_Qtde_Solicitado - regra_UF_Pessoa_CD.Estoque_Qtde);
            }
            else
            {
                qtde_spe = 0;
            }

            QuantidadeEncapsulada qtde_estoque_vendido = new QuantidadeEncapsulada { Valor = 0 };
            QuantidadeEncapsulada qtde_estoque_sem_presenca = new QuantidadeEncapsulada { Valor = 0 };
            if (!await Estoque_produto_saida_v2(usuario_atual, id_pedido_temp, (short)vEmpresaAutoSplit,
                item.Fabricante, item.Produto, (int)(regra_UF_Pessoa_CD.Estoque_Qtde_Solicitado ?? 0), qtde_spe,
                qtde_estoque_vendido, qtde_estoque_sem_presenca,
                lstErros, dbGravacao))
            {
                lstErros.Add(InfraBanco.Constantes.Constantes.ERR_FALHA_OPERACAO_MOVIMENTO_ESTOQUE);
            }

            //altera lista que concordou mesmo sem presença de estoque
            item.Qtde_estoque_vendido = (short)(item.Qtde_estoque_vendido + qtde_estoque_vendido.Valor);
            item.Qtde_estoque_sem_presenca = (short)(item.Qtde_estoque_sem_presenca + qtde_estoque_sem_presenca.Valor);

            movtoEstoque.Total_estoque_vendido += qtde_estoque_vendido.Valor;
            movtoEstoque.Total_estoque_sem_presenca += qtde_estoque_sem_presenca.Valor;

            if (!string.IsNullOrEmpty(movtoEstoque.Slog_item_autosplit))
            {
                movtoEstoque.Slog_item_autosplit = movtoEstoque.Slog_item_autosplit + " ";
            }

            movtoEstoque.Slog_item_autosplit = movtoEstoque.Slog_item_autosplit + "(" + item.Fabricante + ")" +
                item.Produto + ":" + " Qtde Solicitada = " +
                regra_UF_Pessoa_CD.Estoque_Qtde_Solicitado + "," +
                " Qtde Sem Presença Autorizada = " + qtde_spe.ToString() + "," +
                " Qtde Estoque Vendido = " + qtde_estoque_vendido.Valor.ToString() + "," +
                " Qtde Sem Presença = " + qtde_estoque_sem_presenca.Valor.ToString();

            return movtoEstoque;
        }

        private async Task MontarTpedidoItemParaCadastrar(Cl_ITEM_PEDIDO_NOVO v_item, string id_pedido_temp,
            TpedidoItem tpedidoItem, ContextoBdGravacao dbGravacao)
        {

            tpedidoItem.Pedido = id_pedido_temp;
            tpedidoItem.Fabricante = v_item.Fabricante;
            tpedidoItem.Produto = v_item.Produto;
            tpedidoItem.Qtde = v_item.Qtde;
            tpedidoItem.Desc_Dado = v_item.Desc_Dado;
            tpedidoItem.Preco_Venda = v_item.Preco_Venda;
            tpedidoItem.Preco_NF = v_item.Preco_NF;
            tpedidoItem.Preco_Fabricante = v_item.Preco_fabricante;
            tpedidoItem.Vl_Custo2 = v_item.Vl_custo2;
            tpedidoItem.Preco_Lista = v_item.Preco_lista;
            tpedidoItem.Margem = v_item.Margem;
            tpedidoItem.Desc_Max = v_item.Desc_max;
            tpedidoItem.Comissao = v_item.Comissao;
            tpedidoItem.Descricao = v_item.Descricao;
            tpedidoItem.Descricao_Html = v_item.Descricao_html;
            tpedidoItem.Ean = v_item.Ean;
            tpedidoItem.Grupo = v_item.Grupo;
            tpedidoItem.Peso = v_item.Peso;
            tpedidoItem.Qtde_Volumes = v_item.Qtde_volumes;
            tpedidoItem.Abaixo_Min_Status = v_item.Abaixo_min_status;
            tpedidoItem.Abaixo_Min_Autorizacao = v_item.Abaixo_min_autorizacao;
            tpedidoItem.Abaixo_Min_Autorizador = v_item.Abaixo_min_autorizador;
            tpedidoItem.Abaixo_Min_Superv_Autorizador = v_item.Abaixo_min_superv_autorizador;
            tpedidoItem.Sequencia = v_item.Sequencia;
            tpedidoItem.Markup_Fabricante = v_item.Markup_fabricante;
            tpedidoItem.CustoFinancFornecCoeficiente = v_item.CustoFinancFornecCoeficiente;
            tpedidoItem.CustoFinancFornecPrecoListaBase = v_item.CustoFinancFornecPrecoListaBase;
            tpedidoItem.Cubagem = v_item.Cubagem;
            tpedidoItem.Ncm = v_item.Ncm;
            tpedidoItem.Cst = v_item.Cst;
            tpedidoItem.Descontinuado = v_item.Descontinuado;

            dbGravacao.Add(tpedidoItem);
            await dbGravacao.SaveChangesAsync();
        }
    }
}
