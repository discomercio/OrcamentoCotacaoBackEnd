import { Component, OnInit, Input } from '@angular/core';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends TelaDesktopBaseComponent implements OnInit {

  @Input() menuDesktop = false;

  constructor(telaDesktopService: TelaDesktopService,
    public readonly router:Router) {
    super(telaDesktopService);
  }

  ngOnInit() {
  }

  cliqueNovoPrepedido(){
    this.router.navigateByUrl('/novo-prepedido');
  }
}
