using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoClienteDados
    {
        public PedidoCriacaoClienteDados(Constantes.TipoPessoa tipo, string cnpj_Cpf, string? midia)
        {
            Tipo = tipo ?? throw new ArgumentNullException(nameof(tipo));
            Cnpj_Cpf = cnpj_Cpf ?? throw new ArgumentNullException(nameof(cnpj_Cpf));

            Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(Cnpj_Cpf);
            Midia = midia;
        }

        public InfraBanco.Constantes.Constantes.TipoPessoa Tipo { get; }
        public string Cnpj_Cpf { get; }
        public string? Midia { get; }

        static public PedidoCriacaoClienteDados PedidoCriacaoClienteDados_de_DadosClienteCadastroDados(Cliente.Dados.DadosClienteCadastroDados origem, string? midia)
        {
            if (origem is null)
            {
                throw new ArgumentNullException(nameof(origem));
            }

            return new PedidoCriacaoClienteDados(
                cnpj_Cpf: origem.Cnpj_Cpf,
                tipo: new Constantes.TipoPessoa(origem.Tipo),
                midia: midia
            );
        }
    }
}
