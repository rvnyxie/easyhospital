import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      // Locality
      {
        path: '/localities',
        name: '::Menu:Locality',
        iconClass: 'fas fa-map-marker-alt',
        order: 2,
        layout: eLayoutType.application
      },
      {
        path: '/provinces',
        name: '::Menu:Provinces',
        parentName: '::Menu:Locality',
        layout: eLayoutType.application
      },
      {
        path: '/districts',
        name: '::Menu:Districts',
        parentName: '::Menu:Locality',
        layout: eLayoutType.application
      },
      {
        path: '/communes',
        name: '::Menu:Communes',
        parentName: '::Menu:Locality',
        layout: eLayoutType.application
      },
      // Hospital
      {
        path: '/hospitals',
        name: '::Menu:Hospitals',
        iconClass: 'fas fa-hospital',
        order: 3,
        layout: eLayoutType.application
      },
      // Patient
      {
        path: '/patients',
        name: '::Menu:Patients',
        iconClass: 'fas fa-bed',
        order: 4,
        layout: eLayoutType.application
      },
      // User Hospital
      {
        path: '/user-hospitals',
        name: '::Menu:UserHospitals',
        iconClass: 'fas fa-hospital-user',
        order: 5,
        layout: eLayoutType.application
      }
    ]);
  };
}
