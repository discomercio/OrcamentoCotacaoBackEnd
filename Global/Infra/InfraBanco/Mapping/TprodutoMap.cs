using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    //COMENTADOS: NAO ESTAO MAPEADOS EN ENTIDADE C#

    public class TprodutoMap : IEntityTypeConfiguration<Tproduto>
    {
        public void Configure(EntityTypeBuilder<Tproduto> builder)
        {
            builder.ToTable("T_produto");
            builder.HasKey(o => new { o.Fabricante, o.Produto });

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasColumnType("varchar(120)");

            builder.Property(x => x.Ean)
                .HasColumnName("ean")
                .HasColumnType("varchar(14)");

            builder.Property(x => x.Grupo)
                .HasColumnName("grupo")
                .HasColumnType("varchar(4)");

            builder.Property(x => x.Preco_Fabricante)
                .HasColumnName("preco_fabricante")
                .HasColumnType("money");

            //builder.Property(x => x.Estoque_Critico)
            //    .HasColumnName("estoque_critico")
            //    .HasColumnType("smallint"); 

            builder.Property(x => x.Peso)
                .HasColumnName("peso")
                .HasColumnType("real");

            builder.Property(x => x.Qtde_Volumes)
                .HasColumnName("qtde_volumes")
                .HasColumnType("smallint");

            //builder.Property(x => x.DtCadastro)
            //    .HasColumnName("dt_cadastro")
            //    .HasColumnType("datetime");

            //builder.Property(x => x.DtUltimaAtualizacao)
            //    .HasColumnName("dt_ult_atualizacao")
            //    .HasColumnType("datetime");

            builder.Property(x => x.Excluido_status)
                .HasColumnName("excluido_status")
                .HasColumnType("smallint");

            builder.Property(x => x.Vl_Custo2)
                .HasColumnName("vl_custo2")
                .HasColumnType("money");

            builder.Property(x => x.Descricao_Html)
                .HasColumnName("descricao_html")
                .HasColumnType("varchar(4000)");

            builder.Property(x => x.Cubagem)
                .HasColumnName("cubagem")
                .HasColumnType("real");

            builder.Property(x => x.Ncm)
                .HasColumnName("ncm")
                .HasColumnType("varchar(8)");

            builder.Property(x => x.Cst)
                .HasColumnName("cst")
                .HasColumnType("varchar(3)");

            //builder.Property(x => x.Perc_MVA_ST)
            //    .HasColumnName("perc_MVA_ST")
            //    .HasColumnType("real");

            //builder.Property(x => x.Deposito_Zona_Id)
            //    .HasColumnName("deposito_zona_id")
            //    .HasColumnType("int");

            //builder.Property(x => x.Deposito_Zona_Usuario_Ult_Atualzacao)
            //    .HasColumnName("deposito_zona_usuario_ult_atualiz")
            //    .HasColumnType("varchar(10)");

            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("deposito_zona_dt_hr_ult_atualiz")
            //    .HasColumnType("datetime");

            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("farol_qtde_comprada")
            //    .HasColumnType("xxxxxxxxxxx");

            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("farol_qtde_comprada_usuario_ult_atualiz")
            //    .HasColumnType("xxxxxxxxxxx");
           
            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("farol_qtde_comprada_dt_hr_ult_atualiz")
            //    .HasColumnType("xxxxxxxxxxx");
           
            builder.Property(x => x.Descontinuado)
                .HasColumnName("descontinuado")
                .HasColumnType("varchar(1)");
          
            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("potencia_BTU")
            //    .HasColumnType("xxxxxxxxxxx");

            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("ciclo")
            //    .HasColumnType("xxxxxxxxxxx");
           
            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("posicao_mercado")
            //    .HasColumnType("xxxxxxxxxxx");
           
            builder.Property(x => x.Subgrupo)
                .HasColumnName("subgrupo")
                .HasColumnType("varchar(10)");
           
            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("cod_produto_xml_fabricante")
            //    .HasColumnType("xxxxxxxxxxx");
           
            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("cod_produto_alfanum_fabricante")
            //    .HasColumnType("xxxxxxxxxxx");
           
            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("potencia_valor")
            //    .HasColumnType("xxxxxxxxxxx");
            
            //builder.Property(x => x.xxxxxxxxx)
            //    .HasColumnName("id_unidade_potencia")
            //    .HasColumnType("xxxxxxxxxxx");
        }
    }
}


