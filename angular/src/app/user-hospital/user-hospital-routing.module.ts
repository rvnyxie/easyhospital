import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserHospitalComponent } from './user-hospital.component';

const routes: Routes = [{ path: '', component: UserHospitalComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserHospitalRoutingModule { }
