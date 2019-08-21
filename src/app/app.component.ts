import { Component, OnInit } from '@angular/core';
import { TelaDesktopService } from './servicos/telaDesktop/telaDesktop.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  telaDesktop: boolean = true;

  constructor(private telaDesktopService: TelaDesktopService) {
    telaDesktopService.telaAtual$.subscribe(r => this.telaDesktop = r);
  }

  ngOnInit(): void {
  }
  title = 'Sistema de pré-pedidos';
}
