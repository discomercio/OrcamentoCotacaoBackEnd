import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { BuscarClienteService } from 'src/app/servicos/cliente/buscar-cliente.service';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { Location } from '@angular/common';
import { AlertDialogComponent } from 'src/app/utils/alert-dialog/alert-dialog.component';
import { MatDialog } from '@angular/material';
import { Constantes } from 'src/app/dto/Constantes';

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

    this.dadosClienteCadastroDto = (router.getCurrentNavigation().extras.state) as DadosClienteCadastroDto;
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
          this.dadosClienteCadastroDto = r;
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
    //afazer contribuinte_icms_status
  }
  private dadosClienteCadastroDtoIe: string;
  private dadosClienteCadastroDtoProdutorRural: number;
  //afazer: contribuinte_icms_status
  salvarAtivo(): boolean {
    //diz se o botão de salvar está ligado
    if(!this.dadosClienteCadastroDto){
      return false;
    }
    if (this.dadosClienteCadastroDtoIe !== this.dadosClienteCadastroDto.Ie){
      return true;
    }
    if (this.dadosClienteCadastroDtoProdutorRural !== this.dadosClienteCadastroDto.ProdutorRural){
      return true;
    }
    //afazer: contribuinte_icms_status
    return false;
  }

  //vamos salvar as alterações
  salvar(continuar: boolean): void {
    let constantes = new Constantes();
    //algumas validações
    //afazer: terminar as validações
    console.log(this.dadosClienteCadastroDto.ProdutorRural);
    if (!this.dadosClienteCadastroDto.ProdutorRural || this.dadosClienteCadastroDto.ProdutorRural === constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL) {
      this.mostrarMensagem('Erro: informe se o cliente é produtor rural ou não!!');
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

