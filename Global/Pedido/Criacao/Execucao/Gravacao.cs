using InfraBanco.Modelos;
using System;
using System.Collections.Generic;

namespace Pedido.Criacao.Execucao
{
    public class Gravacao
    {
        #region Id_pedido_base
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
        #endregion

        #region Tpedido_pai
        public Tpedido Tpedido_pai
        {
            get
            {
                if (_tpedido_pai == null)
                    throw new ApplicationException($"_tpedido_pai  acessado antes de ser calculado.");
                return _tpedido_pai;
            }
            set => _tpedido_pai = value;
        }
        private Tpedido? _tpedido_pai;
        #endregion

        #region Hora_pedido
        public string Hora_pedido
        {
            get
            {
                if (_hora_pedido == null)
                    throw new ApplicationException($"_hora_pedido acessado antes de ser calculado.");
                return _hora_pedido;
            }
            set => _hora_pedido = value;
        }
        private string? _hora_pedido;
        #endregion
        #region DataHoraCriacao
        public DateTime DataHoraCriacao = DateTime.Now;
        #endregion

        #region ListaRegrasControleEstoque
        public List<Produto.RegrasCrtlEstoque.RegrasBll> ListaRegrasControleEstoque
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
        #endregion

        #region EmpresasAutoSplit
        //este é simplesmente uma lsita de ints, mas encapsulamos em uma classe
        public class EmpresaAutoSplitDados
        {
            public EmpresaAutoSplitDados(int id_nfe_emitente)
            {
                Id_nfe_emitente = id_nfe_emitente;
            }

            public int Id_nfe_emitente { get; }
        }
        public List<EmpresaAutoSplitDados> EmpresasAutoSplit
        {
            get
            {
                if (_empresasAutoSplit == null)
                    throw new ApplicationException($"_empresasAutoSplit acessado antes de ser calculado.");
                return _empresasAutoSplit;
            }
            set => _empresasAutoSplit = value;
        }
        private List<EmpresaAutoSplitDados>? _empresasAutoSplit = null;
        #endregion

        #region VlogAutoSplit
        //preenchido no Grava60 e usado no Grava90
        public readonly List<string> VlogAutoSplit = new List<string>();
        #endregion

        #region Log_cliente_indicador
        public string Log_cliente_indicador
        {
            get
            {
                if (_log_cliente_indicador == null)
                    throw new ApplicationException($"_log_cliente_indicador  acessado antes de ser calculado.");
                return _log_cliente_indicador;
            }
            set => _log_cliente_indicador = value;
        }
        private string? _log_cliente_indicador;
        #endregion
    }

}

