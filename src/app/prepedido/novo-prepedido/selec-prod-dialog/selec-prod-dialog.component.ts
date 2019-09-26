import { Component, OnInit, Inject, ElementRef, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, PageEvent } from '@angular/material';
import { SelecProdInfo } from './selec-prod-info';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { ProdutoTela } from './produto-tela';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { supportsPassiveEventListeners } from '@angular/cdk/platform';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';

@Component({
  selector: 'app-selec-prod-dialog',
  templateUrl: './selec-prod-dialog.component.html',
  styleUrls: ['./selec-prod-dialog.component.scss']
})
export class SelecProdDialogComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(
    public readonly dialogRef: MatDialogRef<SelecProdDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public selecProdInfoPassado: SelecProdInfo,
    telaDesktopService: TelaDesktopService,
    public readonly alertaService: AlertaService) {
    super(telaDesktopService);

    //no desktop deixamos um limite inicial maior
    this.limiteInicial = 20;
    if (this.telaDesktop) {
      this.limiteInicial = 80;
    }
    this.limiteTela = this.limiteInicial;

  }

  //#region carga
  public prodsArray: ProdutoTela[] = new Array();
  //dados vazios, para mostrar enquanto constroi a lista
  public prodsTela: ProdutoTela[] = new Array();

  /*
  a tela com 600 produtos ficava muito lenta
  então temos o seguinte fucnionamento: ao carregar, mostra os primerios X registros
  depois, se clicar no botão para mostrar tudo, aí carrega todos, e demora o que demorar...
  */

  //maximo que mostramos na tela
  public limiteMaximo = 1000 * 1000; //1 mega registros...
  //quantos mostramos na tela inicialmente
  public limiteInicial = 50;
  public limiteTela = this.limiteInicial;

  //se estamos carregando a lista completa de produtos
  //mas ainda não terminou de mostrar
  //aí o botão muda, ao inves de ser Mostrar todo, fica Carregando
  public limiteMudando = false;

  public limiteZerar() {
    //mostramos sem limites
    this.limiteTela = this.limiteMaximo;
    this.limiteMudando = true;
    setTimeout(() => {
      this.atualizarProdsTela();
      this.limiteMudando = false;
    }, 0);
  }

  private transferirDadosArray() {
    //sem limite!
    const limite = this.limiteMaximo;
    for (let copiar = 0; copiar < limite; copiar++) {
      //acabou?
      if (!(this.prodsArray.length < this.selecProdInfoPassado.produtoComboDto.ProdutoDto.length))
        break;
      //colocamos mais um
      this.prodsArray.push(new ProdutoTela(this.selecProdInfoPassado.produtoComboDto.ProdutoDto[this.prodsArray.length], this.selecProdInfoPassado.produtoComboDto.ProdutoCompostoDto));
    }

    ProdutoTela.AtualizarVisiveis(this.prodsArray, this.digitado);
    this.atualizarProdsTela();
  }

  public atualizarProdsTela() {
    this.prodsTela = this.prodsArray.filter(el => el.visivel).slice(0, this.limiteTela);
  }

  ngOnInit() {
    this.produto = ProdutoTela.FabrProd(this.selecProdInfoPassado.Fabricante, this.selecProdInfoPassado.Produto);
    this.qdte = this.selecProdInfoPassado.Qte;
    if (this.selecProdInfoPassado.Produto) {
      this.digitado = this.selecProdInfoPassado.Produto;
    }

    this.transferirDadosArray();
  }
  //#endregion


  //precisamos disto para acertar o foco
  //somente o autofocus do html não funciona quando carrega a ciaxa de diálogo pela segunda vez
  @ViewChild("digitadocx", { static: true }) digitadoCx: ElementRef;
  ngAfterViewInit() {
    setTimeout(() => {
      //TEM QUE SER por timeout para evitar o erro
      //ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked. Previous value: 'mat-form-field-should-float: false'. Current value: 'mat-form-field-should-float: true'.
      this.digitadoCx.nativeElement.focus();
    }, 1);
  }

  //alteraram o produto
  digitado = "";
  digitouProd(e: Event) {
    this.digitado = ((e.target) as HTMLInputElement).value;
    ProdutoTela.AtualizarVisiveis(this.prodsArray, this.digitado);
    this.atualizarProdsTela();
  }

  onNoClick(): void {
    this.selecProdInfoPassado.ClicouOk = false;
    this.dialogRef.close(false);
  }
  onAdicionarClick(): void {
    if (!this.produto || this.produto === "") {
      this.alertaService.mostrarMensagem("Por favor, selecione um produto.");
      return;
    }
    if (!this.qdte || this.qdte <= 0) {
      this.alertaService.mostrarMensagem("Por favor, selecione uma quantidade.");
      return;
    }

    //separado por /
    //depende do ProdutoTela.FabrProd
    this.selecProdInfoPassado.Fabricante = this.produto.split("/")[0];
    this.selecProdInfoPassado.Produto = this.produto.split("/")[1];

    this.selecProdInfoPassado.Qte = this.qdte;
    this.selecProdInfoPassado.ClicouOk = true;
    this.dialogRef.close(true);
  }

  public produtoDescr(fabricante: string, produto: string) {
    return this.selecProdInfoPassado.produtoComboDto.ProdutoDto.filter(el => el.Fabricante == fabricante && el.Produto == produto)[0];
  }

  public moedaUtils = new MoedaUtils();

  //a quantidade
  public qdte: number;
  //o produto
  public produto: string;
  //para formatar o produto e o fabricante
  public ProdutoTelaFabrProd = ProdutoTela.FabrProd;

}
