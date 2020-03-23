import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Location } from '@angular/common';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { NovoPrepedidoDadosService } from '../novo-prepedido-dados.service';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { Router, ActivatedRoute } from '@angular/router';
import { PrepedidoProdutoDtoPrepedido } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrepedidoProdutoDtoPrepedido';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { PrepedidoBuscarService } from 'src/app/servicos/prepedido/prepedido-buscar.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { ProdutoService } from 'src/app/servicos/produto/produto.service';
import { MatDialog } from '@angular/material';
import { SelecProdDialogComponent } from '../selec-prod-dialog/selec-prod-dialog.component';
import { SelecProdInfo } from '../selec-prod-dialog/selec-prod-info';
import { ProdutoComboDto } from 'src/app/dto/Produto/ProdutoComboDto';
import { ProdutoDto } from 'src/app/dto/Produto/ProdutoDto';
import { asapScheduler } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/utils/confirmation-dialog/confirmation-dialog.component';
import { DadosPagtoComponent } from '../dados-pagto/dados-pagto.component';
import { NgForm } from '@angular/forms';
import { debugOutputAstAsTypeScript } from '@angular/compiler';
import { ConfirmarEnderecoComponent } from '../confirmar-endereco/confirmar-endereco.component';
import { ConfirmarClienteComponent } from '../confirmar-cliente/confirmar-cliente.component';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { TestBed } from '@angular/core/testing';
import { CepComponent } from 'src/app/cliente/cep/cep/cep.component';
import { listLazyRoutes } from '@angular/compiler/src/aot/lazy_routes';
import { ProdutoCompostoDto } from 'src/app/dto/Produto/ProdutoCompostoDto';

@Component({
  selector: 'app-itens',
  templateUrl: './itens.component.html',
  styleUrls: [
    './itens.component.scss',
    '../../../estilos/tabelaresponsiva.scss',
    '../../../estilos/numeros.scss'
  ]
})
export class ItensComponent extends TelaDesktopBaseComponent implements OnInit {

  //#region dados
  //dados sendo criados
  criando = true;
  prePedidoDto: PrePedidoDto;
  //#endregion

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly location: Location,
    private readonly router: Router,
    private readonly novoPrepedidoDadosService: NovoPrepedidoDadosService,
    public readonly prepedidoBuscarService: PrepedidoBuscarService,
    public readonly alertaService: AlertaService,
    public readonly produtoService: ProdutoService,
    public readonly dialog: MatDialog,
    telaDesktopService: TelaDesktopService
  ) {
    super(telaDesktopService);
  }

  carregandoDto = true;
  ngOnInit() {

    //se tem um parâmetro no link, colocamos ele no serviço
    let numeroPrepedido = this.activatedRoute.snapshot.params.numeroPrepedido;
    if (!!numeroPrepedido) {
      //se for igual ao que está no serviço, podemos usar o que está no serviço
      this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
      if (!this.prePedidoDto || this.prePedidoDto.NumeroPrePedido.toString() !== numeroPrepedido.toString()) {
        //ou não tem nada no serviço ou está diferente
        this.prepedidoBuscarService.buscar(numeroPrepedido).subscribe({
          next: (r) => {
            if (r == null) {
              this.alertaService.mostrarErroInternet(r);
              this.router.navigate(["/novo-prepedido"]);
              return;
            }
            //detalhes do prepedido
            this.carregandoDto = false;
            this.prePedidoDto = r;
            this.novoPrepedidoDadosService.setar(r);
            this.criando = !this.prePedidoDto.NumeroPrePedido;
            this.inscreverPermite_RA_Status();
            this.inscreverProdutoComboDto();

          },
          error: (r) => { debugger; this.alertaService.mostrarErroInternet(r) }
        });
        return;
      }
    }

    this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
    //se veio diretamente para esta tela, e não tem nada no serviço, não podemos continuar
    //então voltamos para o começo do processo!
    if (!this.prePedidoDto) {
      this.router.navigate(["/novo-prepedido"]);
      return;
    }

    this.carregandoDto = false;
    this.criando = !this.prePedidoDto.NumeroPrePedido;
    this.inscreverPermite_RA_Status();
    this.inscreverProdutoComboDto();
    this.obtemPercentualVlPedidoRA();
  }

  //#region formatação de dados para a tela

  moedaUtils: MoedaUtils = new MoedaUtils();

  cpfCnpj() {
    let ret = "CPF: ";
    if (this.prePedidoDto.DadosCliente.Tipo == new Constantes().ID_PJ) {
      ret = "CNPJ: ";
    }
    //fica melhor sem nada na frente:
    ret = "";
    return ret + CpfCnpjUtils.cnpj_cpf_formata(this.prePedidoDto.DadosCliente.Cnpj_Cpf);
  }

  permite_RA_Status = false;
  inscreverPermite_RA_Status() {
    this.prepedidoBuscarService.Obter_Permite_RA_Status().subscribe({
      next: (r) => {
        if (r != 0) {
          this.permite_RA_Status = true;
          this.novoPrepedidoDadosService.prePedidoDto.PermiteRAStatus = 1;
        }
      },
      error: (r) => this.alertaService.mostrarErroInternet(r)
    });
  }



  carregandoProds = true;
  produtoComboDto: ProdutoComboDto;
  inscreverProdutoComboDto() {
    this.produtoService.listarProdutosCombo(this.prePedidoDto.DadosCliente.Loja, this.prePedidoDto.DadosCliente.Id).subscribe({
      next: (r: ProdutoComboDto) => {
        if (!!r) {
          this.produtoComboDto = r;
          this.produtoComboDto.ProdutoDto = this.produtoComboDto.ProdutoDto.filter(el => el.Preco_lista && el.Preco_lista != 0);
          this.carregandoProds = false;
          if (this.clicouAddProd)
            this.adicionarProduto();
        } else {
          this.alertaService.mostrarMensagem("Erro ao acessar a lista de produtos: nenhum produto retornado. Por favor, entre em contato com o suporte técnico.")
        }
      },
      error: (r: ProdutoComboDto) => this.alertaService.mostrarErroInternet(r)
    });
  }

  //mensagens de estoque
  estoqueItem(i: PrepedidoProdutoDtoPrepedido): ProdutoDto {
    if (!this.produtoComboDto) {
      return null;
    }

    //procuramos esse item
    const item = this.produtoComboDto.ProdutoDto.filter(el => el.Fabricante === i.Fabricante && el.Produto === i.NumProduto);
    if (!item || item.length <= 0) {
      return null;
    }
    //achamos o item
    return item[0];
  }
  estoqueExcedido(i: PrepedidoProdutoDtoPrepedido): boolean {
    const item = this.estoqueItem(i);
    //se nao achamos, dizemos que não tem que mostrar a mensagem não...
    if (!item) {
      return false;
    }
    if (item.Estoque < i.Qtde) {
      return true;
    }
    return false;
  }
  estoqueExistente(i: PrepedidoProdutoDtoPrepedido): number {
    //para imprimir quantos itens tem em estoque
    const item = this.estoqueItem(i);

    if (!item) {
      return null;
    }
    return item.Estoque;
  }

  public msgQtdePermitida: string = "";
  qtdeVendaPermitida(i: PrepedidoProdutoDtoPrepedido): boolean {
    //busca o item na lista
    this.msgQtdePermitida = "";

    const item = this.estoqueItem(i);
    if (!item) {
      return false;
    }

    if (i.Qtde > item.Qtde_Max_Venda) {
      this.msgQtdePermitida = "Quantidade solicitada excede a quantidade máxima de venda permitida!";
      return true;
    }
    else
      return false;

  }

  produtoTemAviso(i: PrepedidoProdutoDtoPrepedido): boolean {
    const item = this.estoqueItem(i);
    //se nao achamos, dizemos que não tem que mostrar a mensagem não...
    if (!item) {
      return false;
    }
    if (!item.Alertas || item.Alertas.trim() === "") {
      return false;
    }
    return true;
  }
  produtoMsgAviso(i: PrepedidoProdutoDtoPrepedido): string {
    const item = this.estoqueItem(i);
    if (!item) {
      return "";
    }
    return item.Alertas;
  }

  totalPedido(): number {
    return this.prePedidoDto.ListaProdutos.reduce((sum, current) => sum + current.TotalItem, 0);
  }

  totalPedidoRA(): number {
    return this.prePedidoDto.ListaProdutos.reduce((sum, current) => sum + current.Qtde * current.Preco_Lista, 0);
  }
  //#endregion

  //componente de forma de pagamento, precisa do static false
  @ViewChild("dadosPagto", { static: false }) dadosPagto: DadosPagtoComponent;

  //#region digitacao de numeros

  /*
  digitouQte: atualiza o vl total a partir do vl venda
  digitouPreco: atualiza o vl venda a partir do preco e do desc, depois digitouQte
  digitouDesc: atualiza o vl venda a partir do preco e do desc, depois digitouQte
  digitouVlVenda: atualiza o desc a partir do preco e do VlVenda, depois digitouQte
  */

  digitouQte(i: PrepedidoProdutoDtoPrepedido) {
    //necessário trazer e verificar a variavel "qtde_max_permitida" na tabela "T_produto_loja" 
    //para limitar a qtde de compra para o usuário

    if (i.Qtde <= 0) {
      i.Qtde = 1;
    }
    i.TotalItem = i.VlUnitario * i.Qtde; // VlUnitario = Vl Venda na tela
    this.dadosPagto.prepedidoAlterado();

  }
  digitouPreco(e: Event, i: PrepedidoProdutoDtoPrepedido) {
    let valor = ((e.target) as HTMLInputElement).value;
    let v: any = valor.replace(/\D/g, '');
    v = (v / 100).toFixed(2) + '';

    //se não alteraram nada, ignoramos
    if (i.Preco === Number.parseFloat(v))
      return;

    i.Preco = Number.parseFloat(v);
    if (i.Desconto) {
      i.VlUnitario = i.Preco * (1 - i.Desconto / 100);
      i.VlUnitario = Number.parseFloat(i.VlUnitario.toFixed(2));
    }
    else {
      i.VlUnitario = i.Preco;
    }

    this.digitouQte(i);
  }

  digitouPreco_Lista(e: Event, i: PrepedidoProdutoDtoPrepedido) {
    debugger;
    let valor = ((e.target) as HTMLInputElement).value;
    let v: any = valor.replace(/\D/g, '');
    v = (v / 100).toFixed(2) + '';


    if (Number.parseFloat(i.VlLista.toFixed(2)) === Number.parseFloat(v)) {
      i.AlterouValorRa = false;
    }
    else {
      i.AlterouValorRa = true;
    }

    i.Preco_Lista = Number.parseFloat(v);

    this.somarRA();



    this.digitouQte(i);
  }

  formatarPreco_Lista(e: Event, i: PrepedidoProdutoDtoPrepedido) {
    let valor = ((e.target) as HTMLInputElement).value;
    let v: any = valor.replace(/\D/g, '');
    v = (v / 100).toFixed(2) + '';

    i.Preco_Lista = v;
  }

  somaRA: number;
  somarRA(): number {
    debugger;
    let total = this.totalPedido();
    let totalRa = this.totalPedidoRA();
    return this.somaRA = totalRa - total;
  }


  formataVlVenda(e: Event, i: PrepedidoProdutoDtoPrepedido) {
    let valor = ((e.target) as HTMLInputElement).value;
    let v: any = valor.replace(/\D/g, '');
    v = (v / 100).toFixed(2) + '';

    i.VlUnitario = v;
  }

  digitouVlVenda(e: Event, i: PrepedidoProdutoDtoPrepedido) {
    debugger;
    let valor = ((e.target) as HTMLInputElement).value;
    let v: any = valor.replace(/\D/g, '');
    v = (v / 100).toFixed(2) + '';
    // //se não alteraram nada, ignoramos
    // if (i.VlUnitario.toFixed(2) === v)
    //   return;

    //Gabriel apenas teste, pois os valores estão se acumulando e iremos alterar aqui
    i.TotalItem = i.Qtde * i.VlLista;
    i.VlTotalItem = i.Qtde * i.VlLista;
    debugger;
    // i.VlUnitario = Number.parseFloat(v);
    //teste de calculo
    i.Desconto = 100 * (i.VlLista - v) / i.VlLista;
    //calcula o desconto
    // i.Desconto = 100 * (i.Preco - i.VlUnitario) / i.Preco;
    i.Desconto = Number.parseFloat(i.Desconto.toFixed(2));

    if (i.VlLista == i.VlUnitario) {
      i.AlterouVlVenda = false;
    } else {
      i.AlterouVlVenda = true;
    }

    // i.AlterouVlVenda = true;

    this.digitouQte(i);
  }
  digitouDesc(e: Event, i: PrepedidoProdutoDtoPrepedido) {
    let valor = ((e.target) as HTMLInputElement).value;
    let v: any = valor.replace(/\D/g, '');
    //tem 1 casa
    v = (v / 100).toFixed(2) + '';

    debugger;
    //se o desconto for digitado estamos alterando o valor de venda e não devemos mais alterar esse valor
    if (i.Desconto == 0 || i.Desconto.toString() == '') {
      i.AlterouVlVenda = false;
    } else {
      i.AlterouVlVenda = true;
    }

    this.digitouDescValor(i, v);
  }

  formatarDesc(e: Event, i: PrepedidoProdutoDtoPrepedido) {
    debugger;
    let valor = ((e.target) as HTMLInputElement).value;
    let v: any = valor.replace(/\D/g, '');
    v = (v / 100).toFixed(2) + '';

    i.Desconto = v;
  }

  digitouDescValor(i: PrepedidoProdutoDtoPrepedido, v: string) {
    debugger;
    //se não alteraram nada, ignoramos
    if (i.Desconto === Number.parseFloat(v))
      return;

    i.Desconto = Number.parseFloat(v);
    //não deixa números negativos e nem maior que 100
    /*
    //pensando bem, deixa negativos sim!
    é que parece que tem caso na base com desconto negativo...
    if (i.Desconto <= 0) {
      i.Desconto = 0;
    }
    */

    if (i.Desconto > 100) {
      i.Desconto = 100;
    }

    if (i.Desconto) {
      i.VlUnitario = i.VlLista * (1 - i.Desconto / 100);
      i.VlUnitario = Number.parseFloat(i.VlUnitario.toFixed(2));
    }
    else {
      i.VlUnitario = i.VlLista;
    }
    this.digitouQte(i);
  }
  //#endregion

  //#region navegação

  percentualVlPedidoRA: number;
  obtemPercentualVlPedidoRA() {
    this.prepedidoBuscarService.ObtemPercentualVlPedidoRA().subscribe({
      next: (r: number) => {
        if (!!r) {
          debugger;
          this.percentualVlPedidoRA = r;
        }
      },
      error: (r: number) => this.alertaService.mostrarErroInternet(r)
    });
  }

  voltar() {
    // if(!this.dadosPagto.podeContinuar())
    // return false;
    this.dadosPagto.podeContinuar(false);
    // this.dadosPagto.atribuirFormaPagtoParaDto();
    this.location.back();
  }
  continuar() {

    //verificar se tem produtos com qtde maior que o permitido
    let q: number = 0;
    this.prePedidoDto.ListaProdutos.forEach(r => {
      if (this.qtdeVendaPermitida(r)) {
        q++;
      }
    });
    if (q > 0) {
      this.alertaService.mostrarMensagem("Produtos com quantidades maiores que a quantidade máxima permitida para venda!");
      return;
    }
    //validação: tem que ter algum produto
    if (this.prePedidoDto.ListaProdutos.length === 0) {
      this.alertaService.mostrarMensagem("Selecione ao menos um produto para continuar.");
      return;
    }

    debugger;
    if (this.prePedidoDto.PermiteRAStatus == 1) {
      if (this.percentualVlPedidoRA != 0 && this.percentualVlPedidoRA != undefined) {

        let vlAux = (this.percentualVlPedidoRA / 100) * this.totalPedido();
        if (this.somaRA > vlAux) {
          this.alertaService.mostrarMensagem("O valor total de RA excede o limite permitido!");
          return;
        }
      }
    }

    //verifica se a forma de pgamento tem algum aviso
    if (!this.dadosPagto.podeContinuar(true)) {
      return;
    }

    let numeroPrepedido = this.activatedRoute.snapshot.params.numeroPrepedido;
    if (!!numeroPrepedido) {
      this.router.navigate(["../../observacoes"], { relativeTo: this.activatedRoute });
    }
    else {
      this.router.navigate(["../observacoes"], { relativeTo: this.activatedRoute });
    }
  }

  adicionarProduto() {
    //ele mesmo já adiciona
    this.clicouAddProd = true;
    this.mostrarProdutos(null);
  }

  removerLinha(i: PrepedidoProdutoDtoPrepedido) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: `Remover este item do pré-pedido?`
    });

    debugger;

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.prePedidoDto.ListaProdutos = this.prePedidoDto.ListaProdutos.filter(function (value, index, arr) {
          return value !== i;
        });
        //Gabriel
        this.dadosPagto.prepedidoAlterado();
      }
    });
  }


  public clicouAddProd: boolean = false;

  verificarCargaProdutos(): boolean {
    if (this.carregandoProds) {
      //ainda não carregou, vamos esperar....
      //this.alertaService.mostrarMensagem("Lista de produtos ainda sendo carregada. Por favor, tente novamente em alguns instantes.");
      return false;
    }
    return true;
  }

  //Gabriel
  //criaremos uma lista para armazenar os itens pelo item principal, independente se é produto composto
  public lstProdutoAdd: PrepedidoProdutoDtoPrepedido[] = [];

  mostrarProdutos(linha: PrepedidoProdutoDtoPrepedido) {

    if (!this.verificarCargaProdutos()) {
      return;
    }
    const selecProdInfo = new SelecProdInfo();
    selecProdInfo.produtoComboDto = this.produtoComboDto;
    selecProdInfo.ClicouOk = false;
    if (linha) {
      selecProdInfo.Produto = linha.NumProduto;
      selecProdInfo.Fabricante = linha.Fabricante;
      selecProdInfo.Qte = linha.Qtde;
    }
    let options: any = {
      autoFocus: false,
      width: "55vw",
      //não colocamos aqui para que ele ajuste melhor: height:"85vh",      
      data: selecProdInfo
    };
    if (!this.telaDesktop) {
      //opções para celular
      options = {
        autoFocus: false,
        width: "100vw", //para celular, precisamos da largura toda
        maxWidth: "100vw",
        //não colocamos aqui para que ele ajuste melhor: height:"85vh",
        data: selecProdInfo
      };
    }


    const dialogRef = this.dialog.open(SelecProdDialogComponent, options);
    dialogRef.afterClosed().subscribe((result) => {
      if (result && selecProdInfo.ClicouOk) {
        //vamos editar ou adicionar um novo
        if (linha) {
          //editando
          //se mudou o produto, temos que mdar vários campos
          if (linha.NumProduto !== selecProdInfo.Produto || linha.Fabricante !== selecProdInfo.Fabricante) {
            //mudou o produto, temos que mudar muita coisa!
            const filhosDiretos = this.filhosDeProdutoComposto(selecProdInfo);
            if (!filhosDiretos) {
              //não é produto composto
              this.atualizarProduto(linha, selecProdInfo.Fabricante, selecProdInfo.Produto, selecProdInfo.Qte);
            }
            else {
              //produto composto
              //removemos o item atual e colocamostodos os novos
              this.prePedidoDto.ListaProdutos = this.prePedidoDto.ListaProdutos.filter(el => el != linha);

              //colcoamos todos os novos
              for (let i = 0; i < filhosDiretos.length; i++) {
                let novo = new PrepedidoProdutoDtoPrepedido();
                this.prePedidoDto.ListaProdutos.push(novo);
                this.atualizarProduto(novo, filhosDiretos[i].Fabricante, filhosDiretos[i].Produto, selecProdInfo.Qte * filhosDiretos[i].Qtde);
              }

            }
          }
          else {
            //o produto ficou o mesmo, só atualizamos, menos bagunça
            this.atualizarProduto(linha, selecProdInfo.Fabricante, selecProdInfo.Produto, selecProdInfo.Qte);
          }
        }
        else {
          let filhosDiretosNovo = this.filhosDeProdutoComposto(selecProdInfo);
          debugger;

          let itemrepetido = new Array();
          if (filhosDiretosNovo != null) {
            //pegamos 2 item se for repetido
            this.prePedidoDto.ListaProdutos.forEach(x => {
              filhosDiretosNovo.forEach(y => {
                if (y.Produto == x.NumProduto)
                  itemrepetido.push(y);
              })
            });
            //se a lista de produtos estiver com 11 itens, não iremos add um produto composto
            //mostraremos uma msg que este item é composto por 2 ou mais itens
            if (this.prePedidoDto.ListaProdutos.length == 11 && itemrepetido.length == 0) {
              this.alertaService.mostrarMensagem("É permitido apenas 12 itens por Pré-Pedido!\n" +
                "O produto " + selecProdInfo.Produto + " é composto por " + filhosDiretosNovo.length + " itens!");
              return false;
            }
          }
          else {
            //pegamos 1 item se for repetido
            itemrepetido = this.prePedidoDto.ListaProdutos.filter(y => y.NumProduto == selecProdInfo.Produto);
          }
          if (this.prePedidoDto.ListaProdutos.length >= 12 && itemrepetido.length == 0) {
            this.alertaService.mostrarMensagem("É permitido apenas 12 itens por Pré-Pedido! teste");
            return false;
          }
          else {
            if (!filhosDiretosNovo) {
              //não é produto composto
              let novo = new PrepedidoProdutoDtoPrepedido();
              this.prePedidoDto.ListaProdutos.push(novo);
              this.atualizarProduto(novo, selecProdInfo.Fabricante, selecProdInfo.Produto, selecProdInfo.Qte);

              //vamos arrumar eventuais produtos repetidos
              this.arrumarProdsRepetidos();
            }
            else {
              if ((this.prePedidoDto.ListaProdutos.length + filhosDiretosNovo.length - itemrepetido.length) <= 12) {
                //produto composto
                for (let i = 0; i < filhosDiretosNovo.length; i++) {
                  if (this.prePedidoDto.ListaProdutos.length < 12 || itemrepetido.length >= 1) {
                    /**
                     * qtos itens tem na tla + qto itens vai entrar na tela e qtos itens repedidos
                     */
                    // if (itemrepetido[i].Produto == filhosDiretosNovo[i].Produto) {
                    let novo = new PrepedidoProdutoDtoPrepedido();
                    this.prePedidoDto.ListaProdutos.push(novo);
                    this.atualizarProduto(novo, filhosDiretosNovo[i].Fabricante, filhosDiretosNovo[i].Produto,
                      selecProdInfo.Qte * filhosDiretosNovo[i].Qtde);

                    //vamos arrumar eventuais produtos repetidos
                    this.arrumarProdsRepetidos();
                    // }
                  }

                  // }
                }
              }
              else {
                this.alertaService.mostrarMensagem("É permitido apenas 12 itens por Pré-Pedido!");
                return false;
              }
            }
          }
        }
      }
    });
  }

  //depois de selecionar o produto, atualiza todos os campos
  atualizarProduto(linha: PrepedidoProdutoDtoPrepedido, fabricante: string, produto: string, qtde: number) {
    let prodInfo = this.produtoComboDto.ProdutoDto.filter(el => el.Fabricante === fabricante && el.Produto === produto)[0];

    if (!prodInfo) {
      prodInfo = new ProdutoDto();
    }
    linha.Fabricante = fabricante;
    linha.NumProduto = produto;
    linha.Descricao = prodInfo.Descricao_html;
    //Obs: string;
    linha.Qtde = qtde;
    //Permite_Ra_Status: number;
    //BlnTemRa: boolean;
    linha.Preco = prodInfo.Preco_lista;
    linha.VlLista = prodInfo.Preco_lista;
    linha.VlUnitario = prodInfo.Preco_lista;
    linha.Preco_Lista = prodInfo.Preco_lista;

    if (!linha.Desconto) {
      linha.Desconto = 0;
    }
    this.digitouDescValor(linha, linha.Desconto.toString());
    this.digitouQte(linha);
  }

  filhosDeProdutoComposto(selecProdInfo: SelecProdInfo) {

    const registros = this.produtoComboDto.ProdutoCompostoDto.filter(el => el.PaiFabricante === selecProdInfo.Fabricante && el.PaiProduto === selecProdInfo.Produto);

    if (!registros) {
      return null;
    }
    if (registros.length <= 0) {
      return null;
    }
    return registros[0].Filhos;
  }


  buscarPaideFilho(item: string) {
    debugger;
    let pai = "";
    let produtosFilhos = new Array();
    const registro = this.produtoComboDto.ProdutoCompostoDto.filter(x => x.Filhos.filter(y => y.Produto == item));
    if (!registro) {
      registro.forEach(x => {
        x.Filhos.forEach(y => {
          if (y.Produto == item) {
            pai = x.PaiProduto;
          }
        })
      })
    }

    return pai;
  }


  //consolidamos produtos repetidos
  arrumarProdsRepetidos() {
    let lp = this.prePedidoDto.ListaProdutos;
    for (let i = 0; i < lp.length; i++) {
      let este = lp[i];

      //se tiver algum repetido, tiramos o proximo repetido
      let continaurBuscaRepetido = true;
      while (continaurBuscaRepetido) {
        continaurBuscaRepetido = false;
        for (let irepetido = i + 1; irepetido < lp.length; irepetido++) {
          let repetido = lp[irepetido];
          if (este.Fabricante === repetido.Fabricante && este.NumProduto == repetido.NumProduto) {
            //repetido, tem que tirar este!
            continaurBuscaRepetido = true;
            este.Qtde += repetido.Qtde;
            this.prePedidoDto.ListaProdutos = this.prePedidoDto.ListaProdutos.filter(el => el !== repetido);
            lp = this.prePedidoDto.ListaProdutos;
            this.digitouQte(este);
          }
        }
      }
    }
  }
  //#endregion

}
