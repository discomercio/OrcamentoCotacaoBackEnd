import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationStart } from '@angular/router';
import { map, filter } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'arclube-erro',
  templateUrl: './erro.component.html',
  styleUrls: ['./erro.component.scss']
})
export class ErroComponent implements OnInit {

  state$: Observable<object>;
  mensagem: string = "Erro interno.";
  constructor(private router: Router, public activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    //this.mensagem = this.router.getCurrentNavigation().extras.state.mensagem;
    // this.state$ = this.activatedRoute.paramMap
    //   .pipe(map(() => window.history.state));
    // this.state$.subscribe(c => {
    //   let cany: any = c;
    //   if (cany && cany.mensagem)
    //     this.mensagem = cany.mensagem;
    // });
  }

  voltarInicio() {
    this.router.navigate(['/']);
  }

}
