import { Component, OnInit, ElementRef, ViewChild, AfterContentInit, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { BuscarClienteService } from 'src/app/servicos/cliente/buscar-cliente.service';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { Location } from '@angular/common';
import { AlertDialogComponent } from 'src/app/utils/alert-dialog/alert-dialog.component';
import { MatDialog } from '@angular/material';
import { Constantes } from 'src/app/dto/Constantes';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';
import { ClienteCadastroUtils } from 'src/app/dto/ClienteCadastroUtils/ClienteCadastroUtils';
import { Options } from 'selenium-webdriver';

@Component({
  selector: 'app-confirmar-cliente',
  templateUrl: './confirmar-cliente.component.html',
  styleUrls: ['./confirmar-cliente.component.scss']
})
export class ConfirmarClienteComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(router: Router,
    activatedRoute: ActivatedRoute,
    telaDesktopService: TelaDesktopService,
    private readonly location: Location,
    public readonly dialog: MatDialog,
    private readonly buscarClienteService: BuscarClienteService) {
    super(telaDesktopService);

    this.dadosClienteCadastroDto = null;
    let clienteCadastroDto: ClienteCadastroDto = (router.getCurrentNavigation().extras.state) as ClienteCadastroDto;
    if (clienteCadastroDto && clienteCadastroDto.DadosCliente) {
      this.dadosClienteCadastroDto = clienteCadastroDto.DadosCliente;
    }
    //se chegar como null é pq foi salvo como link; n~]ao temos dados para mostrar
    if (!this.dadosClienteCadastroDto) {

      //voltamos para a tela anterior
      //router.navigate(["/novo-prepedido"]);

      //ou melhor, fazemos a busca de novo!
      const clienteBusca = activatedRoute.snapshot.params.cpfCnpj;

      buscarClienteService.buscar(clienteBusca).toPromise()
        .then((r) => {
          if (r === null) {
            //erro, voltamos para a tela anterior
            router.navigate(["/novo-prepedido"]);
            return;
          }
          //cliente já existe
          this.dadosClienteCadastroDto = r.DadosCliente;
          this.salvarAtivoInicializar();
        }).catch((r) => {
          //erro, voltamos para a tela anterior
          router.navigate(["/novo-prepedido"]);
        });
    }
  }

  dadosClienteCadastroDto = new DadosClienteCadastroDto();
  ngOnInit() {
  }

  voltar() {
    this.location.back();
  }

  //variáveis apra controlar salvarAtivo
  salvarAtivoInicializar() {
    this.dadosClienteCadastroDtoIe = this.dadosClienteCadastroDto.Ie;
    this.dadosClienteCadastroDtoProdutorRural = this.dadosClienteCadastroDto.ProdutorRural;
    this.dadosClienteCadastroDtoContribuinte_Icms_Status = this.dadosClienteCadastroDto.Contribuinte_Icms_Status;
  }
  private dadosClienteCadastroDtoIe: string;
  private dadosClienteCadastroDtoProdutorRural: number;
  private dadosClienteCadastroDtoContribuinte_Icms_Status: number;
  salvarAtivo(): boolean {
    //diz se o botão de salvar está ligado
    if (!this.dadosClienteCadastroDto) {
      return false;
    }
    //se estiver com NULL é pq ainda não pegou os valores
    if(!this.dadosClienteCadastroDtoIe){
      this.salvarAtivoInicializar();
    }
    if (this.dadosClienteCadastroDtoIe !== this.dadosClienteCadastroDto.Ie) {
      return true;
    }
    if (this.dadosClienteCadastroDtoProdutorRural !== this.dadosClienteCadastroDto.ProdutorRural) {
      return true;
    }
    if (this.dadosClienteCadastroDtoContribuinte_Icms_Status !== this.dadosClienteCadastroDto.Contribuinte_Icms_Status) {
      return true;
    }
    return false;
  }

  //vamos salvar as alterações
  salvar(continuar: boolean): void {
    //as validações
    if (!this.validar()) {
      return;
    }

    //tudo validado!
    this.buscarClienteService.atualizarCliente(this.dadosClienteCadastroDto).subscribe(
      {
        next: (r) => {
          //retorna uma lista de strings com erros
          if (r.length == 0) {
            this.salvarAtivoInicializar();
            if (continuar) {
              //salvamento automático? então já clicamos no continuar
              this.continuarEfetivo();
              return;
            }

            this.mostrarMensagem(`Dados salvos com sucesso.`);
            return;
          }

          //vamos mostrar os erros
          this.mostrarMensagem(`Ocorreu um erro ao salvar os dados. Mensagens de erro: ` + r.join(", "));
        },
        error: (err) => {
          this.mostrarMensagem(`Ocorreu um erro ao salvar os dados. Verifique a conexão com a Internet.`);
        }
      }
    );
  }

  validar(): boolean {
    let constantes = new Constantes();
    let clienteCadastroUtils = new ClienteCadastroUtils();

    // copiado do ClienteEdita.asp
    if (clienteCadastroUtils.ehPf(this.dadosClienteCadastroDto)) {
      if (!this.dadosClienteCadastroDto.ProdutorRural) {
        this.mostrarMensagem('Informe se o cliente é produtor rural ou não!!');
        return false;
      }
      if ((this.dadosClienteCadastroDto.ProdutorRural !== constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) && (this.dadosClienteCadastroDto.ProdutorRural === constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)) {
        this.mostrarMensagem('Informe se o cliente é produtor rural ou não!!');
        return false;
      }
      if (this.dadosClienteCadastroDto.ProdutorRural !== constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO) {
        if ((this.dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
          && (this.dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
          && (this.dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)) {
          this.mostrarMensagem('Informe se o cliente é contribuinte do ICMS, não contribuinte ou isento!!');
          return false;
        }
        if ((this.dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
          && (this.dadosClienteCadastroDto.Ie || (this.dadosClienteCadastroDto.Ie.trim() == ""))) {
          this.mostrarMensagem('Se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!!');
          //f.ie.focus();
          return false;
        }
        if ((this.dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
          && (this.dadosClienteCadastroDto.Ie && (this.dadosClienteCadastroDto.Ie.toUpperCase().indexOf('ISEN') >= 0))) {
          this.mostrarMensagem('Se cliente é não contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!');
          //f.ie.focus();
          return false;
        }
        if ((this.dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
          && (this.dadosClienteCadastroDto.Ie || this.dadosClienteCadastroDto.Ie.toUpperCase().indexOf('ISEN') >= 0)) {
          this.mostrarMensagem('Se cliente é contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!');
          //f.ie.focus();
          return false;
        }
      }
    }
    else {
      //pessoa jurídica
      if ((this.dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
        && (this.dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
        && (this.dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)) {
        this.mostrarMensagem('Informe se o cliente é contribuinte do ICMS, não contribuinte ou isento!!');
        return false;
      }
      if ((this.dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
        && (this.dadosClienteCadastroDto.Ie || (this.dadosClienteCadastroDto.Ie.trim() == ""))) {
        this.mostrarMensagem('Se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!!');
        //f.ie.focus();
        return false;
      }
      if ((this.dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
        && (this.dadosClienteCadastroDto.Ie && (this.dadosClienteCadastroDto.Ie.toUpperCase().indexOf('ISEN') >= 0))) {
        this.mostrarMensagem('Se cliente é não contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!');
        //f.ie.focus();
        return false;
      }
      if ((this.dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
        && (this.dadosClienteCadastroDto.Ie || this.dadosClienteCadastroDto.Ie.toUpperCase().indexOf('ISEN') >= 0)) {
        this.mostrarMensagem('Se cliente é contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!');
        //f.ie.focus();
        return false;
      }
    }

    // Verifica se o campo IE está vazio quando contribuinte ICMS = isento
    if (clienteCadastroUtils.ehPf(this.dadosClienteCadastroDto)) {
      if (this.dadosClienteCadastroDto.ProdutorRural && this.dadosClienteCadastroDto.ProdutorRural !== constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO) {
        if (this.dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO) {
          if ((this.dadosClienteCadastroDto.Ie && (this.dadosClienteCadastroDto.Ie.trim() !== ""))) {
            this.mostrarMensagem("Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!");
            // fCAD.ie.focus();
            return false;
          }
        }
      }
    }
    else {
      //pessoa jurídica
      if (this.dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO) {
        if ((this.dadosClienteCadastroDto.Ie && (this.dadosClienteCadastroDto.Ie.trim() !== ""))) {
          this.mostrarMensagem("Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!");
          // fCAD.ie.focus();
          return false;
        }
      }
    }

    //tudo certo!
    return true;
  }

  mostrarMensagem(msg: string): void {
    const dialogRef = this.dialog.open(AlertDialogComponent, {
      width: '350px',
      data: msg
    });
  }

  continuar(): void {
    //salvamos automaticamente
    if (this.salvarAtivo()) {
      this.salvar(true);
      return;
    }
    this.continuarEfetivo();
  }
  continuarEfetivo(): void {
    //continuamos
    window.alert("afazer: continuar");
  }

}

