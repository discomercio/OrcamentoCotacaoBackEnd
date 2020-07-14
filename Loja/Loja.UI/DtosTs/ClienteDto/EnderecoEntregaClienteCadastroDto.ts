export class EnderecoEntregaClienteCadastroDto {
    EndEtg_endereco: string;
    EndEtg_endereco_numero: string;
    EndEtg_endereco_complemento: string;
    EndEtg_bairro: string;
    EndEtg_cidade: string;
    EndEtg_uf: string;
    EndEtg_cep: string;
    //codigo da justificativa, preenchdio quando está criando (do spa para a api)
    EndEtg_cod_justificativa: string;
    //descrição da justificativa, preenchdio para mostrar (da api para o spa)
    EndEtg_descricao_justificativa: string;
    //se foi selecionado um endereco diferente para a entrega (do spa para a api)
    OutroEndereco: boolean;
}