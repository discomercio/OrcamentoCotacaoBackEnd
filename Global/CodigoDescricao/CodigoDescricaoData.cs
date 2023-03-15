using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodigoDescricao
{
    public class CodigoDescricaoData : BaseData<TcodigoDescricao, TcodigoDescricaoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public CodigoDescricaoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TcodigoDescricao Atualizar(TcodigoDescricao obj)
        {
            throw new NotImplementedException();
        }

        public TcodigoDescricao AtualizarComTransacao(TcodigoDescricao model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcodigoDescricao obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TcodigoDescricao obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TcodigoDescricao Inserir(TcodigoDescricao obj)
        {
            throw new NotImplementedException();
        }

        public TcodigoDescricao InserirComTransacao(TcodigoDescricao model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcodigoDescricao> PorFilroComTransacao(TcodigoDescricaoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcodigoDescricao> PorFiltro(TcodigoDescricaoFiltro obj)
        {
            using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var saida = from c in db.TcodigoDescricao
                            select c;

                if (!string.IsNullOrEmpty(obj.Grupo))
                {
                    saida = saida.Where(x => x.Grupo == obj.Grupo);
                }
                if (!string.IsNullOrEmpty(obj.Codigo))
                {
                    saida = saida.Where(x => x.Codigo == obj.Codigo);
                }

                return saida.ToList();
            }
        }
    }
}
