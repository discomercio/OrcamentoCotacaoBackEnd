import { TelaDesktopService } from './telaDesktop.service';

/*
classe que implementa a variável telaDesktop
usamos em diversos compoentes, mais fácil colocar em uma classe base
*/

export class TelaDesktopBaseComponent {
    public telaDesktop: boolean = true;
    constructor(telaDesktopService: TelaDesktopService) {        
        telaDesktopService.telaAtual$.subscribe(r => this.telaDesktop = r);
    }

}