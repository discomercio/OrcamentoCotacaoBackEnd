using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Criacao.Execucao
{
    public class Transportadora
    {
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
    }
}
