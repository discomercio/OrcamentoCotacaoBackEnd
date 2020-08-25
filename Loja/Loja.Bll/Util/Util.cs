using Loja.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Loja.Modelos;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Loja.Bll.RegrasCtrlEstoque;
using Loja.Bll.Dto.ProdutoDto;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;
using Loja.Modelo;
using System.Globalization;

namespace Loja.Bll.Util
{
    public class Util
    {
        public static string FormataMoeda(decimal? valor)
        {
            if (!valor.HasValue)
                return "";
            //@*200218: hamilton pediu para colocar sem o cifrão *@
            return String.Format("{0:N2}", valor);
        }

        public static string FormataTelefone(string ddd, string tel)
        {
            string retorno = "";

            if (!String.IsNullOrEmpty(ddd) && !String.IsNullOrEmpty(tel))
            {
                string final = tel.Substring(tel.Length - 4);
                string inicio = tel.Substring(0, tel.Length - 4);
                retorno = "(" + ddd.Trim() + ") " + inicio + "-" + final;
            }

            return retorno;
        }
        public static string FormatCpf_Cnpj_Ie(string cpf_cnpj)
        {
            //caso esteja vazio, não formatamos
            if (!UInt64.TryParse(cpf_cnpj, out UInt64 convertido))
                return cpf_cnpj;

            if (cpf_cnpj.Length > 11)
            {
                if (cpf_cnpj.Length > 12)
                    return convertido.ToString(@"00\.000\.000\/0000\-00");
                else
                    return convertido.ToString(@"000\.000\.000\.000");
            }
            else
            {
                return convertido.ToString(@"000\.000\.000\-00");
            }

        }

        public static string FormataCep(string cep)
        {
            if (!UInt64.TryParse(cep, out UInt64 convertido))
                return cep;

            return convertido.ToString(@"00000-000");


        }

        public static string MontarDDD(string telefone)
        {
            telefone = telefone ?? "   ";
            string retorno = "";

            retorno = telefone.Substring(1, 2);

            return retorno;
        }

        public static string MontarTelefone(string telefone)
        {
            string retorno = "";

            retorno = telefone.Trim().Substring(4);

            return retorno;
        }

        public static async Task<bool> IsActivatedFlagPedidoUsarMemorizacaoCompletaEnderecos(LojaContextoBdProvider contexto)
        {
            bool retorno = false;

            var db = contexto.GetContextoLeitura();

            Tparametro tparametro = await BuscarRegistroParametro(Constantes.Constantes.ID_PARAMETRO_Flag_Pedido_MemorizacaoCompletaEnderecos,
                contexto.GetContextoLeitura());

            if (tparametro.Campo_inteiro == 1)
                retorno = true;

            return retorno;
        }

        public static async Task<string> GerarNsu(LojaContextoBdGravacao dbgravacao, string id_nsu)
        {
            string retorno = "";
            int n_nsu = -1;
            string s = "0";
            int asc;
            char chr;

            if (id_nsu == "")
                retorno = "Não foi especificado o NSU a ser gerado!!";

            for (int i = 0; i <= 100; i++)
            {
                var ret = from c in dbgravacao.Tcontroles
                          where c.Id_Nsu == id_nsu
                          select c;

                var controle = await ret.FirstOrDefaultAsync();


                if (!string.IsNullOrEmpty(controle.Nsu))
                {
                    if (controle.Seq_Anual != 0)
                    {
                        if (DateTime.Now.Year > controle.Dt_Ult_Atualizacao.Year)
                        {
                            s = Normaliza_Codigo(s, Constantes.Constantes.TAM_MAX_NSU);
                            controle.Dt_Ult_Atualizacao = DateTime.Now;
                            if (!String.IsNullOrEmpty(controle.Ano_Letra_Seq))
                            {
                                //Precisa revisar essa parte, pois lendo a doc do BD e analisando os dados na base não bate
                                asc = int.Parse(controle.Ano_Letra_Seq) + controle.Ano_Letra_Step;
                                chr = (char)asc;
                            }
                        }
                    }
                    n_nsu = int.Parse(controle.Nsu);
                }
                if (n_nsu < 0)
                {
                    retorno = "O NSU gerado é inválido!!";
                }
                n_nsu += 1;
                s = Convert.ToString(n_nsu);
                s = Normaliza_Codigo(s, Constantes.Constantes.TAM_MAX_NSU);
                if (s.Length == 12)
                {
                    i = 101;
                    //para salvar o novo numero
                    controle.Nsu = s;
                    if (DateTime.Now > controle.Dt_Ult_Atualizacao)
                        controle.Dt_Ult_Atualizacao = DateTime.Now;

                    retorno = controle.Nsu;

                    try
                    {
                        dbgravacao.Update(controle);
                        await dbgravacao.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        retorno = "Não foi possível gerar o NSU, pois ocorreu o seguinte erro: " + ex.HResult + ":" + ex.Message;
                    }
                }
            }

            return retorno;
        }

        public static string RemoverAcentos(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string Normaliza_Codigo(string cod, int tamanho_default)
        {
            string retorno = cod;
            string s = "0";


            if (cod != "")
            {
                for (int i = cod.Length; i < tamanho_default; i++)
                {
                    retorno = s + retorno;
                }
            }

            return retorno;
        }

        //Metodo recebe um parametro chave para realizar a verificação
        public static bool OpercaoPermitida(int idOperacao, string lstOpercoes)
        {
            bool retorno = false;
            string operacao = idOperacao.ToString().Trim();

            if (!String.IsNullOrEmpty(operacao))
            {
                operacao = "|" + operacao + "|";
                if (lstOpercoes.IndexOf(operacao) != -1)
                    retorno = true;
            }

            return retorno;
        }

        public static string SoDigitosCpf_Cnpj(string cpf_cnpj)
        {
            string retorno = "";

            if (cpf_cnpj.Length > 11)
            {
                if (cpf_cnpj.Length > 12)
                    retorno = cpf_cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                else
                    retorno = cpf_cnpj.Replace(".", "");
            }
            else
                retorno = cpf_cnpj.Replace(".", "").Replace("-", "");

            return retorno;
        }

        public static async Task<IEnumerable<string>> BuscarListaOrcamentistaEIndicador(LojaContextoBdProvider contexto,
            string indicador, string usuarioSistema, string loja)
        {
            //paraTeste
            //loja = "202";

            List<string> lstOrcaIndica = new List<string>();

            var db = contexto.GetContextoLeitura();

            if (Constantes.Constantes.ID_PARAM_SITE == Constantes.Constantes.COD_SITE_ASSISTENCIA_TECNICA)
            {
#pragma warning disable CS0162 // Unreachable code detected
                var orcaTask = from c in db.TorcamentistaEindicadors
#pragma warning restore CS0162 // Unreachable code detected
                               where (c.Apelido == indicador || c.Status == "A")
                               orderby c.Apelido
                               select c.Apelido;

                lstOrcaIndica = await orcaTask.ToListAsync();
            }
            else
            {
                if (IsLojaVrf(loja) || loja == Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                {
                    var orcaTask = from c in db.TorcamentistaEindicadors
                                   where (c.Apelido == indicador ||
                                          (c.Status == "A" && c.Loja == loja))
                                   orderby c.Apelido
                                   select c.Apelido;

                    lstOrcaIndica = await orcaTask.ToListAsync();
                }
                else if (loja == Constantes.Constantes.NUMERO_LOJA_OLD03 ||
                    loja == Constantes.Constantes.NUMERO_LOJA_OLD03_BONIFICACAO)
                {
                    var orcaTask = from c in db.TorcamentistaEindicadors
                                   where (c.Apelido == indicador || c.Status == "A")
                                   orderby c.Apelido
                                   select c.Apelido;

                    lstOrcaIndica = await orcaTask.ToListAsync();
                }
                else
                {
                    var orcaTask = from c in db.TorcamentistaEindicadors
                                   where (c.Apelido == indicador ||
                                          (c.Status == "A" && c.Vendedor == usuarioSistema))
                                   orderby c.Apelido
                                   select c.Apelido;

                    lstOrcaIndica = await orcaTask.ToListAsync();
                }
            }

            return lstOrcaIndica;
        }

        public static async Task<IEnumerable<string>> BuscarOrcamentistaEIndicadorParaProdutos(LojaContextoBdProvider contexto,
            string usuarioSistema, string lstOperacoesPermitidas, string loja)
        {
            //paraTeste
            //loja = "202";


            List<string> lstOrcaIndica = new List<string>();

            var db = contexto.GetContextoLeitura();

            if (Constantes.Constantes.ID_PARAM_SITE == Constantes.Constantes.COD_SITE_ASSISTENCIA_TECNICA)
            {
#pragma warning disable CS0162 // Unreachable code detected
                var orcaTask = from c in db.TorcamentistaEindicadors
#pragma warning restore CS0162 // Unreachable code detected
                               where c.Status == "A"
                               orderby c.Apelido
                               select c.Apelido;

                lstOrcaIndica = await orcaTask.ToListAsync();
            }
            else
            {
                if (IsLojaVrf(loja) || loja == Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                {
                    var orcaTask = from c in db.TorcamentistaEindicadors
                                   where c.Status == "A" && c.Loja == loja
                                   orderby c.Apelido
                                   select c.Apelido;

                    lstOrcaIndica = await orcaTask.ToListAsync();
                }
                else if (loja == Constantes.Constantes.NUMERO_LOJA_OLD03 ||
                    loja == Constantes.Constantes.NUMERO_LOJA_OLD03_BONIFICACAO ||
                    lstOperacoesPermitidas.IndexOf(
                        Constantes.Constantes.OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO.ToString()) != -1)
                {
                    var orcaTask = from c in db.TorcamentistaEindicadors
                                   where c.Status == "A"
                                   orderby c.Apelido
                                   select c.Apelido;

                    lstOrcaIndica = await orcaTask.ToListAsync();
                }
                else
                {
                    var orcaTask = from c in db.TorcamentistaEindicadors
                                   where c.Status == "A" && c.Vendedor == usuarioSistema
                                   orderby c.Apelido
                                   select c.Apelido;

                    lstOrcaIndica = await orcaTask.ToListAsync();
                }
            }

            return lstOrcaIndica;
        }

        private static bool IsLojaVrf(string loja)
        {
            bool retorno = false;

            if (loja == Constantes.Constantes.NUMERO_LOJA_VRF ||
                loja == Constantes.Constantes.NUMERO_LOJA_VRF2 ||
                loja == Constantes.Constantes.NUMERO_LOJA_VRF3 ||
                loja == Constantes.Constantes.NUMERO_LOJA_VRF4)
                retorno = true;

            return retorno;
        }

        public static async Task<IEnumerable<TorcamentistaEindicador>> BuscarOrcamentistaEIndicadorListaCompleta(LojaContextoBdProvider contexto,
            string usuarioSistema, string lstOperacoesPermitidas, string loja)
        {
            List<TorcamentistaEindicador> lstOrcaIndica = new List<TorcamentistaEindicador>();

            var db = contexto.GetContextoLeitura();

            if (Constantes.Constantes.ID_PARAM_SITE == Constantes.Constantes.COD_SITE_ASSISTENCIA_TECNICA)
            {
#pragma warning disable CS0162 // Unreachable code detected
                var orcaTask = from c in db.TorcamentistaEindicadors
#pragma warning restore CS0162 // Unreachable code detected
                               where c.Status == "A"
                               orderby c.Apelido
                               select c;

                lstOrcaIndica = await orcaTask.ToListAsync();
            }
            else
            {
                if (IsLojaVrf(loja) || loja == Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                {
                    var orcaTask = from c in db.TorcamentistaEindicadors
                                   where c.Status == "A" && c.Loja == loja
                                   orderby c.Apelido
                                   select c;

                    lstOrcaIndica = await orcaTask.ToListAsync();
                }
                else if (loja == Constantes.Constantes.NUMERO_LOJA_OLD03 ||
                    loja == Constantes.Constantes.NUMERO_LOJA_OLD03_BONIFICACAO ||
                    lstOperacoesPermitidas.IndexOf(
                        Constantes.Constantes.OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO.ToString()) != -1)
                {
                    var orcaTask = from c in db.TorcamentistaEindicadors
                                   where c.Status == "A"
                                   orderby c.Apelido
                                   select c;

                    lstOrcaIndica = await orcaTask.ToListAsync();
                }
                else
                {
                    var orcaTask = from c in db.TorcamentistaEindicadors
                                   where c.Status == "A" && c.Vendedor == usuarioSistema
                                   orderby c.Apelido
                                   select c;

                    lstOrcaIndica = await orcaTask.ToListAsync();
                }
            }

            return lstOrcaIndica;
        }



        public static string MontaLogInclusao(Object obj, string campos_a_omitir)
        {
            string retorno = "";

            PropertyInfo[] propriedades = obj.GetType().GetProperties();

            for (int i = 0; i < propriedades.Length; i++)
            {
                ColumnAttribute coluna = (ColumnAttribute)Attribute.GetCustomAttribute(propriedades[i], typeof(ColumnAttribute));

                if (coluna != null)
                {
                    if (!campos_a_omitir.Contains(coluna.Name))
                    {
                        var valor = propriedades[i].GetValue(obj, null);

                        if (valor != null)
                        {
                            retorno = retorno + coluna.Name + "=" + valor + "; ";
                        }
                    }
                }
            }

            return retorno;
        }

        public static string MontaLogExclusao(Object obj, string campos_a_omitir)
        {
            string retorno = "";

            PropertyInfo[] propriedades = obj.GetType().GetProperties();

            for (int i = 0; i < propriedades.Length; i++)
            {
                ColumnAttribute coluna = (ColumnAttribute)Attribute.GetCustomAttribute(propriedades[i], typeof(ColumnAttribute));

                if (coluna != null)
                {
                    if (!campos_a_omitir.Contains(coluna.Name))
                    {
                        var valor = propriedades[i].GetValue(obj, null);

                        if (valor != null)
                        {
                            retorno = retorno + coluna.Name + "=" + valor + "; ";
                        }
                    }
                }
            }

            return retorno;
        }
        public static string MontaLogAlteracao(Object objLog, Object objLogBase, string campos_a_omitir)
        {
            string retorno = "";

            PropertyInfo[] propriedades = objLog.GetType().GetProperties();
            PropertyInfo[] propriedadesBase = objLogBase.GetType().GetProperties();

            for (int i = 0; i < propriedadesBase.Length; i++)
            {

                ColumnAttribute coluna = (ColumnAttribute)Attribute.GetCustomAttribute(propriedades[i], typeof(ColumnAttribute));
                ColumnAttribute colunaBase = (ColumnAttribute)Attribute.GetCustomAttribute(propriedadesBase[i], typeof(ColumnAttribute));

                if (coluna != null)
                {

                    if (coluna.Name == colunaBase.Name)
                    {
                        if (!campos_a_omitir.Contains(coluna.Name))
                        {
                            var valor = propriedades[i].GetValue(objLog, null);
                            var valorBase = propriedadesBase[i].GetValue(objLogBase, null);

                            if (valor != null && valorBase != null)
                            {
                                if (!valor.Equals(valorBase))
                                {
                                    retorno = retorno + coluna.Name + ": " + valorBase + " => " + valor + "; ";
                                }
                            }
                        }
                    }
                }

            }

            return retorno;
        }

        public static string MontaLog(Object obj, string log, string campos_a_omitir)
        {
            return MontaLogInterno(obj, log, campos_a_omitir, null);
        }
        public static string MontaLogCamposIncluir(Object obj, string log, string campos_a_incluir)
        {
            return MontaLogInterno(obj, log, null, campos_a_incluir);
        }
        private static string MontaLogInterno(Object obj, string log, string campos_a_omitir, string campos_a_incluir)
        {
            PropertyInfo[] property = obj.GetType().GetProperties();
            campos_a_omitir = campos_a_omitir.ToLower();
            campos_a_incluir = campos_a_incluir.ToLower();

            foreach (var c in property)
            {
                //pegando o real nome da coluna 
                ColumnAttribute column = (ColumnAttribute)Attribute.GetCustomAttribute(c, typeof(ColumnAttribute));
                if (column != null)
                {
                    string coluna = column.Name;

                    bool incluir = false;

                    if (string.IsNullOrWhiteSpace(campos_a_incluir))
                    {
                        incluir = true;
                    }
                    else
                    {
                        if (campos_a_incluir.Contains(coluna.ToLower()))
                            incluir = true;
                    }

                    if (string.IsNullOrWhiteSpace(campos_a_omitir))
                    {
                        //nao fazemos nada
                    }
                    else
                    {
                        if (campos_a_omitir.Contains(coluna.ToLower()))
                            incluir = false;
                    }

                    if (incluir)
                    {
                        //pegando o valor coluna
                        var value = (c.GetValue(obj, null));
                        log = log + coluna + "=" + value + "; ";
                    }
                }
            }

            return log;
        }

        public static bool GravaLog(LojaContextoBdGravacao dbgravacao, string apelido, string loja, string pedido, string id_cliente,
            string operação, string log, LojaContextoBdProvider contexto)
        {
            if (apelido == null)
                return false;

            Tlog tLog = new Tlog
            {
                Data = DateTime.Now,
                Usuario = apelido,
                Loja = loja,
                Pedido = pedido,
                Id_Cliente = id_cliente,
                Operacao = operação,
                Complemento = log
            };

            dbgravacao.Add(tLog);
            //comentado para teste
            //dbgravacao.SaveChanges();

            return true;
        }

        public static bool ValidaCpf_Cnpj(string cpf_cnpj)
        {
            bool retorno = false;

            cpf_cnpj = cpf_cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

            if (cpf_cnpj.Length >= 11)
            {
                retorno = true;

                if (cpf_cnpj.Length >= 14)
                {
                    retorno = false;
                }
            }
            if (cpf_cnpj.Length < 11)
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool VerificaUf(string uf)
        {
            bool retorno = false;
            string sigla = "AC AL AM AP BA CE DF ES GO MA MG MS MT PA PB PE PI PR RJ RN RO RR RS SC SE SP TO  ";

            if (uf.Length == 2 && sigla.Contains(uf.ToUpper()))
                retorno = true;

            return retorno;
        }
        public static bool VerificaCep(string cep)
        {
            bool retorno = false;
            string cepFormat = "";

            if (cep != "")
            {
                cepFormat = cep.Replace("-", "");
                if (cepFormat.Length == 8)
                    retorno = true;
            }

            return retorno;
        }

        public static string MultiCdRegraDeterminaPessoa(string tipoCliente, byte contribuinteIcmsStatus, byte produtoRuralStatus)
        {
            string tipoPessoa = "";

            if (tipoCliente == Constantes.Constantes.ID_PF)
            {
                if (produtoRuralStatus == byte.Parse(Constantes.Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM))
                    tipoPessoa = Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL;
                else if (produtoRuralStatus == byte.Parse(Constantes.Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO))
                    tipoPessoa = Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA;
            }
            else if (tipoCliente == Constantes.Constantes.ID_PJ)
            {
                if (contribuinteIcmsStatus == byte.Parse(Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM))
                    tipoPessoa = Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE;
                else if (contribuinteIcmsStatus == byte.Parse(Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO))
                    tipoPessoa = Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE;
                else if (contribuinteIcmsStatus == byte.Parse(Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO))
                    tipoPessoa = Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO;
            }

            return tipoPessoa;
        }

        public static async Task ObterCtrlEstoqueProdutoRegra_Teste(List<string> lstErros,
            List<RegrasBll> lstRegrasCrtlEstoque, string uf, string cliente_regra, ILojaContextoBd db)
        {
            var dbTwmsRegraCdXUfXPessoaXCds = (from c in db.TwmsRegraCdXUfXPessoaXCds
                                               join nfe in db.TnfEmitentes on c.Id_nfe_emitente equals nfe.Id
                                               select c).ToList();



            //essa query esta copiando o id do produto 
            var testeRegras = from c in db.TprodutoXwmsRegraCds
                              join r1 in db.TwmsRegraCds on c.Id_wms_regra_cd equals r1.Id
                              join r2 in db.TwmsRegraCdXUfs on r1.Id equals r2.Id_wms_regra_cd
                              join r3 in db.TwmsRegraCdXUfPessoas on r2.Id equals r3.Id_wms_regra_cd_x_uf
                              where r2.Uf == uf &&
                                    r3.Tipo_pessoa == cliente_regra
                              orderby c.Produto
                              select new
                              {
                                  prod_x_reg = c,
                                  regra1 = r1,
                                  regra2 = r2,
                                  regra3 = r3//,
                                  //regra4 = dbTwmsRegraCdXUfXPessoaXCds.Where(r => r.Id_wms_regra_cd_x_uf_x_pessoa == r3.Id).ToList()
                              };
            var lista = await testeRegras.ToListAsync();

            RegrasBll itemRegra = new RegrasBll();

            foreach (var item in lstRegrasCrtlEstoque)
            {
                foreach (var r in lista)
                {
                    if (r.prod_x_reg.Produto == item.Produto)
                    {
                        item.St_Regra = true;
                        item.TwmsRegraCd = new t_WMS_REGRA_CD
                        {
                            Id = r.regra1.Id,
                            Apelido = r.regra1.Apelido,
                            Descricao = r.regra1.Descricao,
                            St_inativo = r.regra1.St_inativo

                        };
                        item.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
                        {
                            Id = r.regra2.Id,
                            Id_wms_regra_cd = r.regra2.Id_wms_regra_cd,
                            Uf = r.regra2.Uf,
                            St_inativo = r.regra2.St_inativo
                        };
                        item.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
                        {
                            Id = r.regra3.Id,
                            Id_wms_regra_cd_x_uf = r.regra3.Id_wms_regra_cd_x_uf,
                            Tipo_pessoa = r.regra3.Tipo_pessoa,
                            St_inativo = r.regra3.St_inativo,
                            Spe_id_nfe_emitente = r.regra3.Spe_id_nfe_emitente
                        };
                        item.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();

                        //foreach (var r4 in r.regra4)
                        foreach (var r4 in dbTwmsRegraCdXUfXPessoaXCds.Where(r2 => r2.Id_wms_regra_cd_x_uf_x_pessoa == r.regra3.Id).ToList())
                        {
                            t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
                            {
                                Id = r4.Id,
                                Id_wms_regra_cd_x_uf_x_pessoa = r4.Id_wms_regra_cd_x_uf_x_pessoa,
                                Id_nfe_emitente = r4.Id_nfe_emitente,
                                Ordem_prioridade = r4.Ordem_prioridade,
                                St_inativo = r4.St_inativo
                            };
                            item.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
                        }
                    }
                }
            }
        }

        public static async Task<string> ObterApelidoEmpresaNfeEmitentes(int id_nfe_emitente, ILojaContextoBd db)
        {
            string apelidoEmpresa = "";

            if (id_nfe_emitente == 0)
            {
                apelidoEmpresa = "Cliente";
                return apelidoEmpresa;
            }

            var apelidoTask = from c in db.TnfEmitentes
                              where c.Id == id_nfe_emitente
                              select c.Apelido;

            apelidoEmpresa = await apelidoTask.FirstOrDefaultAsync();

            return apelidoEmpresa;
        }

        public static string DescricaoMultiCDRegraTipoPessoa(string codTipoPessoa)
        {
            string retorno = "";

            codTipoPessoa = codTipoPessoa.ToUpper();

            switch (codTipoPessoa)
            {
                case Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA:
                    retorno = "Pessoa Física";
                    break;
                case Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL:
                    retorno = "Produtor Rural";
                    break;
                case Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE:
                    retorno = "PJ Contribuinte";
                    break;
                case Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE:
                    retorno = "PJ Não Contribuinte";
                    break;
                case Constantes.Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO:
                    retorno = "PJ Isento";
                    break;
            }

            return retorno;
        }

        public static void ObterDisponibilidadeEstoque(List<RegrasBll> lstRegrasCrtlEstoque, List<ProdutoDto> lst_produtos,
            List<string> lstErros, LojaContextoBdProvider contextoProvider, int id_nfe_emitente_selecao_manual)
        {
            id_nfe_emitente_selecao_manual = 0;

            foreach (var r in lstRegrasCrtlEstoque)
            {
                if (!string.IsNullOrEmpty(r.Produto))
                {
                    if (r.TwmsRegraCd != null)
                    {
                        foreach (var p in r.TwmsCdXUfXPessoaXCd)
                        {
                            if (p.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || p.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (p.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    foreach (var produto in lst_produtos)
                                    {
                                        if (r.Fabricante == produto.Fabricante && r.Produto == produto.Produto)
                                        {
                                            p.Estoque_Fabricante = produto.Fabricante;
                                            p.Estoque_Produto = produto.Produto;
                                            p.Estoque_DescricaoHtml = produto.Descricao_html;
                                            p.Estoque_Qtde_Solicitado = produto.QtdeSolicitada;
                                            p.Estoque_Qtde = 0;
                                            //if (!await EstoqueVerificaDisponibilidadeIntegralV2(p,
                                            //    produto, contextoProvider))
                                            //{
                                            //    lstErros.Add("Falha ao tentar consultar disponibilidade no estoque do produto (" +
                                            //        r.Fabricante + ")" + r.Produto);
                                            //}
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        public static async Task<bool> EstoqueVerificaDisponibilidadeIntegralV2(t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD regra,
            PedidoProdutosDtoPedido produto, ILojaContextoBd contextoBd)
        {
            bool retorno = false;
            if (regra.Estoque_Qtde_Solicitado > 0 && regra.Estoque_Produto != "")
            {
                var retornaRegra = await BuscarListaQtdeEstoque(regra, contextoBd);
                produto.Qtde_estoque_total_disponivel = retornaRegra.Estoque_Qtde_Estoque_Global;
                retorno = true;
            }
            return retorno;
        }

        public static async Task<float> ObterPercentualDesagioRAIndicador(string indicador, LojaContextoBdProvider contexto)
        {
            var db = contexto.GetContextoLeitura();
            var percDesagioIndicadorRATask = (from c in db.TorcamentistaEindicadors
                                              where c.Apelido == indicador
                                              select c.Perc_Desagio_RA).FirstOrDefaultAsync();

            float percDesagioIndicadorRA = await percDesagioIndicadorRATask ?? 0;

            return percDesagioIndicadorRA;
        }
        public static async Task<decimal> ObterLimiteMensalComprasDoIndicador(string indicador, LojaContextoBdProvider contexto)
        {
            var db = contexto.GetContextoLeitura();
            var vlLimiteMensalCompraTask = (from c in db.TorcamentistaEindicadors
                                            where c.Apelido == indicador
                                            select c.Vl_Limite_Mensal).FirstOrDefaultAsync();

            decimal vlLimiteMensalCompra = await vlLimiteMensalCompraTask;

            return vlLimiteMensalCompra;
        }

        public static async Task<decimal> CalcularLimiteMensalConsumidoDoIndicador(string indicador, DateTime data, LojaContextoBdProvider contexto)
        {
            //buscar data por ano-mes-dia]
            //SELECT ISNULL(SUM(qtde* preco_venda),0) AS vl_total
            //FROM t_PEDIDO tP INNER JOIN t_PEDIDO_ITEM tPI ON(tP.pedido= tPI.pedido)
            //WHERE(st_entrega <> 'CAN') AND
            //     (indicador = 'POLITÉCNIC') AND
            //     (data >= '2020-02-01') AND
            //     (data < '2020-03-01')

            var db = contexto.GetContextoLeitura();

            DateTime dataInferior = new DateTime(data.Year, data.Month, 1);
            DateTime dataSuperior = dataInferior.AddMonths(1);


            var vlTotalConsumidoTask = from c in db.TpedidoItems.Include(x => x.Tpedido)
                                       where c.Tpedido.St_Entrega != "CAN" &&
                                             c.Tpedido.Indicador == indicador &&
                                             c.Tpedido.Data.HasValue &&
                                             c.Tpedido.Data.Value.Date >= dataInferior &&
                                             c.Tpedido.Data.Value.Date < dataSuperior
                                       select new
                                       {
                                           qtde = c.Qtde,
                                           precoVenda = c.Preco_Venda
                                       };

            decimal vlTotalConsumido = await vlTotalConsumidoTask.SumAsync(x => (short)x.qtde * x.precoVenda);

            var vlTotalDevolvidoTask = from c in db.TpedidoItemDevolvidos.Include(x => x.Tpedido)
                                       where c.Tpedido.Indicador == indicador &&
                                             c.Tpedido.Data.HasValue &&
                                             c.Tpedido.Data.Value.Date >= dataInferior &&
                                             c.Tpedido.Data.Value.Date < dataSuperior
                                       select new
                                       {
                                           qtde = c.Qtde,
                                           precoVenda = c.Preco_Venda
                                       };
            decimal vlTotalDevolvido = await vlTotalDevolvidoTask.SumAsync(x => (short)x.qtde * x.precoVenda);

            decimal vlTotalConsumidoRetorno = vlTotalConsumido - vlTotalDevolvido;

            return vlTotalConsumidoRetorno;
        }

        public static async Task<float> VerificarSemDesagioRA(LojaContextoBdProvider contextoProvider)
        {//busca o percentual de RA sem desagio ID_PARAM_PERC_LIMITE_RA_SEM_DESAGIO            
            string semDesagio = await LeParametroControle(
                Constantes.Constantes.ID_PARAM_PERC_LIMITE_RA_SEM_DESAGIO, contextoProvider);

            float retorno = float.Parse(semDesagio);

            return retorno;
        }

        //public static async Task VerificarEstoque(List<RegrasBll> lst_cliente_regra, t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD produto,
        //    int id_nfe_emitente_selecao_manual, ContextoBdProvider contextoProvider)
        //{

        //    foreach (var regra in lst_cliente_regra)
        //    {
        //        if (regra.TwmsRegraCd != null)
        //        {
        //            foreach (var p1 in lst1)
        //            {
        //                foreach (var r in regra.TwmsCdXUfXPessoaXCd)
        //                {
        //                    if (r.Id_nfe_emitente != 0)
        //                    {

        //                        if (regra.Produto == p1.Produto)
        //                        {
        //                            //verificar se é inativo
        //                            if (r.St_inativo == 0)
        //                            {
        //                                var lst1 = await BuscarListaQtdeEstoque(produto, id_nfe_emitente_selecao_manual, contextoProvider);
        //                                //valor subtraido
        //                                r.Estoque_Qtde += (short)(p1.Qtde - p1.Qtde_Utilizada);

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}



        //alterando o metodo para recebee o Produto e o cd selecionado
        private static async Task<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD> BuscarListaQtdeEstoque(t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD regra,
            ILojaContextoBd contextoBd)
        {
            var db = contextoBd;
            List<ProdutosEstoqueDto> produtosEstoqueDtos = new List<ProdutosEstoqueDto>();
            int qtde = 0;
            int qtdeUtilizada = 0;
            int saldo = 0;

            if (regra.Estoque_Qtde_Solicitado > 0 && !string.IsNullOrEmpty(regra.Estoque_Produto))
            {
                var estoqueCDTask = (from c in db.TestoqueItems.Include(r => r.Testoque)
                                     where c.Testoque.Id_nfe_emitente == regra.Id_nfe_emitente &&
                                           c.Fabricante == regra.Estoque_Fabricante &&
                                           c.Produto == regra.Estoque_Produto &&
                                           (c.Qtde - c.Qtde_utilizada) > 0
                                     select new
                                     {
                                         qtde = (int)c.Qtde,
                                         qtdeUtilizada = (int)c.Qtde_utilizada
                                     });
                if (estoqueCDTask != null)
                {
                    qtde = await estoqueCDTask.SumAsync(x => x.qtde);
                    qtdeUtilizada = await estoqueCDTask.SumAsync(x => x.qtdeUtilizada);
                    saldo = qtde - qtdeUtilizada;
                    regra.Estoque_Qtde = (short)(qtde - qtdeUtilizada);


                    var estoqueGlobalTask = (from c in db.TestoqueItems.Include(r => r.Testoque)
                                             where c.Fabricante == regra.Estoque_Fabricante &&
                                                   c.Produto == regra.Estoque_Produto &&
                                                   (c.Qtde - c.Qtde_utilizada) > 0 &&
                                                   (c.Testoque.Id_nfe_emitente == regra.Id_nfe_emitente ||
                                                    db.TnfEmitentes.Where(r => r.St_Habilitado_Ctrl_Estoque == 1 && r.St_Ativo == 1)
                                                    .Select(r => r.Id).Contains(c.Testoque.Id_nfe_emitente))
                                             select new
                                             {
                                                 qtde = (int)c.Qtde,
                                                 qtdeUtilizada = (int)c.Qtde_utilizada
                                             });
                    qtde = await estoqueGlobalTask.SumAsync(x => x.qtde);
                    qtdeUtilizada = await estoqueGlobalTask.SumAsync(x => x.qtdeUtilizada);
                    saldo = qtde - qtdeUtilizada;
                    regra.Estoque_Qtde_Estoque_Global = (short)(qtde - qtdeUtilizada);
                }


            }

            return regra;
        }
        //private static async Task<IEnumerable<ProdutosEstoqueDto>> BuscarListaQtdeEstoque(ContextoBdProvider contextoProvider)
        //{
        //    var db = contextoProvider.GetContextoLeitura();

        //    var lstEstoqueQtdeUtilZero = from c in db.Testoques.Include(r => r.TestoqueItem)
        //                                 where (c.TestoqueItem.Qtde - c.TestoqueItem.Qtde_utilizada) > 0 &&
        //                                       c.TestoqueItem.Qtde_utilizada.HasValue
        //                                 select new ProdutosEstoqueDto
        //                                 {
        //                                     Produto = c.TestoqueItem.Produto,
        //                                     Qtde = (int)c.TestoqueItem.Qtde,
        //                                     Qtde_Utilizada = (int)c.TestoqueItem.Qtde_utilizada,
        //                                     Id_nfe_emitente = c.Id_nfe_emitente
        //                                 };

        //    List<ProdutosEstoqueDto> produtosEstoqueDtos = await lstEstoqueQtdeUtilZero.ToListAsync();

        //    return produtosEstoqueDtos;
        //}

        public static async Task VerificarEstoqueComSubQuery(List<RegrasBll> lst_cliente_regra, LojaContextoBdProvider contextoProvider)
        {
            var lst2 = await BuscarListaQtdeEstoqueComSubquery(contextoProvider);

            foreach (var regra in lst_cliente_regra)
            {
                if (regra.TwmsRegraCd != null)
                {
                    foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                    {
                        if (r.Id_nfe_emitente != 0)
                        {
                            foreach (var p2 in lst2)
                            {
                                if (regra.Produto == p2.Produto)
                                {
                                    //valor subtraido
                                    if (!r.Estoque_Qtde_Estoque_Global.HasValue)
                                        r.Estoque_Qtde_Estoque_Global = 0;
                                    r.Estoque_Qtde_Estoque_Global += (short)(p2.Qtde - p2.Qtde_Utilizada);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static async Task<IEnumerable<ProdutosEstoqueDto>> BuscarListaQtdeEstoqueComSubquery(LojaContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lstEstoqueQtdeUtilZeroComSubQuery = from c in db.TestoqueItems.Include(r => r.Testoque)
                                                    where ((c.Qtde - c.Qtde_utilizada) > 0) &&
                                                          ((c.Qtde_utilizada.HasValue) ||
                                                          (from d in db.TnfEmitentes
                                                           where d.St_Habilitado_Ctrl_Estoque == 1 && d.St_Ativo == 1
                                                           select d.Id)
                                                           .Contains(c.Testoque.Id_nfe_emitente))
                                                    select new ProdutosEstoqueDto
                                                    {
                                                        Produto = c.Produto,
                                                        Qtde = (int)c.Qtde,
                                                        Qtde_Utilizada = (int)c.Qtde_utilizada,
                                                        Id_nfe_emitente = c.Testoque.Id_nfe_emitente
                                                    };

            List<ProdutosEstoqueDto> produtosEstoqueDtos = await lstEstoqueQtdeUtilZeroComSubQuery.ToListAsync();

            return produtosEstoqueDtos;
        }

        public static async Task<bool> IsActivatedFlagPedidoUsarMemorizacaoCompletaEnderecos(ILojaContextoBd contexto)
        {
            bool retorno = false;

            Tparametro tparametro = await BuscarRegistroParametro(
                Constantes.Constantes.ID_PARAMETRO_Flag_Pedido_MemorizacaoCompletaEnderecos, contexto);

            if (tparametro != null)
            {
                if (tparametro.Campo_inteiro == 1)
                    retorno = true;
            }

            return retorno;
        }

        public static string TransformaHora_Minutos()
        {
            string hora = DateTime.Now.Hour.ToString().PadLeft(2, '0');
            string minuto = DateTime.Now.AddMinutes(-10).ToString().PadLeft(2, '0');

            return hora + minuto;
        }

        public static async Task<Tparametro> BuscarRegistroParametro(string id, ILojaContextoBd contexto)
        {
            var db = contexto;

            var parametroTask = from c in db.Tparametros
                                where c.Id == id
                                select c;

            Tparametro parametro = await parametroTask.FirstOrDefaultAsync();

            return parametro;
        }

        public static async Task<IEnumerable<Tparametro>> BuscarRegistroParametroLista(string id, LojaContextoBdProvider contexto)
        {
            var db = contexto.GetContextoLeitura();

            var parametroTask = from c in db.Tparametros
                                where c.Id == id
                                select c;

            var parametro = await parametroTask.ToListAsync();

            return parametro;
        }

        public static DateTime LimiteDataBuscas()
        {
            DateTime data = new DateTime();
            data = DateTime.Now.AddDays(-60);
            return data;
        }

        public static async Task<string> ObterDescricao_Cod(string grupo, string cod, LojaContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var desc = from c in db.TcodigoDescricaos
                       where c.Grupo == grupo && c.Codigo == cod
                       select c.Descricao;

            string result = await desc.FirstOrDefaultAsync();

            if (result == null || result == "")
                return "Código não cadastrado (" + cod + ")";

            return result;
        }
        public static string OpcaoFormaPagto(string codigo)
        {
            string retorno = "";

            switch (codigo)
            {
                case Constantes.Constantes.ID_FORMA_PAGTO_DINHEIRO:
                    retorno = "Dinheiro";
                    break;
                case Constantes.Constantes.ID_FORMA_PAGTO_DEPOSITO:
                    retorno = "Depósito";
                    break;
                case Constantes.Constantes.ID_FORMA_PAGTO_CHEQUE:
                    retorno = "Cheque";
                    break;
                case Constantes.Constantes.ID_FORMA_PAGTO_BOLETO:
                    retorno = "Boleto";
                    break;
                case Constantes.Constantes.ID_FORMA_PAGTO_CARTAO:
                    retorno = "Cartão (internet)";
                    break;
                case Constantes.Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA:
                    retorno = "Cartão (maquineta)";
                    break;
                case Constantes.Constantes.ID_FORMA_PAGTO_BOLETO_AV:
                    retorno = "Boleto AV";
                    break;
            };

            return retorno;
        }

        public static async Task<IEnumerable<Tdesconto>> BuscarListaIndicadoresLoja(string cliente_id, string loja,
            LojaContextoBdProvider contexto)
        {
            var db = contexto.GetContextoLeitura();

            List<Tdesconto> lst_tdesconto = await (from c in db.Tdescontos
                                                   where c.Usado_status == 0 &&
                                                         c.Cancelado_status == 0 &&
                                                         c.Id_cliente == cliente_id &&
                                                         c.Loja == loja &&
                                                         c.Data >= DateTime.Now.AddMinutes(-30)
                                                   orderby c.Fabricante, c.Produto, c.Data descending
                                                   select c).ToListAsync();
            return lst_tdesconto;
        }

        public static async Task<string> LeParametroControle(string id, LojaContextoBdProvider contextoBdProvider)
        {
            var db = contextoBdProvider.GetContextoLeitura();

            string controle = await (from c in db.Tcontroles
                                     where c.Id_Nsu == id
                                     select c.Nsu).FirstOrDefaultAsync();


            return controle;

        }

        public static async Task<int> Fin_gera_nsu(string id_nsu, List<string> lstErros, LojaContextoBdGravacao dbgravacao)
        {
            int intRetorno = 0;
            //int intRecordsAffected = 0;
            //int intQtdeTentativas, intNsuUltimo, intNsuNovo;
            //bool blnSucesso = true;
            int nsu = 0;

            //conta a qtde de id
            var qtdeIdFin = from c in dbgravacao.TfinControles
                            where c.Id == id_nsu
                            select c.Id;


            if (qtdeIdFin != null)
            {
                intRetorno = await qtdeIdFin.CountAsync();
            }

            //não está cadastrado, então cadastra agora 
            if (intRetorno == 0)
            {
                //criamos um novo para salvar
                TfinControle tfinControle = new TfinControle();

                tfinControle.Id = id_nsu;
                tfinControle.Nsu = 0;
                tfinControle.Dt_hr_ult_atualizacao = DateTime.Now;

                dbgravacao.Add(tfinControle);

            }

            //laço de tentativas para gerar o nsu(devido a acesso concorrente)


            //obtém o último nsu usado
            var tfincontroleEditando = await (from c in dbgravacao.TfinControles
                                              where c.Id == id_nsu
                                              select c).FirstOrDefaultAsync();


            if (tfincontroleEditando == null)
            {
                lstErros.Add("Falha ao localizar o registro para geração de NSU (" + id_nsu + ")!");
                return nsu;
            }


            tfincontroleEditando.Id = id_nsu;
            tfincontroleEditando.Nsu++;
            tfincontroleEditando.Dt_hr_ult_atualizacao = DateTime.Now;
            //tenta atualizar o banco de dados
            dbgravacao.Update(tfincontroleEditando);

            await dbgravacao.SaveChangesAsync();

            return tfincontroleEditando.Nsu;
        }

        public static async Task<TtransportadoraCep> ObterTransportadoraPeloCep(string cep, ILojaContextoBd contextoDb)
        {
            cep = cep.Replace("-", "").Trim();

            int cepteste = int.Parse(cep);
            cep = cepteste.ToString();
            var db = contextoDb;



            TtransportadoraCep transportadoraCep = await (from c in db.TtransportadoraCeps
                                                          where (c.Tipo_range == 1 && c.Cep_unico == cep) ||
                                                                (
                                                                    c.Tipo_range == 2 &&
                                                                     (
                                                                         c.Cep_faixa_inicial.CompareTo(cep) <= 0 &&
                                                                         c.Cep_faixa_final.CompareTo(cep) >= 0
                                                                      )
                                                                )
                                                          select c).FirstOrDefaultAsync();

            return transportadoraCep;
        }

        //afazer melhorar esse metodo
        public static async Task<IEnumerable<RegrasBll>> Buscar_IdCDselecionado(PedidoProdutosDtoPedido produto, Tcliente cliente,
            int id_nfe_emitente_selecao_manual,
            ProdutoValidadoComEstoqueDto prodValidadoEstoque, ILojaContextoBd contextoBd)
        {
            string cliente_regra = MultiCdRegraDeterminaPessoa(cliente.Tipo,
                cliente.Contribuinte_Icms_Status, cliente.Produtor_Rural_Status);


            List<RegrasBll> regraCrtlEstoque = (await
                Loja.Bll.ProdutoBll.ProdutoBll.ObterCtrlEstoqueProdutoRegraParaUMProduto(
                produto, cliente, prodValidadoEstoque.ListaErros, contextoBd)).ToList();

            await ObterCtrlEstoqueProdutoRegra_Teste(prodValidadoEstoque.ListaErros, regraCrtlEstoque, cliente.Uf,
                cliente_regra, contextoBd);

            Loja.Bll.ProdutoBll.ProdutoBll.VerificarRegrasAssociadasParaUMProduto(regraCrtlEstoque,
                prodValidadoEstoque.ListaErros, cliente, id_nfe_emitente_selecao_manual);

            if (id_nfe_emitente_selecao_manual != 0)
                await Loja.Bll.ProdutoBll.ProdutoBll.VerificarCDHabilitadoTodasRegras(regraCrtlEstoque,
                    id_nfe_emitente_selecao_manual, prodValidadoEstoque.ListaErros, contextoBd);

            await Loja.Bll.ProdutoBll.ProdutoBll.ObterDisponibilidadeEstoque(regraCrtlEstoque, produto,
                prodValidadoEstoque.ListaErros, id_nfe_emitente_selecao_manual, contextoBd);

            return regraCrtlEstoque;
        }

        public static async Task<TorcamentistaEindicador> BuscarOrcamentistaEIndicador(string indicador,
            ILojaContextoBd contextoBd)
        {
            var db = contextoBd;

            TorcamentistaEindicador torcamentista = await (from c in db.TorcamentistaEindicadors
                                                           where c.Apelido == indicador
                                                           select c).FirstOrDefaultAsync();

            return torcamentista;
        }

        public static bool ValidarTipoCustoFinanceiroFornecedor(List<string> lstErros, string custoFinanceiroTipoParcelato,
            int c_custoFinancFornecQtdeParcelas)
        {
            bool retorno = true;

            if (custoFinanceiroTipoParcelato != Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA &&
                custoFinanceiroTipoParcelato != Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                custoFinanceiroTipoParcelato != Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
            {
                lstErros.Add("A forma de pagamento não foi informada (à vista, com entrada, sem entrada).");
                retorno = false;
            }
            if (custoFinanceiroTipoParcelato != Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                custoFinanceiroTipoParcelato != Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
            {

                if (c_custoFinancFornecQtdeParcelas <= 0)
                {
                    lstErros.Add("Não foi informada a quantidade de parcelas para a forma de pagamento selecionada " +
                        "(" + DescricaoCustoFornecTipoParcelamento(custoFinanceiroTipoParcelato) + ")");
                    retorno = false;
                }
            }

            return retorno;
        }

        public static string DescricaoCustoFornecTipoParcelamento(string custoFinanceiro)
        {
            string retorno = "";

            switch (custoFinanceiro)
            {
                case Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA:
                    retorno = "Com Entrada";
                    break;
                case Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA:
                    retorno = "Sem Entrada";
                    break;
                case Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA:
                    retorno = "À Vista";
                    break;
            }

            return retorno;
        }


        public static async Task<bool> Grava_log_estoque_v2(string strUsuario, short id_nfe_emitente, string strFabricante,
            string strProduto, short intQtdeSolicitada, short intQtdeAtendida, string strOperacao,
            string strCodEstoqueOrigem, string strCodEstoqueDestino, string strLojaEstoqueOrigem,
            string strLojaEstoqueDestino, string strPedidoEstoqueOrigem, string strPedidoEstoqueDestino,
            string strDocumento, string strComplemento, string strIdOrdemServico, LojaContextoBdGravacao contexto)
        {

            TestoqueLog testoqueLog = new TestoqueLog();

            testoqueLog.data = DateTime.Now.Date;
            testoqueLog.Data_hora = DateTime.Now;
            testoqueLog.Usuario = strUsuario;
            testoqueLog.Id_nfe_emitente = id_nfe_emitente;
            testoqueLog.Fabricante = strFabricante;
            testoqueLog.Produto = strProduto;
            testoqueLog.Qtde_solicitada = intQtdeSolicitada;
            testoqueLog.Qtde_atendida = intQtdeAtendida;
            testoqueLog.Operacao = strOperacao;
            testoqueLog.Cod_estoque_origem = strCodEstoqueOrigem;
            testoqueLog.Cod_estoque_destino = strCodEstoqueDestino;
            testoqueLog.Loja_estoque_origem = strLojaEstoqueOrigem;
            testoqueLog.Loja_estoque_destino = strLojaEstoqueDestino;
            testoqueLog.Pedido_estoque_origem = strPedidoEstoqueOrigem;
            testoqueLog.Pedido_estoque_destino = strPedidoEstoqueDestino;
            testoqueLog.Documento = strDocumento;
            testoqueLog.Complemento = strComplemento.Length > 80 ? strComplemento.Substring(0, 80) : strComplemento;
            testoqueLog.Id_ordem_servico = strIdOrdemServico;

            contexto.Add(testoqueLog);
            await contexto.SaveChangesAsync();


            return true;
        }
    }
}
