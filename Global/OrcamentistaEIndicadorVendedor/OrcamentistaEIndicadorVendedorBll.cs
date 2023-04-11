using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using OrcamentistaEindicador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static InfraBanco.Constantes.Constantes;

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

            if (!oei.Any())
                throw new KeyNotFoundException();

            if (_data.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro()
            {
                email = objOrcamentistaEIndicadorVendedor.Email
            }).Any())
            {
                throw new ArgumentException("Email já cadastrado");
            }

            var atualizacaoSenhaMensagem = SenhaValida(
                objOrcamentistaEIndicadorVendedor.Nome,
                senha);

            if (atualizacaoSenhaMensagem.Length > 0)
            {
                throw new ArgumentException(atualizacaoSenhaMensagem);
            }

            string senha_codificada = UtilsGlobais.Util.codificaDado(senha, false);

            if (string.IsNullOrEmpty(senha_codificada))
                throw new ArgumentException("Falha na codificação de senha.");

            objOrcamentistaEIndicadorVendedor.Datastamp = senha_codificada;

            objOrcamentistaEIndicadorVendedor.IdIndicador = oei.First().IdIndicador;
            objOrcamentistaEIndicadorVendedor.UsuarioCadastro = parceiro;
            objOrcamentistaEIndicadorVendedor.UsuarioUltimaAlteracao = parceiro;
            objOrcamentistaEIndicadorVendedor.DataCadastro = DateTime.Now;
            objOrcamentistaEIndicadorVendedor.DataUltimaAlteracao = DateTime.Now;
            objOrcamentistaEIndicadorVendedor.DataUltimaAlteracaoSenha = null;

            return _data.Inserir(objOrcamentistaEIndicadorVendedor);
        }

        public TorcamentistaEIndicadorVendedor Atualizar(TorcamentistaEIndicadorVendedor objOrcamentistaEIndicadorVendedor, 
            string senha, 
            string parceiro,
            string vendedor, 
            TipoUsuario tipoUsuario,
            bool SelecionarQualquerIndicadorDaLoja
            )
        {
            var oeiv = _data.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro() { id = objOrcamentistaEIndicadorVendedor.Id }).FirstOrDefault();
            if (oeiv == null) throw new KeyNotFoundException();


            //var oei = _dataIndicador.PorFiltro(new TorcamentistaEindicadorFiltro()
            //{
            //    apelido = parceiro
            //});

            // Parceiro
            if ((int)tipoUsuario ==2)
            {
                if (oeiv.VendedorResponsavel != vendedor)
                    throw new ArgumentException("Não é permitido alterar um usuário de outro vendedor responsável");
            }

            // Usuário Interno
            if ((int)tipoUsuario == 1 && !SelecionarQualquerIndicadorDaLoja)
            {
                if (oeiv.VendedorResponsavel != vendedor)
                    throw new ArgumentException("Não é permitido alterar um usuário de outro vendedor responsável");
            }

            var existeEmail = _data.PorFiltro(new TorcamentistaEIndicadorVendedorFiltro()
            {
                email = objOrcamentistaEIndicadorVendedor.Email
            });

            if (existeEmail.Any() && existeEmail.FirstOrDefault().Id != objOrcamentistaEIndicadorVendedor.Id)
                throw new ArgumentException("Email já cadastrado");

            var atualizacaoSenhaMensagem = SenhaValida(
                objOrcamentistaEIndicadorVendedor.Nome,
                senha);

            if (atualizacaoSenhaMensagem.Length > 0)
            {
                throw new ArgumentException(atualizacaoSenhaMensagem);
            }

            string senha_codificada = UtilsGlobais.Util.codificaDado(senha, false);

            if (string.IsNullOrEmpty(senha_codificada))
                throw new ArgumentException("Falha na codificação de senha.");

            objOrcamentistaEIndicadorVendedor.Datastamp = senha_codificada;

            if (oeiv.Datastamp != senha_codificada && oeiv.Datastamp != senha)
            {
                oeiv.DataUltimaAlteracaoSenha = null;
            }
           

            oeiv.Datastamp = senha_codificada;

            oeiv.Nome = objOrcamentistaEIndicadorVendedor.Nome;
            //oeiv.IdIndicador = oei.First().IdIndicador;
            oeiv.Celular = objOrcamentistaEIndicadorVendedor.Celular;
            oeiv.Telefone = objOrcamentistaEIndicadorVendedor.Telefone;
            oeiv.Email = objOrcamentistaEIndicadorVendedor.Email;
            oeiv.Ativo = objOrcamentistaEIndicadorVendedor.Ativo;
            oeiv.UsuarioUltimaAlteracao = parceiro;
            oeiv.DataUltimaAlteracao = DateTime.Now;

            return _data.Atualizar(oeiv);
        }

        public string SenhaValida(
            string usuario,
            string senha)
        {
            var regex = new Regex(@"^(?=.*[0-9])(?=.*[a-zA-Z])[a-zA-Z0-9]{8,}$");

            if (!regex.IsMatch(senha))
            {
                return "A senha deve conter pelo menos 8 caracteres entre letras e dígitos.";
            }

            if (senha == usuario.ToUpper().Trim())
            {
                return "A nova senha não pode ser igual ao identificador do usuário!";
            }

            return string.Empty;
        }

        public List<TorcamentistaEIndicadorVendedor> BuscarVendedorParceirosPorParceiros(TorcamentistaEIndicadorVendedorFiltro obj)
        {
            return _data.BuscarVendedorParceirosPorParceiros(obj);
        }
    }
}