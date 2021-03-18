using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InfraBanco.Modelos;
using InfraBanco.Constantes;
using Produto.RegrasCrtlEstoque;
using UtilsGlobais;
using Produto;
using InfraBanco;

#nullable enable

namespace Pedido
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0054:Use compound assignment", Justification = "Estilo de código")]
    public class PedidoBll
    {
        //todo: limpar esta classe
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly ProdutoGeralBll produtoGeralBll;
        public readonly Prepedido.PrepedidoBll PrepedidoBll;

        public PedidoBll(InfraBanco.ContextoBdProvider contextoProvider,
            ProdutoGeralBll produtoGeralBll,
            Prepedido.PrepedidoBll prepedidoBll)
        {
            this.contextoProvider = contextoProvider;
            this.produtoGeralBll = produtoGeralBll;
            this.PrepedidoBll = prepedidoBll;
        }

     
        public async Task<float> BuscarCoeficientePercentualCustoFinanFornec(Criacao.PedidoCriacao criacao,
            PedidoCriacaoDados pedido, short qtdeParcelas, string siglaPagto, List<string> lstErros)
        {
            float coeficiente = 0;

            var db = contextoProvider.GetContextoLeitura();

            if (siglaPagto == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
                coeficiente = 1;
            else
            {
                foreach (var i in pedido.ListaProdutos)
                {
                    var percCustoTask = from c in criacao.Execucao.TabelasBanco.TpercentualCustoFinanceiroFornecedors_Coeficiente
                                        where c.Fabricante == i.Fabricante &&
                                              c.Tipo_Parcelamento == siglaPagto &&
                                              c.Qtde_Parcelas == qtdeParcelas
                                        select new { c.Coeficiente };

                    var percCusto = percCustoTask.FirstOrDefault();

                    if (percCusto != null)
                    {
                        coeficiente = percCusto.Coeficiente;
                        if (i.Preco_Lista != (decimal)coeficiente * (i.CustoFinancFornecPrecoListaBase_Conferencia))
                            lstErros.Add("Preco_Lista inconsistente");
                    }
                    else
                    {
                        lstErros.Add("Opção de parcelamento não disponível para fornecedor " + i.Fabricante + ": " +
                            Prepedido.PrepedidoBll.DecodificaCustoFinanFornecQtdeParcelas(pedido.FormaPagtoCriacao.C_forma_pagto, qtdeParcelas) + " parcela(s)");
                    }

                }
            }

            return coeficiente;
        }


        //se mantiver, tentar unificar com Prepedido.PrepedidoBll.ObterCtrlEstoqueProdutoRegra
        public async Task<IEnumerable<RegrasBll>> ObterCtrlEstoqueProdutoRegraParaUMProduto(PedidoCriacaoProdutoDados produto,
            Tcliente tcliente, List<string> lstErros)
        {
            List<RegrasBll> lstRegrasCrtlEstoque = new List<RegrasBll>();

            var db = contextoProvider.GetContextoLeitura();

            //vamos verificar passando todos os produtos simples da lista de produto que irá para ser selecionado

            var regraProdutoTask = from c in db.TprodutoXwmsRegraCds
                                   where c.Fabricante == produto.Fabricante &&
                                         c.Produto == produto.Produto
                                   select c;

            var regra = await regraProdutoTask.FirstOrDefaultAsync();

            if (regra == null)
            {
                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                    Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': produto (" + produto.Fabricante + ")" +
                    produto.Produto + " não possui regra associada");
            }
            else
            {
                if (regra.Id_wms_regra_cd == 0)
                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                        Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': produto (" + produto.Fabricante + ")" +
                        produto.Produto + " não está associado a nenhuma regra");
                else
                {
                    var wmsRegraTask = from c in db.TwmsRegraCds
                                       where c.Id == regra.Id_wms_regra_cd
                                       select c;

                    var wmsRegra = await wmsRegraTask.FirstOrDefaultAsync();
                    if (wmsRegra == null)
                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                            Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': regra associada ao produto (" +
                            produto.Fabricante + ")" +
                            produto.Produto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
                    else
                    {
                        RegrasBll itemRegra = new RegrasBll();
                        itemRegra.Fabricante = produto.Fabricante;
                        itemRegra.Produto = produto.Produto;

                        itemRegra.TwmsRegraCd = new t_WMS_REGRA_CD
                        {
                            Id = wmsRegra.Id,
                            Apelido = wmsRegra.Apelido,
                            Descricao = wmsRegra.Descricao,
                            St_inativo = wmsRegra.St_inativo
                        };

                        var wmsRegraCdXUfTask = from c in db.TwmsRegraCdXUfs
                                                where c.Id_wms_regra_cd == itemRegra.TwmsRegraCd.Id &&
                                                      c.Uf == tcliente.Uf
                                                select c;
                        var wmsRegraCdXUf = await wmsRegraCdXUfTask.FirstOrDefaultAsync();

                        if (wmsRegraCdXUf == null)
                        {
                            itemRegra.St_Regra_ok = false;
                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                                Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': regra associada ao produto (" +
                                produto.Fabricante + ")" +
                                produto.Produto + " não está cadastrada para a UF '" + tcliente.Uf + "' (Id=" +
                                regra.Id_wms_regra_cd + ")");
                        }
                        else
                        {
                            itemRegra.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
                            {
                                Id = wmsRegraCdXUf.Id,
                                Id_wms_regra_cd = wmsRegraCdXUf.Id_wms_regra_cd,
                                Uf = wmsRegraCdXUf.Uf,
                                St_inativo = wmsRegraCdXUf.St_inativo
                            };

                            //buscar a sigla tipo pessoa
                            var tipo_pessoa = UtilsProduto.MultiCdRegraDeterminaPessoa(tcliente.Tipo,
                                tcliente.Contribuinte_Icms_Status, tcliente.Produtor_Rural_Status);

                            var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
                                                           where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
                                                                 c.Tipo_pessoa == tipo_pessoa
                                                           select c;

                            var wmsRegraCdXUfXPessoa = await wmsRegraCdXUfXPessoaTask.FirstOrDefaultAsync();

                            if (wmsRegraCdXUfXPessoa == null)
                            {
                                itemRegra.St_Regra_ok = false;
                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                    tcliente.Uf + "' e '" +
                                    Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                    "': regra associada ao produto (" + produto.Fabricante + ")" +
                                    produto.Produto + " não está cadastrada para a UF '" + tcliente.Uf +
                                    "' (Id=" + regra.Id_wms_regra_cd + ")");
                            }
                            else
                            {
                                itemRegra.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
                                {
                                    Id = wmsRegraCdXUfXPessoa.Id,
                                    Id_wms_regra_cd_x_uf = wmsRegraCdXUfXPessoa.Id_wms_regra_cd_x_uf,
                                    Tipo_pessoa = wmsRegraCdXUfXPessoa.Tipo_pessoa,
                                    St_inativo = wmsRegraCdXUfXPessoa.St_inativo,
                                    Spe_id_nfe_emitente = wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                };

                                if (wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                                {
                                    itemRegra.St_Regra_ok = false;
                                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                        tcliente.Uf + "' e '" +
                                        Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                        "': regra associada ao produto (" + produto.Fabricante + ")" +
                                        produto.Produto +
                                        " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id="
                                        + regra.Id_wms_regra_cd + ")");
                                }
                                else
                                {
                                    var nfEmitenteTask = from c in db.TnfEmitentes
                                                         where c.Id == wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                                         select c;
                                    var nfEmitente = await nfEmitenteTask.FirstOrDefaultAsync();

                                    if (nfEmitente != null)
                                    {
                                        if (nfEmitente.St_Ativo != 1)
                                        {
                                            itemRegra.St_Regra_ok = false;
                                            lstErros.Add("Falha na regra de consumo do estoque para a UF '" +
                                                tcliente.Uf + "' e '" +
                                                Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                                "': regra associada ao produto (" + produto.Fabricante +
                                                ")" + produto.Produto +
                                                " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
                                                "(Id=" + regra.Id_wms_regra_cd + ")");
                                        }
                                    }
                                    var wmsRegraCdXUfXPessoaXcdTask = from c in db.TwmsRegraCdXUfXPessoaXCds
                                                                      where c.Id_wms_regra_cd_x_uf_x_pessoa == wmsRegraCdXUfXPessoa.Id
                                                                      orderby c.Ordem_prioridade
                                                                      select c;
                                    var wmsRegraCdXUfXPessoaXcd = await wmsRegraCdXUfXPessoaXcdTask.ToListAsync();

                                    if (wmsRegraCdXUfXPessoaXcd.Count == 0)
                                    {
                                        itemRegra.St_Regra_ok = false;
                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                            tcliente.Uf + "' e '" +
                                            Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                            "': regra associada ao produto (" + produto.Fabricante + ")" +
                                            produto.Produto +
                                            " não especifica nenhum CD para consumo do estoque (Id=" +
                                            regra.Id_wms_regra_cd + ")");
                                    }
                                    else
                                    {
                                        itemRegra.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();
                                        foreach (var i in wmsRegraCdXUfXPessoaXcd)
                                        {
                                            t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
                                            {
                                                Id = i.Id,
                                                Id_wms_regra_cd_x_uf_x_pessoa = i.Id_wms_regra_cd_x_uf_x_pessoa,
                                                Id_nfe_emitente = i.Id_nfe_emitente,
                                                Ordem_prioridade = i.Ordem_prioridade,
                                                St_inativo = i.St_inativo
                                            };

                                            var nfeCadastroPrincipalTask = from c in db.TnfEmitentes
                                                                           where c.Id == item_cd_uf_pess_cd.Id_nfe_emitente
                                                                           select c;

                                            var nfeCadastroPrincipal = await nfeCadastroPrincipalTask.FirstOrDefaultAsync();

                                            if (nfeCadastroPrincipal != null)
                                            {
                                                if (nfeCadastroPrincipal.St_Ativo != 1)
                                                    item_cd_uf_pess_cd.St_inativo = 1;
                                            }

                                            itemRegra.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
                                        }
                                        foreach (var i in itemRegra.TwmsCdXUfXPessoaXCd)
                                        {

                                            if (i.Id_nfe_emitente == itemRegra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                            {
                                                if (i.St_inativo == 1)
                                                    lstErros.Add(
                                                        "Falha na leitura da regra de consumo do estoque para a UF '" +
                                                        tcliente.Uf + "' e '" +
                                                        Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                                        "': regra associada ao produto (" +
                                                        produto.Fabricante + ")" + produto.Produto +
                                                        " especifica o CD '" + Util.ObterApelidoEmpresaNfeEmitentes(wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente,
                                                        contextoProvider) +
                                                        "' para alocação de produtos sem presença no estoque, sendo que este CD está desativado para " +
                                                        "processar produtos disponíveis (Id=" + regra.Id_wms_regra_cd + ")");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        lstRegrasCrtlEstoque.Add(itemRegra);
                    }
                }
            }
            return lstRegrasCrtlEstoque;
        }

        //se mantiver, remover rotina em favor da ProdutoGeralBll.VerificarRegrasAssociadasAosProdutos
        public static void VerificarRegrasAssociadasParaUMProduto(List<RegrasBll> lst, List<string> lstErros,
            Tcliente cliente, int id_nfe_emitente_selecao_manual)
        {

            /*id_nfe_emitente_selecao_manual = 0;*///esse é a seleção do checkebox 

            foreach (var i in lst)
            {
                if (!string.IsNullOrEmpty(i.Produto))
                {
                    if (i.TwmsRegraCd.Id == 0)
                    {
                        lstErros.Add("Produto (" + i.Fabricante + ")" + i.Produto +
                            " não possui regra de consumo do estoque associada");
                    }
                    else if (i.St_Regra_ok == false)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto + " está desativada");
                    }
                    else if (i.TwmsRegraCdXUf.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto + " está bloqueada para a UF '" +
                            cliente.Uf + "'");
                    }
                    else if (i.TwmsRegraCdXUfXPessoa.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            i.Fabricante + ")" + i.Produto + " está bloqueada para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    else if (i.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto +
                            " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    else
                    {
                        int qtdeCdAtivo = 0;
                        foreach (var t in i.TwmsCdXUfXPessoaXCd)
                        {
                            if (t.Id_nfe_emitente > 0)
                            {
                                if (t.St_inativo == 0)
                                {
                                    qtdeCdAtivo = qtdeCdAtivo + 1;
                                }
                            }
                        }

                        if (qtdeCdAtivo == 0 && id_nfe_emitente_selecao_manual == 0)
                        {
                            lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                                "' associada ao produto (" + i.Fabricante + ")" + i.Produto +
                                " não especifica nenhum CD ativo para clientes '" + cliente.Tipo +
                                "' da UF '" + cliente.Uf + "'");


                        }
                    }
                }
            }
        }

   
      
     

    }
}
