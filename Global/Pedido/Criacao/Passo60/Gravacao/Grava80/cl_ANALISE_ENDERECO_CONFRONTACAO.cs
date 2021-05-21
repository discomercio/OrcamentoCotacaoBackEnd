using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Criacao.Passo60.Gravacao.Grava80
{
    public class Cl_ANALISE_ENDERECO_CONFRONTACAO
    {
        public Cl_ANALISE_ENDERECO_CONFRONTACAO()
        {
            //temos um construtor sem parâmetors porque ficou bem mais claro no Grava80 porque não podemos usar parâmetros nomeados dentro do select
            //e por isso as propriedads tem get e set
        }

        public Cl_ANALISE_ENDERECO_CONFRONTACAO(string? pedido, string? id_cliente,
            string? tipo_endereco, string? endereco_logradouro, string? endereco_bairro,
            string? endereco_cidade, string? endereco_uf, string? endereco_cep,
            string? endereco_numero, string? endereco_complemento)
        {
            Pedido = pedido;
            Id_cliente = id_cliente;
            Tipo_endereco = tipo_endereco;
            Endereco_logradouro = endereco_logradouro;
            Endereco_bairro = endereco_bairro;
            Endereco_cidade = endereco_cidade;
            Endereco_uf = endereco_uf;
            Endereco_cep = endereco_cep;
            Endereco_numero = endereco_numero;
            Endereco_complemento = endereco_complemento;
        }


        //temos um construtor sem parâmetors porque ficou bem mais claro no Grava80 porque não podemos usar parâmetros nomeados dentro do select
        //e por isso as propriedads tem get e set
        public string? Pedido { get; set; }
        public string? Id_cliente { get; set; }
        public string? Tipo_endereco { get; set; }
        public string? Endereco_logradouro { get; set; }
        public string? Endereco_bairro { get; set; }
        public string? Endereco_cidade { get; set; }
        public string? Endereco_uf { get; set; }
        public string? Endereco_cep { get; set; }
        public string? Endereco_numero { get; set; }
        public string? Endereco_complemento { get; set; }
        public DateTime? Data_hora { get; set; }

    }
}
