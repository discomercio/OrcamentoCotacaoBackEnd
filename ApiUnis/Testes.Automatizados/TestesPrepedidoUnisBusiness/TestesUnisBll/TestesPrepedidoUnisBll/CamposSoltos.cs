using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoBusiness.Bll.ClienteBll;
using PrepedidoBusiness.Bll.PrepedidoBll;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Testes.Automatizados.InicializarBanco.InicializarBancoGeral;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class CamposSoltos : TestesPrepedidoUnisBll
    {
        public CamposSoltos(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll, PrepedidoBll prepedidoBll, ClienteBll clienteBll) :
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
            Teste(c => c.ListaProdutos[0].Preco_NF = 11, "Preço de nota fiscal (Preco_NF R$ 11,00 x R$ 694,05) está incorreto!");
        }
        [Fact]
        public void Parcial_CustoFinancFornecCoeficiente()
        {
            Teste(c =>
                c.ListaProdutos[0].CustoFinancFornecCoeficiente = 11
            , "Coeficiente do fabricante (003) esta incorreto!");
        }
        [Fact]
        public void Parcial_CustoFinancFornecCoeficiente_Avista()
        {
            //Conforme foi incluido uma validação na forma de pagto, o teste não esta chegando para pegar essa msg
            //precisa fazer a busca de formaPagto para fazer a validação com a forma de pagto que esta sendo enviada 
            //para cadastrar Prepedido
            TesteAvista(c => c.ListaProdutos[0].CustoFinancFornecCoeficiente = 2, "Coeficiente do fabricante (003) esta incorreto!");
        }
        [Fact]
        public void Parcial_CustoFinancFornecPrecoListaBase()
        {
            Teste(c => c.ListaProdutos[0].CustoFinancFornecPrecoListaBase = 11, "Custo financeiro preço lista base (CustoFinancFornecPrecoListaBase R$ 11,00 x R$ 694,05) esta incorreto!");
        }


        /*
         * todo: testar o resto das estruturas
         * 
        EnderecoCadastralCliente	EnderecoCadastralClientePrepedidoUnisDto{...}
        OutroEndereco*	boolean
        EnderecoEntrega	EnderecoEntregaClienteCadastroUnisDto{...}
        ListaProdutos	[...]
        PermiteRAStatus	boolean
        ValorTotalDestePedidoComRA	number($double)
        VlTotalDestePedido	number($double)
        DetalhesPrepedido	DetalhesPrePedidoUnisDto{...}
        FormaPagtoCriacao	FormaPagtoCriacaoUnisDto{...}
        */

    }
}
