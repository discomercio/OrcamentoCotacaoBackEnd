using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.AcessoDto
{
    public class LoginUnisDto
    {
        //todo: afazer
        //Não sei quais os campos que serão validados na tabela para poder colocar a qtde de carcateres
        //edu: estão na t_USUARIO. A senha, não sei, verificar com o Hamilton ou verificar no sistema

        [Required]
        public string Usuario { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
