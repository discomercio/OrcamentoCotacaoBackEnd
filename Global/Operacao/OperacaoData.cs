using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Operacao
{
    public class OperacaoData : BaseData<Toperacao, ToperacaoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OperacaoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public Toperacao Atualizar(Toperacao obj)
        {
            throw new NotImplementedException();
        }

        public Toperacao AtualizarComTransacao(Toperacao model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(Toperacao obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(Toperacao obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public Toperacao Inserir(Toperacao obj)
        {
            throw new NotImplementedException();
        }

        public Toperacao InserirComTransacao(Toperacao model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<Toperacao> PorFilroComTransacao(ToperacaoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<Toperacao> PorFiltro(ToperacaoFiltro obj)
        {
            throw new NotImplementedException();
        }
    }
}
