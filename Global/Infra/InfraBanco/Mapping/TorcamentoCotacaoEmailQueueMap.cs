using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TorcamentoCotacaoEmailQueueMap : IEntityTypeConfiguration<TorcamentoCotacaoEmailQueue>
    {
        public void Configure(EntityTypeBuilder<TorcamentoCotacaoEmailQueue> builder)
        {
            builder.ToTable("t_ORCAMENTO_COTACAO_EMAIL_QUEUE");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.Property(x => x.IdCfgUnidadeNegocio)
                .HasColumnName("IdCfgUnidadeNegocio")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.From)
                .HasColumnName("From")
                .HasColumnType("varchar")
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(x => x.FromDisplayName)
                .HasColumnName("FromDisplayName")
                .HasColumnType("varchar")
                .HasMaxLength(1024);

            builder.Property(x => x.To)
                .HasColumnName("TO")
                .HasColumnType("varchar")
                .HasMaxLength(1024);

            builder.Property(x => x.Cc)
                .HasColumnName("CC")
                .HasColumnType("varchar")
                .HasMaxLength(1024);

            builder.Property(x => x.Bcc)
                .HasColumnName("BCC")
                .HasColumnType("varchar")
                .HasMaxLength(1024);

            builder.Property(x => x.Subject)
                .HasColumnName("Subject")
                .HasColumnType("varchar")
                .HasMaxLength(240);

            builder.Property(x => x.Body)
                .HasColumnName("Body")
                .HasColumnType("varchar");

            builder.Property(x => x.Sent)
                .HasColumnName("Sent")
                .HasColumnType("bit");

            builder.Property(x => x.DateSent)
                .HasColumnName("DateSent")
                .HasColumnType("DateTime");

            builder.Property(x => x.DateScheduled)
                .HasColumnName("DateScheduled")
                .HasColumnType("DateTime");

            builder.Property(x => x.DateCreated)
                .HasColumnName("DateCreated")
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.AttemptsQty)
                .HasColumnName("AttemptsQty")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.DateLastAttempt)
                .HasColumnName("DateLastAttempt")
                .HasColumnType("DateTime");

            builder.Property(x => x.ErrorMsgLastAttempt)
                .HasColumnName("ErrorMsgLastAttempt")
                .HasColumnType("varchar");

            builder.Property(x => x.Attachment)
                .HasColumnName("Attachment")
                .HasColumnType("varchar");

            //builder.Ignore(x => x.TorcamentoCotacaoItemUnificado);
            //builder.Ignore(x => x.TorcamentoCotacaoItemAtomicoCustoFin);

            //builder
            //    .HasOne(x => x.TorcamentoCotacaoItemUnificado)
            //    .WithMany(o => o.TorcamentoCotacaoOpcaoItemAtomicos)
            //    .HasForeignKey(f => f.IdItemUnificado);

            //builder
            //    .HasOne(x => x.TorcamentoCotacaoItemAtomicoCustoFin)
            //    .WithOne(o => o.TorcamentoCotacaoOpcaoItemAtomico)
            //    .HasForeignKey<TorcamentoCotacaoOpcaoItemAtomico>(f => f.IdItemUnificado);
        }
    }
}
