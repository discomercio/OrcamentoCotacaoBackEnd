﻿using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Dto.Pedido.DetalhesPedido
{
    public class OcorrenciasDtoPedido
    {
        public string Usuario { get; set; }
        public DateTime Dt_Hr_Cadastro { get; set; }
        public string Situacao { get; set; }//se qtde _msg_central > 0 = Em Andamento senão = Aberta
        public string Contato { get; set; }
        public string Texto_Ocorrencia { get; set; }
        public List<MensagemDtoOcorrenciaPedido> mensagemDtoOcorrenciaPedidos { get; set; }
        public string Finalizado_Usuario { get; set; }
        public DateTime? Finalizado_Data_Hora { get; set; }
        public string Tipo_Ocorrencia { get; set; }//obtem_descricao_tabela_t_codigo_descricao(GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__TIPO_OCORRENCIA, Trim("" & rs("tipo_ocorrencia")))
        public string Texto_Finalizacao { get; set; }


        public static List<OcorrenciasDtoPedido> ListaOcorrenciasDtoPedido_De_OcorrenciasPedidoDados(IEnumerable<OcorrenciasPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<OcorrenciasDtoPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(OcorrenciasDtoPedido_De_OcorrenciasPedidoDados(p));
            return ret;
        }
        public static OcorrenciasDtoPedido OcorrenciasDtoPedido_De_OcorrenciasPedidoDados(OcorrenciasPedidoDados origem)
        {
            if (origem == null) return null;
            return new OcorrenciasDtoPedido()
            {
                Usuario = origem.Usuario,
                Dt_Hr_Cadastro = origem.Dt_Hr_Cadastro,
                Situacao = origem.Situacao,
                Contato = origem.Contato,
                Texto_Ocorrencia = origem.Texto_Ocorrencia,
                mensagemDtoOcorrenciaPedidos = MensagemDtoOcorrenciaPedido.ListaMensagemDtoOcorrenciaPedido_De_MensagemOcorrenciaPedidoDados(origem.mensagemDtoOcorrenciaPedidos),
                Finalizado_Usuario = origem.Finalizado_Usuario,
                Finalizado_Data_Hora = origem.Finalizado_Data_Hora,
                Tipo_Ocorrencia = origem.Tipo_Ocorrencia,
                Texto_Finalizacao = origem.Texto_Finalizacao
            };
        }
    }
}
