using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacao.Data.Entities
{
    public class ProdutoRegraCrtlEstoque
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public bool StRegraOk { get; set; }
        public int RegraCdId { get; set; }
        public byte RegraCdStInativo { get; set; }
        public string RegraCdApelido { get; set; }
        public string RegraCdDescricao { get; set; }
        public int RegraCdUfId { get; set; }
        public int RegraCdUfIdWmsRegraCd { get; set; }
        public string RegraCdUfUf { get; set; }
        public byte RegraCdUfStinativo { get; set; }
        public int RegraCdUfPessoaId { get; set; }
        public int RegraCdUfPessoaIdWmsRegraCdUf { get; set; }
        public string RegraCdUfPessoaTipoPessoa { get; set; }
        public byte RegraCdUfPessoaStInativo { get; set; }
        public int RegraCdUfPessoaSpeIdNfeEmitente { get; set; }
        public List<RegraCdUfPessoaCd> RegraCdUfPessoaCds { get; set; }

        public class RegraCdUfPessoaCd
        {
            public int Id { get; set; }
            public int IdWmsRegraCdUfPessoa { get; set; }
            public int IdNfeEmitente { get; set; }
            public int OrdemPrioridade { get; set; }
            public byte StInativo { get; set; }
            public string EstoqueFabricante { get; set; }
            public string EstoqueProduto { get; set; }
            public string EstoqueDescricao { get; set; }
            public string EstoqueDescricaoHtml { get; set; }
            public short? EstoqueQtdeSolicitado { get; set; }
            public short EstoqueQtde { get; set; }
            public short? EstoqueQtdeEstoqueGlobal { get; set; }
            public short? EstoqueQtdeEstoque { get; set; }
        }
    }
}
