import { NgModule } from '@angular/core';

import { UserHospitalRoutingModule } from './user-hospital-routing.module';
import { UserHospitalComponent } from './user-hospital.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    UserHospitalComponent
  ],
  imports: [
    SharedModule,
    UserHospitalRoutingModule
  ]
})
export class UserHospitalModule { }
