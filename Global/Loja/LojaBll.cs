using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System.Collections.Generic;

namespace Loja
{
    public class LojaBll 
    {
        private readonly LojaData _lojaData;

        public LojaBll(LojaData lojaData) 
        {
            _lojaData = lojaData;
        }

        public Tloja Atualizar(Tloja obj)
        {
            return _lojaData.Atualizar(obj);
        }

        public bool Excluir(Tloja obj)
        {
            return _lojaData.Excluir(obj);
        }

        public Tloja Inserir(Tloja obj)
        {
            return _lojaData.Inserir(obj);
        }

        public List<Tloja> PorFiltro(TlojaFiltro obj)
        {
            return _lojaData.PorFiltro(obj);
        }

        public object BuscarPercMaxPorLoja(string loja)
        {
            return _lojaData.BuscarPercMaxPorLoja(loja);
        }
    }
}
