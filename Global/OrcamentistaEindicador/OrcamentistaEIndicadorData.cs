using ClassesBase;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentistaEindicador
{
    public class OrcamentistaEIndicadorData : BaseData<TorcamentistaEindicador, TorcamentistaEindicadorFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentistaEIndicadorData(ContextoBdProvider contextoProvider)
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
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = (from parceiro in db.TorcamentistaEindicadors
                                 select parceiro);

                    if (!string.IsNullOrEmpty(obj.loja))
                    {
                        saida = saida.Where(x => x.Loja == obj.loja);
                    }

                    if (!string.IsNullOrEmpty(obj.apelido))
                    {
                        saida = saida.Where(x => x.Apelido == obj.apelido);
                    }
                    if (!string.IsNullOrEmpty(obj.datastamp))
                    {
                        saida = saida.Where(x => x.Datastamp == obj.datastamp);
                    }
                    if (!string.IsNullOrEmpty(obj.vendedorId))
                    {
                        saida = saida.Where(x => x.Vendedor == obj.vendedorId);
                        //saida = saida.Join(db.TorcamentistaEIndicadorVendedor, x => x.Apelido, y => y.IdIndicador, (x, y) => x).Where(x => x.Apelido == obj.vendedorId);
                    }

                    return saida.ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<TorcamentistaEindicador> ValidarParceiro(string apelido, string senha_digitada_datastamp, bool somenteValidar)
        {
            apelido = apelido.ToUpper().Trim();

            var db = contextoProvider.GetContextoLeitura();
            //Validar o dados no bd
            var dados = from c in db.TorcamentistaEindicadors
                        where c.Apelido == apelido
                        select c;
            //new
            //{
            //    c.Razao_Social_Nome,
            //    c.Senha,
            //    c.Datastamp,
            //    c.Dt_Ult_Alteracao_Senha,
            //    c.Hab_Acesso_Sistema,
            //    c.Status,
            //    c.Loja
            //};

            string retorno = null;
            var t = await dados.FirstOrDefaultAsync();

            //se o apelido nao existe
            if (t == null)
                retorno = await Task.FromResult(retorno);
            if (t.Datastamp == "")
                retorno = await Task.FromResult(Constantes.ERR_USUARIO_BLOQUEADO);
            if (t.Hab_Acesso_Sistema != 1)
                retorno = await Task.FromResult(Constantes.ERR_USUARIO_BLOQUEADO);
            if (t.Status != "A")
                retorno = await Task.FromResult(Constantes.ERR_USUARIO_BLOQUEADO);
            if (t.Loja == "")
                retorno = await Task.FromResult(Constantes.ERR_IDENTIFICACAO_LOJA);

            //if (!somenteValidar)
            //{
            //    //validar a senha
            //    var senha_digitada_decod = Util.decodificaDado(senha_digitada_datastamp, Constantes.FATOR_CRIPTO);

            //    //para garantir que sempre a as senhas são maiusculas iremos decodificar o datastamp 
            //    //e comparar os 2 convertido para maiusculas
            //    var senha_banco_datastamp_decod = Util.decodificaDado(t.Datastamp, Constantes.FATOR_CRIPTO);

            //    if (senha_digitada_decod.ToUpper().Trim() != senha_banco_datastamp_decod.ToUpper().Trim())
            //        return await Task.FromResult(retorno);//retorna null

            //    //Fazer Update no bd
            //    using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            //    {
            //        TorcamentistaEindicador orcamentista = await dbgravacao.TorcamentistaEindicadors
            //        .Where(c => c.Apelido == apelido).SingleAsync();
            //        orcamentista.Dt_Ult_Acesso = DateTime.Now;

            //        await dbgravacao.SaveChangesAsync();
            //        dbgravacao.transacao.Commit();
            //    }

            //    if (t.Dt_Ult_Alteracao_Senha == null)
            //    {
            //        //Senha expirada, precisa mandar alguma valor de senha expirada
            //        //coloquei o valor "4" para saber quando a senha esta expirada
            //        return await Task.FromResult("4");
            //    }
            //}

            return await Task.FromResult(t);//t.Razao_Social_Nome);
        }
    }
}
