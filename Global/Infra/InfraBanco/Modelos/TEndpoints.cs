using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraBanco.Modelos
{
    [Table("EndPoints")]
    public class TEndpoints:IModel
    {
        [Column("Id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("ActionName")]
        public string ActionName { get; set; }

        [Column("Delay")]
        public int Delay { get; set; }
    }
}
