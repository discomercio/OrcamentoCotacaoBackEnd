using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class MensagemDtoOcorrenciaPedido
    {
        public DateTime Dt_Hr_Cadastro { get; set; }
        public string Usuario { get; set; }
        public string Loja { get; set; }
        public string Texto_Mensagem { get; set; }

        public static List<MensagemDtoOcorrenciaPedido> ListaMensagemDtoOcorrenciaPedido_De_MensagemOcorrenciaPedidoDados(IEnumerable<MensagemOcorrenciaPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<MensagemDtoOcorrenciaPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(MensagemDtoOcorrenciaPedido_De_MensagemOcorrenciaPedidoDados(p));
            return ret;
        }
        public static MensagemDtoOcorrenciaPedido MensagemDtoOcorrenciaPedido_De_MensagemOcorrenciaPedidoDados(MensagemOcorrenciaPedidoDados origem)
        {
            if (origem == null) return null;
            return new MensagemDtoOcorrenciaPedido()
            {
                Dt_Hr_Cadastro = origem.Dt_Hr_Cadastro,
                Usuario = origem.Usuario,
                Loja = origem.Loja,
                Texto_Mensagem = origem.Texto_Mensagem
            };
        }
    }
}
