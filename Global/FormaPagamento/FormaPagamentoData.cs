using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using InfraBanco.Constantes;

namespace FormaPagamento
{
    public class FormaPagamentoData : BaseData<TcfgPagtoFormaStatus, TcfgPagtoFormaStatusFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public FormaPagamentoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TcfgPagtoFormaStatus Atualizar(TcfgPagtoFormaStatus obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcfgPagtoFormaStatus obj)
        {
            throw new NotImplementedException();
        }

        public TcfgPagtoFormaStatus Inserir(TcfgPagtoFormaStatus obj)
        {
            throw new NotImplementedException();
        }

        public List<TcfgPagtoFormaStatus> PorFiltro(TcfgPagtoFormaStatusFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = from formaPagto in db.TcfgPagtoFormaStatus
                                select formaPagto;

                    if (obj.IncluirTcfgPagtoForma)
                    {
                        saida = saida.Include(x => x.TcfgPagtoForma);
                    }
                    if (obj.IdCfgModulo != 0)
                    {
                        saida = saida.Where(x => x.IdCfgModulo == obj.IdCfgModulo);
                    }
                    if (obj.IdCfgTipoPessoaCliente != 0)
                    {
                        saida = saida.Where(x => x.IdCfgTipoPessoaCliente == obj.IdCfgTipoPessoaCliente);
                    }
                    if (obj.IdCfgTipoUsuarioPerfil != 0)
                    {
                        saida = saida.Where(x => x.IdCfgTipoUsuarioPerfil == obj.IdCfgTipoUsuarioPerfil);
                    }
                    if (obj.PedidoComIndicador.HasValue)
                    {
                        saida = saida.Where(x => x.PedidoComIndicador == obj.PedidoComIndicador);
                    }
                    if (obj.Habilitado.HasValue)
                    {
                        saida = saida.Where(x => x.Habilitado == obj.Habilitado);
                    }

                    return saida.ToList();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public async Task<int> GetMaximaQtdeParcelasCartaoVisa()
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    return await (from c in db.TprazoPagtoVisanets
                            where c.Tipo == Constantes.COD_VISANET_PRAZO_PAGTO_LOJA
                            select c.Qtde_parcelas).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            
        }

        public TcfgPagtoFormaStatus InserirComTransacao(TcfgPagtoFormaStatus model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }
    }
}
