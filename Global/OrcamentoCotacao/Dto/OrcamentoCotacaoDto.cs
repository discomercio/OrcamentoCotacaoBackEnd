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
        public string vendedor { get; set; }
        public int idUsuarioCadastro { get; set; }
        public string usuarioCadastro { get; set; }
        public string tipoCliente { get; set; }
        public int? idIndicador { get; set; }
        //parceiro
        //vendedorParceiro
        public List<OrcamentoOpcaoResponseViewModel> listaOpcoes { get; set; }
        public RemetenteDestinatarioResponseViewModel mensageria { get; set; }
        public List<FormaPagamentoResponseViewModel> listaFormasPagto { get; set; }


        //public int idVendedor { get; set; }
        //public string loja { get; set; }
        //public int? idIndicadorVendedor { get; set; }
        //public bool aceiteWhatsApp { get; set; }
        //public string tipoCliente { get; set; }
        //public DateTime? validadeAnterior { get; set; }
        //public int qtdeRenovacao { get; set; }
        //public int idUsuarioUltRenovacao { get; set; }
        //public DateTime? dataHoraUltRenovacao { get; set; }
        //public string observacao { get; set; }
        //public int instaladorInstalaStatus { get; set; }
        //public int garantiaIndicadorStatus { get; set; }
        //public int stEtgImediata { get; set; }
        //public DateTime? previsaoEntregaData { get; set; }
        //public int status { get; set; }
        //public string statusNome { get; set; }
        //public int idTipoUsuarioContextoUltStatus { get; set; }
        //public int idUsuarioUltStatus { get; set; }
        //public DateTime dataUltStatus { get; set; }
        //public DateTime dataHoraUltStatus { get; set; }
        //public string idOrcamento { get; set; }
        //public string idPedido { get; set; }
        //public int idTipoUsuarioContextoCadastro { get; set; }
        //public DateTime dataCadastro { get; set; }
        //public DateTime dataHoraCadastro { get; set; }
        //public int idTipoUsuarioContextoUltAtualizacao { get; set; }
        //public int idUsuarioUltAtualizacao { get; set; }
        //public DateTime? dataHoraUltAtualizacao { get; set; }
    }
}

