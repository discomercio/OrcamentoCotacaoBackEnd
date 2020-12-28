Feature: EnderecoEntregaListaCampos

Scenario: Lista de campos exigidos
Aqui é somente para ter um resumo, o teste efetivo está sendo feito nos outros arquivos

Exigidos sempre:
    bool OutroEndereco 

Exigidos sempre se for OutroEndereco:
    string EndEtg_endereco 
    string EndEtg_endereco_numero 
    string EndEtg_endereco_complemento 
    string EndEtg_bairro 
    string EndEtg_cidade 
    string EndEtg_uf 
    string EndEtg_cep 
    string EndEtg_cod_justificativa 

Todos os outros são proibidos para clientes PF.

Exigidos somente para clientes PJ:
    string EndEtg_tipo_pessoa 
    string EndEtg_cnpj_cpf 
    string EndEtg_nome 
    byte EndEtg_contribuinte_icms_status 
    byte EndEtg_produtor_rural_status 
    string EndEtg_ie 

Opcionais para clientes PJ:
    string EndEtg_rg 
    string EndEtg_email 
    string EndEtg_email_xml 

Opcionais para clientes PJ entrega PJ, proibido para clientes PJ entrega PF:
    string EndEtg_ddd_com 
    string EndEtg_tel_com 
    string EndEtg_ramal_com 
    string EndEtg_ddd_com_2 
    string EndEtg_tel_com_2 
    string EndEtg_ramal_com_2 

Opcionais para clientes PJ entrega PF, proibido para clientes PJ entrega PJ:
    string EndEtg_ddd_res 
    string EndEtg_tel_res 
    string EndEtg_ddd_cel 
    string EndEtg_tel_cel 

