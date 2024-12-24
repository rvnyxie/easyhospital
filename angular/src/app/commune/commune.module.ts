import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CommuneRoutingModule } from './commune-routing.module';
import { CommuneComponent } from './commune.component';


@NgModule({
  declarations: [
    CommuneComponent
  ],
  imports: [
    CommonModule,
    CommuneRoutingModule
  ]
})
export class CommuneModule { }
