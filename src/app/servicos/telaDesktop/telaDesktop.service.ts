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
  private telaDesktop: boolean;
  constructor(private breakpointObserver: BreakpointObserver) {
    this.breakpointObserver
      .observe([Breakpoints.Small, Breakpoints.HandsetPortrait])
      .subscribe((state: BreakpointState) => {
        this.telaDesktop = !state.matches;
        this.telaAtual$.next(this.telaDesktop);
      });
      
  }

}
