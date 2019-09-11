export class EnderecoEntregaDtoClienteCadastro {
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
    OutroEndereco:boolean;
}
