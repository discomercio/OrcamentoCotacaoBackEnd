using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoConfiguracaoDados
    {
        public PedidoCriacaoConfiguracaoDados(Constantes.CodSistemaResponsavel sistemaResponsavelCadastro, decimal limiteArredondamento, decimal maxErroArredondamento, int limitePedidosExatamenteIguais_Numero, int limitePedidosExatamenteIguais_TempoSegundos, int limitePedidosMesmoCpfCnpj_Numero, int limitePedidosMesmoCpfCnpj_TempoSegundos)
        {
            SistemaResponsavelCadastro = sistemaResponsavelCadastro;
            LimiteArredondamento = limiteArredondamento;
            MaxErroArredondamento = maxErroArredondamento;
            LimitePedidosExatamenteIguais_Numero = limitePedidosExatamenteIguais_Numero;
            LimitePedidosExatamenteIguais_TempoSegundos = limitePedidosExatamenteIguais_TempoSegundos;
            LimitePedidosMesmoCpfCnpj_Numero = limitePedidosMesmoCpfCnpj_Numero;
            LimitePedidosMesmoCpfCnpj_TempoSegundos = limitePedidosMesmoCpfCnpj_TempoSegundos;
        }

        public InfraBanco.Constantes.Constantes.CodSistemaResponsavel SistemaResponsavelCadastro { get; }
        public decimal LimiteArredondamento { get; }
        public decimal MaxErroArredondamento { get; }
        public int LimitePedidosExatamenteIguais_Numero { get; }
        public int LimitePedidosExatamenteIguais_TempoSegundos { get; }
        public int LimitePedidosMesmoCpfCnpj_Numero { get; }
        public int LimitePedidosMesmoCpfCnpj_TempoSegundos { get; }

    }
}
