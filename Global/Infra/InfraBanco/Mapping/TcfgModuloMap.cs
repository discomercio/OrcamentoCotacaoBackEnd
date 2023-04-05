using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraBanco.Mapping
{
    public class TcfgModuloMap : IEntityTypeConfiguration<TcfgModulo>
    {
        public void Configure(EntityTypeBuilder<TcfgModulo> builder)
        {
            builder.ToTable("t_CFG_MODULO");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Ordenacao)
                .HasColumnName("Ordenacao")
                .HasColumnType("smallint")
                .IsRequired();
        }
    }
}
