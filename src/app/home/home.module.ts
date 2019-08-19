import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home/home.component';
import { MatInputModule, MatIconModule, MatSnackBarModule, MatSidenavModule, MatDividerModule, MatBadgeModule, MatMenu, MatMenuModule, MatListModule } from '@angular/material';
import {MatButtonModule} from '@angular/material/button';

@NgModule({
  declarations: [ HomeComponent],
  imports: [
    CommonModule,
    MatIconModule,
    HomeRoutingModule,
    MatBadgeModule,
    MatButtonModule,
    MatMenuModule,
    MatListModule,
    MatDividerModule,
    MatSidenavModule
  ]
})
export class HomeModule { }
