using InfraBanco;
using System;
using System.Collections.Generic;
using System.Linq;
using Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll;

namespace Testes.Automatizados.InicializarBanco
{
    public class InicializarBancoCep
    {
        private static bool _inicialziado = false;
        private readonly ContextoCepProvider contextoCepProvider;

        public InicializarBancoCep(InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoCepProvider = contextoCepProvider;

            if (!_inicialziado)
            {
                _inicialziado = true;
                Inicalizar();
            }
        }

        public static class DadosCep
        {
            public static string Cep = "04321001";
            public static string Cidade = "cidade somente no CEP";
            public static string CepNaoExiste = "14321001";

            //este tem que ser AP
            public static string Ufe_sg = "AP";
            public static string Ufe_sgNaoExiste = "XX";
        }

        private void Inicalizar()
        {
            var db = contextoCepProvider.GetContextoLeitura();

            var Bai_nu_sequencial_ini = 1;
            var Loc_nu_sequencial = 1;

            db.LogBairros.Add(new InfraBanco.Modelos.LogBairro()
            {
                Bai_nu_sequencial = Bai_nu_sequencial_ini,
                Bai_no = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Bairro
            });

            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade()
            {
                Cep_dig = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Cep,
                Loc_nu_sequencial = Loc_nu_sequencial,
                Loc_nosub = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Cidade
            });

            string[] Log_no_array = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Endereco.Split(' ');
            var Log_no = string.Join(' ', new List<string>(Log_no_array).Skip(1));
            db.LogLogradouros.Add(new InfraBanco.Modelos.LogLogradouro()
            {
                Bai_nu_sequencial_ini = Bai_nu_sequencial_ini,
                Loc_nu_sequencial = Loc_nu_sequencial,
                Cep_dig = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Cep,
                Ufe_sg = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Uf,
                Log_tipo_logradouro = Log_no_array[0],
                Log_no = Log_no
            });


            //==============================

            Bai_nu_sequencial_ini++;
            Loc_nu_sequencial++;

            db.LogBairros.Add(new InfraBanco.Modelos.LogBairro()
            {
                Bai_nu_sequencial = Bai_nu_sequencial_ini,
                Bai_no = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoCadastralCliente.Endereco_bairro
            });

            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade()
            {
                Cep_dig = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoCadastralCliente.Endereco_cep,
                Loc_nu_sequencial = Loc_nu_sequencial,
                Loc_nosub = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoCadastralCliente.Endereco_cidade
            });

            Log_no_array = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoCadastralCliente.Endereco_logradouro.Split(' ');
            Log_no = string.Join(' ', new List<string>(Log_no_array).Skip(1));
            db.LogLogradouros.Add(new InfraBanco.Modelos.LogLogradouro()
            {
                Bai_nu_sequencial_ini = Bai_nu_sequencial_ini,
                Loc_nu_sequencial = Loc_nu_sequencial,
                Cep_dig = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoCadastralCliente.Endereco_cep,
                Ufe_sg = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoCadastralCliente.Endereco_uf,
                Log_tipo_logradouro = Log_no_array[0],
                Log_no = Log_no
            });

            //==============================
            Bai_nu_sequencial_ini++;
            Loc_nu_sequencial++;

            db.LogBairros.Add(new InfraBanco.Modelos.LogBairro()
            {
                Bai_nu_sequencial = Bai_nu_sequencial_ini,
                Bai_no = DadosPrepedidoUnisBll.PrepedidoEnderecoEntregaCompleto().EnderecoEntrega.EndEtg_bairro
            });
            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade()
            {
                Cep_dig = DadosPrepedidoUnisBll.PrepedidoEnderecoEntregaCompleto().EnderecoEntrega.EndEtg_cep,
                Loc_nu_sequencial = Loc_nu_sequencial,
                Loc_nosub = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoEntrega.EndEtg_cidade
            });

            Log_no_array = DadosPrepedidoUnisBll.PrepedidoEnderecoEntregaCompleto().EnderecoEntrega.EndEtg_endereco.Split(' ');
            Log_no = string.Join(' ', new List<string>(Log_no_array).Skip(1));
            db.LogLogradouros.Add(new InfraBanco.Modelos.LogLogradouro()
            {
                Bai_nu_sequencial_ini = Bai_nu_sequencial_ini,
                Loc_nu_sequencial = Loc_nu_sequencial,
                Cep_dig = DadosPrepedidoUnisBll.PrepedidoEnderecoEntregaCompleto().EnderecoEntrega.EndEtg_cep,
                Ufe_sg = DadosPrepedidoUnisBll.PrepedidoEnderecoEntregaCompleto().EnderecoEntrega.EndEtg_uf,
                Log_tipo_logradouro = Log_no_array[0],
                Log_no = Log_no,
            });

            Bai_nu_sequencial_ini++;
            Loc_nu_sequencial++;
            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade() { Cep_dig = DadosCep.Cep, Ufe_sg = DadosCep.Ufe_sg, Loc_nosub = DadosCep.Cidade, Loc_nu_sequencial = Loc_nu_sequencial });

            db.SaveChanges();
        }

        //esta versão não permite testar os bairros errados (poruqe não informa os bairros)
        private void Inicalizar_somente_LogLocalidades()
        {
            var db = contextoCepProvider.GetContextoLeitura();

            var Loc_nu_sequencial = 1;
            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade()
            {
                Cep_dig = Testes.Automatizados.InicializarBanco.InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Cep,
                Ufe_sg = Testes.Automatizados.InicializarBanco.InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Uf,
                Loc_nosub = Testes.Automatizados.InicializarBanco.InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Cidade,
                Loc_nu_sequencial = Loc_nu_sequencial++
            });

            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade() { Cep_dig = DadosCep.Cep, Ufe_sg = DadosCep.Ufe_sg, Loc_nosub = DadosCep.Cidade, Loc_nu_sequencial = Loc_nu_sequencial++ });

            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade()
            {
                Cep_dig = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoCadastralCliente.Endereco_cep,
                Ufe_sg = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoCadastralCliente.Endereco_uf,
                Loc_nosub = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().EnderecoCadastralCliente.Endereco_cidade,
                Loc_nu_sequencial = Loc_nu_sequencial++
            });


            db.SaveChanges();
        }

    }
}
