using InfraBanco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Especificacao.Testes.Utils.BancoTestes
{
    public class InicializarBancoCep
    {
        private readonly ContextoCepProvider contextoCepProvider;

        public InicializarBancoCep(InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoCepProvider = contextoCepProvider;
        }

        public void Inicializar(bool apagarDadosExistentes)
        {
            ContextoCepBd db = contextoCepProvider.GetContextoLeitura();

            if (apagarDadosExistentes)
            {
                foreach (var c in db.LogBairros)
                    db.LogBairros.Remove(c);
                foreach (var c in db.LogLocalidades)
                    db.LogLocalidades.Remove(c);
                foreach (var c in db.LogLogradouros)
                    db.LogLogradouros.Remove(c);
                db.SaveChanges();
            }

            int Bai_nu_sequencial_ini = 1;
            int Loc_nu_sequencial = 1;

            //do Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedidoDados
            InicializarRegistro(db, ref Bai_nu_sequencial_ini, ref Loc_nu_sequencial,
                "02045080", "São Paulo", "Jardim São Paulo(Zona Norte)", "Rua Professor Fábio Fanucchi", "SP");
            InicializarRegistro(db, ref Bai_nu_sequencial_ini, ref Loc_nu_sequencial,
                "02408150", "São Paulo", "Água Fria", "Rua Francisco Pecoraro", "SP");


            db.SaveChanges();
        }

        private void InicializarRegistro(ContextoCepBd db, ref int Bai_nu_sequencial_ini, ref int Loc_nu_sequencial,
            string cep, string cidade, string bairro, string endereco, string uf)
        {
            db.LogBairros.Add(new InfraBanco.Modelos.LogBairro()
            {
                Bai_nu_sequencial = Bai_nu_sequencial_ini,
                Bai_no = bairro
            });

            db.LogLocalidades.Add(new InfraBanco.Modelos.LogLocalidade()
            {
                Cep_dig = cep,
                Loc_nu_sequencial = Loc_nu_sequencial,
                Loc_nosub = cidade
            });

            string[] Log_no_array = endereco.Split(' ');
            var Log_no = string.Join(' ', new List<string>(Log_no_array).Skip(1));
            db.LogLogradouros.Add(new InfraBanco.Modelos.LogLogradouro()
            {
                Bai_nu_sequencial_ini = Bai_nu_sequencial_ini,
                Loc_nu_sequencial = Loc_nu_sequencial,
                Cep_dig = cep,
                Ufe_sg = uf,
                Log_tipo_logradouro = Log_no_array[0],
                Log_no = Log_no
            });


            Bai_nu_sequencial_ini++;
            Loc_nu_sequencial++;
        }

    }
}
