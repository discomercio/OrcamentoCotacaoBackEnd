import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { NovoPrepedidoDadosService } from '../novo-prepedido-dados.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { MatDialog } from '@angular/material';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { Router, ActivatedRoute } from '@angular/router';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { PrepedidoBuscarService } from 'src/app/servicos/prepedido/prepedido-buscar.service';

@Component({
  selector: 'app-confirmar-prepedido',
  templateUrl: './confirmar-prepedido.component.html',
  styleUrls: ['./confirmar-prepedido.component.scss']
})
export class ConfirmarPrepedidoComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly location: Location,
    private readonly router: Router,
    private readonly novoPrepedidoDadosService: NovoPrepedidoDadosService,
    public readonly alertaService: AlertaService,
    public readonly dialog: MatDialog,
    public readonly prepedidoBuscarService: PrepedidoBuscarService,
    telaDesktopService: TelaDesktopService
  ) {
    super(telaDesktopService);
  }

  ngOnInit() {
    //pegamos o que está no serviço
    this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
    if (!this.prePedidoDto) {
      this.router.navigate(["/novo-prepedido"]);
      return;
    }
    this.criando = !this.prePedidoDto.NumeroPrePedido;
  }
  //#region dados
  //dados sendo criados
  criando = true;
  prePedidoDto: PrePedidoDto;
  //#endregion

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
  totalPedido(): number {
    return this.prePedidoDto.ListaProdutos.reduce((sum, current) => sum + current.TotalItem, 0);
  }

  //#endregion

  //#region navegação
  voltar() {
    this.location.back();
  }
  continuar() {
    this.prepedidoBuscarService.cadastrarPrepedido(this.prePedidoDto).subscribe(
      {
        next: (r) => {
          if (r.length == 0) {
            this.alertaService.mostrarMensagem("Pré-pedido criado com sucesso.");
            //afazer: mostrar  o número do pré-epdido criado
            this.router.navigate(["/"]);
          }
          else {
            //deu erro!
            this.alertaService.mostrarMensagem("Erros ao criar o pré-pedido. \nLista de erros: \n" + r.join("\n"));
          }
        },
        error: (err) => {
          this.alertaService.mostrarErroInternet();
        }
      }
    );
  }
  //#endregion


}
