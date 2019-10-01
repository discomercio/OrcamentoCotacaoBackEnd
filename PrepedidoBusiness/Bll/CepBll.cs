using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PrepedidoBusiness.Dto.Cep;
using Microsoft.EntityFrameworkCore;

namespace PrepedidoBusiness.Bll
{
    public class CepBll
    {
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;

        public CepBll(InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoCepProvider = contextoCepProvider;
        }

        public async Task<IEnumerable<CepDto>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            List<CepDto> cepDto = new List<CepDto>();

            if (!string.IsNullOrEmpty(cep))
            {
                cep = cep.Replace("-", "");

                if (cep.Length == 8)
                {
                    cepDto = (await BuscarPorCep(cep)).ToList();
                }
            }
            else if (!string.IsNullOrEmpty(endereco) && !string.IsNullOrEmpty(uf) && !string.IsNullOrEmpty(cidade))
            {
                endereco = endereco.Replace("Rua", "").Replace("R", "").Replace(":", "").Replace("Av", "").Replace("Avenida", "").Replace(".", "").Trim();

                cepDto = (await BuscarPorEndereco(endereco, uf, cidade)).ToList();
            }

            return cepDto;
        }

        public async Task<IEnumerable<CepDto>> BuscarPorCep(string cep)
        {
            var db = contextoCepProvider.GetContextoLeitura();

            var cepTask = ((from c in db.LogLogradouros
                            join d in db.LogBairros on c.Bai_nu_sequencial_ini equals d.Bai_nu_sequencial
                            join e in db.LogLocalidades on c.Loc_nu_sequencial equals e.Loc_nu_sequencial
                            where c.Cep_dig.EndsWith(cep)
                            select new CepDto
                            {
                                Cep = c.Cep_dig,
                                Uf = c.Ufe_sg,
                                Cidade = e.Loc_nosub,
                                Bairro = d.Bai_no,
                                Endereco = c.Log_tipo_logradouro + " " + c.Log_no,
                                LogradouroComplemento = c.Log_complemento
                            })
                           .Union(from c in db.LogLocalidades
                                  where c.Cep_dig.EndsWith(cep)
                                  select new CepDto
                                  {
                                      Cep = c.Cep_dig,
                                      Uf = c.Ufe_sg,
                                      Cidade = c.Loc_nosub
                                  })
                           .Union(from c in db.TcepLogradouros
                                  where c.Cep8_log.EndsWith(cep)
                                  select new CepDto
                                  {
                                      Cep = c.Cep8_log,
                                      Uf = c.Uf_log,
                                      Cidade = c.Nome_local,
                                      Bairro = c.Extenso_bai,
                                      Endereco = c.Abrev_tipo + " " + c.Nome_log,
                                      LogradouroComplemento = c.Comple_log
                                  }));

            var cepDto = await cepTask.ToListAsync();

            return cepDto;
        }

        public async Task<IEnumerable<CepDto>> BuscarPorEndereco(string endereco, string uf, string cidade)
        {
            var db = contextoCepProvider.GetContextoLeitura();

            var cepTask = ((from c in db.LogLogradouros
                            join d in db.LogBairros on c.Bai_nu_sequencial_ini equals d.Bai_nu_sequencial
                            join e in db.LogLocalidades on c.Loc_nu_sequencial equals e.Loc_nu_sequencial
                            where (c.Ufe_sg == uf) &&
                                  (e.Loc_nosub == cidade) &&
                                  (c.Log_no.Contains(endereco))
                            select new CepDto
                            {
                                Cep = c.Cep_dig,
                                Uf = c.Ufe_sg,
                                Cidade = e.Loc_nosub,
                                Bairro = d.Bai_no,
                                Endereco = c.Log_tipo_logradouro + " " + c.Log_no,
                                LogradouroComplemento = c.Log_complemento
                            }).Take(300)
                            .Union(from c in db.LogLocalidades
                                   where c.Ufe_sg == uf &&
                                         c.Loc_nosub == endereco &&
                                         c.Cep_dig.Length > 0
                                   select new CepDto
                                   {
                                       Cep = c.Cep_dig,
                                       Uf = c.Ufe_sg,
                                       Cidade = c.Loc_nosub
                                   })
                            .Union(from c in db.TcepLogradouros
                                   where c.Uf_log == uf &&
                                         c.Nome_local == endereco
                                   orderby c.Uf_log, c.Nome_local, c.Extenso_bai, c.Nome_log
                                   select new CepDto
                                   {
                                       Cep = c.Cep8_log,
                                       Uf = c.Uf_log,
                                       Cidade = c.Nome_local,
                                       Bairro = c.Extenso_bai,
                                       Endereco = c.Abrev_tipo + " " + c.Nome_log,
                                       LogradouroComplemento = c.Comple_log
                                   }).Take(300));

            var cepDto = await cepTask.ToListAsync();

            return cepDto;
        }
    }
}
