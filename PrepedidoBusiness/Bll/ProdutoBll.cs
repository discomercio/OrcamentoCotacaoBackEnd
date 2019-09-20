using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Produto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfraBanco.Modelos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PrepedidoBusiness.Utils;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Bll.Regras;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using PrepedidoBusiness.Dto.Prepedido;

namespace PrepedidoBusiness.Bll
{
    public class ProdutoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public ProdutoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }
        #region
        //public async Task<IEnumerable<ProdutoDto>> BuscarProduto(string codProduto, string loja, string apelido, List<string> lstErros)
        //{
        //    //paraTeste
        //    //apelido = "MARISARJ";
        //    //codProduto = "003000";
        //    //loja = "202";
        //    int qtde = 0;
        //    bool lojaHabilitada = false;
        //    decimal vlProdCompostoPrecoListaLoja = 0;
        //    ProdutoDto produtoDto = new ProdutoDto();

        //    List<ProdutoDto> lstProduto = new List<ProdutoDto>();

        //    var db = contextoProvider.GetContextoLeitura();

        //    if (string.IsNullOrEmpty(codProduto))
        //        return null;
        //    else if (string.IsNullOrEmpty(loja))
        //        return null;

        //    if (Util.LojaHabilitadaProdutosECommerce(loja))
        //    {
        //        var prodCompostoTask = from c in db.TecProdutoCompostos
        //                               where c.Produto_Composto == codProduto
        //                               select c;

        //        string parada = "";

        //        var prodComposto = prodCompostoTask.FirstOrDefault();

        //        if (prodComposto.Produto_Composto != null)
        //        {
        //            var prodCompostoItensTask = from c in db.TecProdutoCompostoItems
        //                                        where c.Fabricante_composto == prodComposto.Fabricante_Composto &&
        //                                              c.Produto_composto == prodComposto.Produto_Composto &&
        //                                              c.Excluido_status == 0
        //                                        orderby c.Sequencia
        //                                        select c;
        //            var prodCompostoItens = prodCompostoItensTask.ToList();

        //            if (prodCompostoItens.Count > 0)
        //            {
        //                foreach (var pi in prodCompostoItens)
        //                {
        //                    var produtoTask = from c in db.Tprodutos.Include(r => r.TprodutoLoja)
        //                                      where c.TprodutoLoja.Fabricante == pi.Fabricante_item &&
        //                                            c.TprodutoLoja.Produto == pi.Produto_item &&
        //                                            c.TprodutoLoja.Loja == loja
        //                                      select c;

        //                    var produto = await produtoTask.FirstOrDefaultAsync();

        //                    if (string.IsNullOrEmpty(produto.Produto))
        //                        lstErros.Add("O produto(" + pi.Fabricante_item + ")" + pi.Produto_item + " não está disponível para a loja " + loja + "!!");
        //                    else
        //                    {
        //                        produtoDto = new ProdutoDto
        //                        {
        //                            Fabricante = pi.Fabricante_item,
        //                            Produto = pi.Produto_item,
        //                            Qtde = pi.Qtde,
        //                            ValorLista = produto.TprodutoLoja.Preco_Lista,
        //                            Descricao = produto.Descricao
        //                        };
        //                    }
        //                }
        //            }

        //        }
        //        else
        //        {
        //            var produtoTask = from c in db.Tprodutos.Include(r => r.TprodutoLoja)
        //                              where c.TprodutoLoja.Produto == codProduto &&
        //                                    c.TprodutoLoja.Loja == loja
        //                              select c;

        //            var produto = await produtoTask.FirstOrDefaultAsync();

        //            if (string.IsNullOrEmpty(produto.Produto))
        //                lstErros.Add("Produto '" + codProduto + "' não foi encontrado para a loja " + loja + "!!");
        //            else
        //            {
        //                produtoDto = new ProdutoDto
        //                {
        //                    Fabricante = produto.Fabricante,
        //                    Produto = produto.Produto,
        //                    Qtde = qtde,
        //                    ValorLista = produto.TprodutoLoja.Preco_Lista,
        //                    Descricao = produto.Descricao
        //                };
        //            }
        //        }
        //    }



        //    return lstProduto;
        //}

        //public async Task<ProdutoDto> ListarProdutosCombo(string loja)
        //{
        //    //Necessario buscar os dados do Orcamentista para verificação abaixo
        //    if (Util.LojaHabilitadaProdutosECommerce(loja))
        //    {

        //    }
        //}
        #endregion

        public async Task<ProdutoComboDto> ListaProdutosCombo(string apelido, string loja, string id_cliente)
        {
            ProdutoComboDto retorno = new ProdutoComboDto();
            //string loja = "202";
            //string id_cliente = "000000605954";

            //var db = contextoProvider.GetContextoLeitura();
            ////Buscar dados do cliente
            //var clienteTask = (from c in db.Tclientes
            //                   where c.Id == id_cliente
            //                   select new
            //                   {
            //                       tipo_cliente = c.Tipo,
            //                       contribuite_icms_status = c.Contribuinte_Icms_Status,
            //                       produtor_rural_status = c.Produtor_Rural_Status,
            //                       uf = c.Uf
            //                   }).FirstOrDefaultAsync();

            //var cliente = await clienteTask;

            ////obtém  a sigla para regra
            //string cliente_regra = Util.MultiCdRegraDeterminaPessoa(cliente.tipo_cliente, cliente.contribuite_icms_status,
            //    cliente.produtor_rural_status);

            var lstProdutosCompostos = BuscarProdutosCompostos(loja);
            List<ProdutoDto> lstTodosProdutos = (await BuscarTodosProdutos(loja)).ToList();

            //List<RegrasBll> lst_cliente_regra = new List<RegrasBll>();
            //MontaListaRegras(lstTodosProdutos, lst_cliente_regra);

            //List<string> lstErros = new List<string>();
            //await ObterCtrlEstoqueProdutoRegra_Teste(lstErros, lst_cliente_regra, cliente);
            ////varificar se houve erros ao processar as regras
            //if (lstErros.Count > 0)
            //{
            //    //chama o metodo verificar regras
            //}

            retorno.ProdutoCompostoDto = (await lstProdutosCompostos).ToList();
            retorno.ProdutoDto = lstTodosProdutos;
            return retorno;
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

        public async Task<IEnumerable<ProdutoCompostoDto>> BuscarProdutosCompostos(string loja)
        {

            loja = "202";
            var db = contextoProvider.GetContextoLeitura();

            var produtosCompostosTask = from d in (from c in db.Tprodutos
                                                   join pc in db.TecProdutoCompostos on c.Produto equals pc.Produto_Composto
                                                   join pci in db.TecProdutoCompostoItems on pc.Fabricante_Composto equals pci.Fabricante_composto
                                                   join pl in db.TprodutoLojas on pci.Produto_item equals pl.Produto
                                                   where pl.Loja == loja &&
                                                         pl.Vendavel == "S" &&
                                                         c.Fabricante == pc.Fabricante_Composto &&
                                                         pc.Produto_Composto == pci.Produto_composto
                                                   select new
                                                   {

                                                       fabricante_pai = c.Fabricante,
                                                       produto_pai = c.Produto,
                                                       valor = (decimal)pl.Preco_Lista,
                                                       qtde = (int)pci.Qtde,
                                                       produtosFilhos = new ProdutoFilhoDto
                                                       {
                                                           Fabricante = c.Fabricante,
                                                           Produto = pci.Produto_item,
                                                           Qtde = pci.Qtde
                                                       }
                                                   })
                                        group d by d.produto_pai into g
                                        select new ProdutoCompostoDto
                                        {
                                            PaiFabricante = g.Select(r => r.fabricante_pai).FirstOrDefault(),
                                            PaiProduto = g.Select(r => r.produto_pai).FirstOrDefault(),
                                            Preco_total_Itens = g.Sum(r => r.qtde * r.valor),
                                            Filhos = g.Select(r => r.produtosFilhos).ToList()
                                        };

            List<ProdutoCompostoDto> produto = await produtosCompostosTask.ToListAsync();

            return produto;
        }

        public async Task<IEnumerable<ProdutoDto>> BuscarTodosProdutos(string loja)
        {
            loja = "202";

            var db = contextoProvider.GetContextoLeitura();

            var todosProdutosTask = from c in db.Tprodutos
                                    join pl in db.TprodutoLojas on c.Produto equals pl.Produto
                                    where pl.Vendavel == "S" &&
                                          pl.Loja == loja
                                    select new ProdutoDto
                                    {
                                        Fabricante = c.Fabricante,
                                        Produto = pl.Produto,
                                        Descricao_html = c.Descricao_Html,
                                        Preco_lista = pl.Preco_Lista
                                    };

            List<ProdutoDto> lstTodosProdutos = await todosProdutosTask.ToListAsync();

            return lstTodosProdutos;
        }

        //Monta produtos seguindo as regras existentes
        //public async Task<IEnumerable<ProdutoDto>> MontaProdutos(List<PrepedidoProdutoDtoPrepedido> lstProdutos, List<string> lstErros, string loja)
        //{
        //    ProdutoDto produtoDto = new ProdutoDto();
        //    List<ProdutoDto> lstProdutoDto = new List<ProdutoDto>();

        //    var db = contextoProvider.GetContextoLeitura();

        //    foreach (var p in lstProdutos)
        //    {
        //        string fabricante = Util.Normaliza_Codigo(p.Fabricante, Constantes.TAM_MIN_FABRICANTE);
        //        string codProduto = Util.Normaliza_Codigo(p.NumProduto, Constantes.TAM_MIN_PRODUTO);

        //        int qtde = (int)p.Qtde;

        //        if (string.IsNullOrEmpty(fabricante) && !string.IsNullOrEmpty(codProduto))
        //        {
        //            var prodCompostoTask = from c in db.TecProdutoCompostos
        //                                   where c.Produto_Composto == codProduto
        //                                   select c;
        //            var prodComposto = (await prodCompostoTask.FirstOrDefaultAsync());

        //            if (prodComposto.Produto_Composto != null)
        //            {
        //                var prodCompostoItensTask = from c in db.TecProdutoCompostoItems
        //                                            where c.Fabricante_composto == prodComposto.Fabricante_Composto &&
        //                                                  c.Produto_composto == prodComposto.Produto_Composto &&
        //                                                  c.Excluido_status == 0
        //                                            orderby c.Sequencia
        //                                            select c;
        //                var prodCompostoItens = prodCompostoItensTask.ToList();

        //                if (prodCompostoItens.Count > 0)
        //                {
        //                    foreach (var pi in prodCompostoItens)
        //                    {
        //                        var produtoTask = from c in db.Tprodutos.Include(r => r.TprodutoLoja)
        //                                          where c.TprodutoLoja.Fabricante == pi.Fabricante_item &&
        //                                                c.TprodutoLoja.Produto == pi.Produto_item &&
        //                                                c.TprodutoLoja.Loja == loja
        //                                          select c;

        //                        var produto = await produtoTask.FirstOrDefaultAsync();

        //                        if (string.IsNullOrEmpty(produto.Produto))
        //                            lstErros.Add("O produto(" + pi.Fabricante_item + ")" + pi.Produto_item + " não está disponível para a loja " + loja + "!!");
        //                        else
        //                        {
        //                            produtoDto = new ProdutoDto
        //                            {
        //                                Fabricante = pi.Fabricante_item,
        //                                Produto = pi.Produto_item,
        //                                Qtde = pi.Qtde,
        //                                ValorLista = produto.TprodutoLoja.Preco_Lista,
        //                                Descricao = produto.Descricao
        //                            };
        //                            lstProdutoDto.Add(produtoDto);
        //                        }
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                //faz produto normal
        //                var produtoTask = from c in db.Tprodutos.Include(r => r.TprodutoLoja)
        //                                  where c.TprodutoLoja.Fabricante == fabricante &&
        //                                        c.TprodutoLoja.Produto == codProduto &&
        //                                        c.TprodutoLoja.Loja == loja
        //                                  select c;

        //                var produto = await produtoTask.FirstOrDefaultAsync();

        //                if (string.IsNullOrEmpty(produto.Produto))
        //                    lstErros.Add("Produto '" + codProduto + "' não foi encontrado para a loja " + loja + "!!");

        //                produtoDto = new ProdutoDto
        //                {
        //                    Fabricante = produto.Fabricante,
        //                    Produto = produto.Produto,
        //                    Qtde = qtde,
        //                    ValorLista = produto.TprodutoLoja.Preco_Lista,
        //                    Descricao = produto.Descricao
        //                };
        //                lstProdutoDto.Add(produtoDto);
        //            }
        //        }

        //    }

        //    return lstProdutoDto;
        //}



        public async Task<IEnumerable<RegrasBll>> ObterCtrlEstoqueProdutoRegra_Teste(List<string> lstErros,
            List<RegrasBll> lstRegrasCrtlEstoque, dynamic cliente)
        {
            //o cliente esta sendo passado como dynamic
            string uf = cliente.uf;
            string tipo = cliente.tipo_cliente;

            var db = contextoProvider.GetContextoLeitura();

            var testeRegras = from c in db.TprodutoXwmsRegraCds
                              join r1 in db.TwmsRegraCds on c.Id_wms_regra_cd equals r1.Id
                              join r2 in db.TwmsRegraCdXUfs on r1.Id equals r2.Id_wms_regra_cd
                              join r3 in db.TwmsRegraCdXUfPessoas on r2.Id equals r3.Id_wms_regra_cd_x_uf
                              join r4 in db.TwmsRegraCdXUfXPessoaXCds on r3.Id equals r4.Id_wms_regra_cd_x_uf_x_pessoa
                              where r2.Uf == uf &&
                                    r3.Tipo_pessoa == tipo
                              orderby r4.Ordem_prioridade
                              select new
                              {
                                  prod_x_reg = c,
                                  regra1 = r1,
                                  regra2 = r2,
                                  regra3 = r3,
                                  regra4 = r4
                              };
            var llista = testeRegras.ToList();
            var x = 1;

            RegrasBll itemRegra = new RegrasBll();

            foreach (var item in lstRegrasCrtlEstoque)
            {
                foreach (var r in llista)
                {
                    if (r.prod_x_reg.Produto == item.Produto)
                    {
                        item.St_Regra = true;
                        item.TwmsRegraCd = new t_WMS_REGRA_CD
                        {
                            Id = r.regra1.Id,
                            Apelido = r.regra1.Apelido,
                            Descricao = r.regra1.Descricao,
                            St_inativo = r.regra1.St_inativo

                        };
                        item.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
                        {
                            Id = r.regra2.Id,
                            Id_wms_regra_cd = r.regra2.Id_wms_regra_cd,
                            Uf = r.regra2.Uf,
                            St_inativo = r.regra2.St_inativo
                        };
                        item.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
                        {
                            Id = r.regra3.Id,
                            Id_wms_regra_cd_x_uf = r.regra3.Id_wms_regra_cd_x_uf,
                            Tipo_pessoa = r.regra3.Tipo_pessoa,
                            St_inativo = r.regra3.St_inativo,
                            Spe_id_nfe_emitente = r.regra3.Spe_id_nfe_emitente
                        };
                        item.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();

                        t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
                        {
                            Id = r.regra4.Id,
                            Id_wms_regra_cd_x_uf_x_pessoa = r.regra4.Id_wms_regra_cd_x_uf_x_pessoa,
                            Id_nfe_emitente = r.regra4.Id_nfe_emitente,
                            Ordem_prioridade = r.regra4.Ordem_prioridade,
                            St_inativo = r.regra4.St_inativo
                        };

                        item.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);

                    }
                }
            }
            #region
            //    var regraProdutoTask = from c in db.TprodutoXwmsRegraCds
            //                           where c.Fabricante == item.Fabricante &&
            //                                 c.Produto == item.Produto
            //                           select c;

            //    var regra = await regraProdutoTask.FirstOrDefaultAsync();

            //    if (regra == null)
            //    {
            //        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + cliente.uf + "' e '" +
            //            Util.DescricaoMultiCDRegraTipoPessoa(cliente.tipo_cliente) + "': produto (" + item.Fabricante + ")" +
            //            item.Produto + " não possui regra associada");
            //    }
            //    else
            //    {
            //        if (regra.Id_wms_regra_cd == 0)
            //            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + cliente.uf + "' e '" +
            //                Util.DescricaoMultiCDRegraTipoPessoa(cliente.tipo_cliente) + "': produto (" + item.Fabricante + ")" +
            //                item.Produto + " não está associado a nenhuma regra");
            //        else
            //        {
            //            var wmsRegraTask = from c in db.TwmsRegraCds
            //                               where c.Id == regra.Id_wms_regra_cd
            //                               select c;

            //            var wmsRegra = await wmsRegraTask.FirstOrDefaultAsync();
            //            if (wmsRegra == null)
            //                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + cliente.uf + "' e '" +
            //                    Util.DescricaoMultiCDRegraTipoPessoa(cliente.tipo_cliente) + "': regra associada ao produto (" + item.Fabricante + ")" +
            //                    item.Produto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
            //            else
            //            {

            //                //fazer montagem dos dados apartir daqui

            //                itemRegra.Fabricante = item.Fabricante;
            //                itemRegra.Produto = item.Produto;

            //                item.TwmsRegraCd = new t_WMS_REGRA_CD
            //                {
            //                    Id = wmsRegra.Id,
            //                    Apelido = wmsRegra.Apelido,
            //                    Descricao = wmsRegra.Descricao,
            //                    St_inativo = wmsRegra.St_inativo
            //                };

            //                //itemRegra.TwmsRegraCd = new t_WMS_REGRA_CD
            //                //{
            //                //    Id = wmsRegra.Id,
            //                //    Apelido = wmsRegra.Apelido,
            //                //    Descricao = wmsRegra.Descricao,
            //                //    St_inativo = wmsRegra.St_inativo
            //                //};

            //                var wmsRegraCdXUfTask = from c in db.TwmsRegraCdXUfs
            //                                        where c.Id_wms_regra_cd == item.TwmsRegraCd.Id &&
            //                                              c.Uf == uf
            //                                        select c;
            //                var wmsRegraCdXUf = await wmsRegraCdXUfTask.FirstOrDefaultAsync();

            //                if (wmsRegraCdXUf == null)
            //                {
            //                    item.St_Regra = false;
            //                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + cliente.uf + "' e '" +
            //                        Util.DescricaoMultiCDRegraTipoPessoa(cliente.tipo_cliente) + "': regra associada ao produto (" + item.Fabricante + ")" +
            //                        item.Produto + " não está cadastrada para a UF '" + cliente.uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
            //                }
            //                else
            //                {
            //                    item.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
            //                    {
            //                        Id = wmsRegraCdXUf.Id,
            //                        Id_wms_regra_cd = wmsRegraCdXUf.Id_wms_regra_cd,
            //                        Uf = wmsRegraCdXUf.Uf,
            //                        St_inativo = wmsRegraCdXUf.St_inativo
            //                    };
            //                    //itemRegra.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
            //                    //{
            //                    //    Id = wmsRegraCdXUf.Id,
            //                    //    Id_wms_regra_cd = wmsRegraCdXUf.Id_wms_regra_cd,
            //                    //    Uf = wmsRegraCdXUf.Uf,
            //                    //    St_inativo = wmsRegraCdXUf.St_inativo
            //                    //};

            //                    var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
            //                                                   where c.Id_wms_regra_cd_x_uf == item.TwmsRegraCdXUf.Id &&
            //                                                         c.Tipo_pessoa == tipo
            //                                                   select c;

            //                    var wmsRegraCdXUfXPessoa = await wmsRegraCdXUfXPessoaTask.FirstOrDefaultAsync();

            //                    if (wmsRegraCdXUfXPessoa == null)
            //                    {
            //                        item.St_Regra = false;
            //                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + cliente.uf + "' e '" +
            //                            Util.DescricaoMultiCDRegraTipoPessoa(cliente.tipo_cliente) + "': regra associada ao produto (" + item.Fabricante + ")" +
            //                            item.Produto + " não está cadastrada para a UF '" + cliente.uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
            //                    }
            //                    else
            //                    {
            //                        item.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
            //                        {
            //                            Id = wmsRegraCdXUfXPessoa.Id,
            //                            Id_wms_regra_cd_x_uf = wmsRegraCdXUfXPessoa.Id_wms_regra_cd_x_uf,
            //                            Tipo_pessoa = wmsRegraCdXUfXPessoa.Tipo_pessoa,
            //                            St_inativo = wmsRegraCdXUfXPessoa.St_inativo,
            //                            Spe_id_nfe_emitente = wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
            //                        };
            //                        //itemRegra.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
            //                        //{
            //                        //    Id = wmsRegraCdXUfXPessoa.Id,
            //                        //    Id_wms_regra_cd_x_uf = wmsRegraCdXUfXPessoa.Id_wms_regra_cd_x_uf,
            //                        //    Tipo_pessoa = wmsRegraCdXUfXPessoa.Tipo_pessoa,
            //                        //    St_inativo = wmsRegraCdXUfXPessoa.St_inativo,
            //                        //    Spe_id_nfe_emitente = wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
            //                        //};

            //                        if (wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
            //                        {
            //                            item.St_Regra = false;
            //                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + cliente.uf + "' e '" +
            //                                Util.DescricaoMultiCDRegraTipoPessoa(cliente.tipo_cliente) + "': regra associada ao produto (" + item.Fabricante + ")" +
            //                                item.Produto + " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id=" + regra.Id_wms_regra_cd + ")");
            //                        }
            //                        else
            //                        {
            //                            var nfEmitenteTask = from c in db.TnfEmitentes
            //                                                 where c.Id == wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
            //                                                 select c;
            //                            var nfEmitente = await nfEmitenteTask.FirstOrDefaultAsync();

            //                            if (nfEmitente != null)
            //                            {
            //                                if (nfEmitente.St_Ativo != 1)
            //                                {
            //                                    item.St_Regra = false;
            //                                    lstErros.Add("Falha na regra de consumo do estoque para a UF '" + cliente.uf + "' e '" +
            //                                        Util.DescricaoMultiCDRegraTipoPessoa(cliente.tipo_cliente) + "': regra associada ao produto (" + item.Fabricante +
            //                                        ")" + item.Produto + " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
            //                                        "(Id=" + regra.Id_wms_regra_cd + ")");
            //                                }
            //                            }
            //                            var wmsRegraCdXUfXPessoaXcdTask = from c in db.TwmsRegraCdXUfXPessoaXCds
            //                                                              where c.Id_wms_regra_cd_x_uf_x_pessoa == wmsRegraCdXUfXPessoa.Id
            //                                                              orderby c.Ordem_prioridade
            //                                                              select c;
            //                            var wmsRegraCdXUfXPessoaXcd = await wmsRegraCdXUfXPessoaXcdTask.ToListAsync();

            //                            if (wmsRegraCdXUfXPessoaXcd.Count == 0)
            //                            {
            //                                item.St_Regra = false;
            //                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + cliente.uf + "' e '" +
            //                                    Util.DescricaoMultiCDRegraTipoPessoa(cliente.tipo_cliente) + "': regra associada ao produto (" + item.Fabricante + ")" +
            //                                    item.Produto + " não especifica nenhum CD para consumo do estoque (Id=" + regra.Id_wms_regra_cd + ")");
            //                            }
            //                            else
            //                            {
            //                                foreach (var i in wmsRegraCdXUfXPessoaXcd)
            //                                {
            //                                    t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
            //                                    {
            //                                        Id = i.Id,
            //                                        Id_wms_regra_cd_x_uf_x_pessoa = i.Id_wms_regra_cd_x_uf_x_pessoa,
            //                                        Id_nfe_emitente = i.Id_nfe_emitente,
            //                                        Ordem_prioridade = i.Ordem_prioridade,
            //                                        St_inativo = i.St_inativo
            //                                    };

            //                                    var nfeCadastroPrincipalTask = from c in db.TnfEmitentes
            //                                                                   where c.Id == item_cd_uf_pess_cd.Id_nfe_emitente
            //                                                                   select c;

            //                                    var nfeCadastroPrincipal = await nfeCadastroPrincipalTask.FirstOrDefaultAsync();

            //                                    if (nfeCadastroPrincipal != null)
            //                                    {
            //                                        if (nfeCadastroPrincipal.St_Ativo != 1)
            //                                            item_cd_uf_pess_cd.St_inativo = 1;
            //                                    }
            //                                    item.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();
            //                                    item.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
            //                                    //itemRegra.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
            //                                }
            //                                foreach (var i in item.TwmsCdXUfXPessoaXCd)
            //                                {

            //                                    if (i.Id_nfe_emitente == item.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
            //                                    {
            //                                        if (i.St_inativo == 1)
            //                                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + cliente.uf + "' e '" +
            //                                                Util.DescricaoMultiCDRegraTipoPessoa(cliente.tipo_cliente) + "': regra associada ao produto (" +
            //                                                item.Fabricante + ")" + item.Produto + " especifica o CD '" + Util.ObterApelidoEmpresaNfeEmitentes(wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente, contextoProvider) +
            //                                                "' para alocação de produtos sem presença no estoque, sendo que este CD está desativado para " +
            //                                                "processar produtos disponíveis (Id=" + regra.Id_wms_regra_cd + ")");
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //                //lstRegrasCrtlEstoque.Add(itemRegra);
            //            }
            //        }
            //    }
            //}
            #endregion
            return lstRegrasCrtlEstoque;
        }


        public static void VerificarRegrasAssociadasAosProdutos(List<RegrasBll> lstRegras, List<string> lstErros, DadosClienteCadastroDto cliente)
        {
            foreach (var r in lstRegras)
            {
                if (!string.IsNullOrEmpty(r.Produto))
                {
                    if (r.TwmsRegraCd.Id == 0)
                        lstErros.Add("Produto (" + r.Fabricante + ")" + r.Produto + " não possui regra de consumo do estoque associada");
                    else if (r.TwmsRegraCd.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto(" +
                            r.Fabricante + ")" + r.Produto + " está desativada");
                    }
                    else if (r.TwmsRegraCdXUf.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " está bloqueada para a UF '" + cliente.Uf + "'");
                    }
                    else if (r.TwmsRegraCdXUfXPessoa.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " está bloqueada para clientes '" + cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    else if (r.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    int verificaErros = 0;
                    foreach (var re in r.TwmsCdXUfXPessoaXCd)
                    {
                        if (re.Id_nfe_emitente > 0)
                        {
                            if (re.St_inativo == 0)
                                verificaErros++;
                        }
                    }
                    if (verificaErros == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + r.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            r.Fabricante + ")" + r.Produto + " não especifica nenhum CD ativo para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                }
            }
        }

    }
}
