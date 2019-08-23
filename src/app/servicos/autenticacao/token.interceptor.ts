import { HttpInterceptor, HttpRequest, HttpHandler, HttpSentEvent, HttpHeaderResponse, HttpProgressEvent, HttpResponse, HttpUserEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.prod';
import { AutenticacaoService } from './autenticacao.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private autenticacaoService: AutenticacaoService) {

  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpSentEvent
    | HttpHeaderResponse | HttpProgressEvent | HttpResponse<any> | HttpUserEvent<any>> {

    this.autenticacaoService.renovarTokenSeNecessario();

    //adicioan o header de autenticação
    if (this.autenticacaoService.authEstaLogado()) {
      req = req.clone({
        setHeaders: {
          'Authorization': 'Bearer ' + this.autenticacaoService.obterToken()
        }
      });
    }
    return next.handle(req);
  }
}


