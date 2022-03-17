using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoBusiness.Dto.ClienteCadastro.Referencias
{
    public class RefComercialDtoCliente
    {
        [Required]
        [MaxLength(60)]
        public string Nome_Empresa { get; set; }

        [MaxLength(40)]
        public string Contato { get; set; }

        [MaxLength(2)]
        public string Ddd { get; set; }

        [MaxLength(9)]
        public string Telefone { get; set; }

        public int Ordem { get; set; }


        public static List<RefComercialDtoCliente> ListaRefComercialDtoCliente_De_RefComercialClienteDados(IEnumerable<Cliente.Dados.Referencias.RefComercialClienteDados> origem)
        {
            if (origem == null) return null;
            var ret = new List<RefComercialDtoCliente>();
            if (origem != null)
                foreach (var p in origem)
                    ret.Add(RefComercialDtoCliente_De_RefComercialClienteDados(p));
            return ret;
        }

        public static RefComercialDtoCliente RefComercialDtoCliente_De_RefComercialClienteDados(Cliente.Dados.Referencias.RefComercialClienteDados origem)
        {
            if (origem == null) return null;
            var ret = new RefComercialDtoCliente()
            {
                Nome_Empresa = origem.Nome_Empresa,
                Contato = origem.Contato,
                Ddd = origem.Ddd,
                Telefone = origem.Telefone,
                Ordem = origem.Ordem
            };
            return ret;
        }

        public static List<Cliente.Dados.Referencias.RefComercialClienteDados> ListaRefComercialClienteDados_De_RefComercialDtoCliente(IEnumerable<RefComercialDtoCliente> origem)
        {
            if (origem == null) return null;
            var ret = new List<Cliente.Dados.Referencias.RefComercialClienteDados>();
            if (origem != null)
                foreach (var p in origem)
                    ret.Add(RefComercialClienteDados_De_RefComercialDtoCliente(p));
            return ret;
        }

        public static Cliente.Dados.Referencias.RefComercialClienteDados RefComercialClienteDados_De_RefComercialDtoCliente(RefComercialDtoCliente origem)
        {
            if (origem == null) return null;
            var ret = new Cliente.Dados.Referencias.RefComercialClienteDados()
            {
                Nome_Empresa = origem.Nome_Empresa,
                Contato = origem.Contato,
                Ddd = origem.Ddd,
                Telefone = origem.Telefone,
                Ordem = origem.Ordem
            };
            return ret;
        }

    }
}
