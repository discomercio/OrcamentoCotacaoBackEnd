using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using OrcamentistaEindicador;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentistaEIndicadorVendedor
{
    public class OrcamentistaEIndicadorVendedorBll : BaseBLL<TorcamentistaEIndicadorVendedor, TorcamentistaEIndicadorVendedorFiltro>
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        public OrcamentistaEIndicadorVendedorData _data { get; set; }
        public OrcamentistaEIndicadorData _dataIndicador { get; set; }

        public OrcamentistaEIndicadorVendedorBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentistaEIndicadorVendedorData(contextoBdProvider))
        {
            this.contextoProvider = contextoBdProvider;            
            _data = (OrcamentistaEIndicadorVendedorData)base.data;
            _dataIndicador = new OrcamentistaEIndicadorData(contextoBdProvider);
        }

        public TorcamentistaEIndicadorVendedor Inserir(TorcamentistaEIndicadorVendedor objOrcamentistaEIndicadorVendedor, string parceiro, string vendedor)
        {
            var oei = _dataIndicador.PorFiltro(new TorcamentistaEindicadorFiltro()
            {
                apelido = parceiro
            });

            if(!oei.Any())
                throw new KeyNotFoundException();
            objOrcamentistaEIndicadorVendedor.IdIndicador = oei.First().Id;
            objOrcamentistaEIndicadorVendedor.UsuarioCadastro = vendedor;
            objOrcamentistaEIndicadorVendedor.UsuarioUltimaAlteracao = vendedor;
            objOrcamentistaEIndicadorVendedor.DataCadastro = DateTime.Now;
            objOrcamentistaEIndicadorVendedor.DataUltimaAlteracao = DateTime.Now;
            objOrcamentistaEIndicadorVendedor.DataUltimaAlteracaoSenha = DateTime.Now;

            return _data.Inserir(objOrcamentistaEIndicadorVendedor);
        }


        public TorcamentistaEIndicadorVendedor Atualizar(TorcamentistaEIndicadorVendedor objOrcamentistaEIndicadorVendedor, string parceiro, string vendedor)
        {
            var oeiv = _data.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { id = objOrcamentistaEIndicadorVendedor.Id }).FirstOrDefault();
            if(oeiv == null) throw new KeyNotFoundException();

            if (oeiv.VendedorResponsavel != parceiro)
                throw new Exception("Não é permitido alterar um usuário de outro vendendor responsável");

            oeiv.Nome = objOrcamentistaEIndicadorVendedor.Nome;
            oeiv.Celular = objOrcamentistaEIndicadorVendedor.Celular;
            oeiv.Telefone = objOrcamentistaEIndicadorVendedor.Telefone;
            oeiv.Email = objOrcamentistaEIndicadorVendedor.Email;
            oeiv.Ativo = objOrcamentistaEIndicadorVendedor.Ativo;
            oeiv.UsuarioUltimaAlteracao = vendedor;
            oeiv.DataUltimaAlteracao = DateTime.Now;
            oeiv.DataUltimaAlteracaoSenha = DateTime.Now;

            return _data.Atualizar(oeiv);
        }
    }
}
