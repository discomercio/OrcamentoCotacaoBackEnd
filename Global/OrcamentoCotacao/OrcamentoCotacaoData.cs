using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace OrcamentoCotacao
{
    public class OrcamentoCotacaoData:BaseData<TorcamentoCotacao, TorcamentoCotacaoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentoCotacao Atualizar(TorcamentoCotacao obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentoCotacao obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacao Inserir(TorcamentoCotacao obj)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacao> PorFiltro(TorcamentoCotacaoFiltro obj)
        {
            try
            {
                using(var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = from c in db.TorcamentoCotacao
                                select c;

                    if (obj.LimitarData)
                    {
                        saida = saida.Where(x => x.DataCadastro > DateTime.Now.AddDays(-60));
                    }
                    if (!string.IsNullOrEmpty(obj.Loja))
                    {
                        saida = saida.Where(x => x.Loja == obj.Loja);
                    }
                    if (!string.IsNullOrEmpty(obj.Status))
                    {
                        saida = saida.Include(x => x.TcfgOrcamentoCotacaoStatus.Equals(obj.Status));
                    }
                    if (obj.Tusuario)
                    {
                        saida = saida.Include(x => x.Tusuario);
                    }

                    return saida.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
