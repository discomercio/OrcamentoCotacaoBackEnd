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
    let senha_nova = this.senhaNova.toUpperCase().trim();
    let senha_nova_confirma = this.senhaNovaConfirma.toUpperCase().trim();
    let senha_atual = this.senha.toUpperCase().trim();

    if (senha_atual.length < 5) {
      msg = "A senha atual deve possuir no mínimo 5 caracteres.";
      this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
        duration: environment.esperaErros
      });
      return;
    }

    if (senha_nova.length < 5) {
      msg = "A nova senha deve possuir no mínimo 5 caracteres.";
      this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
        duration: environment.esperaErros
      });
      return;
    }
    if (senha_nova_confirma.length < 5) {
      msg = "A confirmação da nova senha deve possuir no mínimo 5 caracteres.";
      this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
        duration: environment.esperaErros
      });
      return;
    }
    if (senha_nova != senha_nova_confirma) {
      msg = "A confirmação da nova senha está incorreta.";
      this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
        duration: environment.esperaErros
      });
      return;
    }
    if (senha_atual == senha_nova) {
      msg = "A nova senha deve ser diferente da senha atual.";
      this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
        duration: environment.esperaErros
      });
      return;
    }
    if (senha_nova == this.autenticacaoService._NomeUsuario.toUpperCase().trim()) {
      msg = "A nova senha não pode ser igual ao identificador do usuário!";
      this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
        duration: environment.esperaErros
      });
      return;
    }

    //vamos codificar as senhas com elas em maiusculas
    var key = this.autenticacaoService.gerarChave();
    let senha_cod = this.autenticacaoService.CodificaSenha(senha_atual, key);
    let senha_nova_cod = this.autenticacaoService.CodificaSenha(senha_nova, key);
    let senha_nova_confirma_cod = this.autenticacaoService.CodificaSenha(senha_nova_confirma, key);

    //as senhas serão passadas codificadas em maiusculas
    //preciso guardar o nome de usuario          
    this.autenticacaoService.alterarSenha(this.autenticacaoService._NomeUsuario, senha_cod,
      senha_nova_cod, senha_nova_confirma_cod).subscribe({
        next: (e) => {
          //fazer a chamada para realizar o login, passando a senha nova e o apelido
          if (e == "" || e == null) {
            this._snackBar.open("Alteração de senha realizada com sucesso!", undefined, {
              duration: environment.esperaErros
            });
            this.autenticacaoService.senhaExpirada = false;

            // this.alertaService.mostrarMensagem("Alteração de senha realizada com sucesso!");
            this.autenticacaoService.authLogin(this.autenticacaoService._NomeUsuario, senha_nova,
              this.autenticacaoService.lembrarSenhaParaAlterarSenha, () => { }, this._snackBar, this.router, this.appComponent);
            return;

          }
          //retornando erro
          msg = e.toString();

          this._snackBar.open("Erro ao alterar senha: " + msg, undefined, {
            duration: environment.esperaErros
          });

          //Qualquer problema, podemos verificar se a msg é "Usuário bloqueado" e 
          //podemos redirecionar ele para a tela de login, pois ele ainda não esta logado
          //No momento estamos deixando ele na tela de alterar senha
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

