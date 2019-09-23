import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { DetalhesDtoPrepedido } from 'src/app/dto/Prepedido/DetalhesPrepedido/DetalhesDtoPrepedido';
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
import { ProdutoComboDto } from 'src/app/servicos/produto/produtodto';
import { ProdutoService } from 'src/app/servicos/produto/produto.service';
import { MatDialog } from '@angular/material';
import { SelecProdDialogComponent } from '../selec-prod-dialog/selec-prod-dialog.component';
import { SelecProdInfo } from '../selec-prod-dialog/selec-prod-info';

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
              this.alertaService.mostrarErroInternet();
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
          error: (r) => this.alertaService.mostrarErroInternet()
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
        }
      },
      error: (r) => this.alertaService.mostrarErroInternet()
    });
  }

  carregandoProds = true;
  produtoComboDto: ProdutoComboDto;
  inscreverProdutoComboDto() {
    this.produtoService.listarProdutosCombo(this.prePedidoDto.DadosCliente.Id).subscribe({
      next: (r: ProdutoComboDto) => {
        if (!!r) {
          this.produtoComboDto = r;
          this.carregandoProds = false;
        }
      },
      error: (r: ProdutoComboDto) => this.alertaService.mostrarErroInternet()
    });
  }

  //#endregion

  //#region digitacao de numeros

  /*
  digitouQte: atualiza o vl total a partir do vl venda
  digitouPreco: atualiza o vl venda a partir do preco e do desc, depois digitouQte
  digitouDesc: atualiza o vl venda a partir do preco e do desc, depois digitouQte
  digitouVlVenda: atualiza o desc a partir do preco e do VlVenda, depois digitouQte
  */

  digitouQte(i: PrepedidoProdutoDtoPrepedido) {
    //não deixa números negativos
    if (i.Qtde <= 0) {
      i.Qtde = 1;
    }
    i.TotalItem = i.VlUnitario * i.Qtde; // VlUnitario = Vl Venda na tela
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
    }
    else {
      i.VlUnitario = i.Preco;
    }

    this.digitouQte(i);
  }
  digitouVlVenda(e: Event, i: PrepedidoProdutoDtoPrepedido) {
    let valor = ((e.target) as HTMLInputElement).value;
    let v: any = valor.replace(/\D/g, '');
    v = (v / 100).toFixed(2) + '';

    //se não alteraram nada, ignoramos
    if (i.VlUnitario === Number.parseFloat(v))
      return;

    i.VlUnitario = Number.parseFloat(v);

    //calcula o desconto
    i.Desconto = 100 * (i.Preco - i.VlUnitario) / i.Preco;
    this.digitouQte(i);
  }
  digitouDesc(e: Event, i: PrepedidoProdutoDtoPrepedido) {
    let valor = ((e.target) as HTMLInputElement).value;
    let v: any = valor.replace(/\D/g, '');
    //tem 1 casa
    v = (v / 10).toFixed(2) + '';

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
      i.VlUnitario = i.Preco * (1 - i.Desconto / 100);
    }
    else {
      i.VlUnitario = i.Preco;
    }
    this.digitouQte(i);
  }
  //#endregion

  //#region navegação
  voltar() {
    this.location.back();
  }
  continuar() {
    let msg = "teste ";
    for (let i = 0; i < 1000; i++)
      msg += "teste ";
    this.alertaService.mostrarMensagemComLargura(msg, "100vw");
    window.alert("Afazer: salvar o pedido");
  }
  adicionarProduto() {
    //ele mesmo já adiciona
    this.mostrarProdutos(null);
  }
  removerLinha(i: PrepedidoProdutoDtoPrepedido) {
    this.prePedidoDto.ListaProdutos = this.prePedidoDto.ListaProdutos.filter(function (value, index, arr) {
      return value !== i;
    });
  }
  verificarCargaProdutos(): boolean {
    if (this.carregandoProds) {
      //ainda não carregou, vamos esperar....
      this.alertaService.mostrarMensagem("Lista de produtos ainda sendo carregada. Por favor, tente novamente em alguns instantes.");
      return false;
    }
    return true;
  }
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
    const dialogRef = this.dialog.open(SelecProdDialogComponent, {
      autoFocus: false,
      width: "100em",
      //não colocamos aqui para que ele ajuste melhor: height:"85vh",
      data: selecProdInfo
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result && selecProdInfo.ClicouOk) {
        //vamos editar ou adicionar um novo
        if (linha) {
          //editando

          //se mudou o produto, temos que mdar vários campos
          if(linha.NumProduto !== selecProdInfo.Produto || linha.Fabricante !== selecProdInfo.Fabricante){
            window.alert("Afazer: mudar o produto");
          }
          
          linha.Qtde = selecProdInfo.Qte;
          this.digitouQte(linha);
        }
        else {
          //adicionando
          window.alert("Afazer: criar o produto");
          let novo = new PrepedidoProdutoDtoPrepedido();
          this.prePedidoDto.ListaProdutos.push(novo);
        }
      }
    });



  }
  //#endregion

}
