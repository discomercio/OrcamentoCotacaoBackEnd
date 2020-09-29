using Loja.Bll.Dto.PrepedidoDto;
using Loja.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Loja.Bll.Bll.AcessoBll;
using System.Threading.Tasks;

#nullable enable
namespace Loja.Bll.Bll.PrepedidoBll
{
    public class PrepedidoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly ProdutoBll.ProdutoBll produtoBll;
        private readonly ClienteBll.ClienteBll clienteBll;

        public PrepedidoBll(InfraBanco.ContextoBdProvider contextoProvider, ProdutoBll.ProdutoBll produtoBll, ClienteBll.ClienteBll clienteBll)
        {
            this.contextoProvider = contextoProvider;
            this.produtoBll = produtoBll;
            this.clienteBll = clienteBll;
        }

        public async Task<ResumoPrepedidoListaDto> ResumoPrepedidoLista(UsuarioLogado usuarioLogado)
        {
            var db = contextoProvider.GetContextoLeitura();
            var sql = from t_ORCAMENTO in db.Torcamentos
                      join t_CLIENTE in db.Tclientes on t_ORCAMENTO.Id_Cliente equals t_CLIENTE.Id
                      select new
                      {
                          t_ORCAMENTO.Data,
                          t_ORCAMENTO.Orcamento,
                          t_ORCAMENTO.Vl_Total,
                          t_CLIENTE.Nome_Iniciais_Em_Maiusculas,
                          t_ORCAMENTO.Orcamentista,
                          t_ORCAMENTO.Loja,
                          //adicionais para o where
                          t_ORCAMENTO.St_Fechamento,
                          t_ORCAMENTO.St_Orcamento,
                          t_ORCAMENTO.St_Orc_Virou_Pedido,
                          t_ORCAMENTO.Vendedor
                      };

            //'	CRITÉRIO: STATUS DE FECHAMENTO DO ORÇAMENTOS
            sql = sql.Where(r => string.IsNullOrEmpty(r.St_Fechamento));

            //'	CRITÉRIO: STATUS DO ORÇAMENTO
            sql = sql.Where(r => string.IsNullOrEmpty(r.St_Orcamento));

            //'	CRITÉRIO: ORÇAMENTOS QUE NÃO VIRARAM PEDIDOS (NOVO CAMPO DE CONTROLE)
            sql = sql.Where(r => r.St_Orc_Virou_Pedido == 0);

            //'	CRITÉRIO: LOJA (CADA LOJA SÓ PODE CONSULTAR SEUS PRÓPRIOS ORÇAMENTOS)
            //mostramos todas as lojas e o usuário pode filtar se ele tiver a permissão
            if (!usuarioLogado.Operacao_permitida(Constantes.Constantes.OP_LJA_LOGIN_TROCA_RAPIDA_LOJA))
            {
                sql = sql.Where(r => r.Loja == usuarioLogado.Loja_atual_id);
            }

            //'	VERIFICA PERMISSÃO DE ACESSO DO USUÁRIO
            if (!usuarioLogado.Operacao_permitida(Constantes.Constantes.OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO))
            {
                //'	CRITÉRIO: VENDEDOR (PODE ACESSAR ORÇAMENTOS DE TODOS OS VENDEDORES DA LOJA?)
                sql = sql.Where(r => r.Vendedor == usuarioLogado.Usuario_atual);
            }

            //agora o select dos campos que vamos usar
            var dadosSql = sql.Select(r => new
            {
                r.Loja,
                r.Data,
                r.Orcamento,
                r.Vl_Total,
                r.Nome_Iniciais_Em_Maiusculas,
                r.Orcamentista,
            });


            //executar consulta
            var dados = await dadosSql.ToListAsync();

            //e transferir para a saida
            var ret = new ResumoPrepedidoListaDto();
            foreach (var dado in dados)
                ret.Itens.Add(new ResumoPrepedidoDto(dado.Loja, dado.Orcamentista, dado.Orcamento, dado.Nome_Iniciais_Em_Maiusculas, dado.Data, dado.Vl_Total ?? 0));

            return ret;
        }
    }
}
