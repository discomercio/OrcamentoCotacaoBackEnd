@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: Especificacao\Pedido\Passo60\Gravacao\Passo90\Log

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Reiniciar banco ao terminar cenário
	Given Pedido base

#loja/PedidoNovoConfirma.asp
#Monta o log do pedido (variável s_log)
#Monta o log dos itens do pedido
#Monta o log do auto-slipt (incluindo log dos filhotes)
Scenario: Log - pedido magento sem indicador
	#Vamos validar o pedido magento
	When Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123457092"
	Then Sem nenhum erro
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "vl total=3.132,90; ra=""; indicador=""; vl_total_nf=3.132,90; vl_total_ra=0,00; perc_rt=0; qtde_parcelas=1; st_etg_imediata=2; stbemusoconsumo=1; instaladorinstalastatus=1; obs_1=teste magento; pedido_bs_x_ac = 123457092; marketplace_codigo_origem = 001; status da análise crédito: 8 - pendente vendas; tipo_parcelamento=1; av_forma_pagto=6; custofinancfornectipoparcelamento=av; custofinancfornecqtdeparcelas=0; endereço cobrança=rua professor fábio fanucchi, 97 - jardim são paulo(zona norte) - são paulo - sp - 0204-080 (email=testecad@gabriel.com, email_xml=, nome=vivian, ddd_res=11, tel_res=11111111, ddd_com=11, tel_com=12345678, ramal_com=, ddd_cel=11, tel_cel=981603313, ddd_com_2=, tel_com_2=, ramal_com_2=, tipo_pessoa=pf, cnpj_cpf=14039603052, contribuinte_icms_status=0, produtor_rural_status=1, ie=, rg=, contato=); endereço entrega=mesmo do cadastro; escolha automática de transportadora=n; garantiaindicadorstatus=0; perc_desagio_ra_liquida=0; pedido_bs_x_at=; cod_origem_pedido=001; operação de origem: cadastramento semi-automático de pedido do e-commerce (nº magento pedido_bs_x_ac=123457092;\r 2x003220(003); preco_lista=626,58; desc_dado=0; preco_venda=626,58; preco_nf=626,58; custofinancforneccoeficiente=1; custofinancfornecprecolistabase=626,58; estoque_vendido=2;\r 2x003221(003); preco_lista=939,87; desc_dado=0; preco_venda=939,87; preco_nf=939,87; custofinancforneccoeficiente=1; custofinancfornecprecolistabase=939,87; estoque_vendido=2"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "opcao_possui_ra" = "-"

Scenario: Log - pedido magento com indicador
	When Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	When Informo "Frete" = "377.34"
	When Lista de itens "0" informo "Preco_NF" = "704.05"
	When Lista de itens "1" informo "Preco_NF" = "862.4"
	#mesmo que a gente distribua de uma forma, ao salvar o pedido no magento ele sempre lança o desconto porporcional em todos os produtos
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "vl total=3.132,90; ra=S; indicador=frete; vl_total_nf=3.510,24; vl_total_ra=377,34; perc_rt=0; qtde_parcelas=1; st_etg_imediata=2; stbemusoconsumo=1; instaladorinstalastatus=1; obs_1=teste magento; pedido_bs_x_ac = 123456789; marketplace_codigo_origem = 001; status da análise crédito: 8 - pendente vendas; tipo_parcelamento=1; av_forma_pagto=6; custofinancfornectipoparcelamento=av; custofinancfornecqtdeparcelas=0; endereço cobrança=rua professor fábio fanucchi, 97 - jardim são paulo(zona norte) - são paulo - sp - 0204-080 (email=testecad@gabriel.com, email_xml=, nome=vivian, ddd_res=11, tel_res=11111111, ddd_com=11, tel_com=12345678, ramal_com=, ddd_cel=11, tel_cel=981603313, ddd_com_2=, tel_com_2=, ramal_com_2=, tipo_pessoa=pf, cnpj_cpf=14039603052, contribuinte_icms_status=0, produtor_rural_status=1, ie=, rg=, contato=); endereço entrega=mesmo do cadastro; escolha automática de transportadora=n; garantiaindicadorstatus=0; perc_desagio_ra_liquida=0; pedido_bs_x_at=; cod_origem_pedido=001; operação de origem: cadastramento semi-automático de pedido do e-commerce (nº magento pedido_bs_x_ac=123456789;"
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "2x003220(003); preco_lista=626,58; desc_dado=0; preco_venda=626,58; preco_nf=702,05; custofinancforneccoeficiente=1; custofinancfornecprecolistabase=626,58; estoque_vendido=2;\r 2x003221(003); preco_lista=939,87; desc_dado=0; preco_venda=939,87; preco_nf=1.053,07; custofinancforneccoeficiente=1; custofinancfornecprecolistabase=939,87; estoque_vendido=2"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "opcao_possui_ra" = "S"

