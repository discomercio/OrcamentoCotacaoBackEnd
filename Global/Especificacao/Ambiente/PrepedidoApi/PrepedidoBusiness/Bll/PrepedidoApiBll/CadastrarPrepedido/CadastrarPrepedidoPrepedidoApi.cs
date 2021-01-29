﻿using InfraBanco.Constantes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
namespace Especificacao.Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido
{
    class CadastrarPrepedidoPrepedidoApi : Testes.Pedido.IPedidoPassosComuns
    {
        private readonly global::PrepedidoBusiness.Bll.PrepedidoApiBll prepedidoApiBll;

        public CadastrarPrepedidoPrepedidoApi()
        {
            prepedidoApiBll = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::PrepedidoBusiness.Bll.PrepedidoApiBll>();
        }

        private global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto prePedidoDto = CadastrarPrepedidoDados.PrepedidoBase();
        public void GivenPrepedidoBase()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            prePedidoDto = CadastrarPrepedidoDados.PrepedidoBase();
        }
        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            prePedidoDto = CadastrarPrepedidoDados.PrepedidoBaseComEnderecoDeEntrega();
        }

        public void GivenPedidoBaseClientePF()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            prePedidoDto = CadastrarPrepedidoDados.PrepedidoBaseClientePF();
        }

        public void GivenPedidoBaseClientePJ()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            prePedidoDto = CadastrarPrepedidoDados.PrepedidoBaseClientePJ();
        }

        public void GivenPedidoBaseClientePJComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJComEnderecoDeEntrega(this);
            prePedidoDto = CadastrarPrepedidoDados.PrepedidoBaseClientePJComEnderecoDeEntrega();
        }

        public void GivenPedidoBase()
        {
            GivenPrepedidoBase();
        }

        public void RecalcularTotaisDoPedido()
        {
            decimal totalCompare = 0;
            decimal totalRaCompare = 0;
            foreach (var x in prePedidoDto.ListaProdutos)
            {
                totalCompare += Math.Round((decimal)(x.Preco_Venda * x.Qtde ?? 0), 2);
                totalRaCompare += Math.Round((decimal)(x.Preco_NF * x.Qtde ?? 0), 2);
            }
            prePedidoDto.VlTotalDestePedido = totalCompare;
            prePedidoDto.ValorTotalDestePedidoComRA = totalRaCompare;
        }
        public void DeixarFormaDePagamentoConsistente()
        {
            Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido.EstaticoDeixarFormaDePagamentoConsistente(prePedidoDto);
        }


        public void WhenInformo(string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.Informo(campo, valor, this);
            switch (campo)
            {
                case "CPF/CNPJ":
                    prePedidoDto.DadosCliente.Cnpj_Cpf = valor;
                    return;
            }

            //acertos em campos
            if (campo.ToLower() == "vl_total_NF".ToLower())
                campo = "ValorTotalDestePedidoComRA";
            if (campo.ToLower() == "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas".ToLower())
                campo = "FormaPagtoCriacao.Qtde_Parcelas";
            if (campo.ToLower() == "CustoFinancFornecQtdeParcelas".ToLower())
                campo = "FormaPagtoCriacao.Qtde_Parcelas";

            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoDto))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoDto.FormaPagtoCriacao))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoDto.EnderecoEntrega))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoDto.EnderecoCadastroClientePrepedido))
                return;
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, prePedidoDto.DetalhesPrepedido))
                return;

            switch (campo)
            {
                case "EndEtg_obs":
                    prePedidoDto.EnderecoEntrega.EndEtg_cod_justificativa = valor;
                    return;
                case "Endereco_EmailXml":
                    prePedidoDto.EnderecoCadastroClientePrepedido.Endereco_email_xml = valor;
                    return;
                case "EnderecoCadastralCliente.Endereco_tipo_pessoa":
                    prePedidoDto.EnderecoCadastroClientePrepedido.Endereco_tipo_pessoa = valor;
                    return;
                default:
                    Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.WhenInformo");
                    break;
            }
        }

        public void ListaDeItensComXitens(int numeroItens)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(numeroItens, this);
            numeroItens = numeroItens < 0 ? 0 : numeroItens;
            numeroItens = numeroItens > 100 ? 100 : numeroItens;
            var lp = prePedidoDto.ListaProdutos;
            while (lp.Count < numeroItens)
                lp.Add(new global::PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrepedidoProdutoDtoPrepedido());
            while (lp.Count > numeroItens)
                lp.RemoveAt(lp.Count - 1);
        }

        public void ListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(numeroItem, campo, valor, this);
            var item = prePedidoDto.ListaProdutos[numeroItem];
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, item))
                return;
            Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.ListaDeItensInformo");
        }


        public void ThenErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            ThenErro(p0, true);
        }
        public void ThenSemErro(string p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            ThenErro(p0, false);
        }
        public void ThenSemNenhumErro()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            ThenErro(null, false);
        }


        //para quando querem aessar o prepedido criado
        public IEnumerable<string> UltimoResultadoCadastrarPrepedido { get; private set; } = new List<string>();

        private void ThenErro(string? erro, bool erroDeveExistir)
        {
            if (ignorarFeature) return;

            Testes.Utils.LogTestes.LogOperacoes2.ChamadaController(prepedidoApiBll.GetType(), "CadastrarPrepedido", this);

            string apelido = CadastrarPrepedidoDados.Usuario;
            //exatamente igual ao PrepedidoApi.Controllers public async Task<IActionResult> CadastrarPrepedido(PrePedidoDto prePedido)
            //mas nao verificamos prepedido repetido
            IEnumerable<string> ret = prepedidoApiBll.CadastrarPrepedido(prePedidoDto, apelido.Trim(), 0.01M, false,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS).Result;
            UltimoResultadoCadastrarPrepedido = ret;

            //se não tem erro, retorna somente 1 item com o prepedio, tipo ["227905Z"]
            var erros = new List<string>();
            var lsita = UltimoResultadoCadastrarPrepedido.ToList();
            if (lsita.Count() != 1)
                erros = lsita;
            if (lsita.Count() > 0 && lsita[0].Length != "227905Z".Length)
                erros = lsita;
            Testes.Pedido.HelperImplementacaoPedido.CompararMensagemErro(erro, erroDeveExistir, erros, this);
        }

        private bool ignorarFeature = false;
        public void GivenIgnorarCenarioNoAmbiente(string p0)
        {
            Testes.Pedido.PedidoPassosComuns.IgnorarCenarioNoAmbiente(p0, ref ignorarFeature, this.GetType());
        }

    }
}