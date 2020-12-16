using InfraBanco;
using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll;

namespace Testes.Automatizados.InicializarBanco
{
    public class InicializarBancoGeral
    {
        private static bool _inicialziado = false;
        private readonly ContextoBdProvider contextoBdProvider;
        private readonly Testes.Automatizados.InicializarBanco.InicializarBancoCep inicializarCep;

        public InicializarBancoGeral(InfraBanco.ContextoBdProvider contextoBdProvider, Testes.Automatizados.InicializarBanco.InicializarBancoCep inicializarCep)
        {
            this.contextoBdProvider = contextoBdProvider;
            this.inicializarCep = inicializarCep;
            GarantirInicializado();
        }

        public void GarantirInicializado()
        {
            if (!_inicialziado)
            {
                _inicialziado = true;
                Inicalizar();
            }
        }

        public void TclientesApagar()
        {
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing())
            {
                foreach (var c in db.Tclientes)
                    db.Tclientes.Remove(c);

                db.SaveChanges();
            }
        }

        private void Inicalizar()
        {
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing())
            {
                db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
                {
                    Apelido = Dados.Orcamentista.Apelido.ToUpper(),
                    Vendedor = Dados.Orcamentista.Apelido_sem_ra.ToUpper(),
                    Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                    Permite_RA_Status = 1
                });
                db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
                {
                    Apelido = Dados.Orcamentista.Apelido_com_ra.ToUpper(),
                    Vendedor = Dados.Orcamentista.Apelido_com_ra.ToUpper(),
                    Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                    Permite_RA_Status = 1
                });
                db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
                {
                    Apelido = Dados.Orcamentista.Apelido_sem_ra.ToUpper(),
                    Vendedor = Dados.Orcamentista.Apelido_sem_ra.ToUpper(),
                    Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                    Permite_RA_Status = 0
                });
                db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
                {
                    Apelido = Dados.Orcamentista.Apelido_sem_vendedor.ToUpper(),
                    Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                    Permite_RA_Status = 0
                });
                db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
                {
                    Apelido = Dados.Orcamentista.Apelido_sem_loja.ToUpper(),
                    Vendedor = Dados.Orcamentista.Apelido_sem_loja.ToUpper(),
                    Loja = "loja nao e-commerce",
                    Permite_RA_Status = 1
                });


                db.Tparametros.Add(new InfraBanco.Modelos.Tparametro() { Id = Constantes.ID_PARAMETRO_PERC_DESAGIO_RA_LIQUIDA, Campo_Real = 25 });
                db.Tparametros.Add(new InfraBanco.Modelos.Tparametro() { Id = Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal, Campo_inteiro = 1 });

                db.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole() { Id_Nsu = Constantes.NSU_CADASTRO_CLIENTES, Nsu = "000000645506" });
                db.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole() { Id_Nsu = Constantes.NSU_ORCAMENTO, Nsu = "000000000006" });
                //deixamos com 26% porque com 6,2% dá erro nos casos que a gente estava usando
                db.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole() { Id_Nsu = Constantes.ID_PARAM_PercVlPedidoLimiteRA, Nsu = "26,20" });

                db.Tbancos.Add(new InfraBanco.Modelos.Tbanco() { Codigo = Testes.Automatizados.InicializarBanco.InicializarClienteDados.ClienteNaoCadastradoPJ().RefBancaria[0].Banco });

                //permitr pagamento a vista para todos
                short idPagamentoAvista = 1;
                short idPagamentoParcUnica = 5;
                short idPagamentoComEntrada = 3;
                db.TformaPagtos.Add(new InfraBanco.Modelos.TformaPagto() { Hab_a_vista = 1, Id = idPagamentoAvista });
                db.TformaPagtos.Add(new InfraBanco.Modelos.TformaPagto() { Hab_parcela_unica = 1, Id = idPagamentoParcUnica });
                db.TformaPagtos.Add(new InfraBanco.Modelos.TformaPagto() { Hab_prestacao = 1, Hab_entrada = 1, Id = idPagamentoComEntrada });

                /*
                 * sem restrições
                db.torcamentistaEIndicadorRestricaoFormaPagtos.Add(new InfraBanco.Modelos.TorcamentistaEIndicadorRestricaoFormaPagto()
                {
                    Id_orcamentista_e_indicador = Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS,
                    Tipo_cliente = "PJ",
                    St_restricao_ativa = 1,
                    Id_forma_pagto = idPagamentoAvista
                });
                db.torcamentistaEIndicadorRestricaoFormaPagtos.Add(new InfraBanco.Modelos.TorcamentistaEIndicadorRestricaoFormaPagto()
                {
                    Id_orcamentista_e_indicador = Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS,
                    Tipo_cliente = "PF",
                    St_restricao_ativa = 1,
                    Id_forma_pagto = idPagamentoAvista
                });
                */


                InicalizarProdutos(db);
                InicializarListaEnderecoEntregaJustificativa(db);

                db.SaveChanges();
            }

        }

        private void InicalizarProdutos(ContextoBdGravacao db)
        {
            var Id_wms_regra_cd = 1;
            var Id_wms_regra_cd_x_uf = 1;
            short Spe_id_nfe_emitente = 1;
            int Id_wms_regra_cd_x_uf_x_pessoa = 1;

            db.TnfEmitentes.Add(new InfraBanco.Modelos.TnfEmitente()
            {
                Id = Spe_id_nfe_emitente,
                St_Ativo = 1
            });


            var dado = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez();
            var produtos = dado.ListaProdutos;
            foreach (var item in produtos)
            {
                if (!db.Tfabricantes.Any(f => f.Fabricante == item.Fabricante))
                {
                    db.Tfabricantes.Add(new InfraBanco.Modelos.Tfabricante() { Fabricante = item.Fabricante });
                    db.SaveChanges();
                }

                if (!db.Tprodutos.Any(f => f.Fabricante == item.Fabricante && f.Produto == item.Produto))
                {
                    db.Tprodutos.Add(new InfraBanco.Modelos.Tproduto()
                    {
                        Fabricante = item.Fabricante,
                        Produto = item.Produto,
                        Descontinuado = "N"
                    });

                    db.TprodutoLojas.Add(new InfraBanco.Modelos.TprodutoLoja()
                    {
                        Produto = item.Produto,
                        Fabricante = item.Fabricante,
                        Vendavel = "S",
                        Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                        Preco_Lista = item.CustoFinancFornecPrecoListaBase
                    });

                    {
                        //regras de consumo de estoque

                        db.TprodutoXwmsRegraCds.Add(new InfraBanco.Modelos.TprodutoXwmsRegraCd()
                        {
                            Produto = item.Produto,
                            Fabricante = item.Fabricante,
                            Id_wms_regra_cd = Id_wms_regra_cd
                        });

                        db.TwmsRegraCds.Add(new InfraBanco.Modelos.TwmsRegraCd()
                        {
                            Id = Id_wms_regra_cd,
                            Apelido = "wmsRegra.Apelido",
                            Descricao = "wmsRegra.Descricao",
                            St_inativo = 0
                        });

                        db.TwmsRegraCdXUfs.Add(new InfraBanco.Modelos.TwmsRegraCdXUf()
                        {
                            Id = Id_wms_regra_cd_x_uf,
                            Id_wms_regra_cd = Id_wms_regra_cd,
                            Uf = dado.EnderecoCadastralCliente.Endereco_uf
                        });

                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA, ref Id_wms_regra_cd_x_uf_x_pessoa);
                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL, ref Id_wms_regra_cd_x_uf_x_pessoa);
                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE, ref Id_wms_regra_cd_x_uf_x_pessoa);
                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE, ref Id_wms_regra_cd_x_uf_x_pessoa);
                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO, ref Id_wms_regra_cd_x_uf_x_pessoa);

                        //Spe_id_nfe_emitente++;
                        Id_wms_regra_cd_x_uf++;
                        Id_wms_regra_cd++;
                    }

                    db.SaveChanges();
                }
            }

            //coeficientes
            foreach (var f in db.Tfabricantes)
            {
                db.TpercentualCustoFinanceiroFornecedors.Add(new InfraBanco.Modelos.TpercentualCustoFinanceiroFornecedor()
                {
                    Fabricante = f.Fabricante,
                    Tipo_Parcelamento = dado.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento,
                    Qtde_Parcelas = (short)dado.FormaPagtoCriacao.C_pc_qtde.Value,
                    Coeficiente = produtos.First(lp => lp.Fabricante == f.Fabricante).CustoFinancFornecCoeficiente
                });

                db.TpercentualCustoFinanceiroFornecedors.Add(new InfraBanco.Modelos.TpercentualCustoFinanceiroFornecedor()
                {
                    Fabricante = f.Fabricante,
                    Tipo_Parcelamento = "SE",
                    Qtde_Parcelas = 4,
                    Coeficiente = 1.0527f
                });

                db.TpercentualCustoFinanceiroFornecedors.Add(new InfraBanco.Modelos.TpercentualCustoFinanceiroFornecedor()
                {
                    Fabricante = f.Fabricante,
                    Tipo_Parcelamento = "CE",
                    Qtde_Parcelas = 3,
                    Coeficiente = 1.0527f
                });
            }
        }

        private static void InicalizarProdutos_TwmsRegraCdXUfPessoas(ContextoBdGravacao db, int Id_wms_regra_cd_x_uf, short Spe_id_nfe_emitente, string Tipo_pessoa,
            ref int Id_wms_regra_cd_x_uf_x_pessoa)
        {
            db.TwmsRegraCdXUfPessoas.Add(new InfraBanco.Modelos.TwmsRegraCdXUfPessoa()
            {
                Id = Id_wms_regra_cd_x_uf_x_pessoa,
                Tipo_pessoa = Tipo_pessoa,
                Id_wms_regra_cd_x_uf = Id_wms_regra_cd_x_uf,
                Spe_id_nfe_emitente = Spe_id_nfe_emitente,
                St_inativo = 0
            });

            db.TwmsRegraCdXUfXPessoaXCds.Add(new InfraBanco.Modelos.TwmsRegraCdXUfXPessoaXCd()
            {
                Id_wms_regra_cd_x_uf_x_pessoa = Id_wms_regra_cd_x_uf_x_pessoa,
                Id_nfe_emitente = Spe_id_nfe_emitente
            });

            Id_wms_regra_cd_x_uf_x_pessoa++;
        }

        static public class Dados
        {
            static public class Orcamentista
            {
                public static string Apelido = "Konar";
                public static string Apelido_com_ra = "Apelido_com_ra";
                public static string Apelido_sem_ra = "Apelido_sem_ra";
                public static string Apelido_sem_vendedor = "Apelido_sem_vendedor";
                public static string Apelido_sem_loja = "Apelido_sem_loja";
                public static string ApelidoNaoExiste = "XXX";
            }
        }

        private static void InicializarListaEnderecoEntregaJustificativa(ContextoBdGravacao db)
        {
            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "001",
                Descricao = "Casa de Veraneio"
            });

            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                //não posso adicionar o "Grupo" para os outros 
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "002",
                Descricao = "Doação"
            });

            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "003",
                Descricao = "Nova Unidade da Empresa/Filial"
            });
            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "004",
                Descricao = "Parente do Proprietário (Pais, Filhos e Irmãos)"
            });
            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "005",
                Descricao = "Residência do Proprietário"
            });
            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "006",
                Descricao = "Endereço Comercial do Proprietário"
            });
            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "007",
                Descricao = "Pedido Arclube"
            });
            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "008",
                Descricao = "Endereço da Obra"
            });
            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "009",
                Descricao = "Endereço Novo Cliente"
            });
            db.TcodigoDescricaos.Add(new InfraBanco.Modelos.TcodigoDescricao()
            {
                Grupo = Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                Lojas_Habilitadas = null,
                St_Inativo = 0,
                Codigo = "010",
                Descricao = "Acerto Interno"
            });
        }
    }
}
