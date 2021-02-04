using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Pedido.Dados.Criacao;
using Produto.RegrasCrtlEstoque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava60
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0054:Use compound assignment", Justification = "Estilo de código")]
    class Grava60 : PassoBaseGravacao
    {
        private readonly Grava70.Grava70 Grava70;
        public Grava60(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao)
        {
            Grava70 = new Grava70.Grava70(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao);
        }

        public async Task ExecutarAsync()
        {

            //Passo60: criar pedidos -'	CADASTRA O PEDIDO E PROCESSA A MOVIMENTAÇÃO NO ESTOQUE
            //	Loop nos CDs a utilizar
            //		Gerar o número do pedido: Passo60 / Gerar_o_numero_do_pedido.feature
            //		Adiciona um novo pedido
            //		Preenche os campos do pedido: Passo60 / Preenche_os_campos_do_pedido.feature
            //			a maioria no pai e filhotes, alguns só no pai, alguns só nos filhotes
            //		Salva o registro em t_pedido

            //		Loop nas regras:
            //			Especificado em Passo60 / Itens / Gerar_t_PEDIDO_ITEM.feature
            //				Se essa regra cobrir um dos itens do pedido, adicionar registro em t_PEDIDO_ITEM(linha 2090 até 2122)
            //				Note que a quantidade rs("qtde") é a que foi alocada para esse filhote pela regra, não a quantidade total do pedido inteiro
            //				A sequencia do t_PEDIDO_ITEM para esse pedido(base ou filhote) começa de 1 e é sequencial.
            //			Se qtde_solicitada > qtde_estoque, qtde_spe(quantidade_sen_presença_estoque) fica com o número de itens faltando
            //		   chama rotina ESTOQUE_produto_saida_v2, em Passo60 / Itens / ESTOQUE_produto_saida_v2.feature
            //				A quantidade deste item ou efetivamente sai do estoque(atualizando t_ESTOQUE_ITEM)
            //				ou entra como venda sem presença no estoque(novo registro na tabela t_ESTOQUE_MOVIMENTO, operacao = OP_ESTOQUE_VENDA, estoque = ID_ESTOQUE_SEM_PRESENCA)
            //			Monta o log do item - Passo60 / Itens / Log.feature
            //
            //
            //		Determina o status st_entrega deste pedido(Passo60 / st_entrega.feature)

            //no loja/PedidoNovoConfirma.asp, vai do 
            //cn.BeginTrans 
            //até o 
            //s="UPDATE t_PEDIDO SET pedido='" & id_pedido & "' WHERE pedido='" & id_pedido_temp & "'"


            bool permite_RA_status = false;
            {
                if (!string.IsNullOrEmpty(Pedido.Ambiente.Indicador))
                {
                    permite_RA_status = Execucao.TOrcamentista_Permite_RA_Status != 0;
                }

                if ((Pedido.Ambiente.Operacao_origem == Constantes.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO)
                                && Execucao.BlnMagentoPedidoComIndicador)
                {
                    permite_RA_status = true;
                }
            }


            //cira alista de produtos com as variaveis auxiliares
            List<ProdutoGravacao> listaProdutoGravacao = ProdutoGravacao.ListaProdutoGravacao(Pedido.ListaProdutos);

            //s_hora_pedido = retorna_so_digitos(formata_hora(Now))
            Execucao.Gravacao.Hora_pedido = UtilsGlobais.Util.HoraParaBanco(Execucao.Gravacao.DataHoraCriacao);
            int indice_pedido = 0;

            foreach (var vEmpresaAutoSplit_iv in Execucao.Gravacao.EmpresasAutoSplit)
            {
                //o primeiro é 1, inicializamos com 0
                indice_pedido += 1;
                Tpedido tpedido = new Tpedido();
                InicializarCamposDefault(tpedido);

                //'	Controla a quantidade de pedidos no auto-split
                //'	pedido-base: indice_pedido=1
                //'	pedido-filhote 'A' => indice_pedido=2
                //'	pedido-filhote 'B' => indice_pedido=3
                //'	etc
                string id_pedido = Execucao.Gravacao.Id_pedido_base;
                if (indice_pedido == 1)
                {
                    Retorno.Id = id_pedido;
                }
                else
                {
                    id_pedido = Execucao.Gravacao.Id_pedido_base +
                        InfraBanco.Constantes.Constantes.COD_SEPARADOR_FILHOTE +
                        Gera_num_pedido.Gera_letra_pedido_filhote(indice_pedido - 1);
                    Retorno.ListaIdPedidosFilhotes.Add(id_pedido);
                }
                string id_pedido_temp = id_pedido;


                //transferir campos
                tpedido.Pedido = id_pedido_temp;
                tpedido.Loja = Pedido.Ambiente.Loja;
                tpedido.Data = Execucao.Gravacao.DataHoraCriacao.Date;
                tpedido.Hora = Execucao.Gravacao.Hora_pedido;

                if (indice_pedido == 1)
                    CamposPedidoPai(tpedido);
                else
                    CamposPedidoFilhote(tpedido);

                CamposPedidoPaieFilhote(permite_RA_status, vEmpresaAutoSplit_iv, tpedido);

                //=======================
                //salvar os itens
                short sequencia_item = 0;
                var total_estoque_vendido = 0;
                var total_estoque_sem_presenca = 0;
                var s_log_item_autosplit = "";
                foreach (var vProdRegra_iRegra in Execucao.Gravacao.ListaRegrasControleEstoque)
                {
                    //vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD) é twmsCdXUfXPessoaXCd
                    foreach (var twmsCdXUfXPessoaXCd in vProdRegra_iRegra.TwmsCdXUfXPessoaXCd)
                    {
                        if ((twmsCdXUfXPessoaXCd.Id_nfe_emitente == vEmpresaAutoSplit_iv.Id_nfe_emitente)
                            && (twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado > 0))
                        {
                            //'	LOCALIZA O PRODUTO EM V_ITEM
                            ProdutoGravacao? linha_pedido = null;
                            foreach (var produto in listaProdutoGravacao)
                            {
                                if ((produto.Pedido.Fabricante == twmsCdXUfXPessoaXCd.Estoque_Fabricante)
                                    && (produto.Pedido.Produto == twmsCdXUfXPessoaXCd.Estoque_Produto))
                                {
                                    linha_pedido = produto;
                                    break;
                                }
                            }
                            if (linha_pedido != null)
                            {
                                sequencia_item += (short)1;
                                Criar_TpedidoItem(id_pedido_temp, sequencia_item, twmsCdXUfXPessoaXCd, linha_pedido);

                                int qtde_spe;
                                if (twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado > twmsCdXUfXPessoaXCd.Estoque_Qtde_Estoque)
                                {
                                    qtde_spe = twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado.Value - twmsCdXUfXPessoaXCd.Estoque_Qtde_Estoque.Value;
                                }
                                else
                                {
                                    qtde_spe = 0;
                                }

                                Produto.Estoque.Estoque.QuantidadeEncapsulada qtde_estoque_vendido = new Produto.Estoque.Estoque.QuantidadeEncapsulada { Valor = 0 };
                                Produto.Estoque.Estoque.QuantidadeEncapsulada qtde_estoque_sem_presenca = new Produto.Estoque.Estoque.QuantidadeEncapsulada { Valor = 0 };
                                if (!await Produto.Estoque.Estoque.Estoque_produto_saida_v2(
                                    Pedido.Ambiente.Usuario, id_pedido_temp,
                                    (short)vEmpresaAutoSplit_iv.Id_nfe_emitente,
                                    linha_pedido.Pedido.Fabricante, linha_pedido.Pedido.Produto,
                                    (int)(twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado ?? 0),
                                    qtde_spe,
                                    qtde_estoque_vendido,
                                    qtde_estoque_sem_presenca,
                                    Retorno.ListaErros, ContextoBdGravacao))
                                {
                                    Retorno.ListaErros.Add("Erro em operação de movimentação de estoque, código do erro " + InfraBanco.Constantes.Constantes.ERR_FALHA_OPERACAO_MOVIMENTO_ESTOQUE);
                                    //pode abortar tudo!
                                    return;
                                }


                                linha_pedido.Qtde_estoque_vendido = linha_pedido.Qtde_estoque_vendido + qtde_estoque_vendido.Valor;
                                linha_pedido.Qtde_estoque_sem_presenca = linha_pedido.Qtde_estoque_sem_presenca + qtde_estoque_sem_presenca.Valor;

                                total_estoque_vendido = total_estoque_vendido + qtde_estoque_vendido.Valor;
                                total_estoque_sem_presenca = total_estoque_sem_presenca + qtde_estoque_sem_presenca.Valor;

                                /*
//todo: log grava60
                                '	LOG
                                    if s_log_item_autosplit <> "" then s_log_item_autosplit = s_log_item_autosplit & chr(13)
                                    s_log_item_autosplit = s_log_item_autosplit & "(" & .fabricante & ")" & .produto & ":" & _
                                                " Qtde Solicitada = " & twmsCdXUfXPessoaXCd.estoque.qtde_solicitada & "," & _
                                                " Qtde Sem Presença Autorizada = " & Cstr(qtde_spe) & "," & _
                                                " Qtde Estoque Vendido = " & Cstr(qtde_estoque_vendido_aux) & "," & _
                                                " Qtde Sem Presença = " & Cstr(qtde_estoque_sem_presenca_aux)
                */
                            }
                        }
                    }
                }

                /*
//todo: log grava60

                '	LOG
                    if Trim("" & vLogAutoSplit(UBound(vLogAutoSplit))) <> "" then redim preserve vLogAutoSplit(UBound(vLogAutoSplit)+1)
                    vLogAutoSplit(UBound(vLogAutoSplit)) = id_pedido & " (" & obtem_apelido_empresa_NFe_emitente(vEmpresaAutoSplit_iv.Id_nfe_emitente) & ")" & chr(13) & _
                                                            s_log_item_autosplit

                */
                //'	STATUS DE ENTREGA
                if (total_estoque_vendido == 0)
                {
                    tpedido.St_Entrega = Constantes.ST_ENTREGA_ESPERAR;
                }
                else
                {
                    if (total_estoque_sem_presenca == 0)
                    {
                        tpedido.St_Entrega = Constantes.ST_ENTREGA_SEPARAR;
                    }
                    else
                    {
                        tpedido.St_Entrega = Constantes.ST_ENTREGA_SPLIT_POSSIVEL;
                    }
                }

                //ajustes do passo70
                await Grava70.Executar(tpedido, indice_pedido);

                ContextoBdGravacao.Add(tpedido);
            }

        }

        private void Criar_TpedidoItem(string id_pedido_temp, short sequencia_item, t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD twmsCdXUfXPessoaXCd, ProdutoGravacao linha_pedido)
        {
            //só pode ter um e tem que ter um
            var dados_produto = (from p in Execucao.TabelasBanco.TprodutoLoja_Include_Tprodtuo_Tfabricante
                                 where p.Fabricante == linha_pedido.Pedido.Fabricante && p.Produto == linha_pedido.Pedido.Produto
                                 select p).First();

            var tpedidoitem = new TpedidoItem();
            InicializarCamposDefaultItem(tpedidoitem);

            //with linha_pedido
            tpedidoitem.Pedido = id_pedido_temp;
            tpedidoitem.Fabricante = linha_pedido.Pedido.Fabricante;
            tpedidoitem.Produto = linha_pedido.Pedido.Produto;
            tpedidoitem.Qtde = twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado;
            tpedidoitem.Desc_Dado = linha_pedido.Pedido.Desc_Dado;
            tpedidoitem.Preco_Venda = linha_pedido.Pedido.Preco_Venda;
            tpedidoitem.Preco_NF = linha_pedido.Pedido.Preco_NF;
            tpedidoitem.Preco_Fabricante = dados_produto.Tproduto.Preco_Fabricante;
            tpedidoitem.Vl_Custo2 = dados_produto.Tproduto.Vl_Custo2;
            tpedidoitem.Preco_Lista = linha_pedido.Pedido.Preco_Lista;
            tpedidoitem.Margem = dados_produto.Margem;
            tpedidoitem.Desc_Max = dados_produto.Desc_Max;
            tpedidoitem.Comissao = dados_produto.Comissao;
            tpedidoitem.Descricao = dados_produto.Tproduto.Descricao;
            tpedidoitem.Descricao_Html = dados_produto.Tproduto.Descricao_Html;
            tpedidoitem.Ean = dados_produto.Tproduto.Ean;
            tpedidoitem.Grupo = dados_produto.Tproduto.Grupo;
            tpedidoitem.Subgrupo = dados_produto.Tproduto.Subgrupo;
            tpedidoitem.Peso = dados_produto.Tproduto.Peso;
            tpedidoitem.Qtde_Volumes = dados_produto.Tproduto.Qtde_Volumes;
            //todo: grva60, gravar campos de autorização de desconto
            //tpedidoitem.Abaixo_Min_Status = linha_pedido.abaixo_min_status;
            //tpedidoitem.Abaixo_Min_Autorizacao = linha_pedido.abaixo_min_autorizacao; //nao gravar null
            //tpedidoitem.Abaixo_Min_Autorizador = linha_pedido.abaixo_min_autorizador; //nao gravar null
            //tpedidoitem.Abaixo_Min_Superv_Autorizador = linha_pedido.abaixo_min_superv_autorizador; //nao gravar null
            tpedidoitem.Sequencia = sequencia_item;
            tpedidoitem.Markup_Fabricante = dados_produto.Tfabricante.Markup;
            tpedidoitem.CustoFinancFornecCoeficiente = linha_pedido.Pedido.CustoFinancFornecCoeficiente_Conferencia;
            tpedidoitem.CustoFinancFornecPrecoListaBase = linha_pedido.Pedido.CustoFinancFornecPrecoListaBase_Conferencia;
            tpedidoitem.Cubagem = dados_produto.Tproduto.Cubagem;
            tpedidoitem.Ncm = dados_produto.Tproduto.Ncm;
            tpedidoitem.Cst = dados_produto.Tproduto.Cst;
            tpedidoitem.Descontinuado = dados_produto.Tproduto.Descontinuado;

            ContextoBdGravacao.Add(tpedidoitem);
        }

        private void CamposPedidoPaieFilhote(bool permite_RA_status, Execucao.Execucao.GravacaoDados.EmpresaAutoSplitDados vEmpresaAutoSplit_iv, Tpedido tpedido)
        {

            //'	CAMPOS ARMAZENADOS TANTO NO PEDIDO-PAI QUANTO NO PEDIDO-FILHOTE
            tpedido.Id_Cliente = Execucao.Id_cliente;
            tpedido.Midia = Pedido.Cliente.Midia;
            tpedido.Servicos = "";
            tpedido.Vendedor = Pedido.Ambiente.Vendedor;
            tpedido.Usuario_Cadastro = Pedido.Ambiente.Usuario;
            tpedido.St_Entrega = "";
            tpedido.Pedido_Bs_X_At = Pedido.Extra.Pedido_bs_x_at ?? "";

            if (!string.IsNullOrEmpty(Pedido.DetalhesPedido.EntregaImediata))
            {
                tpedido.St_Etg_Imediata = short.Parse(Pedido.DetalhesPedido.EntregaImediata);
                tpedido.Etg_Imediata_Data = Execucao.Gravacao.DataHoraCriacao;
                tpedido.Etg_Imediata_Usuario = Pedido.Ambiente.Usuario;
            }
            if (short.Parse(Pedido.DetalhesPedido.EntregaImediata) == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO)
            {
                tpedido.PrevisaoEntregaData = Pedido.DetalhesPedido.EntregaImediataData;
                tpedido.PrevisaoEntregaUsuarioUltAtualiz = Pedido.Ambiente.Usuario;
                tpedido.PrevisaoEntregaDtHrUltAtualiz = Execucao.Gravacao.DataHoraCriacao;
            }
            tpedido.StBemUsoConsumo = Pedido.DetalhesPedido.BemDeUso_Consumo;

            if (Pedido.DetalhesPedido.InstaladorInstala != (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO_DEFINIDO)
            {
                tpedido.InstaladorInstalaStatus = Pedido.DetalhesPedido.InstaladorInstala;
                tpedido.InstaladorInstalaUsuarioUltAtualiz = Pedido.Ambiente.Usuario;
                tpedido.InstaladorInstalaDtHrUltAtualiz = Execucao.Gravacao.DataHoraCriacao;
            }

            tpedido.Pedido_Bs_X_Ac = Pedido.Marketplace.Pedido_bs_x_ac;
            tpedido.Pedido_Bs_X_Marketplace = Pedido.Marketplace.Pedido_bs_x_marketplace;
            tpedido.Marketplace_codigo_origem = Pedido.Marketplace.Marketplace_codigo_origem;

            tpedido.Nfe_Texto_Constar = Pedido.Extra.Nfe_Texto_Constar ?? "";
            tpedido.Nfe_XPed = Pedido.Extra.Nfe_XPed ?? "";

            tpedido.Loja_Indicou = Pedido.Ambiente.Loja_indicou;
            tpedido.Comissao_Loja_Indicou = Criacao.Execucao.Comissao_loja_indicou;
            tpedido.Venda_Externa = Pedido.Ambiente.Venda_Externa ? (short)1 : (short)0;
            tpedido.Indicador = Pedido.Ambiente.Indicador;

            tpedido.GarantiaIndicadorStatus = byte.Parse(Pedido.DetalhesPedido.GarantiaIndicador);
            tpedido.GarantiaIndicadorUsuarioUltAtualiz = Pedido.Ambiente.Usuario;
            tpedido.GarantiaIndicadorDtHrUltAtualiz = Execucao.Gravacao.DataHoraCriacao;

            if (Pedido.EnderecoEntrega.OutroEndereco)
            {
                tpedido.St_End_Entrega = 1;
                //no ASP limita os tamanhos; aqui o tamanho é validado em uma etapa anterior
                tpedido.EndEtg_Endereco = Pedido.EnderecoEntrega.EndEtg_endereco;
                tpedido.EndEtg_Endereco_Numero = Pedido.EnderecoEntrega.EndEtg_endereco_numero;
                tpedido.EndEtg_Endereco_Complemento = Pedido.EnderecoEntrega.EndEtg_endereco_complemento;
                tpedido.EndEtg_Bairro = Pedido.EnderecoEntrega.EndEtg_bairro;
                tpedido.EndEtg_Cidade = Pedido.EnderecoEntrega.EndEtg_cidade;
                tpedido.EndEtg_UF = Pedido.EnderecoEntrega.EndEtg_uf;
                tpedido.EndEtg_Cep = Pedido.EnderecoEntrega.EndEtg_cep;
                tpedido.EndEtg_Cod_Justificativa = Pedido.EnderecoEntrega.EndEtg_cod_justificativa;
                tpedido.EndEtg_email = Pedido.EnderecoEntrega.EndEtg_email;
                tpedido.EndEtg_email_xml = Pedido.EnderecoEntrega.EndEtg_email_xml;
                tpedido.EndEtg_nome = Pedido.EnderecoEntrega.EndEtg_nome;
                tpedido.EndEtg_ddd_res = Pedido.EnderecoEntrega.EndEtg_ddd_res;
                tpedido.EndEtg_tel_res = Pedido.EnderecoEntrega.EndEtg_tel_res;
                tpedido.EndEtg_ddd_com = Pedido.EnderecoEntrega.EndEtg_ddd_com;
                tpedido.EndEtg_tel_com = Pedido.EnderecoEntrega.EndEtg_tel_com;
                tpedido.EndEtg_ramal_com = Pedido.EnderecoEntrega.EndEtg_ramal_com;
                tpedido.EndEtg_ddd_cel = Pedido.EnderecoEntrega.EndEtg_ddd_cel;
                tpedido.EndEtg_tel_cel = Pedido.EnderecoEntrega.EndEtg_tel_cel;
                tpedido.EndEtg_ddd_com_2 = Pedido.EnderecoEntrega.EndEtg_ddd_com_2;
                tpedido.EndEtg_tel_com_2 = Pedido.EnderecoEntrega.EndEtg_tel_com_2;
                tpedido.EndEtg_ramal_com_2 = Pedido.EnderecoEntrega.EndEtg_ramal_com_2;
                tpedido.EndEtg_tipo_pessoa = Pedido.EnderecoEntrega.EndEtg_tipo_pessoa;
                tpedido.EndEtg_cnpj_cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(Pedido.EnderecoEntrega.EndEtg_cnpj_cpf);
                tpedido.EndEtg_contribuinte_icms_status = Pedido.EnderecoEntrega.EndEtg_contribuinte_icms_status;
                tpedido.EndEtg_produtor_rural_status = Pedido.EnderecoEntrega.EndEtg_produtor_rural_status;
                tpedido.EndEtg_ie = Pedido.EnderecoEntrega.EndEtg_ie;
                tpedido.EndEtg_rg = Pedido.EnderecoEntrega.EndEtg_rg;
            }
            //'OBTENÇÃO DE TRANSPORTADORA QUE ATENDA AO CEP INFORMADO, SE HOUVER
            if (!string.IsNullOrEmpty(Execucao.Transportadora.Transportadora_Id))
            {
                tpedido.Transportadora_Id = Execucao.Transportadora.Transportadora_Id;
                tpedido.Transportadora_Data = Execucao.Gravacao.DataHoraCriacao;
                tpedido.Transportadora_Usuario = Pedido.Ambiente.Usuario;
                tpedido.Transportadora_Selecao_Auto_Status = Execucao.Transportadora.Transportadora_Selecao_Auto_status();
                tpedido.Transportadora_Selecao_Auto_Cep = Execucao.Transportadora.Transportadora_Selecao_Auto_Cep;
                tpedido.Transportadora_Selecao_Auto_Transportadora = Execucao.Transportadora.Transportadora_Selecao_Auto_Transportadora;
                tpedido.Transportadora_Selecao_Auto_Tipo_Endereco = Execucao.Transportadora.Transportadora_Selecao_Auto_Tipo_Endereco;
                tpedido.Transportadora_Selecao_Auto_Data_Hora = Execucao.Gravacao.DataHoraCriacao;
            }
            //'01/02/2018: os pedidos do Arclube usam o RA para incluir o valor do frete e, portanto, não devem ter deságio do RA
            if ((Pedido.Ambiente.Loja != Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                && !Execucao.BlnMagentoPedidoComIndicador)
            {
                tpedido.Perc_Desagio_RA_Liquida = Execucao.ParametroPercDesagioRALiquida;
            }


            /*
             * não temos esses campos na API do magento
             * são campos antigos e não serão mais alimentados
                if (operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO) And blnMagentoPedidoComIndicador then
                    rs("magento_installer_commission_value") = percCommissionValue
                    rs("magento_installer_commission_discount") = percCommissionDiscount
                    rs("magento_shipping_amount") = vlMagentoShippingAmount
                    end if
                    */


            tpedido.Permite_RA_Status = (short)(permite_RA_status ? 1 : 0);
            if (permite_RA_status)
                tpedido.Opcao_Possui_RA = Pedido.Valor.PedidoPossuiRa() ? "S" : "N";
            else
                tpedido.Opcao_Possui_RA = "-"; // ' Não se aplica


            tpedido.Endereco_memorizado_status = 1;

            //no ASP limita os tamanhos; aqui o tamanho é validado em uma etapa anterior
            tpedido.Endereco_logradouro = Pedido.EnderecoCadastralCliente.Endereco_logradouro;
            tpedido.Endereco_numero = Pedido.EnderecoCadastralCliente.Endereco_numero;
            tpedido.Endereco_complemento = Pedido.EnderecoCadastralCliente.Endereco_complemento;
            tpedido.Endereco_bairro = Pedido.EnderecoCadastralCliente.Endereco_bairro;
            tpedido.Endereco_cidade = Pedido.EnderecoCadastralCliente.Endereco_cidade;

            tpedido.Endereco_uf = Pedido.EnderecoCadastralCliente.Endereco_uf;
            tpedido.Endereco_cep = Pedido.EnderecoCadastralCliente.Endereco_cep;

            tpedido.St_memorizacao_completa_enderecos = 1;
            tpedido.Endereco_email = Pedido.EnderecoCadastralCliente.Endereco_email;
            tpedido.Endereco_email_xml = Pedido.EnderecoCadastralCliente.Endereco_email_xml;
            tpedido.Endereco_nome = Pedido.EnderecoCadastralCliente.Endereco_nome;
            tpedido.Endereco_ddd_res = Pedido.EnderecoCadastralCliente.Endereco_ddd_res;
            tpedido.Endereco_tel_res = Pedido.EnderecoCadastralCliente.Endereco_tel_res;
            tpedido.Endereco_ddd_com = Pedido.EnderecoCadastralCliente.Endereco_ddd_com;
            tpedido.Endereco_tel_com = Pedido.EnderecoCadastralCliente.Endereco_tel_com;
            tpedido.Endereco_ramal_com = Pedido.EnderecoCadastralCliente.Endereco_ramal_com;
            tpedido.Endereco_ddd_cel = Pedido.EnderecoCadastralCliente.Endereco_ddd_cel;
            tpedido.Endereco_tel_cel = Pedido.EnderecoCadastralCliente.Endereco_tel_cel;
            tpedido.Endereco_ddd_com_2 = Pedido.EnderecoCadastralCliente.Endereco_ddd_com_2;
            tpedido.Endereco_tel_com_2 = Pedido.EnderecoCadastralCliente.Endereco_tel_com_2;
            tpedido.Endereco_ramal_com_2 = Pedido.EnderecoCadastralCliente.Endereco_ramal_com_2;
            tpedido.Endereco_tipo_pessoa = Pedido.EnderecoCadastralCliente.Endereco_tipo_pessoa;
            tpedido.Endereco_cnpj_cpf = Pedido.EnderecoCadastralCliente.Endereco_cnpj_cpf;
            tpedido.Endereco_contribuinte_icms_status = Pedido.EnderecoCadastralCliente.Endereco_contribuinte_icms_status;
            tpedido.Endereco_produtor_rural_status = Pedido.EnderecoCadastralCliente.Endereco_produtor_rural_status;
            tpedido.Endereco_ie = Pedido.EnderecoCadastralCliente.Endereco_ie;
            tpedido.Endereco_rg = Pedido.EnderecoCadastralCliente.Endereco_rg;
            tpedido.Endereco_contato = Pedido.EnderecoCadastralCliente.Endereco_contato ?? "";

            if ((Pedido.Ambiente.Operacao_origem == Constantes.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO)
                || ((Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE) && !string.IsNullOrEmpty(Pedido.Marketplace.Pedido_bs_x_ac)))
            {
                tpedido.Plataforma_Origem_Pedido = (int)Constantes.Cod_plataforma_origem.COD_PLATAFORMA_ORIGEM_PEDIDO__MAGENTO;
            }
            else
            {
                tpedido.Plataforma_Origem_Pedido = (int)Constantes.Cod_plataforma_origem.COD_PLATAFORMA_ORIGEM_PEDIDO__ERP;
            }

            tpedido.Sistema_responsavel_atualizacao = (int)Pedido.Configuracao.SistemaResponsavelCadastro;
            tpedido.Sistema_responsavel_cadastro = (int)Pedido.Configuracao.SistemaResponsavelCadastro;

            tpedido.Id_Nfe_Emitente = (short)vEmpresaAutoSplit_iv.Id_nfe_emitente;
        }

        private void CamposPedidoFilhote(Tpedido tpedido)
        {
            //'	PEDIDO FILHOTE
            //'	==============
            tpedido.St_Auto_Split = 1;
            tpedido.Split_Status = 1;
            tpedido.Split_Data = Execucao.Gravacao.DataHoraCriacao.Date;
            tpedido.Split_Hora = Execucao.Gravacao.Hora_pedido;
            tpedido.Split_Usuario = Constantes.ID_USUARIO_SISTEMA;
            tpedido.St_Pagto = "";
            tpedido.Usuario_St_Pagto = "";
            tpedido.St_Recebido = "";
            tpedido.Obs_1 = "";
            tpedido.Obs_2 = "";
            tpedido.Qtde_Parcelas = 0;
            tpedido.Forma_Pagto = "";
        }

        private void CamposPedidoPai(Tpedido tpedido)
        {
            //'	PEDIDO BASE
            //'	===========
            if (Execucao.Gravacao.EmpresasAutoSplit.Count > 1)
                tpedido.St_Auto_Split = 1;
            tpedido.Split_Status = 0;   //nao estava no ASP, é o valor default do banco

            if ((tpedido.St_Pagto ?? "") != Constantes.ST_PAGTO_NAO_PAGO)
            {
                tpedido.Dt_St_Pagto = Execucao.Gravacao.DataHoraCriacao.Date;
                tpedido.Dt_Hr_St_Pagto = Execucao.Gravacao.DataHoraCriacao;
                tpedido.Usuario_St_Pagto = Pedido.Ambiente.Usuario;
            }
            tpedido.St_Pagto = Constantes.ST_PAGTO_NAO_PAGO;
            //st_recebido: este campo não está mais sendo usado
            tpedido.Obs_1 = Pedido.DetalhesPedido.Obter_obs_1() ?? "";
            tpedido.Obs_2 = Pedido.DetalhesPedido.Obter_obs_2() ?? "";

            //'	Forma de Pagamento (nova versão)
            tpedido.Tipo_Parcelamento = short.Parse(Pedido.FormaPagtoCriacao.Rb_forma_pagto);
            if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                tpedido.Av_Forma_Pagto = short.Parse(Pedido.FormaPagtoCriacao.Op_av_forma_pagto);
                tpedido.Qtde_Parcelas = 1;
            }
            else
            {
                if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                {
                    tpedido.Pu_Forma_Pagto = short.Parse(Pedido.FormaPagtoCriacao.Op_pu_forma_pagto);
                    tpedido.Pu_Valor = Pedido.FormaPagtoCriacao.C_pu_valor ?? 0;
                    tpedido.Pu_Vencto_Apos = (short)(Pedido.FormaPagtoCriacao.C_pu_vencto_apos ?? 0);
                    tpedido.Qtde_Parcelas = 1;
                }
                else
                {
                    if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                    {
                        tpedido.Pc_Qtde_Parcelas = (short)(Pedido.FormaPagtoCriacao.C_pc_qtde ?? 0);
                        tpedido.Pc_Valor_Parcela = Pedido.FormaPagtoCriacao.C_pc_valor ?? 0;
                        tpedido.Qtde_Parcelas = (short)(Pedido.FormaPagtoCriacao.C_pc_qtde ?? 0);
                    }
                    else
                    {
                        if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                        {
                            tpedido.Pc_Maquineta_Qtde_Parcelas = (short)(Pedido.FormaPagtoCriacao.C_pc_maquineta_qtde ?? 0);
                            tpedido.Pc_Maquineta_Valor_Parcela = Pedido.FormaPagtoCriacao.C_pc_maquineta_valor ?? 0;
                            tpedido.Qtde_Parcelas = (short)(Pedido.FormaPagtoCriacao.C_pc_maquineta_qtde ?? 0);
                        }
                        else
                        {
                            if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                            {
                                tpedido.Pce_Forma_Pagto_Entrada = short.Parse(Pedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto);
                                tpedido.Pce_Forma_Pagto_Prestacao = short.Parse(Pedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto);
                                tpedido.Pce_Entrada_Valor = Pedido.FormaPagtoCriacao.C_pce_entrada_valor ?? 0;
                                tpedido.Pce_Prestacao_Qtde = (short)(Pedido.FormaPagtoCriacao.C_pce_prestacao_qtde ?? 0);
                                tpedido.Pce_Prestacao_Valor = Pedido.FormaPagtoCriacao.C_pce_prestacao_valor ?? 0;
                                tpedido.Pce_Prestacao_Periodo = (short)(Pedido.FormaPagtoCriacao.C_pce_prestacao_periodo ?? 0);
                                //'	Entrada + Prestações
                                tpedido.Qtde_Parcelas = (short)((Pedido.FormaPagtoCriacao.C_pce_prestacao_qtde ?? 0) + 1);
                            }
                            else
                            {
                                if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                                {
                                    tpedido.Pse_Forma_Pagto_Prim_Prest = short.Parse(Pedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto);
                                    tpedido.Pse_Forma_Pagto_Demais_Prest = short.Parse(Pedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto);
                                    tpedido.Pse_Prim_Prest_Valor = Pedido.FormaPagtoCriacao.C_pse_prim_prest_valor ?? 0;
                                    tpedido.Pse_Prim_Prest_Apos = (short)(Pedido.FormaPagtoCriacao.C_pse_prim_prest_apos ?? 0);
                                    tpedido.Pse_Demais_Prest_Qtde = (short)(Pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde ?? 0);
                                    tpedido.Pse_Demais_Prest_Valor = Pedido.FormaPagtoCriacao.C_pse_demais_prest_valor ?? 0;
                                    tpedido.Pse_Demais_Prest_Periodo = (short)(Pedido.FormaPagtoCriacao.C_pse_demais_prest_periodo ?? 0);
                                    //'	1ª prestação + Demais prestações
                                    tpedido.Qtde_Parcelas = (short)((Pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde ?? 0) + 1);
                                }
                            }
                        }
                    }
                }
            }

            tpedido.Forma_Pagto = Pedido.FormaPagtoCriacao.C_forma_pagto;
            tpedido.Vl_Total_Familia = Pedido.Valor.Vl_total;
            var usuario_automatico = "AUTOMÁTICO";
            short cod_an_credito_ok = short.Parse(Constantes.COD_AN_CREDITO_OK);

            if (Execucao.BlnPedidoECommerceCreditoOkAutomatico)
            {
                tpedido.Analise_Credito = cod_an_credito_ok;
                tpedido.Analise_credito_Data = Execucao.Gravacao.DataHoraCriacao;
                tpedido.Analise_Credito_Usuario = usuario_automatico;
            }
            else
            {
                if (Pedido.Valor.Vl_total <= Execucao.Vl_aprov_auto_analise_credito)
                {
                    tpedido.Analise_Credito = cod_an_credito_ok;
                    tpedido.Analise_credito_Data = Execucao.Gravacao.DataHoraCriacao;
                    tpedido.Analise_Credito_Usuario = usuario_automatico;
                }
                else
                {
                    if ((Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_TRANSFERENCIA) || (Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_KITS) || Execucao.IsLojaGarantia)
                    {
                        //'Lojas usadas para pedidos de operações internas
                        tpedido.Analise_Credito = cod_an_credito_ok;
                        tpedido.Analise_credito_Data = Execucao.Gravacao.DataHoraCriacao;
                        tpedido.Analise_Credito_Usuario = usuario_automatico;
                    }
                    else
                    {
                        if ((Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE) && (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA) && (Pedido.FormaPagtoCriacao.Op_av_forma_pagto == Constantes.ID_FORMA_PAGTO_DINHEIRO))
                        {
                            tpedido.Analise_Credito = short.Parse(Constantes.COD_AN_CREDITO_PENDENTE_VENDAS);
                            tpedido.Analise_credito_Data = Execucao.Gravacao.DataHoraCriacao;
                            tpedido.Analise_Credito_Usuario = usuario_automatico;
                        }
                        else
                        {
                            if ((Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE) && (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA) && (Pedido.FormaPagtoCriacao.Op_av_forma_pagto == Constantes.ID_FORMA_PAGTO_BOLETO_AV))
                            {
                                tpedido.Analise_Credito = short.Parse(Constantes.COD_AN_CREDITO_PENDENTE_VENDAS);
                                tpedido.Analise_Credito_Pendente_Vendas_Motivo = "006";// 'Aguardando Emissão do Boleto Avulso
                                tpedido.Analise_credito_Data = Execucao.Gravacao.DataHoraCriacao;
                                tpedido.Analise_Credito_Usuario = usuario_automatico;
                            }
                            else
                            {
                                if ((Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA) && ((Pedido.FormaPagtoCriacao.Op_av_forma_pagto == Constantes.ID_FORMA_PAGTO_DEPOSITO) || (Pedido.FormaPagtoCriacao.Op_av_forma_pagto == Constantes.ID_FORMA_PAGTO_BOLETO_AV)))
                                {
                                    tpedido.Analise_Credito = short.Parse(Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO);
                                    tpedido.Analise_credito_Data = Execucao.Gravacao.DataHoraCriacao;
                                    tpedido.Analise_Credito_Usuario = usuario_automatico;
                                }
                                else
                                {
                                    if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                                    {
                                        tpedido.Analise_Credito = short.Parse(Constantes.COD_AN_CREDITO_PENDENTE_VENDAS);
                                        tpedido.Analise_credito_Data = Execucao.Gravacao.DataHoraCriacao;
                                        tpedido.Analise_Credito_Usuario = usuario_automatico;


                                    }
                                }
                            }
                        }
                    }
                }
            }

            /*
             * todo: perc_rt
            pergunta hamilton: o que é o perc_rt? Lemos da T_CODIGO_DESCRICAO, certo?
            usar o fluxo autal, procurar PedidoECommerce_Origem_Grupo. temos que ler conforme a origem e ler o valor percentual.
            ver PedidoNovoConfirma.asp '	OBTÉM O PERCENTUAL DE COMISSÃO DO MARKETPLACE
            */

            //'	CUSTO FINANCEIRO FORNECEDOR
            tpedido.CustoFinancFornecTipoParcelamento = Execucao.C_custoFinancFornecTipoParcelamento;
            tpedido.CustoFinancFornecQtdeParcelas = Execucao.C_custoFinancFornecQtdeParcelas;
            tpedido.Vl_Total_NF = Pedido.Valor.Vl_total_NF;
            tpedido.Vl_Total_RA = Pedido.Valor.Vl_total_RA;
            tpedido.Perc_RT = Pedido.Valor.Perc_RT;
            tpedido.Perc_Desagio_RA = Execucao.Perc_desagio_RA;
            tpedido.Perc_Limite_RA_Sem_Desagio = Execucao.Perc_limite_RA_sem_desagio;
        }

        //vamos inicializar os valores padrão
        //precisamos para os testes; em produção, não é necessário pq o banco já faz isso
        private void InicializarCamposDefault(Tpedido tpedido)
        {
            //algus campos não temos na nossa estrutura
            //tpedido.vl_servicos = 0;
            tpedido.Qtde_Parcelas = 0;
            tpedido.Vl_Total_Familia = 0;
            //tpedido.vl_pago_familia = 0;
            tpedido.Split_Status = 0;
            //tpedido.a_entregar_status = 0;
            tpedido.Comissao_Loja_Indicou = 0;
            tpedido.Venda_Externa = 0;
            tpedido.Vl_Frete = 0;
            tpedido.Analise_Credito = 0;
            tpedido.Tipo_Parcelamento = 0;
            tpedido.Av_Forma_Pagto = 0;
            tpedido.Pc_Qtde_Parcelas = 0;
            tpedido.Pc_Valor_Parcela = 0;
            tpedido.Pce_Forma_Pagto_Entrada = 0;
            tpedido.Pce_Forma_Pagto_Prestacao = 0;
            tpedido.Pce_Entrada_Valor = 0;
            tpedido.Pce_Prestacao_Qtde = 0;
            tpedido.Pce_Prestacao_Valor = 0;
            tpedido.Pce_Prestacao_Periodo = 0;
            tpedido.Pse_Forma_Pagto_Prim_Prest = 0;
            tpedido.Pse_Forma_Pagto_Demais_Prest = 0;
            tpedido.Pse_Prim_Prest_Valor = 0;
            tpedido.Pse_Prim_Prest_Apos = 0;
            tpedido.Pse_Demais_Prest_Qtde = 0;
            tpedido.Pse_Demais_Prest_Valor = 0;
            tpedido.Pse_Demais_Prest_Periodo = 0;
            tpedido.Pu_Forma_Pagto = 0;
            tpedido.Pu_Valor = 0;
            tpedido.Pu_Vencto_Apos = 0;
            tpedido.Vl_Total_NF = 0;
            tpedido.Vl_Total_RA = 0;
            tpedido.Perc_RT = 0;
            //tpedido.st_orc_virou_pedido = 0;
            //tpedido.comissao_paga = 0;
            tpedido.Perc_Desagio_RA = 0;
            tpedido.Perc_Limite_RA_Sem_Desagio = 0;
            tpedido.Vl_Total_RA_Liquido = 0;
            tpedido.St_Tem_Desagio_RA = 0;
            tpedido.Qtde_Parcelas_Desagio_RA = 0;
            tpedido.St_End_Entrega = 0;
            tpedido.St_Etg_Imediata = 0;
            //tpedido.frete_status = 0;
            tpedido.Frete_Valor = 0;
            tpedido.StBemUsoConsumo = 0;
            tpedido.PedidoRecebidoStatus = 0;
            tpedido.InstaladorInstalaStatus = 0;
            tpedido.CustoFinancFornecQtdeParcelas = 0;
            //tpedido.BoletoConfeccionadoStatus = 0;
            tpedido.GarantiaIndicadorStatus = 0;
            //tpedido.romaneio_status = 0;
            //tpedido.danfe_impressa_status = 0;
            tpedido.Perc_Desagio_RA_Liquida = 0;
            //tpedido.indicador_editado_manual_status = 0;
            tpedido.Permite_RA_Status = 0;
            //tpedido.st_violado_permite_RA_status = 0;
            tpedido.Endereco_memorizado_status = 0;
            tpedido.Analise_Endereco_Tratar_Status = 0;
            //tpedido.analise_endereco_tratado_status = 0;
            //tpedido.cancelado_auto_status = 0;
            //tpedido.danfe_a_imprimir_status = 0;
            tpedido.Transportadora_Selecao_Auto_Status = 0;
            tpedido.Transportadora_Selecao_Auto_Tipo_Endereco = 0;
            tpedido.Id_Nfe_Emitente = 0;
            //tpedido.st_pedido_novo_analise_credito_msg_alerta = 0;
            //tpedido.MarketplacePedidoRecebidoRegistrarStatus = 0;
            //tpedido.MarketplacePedidoRecebidoRegistradoStatus = 0;
            tpedido.St_Auto_Split = 0;
            tpedido.Plataforma_Origem_Pedido = 0;
            //tpedido.magento_installer_commission_value = 0;
            //tpedido.magento_installer_commission_discount = 0;
            //tpedido.magento_shipping_amount = 0;
            tpedido.Pc_Maquineta_Qtde_Parcelas = 0;
            tpedido.Pc_Maquineta_Valor_Parcela = 0;
            tpedido.Sistema_responsavel_cadastro = 0;
            tpedido.Sistema_responsavel_atualizacao = 0;
            tpedido.St_memorizacao_completa_enderecos = 0;
            tpedido.Endereco_contribuinte_icms_status = 0;
            tpedido.Endereco_produtor_rural_status = 0;
            tpedido.EndEtg_contribuinte_icms_status = 0;
            tpedido.EndEtg_produtor_rural_status = 0;
        }


        //vamos inicializar os valores padrão
        //precisamos para os testes; em produção, não é necessário pq o banco já faz isso
        private void InicializarCamposDefaultItem(TpedidoItem tpedidoitem)
        {
            //algus campos não temos na nossa estrutura
            tpedidoitem.Qtde = 0;
            tpedidoitem.Desc_Dado = 0;
            tpedidoitem.Preco_Venda = 0;
            tpedidoitem.Preco_Fabricante = 0;
            tpedidoitem.Preco_Lista = 0;
            tpedidoitem.Margem = 0;
            tpedidoitem.Desc_Max = 0;
            tpedidoitem.Comissao = 0;
            tpedidoitem.Peso = 0;
            tpedidoitem.Qtde_Volumes = 0;
            tpedidoitem.Abaixo_Min_Status = 0;
            tpedidoitem.Sequencia = 0;
            tpedidoitem.Markup_Fabricante = 0;
            tpedidoitem.Preco_NF = 0;
            tpedidoitem.Vl_Custo2 = 0;
            tpedidoitem.CustoFinancFornecCoeficiente = 0;
            tpedidoitem.CustoFinancFornecPrecoListaBase = 0;
            tpedidoitem.Cubagem = 0;
            //tpedidoitem.separacao_rel_nsu = 0;
            //tpedidoitem.separacao_deposito_zona_id = 0;
        }
    }
}
