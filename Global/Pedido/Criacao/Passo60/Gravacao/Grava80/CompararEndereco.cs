using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Criacao.Passo60.Gravacao.Grava80
{
    public static class CompararEndereco
    {
        public static bool IsEnderecoIgual(string? end_logradouro_1, string? end_numero_1, string? end_cep_1,
            string? end_logradouro_2, string? end_numero_2, string? end_cep_2)
        {
            const string PREFIXOS = "|R|RUA|AV|AVEN|AVENIDA|TV|TRAV|TRAVESSA|AL|ALAM|ALAMEDA|PC|PRACA|PQ|PARQUE|EST|ESTR|ESTRADA|CJ|CONJ|CONJUNTO|";

            //garante que não temos nenhum null
            end_logradouro_1 ??= "";
            end_numero_1 ??= "";
            end_cep_1 ??= "";
            end_logradouro_2 ??= "";
            end_numero_2 ??= "";
            end_cep_2 ??= "";

            //'	Normaliza
            end_logradouro_1 = UtilsGlobais.TratamentoTexto.Retira_acentuacao(end_logradouro_1.ToUpperInvariant().Trim());
            end_numero_1 = end_numero_1.ToUpperInvariant().Trim();
            end_cep_1 = UtilsGlobais.Util.Cep_SoDigito(end_cep_1);
            end_logradouro_2 = UtilsGlobais.TratamentoTexto.Retira_acentuacao(end_logradouro_2.ToUpperInvariant().Trim());
            end_numero_2 = end_numero_2.ToUpperInvariant().Trim();
            end_cep_2 = UtilsGlobais.Util.Cep_SoDigito(end_cep_2);

            if (end_cep_1 != end_cep_2)
                return false;


            //'	COMPARA OS NÚMEROS DO ENDEREÇO, LEVANDO EM CONSIDERAÇÃO CASOS COMO: 476/478
            var blnNumeroIgual = false;

            if (end_numero_1 == end_numero_2)
                blnNumeroIgual = true;

            if (!blnNumeroIgual)
            {
                var v_end_numero_1 = end_numero_1.Split("/");
                var n_end_numero_1 = 0;
                foreach (var v_end_numero_1_i in v_end_numero_1)
                {
                    if (UtilsGlobais.TratamentoTexto.Retorna_so_digitos(v_end_numero_1_i) != "")
                        n_end_numero_1 = n_end_numero_1 + 1;
                }


                var v_end_numero_2 = end_numero_2.Split("/");
                var n_end_numero_2 = 0;
                foreach (var v_end_numero_2_i in v_end_numero_2)
                {
                    if (UtilsGlobais.TratamentoTexto.Retorna_so_digitos(v_end_numero_2_i) != "")
                        n_end_numero_2 = n_end_numero_2 + 1;
                }

                if ((n_end_numero_1 == 1) && (n_end_numero_2 == 1))
                {
                    if (end_numero_1 != end_numero_2)
                        return false;
                }
                else
                {
                    foreach (var v_end_numero_1_i in v_end_numero_1)
                    {
                        if (UtilsGlobais.TratamentoTexto.Retorna_so_digitos(v_end_numero_1_i) != "")
                        {
                            foreach (var v_end_numero_2_j in v_end_numero_2)
                            {
                                if (UtilsGlobais.TratamentoTexto.Retorna_so_digitos(v_end_numero_2_j) != "")
                                {
                                    if (v_end_numero_1_i.Trim() == v_end_numero_2_j.Trim())
                                    {
                                        blnNumeroIgual = true;
                                        break;
                                    }
                                }
                            }
                            if (blnNumeroIgual)
                                break;
                        }
                    }
                }
            }
            if (!blnNumeroIgual)
                return false;

            var listaStringsParaVirarEspaco = new List<string> { ",", ".", "-", ";", ":", "=" };
            foreach (var virarEspaco in listaStringsParaVirarEspaco)
            {
                end_logradouro_1 = end_logradouro_1.Replace(virarEspaco, " ");
                end_logradouro_2 = end_logradouro_2.Replace(virarEspaco, " ");
            }

            var s1 = "";
            {
                var v1 = end_logradouro_1.Split(" ");
                foreach (var v1_i in v1)
                {
                    var blnFlag = false;
                    var s = v1_i.Trim();
                    if (s != "")
                    {
                        if (s1 == "")
                        {
                            if (!PREFIXOS.Contains("|" + s + "|"))
                                blnFlag = true;
                        }
                        else
                        {
                            blnFlag = true;
                        }

                        if (blnFlag)
                        {
                            if (s1 != "")
                                s1 = s1 + " ";
                            s1 = s1 + s;
                        }
                    }
                }
            }

            var s2 = "";
            {
                var v2 = end_logradouro_2.Split(" ");
                foreach (var v2_i in v2)
                {
                    var blnFlag = false;
                    var s = v2_i.Trim();
                    if (s != "")
                    {
                        if (s2 == "")
                        {
                            if (!PREFIXOS.Contains("|" + s + "|"))
                                blnFlag = true;
                        }
                        else
                        {
                            blnFlag = true;
                        }

                        if (blnFlag)
                        {
                            if (s2 != "")
                                s2 = s2 + " ";
                            s2 = s2 + s;
                        }
                    }
                }
            }
            if (s1 != s2)
                return false;
            return true;
        }

    }
}
