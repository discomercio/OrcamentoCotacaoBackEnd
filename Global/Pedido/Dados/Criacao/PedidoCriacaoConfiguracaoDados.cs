using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoConfiguracaoDados
    {
        public PedidoCriacaoConfiguracaoDados(decimal limiteArredondamento, decimal maxErroArredondamento, Constantes.CodSistemaResponsavel sistemaResponsavelCadastro)
        {
            LimiteArredondamento = limiteArredondamento;
            MaxErroArredondamento = maxErroArredondamento;
            SistemaResponsavelCadastro = sistemaResponsavelCadastro;
        }

        public decimal LimiteArredondamento { get; }
        public decimal MaxErroArredondamento { get; }
        public InfraBanco.Constantes.Constantes.CodSistemaResponsavel SistemaResponsavelCadastro { get; }
    }
}
