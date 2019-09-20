﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using PrepedidoBusiness.Bll.Regras;

namespace PrepedidoBusiness.Utils
{
    public class Util
    {
        //private readonly InfraBanco.ContextoProvider contextoProvider;

        //public Util(InfraBanco.ContextoProvider contextoProvider)
        //{
        //    this.contextoProvider = contextoProvider;
        //}

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

        public static bool ValidaCpf_Cnpj(string cpf_cnpj)
        {
            bool retorno = false;
            string formatCpf_Cnpj = "";

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

        public static string ObterDescricao_Cod(string grupo, string cod, ContextoProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            var desc = from c in db.TcodigoDescricaos
                       where c.Grupo == grupo && c.Codigo == cod
                       select c.Descricao;

            string result = desc.FirstOrDefault();

            if (result == null || result == "")
                return "Código não cadastrado (" + cod + ")";

            return result;
        }

        public static string OpcaoFormaPagto(string codigo)
        {
            string retorno = "";

            switch (codigo)
            {
                case Constantes.ID_FORMA_PAGTO_DINHEIRO:
                    retorno = "Dinheiro";
                    break;
                case Constantes.ID_FORMA_PAGTO_DEPOSITO:
                    retorno = "Depósito";
                    break;
                case Constantes.ID_FORMA_PAGTO_CHEQUE:
                    retorno = "Cheque";
                    break;
                case Constantes.ID_FORMA_PAGTO_BOLETO:
                    retorno = "Boleto";
                    break;
                case Constantes.ID_FORMA_PAGTO_CARTAO:
                    retorno = "Cartão (internet)";
                    break;
                case Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA:
                    retorno = "Cartão (maquineta)";
                    break;
                case Constantes.ID_FORMA_PAGTO_BOLETO_AV:
                    retorno = "Boleto AV";
                    break;
            };

            return retorno;
        }

        public static bool VerificaUf(string uf)
        {
            bool retorno = false;
            string sigla = "AC AL AM AP BA CE DF ES GO MA MG MS MT PA PB PE PI PR RJ RN RO RR RS SC SE SP TO  ";

            if (uf.Length == 2)
                if (sigla.Contains(uf.ToUpper()))
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

        public static string GeraChave(int fator)
        {
            const int cod_min = 35;
            const int cod_max = 96;
            const int tamanhoChave = 128;

            string chave = "";

            for (int i = 1; i < tamanhoChave; i++)
            {
                int k = (cod_max - cod_min) + 1;
                k *= fator;
                k = (k * i) + cod_min;
                k %= 128;
                chave += (char)k;
            }

            return chave;
        }

        public static string DecodificaSenha(string origem, string chave)
        {
            string s_destino = "";
            string s_origem = origem;
            int i = s_origem.Length - 2;

            s_origem = s_origem.Substring(s_origem.Length - i, i);
            s_origem = s_origem.ToUpper();

            string s;
            string codificar = "";

            for (i = 1; i <= s_origem.Length; i++)
            {
                s = s_origem.Substring((i - 1), 2);
                if (s != "00")
                {
                    codificar = s_origem.Substring((i - 1), (s_origem.Length - i + 1)); ;
                    break;
                }
                i++;
            }

            for (i = 0; i < codificar.Length; i++)
            {
                s = codificar.Substring(i, 2);
                int hexNumber = int.Parse(s, System.Globalization.NumberStyles.HexNumber);
                s_destino += (char)(hexNumber);
                i++;
            }
            s_origem = s_destino;
            s_destino = "";

            string letra;

            for (i = 0; i < s_origem.Length; i++)
            {
                //pega a letra
                letra = chave.Substring(i, 1);
                //Converte para char
                int i_chave = (Convert.ToChar(letra) * 2) + 1;
                int i_dado = Convert.ToChar(s_origem.Substring(i, 1));

                int contaMod = i_chave ^ i_dado;
                contaMod /= 2;
                s_destino += (char)contaMod;
            }

            return s_destino;
        }

        /*afazer: Precisa pedir acesso
         * servidor: Win2008R2BS,29981
         * BD: CEP_Homologacao
         */
        //public static string BuscarCep(string tipo)


        public static string MontaLog(Object obj, string log, string campos_a_omitir)
        {
            PropertyInfo[] property = obj.GetType().GetProperties();
            string[] campos = campos_a_omitir.Split('|');

            foreach (var c in property)
            {
                //pegando o real nome da coluna 
                ColumnAttribute column = (ColumnAttribute)Attribute.GetCustomAttribute(c, typeof(ColumnAttribute));
                string coluna = column.Name;
                if (!campos_a_omitir.Contains(coluna))
                {
                    //pegando o valor coluna
                    var value = (c.GetValue(obj, null));
                    log = log + coluna + "=" + value + "; ";
                }

            }

            return log;
        }
        public static bool GravaLog(string apelido, string loja, string pedido, string id_cliente,
            string operação, string log, ContextoProvider contexto)
        {
            if (apelido == null)
                return false;

            var db = contexto.GetContextoGravacao();

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

            db.Add(tLog);
            db.SaveChanges();

            return true;
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

        public static bool ValidarTipoCustoFinanceiroFornecedor(List<string> lstErros, string custoFinanceiroTipoParcelato)
        {
            bool retorno = true;

            if (custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA &&
                custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
            {
                lstErros.Add("A forma de pagamento não foi informada (à vista, com entrada, sem entrada).");
                retorno = false;
            }
            if (custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
            {
                if (int.Parse(custoFinanceiroTipoParcelato) <= 0)
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
                case Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA:
                    retorno = "Com Entrada";
                    break;
                case Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA:
                    retorno = "Sem Entrada";
                    break;
                case Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA:
                    retorno = "À Vista";
                    break;
            }

            return retorno;
        }

        public static async Task<string> GerarNsu(string id_nsu, ContextoProvider contextoProvider)
        {
            string retorno = "";
            int n_nsu = -1;
            string s = "0";
            int asc;
            char chr;

            var db = contextoProvider.GetContextoGravacao();

            if (id_nsu == "")
                retorno = "Não foi especificado o NSU a ser gerado!!";

            for (int i = 0; i <= 100; i++)
            {
                var ret = from c in db.Tcontroles
                          where c.Id_Nsu == id_nsu
                          select c;

                var controle = await ret.FirstOrDefaultAsync();


                if (!string.IsNullOrEmpty(controle.Nsu))
                {
                    if (controle.Seq_Anual != 0)
                    {
                        if (DateTime.Now.Year > controle.Dt_Ult_Atualizacao.Year)
                        {
                            s = Utils.Util.Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
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
                s = Utils.Util.Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
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
                        //db.Update(controle);
                        //await db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        retorno = "Não foi possível gerar o NSU, pois ocorreu o seguinte erro: " + ex.HResult + ":" + ex.Message;
                    }
                }
            }

            return retorno;
        }

        public static bool LojaHabilitadaProdutosECommerce(string loja)
        {
            bool retorno = false;

            if (loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_BONSHOP)
                retorno = true;
            if (IsLojaVrf(loja))
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_MARCELO_ARTVEN)
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_BONSHOP_LAB)
                retorno = true;

            return retorno;
        }

        private static bool IsLojaVrf(string loja)
        {
            bool retorno = false;

            if (loja == Constantes.NUMERO_LOJA_VRF ||
                loja == Constantes.NUMERO_LOJA_VRF2 ||
                loja == Constantes.NUMERO_LOJA_VRF3 ||
                loja == Constantes.NUMERO_LOJA_VRF4)
                retorno = true;

            return retorno;
        }

        public static string MultiCdRegraDeterminaPessoa(string tipoCliente, byte contribuinteIcmsStatus, byte produtoRuralStatus)
        {
            string tipoPessoa = "";

            if (tipoCliente == Constantes.ID_PF)
            {
                if (produtoRuralStatus == byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL;
                else if (produtoRuralStatus == byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA;
            }
            else if (tipoCliente == Constantes.ID_PJ)
            {
                if (contribuinteIcmsStatus == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE;
                else if (contribuinteIcmsStatus == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE;
                else if (contribuinteIcmsStatus == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO))
                    tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO;
            }

            return tipoPessoa;
        }

        public static string DescricaoMultiCDRegraTipoPessoa(string codTipoPessoa)
        {
            string retorno = "";

            codTipoPessoa = codTipoPessoa.ToUpper();

            switch (codTipoPessoa)
            {
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA:
                    retorno = "Pessoa Física";
                    break;
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL:
                    retorno = "Produtor Rural";
                    break;
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE:
                    retorno = "PJ Contribuinte";
                    break;
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE:
                    retorno = "PJ Não Contribuinte";
                    break;
                case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO:
                    retorno = "PJ Isento";
                    break;
            }

            return retorno;
        }

        public static async Task<string> ObterApelidoEmpresaNfeEmitentes(int id_nfe_emitente, ContextoProvider contextoProvider)
        {
            string apelidoEmpresa = "";

            if (id_nfe_emitente == 0)
            {
                apelidoEmpresa = "Cliente";
                return apelidoEmpresa;
            }

            var db = contextoProvider.GetContextoLeitura();

            var apelidoTask = from c in db.TnfEmitentes
                              where c.Id == id_nfe_emitente
                              select c.Apelido;

            apelidoEmpresa = await apelidoTask.FirstOrDefaultAsync();

            return apelidoEmpresa;
        }


        //Na verdade recebe uma lista de produtos e não o PrepedidoDto
        //public static async Task<IEnumerable<RegrasBll>> ObterCtrlEstoqueProdutoRegra(PrePedidoDto prePedido, List<string> lstErros,
        //    ContextoProvider contextoProvider)
        //{
        //    List<RegrasBll> lstRegrasCrtlEstoque = new List<RegrasBll>();

        //    var db = contextoProvider.GetContextoLeitura();

        //    foreach (var item in prePedido.ListaProdutos)
        //    {
        //        var regraProdutoTask = from c in db.TprodutoXwmsRegraCds
        //                               where c.Fabricante == item.Fabricante &&
        //                                     c.Produto == item.NumProduto
        //                               select c;

        //        var regra = await regraProdutoTask.FirstOrDefaultAsync();

        //        if (regra == null)
        //        {
        //            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
        //                Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
        //                item.NumProduto + " não possui regra associada");
        //        }
        //        else
        //        {
        //            if (regra.Id_wms_regra_cd == 0)
        //                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
        //                    Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
        //                    item.NumProduto + " não está associado a nenhuma regra");
        //            else
        //            {
        //                var wmsRegraTask = from c in db.TwmsRegraCds
        //                                   where c.Id == regra.Id_wms_regra_cd
        //                                   select c;

        //                var wmsRegra = await wmsRegraTask.FirstOrDefaultAsync();
        //                if (wmsRegra == null)
        //                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
        //                        Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
        //                        item.NumProduto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
        //                else
        //                {

        //                    //fazer montagem dos dados apartir daqui
        //                    RegrasBll itemRegra = new RegrasBll();
        //                    itemRegra.Fabricante = item.Fabricante;
        //                    itemRegra.Produto = item.NumProduto;

        //                    itemRegra.TwmsRegraCd = new t_WMS_REGRA_CD
        //                    {
        //                        Id = wmsRegra.Id,
        //                        Apelido = wmsRegra.Apelido,
        //                        Descricao = wmsRegra.Descricao,
        //                        St_inativo = wmsRegra.St_inativo
        //                    };

        //                    var wmsRegraCdXUfTask = from c in db.TwmsRegraCdXUfs
        //                                            where c.Id_wms_regra_cd == itemRegra.TwmsRegraCd.Id &&
        //                                                  c.Uf == prePedido.DadosCliente.Uf
        //                                            select c;
        //                    var wmsRegraCdXUf = await wmsRegraCdXUfTask.FirstOrDefaultAsync();

        //                    if (wmsRegraCdXUf == null)
        //                    {
        //                        itemRegra.St_Regra = false;
        //                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
        //                            Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
        //                            item.NumProduto + " não está cadastrada para a UF '" + prePedido.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
        //                    }
        //                    else
        //                    {
        //                        itemRegra.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
        //                        {
        //                            Id = wmsRegraCdXUf.Id,
        //                            Id_wms_regra_cd = wmsRegraCdXUf.Id_wms_regra_cd,
        //                            Uf = wmsRegraCdXUf.Uf,
        //                            St_inativo = wmsRegraCdXUf.St_inativo
        //                        };

        //                        var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
        //                                                       where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
        //                                                             c.Tipo_pessoa == prePedido.DadosCliente.Tipo
        //                                                       select c;

        //                        var wmsRegraCdXUfXPessoa = await wmsRegraCdXUfXPessoaTask.FirstOrDefaultAsync();

        //                        if (wmsRegraCdXUfXPessoa == null)
        //                        {
        //                            itemRegra.St_Regra = false;
        //                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
        //                                Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
        //                                item.NumProduto + " não está cadastrada para a UF '" + prePedido.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
        //                        }
        //                        else
        //                        {
        //                            itemRegra.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
        //                            {
        //                                Id = wmsRegraCdXUfXPessoa.Id,
        //                                Id_wms_regra_cd_x_uf = wmsRegraCdXUfXPessoa.Id_wms_regra_cd_x_uf,
        //                                Tipo_pessoa = wmsRegraCdXUfXPessoa.Tipo_pessoa,
        //                                St_inativo = wmsRegraCdXUfXPessoa.St_inativo,
        //                                Spe_id_nfe_emitente = wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
        //                            };

        //                            if (wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
        //                            {
        //                                itemRegra.St_Regra = false;
        //                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
        //                                    Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
        //                                    item.NumProduto + " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id=" + regra.Id_wms_regra_cd + ")");
        //                            }
        //                            else
        //                            {
        //                                var nfEmitenteTask = from c in db.TnfEmitentes
        //                                                     where c.Id == wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
        //                                                     select c;
        //                                var nfEmitente = await nfEmitenteTask.FirstOrDefaultAsync();

        //                                if (nfEmitente != null)
        //                                {
        //                                    if (nfEmitente.St_Ativo != 1)
        //                                    {
        //                                        itemRegra.St_Regra = false;
        //                                        lstErros.Add("Falha na regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
        //                                            Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante +
        //                                            ")" + item.NumProduto + " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
        //                                            "(Id=" + regra.Id_wms_regra_cd + ")");
        //                                    }
        //                                }
        //                                var wmsRegraCdXUfXPessoaXcdTask = from c in db.TwmsRegraCdXUfXPessoaXCds
        //                                                                  where c.Id_wms_regra_cd_x_uf_x_pessoa == wmsRegraCdXUfXPessoa.Id
        //                                                                  orderby c.Ordem_prioridade
        //                                                                  select c;
        //                                var wmsRegraCdXUfXPessoaXcd = await wmsRegraCdXUfXPessoaXcdTask.ToListAsync();

        //                                if (wmsRegraCdXUfXPessoaXcd.Count == 0)
        //                                {
        //                                    itemRegra.St_Regra = false;
        //                                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
        //                                        Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
        //                                        item.NumProduto + " não especifica nenhum CD para consumo do estoque (Id=" + regra.Id_wms_regra_cd + ")");
        //                                }
        //                                else
        //                                {
        //                                    foreach (var i in wmsRegraCdXUfXPessoaXcd)
        //                                    {
        //                                        t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
        //                                        {
        //                                            Id = i.Id,
        //                                            Id_wms_regra_cd_x_uf_x_pessoa = i.Id_wms_regra_cd_x_uf_x_pessoa,
        //                                            Id_nfe_emitente = i.Id_nfe_emitente,
        //                                            Ordem_prioridade = i.Ordem_prioridade,
        //                                            St_inativo = i.St_inativo
        //                                        };

        //                                        var nfeCadastroPrincipalTask = from c in db.TnfEmitentes
        //                                                                       where c.Id == item_cd_uf_pess_cd.Id_nfe_emitente
        //                                                                       select c;

        //                                        var nfeCadastroPrincipal = await nfeCadastroPrincipalTask.FirstOrDefaultAsync();

        //                                        if (nfeCadastroPrincipal != null)
        //                                        {
        //                                            if (nfeCadastroPrincipal.St_Ativo != 1)
        //                                                item_cd_uf_pess_cd.St_inativo = 1;
        //                                        }

        //                                        itemRegra.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
        //                                    }
        //                                    foreach (var i in itemRegra.TwmsCdXUfXPessoaXCd)
        //                                    {

        //                                        if (i.Id_nfe_emitente == itemRegra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
        //                                        {
        //                                            if (i.St_inativo == 1)
        //                                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
        //                                                    Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" +
        //                                                    item.Fabricante + ")" + item.NumProduto + " especifica o CD '" + ObterApelidoEmpresaNfeEmitentes(wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente) +
        //                                                    "' para alocação de produtos sem presença no estoque, sendo que este CD está desativado para " +
        //                                                    "processar produtos disponíveis (Id=" + regra.Id_wms_regra_cd + ")");
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    lstRegrasCrtlEstoque.Add(itemRegra);
        //                }
        //            }
        //        }

        //    }

        //    return lstRegrasCrtlEstoque;
        //}

    }
}
