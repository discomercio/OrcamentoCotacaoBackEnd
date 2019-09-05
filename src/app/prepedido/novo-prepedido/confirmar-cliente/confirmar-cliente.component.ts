import { Component, OnInit, ElementRef, ViewChild, AfterContentInit, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { BuscarClienteService } from 'src/app/servicos/cliente/buscar-cliente.service';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';
import { ClienteCadastroUtils } from 'src/app/dto/ClienteCadastroUtils/ClienteCadastroUtils';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';

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
    private readonly alertaService: AlertaService,
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
          this.clienteCadastroDto = r;
          this.salvarAtivoInicializar();
        }).catch((r) => {
          //erro, voltamos para a tela anterior
          router.navigate(["/novo-prepedido"]);
        });
    }
  }

  dadosClienteCadastroDto = new DadosClienteCadastroDto();
  clienteCadastroDto = new ClienteCadastroDto();
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
    if (this.dadosClienteCadastroDtoIe === null) {
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
    let mensagem = new ClienteCadastroUtils().validarInscricaoestadualIcms(this.dadosClienteCadastroDto);
    if (mensagem && mensagem.trim() !== "") {
      this.mostrarMensagem(mensagem);
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
          this.alertaService.mostrarErroInternet();
        }
      }
    );
  }

  mostrarMensagem(msg: string): void {
    this.alertaService.mostrarMensagem(msg);
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
    window.alert("afazer: continuar para confirmação do endereço de entrega");
  }

}

