using Loja.Dto;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using System;
using System.Collections.Generic;

namespace OrcamentoCotacao.Dto
{
    public class OrcamentoCotacaoDto
    {
        //Cliente
        public string nomeCliente { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string uf { get; set; }

        //Orcamento
        public int id { get; set; }
        public string nomeObra { get; set; }
        public DateTime validade { get; set; }
        public int idVendedor { get; set; }
        public int? idIndicador { get; set; }
        public int? idIndicadorVendedor { get; set; }
        public string vendedor { get; set; }
        public string nomeIniciaisEmMaiusculasVendedor { get; set; }
        public string parceiro { get; set; }
        public string razaoSocialNomeIniciaisEmMaiusculasParceiro { get; set; }
        public string vendedorParceiro { get; set; }
        public int idUsuarioCadastro { get; set; }
        public string usuarioCadastro { get; set; }
        public int idTipoUsuarioContextoCadastro { get; set; }
        public string amigavelUsuarioCadastro { get; set; }
        public string tipoCliente { get; set; }
        public int stEntregaImediata { get; set; }
        public DateTime? dataEntregaImediata { get; set; }
        public short status { get; set; }
        public string token { get; set; }
        public string? observacao { get; set; }
        public short? contribuinteIcms { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? previsaoEntregaData { get; set; }
        public string? statusDescricao { get; set; }
        public short? statusOrcamentoCotacaoLink { get; set; }
        public string? prazoMaximoConsultaOrcamento { get; set; }
        public string? loja { get; set; }
        public string? condicoesGerais { get; set; }

        public List<OrcamentoOpcaoResponseViewModel> listaOpcoes { get; set; }
        public RemetenteDestinatarioResponseViewModel mensageria { get; set; }
        public List<FormaPagamentoResponseViewModel> listaFormasPagto { get; set; }

        public LojaViewModel lojaViewModel { get; set; }
    }
}

