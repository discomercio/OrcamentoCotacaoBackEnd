using Produto.RegrasCrtlEstoque;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Criacao.Passo60.Gravacao
{
    class ExecucaoDados
    {


        public string Id_pedido_base
        {
            get
            {
                if (_id_pedido_base == null)
                    throw new ApplicationException($"Id_pedido_base acessado antes de ser calculado.");
                return _id_pedido_base;
            }
            set => _id_pedido_base = value;
        }
        private string? _id_pedido_base;


        public List<RegrasBll> RegrasControleEstoque
        {
            get
            {
                if (_regrasControleEstoque == null)
                    throw new ApplicationException($"RegrasControleEstoque acessado antes de ser calculado.");
                return _regrasControleEstoque;
            }
            set => _regrasControleEstoque = value;
        }
        private List<Produto.RegrasCrtlEstoque.RegrasBll>? _regrasControleEstoque = null;
    }
}
