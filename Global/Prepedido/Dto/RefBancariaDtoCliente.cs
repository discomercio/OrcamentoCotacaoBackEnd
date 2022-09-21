using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dto
{
    public class RefBancariaDtoCliente
    {
        public string Banco { get; set; }
        public string BancoDescricao { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string Ddd { get; set; }
        public string Telefone { get; set; }
        public string Contato { get; set; }
        public int Ordem { get; set; }

        public static List<RefBancariaDtoCliente> ListaRefBancariaDtoCliente_De_RefBancariaClienteDados(IEnumerable<Cliente.Dados.Referencias.RefBancariaClienteDados> refBancariaClienteDados)
        {
            if (refBancariaClienteDados == null) return null;
            var ret = new List<RefBancariaDtoCliente>();
            if (refBancariaClienteDados != null)
                foreach (var p in refBancariaClienteDados)
                    ret.Add(RefBancariaDtoCliente_De_RefBancariaClienteDados(p));
            return ret;
        }

        public static RefBancariaDtoCliente RefBancariaDtoCliente_De_RefBancariaClienteDados(Cliente.Dados.Referencias.RefBancariaClienteDados refBancariaClienteDados)
        {
            if (refBancariaClienteDados == null) return null;
            var ret = new RefBancariaDtoCliente()
            {
                Banco = refBancariaClienteDados.Banco,
                BancoDescricao = refBancariaClienteDados.BancoDescricao,
                Agencia = refBancariaClienteDados.Agencia,
                Conta = refBancariaClienteDados.Conta,
                Ddd = refBancariaClienteDados.Ddd,
                Telefone = refBancariaClienteDados.Telefone,
                Contato = refBancariaClienteDados.Contato,
                Ordem = refBancariaClienteDados.Ordem
            };
            return ret;
        }

        public static List<Cliente.Dados.Referencias.RefBancariaClienteDados> ListaRefBancariaClienteDados_De_RefBancariaDtoCliente(IEnumerable<RefBancariaDtoCliente> refBancariaClienteDados)
        {
            if (refBancariaClienteDados == null) return null;
            var ret = new List<Cliente.Dados.Referencias.RefBancariaClienteDados>();
            if (refBancariaClienteDados != null)
                foreach (var p in refBancariaClienteDados)
                    ret.Add(RefBancariaClienteDados_De_RefBancariaDtoCliente(p));
            return ret;
        }

        public static Cliente.Dados.Referencias.RefBancariaClienteDados RefBancariaClienteDados_De_RefBancariaDtoCliente(RefBancariaDtoCliente refBancariaClienteDados)
        {
            if (refBancariaClienteDados == null) return null;
            var ret = new Cliente.Dados.Referencias.RefBancariaClienteDados()
            {
                Banco = refBancariaClienteDados.Banco,
                BancoDescricao = refBancariaClienteDados.BancoDescricao,
                Agencia = refBancariaClienteDados.Agencia,
                Conta = refBancariaClienteDados.Conta,
                Ddd = refBancariaClienteDados.Ddd,
                Telefone = refBancariaClienteDados.Telefone,
                Contato = refBancariaClienteDados.Contato,
                Ordem = refBancariaClienteDados.Ordem
            };
            return ret;
        }

    }
}
