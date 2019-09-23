import { Component, OnInit, Inject, ElementRef, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, PageEvent } from '@angular/material';
import { SelecProdInfo } from './selec-prod-info';
import { ProdutoComboDto } from 'src/app/servicos/produto/produtodto';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { ProdutoTela } from './produto-tela';
import { MoedaUtils } from 'src/app/utils/moedaUtils';

@Component({
  selector: 'app-selec-prod-dialog',
  templateUrl: './selec-prod-dialog.component.html',
  styleUrls: ['./selec-prod-dialog.component.scss']
})
export class SelecProdDialogComponent implements OnInit {

  constructor(
    public readonly dialogRef: MatDialogRef<SelecProdDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public selecProdInfoPassado: SelecProdInfo,
    public readonly alertaService: AlertaService) { }

  //#region carga
  //dados vazios, para mostrar enquanto constroi a lista
  public prods: ProdutoTela[] = new Array();
  //se estmoas carregando ainda
  public carregando = true;
  private maisDadosProd() {
    //transfere alguns dados por vez
    for (let copiar = 0; copiar < 30; copiar++) {
      //acabou?
      if (!(this.prods.length < this.selecProdInfoPassado.produtoComboDto.ProdutoDto.length))
        break;
      //colocamos mais um
      this.prods.push(new ProdutoTela(this.selecProdInfoPassado.produtoComboDto.ProdutoDto[this.prods.length]));
    }

    ProdutoTela.AtualizarVisiveis(this.prods, this.digitado);
    //precisa mais vezes para transferir?
    if (this.prods.length < this.selecProdInfoPassado.produtoComboDto.ProdutoDto.length) {
      //proximo bloco
      setTimeout(() => this.maisDadosProd(), 1);
    }
    else {
      //já acabou de carregar
      this.carregando = false;
    }
  }
  ngOnInit() {
    //transfere só os X primeiros, para a tela carregar mais rápido
    //sem isto, ela já mostra assim que carregar, mas fica travada enquanto é montada. 
    this.carregando = true;
    this.produto = ProdutoTela.FabrProd(this.selecProdInfoPassado.Fabricante, this.selecProdInfoPassado.Produto);
    this.qdte = this.selecProdInfoPassado.Qte;
    if (this.selecProdInfoPassado.Produto) {
      this.digitado = this.selecProdInfoPassado.Produto;
    }

    this.maisDadosProd();
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
    ProdutoTela.AtualizarVisiveis(this.prods, this.digitado);
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

  public moedaUtils= new MoedaUtils();

  //a quantidade
  public qdte: number;
  //o produto
  public produto: string;
  //para formatar o produto e o fabricante
  ProdutoTelaFabrProd = ProdutoTela.FabrProd;

}
