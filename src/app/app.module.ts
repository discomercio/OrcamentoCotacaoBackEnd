import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UtilsModule } from './utils/utils.module';
import { HomeModule } from './home/home.module';
import { MatButtonModule, MatIconModule } from '@angular/material';
import { FlexLayoutModule } from "@angular/flex-layout";
import { PrepedidoModule } from './prepedido/prepedido.module';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FlexLayoutModule.withConfig({
      useColumnBasisZero: false,
      printWithBreakpoints: ['md', 'lt-lg', 'lt-xl', 'gt-sm', 'gt-xs']
    }),
    AppRoutingModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatIconModule,
    HttpClientModule,
    HomeModule,
    PrepedidoModule,
    UtilsModule
  ],
  providers: [  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
