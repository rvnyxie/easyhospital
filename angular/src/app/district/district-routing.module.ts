import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DistrictComponent } from './district.component';

const routes: Routes = [{ path: '', component: DistrictComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DistrictRoutingModule { }
