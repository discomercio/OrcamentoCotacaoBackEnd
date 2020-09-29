using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Loja.Bll.Dto.ProdutoDto;
using Microsoft.EntityFrameworkCore;
using Loja.Bll.RegrasCtrlEstoque;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;
using Loja.Bll.Dto.PedidoDto;
using InfraBanco.Modelos;

namespace Loja.Bll.ProdutoBll
{
    public class ProdutoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public ProdutoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<ConsultaProdutosDto>> ConsultarListaProdutos(string fabricante, string loja)
        {
            //paraTeste
            //string loja = "202";

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
                                      //Cor = c.Cor,
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

            return await Task.FromResult(lstProdutosTask.OrderBy(r=>r.Produto));
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
        //Novo metodo para carregar todo os produtos para tela
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

            var lstProdutosCompostos = BuscarProdutosCompostos(loja);
            List<ProdutoDto> lstTodosProdutos = (await BuscarTodosProdutos(loja)).ToList();

            retorno.ProdutoCompostoDto = (await lstProdutosCompostos).ToList();
            retorno.ProdutoDto = lstTodosProdutos;

            return retorno;
        }

        public async Task<IEnumerable<RegrasBll>> VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_Teste(PedidoProdutosDtoPedido produto,
            string cpf_cnpj, int id_nfe_emitente_selecao_manual)
        {
            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = from c in db.Tclientes
                              where c.Cnpj_Cpf == cpf_cnpj
                              select c;

            Tcliente cliente = await clienteTask.FirstOrDefaultAsync();

            ProdutoValidadoComEstoqueDto prodValidadoEstoque = new ProdutoValidadoComEstoqueDto();
            prodValidadoEstoque.ListaErros = new List<string>();

            //obtém  a sigla para regra
            string cliente_regra = Util.Util.MultiCdRegraDeterminaPessoa(cliente.Tipo, cliente.Contribuinte_Icms_Status,
                cliente.Produtor_Rural_Status);

            //buscar o produto
            //PedidoProdutosDtoPedido produto = new PedidoProdutosDtoPedido();

            List<RegrasBll> regraCrtlEstoque = (await ObterCtrlEstoqueProdutoRegraParaUMProduto(produto, cliente,
                prodValidadoEstoque.ListaErros, contextoProvider)).ToList();

            await Util.Util.ObterCtrlEstoqueProdutoRegra_Teste(prodValidadoEstoque.ListaErros, regraCrtlEstoque, cliente.Uf, cliente_regra, contextoProvider.GetContextoLeitura());

            VerificarRegrasAssociadasParaUMProduto(regraCrtlEstoque, prodValidadoEstoque.ListaErros, cliente, id_nfe_emitente_selecao_manual);

            if (id_nfe_emitente_selecao_manual != 0)
                await VerificarCDHabilitadoTodasRegras(regraCrtlEstoque, id_nfe_emitente_selecao_manual, prodValidadoEstoque.ListaErros, contextoProvider);

            await ObterDisponibilidadeEstoque(regraCrtlEstoque, produto, prodValidadoEstoque.ListaErros, id_nfe_emitente_selecao_manual, contextoProvider);

            //meto responsavel por atribuir a qtde de estoque ao produto
            //await Util.Util.VerificarEstoque(regraCrtlEstoque, produto, id_nfe_emitente_selecao_manual, contextoProvider);

            bool estoqueInsuficiente = VerificarEstoqueInsuficienteUMProduto(regraCrtlEstoque, produto,
                id_nfe_emitente_selecao_manual, prodValidadoEstoque.ListaErros);

            regraCrtlEstoque = VerificarQtdePedidosAutoSplit(regraCrtlEstoque, prodValidadoEstoque.ListaErros, produto, id_nfe_emitente_selecao_manual);

            List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, id_nfe_emitente_selecao_manual);

            await ExisteProdutoDescontinuado(produto, prodValidadoEstoque.ListaErros);


            return regraCrtlEstoque;
        }

        public async Task<ProdutoValidadoComEstoqueDto> VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado(PedidoProdutosDtoPedido produto,
            string cpf_cnpj, int id_nfe_emitente_selecao_manual)
        {
            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = from c in db.Tclientes
                              where c.Cnpj_Cpf == cpf_cnpj
                              select c;

            Tcliente cliente = await clienteTask.FirstOrDefaultAsync();

            ProdutoValidadoComEstoqueDto prodValidadoEstoque = new ProdutoValidadoComEstoqueDto();
            prodValidadoEstoque.ListaErros = new List<string>();

            //obtém  a sigla para regra
            string cliente_regra = Util.Util.MultiCdRegraDeterminaPessoa(cliente.Tipo, cliente.Contribuinte_Icms_Status,
                cliente.Produtor_Rural_Status);

            //buscar o produto
            //PedidoProdutosDtoPedido produto = new PedidoProdutosDtoPedido();

            List<RegrasBll> regraCrtlEstoque = (await ObterCtrlEstoqueProdutoRegraParaUMProduto(produto, cliente, 
                prodValidadoEstoque.ListaErros, contextoProvider)).ToList();

            await Util.Util.ObterCtrlEstoqueProdutoRegra_Teste(prodValidadoEstoque.ListaErros, regraCrtlEstoque, cliente.Uf, cliente_regra, contextoProvider.GetContextoLeitura());

            VerificarRegrasAssociadasParaUMProduto(regraCrtlEstoque, prodValidadoEstoque.ListaErros, cliente, id_nfe_emitente_selecao_manual);

            if (id_nfe_emitente_selecao_manual != 0)
                await VerificarCDHabilitadoTodasRegras(regraCrtlEstoque, id_nfe_emitente_selecao_manual, prodValidadoEstoque.ListaErros, contextoProvider);

            await ObterDisponibilidadeEstoque(regraCrtlEstoque, produto, prodValidadoEstoque.ListaErros, id_nfe_emitente_selecao_manual, contextoProvider);

            //meto responsavel por atribuir a qtde de estoque ao produto
            //await Util.Util.VerificarEstoque(regraCrtlEstoque, produto, id_nfe_emitente_selecao_manual, contextoProvider);
            
            bool estoqueInsuficiente = VerificarEstoqueInsuficienteUMProduto(regraCrtlEstoque, produto,
                id_nfe_emitente_selecao_manual, prodValidadoEstoque.ListaErros);

            VerificarQtdePedidosAutoSplit(regraCrtlEstoque, prodValidadoEstoque.ListaErros, produto, id_nfe_emitente_selecao_manual);

            List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, id_nfe_emitente_selecao_manual);

            await ExisteProdutoDescontinuado(produto, prodValidadoEstoque.ListaErros);

            prodValidadoEstoque.Produto = new ProdutoDto
            {
                Produto = produto.NumProduto,
                Fabricante = produto.Fabricante,
                Estoque = (int)produto.Qtde_estoque_total_disponivel,
                QtdeSolicitada = produto.Qtde,
                Preco_lista = produto.VlLista,
                Descricao_html = produto.Descricao,
                Lst_empresa_selecionada = lst_empresa_selecionada
            };

            return prodValidadoEstoque;
        }

        //parateste
        public static async Task<IEnumerable<RegrasBll>> ObterCtrlEstoqueProdutoRegraParaUMProduto(PedidoProdutosDtoPedido produto,
            Tcliente tcliente, List<string> lstErros, InfraBanco.ContextoBdProvider contextoBd)
        {
            List<RegrasBll> lstRegrasCrtlEstoque = new List<RegrasBll>();

            var db = contextoBd.GetContextoLeitura();

            //vamos verificar passando todos os produtos simples da lista de produto que irá para ser selecionado

            var regraProdutoTask = from c in db.TprodutoXwmsRegraCds
                                   where c.Fabricante == produto.Fabricante &&
                                         c.Produto == produto.NumProduto
                                   select c;

            var regra = await regraProdutoTask.FirstOrDefaultAsync();

            if (regra == null)
            {
                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                    Util.Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': produto (" + produto.Fabricante + ")" +
                    produto.NumProduto + " não possui regra associada");
            }
            else
            {
                if (regra.Id_wms_regra_cd == 0)
                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                        Util.Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': produto (" + produto.Fabricante + ")" +
                        produto.NumProduto + " não está associado a nenhuma regra");
                else
                {
                    var wmsRegraTask = from c in db.TwmsRegraCds
                                       where c.Id == regra.Id_wms_regra_cd
                                       select c;

                    var wmsRegra = await wmsRegraTask.FirstOrDefaultAsync();
                    if (wmsRegra == null)
                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                            Util.Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': regra associada ao produto (" +
                            produto.Fabricante + ")" +
                            produto.NumProduto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
                    else
                    {
                        RegrasBll itemRegra = new RegrasBll();
                        itemRegra.Fabricante = produto.Fabricante;
                        itemRegra.Produto = produto.NumProduto;

                        itemRegra.TwmsRegraCd = new t_WMS_REGRA_CD
                        {
                            Id = wmsRegra.Id,
                            Apelido = wmsRegra.Apelido,
                            Descricao = wmsRegra.Descricao,
                            St_inativo = wmsRegra.St_inativo
                        };

                        var wmsRegraCdXUfTask = from c in db.TwmsRegraCdXUfs
                                                where c.Id_wms_regra_cd == itemRegra.TwmsRegraCd.Id &&
                                                      c.Uf == tcliente.Uf
                                                select c;
                        var wmsRegraCdXUf = await wmsRegraCdXUfTask.FirstOrDefaultAsync();

                        if (wmsRegraCdXUf == null)
                        {
                            itemRegra.St_Regra = false;
                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                                Util.Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': regra associada ao produto (" +
                                produto.Fabricante + ")" +
                                produto.NumProduto + " não está cadastrada para a UF '" + tcliente.Uf + "' (Id=" +
                                regra.Id_wms_regra_cd + ")");
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
                            var tipo_pessoa = Util.Util.MultiCdRegraDeterminaPessoa(tcliente.Tipo,
                                tcliente.Contribuinte_Icms_Status, tcliente.Produtor_Rural_Status);

                            var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
                                                           where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
                                                                 c.Tipo_pessoa == tipo_pessoa
                                                           select c;

                            var wmsRegraCdXUfXPessoa = await wmsRegraCdXUfXPessoaTask.FirstOrDefaultAsync();

                            if (wmsRegraCdXUfXPessoa == null)
                            {
                                itemRegra.St_Regra = false;
                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                    tcliente.Uf + "' e '" +
                                    Util.Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                    "': regra associada ao produto (" + produto.Fabricante + ")" +
                                    produto.NumProduto + " não está cadastrada para a UF '" + tcliente.Uf +
                                    "' (Id=" + regra.Id_wms_regra_cd + ")");
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
                                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                        tcliente.Uf + "' e '" +
                                        Util.Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                        "': regra associada ao produto (" + produto.Fabricante + ")" +
                                        produto.NumProduto +
                                        " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id="
                                        + regra.Id_wms_regra_cd + ")");
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
                                            lstErros.Add("Falha na regra de consumo do estoque para a UF '" +
                                                tcliente.Uf + "' e '" +
                                                Util.Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                                "': regra associada ao produto (" + produto.Fabricante +
                                                ")" + produto.NumProduto +
                                                " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
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
                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                            tcliente.Uf + "' e '" +
                                            Util.Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                            "': regra associada ao produto (" + produto.Fabricante + ")" +
                                            produto.NumProduto +
                                            " não especifica nenhum CD para consumo do estoque (Id=" +
                                            regra.Id_wms_regra_cd + ")");
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
                                                    lstErros.Add(
                                                        "Falha na leitura da regra de consumo do estoque para a UF '" +
                                                        tcliente.Uf + "' e '" +
                                                        Util.Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                                        "': regra associada ao produto (" +
                                                        produto.Fabricante + ")" + produto.NumProduto +
                                                        " especifica o CD '" + Util.Util.ObterApelidoEmpresaNfeEmitentes(wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente, 
                                                        contextoBd) +
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



            return lstRegrasCrtlEstoque;
        }

        public static void VerificarRegrasAssociadasParaUMProduto(List<RegrasBll> lst, List<string> lstErros,
            Tcliente cliente, int id_nfe_emitente_selecao_manual)
        {

            /*id_nfe_emitente_selecao_manual = 0;*///esse é a seleção do checkebox 

            foreach (var i in lst)
            {
                if (!string.IsNullOrEmpty(i.Produto))
                {
                    if (i.TwmsRegraCd.Id == 0)
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
                            cliente.Uf + "'");
                    }
                    else if (i.TwmsRegraCdXUfXPessoa.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            i.Fabricante + ")" + i.Produto + " está bloqueada para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    else if (i.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto +
                            " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    else
                    {
                        int qtdeCdAtivo = 0;
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
                                " não especifica nenhum CD ativo para clientes '" + cliente.Tipo +
                                "' da UF '" + cliente.Uf + "'");


                        }
                    }
                }
            }
        }

        public static async Task ObterDisponibilidadeEstoque(List<RegrasBll> lstRegrasCrtlEstoque, PedidoProdutosDtoPedido produto,
            List<string> lstErros, int id_nfe_emitente_selecao_manual, InfraBanco.ContextoBdProvider contextoBd)
        {
            //int id_nfe_emitente_selecao_manual = 0;

            foreach (var r in lstRegrasCrtlEstoque)
            {
                if (!string.IsNullOrEmpty(r.Produto))
                {
                    if (r.TwmsRegraCd != null)
                    {
                        foreach (var p in r.TwmsCdXUfXPessoaXCd)
                        {
                            if (p.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || p.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (p.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    if (r.Fabricante == produto.Fabricante && r.Produto == produto.NumProduto)
                                    {
                                        p.Estoque_Fabricante = produto.Fabricante;
                                        p.Estoque_Produto = produto.NumProduto;
                                        p.Estoque_DescricaoHtml = produto.Descricao;
                                        p.Estoque_Qtde_Solicitado = produto.Qtde;//essa variavel não deve ser utilizada, a qtde só sera solicitada 
                                        //quando o usuario inserir a qtde 
                                        p.Estoque_Qtde = 0;
                                        if (!await Util.Util.EstoqueVerificaDisponibilidadeIntegralV2(p,
                                            produto, contextoBd))
                                        {
                                            lstErros.Add("Falha ao tentar consultar disponibilidade no estoque do produto (" +
                                                r.Fabricante + ")" + r.Produto);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool VerificarEstoqueInsuficienteUMProduto(List<RegrasBll> lstRegras, PedidoProdutosDtoPedido produto,
            int id_nfe_emitente_selecao_manual, List<string> lstErros)
        {
            bool retorno = false;
            int qtde_estoque_total_disponivel = 0;

            if (!string.IsNullOrEmpty(produto.NumProduto))
            {
                foreach (var regra in lstRegras)
                {
                    if (!string.IsNullOrEmpty(regra.Produto))
                    {
                        foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                        {
                            if (r.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || r.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (r.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    if (regra.Fabricante == produto.Fabricante && regra.Produto == produto.NumProduto)
                                    {
                                        qtde_estoque_total_disponivel += (int)r.Estoque_Qtde;
                                    }
                                }
                            }
                        }
                    }
                }


                if (qtde_estoque_total_disponivel == 0)
                {
                    produto.Qtde_estoque_total_disponivel = 0;
                    lstErros.Add("PRODUTO SEM PRESENÇA NO ESTOQUE");
                    retorno = true;
                }
                else
                {
                    produto.Qtde_estoque_total_disponivel = (short?)qtde_estoque_total_disponivel;
                }
                //bloco comentado, pois iremos fazer a verificação no cliente
                //if (produto.Qtde > produto.Qtde_estoque_total_disponivel)
                //{
                    
                //    lstErros.Add("Quantidade solicitada é maior que estoque disponível!");
                //}
            }

            return retorno;

        }

        public List<RegrasBll> VerificarQtdePedidosAutoSplit(List<RegrasBll> lstRegras, List<string> lstErros,
            PedidoProdutosDtoPedido produto, int id_nfe_emitente_selecao_manual)
        {
            int qtde_a_alocar = 0;

            List<RegrasBll> lstRegras_apoio = lstRegras;
            lstRegras = new List<RegrasBll>();

            if (!string.IsNullOrEmpty(produto.NumProduto))
            {
                qtde_a_alocar = (int)produto.Qtde;

                foreach (var regra in lstRegras_apoio)
                {
                    if (qtde_a_alocar == 0)
                        break;
                    if (!string.IsNullOrEmpty(regra.Produto))
                    {
                        foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                        {
                            if (qtde_a_alocar == 0)
                                break;
                            if (re.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || re.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (re.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    if (regra.Fabricante == produto.Fabricante && regra.Produto == produto.NumProduto)
                                    {
                                        if (re.Estoque_Qtde >= qtde_a_alocar)
                                        {
                                            re.Estoque_Qtde_Solicitado = (short)qtde_a_alocar;
                                            qtde_a_alocar = 0;
                                        }
                                        else if (re.Estoque_Qtde > 0)
                                        {
                                            re.Estoque_Qtde_Solicitado = re.Estoque_Qtde;
                                            qtde_a_alocar = qtde_a_alocar - re.Estoque_Qtde;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                if (qtde_a_alocar > 0)
                {
                    foreach (var regra in lstRegras_apoio)
                    {
                        if (qtde_a_alocar == 0)
                            break;
                        if (!string.IsNullOrEmpty(regra.Produto))
                        {
                            foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (qtde_a_alocar == 0)
                                    break;
                                if (id_nfe_emitente_selecao_manual == 0)
                                {
                                    //seleção automática
                                    if (regra.Fabricante == produto.Fabricante && regra.Produto == produto.NumProduto &&
                                        re.Id_nfe_emitente > 0 &&
                                        re.Id_nfe_emitente == regra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                    {
                                        re.Estoque_Qtde_Solicitado += (short)qtde_a_alocar;
                                        qtde_a_alocar = 0;
                                    }
                                }
                                else
                                {
                                    //seleção manual
                                    if (regra.Fabricante == produto.Fabricante &&
                                       regra.Produto == produto.NumProduto &&
                                       re.Id_nfe_emitente > 0 &&
                                       re.Id_nfe_emitente == regra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                    {
                                        re.Estoque_Qtde_Solicitado += (short)qtde_a_alocar;
                                        qtde_a_alocar = 0;//verificar esse valor
                                    }
                                }
                            }
                        }

                        lstRegras.Add(regra);
                    }
                }
                if (qtde_a_alocar > 0)
                {
                    lstErros.Add("Falha ao processar a alocação de produtos no estoque: restaram " +
                        qtde_a_alocar + " unidades do produto (" +
                        produto.Fabricante + ")" + produto.NumProduto +
                        " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD");
                }
            }

            return lstRegras;
        }

        private List<int> ContagemEmpresasUsadasAutoSplit(List<RegrasBll> lstRegras, int id_nfe_emitente_selecao_manual)
        {
            int qtde_empresa_selecionada = 0;
            List<int> lista_empresa_selecionada = new List<int>();

            foreach (var regra in lstRegras)
            {
                if (!string.IsNullOrEmpty(regra.Produto))
                {
                    foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                    {
                        if (r.Id_nfe_emitente > 0 &&
                            (id_nfe_emitente_selecao_manual == 0 || r.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                        {
                            if (r.Estoque_Qtde_Solicitado > 0)
                            {
                                qtde_empresa_selecionada++;
                                lista_empresa_selecionada.Add(r.Id_nfe_emitente);
                            }
                        }
                    }
                }
            }

            return lista_empresa_selecionada;
        }

        private async Task ExisteProdutoDescontinuado(PedidoProdutosDtoPedido produto, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();


            if (!string.IsNullOrEmpty(produto.NumProduto))
            {
                var produtoTask = (from c in db.Tprodutos
                                   where c.Produto == produto.NumProduto
                                   select c.Descontinuado).FirstOrDefaultAsync();
                var p = await produtoTask;

                if (p.ToUpper() == "S")
                {
                    if (produto.Qtde > produto.Qtde_estoque_total_disponivel)
                        lstErros.Add("Produto (" + produto.Fabricante + ")" + produto.NumProduto +
                            " consta como 'descontinuado' e não há mais saldo suficiente " +
                            "no estoque para atender à quantidade solicitada.");
                }
            }
        }

        //FIM PARA 1 PRODUTO


        //vamos tentar fazer outro metodo para verificar apenas 1 produto que foi selecionado pelo cliente
        //vamos montar a verificação para cada produto selecionado pelo usuário
        #region VerificarRegrasDisponibilidadeEstoqueProdutosSelecionados todos produtos não utilizado
        //public async Task<IEnumerable<string>> VerificarRegrasDisponibilidadeEstoqueProdutosSelecionados(PedidoDtoCriacao pedidoDto,
        //    string loja, string id_cliente, List<ProdutoDto> lstProdutos, int id_nfe_emitente_selecao_manual)
        //{
        //    List<string> lstMgsEstoque = new List<string>();

        //    var db = contextoProvider.GetContextoLeitura();
        //    //Buscar dados do cliente
        //    var clienteTask = (from c in db.Tclientes
        //                       where c.Id == id_cliente
        //                       select new
        //                       {
        //                           tipo_cliente = c.Tipo,
        //                           contribuite_icms_status = c.Contribuinte_Icms_Status,
        //                           produtor_rural_status = c.Produtor_Rural_Status,
        //                           uf = c.Uf
        //                       }).FirstOrDefaultAsync();

        //    var cliente = await clienteTask;

        //    //obtém  a sigla para regra
        //    string cliente_regra = Util.Util.MultiCdRegraDeterminaPessoa(cliente.tipo_cliente, cliente.contribuite_icms_status,
        //        cliente.produtor_rural_status);

        //    //List<RegrasBll> lst_cliente_regra = new List<RegrasBll>();
        //    //MontaListaRegras(lstProdutos, lst_cliente_regra);


        //    List<string> lstErros = new List<string>();

        //    List<RegrasBll> regraCrtlEstoque = (await ObterCtrlEstoqueProdutoRegra(pedidoDto, lstErros)).ToList();

        //    await Util.Util.ObterCtrlEstoqueProdutoRegra_Teste(lstErros, regraCrtlEstoque, cliente.uf, cliente_regra, contextoProvider);

        //    //parateste
        //    //int id_nfe_emitente_selecao_manual = 0;
        //    VerificarRegrasAssociadasAosProdutos(regraCrtlEstoque, lstErros, pedidoDto, id_nfe_emitente_selecao_manual);

        //    //verificar se se é cd manual
        //    await VerificarCDHabilitadoTodasRegras(regraCrtlEstoque, id_nfe_emitente_selecao_manual, lstErros);

        //    //precisamos criar um metodo ou arrumar esse mesmo para poder receber a quantidade de produtos selecionados
        //    //pelo usuário, pois é feito uma verificação em cima da quantidade que foi solicitada 
        //    Util.Util.ObterDisponibilidadeEstoque(regraCrtlEstoque, lstProdutos, lstErros,
        //        contextoProvider, id_nfe_emitente_selecao_manual);

        //    //await Util.Util.VerificarEstoque(regraCrtlEstoque, contextoProvider);
        //    await Util.Util.VerificarEstoqueComSubQuery(regraCrtlEstoque, contextoProvider);

        //    //atribui a qtde de estoque para o produto
        //    //aqui retorna msg do produto para ser verificas se o estoque é insuficiente
        //    //mas aqui jã foi informado sobre o estoque e continuou mesmo assim
        //    bool estoqueInsuficiente = VerificarEstoqueInsuficiente(regraCrtlEstoque, pedidoDto,
        //        id_nfe_emitente_selecao_manual, lstErros);

        //    VerificarQtdePedidosAutoSplit(regraCrtlEstoque, lstErros, pedidoDto);

        //    await ExisteMensagensAlertaProdutos(lstProdutos);

        //    List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, pedidoDto);

        //    //há algum produto descontinuado?
        //    await ExisteProdutoDescontinuado(pedidoDto, lstErros);

        //    if (lstErros.Count > 0)
        //    {
        //        //deu erro
        //        //quando seleção do cd automático, temos msgs 
        //        /*
        //         * "Produto (001)001000: regra 'regra 01' (Id=1) não permite o CD 'Cliente"
        //         * essa msg ocorreu , pois não teve nenhum cd selecionado, foi feito seleção automatica
        //         */
        //        /*
        //         * "Falha ao tentar consultar disponibilidade no estoque do produto (001)001000"
        //         * essa msg ocorreu, pois não tem cd selecionado, foi selecionado automatico
        //         */
        //        /*
        //         * Produto selecionado 
        //         * "PRODUTO SEM PRESENÇA NO ESTOQUE"
        //         */
        //        return lstErros;
        //    }

        //    //estou aqui para terminar isso.
        //    return lstMgsEstoque;
        //}
        #endregion 
        private async Task ExisteProdutoDescontinuado(PedidoDtoCriacao pedido, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();

            foreach (var p in pedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    var produtoTask = (from c in db.Tprodutos
                                       where c.Produto == p.NumProduto
                                       select c.Descontinuado).FirstOrDefaultAsync();
                    var produto = await produtoTask;

                    if (produto.ToUpper() == "S")
                    {
                        if (p.Qtde > p.Qtde_estoque_total_disponivel)
                            lstErros.Add("Produto (" + p.Fabricante + ")" + p.NumProduto +
                                " consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada.");
                    }
                }
            }
        }

        private List<int> ContagemEmpresasUsadasAutoSplit(List<RegrasBll> lstRegras, PedidoDtoCriacao pedido)
        {
            int qtde_empresa_selecionada = 0;
            List<int> lista_empresa_selecionada = new List<int>();

            foreach (var regra in lstRegras)
            {
                if (!string.IsNullOrEmpty(regra.Produto))
                {
                    foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                    {
                        if (r.Id_nfe_emitente > 0)
                        {
                            if (r.Estoque_Qtde_Solicitado > 0)
                            {
                                qtde_empresa_selecionada++;
                                lista_empresa_selecionada.Add(r.Id_nfe_emitente);
                            }
                        }
                    }
                }
            }

            return lista_empresa_selecionada;
        }

        //Metodo que verifica a qtde solicitada e estoque
        private void VerificarQtdePedidosAutoSplit(List<RegrasBll> lstRegras, List<string> lstErros, PedidoDtoCriacao pedido)
        {
            int qtde_a_alocar = 0;

            foreach (var p in pedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    qtde_a_alocar = (int)p.Qtde;

                    foreach (var regra in lstRegras)
                    {
                        if (qtde_a_alocar == 0)
                            break;
                        if (!string.IsNullOrEmpty(regra.Produto))
                        {
                            foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (qtde_a_alocar == 0)
                                    break;
                                if (re.Id_nfe_emitente > 0)
                                {
                                    if (re.St_inativo == 0)
                                    {
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.NumProduto)
                                        {
                                            if (re.Estoque_Qtde >= qtde_a_alocar)
                                            {
                                                re.Estoque_Qtde_Solicitado = (short)qtde_a_alocar;
                                                qtde_a_alocar = 0;
                                            }
                                            else if (re.Estoque_Qtde > 0)
                                            {
                                                re.Estoque_Qtde_Solicitado = re.Estoque_Qtde;
                                                qtde_a_alocar = qtde_a_alocar - re.Estoque_Qtde;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (qtde_a_alocar > 0)
                    {
                        foreach (var regra in lstRegras)
                        {
                            if (qtde_a_alocar == 0)
                                break;
                            if (regra.Produto == p.NumProduto)
                            {
                                foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                                {
                                    if (regra.Fabricante == p.Fabricante &&
                                       regra.Produto == p.NumProduto &&
                                       re.Id_nfe_emitente > 0 &&
                                       re.Id_nfe_emitente == regra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                    {
                                        re.Estoque_Qtde_Solicitado = (short)(qtde_a_alocar + 1);
                                        qtde_a_alocar = 0;
                                    }
                                }
                            }
                        }
                    }
                    if (qtde_a_alocar > 0)
                    {
                        lstErros.Add("Falha ao processar a alocação de produtos no estoque: restaram " + qtde_a_alocar + " unidades do produto (" +
                            p.Fabricante + ")" + p.NumProduto + " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD");
                    }
                }
            }
            //Verificar se necessita retornar algo
        }

        private bool VerificarEstoqueInsuficiente(List<RegrasBll> lstRegras, PedidoDtoCriacao pedidoDto,
            int id_nfe_emitente_selecao_manual, List<string> lstErros)
        {
            bool retorno = false;
            int qtde_estoque_total_disponivel = 0;
            int qtde_estoque_total_disponivel_global = 0;

            foreach (var p in pedidoDto.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    foreach (var regra in lstRegras)
                    {
                        if (!string.IsNullOrEmpty(regra.Produto))
                        {
                            foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (r.Id_nfe_emitente > 0 &&
                                    (id_nfe_emitente_selecao_manual == 0 || r.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                                {
                                    if (r.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                    {
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.NumProduto)
                                        {
                                            qtde_estoque_total_disponivel += r.Estoque_Qtde;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (qtde_estoque_total_disponivel_global == 0)
                    {
                        p.Qtde_estoque_total_disponivel = 0;

                    }
                    else
                    {
                        p.Qtde_estoque_total_disponivel = (short?)qtde_estoque_total_disponivel;
                    }
                    if (p.Qtde > p.Qtde_estoque_total_disponivel)
                    {
                        retorno = true;
                        lstErros.Add("PRODUTO SEM PRESENÇA NO ESTOQUE");
                        
                    }
                }
            }
            return retorno;

        }

        private async Task<IEnumerable<RegrasBll>> ObterCtrlEstoqueProdutoRegra(PedidoDtoCriacao pedidoDto, List<string> lstErros)
        {
            List<RegrasBll> lstRegrasCrtlEstoque = new List<RegrasBll>();

            var db = contextoProvider.GetContextoLeitura();

            //vamos verificar passando todos os produtos simples da lista de produto que irá para ser selecionado


            foreach (var item in pedidoDto.ListaProdutos)
            {
                var regraProdutoTask = from c in db.TprodutoXwmsRegraCds
                                       where c.Fabricante == item.Fabricante &&
                                             c.Produto == item.NumProduto
                                       select c;

                var regra = await regraProdutoTask.FirstOrDefaultAsync();

                if (regra == null)
                {
                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + pedidoDto.DadosCliente.Uf + "' e '" +
                        Util.Util.DescricaoMultiCDRegraTipoPessoa(pedidoDto.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
                        item.NumProduto + " não possui regra associada");
                }
                else
                {
                    if (regra.Id_wms_regra_cd == 0)
                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + pedidoDto.DadosCliente.Uf + "' e '" +
                            Util.Util.DescricaoMultiCDRegraTipoPessoa(pedidoDto.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
                            item.NumProduto + " não está associado a nenhuma regra");
                    else
                    {
                        var wmsRegraTask = from c in db.TwmsRegraCds
                                           where c.Id == regra.Id_wms_regra_cd
                                           select c;

                        var wmsRegra = await wmsRegraTask.FirstOrDefaultAsync();
                        if (wmsRegra == null)
                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + pedidoDto.DadosCliente.Uf + "' e '" +
                                Util.Util.DescricaoMultiCDRegraTipoPessoa(pedidoDto.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                item.NumProduto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
                        else
                        {
                            RegrasBll itemRegra = new RegrasBll();
                            itemRegra.Fabricante = item.Fabricante;
                            itemRegra.Produto = item.NumProduto;

                            itemRegra.TwmsRegraCd = new t_WMS_REGRA_CD
                            {
                                Id = wmsRegra.Id,
                                Apelido = wmsRegra.Apelido,
                                Descricao = wmsRegra.Descricao,
                                St_inativo = wmsRegra.St_inativo
                            };

                            var wmsRegraCdXUfTask = from c in db.TwmsRegraCdXUfs
                                                    where c.Id_wms_regra_cd == itemRegra.TwmsRegraCd.Id &&
                                                          c.Uf == pedidoDto.DadosCliente.Uf
                                                    select c;
                            var wmsRegraCdXUf = await wmsRegraCdXUfTask.FirstOrDefaultAsync();

                            if (wmsRegraCdXUf == null)
                            {
                                itemRegra.St_Regra = false;
                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + pedidoDto.DadosCliente.Uf + "' e '" +
                                    Util.Util.DescricaoMultiCDRegraTipoPessoa(pedidoDto.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                    item.NumProduto + " não está cadastrada para a UF '" + pedidoDto.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
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
                                var tipo_pessoa = Util.Util.MultiCdRegraDeterminaPessoa(pedidoDto.DadosCliente.Tipo,
                                    pedidoDto.DadosCliente.Contribuinte_Icms_Status, pedidoDto.DadosCliente.ProdutorRural);

                                var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
                                                               where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
                                                                     c.Tipo_pessoa == tipo_pessoa
                                                               select c;

                                var wmsRegraCdXUfXPessoa = await wmsRegraCdXUfXPessoaTask.FirstOrDefaultAsync();

                                if (wmsRegraCdXUfXPessoa == null)
                                {
                                    itemRegra.St_Regra = false;
                                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + pedidoDto.DadosCliente.Uf + "' e '" +
                                        Util.Util.DescricaoMultiCDRegraTipoPessoa(pedidoDto.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                        item.NumProduto + " não está cadastrada para a UF '" + pedidoDto.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
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
                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + pedidoDto.DadosCliente.Uf + "' e '" +
                                            Util.Util.DescricaoMultiCDRegraTipoPessoa(pedidoDto.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                            item.NumProduto + " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id=" + regra.Id_wms_regra_cd + ")");
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
                                                lstErros.Add("Falha na regra de consumo do estoque para a UF '" + pedidoDto.DadosCliente.Uf + "' e '" +
                                                    Util.Util.DescricaoMultiCDRegraTipoPessoa(pedidoDto.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante +
                                                    ")" + item.NumProduto + " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
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
                                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + pedidoDto.DadosCliente.Uf + "' e '" +
                                                Util.Util.DescricaoMultiCDRegraTipoPessoa(pedidoDto.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                                item.NumProduto + " não especifica nenhum CD para consumo do estoque (Id=" + regra.Id_wms_regra_cd + ")");
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
                                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + pedidoDto.DadosCliente.Uf + "' e '" +
                                                            Util.Util.DescricaoMultiCDRegraTipoPessoa(pedidoDto.DadosCliente.Tipo) + "': regra associada ao produto (" +
                                                            item.Fabricante + ")" + item.NumProduto + " especifica o CD '" +
                                                            Util.Util.ObterApelidoEmpresaNfeEmitentes(
                                                                wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente, 
                                                                contextoProvider) +
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

        public void VerificarRegrasAssociadasAosProdutos(List<RegrasBll> lst, List<string> lstErros,
            PedidoDtoCriacao dtoPedido, int id_nfe_emitente_selecao_manual)
        {
            int qtdeCdAtivo = 0;
            //id_nfe_emitente_selecao_manual = 0;//esse é a seleção do checkebox 

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
        public static async Task VerificarCDHabilitadoTodasRegras(List<RegrasBll> lstRegras, 
            int id_nfe_emitente_selecao_manual, List<string> lstErros, InfraBanco.ContextoBdProvider contextoBd)
        {
            //id_nfe_emitente_selecao_manual = 0;//esse é a seleção do checkebox 
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
                        if (t.Id_nfe_emitente == id_nfe_emitente_selecao_manual)
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
                        await Util.Util.ObterApelidoEmpresaNfeEmitentes(id_nfe_emitente_selecao_manual, contextoBd));
                }
                else if (desativado)
                {
                    lstErrosAux.Add("Regra '" + i.TwmsRegraCd.Apelido + "'(Id = " + i.TwmsRegraCd.Id + ") define o CD '" +
                        await Util.Util.ObterApelidoEmpresaNfeEmitentes(id_nfe_emitente_selecao_manual, contextoBd) + 
                        "' como 'desativado'");
                }

            }

            if (lstErrosAux.Count > 0)
            {
                //não iremos utilizar essa msg, mas deixaremos aqui caso necessite
                //string erro = "O CD selecionado manualmente não pode ser usado devido aos seguintes motivos:";
                foreach (var e in lstErrosAux)
                {
                    lstErros.Add(e);
                }

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
