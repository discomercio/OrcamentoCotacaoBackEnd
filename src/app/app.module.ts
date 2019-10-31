import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UtilsModule } from './utils/utils.module';
import { HomeModule } from './home/home.module';
import { MatButtonModule, MatIconModule, MatToolbarModule, MatSidenavModule, MatTableModule, MatDialogModule } from '@angular/material';
import { FlexLayoutModule } from "@angular/flex-layout";
import { PrepedidoModule } from './prepedido/prepedido.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PedidoModule } from './pedido/pedido.module';
import { LoginModule } from './login/login.module';
import { TokenInterceptor } from './servicos/autenticacao/token.interceptor';
import { ClienteModule } from './cliente/cliente.module';
import { TextMaskModule } from 'angular2-text-mask';
import { PlatformLocation } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FlexLayoutModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatSidenavModule,
    MatTableModule,
    HttpClientModule,
    HomeModule,
    PrepedidoModule,
    PedidoModule,
    UtilsModule,
    BrowserModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatButtonModule,
    LoginModule,
    ClienteModule,
    TextMaskModule
    ],
  providers: [ 
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
   ],
  bootstrap: [AppComponent]
})
export class AppModule { }
