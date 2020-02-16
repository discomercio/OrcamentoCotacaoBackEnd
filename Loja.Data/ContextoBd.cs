using Loja.Modelo;
using Loja.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loja.Data
{
    public class ContextoBd
    {
        private readonly ContextoBdBasico contexto;
        internal ContextoBd(ContextoBdBasico contexto)
        {
            this.contexto = contexto;
            //sem nenhum rastreamento de mudanças na conexao (a rigor, com isto não precisamos dos AsNoTracking)
            contexto.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        //necessário apra poder mandar comandos SQL diretamente
        public ContextoBdBasico GetContextoBdBasicoParaSql()
        {
            return contexto;
        }

        public IQueryable<Tcliente> Tclientes { get => contexto.Tclientes.AsNoTracking(); }
        //afazer falta fazer
        public IQueryable<Torcamento> Torcamentos { get => contexto.Torcamentos.AsNoTracking(); }
        public IQueryable<TclienteRefBancaria> TclienteRefBancarias { get => contexto.TclienteRefBancarias.AsNoTracking(); }
        public IQueryable<Tpedido> Tpedidos { get => contexto.Tpedidos.AsNoTracking(); }
        public IQueryable<TorcamentistaEindicador> TorcamentistaEindicadors { get => contexto.TorcamentistaEindicadors.AsNoTracking(); }
        public IQueryable<TsessaoHistorico> TsessaoHistoricos { get => contexto.TsessaoHistoricos.AsNoTracking(); }
        public IQueryable<Tusuario> Tusuarios { get => contexto.Tusuarios.AsNoTracking(); }
        public IQueryable<TusuarioXLoja> TusuarioXLojas { get => contexto.TusuarioXLojas.AsNoTracking(); }
        public IQueryable<Tproduto> Tprodutos { get => contexto.Tprodutos.AsNoTracking(); }
        public IQueryable<TprodutoLoja> TprodutoLojas { get => contexto.TprodutoLojas.AsNoTracking(); }
        public IQueryable<TpedidoItem> TpedidoItems { get => contexto.TpedidoItems.AsNoTracking(); }
        public IQueryable<TpedidoItemDevolvido> TpedidoItemDevolvidos { get => contexto.TpedidoItemDevolvidos.AsNoTracking(); }
        public IQueryable<TpedidoPerda> TpedidoPerdas { get => contexto.TpedidoPerdas.AsNoTracking(); }
        public IQueryable<TpedidoPagamento> TpedidoPagamentos { get => contexto.TpedidoPagamentos.AsNoTracking(); }
        public IQueryable<TestoqueMovimento> TestoqueMovimentos { get => contexto.TestoqueMovimentos.AsNoTracking(); }
        public IQueryable<Ttransportadora> Ttransportadoras { get => contexto.Ttransportadoras.AsNoTracking(); }
        public IQueryable<TpedidoBlocosNotas> TpedidoBlocosNotas { get => contexto.TpedidoBlocosNotas.AsNoTracking(); }
        public IQueryable<TcodigoDescricao> TcodigoDescricaos { get => contexto.TcodigoDescricaos.AsNoTracking(); }
        public IQueryable<TpedidoOcorrenciaMensagem> TpedidoOcorrenciaMensagems { get => contexto.TpedidoOcorrenciaMensagems.AsNoTracking(); }
        public IQueryable<TpedidoOcorrencia> TpedidoOcorrencias { get => contexto.TpedidoOcorrencias.AsNoTracking(); }
        public IQueryable<TpedidoItemDevolvidoBlocoNotas> TpedidoItemDevolvidoBlocoNotas { get => contexto.TpedidoItemDevolvidoBlocoNotas.AsNoTracking(); }
        public IQueryable<TorcamentoItem> TorcamentoItems { get => contexto.TorcamentoItems.AsNoTracking(); }
        public IQueryable<Tbanco> Tbancos { get => contexto.Tbancos.AsNoTracking(); }
        public IQueryable<TclienteRefComercial> TclienteRefComercials { get => contexto.TclienteRefComercials.AsNoTracking(); }
        public IQueryable<Tlog> Tlogs { get => contexto.Tlogs.AsNoTracking(); }
        public IQueryable<Tloja> Tlojas { get => contexto.Tlojas.AsNoTracking(); }
        public IQueryable<Tcontrole> Tcontroles { get => contexto.Tcontroles.AsNoTracking(); }
        public IQueryable<TnfEmitente> TnfEmitentes { get => contexto.TnfEmitentes.AsNoTracking(); }
        public IQueryable<TecProdutoComposto> TecProdutoCompostos { get => contexto.TecProdutoCompostos.AsNoTracking(); }
        public IQueryable<Tfabricante> Tfabricantes { get => contexto.Tfabricantes.AsNoTracking(); }
        public IQueryable<Tparametro> Tparametros { get => contexto.Tparametros.AsNoTracking(); }
        public IQueryable<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedors { get => contexto.TpercentualCustoFinanceiroFornecedors.AsNoTracking(); }
        public IQueryable<TprodutoXwmsRegraCd> TprodutoXwmsRegraCds { get => contexto.TprodutoXwmsRegraCds.AsNoTracking(); }
        public IQueryable<TwmsRegraCd> TwmsRegraCds { get => contexto.TwmsRegraCds.AsNoTracking(); }
        public IQueryable<TwmsRegraCdXUf> TwmsRegraCdXUfs { get => contexto.TwmsRegraCdXUfs.AsNoTracking(); }
        public IQueryable<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoas { get => contexto.TwmsRegraCdXUfPessoas.AsNoTracking(); }
        public IQueryable<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCds { get => contexto.TwmsRegraCdXUfXPessoaXCds.AsNoTracking(); }
        public IQueryable<TecProdutoCompostoItem> TecProdutoCompostoItems { get => contexto.TecProdutoCompostoItems.AsNoTracking(); }
        public IQueryable<Testoque> Testoques { get => contexto.Testoques.AsNoTracking(); }
        public IQueryable<TestoqueItem> TestoqueItems { get => contexto.TestoqueItems.AsNoTracking(); }
        public IQueryable<TprodutoXAlerta> TprodutoXAlertas { get => contexto.TprodutoXAlertas.AsNoTracking(); }
        public IQueryable<TalertaProduto> TalertaProdutos { get => contexto.TalertaProdutos.AsNoTracking(); }
        public IQueryable<TformaPagto> TformaPagtos { get => contexto.TformaPagtos.AsNoTracking(); }
        public IQueryable<TorcamentistaEIndicadorRestricaoFormaPagto> torcamentistaEIndicadorRestricaoFormaPagtos { get => contexto.torcamentistaEIndicadorRestricaoFormaPagtos.AsNoTracking(); }
        public IQueryable<TprazoPagtoVisanet> TprazoPagtoVisanets { get => contexto.TprazoPagtoVisanets.AsNoTracking(); }
        public IQueryable<Tperfil> Tperfils { get => contexto.Tperfils.AsNoTracking(); }
        public IQueryable<TperfilItem> TperfilItems { get => contexto.TperfilItems.AsNoTracking(); }
        public IQueryable<TperfilUsuario> TperfiUsuarios { get => contexto.TperfilUsuarios.AsNoTracking(); }
        public IQueryable<Toperacao> Toperacaos { get => contexto.Toperacaos.AsNoTracking(); }
    }
}
