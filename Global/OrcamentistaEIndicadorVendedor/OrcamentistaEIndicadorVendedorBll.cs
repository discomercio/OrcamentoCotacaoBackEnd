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
        public OrcamentistaEIndicadorVendedorData _data { get; set; }
        public OrcamentistaEIndicadorData _dataIndicador { get; set; }

        public OrcamentistaEIndicadorVendedorBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentistaEIndicadorVendedorData(contextoBdProvider))
        {
            _data = (OrcamentistaEIndicadorVendedorData)base.data;
            _dataIndicador = new OrcamentistaEIndicadorData(contextoBdProvider);
        }

        public TorcamentistaEIndicadorVendedor Inserir(TorcamentistaEIndicadorVendedor objOrcamentistaEIndicadorVendedor, string senha, string parceiro, string vendedor)
        {
            var oei = _dataIndicador.PorFiltro(new TorcamentistaEindicadorFiltro()
            {
                apelido = parceiro
            });

            if(!oei.Any())
                throw new KeyNotFoundException();

            var oeiv = _data.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro()
            {
                email = objOrcamentistaEIndicadorVendedor.Email
            });

            if (oeiv.Any())
                throw new ArgumentException("Email já cadastrado");


            string senha_codificada = UtilsGlobais.Util.codificaDado(senha, false);

            if (string.IsNullOrEmpty(senha_codificada))
                throw new ArgumentException("Falha na codificação de senha.");

            objOrcamentistaEIndicadorVendedor.Datastamp = senha_codificada;

            objOrcamentistaEIndicadorVendedor.IdIndicador = oei.First().Id;
            objOrcamentistaEIndicadorVendedor.UsuarioCadastro = parceiro;
            objOrcamentistaEIndicadorVendedor.UsuarioUltimaAlteracao = parceiro;
            objOrcamentistaEIndicadorVendedor.DataCadastro = DateTime.Now;
            objOrcamentistaEIndicadorVendedor.DataUltimaAlteracao = DateTime.Now;
            objOrcamentistaEIndicadorVendedor.DataUltimaAlteracaoSenha = null;

            return _data.Inserir(objOrcamentistaEIndicadorVendedor);
        }


        public TorcamentistaEIndicadorVendedor Atualizar(TorcamentistaEIndicadorVendedor objOrcamentistaEIndicadorVendedor, string senha, string parceiro, string vendedor)
        {
            var oeiv = _data.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { id = objOrcamentistaEIndicadorVendedor.Id }).FirstOrDefault();
            if(oeiv == null) throw new KeyNotFoundException();

            if (oeiv.VendedorResponsavel != parceiro)
                throw new ArgumentException("Não é permitido alterar um usuário de outro vendendor responsável");

            var oeivOutroEmail = _data.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro()
            {
                email = objOrcamentistaEIndicadorVendedor.Email
            });

            if (oeivOutroEmail.Any(x=>x.VendedorResponsavel != parceiro))
                throw new ArgumentException("Email já cadastrado");

            string senha_codificada = UtilsGlobais.Util.codificaDado(senha, false);

            if (string.IsNullOrEmpty(senha_codificada))
                throw new ArgumentException("Falha na codificação de senha.");

            objOrcamentistaEIndicadorVendedor.Datastamp = senha_codificada;

            if (oeiv.Datastamp!= senha_codificada)
            {

                oeiv.DataUltimaAlteracaoSenha = DateTime.Now;
            }
            oeiv.Datastamp = senha_codificada;

            oeiv.Nome = objOrcamentistaEIndicadorVendedor.Nome;
            oeiv.Celular = objOrcamentistaEIndicadorVendedor.Celular;
            oeiv.Telefone = objOrcamentistaEIndicadorVendedor.Telefone;
            oeiv.Email = objOrcamentistaEIndicadorVendedor.Email;
            oeiv.Ativo = objOrcamentistaEIndicadorVendedor.Ativo;
            oeiv.UsuarioUltimaAlteracao = parceiro;
            oeiv.DataUltimaAlteracao = DateTime.Now;

            return _data.Atualizar(oeiv);
        }
    }
}
