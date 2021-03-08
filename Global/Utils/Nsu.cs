using InfraBanco;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Modelos;

namespace UtilsGlobais
{
    public static class Nsu
    {
        public static async Task<string> GerarNsu(ContextoBdGravacao dbgravacao, string id_nsu)
        {
            if (string.IsNullOrEmpty(id_nsu))
                throw new ArgumentException("Não foi especificado o NSU a ser gerado!");

            var queryControle = from c in dbgravacao.Tcontroles
                                where c.Id_Nsu == id_nsu
                                select c;

            var controle = await queryControle.FirstOrDefaultAsync();
            if (controle == null)
                throw new ArgumentException($"Não existe registro na tabela de controle para poder gerar este NSU! id_nsu:{id_nsu}");

            int n_nsu = -1;
            if (!string.IsNullOrEmpty(controle.Nsu))
            {
                if (int.TryParse(controle.Nsu, out _))
                {
                    if (controle.Seq_Anual != null && controle.Seq_Anual != 0)
                    {
                        //'	CASO O RELÓGIO DO SERVIDOR SEJA ALTERADO P/ DATAS FUTURAS E PASSADAS, EVITA QUE O CAMPO 'ano_letra_seq' SEJA INCREMENTADO VÁRIAS VEZES
                        if (DateTime.Now.Year > controle.Dt_Ult_Atualizacao.Year)
                        {
                            string saux = "0";
                            saux = Util.Normaliza_Codigo(saux, Constantes.TAM_MAX_NSU);
                            controle.Nsu = saux;
                            controle.Dt_Ult_Atualizacao = DateTime.Now;
                            if (!String.IsNullOrEmpty(controle.Ano_Letra_Seq))
                            {
                                int asc;
                                char chr;
                                asc = Encoding.ASCII.GetBytes(controle.Ano_Letra_Seq)[0] + controle.Ano_Letra_Step;
                                chr = (char)asc;
                                controle.Ano_Letra_Seq = chr.ToString();
                            }
                        }
                    }
                    n_nsu = int.Parse(controle.Nsu);
                }
            }


            if (n_nsu < 0)
            {
                throw new ApplicationException($"O NSU gerado é inválido! id_nsu:{id_nsu}");
            }

            n_nsu += 1;
            string s;
            s = Convert.ToString(n_nsu);
            s = Util.Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
            if (s.Length == 12)
            {
                //para salvar o novo numero
                controle.Nsu = s;
                if (DateTime.Now > controle.Dt_Ult_Atualizacao)
                    controle.Dt_Ult_Atualizacao = DateTime.Now;

                string retorno = controle.Nsu;

                try
                {
                    dbgravacao.Update(controle);
                    await dbgravacao.SaveChangesAsync();
                    return retorno;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Não foi possível gerar o NSU, pois ocorreu o seguinte erro: " + ex.HResult + ":" + ex.Message);
                }
            }

            throw new ApplicationException($"Não foi possível gerar o NSU, tamanho diferente de 12.  id_nsu:{id_nsu}");
        }


        public static async Task<int> Fin_gera_nsu(string id_nsu, ContextoBdGravacao dbgravacao)
        {
            //verifica se o id_nsu já existe
            var existeIdFin = await (from c in dbgravacao.TfinControles
                            where c.Id == id_nsu
                            select c.Id).AnyAsync();


            if (!existeIdFin)
            {
                //'	NÃO ESTÁ CADASTRADO, ENTÃO CADASTRA AGORA
                TfinControle tfinControle = new TfinControle();

                tfinControle.Id = id_nsu;
                tfinControle.Nsu = 0;
                tfinControle.Dt_hr_ult_atualizacao = DateTime.Now;

                dbgravacao.Add(tfinControle);
            }

            //'	OBTÉM O ÚLTIMO NSU USADO
            var tfincontroleEditando = await (from c in dbgravacao.TfinControles
                                              where c.Id == id_nsu
                                              select c).FirstOrDefaultAsync();

            if (tfincontroleEditando == null)
                throw new ApplicationException("Falha ao localizar o registro para geração de NSU (" + id_nsu + ")!");

            tfincontroleEditando.Nsu++;
            tfincontroleEditando.Dt_hr_ult_atualizacao = DateTime.Now;
            dbgravacao.Update(tfincontroleEditando);
            await dbgravacao.SaveChangesAsync();

            return tfincontroleEditando.Nsu;
        }

    }
}
