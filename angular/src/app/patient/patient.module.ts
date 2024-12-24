import { NgModule } from '@angular/core';

import { PatientRoutingModule } from './patient-routing.module';
import { PatientComponent } from './patient.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    PatientComponent
  ],
  imports: [
    SharedModule,
    PatientRoutingModule
  ]
})
export class PatientModule { }
