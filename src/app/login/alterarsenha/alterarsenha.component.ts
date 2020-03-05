import { Component, OnInit } from '@angular/core';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { AutenticacaoService } from 'src/app/servicos/autenticacao/autenticacao.service';
import { LoginformularioComponent } from '../loginformulario/loginformulario.component';
import { Router } from '@angular/router';
import { MatSnackBar, matDialogAnimations } from '@angular/material';
import { AppComponent } from 'src/app/app.component';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-alterarsenha',
  templateUrl: './alterarsenha.component.html',
  styleUrls: ['./alterarsenha.component.scss']
})
export class AlterarsenhaComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(public readonly alertaService: AlertaService,
    private readonly autenticacaoService: AutenticacaoService,
    private readonly router: Router,
    private readonly _snackBar: MatSnackBar,
    private readonly appComponent: AppComponent,
    telaDesktopService: TelaDesktopService) {
    super(telaDesktopService);
  }

  ngOnInit() {
    if (this.autenticacaoService.senhaExpirada)
      this.senhaExpirada = true;
  }

  senha = "";
  senhaNova = "";
  senhaNovaConfirma = "";
  senhaExpirada: boolean = false;

  alterarSenha(): void {

    let msg: string = "";

    if (!!this.senha) {
      if (!!this.senhaNova && !!this.senhaNovaConfirma) {

        if (this.senhaNova.length < 5) {
          msg = "A nova senha deve possuir no mínimo 5 caracteres.";
          this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
            duration: environment.esperaErros
          });
          return;
          // this.alertaService.mostrarMensagem("A nova senha deve possuir no mínimo 5 caracteres.");
        }
        if (this.senhaNovaConfirma.length < 5) {
          msg = "A confirmação da nova senha deve possuir no mínimo 5 caracteres.";
          this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
            duration: environment.esperaErros
          });
          return;
          // this.alertaService.mostrarMensagem("A confirmação da nova senha deve possuir no mínimo 5 caracteres.");
        }
        if (this.senhaNova != this.senhaNovaConfirma) {
          msg = "A confirmação da nova senha está incorreta.";
          this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
            duration: environment.esperaErros
          });
          return;
          // this.alertaService.mostrarMensagem("A confirmação da nova senha está incorreta.");
        }
        if (this.senha == this.senhaNova) {
          msg = "A nova senha deve ser diferente da senha atual.";
          this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
            duration: environment.esperaErros
          });
          return;
          // this.alertaService.mostrarMensagem("A nova senha deve ser diferente da senha atual.");
        }
        if (this.senhaNova == this.senhaNovaConfirma) {
          //preciso guardar o nome de usuario
          this.autenticacaoService.alterarSenha(this.autenticacaoService.usuarioApelidoParaAlterarSenha, this.senha,
            this.senhaNova, this.senhaNovaConfirma).subscribe({
              next: (e) => {
                //fazer a chamada para realizar o login, passando a senha nova e o apelido
                if (e == "" || e == null) {
                  this._snackBar.open("Alteração de senha realizada com sucesso!", undefined, {
                    duration: environment.esperaErros
                  });
                  this.autenticacaoService.senhaExpirada = false;
                  // this.alertaService.mostrarMensagem("Alteração de senha realizada com sucesso!");
                  this.autenticacaoService.authLogin(this.autenticacaoService.usuarioApelidoParaAlterarSenha, this.senhaNova,
                    this.autenticacaoService.lembrarSenhaParaAlterarSenha, () => { }, this._snackBar, this.router, this.appComponent)
                }
                //retornando erro
                if (e != "" && e != null) {
                  msg = e.toString();

                  this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
                    duration: environment.esperaErros
                  });
                  return;
                }

              },
              error: (e) => {
                msg = "" + ((e && e.message) ? e.message : e.toString());
                if (e && e.status === 400)
                  msg = "usuário e senha inválidos."
                if (e && e.status === 0)
                  msg = "servidor de autenticação não disponível."
                if (e && e.status === 500)
                  msg = "erro interno no servidor de autenticação."


                this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
                  duration: environment.esperaErros
                });
              }
            });
        }
      }
    }
  }
}
