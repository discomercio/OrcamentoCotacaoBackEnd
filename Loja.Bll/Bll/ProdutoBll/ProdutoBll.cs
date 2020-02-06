using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Loja.Data;
using System.Linq;
using Loja.Modelos;
using Loja.Bll.Dto.ProdutoDto;
using Microsoft.EntityFrameworkCore;
using Loja.Bll.RegrasCtrlEstoque;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;

namespace Loja.Bll.ProdutoBll
{
    public class ProdutoBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public ProdutoBll(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<ConsultaProdutosDto>> ConsultarListaProdutos(string fabricante)
        {
            //paraTeste
            string loja = "202";

            var db = contextoProvider.GetContextoLeitura();

            int n_fabricante;

            if (!int.TryParse(fabricante, out n_fabricante))
            {
                fabricante = (from c in db.Tfabricantes
                              where c.Nome == fabricante
                              select c.Fabricante).SingleOrDefault();
            }


            var lstProdutosTask = from c in db.TprodutoLojas
                                  join cc in db.Tprodutos
                                  on new { c.Fabricante, c.Produto } equals new { cc.Fabricante, cc.Produto }
                                  where (c.Vendavel == "S" || c.Vendavel == "X") &&
                                        c.Fabricante == fabricante &&
                                        c.Loja == loja
                                  orderby c.Fabricante, cc.Descricao.ToUpper()
                                  select new ConsultaProdutosDto
                                  {
                                      Fabricante = c.Fabricante,
                                      Produto = c.Produto,
                                      Descricao = cc.Descricao,
                                      Preco = c.Preco_Lista,
                                      Cor = c.Cor,
                                      Vendavel = c.Vendavel
                                  };


            List<ConsultaProdutosDto> list = lstProdutosTask.ToList();

            foreach (var i in lstProdutosTask)
            {
                list.Add(new ConsultaProdutosDto
                {

                    Fabricante = i.Fabricante,
                    Produto = i.Produto,
                    Descricao = i.Descricao,
                    Preco = i.Preco, //((decimal)i.Preco).ToString("0.00")
                    Cor = i.Cor,
                    Vendavel = i.Vendavel
                }); ; ;
            }

            return await Task.FromResult(lstProdutosTask);
        }

        public async Task<string> BuscarFabricante(string fabricante)
        {
            var db = contextoProvider.GetContextoLeitura();

            int n_fabricante;
            string nomeFabricante;

            if (!int.TryParse(fabricante, out n_fabricante))
            {
                nomeFabricante = (from c in db.Tfabricantes
                                  where c.Nome == fabricante
                                  select c.Nome).SingleOrDefault();
            }
            else
            {
                nomeFabricante = (from c in db.Tfabricantes
                                  where c.Fabricante == fabricante
                                  select c.Nome).SingleOrDefault();
            }

            return await Task.FromResult(nomeFabricante);
        }

        public async Task<ProdutoComboDto> ListaProdutosCombo(string loja, string id_cliente, PedidoDto pedidoDto)
        {
            ProdutoComboDto retorno = new ProdutoComboDto();
            //string loja = "202";
            //string id_cliente = "000000605954";

            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = (from c in db.Tclientes
                               where c.Id == id_cliente
                               select new
                               {
                                   tipo_cliente = c.Tipo,
                                   contribuite_icms_status = c.Contribuinte_Icms_Status,
                                   produtor_rural_status = c.Produtor_Rural_Status,
                                   uf = c.Uf
                               }).FirstOrDefaultAsync();

            var cliente = await clienteTask;

            //obtém  a sigla para regra
            string cliente_regra = Util.Util.MultiCdRegraDeterminaPessoa(cliente.tipo_cliente, cliente.contribuite_icms_status,
                cliente.produtor_rural_status);

            var lstProdutosCompostos = BuscarProdutosCompostos(loja);
            List<ProdutoDto> lstTodosProdutos = (await BuscarTodosProdutos(loja)).ToList();

            List<RegrasBll> lst_cliente_regra = new List<RegrasBll>();
            MontaListaRegras(lstTodosProdutos, lst_cliente_regra);
            List<string> lstErros = new List<string>();
            await Util.Util.ObterCtrlEstoqueProdutoRegra_Teste(lstErros, lst_cliente_regra, cliente.uf, cliente_regra, contextoProvider);

            //antes de verificar a disponibilidade de estoque é necessário

            //afazer: Verificar disponibilidade de estoque
            Util.Util.ObterDisponibilidadeEstoque(lst_cliente_regra, lstTodosProdutos, lstErros, contextoProvider);

            //retorna as qtdes disponiveis
            await Util.Util.VerificarEstoque(lst_cliente_regra, contextoProvider);
            await Util.Util.VerificarEstoqueComSubQuery(lst_cliente_regra, contextoProvider);

            //buscar o parametro produto 001020 indice 12
            Tparametro tparametro = await Util.Util.BuscarRegistroParametro(Constantes.Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal, contextoProvider);
            //atribui a qtde de estoque para o produto
            IncluirEstoqueProduto(lst_cliente_regra, lstTodosProdutos, tparametro);

            //afazer: Msg de alertas para os produtos
            await ExisteMensagensAlertaProdutos(lstTodosProdutos);

            if (lstErros.Count > 0)
            {
                //chama o metodo verificar regras
            }

            retorno.ProdutoCompostoDto = (await lstProdutosCompostos).ToList();
            retorno.ProdutoDto = lstTodosProdutos;
            return retorno;
        }

        //vamos montar a verificação para cada produto selecionado pelo usuário
        public async Task<IEnumerable<string>> VerificarRegrasDisponibilidadeEstoqueProdutosSelecionados(PedidoDto pedidoDto,
            string loja, string id_cliente, List<ProdutoDto> lstProdutos)
        {
            List<string> lstMgsEstoque = new List<string>();

            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = (from c in db.Tclientes
                               where c.Id == id_cliente
                               select new
                               {
                                   tipo_cliente = c.Tipo,
                                   contribuite_icms_status = c.Contribuinte_Icms_Status,
                                   produtor_rural_status = c.Produtor_Rural_Status,
                                   uf = c.Uf
                               }).FirstOrDefaultAsync();

            var cliente = await clienteTask;

            //obtém  a sigla para regra
            string cliente_regra = Util.Util.MultiCdRegraDeterminaPessoa(cliente.tipo_cliente, cliente.contribuite_icms_status,
                cliente.produtor_rural_status);

            List<RegrasBll> lst_cliente_regra = new List<RegrasBll>();
            MontaListaRegras(lstProdutos, lst_cliente_regra);

            List<string> lstErros = new List<string>();
            await Util.Util.ObterCtrlEstoqueProdutoRegra_Teste(lstErros, lst_cliente_regra, cliente.uf, cliente_regra, contextoProvider);

            VerificarRegrasAssociadasAosProdutos(lst_cliente_regra, lstErros, pedidoDto);

            //precisamos criar um metodo ou arrumar esse mesmo para poder receber a quantidade de produtos selecionados
            //pelo usuário, pois é feito uma verificação em cima da quantidade que foi solicitada 
            Util.Util.ObterDisponibilidadeEstoque(lst_cliente_regra, lstProdutos, lstErros, contextoProvider);

            await Util.Util.VerificarEstoque(lst_cliente_regra, contextoProvider);
            await Util.Util.VerificarEstoqueComSubQuery(lst_cliente_regra, contextoProvider);

            //atribui a qtde de estoque para o produto
            //IncluirEstoqueProduto(lst_cliente_regra, lstProdutos, tparametro);//aqui que esta confuso para passar o parametro

            await ExisteMensagensAlertaProdutos(lstProdutos);

            //estou aqui para terminar isso.
            return lstMgsEstoque;
        }

        //não estou usando 
        private async Task<IEnumerable<RegrasBll>> ObterCtrlEstoqueProdutoRegra(List<ProdutoDto> lstProdutos,
            PedidoDto prePedido, List<string> lstErros)
        {
            List<RegrasBll> lstRegrasCrtlEstoque = new List<RegrasBll>();

            var db = contextoProvider.GetContextoLeitura();

            //vamos verificar passando todos os produtos simples da lista de produto que irá para ser selecionado


            foreach (var item in lstProdutos)
            {
                var regraProdutoTask = from c in db.TprodutoXwmsRegraCds
                                       where c.Fabricante == item.Fabricante &&
                                             c.Produto == item.Produto
                                       select c;

                var regra = await regraProdutoTask.FirstOrDefaultAsync();

                if (regra == null)
                {
                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                        Util.Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
                        item.Produto + " não possui regra associada");
                }
                else
                {
                    if (regra.Id_wms_regra_cd == 0)
                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                            Util.Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
                            item.Produto + " não está associado a nenhuma regra");
                    else
                    {
                        var wmsRegraTask = from c in db.TwmsRegraCds
                                           where c.Id == regra.Id_wms_regra_cd
                                           select c;

                        var wmsRegra = await wmsRegraTask.FirstOrDefaultAsync();
                        if (wmsRegra == null)
                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                Util.Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                item.Produto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
                        else
                        {
                            RegrasBll itemRegra = new RegrasBll();
                            itemRegra.Fabricante = item.Fabricante;
                            itemRegra.Produto = item.Produto;

                            itemRegra.TwmsRegraCd = new t_WMS_REGRA_CD
                            {
                                Id = wmsRegra.Id,
                                Apelido = wmsRegra.Apelido,
                                Descricao = wmsRegra.Descricao,
                                St_inativo = wmsRegra.St_inativo
                            };

                            var wmsRegraCdXUfTask = from c in db.TwmsRegraCdXUfs
                                                    where c.Id_wms_regra_cd == itemRegra.TwmsRegraCd.Id &&
                                                          c.Uf == prePedido.DadosCliente.Uf
                                                    select c;
                            var wmsRegraCdXUf = await wmsRegraCdXUfTask.FirstOrDefaultAsync();

                            if (wmsRegraCdXUf == null)
                            {
                                itemRegra.St_Regra = false;
                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                    Util.Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                    item.Produto + " não está cadastrada para a UF '" + prePedido.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
                            }
                            else
                            {
                                itemRegra.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
                                {
                                    Id = wmsRegraCdXUf.Id,
                                    Id_wms_regra_cd = wmsRegraCdXUf.Id_wms_regra_cd,
                                    Uf = wmsRegraCdXUf.Uf,
                                    St_inativo = wmsRegraCdXUf.St_inativo
                                };

                                //buscar a sigla tipo pessoa
                                var tipo_pessoa = Util.Util.MultiCdRegraDeterminaPessoa(prePedido.DadosCliente.Tipo,
                                    prePedido.DadosCliente.Contribuinte_Icms_Status, prePedido.DadosCliente.ProdutorRural);

                                var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
                                                               where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
                                                                     c.Tipo_pessoa == tipo_pessoa
                                                               select c;

                                var wmsRegraCdXUfXPessoa = await wmsRegraCdXUfXPessoaTask.FirstOrDefaultAsync();

                                if (wmsRegraCdXUfXPessoa == null)
                                {
                                    itemRegra.St_Regra = false;
                                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                        Util.Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                        item.Produto + " não está cadastrada para a UF '" + prePedido.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
                                }
                                else
                                {
                                    itemRegra.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
                                    {
                                        Id = wmsRegraCdXUfXPessoa.Id,
                                        Id_wms_regra_cd_x_uf = wmsRegraCdXUfXPessoa.Id_wms_regra_cd_x_uf,
                                        Tipo_pessoa = wmsRegraCdXUfXPessoa.Tipo_pessoa,
                                        St_inativo = wmsRegraCdXUfXPessoa.St_inativo,
                                        Spe_id_nfe_emitente = wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                    };

                                    if (wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                                    {
                                        itemRegra.St_Regra = false;
                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                            Util.Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                            item.Produto + " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id=" + regra.Id_wms_regra_cd + ")");
                                    }
                                    else
                                    {
                                        var nfEmitenteTask = from c in db.TnfEmitentes
                                                             where c.Id == wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                                             select c;
                                        var nfEmitente = await nfEmitenteTask.FirstOrDefaultAsync();

                                        if (nfEmitente != null)
                                        {
                                            if (nfEmitente.St_Ativo != 1)
                                            {
                                                itemRegra.St_Regra = false;
                                                lstErros.Add("Falha na regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                                    Util.Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante +
                                                    ")" + item.Produto + " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
                                                    "(Id=" + regra.Id_wms_regra_cd + ")");
                                            }
                                        }
                                        var wmsRegraCdXUfXPessoaXcdTask = from c in db.TwmsRegraCdXUfXPessoaXCds
                                                                          where c.Id_wms_regra_cd_x_uf_x_pessoa == wmsRegraCdXUfXPessoa.Id
                                                                          orderby c.Ordem_prioridade
                                                                          select c;
                                        var wmsRegraCdXUfXPessoaXcd = await wmsRegraCdXUfXPessoaXcdTask.ToListAsync();

                                        if (wmsRegraCdXUfXPessoaXcd.Count == 0)
                                        {
                                            itemRegra.St_Regra = false;
                                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                                Util.Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                                item.Produto + " não especifica nenhum CD para consumo do estoque (Id=" + regra.Id_wms_regra_cd + ")");
                                        }
                                        else
                                        {
                                            itemRegra.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();
                                            foreach (var i in wmsRegraCdXUfXPessoaXcd)
                                            {
                                                t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
                                                {
                                                    Id = i.Id,
                                                    Id_wms_regra_cd_x_uf_x_pessoa = i.Id_wms_regra_cd_x_uf_x_pessoa,
                                                    Id_nfe_emitente = i.Id_nfe_emitente,
                                                    Ordem_prioridade = i.Ordem_prioridade,
                                                    St_inativo = i.St_inativo
                                                };

                                                var nfeCadastroPrincipalTask = from c in db.TnfEmitentes
                                                                               where c.Id == item_cd_uf_pess_cd.Id_nfe_emitente
                                                                               select c;

                                                var nfeCadastroPrincipal = await nfeCadastroPrincipalTask.FirstOrDefaultAsync();

                                                if (nfeCadastroPrincipal != null)
                                                {
                                                    if (nfeCadastroPrincipal.St_Ativo != 1)
                                                        item_cd_uf_pess_cd.St_inativo = 1;
                                                }

                                                itemRegra.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
                                            }
                                            foreach (var i in itemRegra.TwmsCdXUfXPessoaXCd)
                                            {

                                                if (i.Id_nfe_emitente == itemRegra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                                {
                                                    if (i.St_inativo == 1)
                                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                                            Util.Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" +
                                                            item.Fabricante + ")" + item.Produto + " especifica o CD '" + Util.Util.ObterApelidoEmpresaNfeEmitentes(wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente, contextoProvider) +
                                                            "' para alocação de produtos sem presença no estoque, sendo que este CD está desativado para " +
                                                            "processar produtos disponíveis (Id=" + regra.Id_wms_regra_cd + ")");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            lstRegrasCrtlEstoque.Add(itemRegra);
                        }
                    }
                }

            }

            return lstRegrasCrtlEstoque;
        }

        public void VerificarRegrasAssociadasAosProdutos(List<RegrasBll> lst, List<string> lstErros, PedidoDto dtoPedido)
        {
            int qtdeCdAtivo = 0;
            int id_nfe_emitente_selecao_manual = 0;//esse é a seleção do checkebox 

            foreach (var i in lst)
            {
                if (i.Produto == 0.ToString())
                {
                    lstErros.Add("Produto (" + i.Fabricante + ")" + i.Produto +
                        " não possui regra de consumo do estoque associada");
                }
                else if (i.St_Regra == false)
                {
                    lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                        "' associada ao produto (" + i.Fabricante + ")" + i.Produto + " está desativada");
                }
                else if (i.TwmsRegraCdXUf.St_inativo == 1)
                {
                    lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                        "' associada ao produto (" + i.Fabricante + ")" + i.Produto + " está bloqueada para a UF '" +
                        dtoPedido.DadosCliente.Uf + "'");
                }
                else if (i.TwmsRegraCdXUfXPessoa.St_inativo == 1)
                {
                    lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido + "' associada ao produto (" +
                        i.Fabricante + ")" + i.Produto + " está bloqueada para clientes '" +
                        dtoPedido.DadosCliente.Tipo + "' da UF '" + dtoPedido.DadosCliente.Uf + "'");
                }
                else if (i.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                {
                    lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                        "' associada ao produto (" + i.Fabricante + ")" + i.Produto +
                        " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" +
                        dtoPedido.DadosCliente.Tipo + "' da UF '" + dtoPedido.DadosCliente.Uf + "'");
                }
                else
                {
                    foreach (var t in i.TwmsCdXUfXPessoaXCd)
                    {
                        if (t.Id_nfe_emitente > 0)
                        {
                            if (t.St_inativo == 0)
                            {
                                qtdeCdAtivo = qtdeCdAtivo + 1;
                            }
                        }
                    }

                    if (qtdeCdAtivo == 0 && id_nfe_emitente_selecao_manual == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto +
                            " não especifica nenhum CD ativo para clientes '" + dtoPedido.DadosCliente.Tipo +
                            "' da UF '" + dtoPedido.DadosCliente.Uf + "'");


                    }
                }
            }
        }

        //Caso seleção do CD Manual
        public void VerificarCDHabilitadoTodasRegras(List<RegrasBll> lstRegras, int selecao_manual, List<string> lstErros)
        {
            selecao_manual = 0;//esse é a seleção do checkebox 
            bool desativado = false;
            bool achou = false;
            List<string> lstErrosAux = new List<string>();

            foreach (var i in lstRegras)
            {
                achou = false;
                desativado = false;
                if (i.Produto != "")
                {
                    foreach (var t in i.TwmsCdXUfXPessoaXCd)
                    {
                        if (t.Id_nfe_emitente == selecao_manual)
                        {
                            achou = true;
                            if (t.St_inativo == 1)
                            {
                                desativado = true;
                            }
                        }
                    }
                }
                if (!achou)
                {
                    lstErrosAux.Add("Produto (" + i.Fabricante + ")" + i.Produto + ": regra '"
                        + i.TwmsRegraCd.Apelido + "' (Id=" + i.TwmsRegraCd.Id + ") não permite o CD '" +
                        Util.Util.ObterApelidoEmpresaNfeEmitentes(selecao_manual, contextoProvider));
                }
                else if (desativado)
                {
                    lstErrosAux.Add("Regra '" + i.TwmsRegraCd.Apelido + "'(Id = " + i.TwmsRegraCd.Id + ") define o CD '" +
                        Util.Util.ObterApelidoEmpresaNfeEmitentes(selecao_manual, contextoProvider) + "' como 'desativado'");
                }

            }

            if(lstErros.Count > 0)
            {
                string erro = "O CD selecionado manualmente não pode ser usado devido aos seguintes motivos:";
                foreach(var e in lstErrosAux)
                {
                    erro = erro + e;
                }

                lstErros.Add(erro);
            }
        }

        public async Task<IEnumerable<ProdutoCompostoDto>> BuscarProdutosCompostos(string loja)
        {

            //loja = "202";
            var db = contextoProvider.GetContextoLeitura();

            //var produtosCompostosTask = from d in (from c in db.Tprodutos
            //                                       join pc in db.TecProdutoCompostos on c.Produto equals pc.Produto_Composto
            //                                       join pci in db.TecProdutoCompostoItems on pc.Fabricante_Composto equals pci.Fabricante_composto
            //                                       join pl in db.TprodutoLojas on pci.Produto_item equals pl.Produto
            //                                       join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
            //                                       where pl.Loja == loja &&
            //                                             pl.Vendavel == "S" &&
            //                                             c.Fabricante == pc.Fabricante_Composto &&
            //                                             pc.Produto_Composto == pci.Produto_composto
            //                                       select new
            //                                       {

            //                                           fabricante_pai = c.Fabricante,
            //                                           fabricante_pai_nome = fab.Nome,
            //                                           produto_pai = c.Produto,
            //                                           valor = (decimal)pl.Preco_Lista,
            //                                           qtde = (int)pci.Qtde,
            //                                           produtosFilhos = new ProdutoFilhoDto
            //                                           {
            //                                               Fabricante = c.Fabricante,
            //                                               Fabricante_Nome = fab.Nome,
            //                                               Produto = pci.Produto_item,
            //                                               Qtde = pci.Qtde
            //                                           }
            //                                       })
            //                            group d by d.produto_pai into g
            //                            select new ProdutoCompostoDto
            //                            {
            //                                PaiFabricante = g.Select(r => r.fabricante_pai).FirstOrDefault(),
            //                                PaiFabricanteNome = g.Select(r => r.fabricante_pai_nome).FirstOrDefault(),
            //                                PaiProduto = g.Select(r => r.produto_pai).FirstOrDefault(),
            //                                Preco_total_Itens = g.Sum(r => r.qtde * r.valor),
            //                                Filhos = g.Select(r => r.produtosFilhos).ToList()
            //                            };
            var produtosCompostosTask = from c in db.Tprodutos
                                        join pc in db.TecProdutoCompostos on c.Produto equals pc.Produto_Composto
                                        join pci in db.TecProdutoCompostoItems on pc.Fabricante_Composto equals pci.Fabricante_composto
                                        join pl in db.TprodutoLojas on pci.Produto_item equals pl.Produto
                                        join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
                                        where pl.Loja == loja &&
                                              pl.Vendavel == "S" &&
                                              c.Fabricante == pc.Fabricante_Composto &&
                                              pc.Produto_Composto == pci.Produto_composto
                                        select new
                                        {

                                            fabricante_pai = c.Fabricante,
                                            fabricante_pai_nome = fab.Nome,
                                            produto_pai = c.Produto,
                                            valor = (decimal)pl.Preco_Lista,
                                            qtde = (int)pci.Qtde,
                                            produtosFilhos = new ProdutoFilhoDto
                                            {
                                                Fabricante = c.Fabricante,
                                                Fabricante_Nome = fab.Nome,
                                                Produto = pci.Produto_item,
                                                Qtde = pci.Qtde
                                            }
                                        };

            var teste = await produtosCompostosTask.ToListAsync();

            var testeTask = from c in teste
                            group c by c.produto_pai into g
                            select new ProdutoCompostoDto
                            {
                                PaiFabricante = g.Select(r => r.fabricante_pai).FirstOrDefault(),
                                PaiFabricanteNome = g.Select(r => r.fabricante_pai_nome).FirstOrDefault(),
                                PaiProduto = g.Select(r => r.produto_pai).FirstOrDefault(),
                                Preco_total_Itens = g.Sum(r => r.qtde * r.valor),
                                Filhos = g.Select(r => r.produtosFilhos).ToList()
                            };

            List<ProdutoCompostoDto> produto = testeTask.ToList();

            return produto;
        }

        public async Task<IEnumerable<ProdutoDto>> BuscarTodosProdutos(string loja)
        {
            //loja = "202";

            var db = contextoProvider.GetContextoLeitura();

            var todosProdutosTask = from c in db.Tprodutos
                                    join pl in db.TprodutoLojas on c.Produto equals pl.Produto
                                    join fab in db.Tfabricantes on c.Fabricante equals fab.Fabricante
                                    where pl.Vendavel == "S" &&
                                          pl.Loja == loja
                                    select new ProdutoDto
                                    {
                                        Fabricante = c.Fabricante,
                                        Fabricante_Nome = fab.Nome,
                                        Produto = pl.Produto,
                                        Descricao_html = c.Descricao_Html,
                                        Preco_lista = pl.Preco_Lista,
                                        Qtde_Max_Venda = pl.Qtde_Max_Venda
                                    };

            List<ProdutoDto> lstTodosProdutos = await todosProdutosTask.ToListAsync();

            return lstTodosProdutos;
        }

        public void MontaListaRegras(List<ProdutoDto> lst_produtos, List<RegrasBll> lst_cliente_regra)
        {
            foreach (var p in lst_produtos)
            {
                lst_cliente_regra.Add(new RegrasBll
                {
                    Fabricante = p.Fabricante,
                    Produto = p.Produto
                });
            }
        }

        private void IncluirEstoqueProduto(List<RegrasBll> lstRegras, List<ProdutoDto> lst_produtos, Tparametro parametro)
        {
            foreach (var p in lst_produtos)
            {
                if (!string.IsNullOrEmpty(p.Produto))
                {
                    int qtde_estoque_total_disponivel = 0;
                    int qtde_estoque_total_disponivel_global = 0;

                    foreach (var regra in lstRegras)
                    {

                        if (regra.TwmsRegraCd != null)
                        {
                            foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (r.Id_nfe_emitente > 0)
                                {
                                    if (r.St_inativo == 0)
                                    {
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.Produto)
                                        {
                                            qtde_estoque_total_disponivel += r.Estoque_Qtde;
                                            if (qtde_estoque_total_disponivel_global == 0)
                                            {
                                                if (r.Estoque_Qtde_Estoque_Global != null)
                                                    qtde_estoque_total_disponivel_global = (int)r.Estoque_Qtde_Estoque_Global;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (parametro.Campo_inteiro == 1)
                    {
                        if (qtde_estoque_total_disponivel_global == 0)
                        {
                            //p.Qtde_estoque_total_disponivel = 0;
                            p.Estoque = 0;
                        }
                        else
                        {
                            p.Estoque = qtde_estoque_total_disponivel_global;
                        }
                    }
                    else
                    {
                        //p.Qtde_estoque_total_disponivel = (short?)qtde_estoque_total_disponivel;
                        p.Estoque = qtde_estoque_total_disponivel;
                    }
                }
            }

        }
        private async Task ExisteMensagensAlertaProdutos(List<ProdutoDto> lst_produtos)
        {
            var db = contextoProvider.GetContextoLeitura();


            var alertasTask = from c in db.TprodutoXAlertas.Include(r => r.TalertaProduto).Include(r => r.Tproduto)
                              where c.TalertaProduto.Ativo == "S"
                              orderby c.Dt_Cadastro, c.Id_Alerta
                              select new
                              {
                                  alerta_fabricante = c.Fabricante,
                                  alerta_produto = c.Produto,
                                  alerta_mensagem = c.TalertaProduto.Mensagem,
                                  alerta_descricao = c.Tproduto.Descricao
                              };

            var alertas = await alertasTask.ToListAsync();

            foreach (var p in lst_produtos)
            {
                foreach (var m in alertas)
                {
                    if (string.IsNullOrEmpty(m.alerta_fabricante))
                    {
                        if (m.alerta_fabricante == p.Fabricante && m.alerta_produto == p.Produto)
                        {
                            p.Alertas = m.alerta_mensagem;
                        }
                    }
                }
            }
        }

        public async Task<IEnumerable<string>> BuscarOrcamentistaEIndicadorParaProdutos(string usuarioSistema,
            string lstOperacoesPermitidas, string loja)
        {
            List<string> lst = (await Util.Util.BuscarOrcamentistaEIndicadorParaProdutos(
                contextoProvider, usuarioSistema, lstOperacoesPermitidas, loja)).ToList();

            return lst;
        }

        public async Task<IEnumerable<string[]>> WmsApelidoEmpresaNfeEmitenteMontaItensSelect(string idDefault)
        {
            List<string[]> lstRetorno = new List<string[]>();

            var db = contextoProvider.GetContextoLeitura();

            var lstTask = from c in db.TnfEmitentes
                          where c.St_Ativo != 0 &&
                                c.St_Habilitado_Ctrl_Estoque != 0
                          orderby c.Id
                          select new
                          {
                              id = c.Id.ToString(),
                              apelido = c.Apelido
                          };

            //geralmente passamos o idDefault com null
            if (!string.IsNullOrEmpty(idDefault))
            {
                lstTask = from c in db.TnfEmitentes
                          where c.St_Ativo != 0 &&
                                c.St_Habilitado_Ctrl_Estoque != 0 ||
                                c.Id == short.Parse(idDefault)
                          orderby c.Id
                          select new
                          {
                              id = c.Id.ToString(),
                              apelido = c.Apelido
                          };
            }

            var lst = await lstTask.ToListAsync();

            foreach (var i in lst)
            {
                lstRetorno.Add(new string[] { i.id, i.apelido });

            }

            return lstRetorno;
        }
    }
}
