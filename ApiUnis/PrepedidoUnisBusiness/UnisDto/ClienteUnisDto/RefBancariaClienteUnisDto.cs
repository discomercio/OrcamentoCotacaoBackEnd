using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class RefBancariaClienteUnisDto
    {
        [Required]
        [MaxLength(4)]
        public string Banco { get; set; }

        [Required]
        [MaxLength(8)]
        public string Agencia { get; set; }

        [Required]
        [MaxLength(12)]
        public string Conta { get; set; }

        [MaxLength(2)]
        public string Ddd { get; set; }

        [MaxLength(9)]
        public string Telefone { get; set; }

        [MaxLength(40)]
        public string Contato { get; set; }

        public int Ordem { get; private set; }

        public static RefBancariaClienteUnisDto RefBancariaClienteUnisDtoDeRefBancariaClienteDto(RefBancariaDtoCliente refBancariaArclube)
        {
            var ret = new RefBancariaClienteUnisDto()
            {
                Agencia = refBancariaArclube.Agencia,
                Banco = refBancariaArclube.Banco,
                Conta = refBancariaArclube.Conta,
                Contato = refBancariaArclube.Contato,
                Ddd = refBancariaArclube.Ddd,
                Telefone = refBancariaArclube.Telefone,
                Ordem = refBancariaArclube.Ordem
            };

            return ret;
        }

        public static RefBancariaDtoCliente RefBancariaClienteDtoDeRefBancariaClienteUnisDto(RefBancariaClienteUnisDto refBancariaUnis)
        {
            var ret = new RefBancariaDtoCliente()
            {
                Agencia = refBancariaUnis.Agencia,
                Banco = refBancariaUnis.Banco,
                Conta = refBancariaUnis.Conta,
                Contato = refBancariaUnis.Contato,
                Ddd = refBancariaUnis.Ddd,
                Telefone = refBancariaUnis.Telefone,
                Ordem = refBancariaUnis.Ordem
            };

            return ret;
        }
    }
}
