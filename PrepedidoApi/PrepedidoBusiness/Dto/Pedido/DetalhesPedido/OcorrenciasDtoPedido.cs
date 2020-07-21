﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Pedido.DetalhesPedido
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
    }
}