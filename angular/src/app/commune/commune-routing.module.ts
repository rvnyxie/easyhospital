import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommuneComponent } from './commune.component';

const routes: Routes = [{ path: '', component: CommuneComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommuneRoutingModule { }
