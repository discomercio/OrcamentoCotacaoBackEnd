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
import { HttpClientModule } from '@angular/common/http';
import { CdkTableModule } from '@angular/cdk/table';
import { PedidoModule } from './pedido/pedido.module';
import { LoginModule } from './login/login.module';

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
    LoginModule
    ],
  providers: [  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
