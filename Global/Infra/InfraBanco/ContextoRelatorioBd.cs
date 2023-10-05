using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraBanco
{
    public class ContextoRelatorioBd : DbContext
    {
        public ContextoRelatorioBd(DbContextOptions<ContextoRelatorioBd> options) : base(options)
        {

        }

    }
}
