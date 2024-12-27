import { NgModule } from '@angular/core';

import { CommuneRoutingModule } from './commune-routing.module';
import { CommuneComponent } from './commune.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputDirective } from 'ng-zorro-antd/input';
import { NzModalContentDirective, NzModalModule } from 'ng-zorro-antd/modal';
import { NzTableComponent, NzTableModule, NzThAddOnComponent, NzThMeasureDirective } from 'ng-zorro-antd/table';
import { SharedModule } from '../shared/shared.module';
import { NzPaginationComponent } from 'ng-zorro-antd/pagination';
import { NzSelectModule } from 'ng-zorro-antd/select';


@NgModule({
  declarations: [
    CommuneComponent
  ],
  imports: [
    SharedModule,
    CommuneRoutingModule,
    NzTableComponent,
    NzTableModule,
    NzThMeasureDirective,
    NzThAddOnComponent,
    NzButtonModule,
    NzModalModule,
    NzModalContentDirective,
    NzFormModule,
    NzInputDirective,
    NzPaginationComponent,
    NzSelectModule
  ]
})
export class CommuneModule { }
