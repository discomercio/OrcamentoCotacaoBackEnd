import { Component, OnInit } from '@angular/core';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { supportsPassiveEventListeners } from '@angular/cdk/platform';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { MatDialog } from '@angular/material';
import { AlertDialogComponent } from 'src/app/utils/alert-dialog/alert-dialog.component';
import { StringUtils } from 'src/app/utils/stringUtils';
import { BuscarClienteService } from 'src/app/servicos/cliente/buscar-cliente.service';
import { ConfirmationDialogComponent } from 'src/app/utils/confirmation-dialog/confirmation-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-selecionar-cliente',
  templateUrl: './selecionar-cliente.component.html',
  styleUrls: ['./selecionar-cliente.component.scss']
})
export class SelecionarClienteComponent extends TelaDesktopBaseComponent implements OnInit {

  //se estamos buscando
  carregando = false;

  //o formulário, só 1 campo
  clienteBusca = "";

  constructor(telaDesktopService: TelaDesktopService,
    public readonly dialog: MatDialog,
    public readonly router: Router,
    public readonly activatedRoute: ActivatedRoute,
    private readonly buscarClienteService: BuscarClienteService) {
    super(telaDesktopService);
  }

  ngOnInit() {
  }


  //deu erro na busca
  //ou não achou nada...
  mostrarErro() {
    this.carregando = false;
    const dialogRef = this.dialog.open(AlertDialogComponent, {
      width: '250px',
      data: `Erro no acesso ao sistema. Verifique a conexão com a internet.`
    });
  }
  buscar() {
    //dá erro se não tiver nenhum dígito
    if (StringUtils.retorna_so_digitos(this.clienteBusca).trim() === "") {
      const dialogRef = this.dialog.open(AlertDialogComponent, {
        width: '250px',
        data: `CNPJ/CPF inválido ou vazio.`
      });
      return;
    }

    //valida
    if (!CpfCnpjUtils.cnpj_cpf_ok(this.clienteBusca)) {
      const dialogRef = this.dialog.open(AlertDialogComponent, {
        width: '250px',
        data: `CNPJ/CPF inválido.`
      });
      return;
    }

    //vamos fazer a busca
    this.carregando = true;
    this.buscarClienteService.buscar(this.clienteBusca).toPromise()
      .then((r) => {
        this.carregando = false;
        if (r === null) {
          this.mostrarNaoCadastrado();
          return;
        }
        //cliente já existe
        this.router.navigate(['confirmar-cliente', StringUtils.retorna_so_digitos(r.DadosCliente.Cnpj_Cpf)], { relativeTo: this.activatedRoute, state: r })
      }).catch((r) => {
        this.mostrarErro();
      });
  }

  //cliente ainda não está cadastrado
  mostrarNaoCadastrado() {
    this.carregando = false;
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '380px',
      data: `Este CNPJ/CPF ainda não está cadastrado. Deseja cadastrá-lo agora?`
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        //vamos cadastrar um novo
        this.router.navigate(['cadastrar-cliente', this.clienteBusca], { relativeTo: this.activatedRoute })
      }
    });
  }

}