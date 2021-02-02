using InfraBanco.Constantes;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Execucao
{
    public class Transportadora
    {
        private Transportadora(
            string? transportadora_Id,
            bool transportadora_Selecao_Auto,
            string? transportadora_Selecao_Auto_Cep,
            string? transportadora_Selecao_Auto_Transportadora,
            byte transportadora_Selecao_Auto_Tipo_Endereco)
        {
            Transportadora_Id = transportadora_Id;
            Transportadora_Selecao_Auto = transportadora_Selecao_Auto;
            Transportadora_Selecao_Auto_Cep = transportadora_Selecao_Auto_Cep;
            Transportadora_Selecao_Auto_Transportadora = transportadora_Selecao_Auto_Transportadora;
            Transportadora_Selecao_Auto_Tipo_Endereco = transportadora_Selecao_Auto_Tipo_Endereco;
        }

        public string? Transportadora_Id { get; private set; }
        public bool Transportadora_Selecao_Auto { get; private set; }
        public byte Transportadora_Selecao_Auto_status()
        {
            if (Transportadora_Selecao_Auto)
                return Constantes.TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_S;
            return Constantes.TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_N;
        }

        public string? Transportadora_Selecao_Auto_Cep { get; private set; }
        public string? Transportadora_Selecao_Auto_Transportadora { get; private set; }
        public byte Transportadora_Selecao_Auto_Tipo_Endereco { get; private set; }

        public static async Task<Transportadora> CriarInstancia(PedidoCriacaoDados pedido)
        {
            //todo: fazer trnasportadora
            var ret = new Transportadora(
                transportadora_Id: null,
                transportadora_Selecao_Auto: false,
                transportadora_Selecao_Auto_Cep: null,
                transportadora_Selecao_Auto_Transportadora: null,
                transportadora_Selecao_Auto_Tipo_Endereco: 0);
            return ret;
        }
    }
}
