﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_CLIENTE_REF_COMERCIAL")]
    public class TclienteRefComercial
    {
        [Column("id_cliente")]
        [Required]
        [MaxLength(12)]
        public string Id_Cliente { get; set; }

        [Column("nome_empresa")]
        [Required]
        [MaxLength(60)]
        public string Nome_Empresa { get; set; }

        [Column("contato")]
        [MaxLength(40)]
        public string Contato { get; set; }

        [Column("ddd")]
        [MaxLength(2)]
        public string Ddd { get; set; }

        [Column("telefone")]
        [MaxLength(9)]
        public string Telefone { get; set; }

        [Column("ordem")]
        public short? Ordem { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(20)]
        public string Usuario_Cadastro { get; set; }

        [Column("excluido_status")]
        public short? Excluido_Status { get; set; }
    }
}