using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoClienteDados
    {
        public PedidoCriacaoClienteDados(string id_cliente, string tipo, string cnpj_Cpf)
        {
            Id_cliente = id_cliente ?? throw new ArgumentNullException(nameof(id_cliente));
            Tipo = tipo ?? throw new ArgumentNullException(nameof(tipo));
            Cnpj_Cpf = cnpj_Cpf ?? throw new ArgumentNullException(nameof(cnpj_Cpf));

            //normalizacao de campos
            Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(Cnpj_Cpf);
        }

        public string Id_cliente { get;  }
        public string Tipo { get;  }
        public string Cnpj_Cpf { get;  }

        //todo: remover esta conversao
        static public PedidoCriacaoClienteDados PedidoCriacaoClienteDados_de_DadosClienteCadastroDados(Cliente.Dados.DadosClienteCadastroDados origem)
        {
            if (origem is null)
            {
                throw new ArgumentNullException(nameof(origem));
            }

            return new PedidoCriacaoClienteDados(
                cnpj_Cpf: origem.Cnpj_Cpf,
                id_cliente: origem.Id,
                tipo: origem.Tipo
            );
        }
    }
}
