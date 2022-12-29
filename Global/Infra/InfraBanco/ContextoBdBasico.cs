using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;

namespace InfraBanco
{
    public class ContextoBdBasico : DbContext
    {
        internal ContextoBdBasico(DbContextOptions<ContextoBdBasico> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoBdBasico).Assembly);

            modelBuilder.Entity<TclienteRefBancaria>()
                .HasKey(o => new { o.Id_Cliente, o.Banco, o.Agencia, o.Conta });
            modelBuilder.Entity<TclienteRefComercial>()
                .HasKey(o => new { o.Id_Cliente, o.Nome_Empresa });
            modelBuilder.Entity<TpedidoItem>()
                .HasKey(o => new { o.Pedido, o.Fabricante, o.Produto });
            modelBuilder.Entity<TorcamentoItem>(o =>
            {
                o.HasKey(x => new { x.Orcamento, x.Fabricante, x.Produto });
            });

            modelBuilder.Entity<Tproduto>()
                .HasKey(x => new { x.Fabricante, x.Produto });
            //modelBuilder.Entity<Tproduto>()
            //    .HasOne(x => x.TecProdutoComposto)
            //    .WithOne(x => x.Tproduto)
            //    .HasForeignKey<Tproduto>(x => new { x.Fabricante, x.Produto });

            //modelBuilder.Entity<TecProdutoComposto>()
            //    .HasOne(x => x.Tproduto)
            //    .WithOne(x => x.TecProdutoComposto)
            //    .HasForeignKey<TecProdutoComposto>(x => new {x.Fabricante_Composto, x.Produto_Composto});

            modelBuilder.Entity<TecProdutoComposto>()
                .HasKey(x => new { x.Fabricante_Composto, x.Produto_Composto });

            modelBuilder.Entity<TecProdutoCompostoItem>()
                .HasKey(x => new { x.Fabricante_composto, x.Produto_composto, x.Fabricante_item, x.Produto_item });

            modelBuilder.Entity<TprodutoXAlerta>()
                .HasKey(x => new { x.Fabricante, x.Produto, x.Id_Alerta });
            modelBuilder.Entity<TprodutoXwmsRegraCd>()
                .HasKey(x => new { x.Fabricante, x.Produto });

            modelBuilder.Entity<TprodutoLoja>()
                .HasKey(x => new { x.Fabricante, x.Produto, x.Loja });

            modelBuilder.Entity<TalertaProduto>()
                .HasKey(x => x.Apelido);

            modelBuilder.Entity<TsessaoHistorico>()
                .HasKey(x => new { x.Usuario, x.DtHrInicio });
            //vira e mexe dá  Database operation expected to affect 1 row(s) but actually affected 0 row(s).
            //porque o entity lê a data e quando faz o update com essa mesma data l SQL não afeta nenhum registro
            modelBuilder.Entity<TsessaoHistorico>()
                .Property(f => f.DtHrInicio).HasColumnType("datetime");

            //não tem chave no banco, mas é obrigatória para o entity. campos NULL não podem fazer parte.
            modelBuilder.Entity<TsessaoAbandonada>()
                .HasKey(o => new
                {
                    o.Usuario,
                    o.SessaoAbandonadaDtHrInicio,
                    //o.SessaoAbandonadaLoja, 
                    o.SessaoAbandonadaModulo,
                    o.SessaoSeguinteDtHrInicio,
                    //o.SessaoSeguinteLoja ,
                    o.SessaoSeguinteModulo
                });


            modelBuilder.Entity<TperfilUsuario>()
                .HasKey(x => new { x.Usuario, x.Id_perfil });
            modelBuilder.Entity<Tperfil>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Toperacao>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<TperfilItem>()
                .HasKey(x => new { x.Id });
            modelBuilder.Entity<TcodigoDescricao>()
                .HasKey(x => new { x.Grupo, x.Codigo });
            modelBuilder.Entity<TpercentualCustoFinanceiroFornecedor>()
                .HasKey(x => new { x.Fabricante, x.Tipo_Parcelamento, x.Qtde_Parcelas });

            modelBuilder.Entity<TestoqueItem>().HasKey(x => new { x.Id_estoque, x.Fabricante, x.Produto });

            modelBuilder.Entity<TestoqueItem>()
                .HasOne(x => x.Testoque)
                .WithMany(x => x.TestoqueItem)
                .HasForeignKey(x => x.Id_estoque);

            modelBuilder.Entity<TpedidoItemDevolvido>()
                .HasOne(x => x.Tpedido)
                .WithMany(x => x.TpedidoItemDevolvido)
                .HasForeignKey(x => x.Pedido);

            modelBuilder.Entity<TprodutoLoja>()
                .HasOne(x => x.Tproduto)
                .WithMany(x => x.TprodutoLoja)
                .HasForeignKey(x => new { x.Fabricante, x.Produto });

            //não tem chave no banco, mas é obrigatória para o entity. campos NULL não podem fazer parte.
            //não é uma chave única, mas suficiente. Embora os campos permitam null, nãoe xiste nenhum null no banco.
            modelBuilder.Entity<TestoqueLog>()
               .HasKey(x => new
               {
                   x.Pedido_estoque_destino,
                   x.Pedido_estoque_origem,
                   x.Fabricante,
                   x.Produto,
                   x.Qtde_atendida,
                   x.Usuario,
                   x.Operacao,
                   x.Cod_estoque_destino,
                   x.Cod_estoque_origem,
                   x.Data_hora,
                   x.Id_nfe_emitente
               });

            modelBuilder.Entity<TavisoExibido>()
                .HasKey(x => new { x.Id, x.Usuario });

            modelBuilder.Entity<TavisoLido>()
                .HasKey(x => new { x.Id, x.Usuario });


            modelBuilder.Entity<TcfgPagtoFormaStatus>()
                .HasOne(x => x.TcfgModulo)
                .WithMany(x => x.TcfgPagtoFormaStatus)
                .HasForeignKey(x => x.IdCfgModulo);
            modelBuilder.Entity<TcfgPagtoFormaStatus>()
                 .HasOne(x => x.TcfgPagtoForma)
                 .WithMany(x => x.TcfgPagtoFormaStatus)
                 .HasForeignKey(x => x.IdCfgPagtoForma);
            modelBuilder.Entity<TcfgPagtoFormaStatus>()
                .HasOne(x => x.TcfgTipoPessoa)
                .WithMany(x => x.TcfgPagtoFormaStatus)
                .HasForeignKey(x => x.IdCfgTipoPessoaCliente);
            modelBuilder.Entity<TcfgPagtoFormaStatus>()
                .HasOne(x => x.TcfgTipoUsuarios)
                .WithMany(x => x.TcfgPagtoFormaStatus)
                .HasForeignKey(x => x.IdCfgTipoUsuarioPerfil);

            modelBuilder.Entity<TcfgPagtoMeioStatus>()
                .HasOne(x => x.TcfgModulo)
                .WithMany(x => x.TcfgPagtoMeioStatus)
                .HasForeignKey(x => x.IdCfgModulo);
            modelBuilder.Entity<TcfgPagtoMeioStatus>()
                .HasOne(x => x.TcfgPagtoForma)
                .WithMany(x => x.TcfgPagtoMeioStatus)
                .HasForeignKey(x => x.IdCfgPagtoForma);
            modelBuilder.Entity<TcfgPagtoMeioStatus>()
                .HasOne(x => x.TcfgPagtoMeio)
                .WithMany(x => x.TcfgPagtoMeioStatus)
                .HasForeignKey(x => x.IdCfgPagtoMeio);
            modelBuilder.Entity<TcfgPagtoMeioStatus>()
                .HasOne(x => x.TcfgTipoParcela)
                .WithMany(x => x.TcfgPagtoMeioStatus)
                .HasForeignKey(x => x.IdCfgTipoParcela);
            modelBuilder.Entity<TcfgPagtoMeioStatus>()
                .HasOne(x => x.TcfgTipoPessoa)
                .WithMany(x => x.TcfgPagtoMeioStatus)
                .HasForeignKey(x => x.IdCfgTipoPessoaCliente);
            modelBuilder.Entity<TcfgPagtoMeioStatus>()
                .HasOne(x => x.TcfgTipoUsuarios)
                .WithMany(x => x.TcfgPagtoMeioStatus)
                .HasForeignKey(x => x.IdCfgTipoUsuarioPerfil);

            modelBuilder.Entity<TpercentualCustoFinanceiroFornecedorHistorico>()
                .HasKey(x => new { x.Fabricante, x.Tipo_Parcelamento, x.Qtde_Parcelas });

            modelBuilder.Entity<TcfgOrcamentoCotacaoStatus>()
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<TcfgTipoUsuarioContexto>()
                .HasOne(x => x.TorcamentoCotacaoOpcaoPagto)
                .WithOne(o => o.TcfgTipoUsuarioContexto)
                .HasForeignKey<TorcamentoCotacaoOpcaoPagto>(f => f.IdTipoUsuarioContextoAprovado);

            modelBuilder.Entity<Tusuario>()
                .HasOne(o => o.TorcamentoCotacaoOpcaoItemAtomicoCustoFin)
                .WithOne(f => f.Tusuario)
                .HasForeignKey<TorcamentoCotacaoOpcaoItemAtomicoCustoFin>(f => f.IdUsuarioDescontoSuperior)
                .HasPrincipalKey<Tusuario>(p => p.Id);

            modelBuilder.Entity<TorcamentoItem>()
                .HasOne(o => o.Tusuario)
                .WithMany(m => m.TorcamentoItem)
                .HasForeignKey(f => f.IdUsuarioDescontoSuperior)
                .HasPrincipalKey(p => p.Id);

            modelBuilder.Entity<TpedidoItem>()
                .HasOne(o => o.Tusuario)
                .WithMany(m => m.TpedidoItem)
                .HasForeignKey(f => f.IdUsuarioDescontoSuperior)
                .HasPrincipalKey(p => p.Id);

            modelBuilder.Entity<TpedidoItemDevolvido>()
                .HasOne(o => o.Tusuario)
                .WithMany(m => m.TpedidoItemDevolvido)
                .HasForeignKey(f => f.IdUsuarioDescontoSuperior)
                .HasPrincipalKey(p => p.Id);

            modelBuilder.Entity<Tfabricante>()
                .HasMany(o => o.TprodutoCatalogos)
                .WithOne(w => w.Tfabricante)
                .HasForeignKey(f => f.Fabricante);
        }

        public DbSet<Tcliente> Tcliente { get; set; }
        public DbSet<Torcamento> Torcamento { get; set; }
        public DbSet<TcfgUnidadeNegocioParametro> TcfgUnidadeNegocioParametro { get; set; }
        public DbSet<TclienteRefBancaria> TclienteRefBancaria { get; set; }
        public DbSet<Tpedido> Tpedido { get; set; }
        public DbSet<TorcamentistaEindicador> TorcamentistaEindicador { get; set; }
        public DbSet<TorcamentistaEIndicadorVendedor> TorcamentistaEindicadorVendedor { get; set; }
        public DbSet<TsessaoHistorico> TsessaoHistorico { get; set; }
        public DbSet<Tproduto> Tproduto { get; set; }

        public DbSet<TprodutoCatalogo> TprodutoCatalogo { get; set; }
        public DbSet<TprodutoCatalogoImagem> TprodutoCatalogoImagem { get; set; }
        public DbSet<TprodutoCatalogoImagemTipo> TprodutoCatalogoImagemTipo { get; set; }
        public DbSet<TProdutoCatalogoPropriedade> TProdutoCatalogoPropriedade { get; set; }
        public DbSet<TProdutoCatalogoPropriedadeOpcao> TProdutoCatalogoPropriedadeOpcao { get; set; }
        public DbSet<TprodutoCatalogoItem> TprodutoCatalogoItem { get; set; }
        public DbSet<TprodutoCatalogoItens> TprodutoCatalogoItens { get; set; }

        public DbSet<TprodutoLoja> TprodutoLoja { get; set; }
        public DbSet<TpedidoItem> TpedidoItem { get; set; }
        public DbSet<TpedidoItemDevolvido> TpedidoItemDevolvido { get; set; }
        public DbSet<TpedidoPerda> TpedidoPerda { get; set; }
        public DbSet<TpedidoPagamento> TpedidoPagamento { get; set; }
        public DbSet<Ttransportadora> Ttransportadora { get; set; }
        public DbSet<TpedidoBlocosNotas> TpedidoBlocosNotas { get; set; }
        public DbSet<TcodigoDescricao> TcodigoDescricao { get; set; }
        public DbSet<TpedidoOcorrenciaMensagem> TpedidoOcorrenciaMensagem { get; set; }
        public DbSet<TpedidoOcorrencia> TpedidoOcorrencia { get; set; }
        public DbSet<TpedidoItemDevolvidoBlocoNotas> TpedidoItemDevolvidoBlocoNotas { get; set; }
        public DbSet<TorcamentoItem> TorcamentoItem { get; set; }
        public DbSet<Tbanco> Tbanco { get; set; }
        public DbSet<TclienteRefComercial> TclienteRefComercial { get; set; }
        public DbSet<Tlog> Tlog { get; set; }
        public DbSet<Tloja> Tloja { get; set; }
        public DbSet<Tcontrole> Tcontrole { get; set; }
        public DbSet<TnfEmitente> TnfEmitente { get; set; }
        public DbSet<TecProdutoComposto> TecProdutoComposto { get; set; }
        public DbSet<Tfabricante> Tfabricante { get; set; }
        public DbSet<Tparametro> Tparametro { get; set; }
        public DbSet<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedor { get; set; }

        public DbSet<TpercentualCustoFinanceiroFornecedorHistorico> TpercentualCustoFinanceiroFornecedorHistorico { get; set; }
        public DbSet<TprodutoXwmsRegraCd> TprodutoXwmsRegraCd { get; set; }
        public DbSet<TwmsRegraCd> TwmsRegraCd { get; set; }
        public DbSet<TwmsRegraCdXUf> TwmsRegraCdXUf { get; set; }
        public DbSet<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoa { get; set; }
        public DbSet<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCd { get; set; }
        public DbSet<TecProdutoCompostoItem> TecProdutoCompostoItem { get; set; }
        public DbSet<TestoqueItem> TestoqueItem { get; set; }
        public DbSet<TprodutoXAlerta> TprodutoXAlerta { get; set; }
        public DbSet<TalertaProduto> TalertaProduto { get; set; }
        public DbSet<TformaPagto> TformaPagto { get; set; }
        public DbSet<TorcamentistaEIndicadorRestricaoFormaPagto> TorcamentistaEIndicadorRestricaoFormaPagto { get; set; }
        public DbSet<TprazoPagtoVisanet> TprazoPagtoVisanet { get; set; }
        public DbSet<TprodutoSubgrupo> TprodutoSubgrupo { get; set; }
        public DbSet<Tusuario> Tusuario { get; set; }
        public DbSet<TsessaoAbandonada> TsessaoAbandonada { get; set; }
        public DbSet<Tperfil> Tperfil { get; set; }
        public DbSet<TperfilUsuario> TperfilUsuario { get; set; }
        public DbSet<Testoque> Testoque { get; set; }
        public DbSet<TestoqueMovimento> TestoqueMovimento { get; set; }
        public DbSet<TcfgModulo> TcfgModulo { get; set; }
        public DbSet<TcfgPagtoForma> TcfgPagtoForma { get; set; }
        public DbSet<TcfgPagtoFormaStatus> TcfgPagtoFormaStatus { get; set; }
        public DbSet<TcfgPagtoMeio> TcfgPagtoMeio { get; set; }
        public DbSet<TcfgPagtoMeioStatus> TcfgPagtoMeioStatus { get; set; }
        public DbSet<TcfgTipoParcela> TcfgTipoParcela { get; set; }
        public DbSet<TcfgTipoPessoa> TcfgTipoPessoa { get; set; }
        public DbSet<TcfgTipoUsuario> TcfgTipoUsuario { get; set; }
        public DbSet<TcfgTipoUsuarioPerfil> TcfgTipoUsuarioPerfil { get; set; }
        public DbSet<TcfgTipoUsuarioContexto> TcfgTipoUsuarioContexto { get; set; }
        public DbSet<TcfgDataType> TcfgDataType { get; set; }
        public DbSet<TcfgTipoPermissaoEdicaoCadastro> TcfgTipoPermissaoEdicaoCadastro { get; set; }
        public DbSet<TcfgTipoPropriedadeProdutoCatalogo> TcfgTipoPropriedadeProdutoCatalogo { get; set; }

        public DbSet<TorcamentoCotacao> TorcamentoCotacao { get; set; }
        public DbSet<TorcamentoCotacaoLink> TorcamentoCotacaoLink { get; set; }
        public DbSet<TorcamentoCotacaoMensagem> TorcamentoCotacaoMensagem { get; set; }
        public DbSet<TorcamentoCotacaoMensagemStatus> TorcamentoCotacaoMensagemStatus { get; set; }
        public DbSet<TcfgOrcamentoCotacaoStatus> TcfgOrcamentoCotacaoStatus { get; set; }
        public DbSet<TcfgOrcamentoCotacaoEmailTemplate> TcfgOrcamentoCotacaoEmailTemplate { get; set; }
        public DbSet<TorcamentoCotacaoOpcao> TorcamentoCotacaoOpcao { get; set; }
        public DbSet<TorcamentoCotacaoEmailQueue> TorcamentoCotacaoEmailQueue { get; set; }
        public DbSet<TorcamentoCotacaoEmail> TorcamentoCotacaoEmail { get; set; }
        public DbSet<TorcamentoCotacaoOpcaoPagto> TorcamentoCotacaoOpcaoPagto { get; set; }
        public DbSet<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificado { get; set; }
        public DbSet<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoOpcaoItemAtomicoCustoFin { get; set; }
        public DbSet<TorcamentoCotacaoOpcaoItemAtomico> TorcamentoCotacaoOpcaoItemAtomico { get; set; }
        public DbSet<TcfgUnidadeNegocio> TcfgUnidadeNegocio { get; set; }
        public DbSet<TcfgParametro> TcfgParametro { get; set; }
        public DbSet<TusuarioXLoja> TusuarioXLoja { get; set; }
        public DbSet<Toperacao> Toperacao { get; set; }
        public DbSet<TperfilItem> TperfilItem { get; set; }
        public DbSet<TtransportadoraCep> TtransportadoraCep { get; set; }
        public DbSet<TpedidoAnaliseEndereco> TpedidoAnaliseEndereco { get; set; }
        public DbSet<TpedidoAnaliseEnderecoConfrontacao> TpedidoAnaliseConfrontacao { get; set; }
        public DbSet<TfinControle> TfinControle { get; set; }
        public DbSet<Tdesconto> Tdesconto { get; set; }
        public DbSet<TpedidoDevolucao> TpedidoDevolucao { get; set; }
        public DbSet<TestoqueLog> TestoqueLog { get; set; }
        public DbSet<TorcamentoCotacaoArquivos> TorcamentoCotacaoArquivos { get; set; }
        public DbSet<Taviso> Taviso { get; set; }
        public DbSet<TavisoExibido> TavisoExibidos { get; set; }
        public DbSet<TavisoLido> TavisoLido { get; set; }
        public DbSet<TperfilItem> TperfilIten { get; set; }
        public DbSet<TorcamentistaEIndicadorVendedor> TorcamentistaEIndicadorVendedor { get; set; }
        public DbSet<TLogV2> TlogV2{ get; set; }
        public DbSet<TcfgOperacao> TcfgOperacao { get; set;}
        public DbSet<TloginHistorico> TloginHistorico { get; set; }
        public DbSet<TemailLsndsvcRemetente> TemailLsndsvcRemetente { get; set; }
        public DbSet<TemailSndsvcMensagem> TemailSndsvcMensagem { get; set; }
    }
}