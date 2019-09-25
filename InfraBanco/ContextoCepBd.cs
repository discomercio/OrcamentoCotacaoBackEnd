using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco
{
    public class ContextoCepBd : DbContext
    {
        public ContextoCepBd(DbContextOptions<ContextoCepBd> opt) : base(opt)
        {

        }
    }
}
