import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.AccountModule.forLazy()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.IdentityModule.forLazy()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.TenantManagementModule.forLazy()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.SettingManagementModule.forLazy()),
  },
  {
    path: 'provinces',
    loadChildren: () =>
      import('./province/province.module').then(m => m.ProvinceModule)
  },
  {
    path: 'districts',
    loadChildren: () =>
      import('./district/district.module').then(m => m.DistrictModule)
  },
  {
    path: 'communes',
    loadChildren: () =>
      import('./commune/commune.module').then(m => m.CommuneModule)
  },
  {
    path: 'hospitals',
    loadChildren: () =>
      import('./hospital/hospital.module').then(m => m.HospitalModule)
  },
  {
    path: 'patients',
    loadChildren: () =>
      import('./patient/patient.module').then(m => m.PatientModule)
  },
  {
    path: 'user-hospitals',
    loadChildren: () =>
      import('./user-hospital/user-hospital.module').then(m => m.UserHospitalModule)
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
