using ClassesBase;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrcamentistaEindicador
{
    public class OrcamentistaEindicadorBll : BaseData<TorcamentistaEindicador, TorcamentistaEindicadorFiltro>
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        public OrcamentistaEindicadorBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentistaEindicador Atualizar(TorcamentistaEindicador obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentistaEindicador obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentistaEindicador Inserir(TorcamentistaEindicador obj)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentistaEindicador> PorFiltro(TorcamentistaEindicadorFiltro obj)
        {
            throw new NotImplementedException();
        }

        public Task<TorcamentistaEindicador> GetParceiros(string vendedorId, int v1, int v2)
        {
            throw new NotImplementedException();
        }

        public Task<TorcamentistaEindicador> GetParceirosByVendedor(string vendedor)
        {
            throw new NotImplementedException();
        }

        public List<Tusuario> GetVendedoresDoParceiro(string parceiro)
        {
            throw new NotImplementedException();
        }
    }
}
