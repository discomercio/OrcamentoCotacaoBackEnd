using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ClienteDto
{
    public class RefBancariaDtoCliente
    {
        public string Banco { get; set; }
        public string BancoDescricao { get; set; }
        public string Agencia { get; set; }
        public string ContaBanco { get; set; }
        public string DddBanco { get; set; }
        public string TelefoneBanco { get; set; }
        public string ContatoBanco { get; set; }
        public int OrdemBanco { get; set; }

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
                ContaBanco = refBancariaClienteDados.Conta,
                DddBanco = refBancariaClienteDados.Ddd,
                TelefoneBanco = refBancariaClienteDados.Telefone,
                ContatoBanco = refBancariaClienteDados.Contato,
                OrdemBanco = refBancariaClienteDados.Ordem
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
                Conta = refBancariaClienteDados.ContatoBanco,
                Ddd = refBancariaClienteDados.DddBanco,
                Telefone = refBancariaClienteDados.TelefoneBanco,
                Contato = refBancariaClienteDados.ContatoBanco,
                Ordem = refBancariaClienteDados.OrdemBanco
            };
            return ret;
        }
    }
}
