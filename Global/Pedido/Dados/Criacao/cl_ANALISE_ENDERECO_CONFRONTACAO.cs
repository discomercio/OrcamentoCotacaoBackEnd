using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class Cl_ANALISE_ENDERECO_CONFRONTACAO
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public Cl_ANALISE_ENDERECO_CONFRONTACAO(TpedidoEnderecoConfrontacaoDados tPedidoEndConfrontacao)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            Construir(
                pedido: tPedidoEndConfrontacao.Pedido.Pedido,
                id_cliente: tPedidoEndConfrontacao.Pedido.Id_Cliente,
                tipo_endereco: tPedidoEndConfrontacao.TipoEndreco,
                endereco_logradouro: tPedidoEndConfrontacao.Pedido.Endereco_logradouro,
                endereco_bairro: tPedidoEndConfrontacao.Pedido.Endereco_bairro,
                endereco_cidade: tPedidoEndConfrontacao.Pedido.Endereco_cidade,
                endereco_uf: tPedidoEndConfrontacao.Pedido.Endereco_uf,
                endereco_cep: tPedidoEndConfrontacao.Pedido.Endereco_cep,
                endereco_numero: tPedidoEndConfrontacao.Pedido.Endereco_numero,
                endereco_complemento: tPedidoEndConfrontacao.Pedido.Endereco_complemento
            );
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public Cl_ANALISE_ENDERECO_CONFRONTACAO(string pedido, string id_cliente,
            string tipo_endereco, string endereco_logradouro, string endereco_bairro,
            string endereco_cidade, string endereco_uf, string endereco_cep,
            string endereco_numero, string endereco_complemento)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            Construir(pedido, id_cliente, tipo_endereco, endereco_logradouro, endereco_bairro, endereco_cidade, endereco_uf, endereco_cep, endereco_numero, endereco_complemento);
        }

        private void Construir(string pedido, string id_cliente, string tipo_endereco, string endereco_logradouro, string endereco_bairro, string endereco_cidade, string endereco_uf, string endereco_cep, string endereco_numero, string endereco_complemento)
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

        public string Pedido { get; private set; }
        public string Id_cliente { get; private set; }
        public string Tipo_endereco { get; private set; }
        public string Endereco_logradouro { get; private set; }
        public string Endereco_bairro { get; private set; }
        public string Endereco_cidade { get; private set; }
        public string Endereco_uf { get; private set; }
        public string Endereco_cep { get; private set; }
        public string Endereco_numero { get; private set; }
        public string Endereco_complemento { get; private set; }

    }
}
