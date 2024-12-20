import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'EasyHospital',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44367/',
    redirectUri: baseUrl,
    clientId: 'EasyHospital_App',
    responseType: 'code',
    scope: 'offline_access EasyHospital',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44367',
      rootNamespace: 'Refined.EasyHospital',
    },
  },
} as Environment;
