import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home/home.component';
import { ConsultaPrepedidoComponent } from './prepedido/consulta-prepedido/consulta-prepedido.component';
import { ErroComponent } from './utils/erro/erro.component';
import { ListaPrepedidoComponent } from './prepedido/lista-prepedido/lista-prepedido.component';
import { ConsultaPedidoComponent } from './pedido/consulta-pedido/consulta-pedido.component';
import { ListaPedidoComponent } from './pedido/lista-pedido/lista-pedido.component';
import { LoginformularioComponent } from './login/loginformulario/loginformulario.component';
import { LoginGuard } from './servicos/autenticacao/login.guard';
import { DetalhesPrepedidoComponent } from './prepedido/detalhes-prepedido/detalhes-prepedido.component';


const routes: Routes = [
  /*
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  }
  */
  {
    path: 'login',
    component: LoginformularioComponent
  },
  {
    path: 'prepedido/consulta',
    canActivate: [LoginGuard],
    component: ConsultaPrepedidoComponent
  },
  {
    path: 'prepedido/lista',
    canActivate: [LoginGuard],
    component: ListaPrepedidoComponent
  },
  {
    path: 'prepedido/detalhes/:numeroPrepedido',
    canActivate: [LoginGuard],
    component: DetalhesPrepedidoComponent
  },
  {
    path: 'pedido/consulta',
    canActivate: [LoginGuard],
    component: ConsultaPedidoComponent
  },
  {
    path: 'pedido/lista',
    canActivate: [LoginGuard],
    component: ListaPedidoComponent
  },
  {
    path: 'pedido/detalhes/:numeroPedido',
    canActivate: [LoginGuard],
    //fazemos no próprio DetalhesPrepedidoComponent, ele identifica pelo parâmetro
    component: DetalhesPrepedidoComponent
  },
  {
    path: 'home',
    canActivate: [LoginGuard],
    component: HomeComponent
  },
  {
    path: 'erro/:mensagem',
    component: ErroComponent
  },
  {
    path: 'erro',
    component: ErroComponent
  },
  {
    path: '**',
    canActivate: [LoginGuard],
    component: HomeComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
