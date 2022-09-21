using Prepedido;
using Prepedido.Bll;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using Xunit;
using Xunit.Abstractions;
using static Testes.Automatizados.InicializarBanco.InicializarBancoGeral;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class CamposSoltos : TestesPrepedidoUnisBll
    {
        public CamposSoltos(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll, PrepedidoBll prepedidoBll, Cliente.ClienteBll clienteBll) :
            base(inicializarBanco, Output, prepedidoUnisBll, clienteUnisBll, prepedidoBll, clienteBll)
        {

        }

        /*
         * vamos testar os campos soltos:
Cnpj_Cpf*	string 
Indicador_Orcamentista*	string 
PermiteRAStatus	boolean
ValorTotalDestePedidoComRA	number($double)
VlTotalDestePedido	number($double)
*/
        [Fact]
        public void Cnpj_Cpf()
        {
            Teste(c => c.Cnpj_Cpf = "", "Cliente não localizado", true, false);
            Teste(c => c.Cnpj_Cpf = "123", "Cliente não localizado", true, false);
            Teste(c => c.Cnpj_Cpf = "12349535078", "Cliente não localizado", true, false);
            Teste(c => c.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().Cnpj_Cpf, "Cliente não localizado", false, false);
            Teste(c => c.EnderecoCadastralCliente.Endereco_cnpj_cpf = "96077309095", "O CPF/CNPJ do cliente está divergindo do cadastro!", true, false);
        }

        [Fact]
        public void Sucesso()
        {
            TestarSucesso(c => { });
        }
        [Fact]
        public void SucessoAvista()
        {
            //Conforme foi incluido uma validação na forma de pagto, o teste não esta chegando para pegar essa msg
            TestarSucessoAvista(c => { });
        }
        [Fact]
        public void ValorTotalDestePedidoComRA()
        {
            Teste(c => c.ValorTotalDestePedidoComRA = 1, "Valor total da forma de pagamento diferente do valor total!");
            Teste(c => c.VlTotalDestePedido = 1, "Os valores totais estão divergindo!");
        }

        [Fact]
        public void ExcedeValorDeRA()
        {
            //gerar o valor que excede o limite de ra            
            TesteParcCartao(c => {
                c.ValorTotalDestePedidoComRA = 4400M;
                c.FormaPagtoCriacao.C_pc_valor = 1100M;
                c.ListaProdutos[0].Preco_NF = 1100M;
                c.ListaProdutos[1].Preco_NF = 1100M;
            }, "O valor total de RA excede o limite permitido!");
        }

        [Fact]
        public void Indicador_Orcamentista()
        {
            Teste(c => c.Indicador_Orcamentista = "um que nao existe", "O Orçamentista não existe!", true, false);
            Teste(c => c.Indicador_Orcamentista = "", "O Orçamentista não existe!", true, false);

            Teste(c => c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_com_ra, "Permite RA status divergente do cadastro do indicador/orçamentista!", false);
            Teste(c => c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_sem_ra, "Permite RA status divergente do cadastro do indicador/orçamentista!", true, false);

            Teste(c => c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_sem_loja, "Loja não habilitada para e-commerce: loja nao e-commerce", true, false);


            Teste(c =>
            {
                c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_sem_vendedor;
                c.PermiteRAStatus = false;
            }, "NÃO HÁ NENHUM VENDEDOR DEFINIDO PARA ATENDÊ-LO", true, false);

            Teste(c =>
            {
                c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_com_ra;
                c.PermiteRAStatus = false;
            }, "Permite RA status divergente do cadastro do indicador/orçamentista!", true, false);
            Teste(c =>
            {
                c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_sem_ra;
                c.PermiteRAStatus = true;
            }
            , "Permite RA status divergente do cadastro do indicador/orçamentista!", true, false);
        }

        //testamos estes 4 campos em separado porque a validação foi implementada depois
        /*
        validar:
        "Preco_NF": 221000.00,
        "CustoFinancFornecCoeficiente": 1.0527,
        "CustoFinancFornecPrecoListaBase": 221041.07
        CustoFinancFornecCoeficiente deve ser 1 se for pagamento a vista
        */
        [Fact]
        public void Parcial_CustoFinancFornecTipoParcelamento()
        {
            Teste(c => c.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento = "xx", "Tipo do parcelamento (CustoFinancFornecTipoParcelamento 'xx') está incorreto!");
            Teste(c => c.FormaPagtoCriacao.Tipo_Parcelamento = 99, "Tipo do parcelamento inválido");
            //a base de teste é parcelado
            Teste(c => c.FormaPagtoCriacao.Tipo_Parcelamento = 1, "Tipo do parcelamento (CustoFinancFornecTipoParcelamento 'SE') está incorreto!");
        }


        [Fact]
        public void Parcial_Preco_NF()
        {
            TesteParcCartao(c =>
            {
                c.ListaProdutos[0].Preco_NF = 11;
                c.Indicador_Orcamentista = "Apelido_sem_ra";
                c.PermiteRAStatus = false;
                c.ListaProdutos[0].Preco_Venda = 687.11m;
                c.VlTotalDestePedido = 1735.12M;

                var ret = c;
                ret.VlTotalDestePedido = ret.ListaProdutos[0].Preco_NF * ret.ListaProdutos[0].Qtde;
                ret.VlTotalDestePedido += ret.ListaProdutos[1].Preco_NF * ret.ListaProdutos[1].Qtde;
            },
            "Valor total da forma de pagamento diferente do valor total!");
        }

        [Fact]
        public void Parcial_Preco_NF2()
        {
            TesteParcCartao(c =>
            {
                c.ListaProdutos[0].Preco_NF = 11;
                c.Indicador_Orcamentista = "Apelido_sem_ra";
                c.PermiteRAStatus = false;
                c.ListaProdutos[0].Preco_Venda = 687.11m;
                c.VlTotalDestePedido = 1735.12M;

                var ret = c;
                ret.VlTotalDestePedido = ret.ListaProdutos[0].Preco_NF * ret.ListaProdutos[0].Qtde;
                ret.VlTotalDestePedido += ret.ListaProdutos[1].Preco_NF * ret.ListaProdutos[1].Qtde;
                ret.FormaPagtoCriacao.C_pc_valor = ret.VlTotalDestePedido / ret.FormaPagtoCriacao.C_pc_qtde;
            },
            "Preço de nota fiscal (Preco_NF R$ 11,00 x Preco_Venda 687,11) está incorreto!",
            true,
            //Não consigui fazer dar esse erro ao cadastrar o PrepedidoDto, o teste da Unis passa
            //É quando chama o segundo teste interno "TesteInternoPrepedidoBll"
            false);
        }

        [Fact]
        public void Parcial_CustoFinancFornecCoeficiente()
        {
            Teste(c =>
                c.ListaProdutos[0].CustoFinancFornecCoeficiente = 11
            , "Coeficiente do fabricante (003) está incorreto!");
        }
        [Fact]
        public void Parcial_CustoFinancFornecCoeficiente_Avista()
        {
            //Conforme foi incluido uma validação na forma de pagto, o teste não esta chegando para pegar essa msg
            //precisa fazer a busca de formaPagto para fazer a validação com a forma de pagto que esta sendo enviada 
            //para cadastrar Prepedido
            TesteAvista(c =>
            {
                c.ListaProdutos[0].CustoFinancFornecCoeficiente = 2;
                c.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 0;
            }, "Coeficiente do fabricante (003) está incorreto!");
        }
        [Fact]
        public void Parcial_CustoFinancFornecPrecoListaBase()
        {
            Teste(c => c.ListaProdutos[0].Preco_Lista = 11, "Custo financeiro preço lista base (Preco_Lista R$ 11,00 x R$ 694,05) esta incorreto!");
        }


        /*
         * faltando testar o resto das estruturas
         * 
        EnderecoCadastralCliente	EnderecoCadastralClientePrepedidoUnisDto{...}
        OutroEndereco*	boolean
        EnderecoEntrega	EnderecoEntregaClienteCadastroUnisDto{...}
        ListaProdutos	[...]
        PermiteRAStatus	boolean
        Vl_total_NF	number($double)
        Vl_total	number($double)
        DetalhesPrepedido	DetalhesPrePedidoUnisDto{...}
        FormaPagtoCriacao	FormaPagtoCriacaoUnisDto{...}
        */

        [Fact]
        public void TestePagtoAvista()
        {
            TesteAvista(c =>
            {
                c.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 2;
            }, "Quantidade da parcela esta divergente!", true, false);

            TesteAvista(c =>
            {
                c.FormaPagtoCriacao.Op_av_forma_pagto = "2";
            }, "Informe uma opção de pagamento (à vista) válida.", true);

        }

        [Fact]
        public void TesteParcelaUnica()
        {
            TesteParcUnica(c =>
            {
                c.FormaPagtoCriacao.Op_pu_forma_pagto = "";
            }, "Indique a forma de pagamento da parcela única.", true);

            TesteParcUnica(c =>
            {
                c.FormaPagtoCriacao.C_pu_valor = null;
            }, "Indique o valor da parcela única.", true);

            TesteParcUnica(c =>
            {
                c.FormaPagtoCriacao.C_pu_valor = 0;
            }, "Valor da parcela única é inválido.", true);

            TesteParcUnica(c =>
            {
                c.FormaPagtoCriacao.C_pu_vencto_apos = null;
            }, "Indique o intervalo de vencimento da parcela única.", true);

            TesteParcUnica(c =>
            {
                c.FormaPagtoCriacao.C_pu_vencto_apos = 0;
            }, "Intervalo de vencimento da parcela única é inválido.", true);

            TesteParcUnica(c =>
            {
                c.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 2;
            }, "Quantidade da parcela esta divergente!", true, false);

            TesteParcUnica(c =>
            {
                c.FormaPagtoCriacao.C_pc_qtde = 0;
                c.FormaPagtoCriacao.C_pc_valor = 0;
                c.FormaPagtoCriacao.C_pu_valor = 3460.04m;
            }, "Valor total da forma de pagamento diferente do valor total!", true);

            TesteParcUnica(c =>
            {
                c.FormaPagtoCriacao.C_pc_qtde = 0;
                c.FormaPagtoCriacao.C_pc_valor = 0;
                c.Indicador_Orcamentista = "Apelido_sem_ra";
                c.PermiteRAStatus = false;
            }, "Valor total da forma de pagamento diferente do valor total!", true);
        }

        [Fact]
        public void TesteParceladoCartao()
        {
            TesteParcCartao(c =>
            {
                c.FormaPagtoCriacao.C_pc_qtde = null;
            }, "Indique a quantidade de parcelas (parcelado no cartão [internet]).", true);

            TesteParcCartao(c =>
            {
                c.FormaPagtoCriacao.C_pc_qtde = 0;
            }, "Quantidade de parcelas inválida (parcelado no cartão [internet]).", true);

            TesteParcCartao(c =>
            {
                c.FormaPagtoCriacao.C_pc_valor = null;
            }, "Indique o valor da parcela (parcelado no cartão [internet]).", true);

            TesteParcCartao(c =>
            {
                c.FormaPagtoCriacao.C_pc_valor = 0m;
            }, "Valor de parcela inválido (parcelado no cartão [internet]).", true);

            TesteParcCartao(c =>
            {
                c.FormaPagtoCriacao.C_pc_qtde = 2;
            }, "Quantidade de parcelas esta divergente!", true, false);

            TesteParcCartao(c =>
            {
            }, "Valor total da forma de pagamento diferente do valor total!", true);

            TesteParcCartao(c =>
            {
                c.Indicador_Orcamentista = "Apelido_sem_ra";
                c.PermiteRAStatus = false;
            }, "Valor total da forma de pagamento diferente do valor total!", true);

        }

        [Fact]
        public void TesteParceladoCartaoMaquineta()
        {
            TesteParcCartaoMaquineta(c =>
            {
                c.FormaPagtoCriacao.C_pc_maquineta_qtde = null;
            }, "Indique a quantidade de parcelas (parcelado no cartão [maquineta]).", true);

            TesteParcCartaoMaquineta(c =>
            {
                c.FormaPagtoCriacao.C_pc_maquineta_qtde = 0;
            }, "Quantidade de parcelas inválida (parcelado no cartão [maquineta]).", true);

            TesteParcCartaoMaquineta(c =>
            {
                c.FormaPagtoCriacao.C_pc_maquineta_valor = null;
            }, "Indique o valor da parcela (parcelado no cartão [maquineta]).", true);

            TesteParcCartaoMaquineta(c =>
            {
                c.FormaPagtoCriacao.C_pc_maquineta_valor = 0m;
            }, "Valor de parcela inválido (parcelado no cartão [maquineta]).", true);

            TesteParcCartaoMaquineta(c =>
            {
                c.FormaPagtoCriacao.C_pc_maquineta_qtde = 2;
            }, "Quantidade de parcelas esta divergente!", true, false);

            TesteParcCartaoMaquineta(c => { }, "Valor total da forma de pagamento diferente do valor total!", true);

            TesteParcCartaoMaquineta(c =>
            {
                c.Indicador_Orcamentista = "Apelido_sem_ra";
                c.PermiteRAStatus = false;
            }, "Valor total da forma de pagamento diferente do valor total!", true);
        }

        [Fact]
        public void TesteComEntrada()
        {
            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = "";
            }, "Indique a forma de pagamento da entrada (parcelado com entrada).", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.C_pce_entrada_valor = 0;
            }, "Indique o valor da entrada (parcelado com entrada).", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.C_pce_entrada_valor = null;
            }, "Valor da entrada inválido (parcelado com entrada).", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = "";
            }, "Indique a forma de pagamento das prestações (parcelado com entrada).", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.C_pce_prestacao_qtde = 0;
            }, "Indique a quantidade de prestações (parcelado com entrada).", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.C_pce_prestacao_qtde = null;
            }, "Quantidade de prestações inválida (parcelado com entrada).", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.C_pce_prestacao_valor = 0;
            }, "Indique o valor da prestação (parcelado com entrada).", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.C_pce_prestacao_valor = null;
            }, "Valor de prestação inválido (parcelado com entrada).", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.C_pce_prestacao_periodo = null;
            }, "Indique o intervalo de vencimento entre as parcelas (parcelado com entrada).", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = "7";
                c.FormaPagtoCriacao.C_pce_prestacao_periodo = 0;
            }, "Intervalo de vencimento inválido (parcelado com entrada).", true);
        }
        [Fact]
        public void TesteComEntrada2()
        {
            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = "3";
                c.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = "3";
                c.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 5;
            }, "Quantidade de parcelas esta divergente!", true, false);

            TestePagamentoComEntrada(c =>
            {
                c.Indicador_Orcamentista = "Apelido_sem_ra";
                c.PermiteRAStatus = false;
                c.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = "3";
                c.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = "3";
            }, "Valor total da forma de pagamento diferente do valor total!", true);

            TestePagamentoComEntrada(c =>
            {
                c.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = "3";
                c.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = "3";
            }, "Valor total da forma de pagamento diferente do valor total!", true);

            TestePagamentoComEntrada(c => { }, "Informe a forma de pagamento da entrada (parcelado com entrada) válida.");

            TestePagamentoComEntrada(c => { }, "Informe a forma de pagamento das prestações (parcelado com entrada) válida.");

        }

        [Fact]
        public void VerificarListaProdutos()
        {
            TesteAvista(c =>
            {
                c.ListaProdutos = new System.Collections.Generic.List<PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto.PrePedidoProdutoPrePedidoUnisDto>();
            }, "Não há itens na lista de produtos!");
        }
    }
}
