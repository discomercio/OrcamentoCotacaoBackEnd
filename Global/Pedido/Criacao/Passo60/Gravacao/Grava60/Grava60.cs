using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava60
{
    class Grava60 : PassoBaseGravacao
    {
        public Grava60(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao)
        {
        }

        public async Task Executar()
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


            //s_hora_pedido = retorna_so_digitos(formata_hora(Now))
            Execucao.Gravacao.Hora_pedido = UtilsGlobais.Util.HoraParaBanco(Execucao.Gravacao.DataHoraCriacao);
            int indice_pedido = 0;
            foreach (var vEmpresaAutoSplit_iv in Execucao.Gravacao.EmpresasAutoSplit)
            {
                //o primeiro é 1, inicializamos com 0
                indice_pedido += 1;
                Tpedido tpedido = new Tpedido();

                //'	Controla a quantidade de pedidos no auto-split
                //'	pedido-base: indice_pedido=1
                //'	pedido-filhote 'A' => indice_pedido=2
                //'	pedido-filhote 'B' => indice_pedido=3
                //'	etc
                string id_pedido = Execucao.Gravacao.Id_pedido_base;
                if (indice_pedido != 1)
                    id_pedido = Execucao.Gravacao.Id_pedido_base +
                        InfraBanco.Constantes.Constantes.COD_SEPARADOR_FILHOTE +
                        Gera_num_pedido.Gera_letra_pedido_filhote(indice_pedido - 1);
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

                //'	CAMPOS ARMAZENADOS TANTO NO PEDIDO-PAI QUANTO NO PEDIDO-FILHOTE
                tpedido.Id_Cliente = Pedido.Cliente.Id_cliente;
                tpedido.Midia = Pedido.Cliente.Midia;
                tpedido.Servicos = "";
                tpedido.Vendedor = Pedido.Ambiente.Vendedor;
                tpedido.Usuario_Cadastro = Pedido.Ambiente.Usuario;
                tpedido.St_Entrega = "";
                tpedido.Pedido_Bs_X_At = Pedido.Extra.Pedido_bs_x_at;

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

                tpedido.Nfe_Texto_Constar = Pedido.Extra.Nfe_Texto_Constar;
                tpedido.Nfe_XPed = Pedido.Extra.Nfe_XPed;

                //todo: terminar de tirar
                /*
						id_pedido_temp_base = Execucao.Gravacao.Id_pedido_base ---não temos o temporario
						s_hora_pedido = Execucao.Gravacao.Hora_pedido 

					dim id_pedido, id_pedido_base, id_pedido_temp, id_pedido_temp_base, indice_pedido, indice_item, sequencia_item, s_hora_pedido, s_log, s_log_cliente_indicador, vLogAutoSplit, s_log_item_autosplit
						id_pedido_base = ""
						id_pedido_temp_base = ""
						s_log=""
						s_log_cliente_indicador=""
						redim vLogAutoSplit(0)
						vLogAutoSplit(UBound(vLogAutoSplit)) = ""
					'	~~~~~~~~~~~~~
						cn.BeginTrans
					'	~~~~~~~~~~~~~

								*/

                //todo:terminar grava60

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

                /*
                //'OBTENÇÃO DE TRANSPORTADORA QUE ATENDA AO CEP INFORMADO, SE HOUVER
                    if sTranspSelAutoTransportadoraId <> "" then
                        tpedido.Transportadora_Id") = sTranspSelAutoTransportadoraId
                        tpedido.Transportadora_Data = Execucao.Gravacao.DataHoraCriacao;
                tpedido.Transportadora_Usuario = Pedido.Ambiente.Usuario;
                tpedido.Transportadora_Selecao_Auto_Status") = iTranspSelAutoStatus
                        tpedido.Transportadora_Selecao_Auto_Cep") = sTranspSelAutoCep
                        tpedido.Transportadora_Selecao_Auto_Transportadora") = sTranspSelAutoTransportadoraId
                        tpedido.Transportadora_Selecao_Auto_Tipo_Endereco") = iTranspSelAutoTipoEndereco
                        tpedido.Transportadora_Selecao_Auto_Data_Hora = Execucao.Gravacao.DataHoraCriacao;
                end if
                */
                //todo: temrinar campos
                    /*
                        '01/02/2018: os pedidos do Arclube usam o RA para incluir o valor do frete e, portanto, não devem ter deságio do RA
                        if (Cstr(loja) <> Cstr(NUMERO_LOJA_ECOMMERCE_AR_CLUBE)) And (Not blnMagentoPedidoComIndicador) then rs("perc_desagio_RA_liquida") = getParametroPercDesagioRALiquida

                        if (operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO) And blnMagentoPedidoComIndicador then
                            rs("magento_installer_commission_value") = percCommissionValue
                            rs("magento_installer_commission_discount") = percCommissionDiscount
                            rs("magento_shipping_amount") = vlMagentoShippingAmount
                            end if

                        rs("permite_RA_status") = permite_RA_status

                        if permite_RA_status = 1 then
                            rs("opcao_possui_RA") = rb_RA
                        else
                            rs("opcao_possui_RA") = "-" ' Não se aplica
                            end if

                        rs("endereco_memorizado_status") = 1

                        if (c_FlagCadSemiAutoPedMagento_FluxoOtimizado = "1") Or (c_FlagCadSemiAutoPedMagento_FluxoOtimizado = "9") then
                            rs("endereco_logradouro") = Left(EndCob_endereco, MAX_TAMANHO_CAMPO_ENDERECO)
                            rs("endereco_numero") = Left(EndCob_endereco_numero, MAX_TAMANHO_CAMPO_ENDERECO_NUMERO)
                            rs("endereco_complemento") = Left(EndCob_endereco_complemento, MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO)
                            rs("endereco_bairro") = Left(EndCob_bairro, MAX_TAMANHO_CAMPO_ENDERECO_BAIRRO)
                            rs("endereco_cidade") = Left(EndCob_cidade, MAX_TAMANHO_CAMPO_ENDERECO_CIDADE)
                        else
                            rs("endereco_logradouro") = EndCob_endereco
                            rs("endereco_numero") = EndCob_endereco_numero
                            rs("endereco_complemento") = EndCob_endereco_complemento
                            rs("endereco_bairro") = EndCob_bairro
                            rs("endereco_cidade") = EndCob_cidade
                            end if

                        rs("endereco_uf") = EndCob_uf
                        rs("endereco_cep") = EndCob_cep

                        if blnUsarMemorizacaoCompletaEnderecos then
                            rs("st_memorizacao_completa_enderecos") = 1
                            rs("endereco_email") = EndCob_email
                            rs("endereco_email_xml") = EndCob_email_xml
                            rs("endereco_nome") = EndCob_nome
                            rs("endereco_ddd_res") = EndCob_ddd_res
                            rs("endereco_tel_res") = EndCob_tel_res
                            rs("endereco_ddd_com") = EndCob_ddd_com
                            rs("endereco_tel_com") = EndCob_tel_com
                            rs("endereco_ramal_com") = EndCob_ramal_com
                            rs("endereco_ddd_cel") = EndCob_ddd_cel
                            rs("endereco_tel_cel") = EndCob_tel_cel
                            rs("endereco_ddd_com_2") = EndCob_ddd_com_2
                            rs("endereco_tel_com_2") = EndCob_tel_com_2
                            rs("endereco_ramal_com_2") = EndCob_ramal_com_2
                            rs("endereco_tipo_pessoa") = EndCob_tipo_pessoa
                            rs("endereco_cnpj_cpf") = EndCob_cnpj_cpf
                            rs("endereco_contribuinte_icms_status") = converte_numero(EndCob_contribuinte_icms_status)
                            rs("endereco_produtor_rural_status") = converte_numero(EndCob_produtor_rural_status)
                            rs("endereco_ie") = EndCob_ie
                            rs("endereco_rg") = EndCob_rg
                            rs("endereco_contato") = EndCob_contato
                            end if

                        if (operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO) OR ( (Cstr(loja) = Cstr(NUMERO_LOJA_ECOMMERCE_AR_CLUBE)) And (Trim(s_pedido_ac) <> "") ) then
                            rs("plataforma_origem_pedido") = COD_PLATAFORMA_ORIGEM_PEDIDO__MAGENTO
                        else
                            rs("plataforma_origem_pedido") = COD_PLATAFORMA_ORIGEM_PEDIDO__ERP
                            end if

                        tpedido.Sistema_responsavel_atualizacao = (int)Pedido.Configuracao.SistemaResponsavelCadastro;
                        tpedido.Sistema_responsavel_cadastro = (int)Pedido.Configuracao.SistemaResponsavelCadastro;

                        rs("id_nfe_emitente") = vEmpresaAutoSplit_iv.Id_nfe_emitente

                        rs.Update
                        if Err <> 0 then
                        '	~~~~~~~~~~~~~~~~
                            cn.RollbackTrans
                        '	~~~~~~~~~~~~~~~~
                            Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)
                            end if

                        if rs.State <> 0 then rs.Close

                        sequencia_item = 0
                        total_estoque_vendido=0
                        total_estoque_sem_presenca=0
                        s_log_item_autosplit = ""
                        for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
                            if Trim(vProdRegra(iRegra).produto) <> "" then
                                for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
                                    if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = vEmpresaAutoSplit_iv.Id_nfe_emitente) And (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada > 0) then
                                    '	LOCALIZA O PRODUTO EM V_ITEM
                                        indice_item = -1
                                        for j=LBound(v_item) to UBound(v_item)
                                            if (Trim("" & v_item(j).fabricante) = vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.fabricante) And _
                                                (Trim("" & v_item(j).produto) = vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.produto) then
                                                indice_item = j
                                                exit for
                                                end if
                                            next

                                        if indice_item > -1 then
                                            sequencia_item = sequencia_item + 1
                                            with v_item(indice_item)
                                                s="SELECT * FROM t_PEDIDO_ITEM WHERE pedido='X'"
                                                rs.Open s, cn
                                                rs.AddNew
                                                rs("pedido") = id_pedido_temp
                                                rs("fabricante") = .fabricante
                                                rs("produto") = .produto
                                                rs("qtde") = vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada
                                                rs("desc_dado") = .desc_dado
                                                rs("preco_venda") = .preco_venda
                                                rs("preco_NF") = .preco_NF
                                                rs("preco_fabricante") = .preco_fabricante
                                                rs("vl_custo2") = .vl_custo2
                                                rs("preco_lista") = .preco_lista
                                                rs("margem") = .margem
                                                rs("desc_max") = .desc_max
                                                rs("comissao") = .comissao
                                                rs("descricao") = .descricao
                                                rs("descricao_html") = .descricao_html
                                                rs("ean") = .ean
                                                rs("grupo") = .grupo
                                                rs("subgrupo") = .subgrupo
                                                rs("peso") = .peso
                                                rs("qtde_volumes") = .qtde_volumes
                                                rs("abaixo_min_status") = .abaixo_min_status
                                                rs("abaixo_min_autorizacao") = .abaixo_min_autorizacao
                                                rs("abaixo_min_autorizador") = .abaixo_min_autorizador
                                                rs("abaixo_min_superv_autorizador") = .abaixo_min_superv_autorizador
                                                rs("sequencia") = sequencia_item
                                                rs("markup_fabricante") = .markup_fabricante
                                                rs("custoFinancFornecCoeficiente") = .custoFinancFornecCoeficiente
                                                rs("custoFinancFornecPrecoListaBase") = .custoFinancFornecPrecoListaBase
                                                rs("cubagem") = .cubagem
                                                rs("ncm") = .ncm
                                                rs("cst") = .cst
                                                rs("descontinuado") = .descontinuado
                                                rs.Update
                                                if Err <> 0 then
                                                '	~~~~~~~~~~~~~~~~
                                                    cn.RollbackTrans
                                                '	~~~~~~~~~~~~~~~~
                                                    Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)
                                                    end if
                                                if rs.State <> 0 then rs.Close

                                                if vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada > vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque then
                                                    qtde_spe = vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada - vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque
                                                else
                                                    qtde_spe = 0
                                                    end if

                                                if Not ESTOQUE_produto_saida_v2(usuario, id_pedido_temp, vEmpresaAutoSplit_iv.Id_nfe_emitente, .fabricante, .produto, vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada, qtde_spe, qtde_estoque_vendido_aux, qtde_estoque_sem_presenca_aux, msg_erro) then
                                                '	~~~~~~~~~~~~~~~~
                                                    cn.RollbackTrans
                                                '	~~~~~~~~~~~~~~~~
                                                    Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_MOVIMENTO_ESTOQUE)
                                                    end if

                                                .qtde_estoque_vendido = .qtde_estoque_vendido + qtde_estoque_vendido_aux
                                                .qtde_estoque_sem_presenca = .qtde_estoque_sem_presenca + qtde_estoque_sem_presenca_aux

                                                total_estoque_vendido = total_estoque_vendido + qtde_estoque_vendido_aux
                                                total_estoque_sem_presenca = total_estoque_sem_presenca + qtde_estoque_sem_presenca_aux

                                            '	LOG
                                                if s_log_item_autosplit <> "" then s_log_item_autosplit = s_log_item_autosplit & chr(13)
                                                s_log_item_autosplit = s_log_item_autosplit & "(" & .fabricante & ")" & .produto & ":" & _
                                                            " Qtde Solicitada = " & vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada & "," & _
                                                            " Qtde Sem Presença Autorizada = " & Cstr(qtde_spe) & "," & _
                                                            " Qtde Estoque Vendido = " & Cstr(qtde_estoque_vendido_aux) & "," & _
                                                            " Qtde Sem Presença = " & Cstr(qtde_estoque_sem_presenca_aux)
                                                end with
                                            end if 'if indice_item > -1
                                        end if 'if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = vEmpresaAutoSplit_iv.Id_nfe_emitente) And (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada > 0)
                                    next 'for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
                                end if 'if Trim(vProdRegra(iRegra).produto) <> ""
                            next 'for iRegra=LBound(vProdRegra) to UBound(vProdRegra)

                        if indice_pedido = 1 then
                            if Not gera_num_pedido(id_pedido_base, msg_erro) then
                            '	~~~~~~~~~~~~~~~~
                                cn.RollbackTrans
                            '	~~~~~~~~~~~~~~~~
                                Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_GERAR_NSU)
                                end if
                            id_pedido = id_pedido_base
                        else
                            id_pedido = id_pedido_base & COD_SEPARADOR_FILHOTE & gera_letra_pedido_filhote(indice_pedido-1)
                            end if

                    '	LOG
                        if Trim("" & vLogAutoSplit(UBound(vLogAutoSplit))) <> "" then redim preserve vLogAutoSplit(UBound(vLogAutoSplit)+1)
                        vLogAutoSplit(UBound(vLogAutoSplit)) = id_pedido & " (" & obtem_apelido_empresa_NFe_emitente(vEmpresaAutoSplit_iv.Id_nfe_emitente) & ")" & chr(13) & _
                                                                s_log_item_autosplit

                '		STATUS DE ENTREGA
                        if total_estoque_vendido = 0 then
                            s = ST_ENTREGA_ESPERAR
                        elseif total_estoque_sem_presenca = 0 then
                            s = ST_ENTREGA_SEPARAR
                        else
                            s = ST_ENTREGA_SPLIT_POSSIVEL
                            end if

                        s = "UPDATE t_PEDIDO SET st_entrega='" & s & "' WHERE pedido='" & id_pedido & "'"
                        cn.Execute(s)
        */
            }
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
            tpedido.Split_Status = 0;   //naop estava no ASP, é o valor default do banco

            if ((tpedido.St_Pagto ?? "") != Constantes.ST_PAGTO_NAO_PAGO)
            {
                tpedido.Dt_St_Pagto = Execucao.Gravacao.DataHoraCriacao.Date;
                tpedido.Dt_Hr_St_Pagto = Execucao.Gravacao.DataHoraCriacao;
                tpedido.Usuario_St_Pagto = Pedido.Ambiente.Usuario;
            }
            tpedido.St_Pagto = Constantes.ST_PAGTO_NAO_PAGO;
            //st_recebido: este campo não está mais sendo usado!!
            tpedido.Obs_1 = Pedido.DetalhesPedido.Obter_obs_1();
            tpedido.Obs_2 = Pedido.DetalhesPedido.Obter_obs_2();

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
                    tpedido.Pu_Valor = Pedido.FormaPagtoCriacao.C_pu_valor;
                    tpedido.Pu_Vencto_Apos = (short)(Pedido.FormaPagtoCriacao.C_pu_vencto_apos ?? 0);
                    tpedido.Qtde_Parcelas = 1;
                }
                else
                {
                    if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                    {
                        tpedido.Pc_Qtde_Parcelas = (short)(Pedido.FormaPagtoCriacao.C_pc_qtde ?? 0);
                        tpedido.Pc_Valor_Parcela = Pedido.FormaPagtoCriacao.C_pc_valor;
                        tpedido.Qtde_Parcelas = (short)(Pedido.FormaPagtoCriacao.C_pc_qtde ?? 0);
                    }
                    else
                    {
                        if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                        {
                            tpedido.Pc_Maquineta_Qtde_Parcelas = (short)(Pedido.FormaPagtoCriacao.C_pc_maquineta_qtde ?? 0);
                            tpedido.Pc_Maquineta_Valor_Parcela = (short)(Pedido.FormaPagtoCriacao.C_pc_maquineta_valor ?? 0);
                            tpedido.Qtde_Parcelas = (short)(Pedido.FormaPagtoCriacao.C_pc_maquineta_qtde ?? 0);
                        }
                        else
                        {
                            if (Pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                            {
                                tpedido.Pce_Forma_Pagto_Entrada = short.Parse(Pedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto);
                                tpedido.Pce_Forma_Pagto_Prestacao = short.Parse(Pedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto);
                                tpedido.Pce_Entrada_Valor = Pedido.FormaPagtoCriacao.C_pce_entrada_valor;
                                tpedido.Pce_Prestacao_Qtde = (short)(Pedido.FormaPagtoCriacao.C_pce_prestacao_qtde ?? 0);
                                tpedido.Pce_Prestacao_Valor = Pedido.FormaPagtoCriacao.C_pce_prestacao_valor;
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
                                    tpedido.Pse_Prim_Prest_Valor = Pedido.FormaPagtoCriacao.C_pse_prim_prest_valor;
                                    tpedido.Pse_Prim_Prest_Apos = (short)(Pedido.FormaPagtoCriacao.C_pse_prim_prest_apos ?? 0);
                                    tpedido.Pse_Demais_Prest_Qtde = (short)(Pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde ?? 0);
                                    tpedido.Pse_Demais_Prest_Valor = Pedido.FormaPagtoCriacao.C_pse_demais_prest_valor;
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
                    if ((Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_TRANSFERENCIA) || (Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_KITS) || Execucao.isLojaGarantia)
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

            //'	CUSTO FINANCEIRO FORNECEDOR
            tpedido.CustoFinancFornecTipoParcelamento = Execucao.C_custoFinancFornecTipoParcelamento;
            tpedido.CustoFinancFornecQtdeParcelas = Execucao.C_custoFinancFornecQtdeParcelas;
            tpedido.Vl_Total_NF = Pedido.Valor.Vl_total_NF;
            tpedido.Vl_Total_RA = Pedido.Valor.Vl_total_RA;
            tpedido.Perc_RT = Pedido.Valor.Perc_RT;
            tpedido.Perc_Desagio_RA = Execucao.Perc_desagio_RA;
            tpedido.Perc_Limite_RA_Sem_Desagio = Execucao.Perc_limite_RA_sem_desagio;
        }
    }
}
