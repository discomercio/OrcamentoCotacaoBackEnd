import { Injectable } from '@angular/core';
import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TelaDesktopService {

  //emitimos eventos quando a tela mudar
  //e guaredamos o status atual da tela
  telaAtual$: BehaviorSubject<boolean> = new BehaviorSubject(true);
  private telaDesktop: boolean = true;
  private jaLido: boolean = false;
  private telaDesktopAnterior: boolean = true;
  constructor(private breakpointObserver: BreakpointObserver) {
    this.breakpointObserver
      .observe([Breakpoints.Small, Breakpoints.HandsetPortrait])
      .subscribe((state: BreakpointState) => {
        this.telaDesktop = !state.matches;

        //está um pouco instável quando chaveia
        //as vezes ele não consegue carregar o ponto de navegaçãoc corretamente
        //então recarregamos a página toda. unico inconveniente: perdemos o que tiver sido digitado nos campos de busca.
        if (this.jaLido && this.telaDesktopAnterior!= this.telaDesktop)
          window.location.reload();
        this.jaLido = true;
        this.telaDesktopAnterior=this.telaDesktop;
        this.telaAtual$.next(this.telaDesktop);
      });

  }

}
