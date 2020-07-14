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
  constructor(private readonly router: Router,
    public readonly activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    const msg = this.activatedRoute.snapshot.params.mensagem;
    if (!!msg) {
      this.mensagem = msg;
    }
  }

  voltarInicio() {
    this.router.navigate(['/']);
  }

}
