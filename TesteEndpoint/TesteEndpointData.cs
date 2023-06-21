using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InfraBanco.ContextoBdGravacao;

namespace TesteEndpoint
{
    public class TesteEndpointData : BaseData<TEndpoints, EndpointsFiltro>
    {
        private readonly ContextoBdProvider _contextoProvider;
        public TesteEndpointData(ContextoBdProvider _contextoProvider)
        {
            this._contextoProvider = _contextoProvider;
        }
        public TEndpoints Atualizar(TEndpoints obj)
        {
            throw new NotImplementedException();
        }

        public TEndpoints AtualizarComTransacao(TEndpoints model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TEndpoints obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TEndpoints obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TEndpoints Inserir(TEndpoints obj)
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
            {
                db.TEndpoints.Add(obj);
                db.SaveChanges();

                return obj;
            }
        }

        public TEndpoints InserirComTransacao(TEndpoints model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.TEndpoints.Add(model);
            contextoBdGravacao.SaveChanges();

            return model;
        }

        public List<TEndpoints> PorFilroComTransacao(EndpointsFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TEndpoints> PorFiltro(EndpointsFiltro obj)
        {
            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(BloqueioTControle.NENHUM))
            {
                var query = db.TEndpoints;

                return query.ToList();
            }
        }
    }
}
