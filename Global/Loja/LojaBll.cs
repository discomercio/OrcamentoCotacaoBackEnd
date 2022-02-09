using ClassesBase;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Loja
{
    public class LojaBll //: BaseData<Tloja, TlojaFiltro>
    {
        public LojaBll(ContextoBdProvider contextoBdProvider) : base(new LojaData(contextoBdProvider))
        {
            this.contextoProvider = contextoProvider;
        }
        public Tloja Atualizar(Tloja obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(Tloja obj)
        {
            throw new NotImplementedException();
        }

        public Tloja Inserir(Tloja obj)
        {
            throw new NotImplementedException();
        }

        public List<Tloja> PorFiltro(TlojaFiltro obj)
        {
            try
            {
                /*SELECT orc.Nome, orc.email, orc.nome, orc.IdParceiro, 
                   orc.ativo, toei.vendedor as IdVendedor FROM t_ORCAMENTISTA_E_INDICADOR_VENDEDOR orc 
                   INNER JOIN t_ORCAMENTISTA_E_INDICADOR toei ON toei.apelido = orc.IdParceiro 
                   WHERE orc.email = @login AND orc.senha = @senha*/
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var lojas = from L in db.Tlojas
                                select L;

                    if (!string.IsNullOrEmpty(obj.Loja))
                    {
                        lojas = lojas.Where(x => x.Loja == obj.Loja);
                    }

                    return lojas.ToList();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
